namespace BookRecomenderApi.Repositories
{
using System.Threading.Tasks;
    using BookRecomenderApi.Models;

    public interface IMailerRepository
    {
        void SendMail(Email email, string templatePath);
    }
}