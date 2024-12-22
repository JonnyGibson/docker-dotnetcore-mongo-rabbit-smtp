using BookRecomenderApi.Repositories;
using System.Linq;
using BookRecomenderApi.Messaging;
using BookRecomenderApi.Models;
using System.Collections.Generic;

namespace BookRecomenderApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class RecomendationController : Controller
    {
        private readonly IRecomendationRepository recRepo;
        private readonly Send _messageSender;

        public RecomendationController(IRecomendationRepository _recRepo, RabbitMQConfig rabbitConfig)
        {
            recRepo = _recRepo;
            _messageSender = new Send(rabbitConfig);
        }

        [HttpPost]
        public ActionResult Post([FromBody] SendRecommendationsRequest request)
        {
            Console.WriteLine($"Received request for {request.Count} recommendations for {request.Recipients.Count} recipients");
            
            if (request.Count > 100)
            {
                return BadRequest("Must be no more than 100 recommendations");
            }

            if (request.Recipients == null || !request.Recipients.Any())
            {
                return BadRequest("At least one recipient email is required");
            }

            try
            {
                var recommendations = recRepo.GetRecomendations(request.Count).Result.ToList();
                Console.WriteLine($"Retrieved {recommendations.Count} recommendations from repository");
                
                // Create a message for each recipient
                var messages = new List<RecommendationMessage>();
                foreach (var recipient in request.Recipients)
                {
                    foreach (var recommendation in recommendations)
                    {
                        messages.Add(new RecommendationMessage(recipient, recommendation));
                    }
                }

                Console.WriteLine($"Sending {messages.Count} messages to queue ({recommendations.Count} recommendations × {request.Recipients.Count} recipients)");
                _messageSender.DoSend(messages);
                
                return Ok(new { 
                    RecommendationCount = recommendations.Count,
                    RecipientCount = request.Recipients.Count,
                    TotalMessageCount = messages.Count
                });
            }
            catch (System.Exception E)
            {
                Console.WriteLine($"Error in RecomendationController: {E.Message}");
                if (E.InnerException != null)
                {
                    Console.WriteLine($"Inner error: {E.InnerException.Message}");
                }
                return NotFound(E.Message);
            }
        }
    }

    public class SendRecommendationsRequest
    {
        public int Count { get; set; }
        public List<string> Recipients { get; set; } = new List<string>();
    }
}