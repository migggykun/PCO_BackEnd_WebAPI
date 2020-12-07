using Microsoft.AspNet.Identity;
using PCO_BackEnd_WebAPI.App_Start;
using PCO_BackEnd_WebAPI.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Email
{
    public class ResetPasswordService : BaseEmailService
    {
        private int userID {get;set;}
        private string code { get; set; }
        private string idToken { get; set; }
        private string callbackURL { get; set; }
        private string emailBody { get; set; }

        public ResetPasswordService(int id)
        {
            this.userID = id;
        }

        public override async Task SendEmail(ApplicationUserManager userManager)
        {
            var user = await base.GetUser(userManager, userID);
            if (user != null)
            {
                string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                string idToken = StringManipulationHelper.SetParameter(user.Email, code, user.IsAdmin);
                string callbackURL = StringManipulationHelper.SetResetPasswordURL(idToken, user.IsAdmin);
                string emailBody = EmailTemplate.FormatResetPasswordBody(callbackURL);
                await SendEmailAsync(user.Email, EmailTemplate.RESET_PASSWORD_HEADER, emailBody);
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