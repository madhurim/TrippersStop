using BusinessLogic;
using ExpressMapper;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.CurrencyConversion.Request;
using TraveLayer.CustomTypes.CurrencyConversion.Response;
using TraveLayer.CustomTypes.Sabre.Response;
using TrippismApi.TraveLayer;
using Trippism.APIExtention.Filters;


namespace Trippism.Areas.CurrencyConversion.Controllers
{
    public class CurrencyConversionController : ApiController
    {
        const string conversionCacheKey = "CurrencyConversion.";
        ICurrencyConversionAPICaller _apiCaller;
        ICacheService _cacheService;
        readonly IBusinessLayer<CurrencyConversionOutput, TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion> _iBusinessLayer;
        //public CurrencyConversionController(ICurrencyConversionAPICaller apiCaller, IBusinessLayer<CurrencyConversionOutput, TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion> iBusinessLayer);
        public CurrencyConversionController(ICurrencyConversionAPICaller apiCaller, ICacheService cacheService,IBusinessLayer<CurrencyConversionOutput, TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion> iBusinessLayer)
        
        {
            _apiCaller = apiCaller;
            _iBusinessLayer = iBusinessLayer;
            _cacheService = cacheService;
        }


        [HttpGet]
        [Route("api/CurrencyConversion/Convert")]
        public async Task<HttpResponseMessage> Convert([FromUri]CurrencyConversionInput currencyConversionInput)
        {            
            return await Task.Run(() =>
            { return GetResponse(currencyConversionInput); });
            ///{ return GetResponse(currencyConversionInput); });
        }

        private HttpResponseMessage GetResponse(CurrencyConversionInput currencyConversionInput)
        {
            string cacheKey = conversionCacheKey + currencyConversionInput.Base;

            CurrencyConversionOutput  rate = _cacheService.GetByKey<TraveLayer.CustomTypes.CurrencyConversion.Response.CurrencyConversionOutput>(cacheKey);            
            if (rate == null)
            {
                APIResponse result = _apiCaller.Get(null).Result;
                                
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    rate = ServiceStackSerializer.DeSerialize<CurrencyConversionOutput>(result.Response);

                    if (rate != null)
                        _cacheService.Save<TraveLayer.CustomTypes.CurrencyConversion.Response.CurrencyConversionOutput>(cacheKey, rate);                    
                }
            }
            rate.Target = currencyConversionInput.Target;                    
            TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion currencyRates = _iBusinessLayer.Process(rate);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, currencyRates);

            return response;            
        }
    }
}
