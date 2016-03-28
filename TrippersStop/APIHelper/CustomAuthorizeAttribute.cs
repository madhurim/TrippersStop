using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TrippismApi
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";

        protected CustomPrincipal CurrentUser
        {
            get { return Thread.CurrentPrincipal as CustomPrincipal; }
            set { Thread.CurrentPrincipal = value as CustomPrincipal; }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                AuthenticationHeaderValue authValue = actionContext.Request.Headers.Authorization;
                if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter) && authValue.Scheme == BasicAuthResponseHeaderValue)
                {
                    Credentials parsedCredentials = GetCredentialsFromHeader(authValue);
                    if (parsedCredentials != null)
                    {
                        if (IsValidKey(parsedCredentials.UserId))
                        {
                            CurrentUser = new CustomPrincipal(parsedCredentials.UserId);
                            return;
                        }
                    }
                }
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                return;

            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                return;
            }
        }

        public class CustomPrincipal : IPrincipal
        {
            public IIdentity Identity { get; private set; }
            public bool IsInRole(string role)
            {
                return true;
            }

            public CustomPrincipal(string UserId)
            {
                this.Identity = new GenericIdentity(UserId);
            }
        }

        public static Credentials GetCredentialsFromHeader(AuthenticationHeaderValue authHeader)
        {
            if (authHeader != null && !String.IsNullOrWhiteSpace(authHeader.Parameter) && authHeader.Scheme == "Basic")
            {
                string userID = string.Empty;
                string parameter64Dec = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));

                if (parameter64Dec.IndexOf(":") > 0)
                {
                    userID = parameter64Dec.Substring(0, parameter64Dec.IndexOf(":"));
                }

                return new Credentials() { UserId = userID };
            }
            return null;
        }

        public static bool IsValidKey(string key)
        {
            try
            {
                string JsonContent = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Scripts/authkeys.js"));
                var ad = Newtonsoft.Json.Linq.JArray.Parse(JsonContent);
                return ad.Any(x => Convert.ToString(x) == key);
            }
            catch
            {
                return false;
            }
        }
    }

    public class Credentials
    {
        public string UserId { get; set; }
    }
}