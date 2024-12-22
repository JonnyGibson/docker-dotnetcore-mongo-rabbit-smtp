using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using Emailer.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq;
using System.Threading;

namespace Emailer
{
    class Program
    {
        private static ManualResetEvent _resetEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            var runnerName = (args.Any())
                ? args[0]
                : "Unnamed Runner";

            Console.WriteLine($"Starting email listener with name: {runnerName}");

            var repo = new MailerRepository();
            string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string templatesPath = Path.Combine(filepath, "Templates");
            
            Console.WriteLine($"Templates path: {templatesPath}");

            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "JGMQ",
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "store",
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "password"
            };

            Console.WriteLine($"Connecting to RabbitMQ at {factory.HostName} with user {factory.UserName}");

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    Console.WriteLine("Successfully connected to RabbitMQ");
                    using (var channel = connection.CreateModel())
                    {
                        var queueDeclare = channel.QueueDeclare(queue: "books",
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        Console.WriteLine($"Queue 'books' declared, current message count: {queueDeclare.MessageCount}");

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            try
                            {
                                var messageBody = Encoding.UTF8.GetString(ea.Body.Span);
                                Console.WriteLine($"Received raw message: {messageBody}");
                                
                                var message = JsonConvert.DeserializeObject<RecommendationMessage>(messageBody);
                                Console.WriteLine($" [x] Processing recommendation '{message.Recommendation.Title}' for {message.RecipientEmail}");
                                
                                repo.SendMail(message, templatesPath, runnerName);
                                Console.WriteLine($" [✓] Sent email for '{message.Recommendation.Title}' to {message.RecipientEmail}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($" [!] Error processing message: {ex.Message}");
                                if (ex.InnerException != null)
                                {
                                    Console.WriteLine($" [!] Inner error: {ex.InnerException.Message}");
                                }
                            }
                        };

                        channel.BasicConsume(queue: "books",
                                             autoAck: true,
                                             consumer: consumer);

                        Console.WriteLine(" [*] Listening for book recommendations. Use CTRL+C to exit.");

                        // Handle shutdown gracefully
                        Console.CancelKeyPress += (sender, eventArgs) =>
                        {
                            Console.WriteLine("Shutting down...");
                            _resetEvent.Set();
                            eventArgs.Cancel = true;
                        };

                        // Wait indefinitely
                        _resetEvent.WaitOne();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner error: {ex.InnerException.Message}");
                }
                throw;
            }
        }
    }
}
