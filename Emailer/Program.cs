using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using Emailer.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq;

namespace Emailer
{
    class Program
    {
        static void Main(string[] args)
        {
            var runnerName = (args.Any())
                ? args[0]
                : "Unnamed Runner";

            var repo = new MailerRepository();
            string filepath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string templatesPath = Path.Combine(filepath, "Templates");

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
                    var recomendation = JsonConvert.DeserializeObject<Recomendation>(message);
                    repo.SendMail(recomendation, templatesPath, runnerName);
                    Console.WriteLine(" [x] Received : {0}", message);
                };

                channel.BasicConsume(queue: "books",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Listning on the Rabbit MQ,  Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
