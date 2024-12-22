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
        private readonly RabbitMQConfig _rabbitConfig;

        public Send(RabbitMQConfig rabbitConfig)
        {
            _rabbitConfig = rabbitConfig;
            Console.WriteLine($"Send initialized with RabbitMQ config - Host: {rabbitConfig.HostName}, User: {rabbitConfig.UserName}");
        }

        public void DoSend(IEnumerable<RecommendationMessage> messages)
        {
            Console.WriteLine("Starting to send messages to RabbitMQ...");
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitConfig.HostName,
                    UserName = _rabbitConfig.UserName,
                    Password = _rabbitConfig.Password
                };
                Console.WriteLine($"Created connection factory for {_rabbitConfig.HostName}");

                using (var connection = factory.CreateConnection())
                {
                    Console.WriteLine("Connected to RabbitMQ");
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "books",
                                          durable: false,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null);

                        Console.WriteLine("Queue declared");

                        foreach (var message in messages)
                        {
                            var json = JsonConvert.SerializeObject(message);
                            var body = Encoding.UTF8.GetBytes(json);

                            channel.BasicPublish(exchange: "",
                                             routingKey: "books",
                                             basicProperties: null,
                                             body: body);

                            Console.WriteLine($"Sent recommendation '{message.Recommendation.Title}' to {message.RecipientEmail}");
                        }

                        // Check final queue count
                        var queueCheck = channel.QueueDeclare(queue: "books",
                                          durable: false,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null);
                        Console.WriteLine($"Final queue message count: {queueCheck.MessageCount}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending messages to RabbitMQ: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner error: {ex.InnerException.Message}");
                }
                throw; // Re-throw to handle in controller
            }
        }
    }
}