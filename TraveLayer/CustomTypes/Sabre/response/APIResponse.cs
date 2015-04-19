using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.Response
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }
    }
}
