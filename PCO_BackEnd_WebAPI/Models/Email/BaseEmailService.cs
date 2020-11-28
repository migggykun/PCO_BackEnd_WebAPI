using PCO_BackEnd_WebAPI.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Email
{
    public abstract class BaseEmailService
    {
        string code { get; set; }
        string idToken { get; set; }
        string callbackURL { get; set; }
        string emailBody { get; set; }
        public abstract Task SendEmail(ApplicationUserManager userManager);

        protected virtual async Task<ApplicationUser> GetUser(ApplicationUserManager userManager, int userID)
        {
            var user = await userManager.FindByIdAsync(userID);
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
