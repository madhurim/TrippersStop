
namespace TrippismProfiles
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    /// <summary>
    /// It's used for exception handling.
    /// </summary>
    public class TripperExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// It's used to perform action on exception.
        /// </summary>
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
            else
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
