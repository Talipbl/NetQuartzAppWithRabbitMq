using NetQuartzSample.Models;
using Newtonsoft.Json;
using Quartz;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetQuartzSample.Tasks
{
    public class ReadQueue : IJob
    {

        public Task Execute(IJobExecutionContext context)
        {
            ConnectionFactory rabbitFactory = new ConnectionFactory();
            rabbitFactory.HostName = "localhost";
            Console.WriteLine("Kuyruğa bağlanıldı...");
            using (IConnection connection = rabbitFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare("sales", true, false, false);
                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume("sales", true, consumer);
                    consumer.Received += (sender, args) =>
                    {
                        var body = args.Body.Span;
                        var jsonBody = Encoding.UTF8.GetString(body);
                        Sale sale = JsonConvert.DeserializeObject<Sale>(jsonBody);
                        Console.WriteLine("»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»");
                        Console.WriteLine("Ürün Adı: " + sale.ProductName + "\n--------------------------"
                            + "\nSatış Zamanı: " + sale.SaleTime + "\n--------------------------"
                            + "\nAdedi: " + sale.Quantity + "\n--------------------------"
                            + "\nToplam Fiyatı: " + (sale.Quantity * sale.Price));
                        SaleManager.AddSale(sale);
                    };
                }
            }
            return Task.CompletedTask;
        }
    }
}
