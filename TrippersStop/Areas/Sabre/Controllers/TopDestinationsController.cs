using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippersStop.TraveLayer;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class TopDestinationsController : ApiController
    {
        // GET api/lookup
        [HttpGet]
        public HttpResponseMessage Get()
        {
            string url = string.Format("v1/lists/top/destinations");
            return GetDataFromSabre(url);
        }
        public HttpResponseMessage GetTopDestinationByTheme(string origin, string theme, string region)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}&theme={1}&region={2}", origin, theme, region);
            return GetDataFromSabre(url);
        }
        public HttpResponseMessage GetTopDestinationByairportCode(string airportCode)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}", airportCode);
            return GetDataFromSabre(url);
        }
        public HttpResponseMessage GetTopDestinationByCountryCode(string countryCode)
        {
            string url = string.Format("v1/lists/top/destinations?origincountry={0}", countryCode);
            return GetDataFromSabre(url);
        }

        public HttpResponseMessage GetTopDestinationByDestinationType(string origin ,string destinationType)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}&destinationtype={1}", origin,destinationType);
            return GetDataFromSabre(url);
        }

        public HttpResponseMessage GetTopDestinations(int number)
        {
            string url = string.Format("v1/lists/top/destinations?topdestinations={0}", number);
            return GetDataFromSabre(url);
        }
        public HttpResponseMessage GetTopDestinationsByDestinationCountry(string destinationcountry)
        {
            string url = string.Format("v1/lists/top/destinations?destinationcountry={0}", destinationcountry);
            return GetDataFromSabre(url);
        }

        public HttpResponseMessage GetTopDestinationsByRegion(string region)
        {
            string url = string.Format("v1/lists/top/destinations?region={0}", region);
            return GetDataFromSabre(url);
        }
        private HttpResponseMessage GetDataFromSabre(string url)
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
