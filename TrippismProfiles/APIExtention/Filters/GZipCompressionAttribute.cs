using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;


namespace TrippismProfiles
{
    /// <summary>
    /// It's used to GZip Compression.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GZipCompressionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// It's used to perform action for Compression.
        /// </summary>
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            var acceptedEncoding = context.Response.RequestMessage.Headers.AcceptEncoding.First().Value;

            if (!acceptedEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)
                && !acceptedEncoding.Equals("deflate", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }
            if (context.Response.Content!=null)
            context.Response.Content = new CompressedContent(context.Response.Content, acceptedEncoding);
        }
    }


}