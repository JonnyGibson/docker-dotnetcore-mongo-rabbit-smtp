namespace BookRecomenderApi.Controllers
{
    using BookRecomenderApi.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;
    using BookRecomenderApi.Repositories;

    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class MailController : Controller
    {
        private readonly IMailerRepository _mailer;
        private IHostingEnvironment _env;
        public MailController(IMailerRepository mailer, IHostingEnvironment env)
        {
            _mailer = mailer;
            _env = env;
        }

        [HttpPost]
        public string Post(string message)
        {
            try
            {
                var webRoot = _env.ContentRootPath;
                var templatesPath = webRoot + "/Templates";
                // var file = webRoot + "/Templates/email.html";
                _mailer.SendMail(new Email()
                {
                    Id = 0,
                    CreatedDate = DateTime.Now,
                    Title = "Not Sure",
                    Content = message
                }, templatesPath);
              
                return "OK";
            }
            catch (System.Exception E)
            {
                return E.Message;
            }


        }

    }
}