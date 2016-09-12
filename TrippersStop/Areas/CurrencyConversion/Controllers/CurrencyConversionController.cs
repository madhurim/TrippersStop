using BusinessLogic;
using ExpressMapper;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
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
        const string conversionCacheKey = "CurrencyConversion";
        ICurrencyConversionAPICaller _apiCaller;
        //ICacheService _cacheService;
        readonly IBusinessLayer<CurrencyConversionOutput, TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion> _iBusinessLayer;
        //public CurrencyConversionController(ICurrencyConversionAPICaller apiCaller, ICacheService cacheService,IBusinessLayer<CurrencyConversionOutput, TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion> iBusinessLayer)
        public CurrencyConversionController(ICurrencyConversionAPICaller apiCaller, IBusinessLayer<CurrencyConversionOutput, TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion> iBusinessLayer)
        {
            _apiCaller = apiCaller;
            _iBusinessLayer = iBusinessLayer;
            //_cacheService = cacheService;
        }


        [HttpGet]
        [ResponseType(typeof(TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion))]
        [Route("api/CurrencyConversion/Convert")]
        [TrippismCache(conversionCacheKey)]
        public async Task<IHttpActionResult> Convert([FromUri]CurrencyConversionInput currencyConversionInput)
        {
            //string cacheKey = conversionCacheKey + string.Join(".", currencyConversionInput.Base, currencyConversionInput.Target);
            //var conversionDetail = _cacheService.GetByKey<TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion>(cacheKey);
            //if (conversionDetail != null)
            //{
            //    return Request.CreateResponse(HttpStatusCode.OK, conversionDetail);
            //}
            return await Task.Run(() =>
            //{ return GetResponse(currencyConversionInput, cacheKey); });
            { return GetResponse(currencyConversionInput); });
        }

        private IHttpActionResult GetResponse(CurrencyConversionInput currencySearch)
        {
            APIResponse result = _apiCaller.Get(null).Result;

            CurrencyConversionOutput rate = new CurrencyConversionOutput();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                rate = ServiceStackSerializer.DeSerialize<CurrencyConversionOutput>(result.Response);
                rate.Target = currencySearch.Target;
                TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion currencyRates = _iBusinessLayer.Process(rate);

                //if (currencyRates != null)
                //    _cacheService.Save<TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion>(cacheKey, currencyRates);

                //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, currencyRates);

                return Ok(currencyRates);
            }
            return ResponseMessage(new HttpResponseMessage(result.StatusCode));
        }
    }
}
