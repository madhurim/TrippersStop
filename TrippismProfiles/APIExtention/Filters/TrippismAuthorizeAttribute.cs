using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using TrippismRepositories;

namespace TrippismProfiles
{
    /// <summary>
    /// It's used for Trippism Authorization.
    /// </summary>
    public class TrippismAuthorizeAttribute : AuthorizeAttribute
    {
        private const string SecurityToken = "SecurityToken";
        /// <summary>
        /// It's used to Authorization request.
        /// </summary>
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }

            HandleUnauthorizedRequest(filterContext);
        }
        /// <summary>
        /// It's used to handle Unauthorized Request.
        /// </summary>
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }

        private bool Authorize(HttpActionContext actionContext)
        {
            try
            {
                var userAgent = ApiHelper.GetClientUserAgent(actionContext.Request);                
                IEnumerable<string> headerValues;
                var token= string.Empty;
                if (actionContext.Request.Headers.TryGetValues(SecurityToken, out headerValues))
                {
                    token = headerValues.FirstOrDefault();
                    var authDetailsRepository = System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver
                        .GetService(typeof(IAuthDetailsRepository)) as IAuthDetailsRepository;
                   var result= SecurityManager.IsTokenValid(token, ApiHelper.GetClientIP(actionContext.Request), userAgent, authDetailsRepository);
                   if (result.Item1)
                   {
                       actionContext.Request.Headers.Add("AuthId",result.Item2.ToString());
                   }
                   return result.Item1;
                }
                return false;              
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}