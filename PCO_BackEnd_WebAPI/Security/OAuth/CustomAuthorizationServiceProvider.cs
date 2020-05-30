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
using PCO_BackEnd_WebAPI.Security.Cryptography.Entity;
using PCO_BackEnd_WebAPI.Security.Cryptography.Hash;
using System.Data.SqlClient;
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
            var form = await context.Request.ReadFormAsync();
            string clientId = form["client_id"];
            string clientSecret = form["client_secret"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                context.Response.ReasonPhrase = "BadRequest";
                return;
            }

            using (ClientEntity clientContext = new ClientEntity())
            {
                string command = string.Format("EXEC pc0_credentials.GetRole @client_id, @client_password");
                SqlParameter parameter1 = new SqlParameter("@client_id", clientId);
                SqlParameter parameter2 = new SqlParameter("@client_password", clientSecret);
                
                //Get Website Type
                var type = clientContext.Database.SqlQuery<string>(command, parameter1, parameter2).ToList();
                if (type != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, type[0]));
                    context.Validated(identity);
                    return;
                }
                else
                {
                    context.SetError("Invalid grant type", "Login credentials are incorrect.");
                }
            }       
        }
    }
}