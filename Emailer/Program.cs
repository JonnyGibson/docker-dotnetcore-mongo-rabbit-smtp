using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Emailer.Models;
using Newtonsoft.Json;

namespace Emailer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "store",
                Password = "password"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "books",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    //send email
                 //   var book = JsonConvert.DeserializeObject<Recomendation>(message);
                    Console.WriteLine(" [x] Received {0}", message);
                };

                channel.BasicConsume(queue: "books",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
