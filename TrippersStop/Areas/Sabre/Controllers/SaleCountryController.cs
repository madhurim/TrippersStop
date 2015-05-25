using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippismApi.TraveLayer;

namespace TrippismApi.Areas.Sabre.Controllers
{
    public class SaleCountryController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public SaleCountryController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;   
        }
        public HttpResponseMessage Get()
        {
            string url = "v1/lists/supported/pointofsalecountries";
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
                OTA_PointofSaleCountryCodeLookup countryCodes = new OTA_PointofSaleCountryCodeLookup();
                countryCodes = ServiceStackSerializer.DeSerialize<OTA_PointofSaleCountryCodeLookup>(result.Response);
                Mapper.CreateMap<OTA_PointofSaleCountryCodeLookup, PointofSaleCountryCode>();
                PointofSaleCountryCode pointofSaleCountryCode = Mapper.Map<OTA_PointofSaleCountryCodeLookup, PointofSaleCountryCode>(countryCodes);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pointofSaleCountryCode);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
