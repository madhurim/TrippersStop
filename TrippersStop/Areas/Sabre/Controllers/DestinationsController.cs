using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using Trippism.TraveLayer;
using TraveLayer.APIServices;
using ServiceStack;
using System.Reflection;
using System.Text;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using AutoMapper;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Configuration;


namespace Trippism.Areas.Sabre.Controllers
{
    public class DestinationsController : ApiController
    {

        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        const string _destinationKey = "Trippism.Destinations.All";
        string _expireTime = ConfigurationManager.AppSettings["RedisExpireInMin"].ToString();
        public DestinationsController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;      
        }

        // GET api/DestinationFinder
        public HttpResponseMessage Get([FromUri]Destinations destinationsRequest)
        {
            string url = GetURL(destinationsRequest);
            return GetResponse(url);
        }

        [Route("api/destinations/theme/{theme}")]
        [HttpGet]
        public HttpResponseMessage GetDestinationsByTheme(string theme,string origin,string departuredate,string returndate,string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&lengthofstay={3}&theme={4}", origin, departuredate, returndate, lengthofstay, theme);
            return GetResponse(url);
        }

        [Route("api/destinations/topcheapest/{topcheapest}")]
        [HttpGet]
        public HttpResponseMessage GetTopCheapestDestinations(int topcheapest, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&lengthofstay={3}", origin, departuredate, returndate, lengthofstay);
            return GetResponse(url, topcheapest);
        }

        private string GetURL(Destinations destinationsRequest)
        {
            StringBuilder url = new StringBuilder();
            url.Append("v1/shop/flights/fares?");
            //url.Append("v1/shop/flights?");
            Type type = destinationsRequest.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string propertyName = string.Empty;
            string propertyValue = string.Empty;
            string separator = string.Empty;
            foreach (PropertyInfo property in properties)
            {
                propertyName = property.Name;
                var result = property.GetValue(destinationsRequest, null);
                if (result != null)
                {
                    propertyValue = result.ToString();
                    if (!string.IsNullOrWhiteSpace(propertyValue))
                    {
                        url.Append(separator);
                        url.Append(string.Join("=", propertyName.ToLower(), propertyValue));
                        //url.Append(string.Join("=", propertyName, propertyValue));
                        separator = "&";
                    }
                }
            }
            return url.ToString();
        }
        private HttpResponseMessage GetResponse(string url,int topcheapest=0)
        {
            SabreApiTokenHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                SabreApiTokenHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_DestinationFinder cities = new OTA_DestinationFinder();
                cities = ServiceStackSerializer.DeSerialize<OTA_DestinationFinder>(result.Response);
                Mapper.CreateMap<OTA_DestinationFinder, Fares>();
                Fares fares = Mapper.Map<OTA_DestinationFinder, Fares>(cities);
                if (topcheapest != 0)
                {
                    fares.FareInfo = fares.FareInfo.OrderBy(f => f.LowestFare).Take(topcheapest).ToList();
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fares);
                return response;
            }         
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}