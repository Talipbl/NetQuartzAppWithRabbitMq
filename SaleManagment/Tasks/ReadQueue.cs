
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

namespace SaleManagement.Tasks
{
    public class ReadQueue : IJob
    {

        public Task Execute(IJobExecutionContext context)
        {
            ConnectionFactory rabbitFactory = new ConnectionFactory();
            rabbitFactory.HostName = "localhost";
            Console.WriteLine("Rapor için bağlanıldı...");
            using (IConnection connection = rabbitFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare("allSales", true, false, false);
                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume("allSales", true, consumer);
                    consumer.Received += (sender, args) =>
                    {
                        var jsonBody = Encoding.UTF8.GetString(args.Body.Span);
                        SaleTransferDto body = JsonConvert.DeserializeObject<SaleTransferDto>(jsonBody);
                        foreach (var item in body.Sales)
                        {
                            Console.WriteLine("»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»»");
                            Console.WriteLine("Ürün Adı: " + item.ProductName + "\n--------------------------"
                           + "\nSatış Zamanı: " + item.SaleTime + "\n--------------------------"
                           + "\nAdedi: " + item.Quantity + "\n--------------------------"
                           + "\nToplam Fiyatı: " + (item.Quantity * item.Price));
                        }
                        Console.WriteLine("========================\nGünlük Satış: " + body.TotalAmount + "\n========================");
                    };
                }
            }
            return Task.CompletedTask;
        }
    }
}
