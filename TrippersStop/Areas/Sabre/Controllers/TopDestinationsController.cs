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
            string url = string.Format("https://api.test.sabre.com/v1/lists/top/destinations");
            return GetDataFeomSabre(url);
        }
        public HttpResponseMessage GetTopDestinationByTheme(string origin, string theme, string region)
        {
            string url = string.Format("https://api.test.sabre.com/v1/lists/top/destinations?origin={0}&theme={1}&region={2}", origin, theme, region);
            return GetDataFeomSabre(url);
        }
        public HttpResponseMessage GetTopDestinationByairportCode(string airportCode)
        {
            string url = string.Format("https://api.test.sabre.com/v1/lists/top/destinations?origin={0}", airportCode);
            return GetDataFeomSabre(url);
        }
        public HttpResponseMessage GetTopDestinationByCountryCode(string countryCode)
        {
            string url = string.Format("https://api.test.sabre.com/v1/lists/top/destinations?origincountry={0}", countryCode);
            return GetDataFeomSabre(url);
        }

        public HttpResponseMessage GetTopDestinationByDestinationType(string origin ,string destinationType)
        {
            string url = string.Format("https://api.test.sabre.com/v1/lists/top/destinations?origin={0}&destinationtype={1}", origin,destinationType);
            return GetDataFeomSabre(url);
        }

        public HttpResponseMessage GetTopDestinations(int number)
        {
            string url = string.Format("https://api.test.sabre.com/v1/lists/top/destinations?topdestinations={0}", number);
            return GetDataFeomSabre(url);
        }
        public HttpResponseMessage GetTopDestinationsByDestinationCountry(string destinationcountry)
        {
            string url = string.Format("https://api.test.sabre.com/v1/lists/top/destinations?destinationcountry={0}", destinationcountry);
            return GetDataFeomSabre(url);
        }

        public HttpResponseMessage GetTopDestinationsByRegion(string region)
        {
            string url = string.Format("https://api.test.sabre.com/v1/lists/top/destinations?region={0}", region);
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
            String result = topDestinationAPI.GetResponseByUrl(url).Result;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}
