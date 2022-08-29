using NetQuartzSample.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SaleManagement;
using System.Text;

Trigger.TriggerTask();

ConnectionFactory rabbitFactory = new ConnectionFactory();
rabbitFactory.HostName = "localhost";
Console.WriteLine("Kuyruğa bağlanıldı...\nSatış başlatıldı...");
using (IConnection connection = rabbitFactory.CreateConnection())
{
    using (IModel channel = connection.CreateModel())
    {
        while (true)
        {
            var product = Console.ReadLine().Split("&");
            Sale sale = new Sale()
            {
                ProductName = product[0],
                SaleTime = DateTime.Now,
                Price = Convert.ToDouble(product[1]),
                Quantity = Convert.ToInt32(product[2])
            };
            var jsonMessage = JsonConvert.SerializeObject(sale);
            byte[] byteMessage = Encoding.UTF8.GetBytes(jsonMessage);
            channel.QueueDeclare("sales", true, false, false);
            channel.BasicPublish(exchange: "", routingKey: "sales", body: byteMessage);
            Console.WriteLine("Ürün Gönderildi...");
        }

    }
}