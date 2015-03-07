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
using TraveLayer.CustomTypes.Sabre.Request;


namespace TrippersStop.Areas.Sabre.Controllers
{
    public class DestinationFinderController : ApiController
    {
        // GET api/DestinationFinder
        public HttpResponseMessage Get(string origin, string departuredate, string returndate)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}",origin,departuredate,returndate);
            return GetDataFeomSabre(url);
        }

        

        // GET api/DestinationFinder
        public HttpResponseMessage Get(string origin, string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&lengthofstay={1}", origin, lengthofstay);
            return GetDataFeomSabre(url);
        }

        public HttpResponseMessage Get(string origin, string earliestdeparturedate, string latestdeparturedate, string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&earliestdeparturedate={1}&latestdeparturedate={2}&lengthofstay={3}", origin, earliestdeparturedate, latestdeparturedate, lengthofstay);
            return GetDataFeomSabre(url);
        }

        public HttpResponseMessage Get(string origin, string earliestdeparturedate, string latestdeparturedate, string lengthofstay, string theme)
        {
            //string url = string.Format("v1/shop/flights/fares?origin={0}&earliestdeparturedate={1}&latestdeparturedate={2}&lengthofstay={3}&theme={4}", destinationFinderRequest.Origin, destinationFinderRequest.Earliestdeparturedate, destinationFinderRequest.Latestdeparturedate, destinationFinderRequest.Lengthofstay, destinationFinderRequest.Theme);
            string url = string.Format("v1/shop/flights/fares?origin={0}&earliestdeparturedate={1}&latestdeparturedate={2}&lengthofstay={3}&theme={4}", origin, earliestdeparturedate, latestdeparturedate, lengthofstay,theme);
            return GetDataFeomSabre(url);
        }

        public HttpResponseMessage GetDestinationByMaxFare(string origin, string departuredate, string returndate, string maxfare)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&maxfare={3}", origin, departuredate, returndate, maxfare);
            return GetDataFeomSabre(url);
        }

        public HttpResponseMessage GetTopDestination(string origin, string departuredate, string returndate, string topdestinations)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&topdestinations={3}", origin, departuredate, returndate, topdestinations);
            return GetDataFeomSabre(url);
        }
        private HttpResponseMessage GetDataFeomSabre(string url)
        {
            SabreAPICaller topDestinationAPI = new SabreAPICaller();
            topDestinationAPI.Accept = "application/json";
            topDestinationAPI.ContentType = "application/x-www-form-urlencoded";
            //TBD : Aoid call for getting token
            string token = topDestinationAPI.GetToken().Result;
            topDestinationAPI.Authorization = "bearer";
            topDestinationAPI.ContentType = "application/json";
            //TBD : URL configurable using XML
            String result = topDestinationAPI.Get(url).Result;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}
