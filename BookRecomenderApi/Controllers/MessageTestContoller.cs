namespace BookRecomenderApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class MessageTestController : Controller
    {

        public MessageTestController()
        {


        }

        [HttpPost]
        public string Post()
        {
            try
            {

                // return Messaging.Send.DoSend(new Models.Todo()
                // {
                //     Id = 0,
                //     InternalId = new MongoDB.Bson.ObjectId(),
                //     Title = "my Title",
                //     Content = "Some Content"
                // });
                return null;
            }
            catch (System.Exception E)
            {
                return E.Message;
            }


        }

    }
}