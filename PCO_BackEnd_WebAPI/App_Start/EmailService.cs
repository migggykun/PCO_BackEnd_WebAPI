using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;
using System.IO;

namespace PCO_BackEnd_WebAPI.App_Start
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Credentials:
            var credentialUserName = ConfigurationManager.AppSettings["FromEmail"];
            var sentFrom = ConfigurationManager.AppSettings["FromEmail"];
            var pwd = ConfigurationManager.AppSettings["password"];
            var senderName = ConfigurationManager.AppSettings["senderName"];

            // Configure the client:
            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient("webmail.philippineoptometry.org");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = false;
            client.Credentials = credentials;

            // Create the message:
            MailAddress from = new MailAddress(sentFrom, senderName);
            MailAddress to = new MailAddress(message.Destination);
            MailMessage mail = new MailMessage(from, to);

            mail.IsBodyHtml = true;
            mail.Subject = message.Subject;
            AlternateView emailBody = GetHTMLEmailBody(message.Body);
            mail.AlternateViews.Add(emailBody);

            // Send:
            return client.SendMailAsync(mail);
        }

        private AlternateView GetHTMLEmailBody(string body)
        {
            string binPath = Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath);
            AlternateView message = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            LinkedResource logo = new LinkedResource(binPath + @"\Resources\pco_logo.png", "image/png");
            LinkedResource header = new LinkedResource(binPath + @"\Resources\header_image.jpg", "image/jpg");
            logo.ContentId = "logo";
            header.ContentId = "header";
            message.LinkedResources.Add(logo);
            message.LinkedResources.Add(header);
            return message;
        }
    }
}