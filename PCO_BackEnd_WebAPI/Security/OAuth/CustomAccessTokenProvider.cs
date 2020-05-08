using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Security.OAuth
{
    public class CustomAccessTokenProvider : AuthenticationTokenProvider
    {

        public void Create(AuthenticationTokenCreateContext context)
        {
            string x = "12345";
            Console.WriteLine(x);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}