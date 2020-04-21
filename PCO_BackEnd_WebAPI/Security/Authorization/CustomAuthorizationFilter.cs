using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Web.Http.Results;

namespace PCO_BackEnd_WebAPI.Security.Authorization
{
    public class CustomAuthorizationFilter : AuthorizeAttribute , IAuthenticationFilter
    {
        private readonly string _role;

        public CustomAuthorizationFilter()
        {
            _role = string.Empty;
        }

        public CustomAuthorizationFilter(string Role)
        {
            _role = Role;
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string authParameter = string.Empty;
            HttpRequestMessage requestMessage = context.Request;
            AuthenticationHeaderValue authorization = requestMessage.Headers.Authorization;
            if (authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Authorization Header", requestMessage);
            }
            if (authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid authorization type", requestMessage);
            }
            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Token", requestMessage);
            }

            context.Principal = TokenManager.GetPrincipal(authorization.Parameter);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result = await context.Result.ExecuteAsync(cancellationToken);
            context.Result = new ResponseMessageResult(result);
        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        private string _reasonPhrease;

        private HttpRequestMessage _request { get; set; }

        public AuthenticationFailureResult(string reasonPhase, HttpRequestMessage request)
        {
            _reasonPhrease = reasonPhase;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            message.RequestMessage = _request;
            message.ReasonPhrase = _reasonPhrease;
            return message;
        }
    }
}