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
                var clientDetails = clientContext.ClientInfos.ToList();               
                foreach (var c in clientDetails)
                {
                    var hashedClientId = new HashManager(clientId, c.salt).HashedValue;
                    var hashedClientSecret = new HashManager(clientSecret, c.salt).HashedValue;

                    if (string.Compare(hashedClientId, c.clientId) == 0 &&
                       string.Compare(hashedClientSecret, c.clientSecret) == 0)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, c.type));
                        context.Validated(identity);
                        return;
                    }
                }
                context.SetError("Invalid grant type", "Login credentials are incorrect.");
            }       
        }
    }
}