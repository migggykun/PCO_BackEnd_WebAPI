using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Security.OAuth
{
    public class CustomOAuthGrantResourceOwner : OAuthGrantResourceOwnerCredentialsContext
    {
        public CustomOAuthGrantResourceOwner(IOwinContext context, OAuthAuthorizationServerOptions options, string clientId, string email, string password, IList<string> scope)
                                            : base (context, options, clientId, email, password, scope)
        {
 
        }
        public string Email { get; set; }
    }
}