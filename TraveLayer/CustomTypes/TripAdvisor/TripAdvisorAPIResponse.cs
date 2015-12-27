using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }
    }
}
