using BookRecomenderApi.Repositories;
using System.Linq;

namespace BookRecomenderApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class RecomendationController : Controller
    {

        private readonly IRecomendationRepository recRepo;
        public RecomendationController(IRecomendationRepository _recRepo)
        {
            recRepo = _recRepo;

        }

        [HttpPost]
        public ActionResult Post(int count)
        {
            if (count > 100)
            {
                return BadRequest("Must be no more than 100");
            }
            try
            {
                var recomendations = recRepo.GetRecomendations(count).Result.ToList();
                Messaging.Send.DoSend(recomendations);
                return Ok(recomendations);

            }
            catch (System.Exception E)
            {
                return NotFound(E.Message);
            }


        }

    }
}