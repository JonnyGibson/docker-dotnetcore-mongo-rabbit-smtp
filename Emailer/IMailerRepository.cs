using System.Threading.Tasks;
using Emailer.Models;


namespace Emailer
{


    public interface IMailerRepository
    {
        void SendMail(Recomendation recomendation, string templatePath);
    }
}