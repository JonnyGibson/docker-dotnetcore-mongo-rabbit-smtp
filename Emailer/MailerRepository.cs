using System;
using System.IO;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Emailer.Models;


namespace Emailer
{

    public class MailerRepository : IMailerRepository
    {

        public const string MAIL_HOST = "localhost";
        // public const string MAIL_HOST = "mail";
        public const int MAIL_PORT = 1025;

        public async void SendMail(Recomendation recomendation, string templatePath, string runnerName)
        {
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MimeKit.MailboxAddress("Mr Recomender", "recs@engine.com"));
            message.To.Add(new MimeKit.MailboxAddress("testMailBox", "test@fake.com"));
            message.Subject = "Recomendation - " + recomendation.title+". Delivered by: "+runnerName;

            var bodyBuilder = new BodyBuilder();
            string templateContent;
            using (StreamReader SourceReader = System.IO.File.OpenText(templatePath + "\\email.tmpl"))
            {
                templateContent = SourceReader.ReadToEnd();
            }
            StringBuilder myStringBuilder = new StringBuilder();

            myStringBuilder.Append("<h2>Here is a book recomendation, please enjoy</h2>");
            myStringBuilder.Append("<h3>Brought to you by our recomendations team!</h3>");
            myStringBuilder.Append("<dl>");
            myStringBuilder.Append("<dt>Title</dt>");
            myStringBuilder.Append($"<dd>{recomendation.title}</dd>");
            myStringBuilder.Append("<dt>Author</dt>");
            myStringBuilder.Append($"<dd>{recomendation.author}</dd>");
            myStringBuilder.Append("<dt>language</dt>");
            myStringBuilder.Append($"<dd>{recomendation.language}</dd>");
            myStringBuilder.Append("<dt>Page Count</dt>");
            myStringBuilder.Append($"<dd>{recomendation.pages}</dd>");
            myStringBuilder.Append("<dt>Wiki</dt>");
            myStringBuilder.Append($"<dd><a href='{recomendation.link}' />{recomendation.link}</dd>");
            myStringBuilder.Append("</dl>");

            bodyBuilder.HtmlBody = string.Format(templateContent,
                        GetStyleText(templatePath),
                        myStringBuilder.ToString()
                      );

            message.Body = bodyBuilder.ToMessageBody();

            using (var mailClient = new SmtpClient())
            {
                await mailClient.ConnectAsync(MAIL_HOST, MAIL_PORT, SecureSocketOptions.None);
                await mailClient.SendAsync(message);
                await mailClient.DisconnectAsync(true);
            }
        }

        private string GetStyleText(string templatePath)
        {
            using (StreamReader SourceReader = System.IO.File.OpenText(templatePath + "/style.css"))
            {
                return SourceReader.ReadToEnd();
            }
        }
    }

}