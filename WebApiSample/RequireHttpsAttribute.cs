using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApiSample
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Found);
                actionContext.Response.Content = new StringContent("<p> use https insted of http </p>", Encoding.UTF8, "text/html");
                UriBuilder builder = new UriBuilder(actionContext.Request.RequestUri);
                builder.Scheme = Uri.UriSchemeHttps;
                builder.Port = 44307;
                actionContext.Response.Headers.Location = builder.Uri;
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
            
        }
    }
}