using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using PCO_BackEnd_WebAPI.Security.Entity;
using System.Data.SqlClient;
using PCO_BackEnd_WebAPI.Models.Roles;

namespace PCO_BackEnd_WebAPI.Security.Authorization
{
    public class CustomAuthFilter : AuthorizationFilterAttribute
    {
        private readonly string _clientType;

        public CustomAuthFilter()
        {
            this._clientType = string.Empty;
        }

        public CustomAuthFilter(string cType)
        {
            this._clientType = cType;
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            IEnumerable<string> requestHeaders;
            string apiKey = null;

            var isAPIKeyExists = actionContext.Request.Headers.TryGetValues(PCO_Constants.APIKEY_HEADER, out requestHeaders);
            if (isAPIKeyExists)
            {
                apiKey = requestHeaders.FirstOrDefault();
                if (apiKey == null)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                else
                {
                    using (ClientCredentialsContext clientContext = new ClientCredentialsContext())
                    {
                        string command = string.Format("EXEC pc0_credentials.GetRole @client_password, @client_type");
                        SqlParameter parameter1 = new SqlParameter("@client_type", this._clientType);
                        SqlParameter parameter2 = new SqlParameter("@client_password", apiKey);

                        //Get Website Type
                        var type = clientContext.Database.SqlQuery<string>(command, parameter1, parameter2).ToList();

                        if (type.Count <= 0)
                        {
                            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        }
                    }
                }
            }
            else
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            return base.OnAuthorizationAsync(actionContext, cancellationToken);

        }
    }
}