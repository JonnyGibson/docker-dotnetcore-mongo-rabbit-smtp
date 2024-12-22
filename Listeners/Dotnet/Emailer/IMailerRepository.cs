using System.Threading.Tasks;
using Emailer.Models;


namespace Emailer
{


    public interface IMailerRepository
    {
        void SendMail(RecommendationMessage message, string templatePath, string runnerName);
    }
}