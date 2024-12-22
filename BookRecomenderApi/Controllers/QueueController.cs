using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using BookRecomenderApi.Models;

namespace BookRecomenderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly RabbitMQConfig _rabbitConfig;

        public QueueController(RabbitMQConfig rabbitConfig)
        {
            _rabbitConfig = rabbitConfig;
        }

        [HttpGet("Status")]
        public IActionResult GetStatus()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitConfig.HostName,
                    UserName = _rabbitConfig.UserName,
                    Password = _rabbitConfig.Password,
                    RequestedConnectionTimeout = TimeSpan.FromSeconds(3)
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                
                var queueDeclare = channel.QueueDeclare(queue: "books",
                                                       durable: false,
                                                       exclusive: false,
                                                       autoDelete: false,
                                                       arguments: null);

                return Ok(new { MessageCount = queueDeclare.MessageCount });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ Connection Error - Host: {_rabbitConfig.HostName}, User: {_rabbitConfig.UserName}");
                Console.WriteLine($"Error Details: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Error: {ex.InnerException.Message}");
                }
                
                return StatusCode(500, new { 
                    Error = "Failed to connect to message queue",
                    Details = ex.Message,
                    Host = _rabbitConfig.HostName
                });
            }
        }
    }
} 