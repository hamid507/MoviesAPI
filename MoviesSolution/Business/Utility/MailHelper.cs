using Business.Dtos;
using Infrastructure.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Text;

namespace Business.Utility
{
    public static class MailHelper
    {
        public static bool SendMail(string fromAddress, string toAddress, MovieDto movieDto)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Movies API (noreply)", fromAddress);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("Recipient", toAddress);
                message.To.Add(to);

                message.Subject = "Movie recommendation of the week!";

                BodyBuilder bodyBuilder = new BodyBuilder();

                var imageBytes = ApiUtils.DownloadImageFromUrl(movieDto.PosterUrl);
                var contentType = new ContentType("image", "jpeg");
                var contentId = MimeKit.Utils.MimeUtils.GenerateMessageId();
                var image = (MimePart)bodyBuilder.LinkedResources.Add(movieDto.PosterId, imageBytes, contentType);
                image.ContentTransferEncoding = ContentEncoding.Base64;
                image.ContentId = contentId;

                bodyBuilder.Attachments.Add(image);

                string htmlBody = GenerateHtmlBodyFromMovie(movieDto, contentId);

                bodyBuilder.HtmlBody = htmlBody;

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                client.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                string host = AppSettings.GetProperty("SmtpHost");
                int port = int.Parse(AppSettings.GetProperty("SmtpPort"));
                string user = AppSettings.GetProperty("SmtpUsername");
                string pass = AppSettings.GetProperty("SmtpPassword");

                client.Connect(host, port, SecureSocketOptions.Auto);
                client.Authenticate(user, pass);

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static string GenerateHtmlBodyFromMovie(MovieDto movieDto, string contentId)
        {
            StringBuilder htmlBodyBuilder = new StringBuilder();

            htmlBodyBuilder.Append("<html>");

            htmlBodyBuilder.Append("<body>");

            htmlBodyBuilder.Append("<h3>");
            htmlBodyBuilder.Append(movieDto.Title);
            htmlBodyBuilder.Append("</h3>");

            htmlBodyBuilder.Append("</br></br>");

            htmlBodyBuilder.Append($"<p>IMDB rating: {movieDto.ImdbRating}</p>");

            htmlBodyBuilder.Append("</br></br>");

            if (!string.IsNullOrEmpty(movieDto.WikiShortDescription))
            {
                htmlBodyBuilder.Append($"<h4>Description:</h4>");
                htmlBodyBuilder.Append($"<p>{movieDto.WikiShortDescription}</p>");
            }

            //htmlBodyBuilder.Append($"<p><img src='cid:{contentId}'/></p>");

            htmlBodyBuilder.Append("</body>");

            htmlBodyBuilder.Append("</html>");

            return htmlBodyBuilder.ToString();
        }
    }
}
