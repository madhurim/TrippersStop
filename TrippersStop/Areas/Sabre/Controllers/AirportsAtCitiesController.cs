using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippersStop.TraveLayer;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class AirportsAtCitiesController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public AirportsAtCitiesController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }
        public HttpResponseMessage Get(string city)
        {
            string url = string.Format("v1/lists/supported/cities/{0}/airports/", city);           
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            APIHelper.SetApiKey(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                _cacheService.Expire(_apiCaller.SabreTokenKey);
                _cacheService.Expire(_apiCaller.SabreTokenExpireKey);
                APIHelper.SetApiKey(_apiCaller, _cacheService);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_AirportsAtCitiesLookup airportsAtCities = new OTA_AirportsAtCitiesLookup();
                airportsAtCities = ServiceStackSerializer.DeSerialize<OTA_AirportsAtCitiesLookup>(result.Response);
                Mapper.CreateMap<OTA_AirportsAtCitiesLookup, AirportsAtCities>();
                AirportsAtCities airports = Mapper.Map<OTA_AirportsAtCitiesLookup, AirportsAtCities>(airportsAtCities);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, airports);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);              
        }
    }
}
