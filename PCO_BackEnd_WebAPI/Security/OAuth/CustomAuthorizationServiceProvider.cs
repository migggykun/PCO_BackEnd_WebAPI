using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Roles;
namespace PCO_BackEnd_WebAPI.Security.OAuth
{
    public class CustomAuthorizationServiceProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            string email = context.UserName;
            string password = context.Password;
            string role;

            UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());
            var user = await Task.Run(() => unitOfWork.Accounts.UserManager.FindAsync(email, password));
            if (user != null)
            {
                role = user.IsAdmin ? UserRoles.ROLE_ADMIN : UserRoles.ROLE_MEMBER;
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
                identity.AddClaim(new Claim(ClaimTypes.Email, email));
                context.Validated(identity);
            }
            else
            {
                context.SetError("Invalid grant type", "Login credentials are incorrect.");
            }
        }
    }
}