using Newtonsoft.Json;
using Quartz;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetQuartzSample.Tasks
{
    public class WriteQueue : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                ConnectionFactory rabbitFactory = new ConnectionFactory();
                rabbitFactory.HostName = "localhost";
                Console.WriteLine("Rapor için bağlanıldı...");
                using (IConnection connection = rabbitFactory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        channel.QueueDeclare("allSales", true, false, false);
                        var jsonMessage = JsonConvert.SerializeObject(SaleManager.GetSales);
                        byte[] byteMessage = Encoding.UTF8.GetBytes(jsonMessage);
                        channel.BasicPublish(exchange: "", routingKey: "allSales", body: byteMessage);
                        Console.WriteLine("Rapor Gönderildi...");
                    }
                }
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
