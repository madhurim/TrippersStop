using BusinessLogic;
using ExpressMapper;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.CurrencyConversion.Request;
using TraveLayer.CustomTypes.CurrencyConversion.Response;
using TraveLayer.CustomTypes.Sabre.Response;
using TrippismApi.TraveLayer;


namespace Trippism.Areas.CurrencyConversion.Controllers
{
    public class CurrencyConversionController : ApiController
    {
        ICurrencyConversionAPICaller _apiCaller;
        readonly IBusinessLayer<CurrencyConversionOutput, TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion> _iBusinessLayer;
        public CurrencyConversionController(ICurrencyConversionAPICaller apiCaller,IBusinessLayer<CurrencyConversionOutput,TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion> iBusinessLayer)
        {
            _apiCaller = apiCaller;
            _iBusinessLayer = iBusinessLayer;
        }

        [HttpGet]
        [Route("api/CurrencyConversion/Convert")]
        public HttpResponseMessage Convert([FromUri]CurrencyConversionInput currencyConversionInput)
        {           
            APIResponse result = _apiCaller.Get(null).Result;

            CurrencyConversionOutput rate = new CurrencyConversionOutput();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                rate = ServiceStackSerializer.DeSerialize<CurrencyConversionOutput>(result.Response);
                rate.Target = currencyConversionInput.Target;
                TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion currencyRates = _iBusinessLayer.Process(rate);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, currencyRates);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
