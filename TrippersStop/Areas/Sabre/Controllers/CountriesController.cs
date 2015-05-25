using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippismApi.TraveLayer;

namespace TrippismApi.Areas.Sabre.Controllers
{
    
    public class CountriesController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public CountriesController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;        
        }
        // GET api/countries
        public HttpResponseMessage Get()
        {
            string url = "v1/lists/supported/countries";
            return GetResponse(url);
        }
        public HttpResponseMessage Get(string pointofsalecountry)
        {
            string url = string.Format("v1/lists/supported/countries?pointofsalecountry={0}", pointofsalecountry);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
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
                OTA_CountriesLookup cities = new OTA_CountriesLookup();
                cities = ServiceStackSerializer.DeSerialize<OTA_CountriesLookup>(result.Response);
                Mapper.CreateMap<OTA_CountriesLookup, Countries>();
                Countries countries = Mapper.Map<OTA_CountriesLookup, Countries>(cities);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, countries);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
