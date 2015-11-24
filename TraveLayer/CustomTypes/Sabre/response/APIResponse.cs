using System.Net;
using System.Net.Http;

namespace TraveLayer.CustomTypes.Sabre.Response
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }
        public string RequestUrl { get; set; }
        public HttpResponseMessage OriginalResponse { get; set; }
    }
}
