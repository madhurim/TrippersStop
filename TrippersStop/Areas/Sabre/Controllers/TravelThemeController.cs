using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippersStop.Helper;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class TravelThemeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string url = string.Format("v1/lists/supported/shop/themes");
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}
