using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippersStop.TraveLayer;
using TraveLayer.APIServices;
using AutoMapper;
using TraveLayer.CustomTypes.Sabre.ViewModels;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class TopDestinationsController : ApiController
    {
        IAPIAsyncCaller apiCaller;
        public TopDestinationsController(IAPIAsyncCaller repository)
        {
            apiCaller = repository;
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            string token = apiCaller.GetToken().Result;
            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
        // GET api/lookup
        [HttpGet]
        public HttpResponseMessage Get()
        {
            string url = string.Format("v1/lists/top/destinations");
            return GetResponse(url);
        }
        public HttpResponseMessage GetTopDestinationByTheme(string origin, string theme, string region)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}&theme={1}&region={2}", origin, theme, region);
            return GetResponse(url);
        }
        public HttpResponseMessage GetTopDestinationByBackweeks(string origin, string lookbackweeks, string topdestinations)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}&lookbackweeks={1}&topdestinations={2}", origin, lookbackweeks, topdestinations);
            return GetResponse(url);
        }
        public HttpResponseMessage GetTopDestinationByairportCode(string airportCode)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}", airportCode);
            return GetResponse(url);
        }
        public HttpResponseMessage GetTopDestinationByCountryCode(string countryCode)
        {
            string url = string.Format("v1/lists/top/destinations?origincountry={0}", countryCode);
            return GetResponse(url);
        }

        public HttpResponseMessage GetTopDestinationByDestinationType(string origin ,string destinationType)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}&destinationtype={1}", origin,destinationType);
            return GetResponse(url);
        }

        public HttpResponseMessage GetTopDestinations(int number)
        {
            string url = string.Format("v1/lists/top/destinations?topdestinations={0}", number);
            return GetResponse(url);
        }
        public HttpResponseMessage GetTopDestinationsByDestinationCountry(string destinationcountry)
        {
            string url = string.Format("v1/lists/top/destinations?destinationcountry={0}", destinationcountry);
            return GetResponse(url);
        }

        public HttpResponseMessage GetTopDestinationsByRegion(string region)
        {
            string url = string.Format("v1/lists/top/destinations?region={0}", region);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            String result = apiCaller.Get(url).Result;
            TopDestinations destinations = new TopDestinations();
            destinations = ServiceStackSerializer.DeSerialize<TopDestinations>(result);
            Mapper.CreateMap<TopDestinations, TopDestination>();
            TopDestination topDestinations = Mapper.Map<TopDestinations, TopDestination>(destinations);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, topDestinations);
            return response;
        }
    }
}
