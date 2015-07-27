using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using Trippism.APIHelper;

namespace Trippism.APIExtention.Filters
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GZipCompressionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            var acceptedEncoding = context.Response.RequestMessage.Headers.AcceptEncoding.First().Value;

            if (!acceptedEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)
                && !acceptedEncoding.Equals("deflate", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            context.Response.Content = new CompressedContent(context.Response.Content, acceptedEncoding);
        }
    }


}