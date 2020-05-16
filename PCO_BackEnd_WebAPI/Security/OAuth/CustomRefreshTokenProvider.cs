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
            context.Ticket.Properties.ExpiresUtc = DateTime.Now.AddDays(30);

            _refreshTokens.TryAdd(token, context.Ticket);
            context.SetToken(token);
        }

        public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            AuthenticationTicket ticket;

            var form = await context.Request.ReadFormAsync();


            string clientId = form["client_id"];
            string clientSecret = form["client_secret"];

            var user = GetAuthenticationTicket(context.Token);
            bool isRefreshTokenValid = IsTokenValid(user);

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || 
                user == null || !isRefreshTokenValid) 
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                context.Response.ReasonPhrase = "BadRequest";
                return;
            }

            using (ClientEntity clientContext = new ClientEntity())
            {
                var clientDetails = clientContext.ClientInfos.ToList();
                string role = user.Identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
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

        private AuthenticationTicket GetAuthenticationTicket(string token)
        {
            return _refreshTokens.FirstOrDefault(x => x.Key == token).Value;
        }

        private bool IsTokenValid(AuthenticationTicket ticket)
        {
            return ticket == null ? false : (ticket.Properties.IssuedUtc >= DateTime.Now ? false : true);  
        }
    }
}