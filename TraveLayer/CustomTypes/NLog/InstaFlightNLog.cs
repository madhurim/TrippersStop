using System.Net.Http;
namespace TraveLayer.CustomTypes.NLog
{
    public class InstaFlightNLog
    {
        public string Request { get; set; }
        public HttpResponseMessage Response { get; set; }
    }
}
