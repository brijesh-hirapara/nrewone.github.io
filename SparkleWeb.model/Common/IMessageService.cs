
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Common
{
    public interface IMessageService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<bool> SendSmsAsync(string number, string message);
        Task<bool> SendEmailAttachmentAsync(string email, string subject, string message, string fileName, string filePath);
    }
    public class MessagingService : IMessageService
    {
        IConfiguration _configuration;

        public MessagingService(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                MailMessage mailMessege = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                mailMessege.From = new MailAddress("wordpress.sparkleweb@gmail.com");
                mailMessege.To.Add(new MailAddress(email));
                mailMessege.Subject = subject;
                mailMessege.IsBodyHtml = true;
                mailMessege.Body = message;           
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential();
                networkCredential.UserName = "wordpress.sparkleweb@gmail.com";
                networkCredential.Password = "sparkle@123";
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                await smtp.SendMailAsync(mailMessege);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //            try
            //            {
            //                var client = new SendGridClient(_configuration.GetSection(Constants.SendGridApiKey).Value);
            //                var msg = new SendGridMessage()
            //                {
            //                    From = new EmailAddress(_configuration.GetSection(Constants.SendGridEmailKey).Value, _configuration.GetSection(Constants.SendGridEmailLabelKey).Value),
            //                    Subject = subject,
            //                    PlainTextContent = message,
            //                    HtmlContent = message
            //                };

            //                msg.AddTo(new EmailAddress(email));

            //                // Disable click tracking.
            //                // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            //                msg.SetClickTracking(false, false);

            //                var response = await client.SendEmailAsync(msg);

            //                return response.StatusCode.Equals(HttpStatusCode.Accepted) ? true : false;
            //            }
            //            catch (Exception ex)
            //            {
            //#if DEBUG
            //                Console.WriteLine(ex.Message);
            //#endif

            //                return false;
            //            }

        }

        public Task<bool> SendEmailAttachmentAsync(string email, string subject, string message, string fileName, string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendSmsAsync(string number, string message)
        {
            throw new NotImplementedException();
        }
    }
}
