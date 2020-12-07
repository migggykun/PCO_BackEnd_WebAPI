using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using PCO_BackEnd_WebAPI.Models.Helpers;
using System.Reflection;
using System.IO;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.App_Start;
using Microsoft.AspNet.Identity;

namespace PCO_BackEnd_WebAPI.Models.Email
{
    public class ConfirmEmailService : BaseEmailService
    {

        private int userID { get; set; }
        private string code { get; set; }
        private string idToken { get; set; }
        private string callbackURL { get; set; }
        private string emailBody { get; set; }

        public ConfirmEmailService(int id)
        {
            this.userID = id;
        }

        public override async Task SendEmail(ApplicationUserManager userManager)
        {
            var user = await base.GetUser(userManager, userID);
            if (user != null)
            {
                string code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                string idToken = StringManipulationHelper.SetParameter(user.Email, code, user.IsAdmin);
                string callbackURL = StringManipulationHelper.SetConfirmEmailUrl(idToken, user.IsAdmin);
                string emailBody = EmailTemplate.FormatConfirmEmailBody(callbackURL);
                await SendEmailAsync(user.Email, EmailTemplate.CONFIRM_EMAIL_HEADER, emailBody);
            }
        }

        private async Task SendEmailAsync(string email, string header, string body)
        {
            EmailService es = new EmailService();
            IdentityMessage im = new IdentityMessage();
            im.Subject = header;
            im.Body = body;
            im.Destination = email;
            await es.SendAsync(im);
        }

    }
}