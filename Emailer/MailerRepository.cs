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

       //  public const string MAIL_HOST = "localhost";
        public const string MAIL_HOST = "mail";
        public const int MAIL_PORT = 1025;

        public async void SendMail(Recomendation recomendation, string templatePath)
        {
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MimeKit.MailboxAddress("Mr ToDo", "todo@engine.com"));
            message.To.Add(new MimeKit.MailboxAddress("testMailBox", "test@fake.com"));
            message.Subject = "A new To Do - " + recomendation.author;

            var bodyBuilder = new BodyBuilder();
            string templateContent;
            using (StreamReader SourceReader = System.IO.File.OpenText(templatePath + "\\email.tmpl"))
            {
                templateContent = SourceReader.ReadToEnd();
            }
            StringBuilder myStringBuilder = new StringBuilder();

            myStringBuilder.Append("<h2>Welcome to To Do, how do you do?</h2>");
            myStringBuilder.Append("A new item has been added");
            myStringBuilder.Append("<dl>");
            myStringBuilder.Append("<dt>Title</dt>");
            myStringBuilder.Append($"<dd>{recomendation.author}</dd>");
            myStringBuilder.Append("<dt>Content</dt>");
            myStringBuilder.Append($"<dd>{recomendation.country}</dd>");
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