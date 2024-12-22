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
        public const string MAIL_HOST = "mail";  // Use container name in Docker
        public const int MAIL_PORT = 1025;

        public async void SendMail(RecommendationMessage message, string templatePath, string runnerName)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Mr Recomender", "recs@engine.com"));
            email.To.Add(new MailboxAddress(message.RecipientEmail, message.RecipientEmail));
            email.Subject = $"Book Recommendation - {message.Recommendation.Title}. Delivered by: {runnerName}";

            var bodyBuilder = new BodyBuilder();
            string templateContent;
            using (StreamReader SourceReader = System.IO.File.OpenText($"{templatePath}{Path.DirectorySeparatorChar}email.tmpl"))
            {
                templateContent = SourceReader.ReadToEnd();
            }
            StringBuilder myStringBuilder = new StringBuilder();

            myStringBuilder.Append("<h2>Here is a book recommendation, please enjoy</h2>");
            myStringBuilder.Append("<h3>Brought to you by our recommendations team!</h3>");
            myStringBuilder.Append("<dl>");
            myStringBuilder.Append("<dt>Title</dt>");
            myStringBuilder.Append($"<dd>{message.Recommendation.Title}</dd>");
            myStringBuilder.Append("<dt>Author</dt>");
            myStringBuilder.Append($"<dd>{message.Recommendation.Author}</dd>");
            myStringBuilder.Append("<dt>Language</dt>");
            myStringBuilder.Append($"<dd>{message.Recommendation.Language}</dd>");
            myStringBuilder.Append("<dt>Page Count</dt>");
            myStringBuilder.Append($"<dd>{message.Recommendation.Pages}</dd>");
            myStringBuilder.Append("<dt>Wiki</dt>");
            myStringBuilder.Append($"<dd><a href='{message.Recommendation.Link}' />{message.Recommendation.Link}</dd>");
            myStringBuilder.Append("</dl>");

            bodyBuilder.HtmlBody = string.Format(templateContent,
                        GetStyleText(templatePath),
                        myStringBuilder.ToString()
                      );

            email.Body = bodyBuilder.ToMessageBody();

            using (var mailClient = new SmtpClient())
            {
                await mailClient.ConnectAsync(MAIL_HOST, MAIL_PORT, SecureSocketOptions.None);
                await mailClient.SendAsync(email);
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