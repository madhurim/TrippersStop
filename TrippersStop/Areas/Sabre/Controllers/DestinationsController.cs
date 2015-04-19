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
using System.Reflection;
using System.Text;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using AutoMapper;
using TraveLayer.CustomTypes.Sabre.Response;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class DestinationsController : ApiController
    {

        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
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
        private string GetURL(Destinations destinationsRequest)
        {
            StringBuilder url = new StringBuilder();
            url.Append("v1/shop/flights/fares?");
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
                        url.Append(string.Join("=", propertyName, propertyValue));
                        separator = "&";
                    }
                }
            }
            return url.ToString();
        }
        private HttpResponseMessage GetResponse(string url)
        {
            APIHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                APIHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_DestinationFinder cities = new OTA_DestinationFinder();
                cities = ServiceStackSerializer.DeSerialize<OTA_DestinationFinder>(result.Response);
                Mapper.CreateMap<OTA_DestinationFinder, Fares>();
                Fares fares = Mapper.Map<OTA_DestinationFinder, Fares>(cities);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fares);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}