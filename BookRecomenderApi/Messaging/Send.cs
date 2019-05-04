using System;
using System.Text;
using RabbitMQ.Client;
using BookRecomenderApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BookRecomenderApi.Messaging
{
    public class Send
    {
        public static void DoSend(IEnumerable<Recomendation> recomendations)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "JGMQ",
                UserName = "store",
                Password = "password"
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "books",
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                    foreach (var item in recomendations)
                    {
                        string message = JsonConvert.SerializeObject(item);
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "books",
                                             basicProperties: null,
                                             body: body);
                    }
                }
            }
        }
    }
}