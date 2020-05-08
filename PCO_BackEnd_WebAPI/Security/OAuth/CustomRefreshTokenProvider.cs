using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using PCO_BackEnd_WebAPI.Security.Cryptography.Entity;
using PCO_BackEnd_WebAPI.Security.Cryptography.Hash;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;
namespace PCO_BackEnd_WebAPI.Security.OAuth
{
    public class CustomRefreshTokenProvider : AuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            string token = context.SerializeTicket();
            _refreshTokens.TryAdd(token, context.Ticket);
            context.SetToken(token);
        }

        public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            AuthenticationTicket ticket;

            var form = await context.Request.ReadFormAsync();
            var user = _refreshTokens.FirstOrDefault(x => x.Key == context.Token).Value;

            string role = user.Identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            string clientId = form["client_id"];
            string clientSecret = form["client_secret"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || user == null)
            {
                context.Response.StatusCode = 400; //Unauthorized
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

                    if(string.Compare(hashedClientId, c.clientId) == 0 && 
                       string.Compare(hashedClientSecret, c.clientSecret) == 0 &&
                       string.Compare(role, c.type) == 0)
                    {
                        if (_refreshTokens.TryRemove(context.Token, out ticket))
                        {
                            context.SetTicket(ticket);
                        }
                        break;
                    }
                }
            }
        }
    }
}