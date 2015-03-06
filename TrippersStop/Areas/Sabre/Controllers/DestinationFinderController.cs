using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippersStop.TraveLayer;
using TraveLayer.APIServices;
using ServiceStack;


namespace TrippersStop.Areas.Sabre.Controllers
{
    public class DestinationFinderController : ApiController
    {
        // GET api/DestinationFinder
        public HttpResponseMessage Get(string origin, string departuredate, string returndate)
        {
            string url = string.Format("https://api.test.sabre.com/v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}",origin,departuredate,returndate);
            SabreAPICaller topDestinationAPI = new SabreAPICaller();
            topDestinationAPI.Accept = "application/json";
            topDestinationAPI.ContentType = "application/x-www-form-urlencoded";
            //TBD : Aoid call for getting token
            string token = topDestinationAPI.GetToken().Result;
            topDestinationAPI.Authorization = "bearer";
            topDestinationAPI.ContentType = "application/json";
            //TBD : URL configurable using XML
            String result = topDestinationAPI.GetResponseByUrl(url).Result;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        // GET api/DestinationFinder
        public HttpResponseMessage Get(string origin, string lengthofstay)
        {
            string url = string.Format("https://api.test.sabre.com/v1/shop/flights/fares?origin={0}&lengthofstay={1}", origin, lengthofstay);
            SabreAPICaller topDestinationAPI = new SabreAPICaller();
            topDestinationAPI.Accept = "application/json";
            topDestinationAPI.ContentType = "application/x-www-form-urlencoded";
            //TBD : Aoid call for getting token
            string token = topDestinationAPI.GetToken().Result;
            topDestinationAPI.Authorization = "bearer";
            topDestinationAPI.ContentType = "application/json";
            //TBD : URL configurable using XML
            String result = topDestinationAPI.GetResponseByUrl(url).Result;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

      
    }
}
