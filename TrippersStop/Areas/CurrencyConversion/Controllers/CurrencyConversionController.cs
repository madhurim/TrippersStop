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
        public CurrencyConversionController(ICurrencyConversionAPICaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        [HttpGet]
        [Route("api/CurrencyConversion/Convert")]
        public HttpResponseMessage Convert(CurrencyConversionInput currencyConversionInput)
        {           
            APIResponse result = _apiCaller.Get(null).Result;

            CurrencyConversionOutput rate = new CurrencyConversionOutput();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                rate = ServiceStackSerializer.DeSerialize<CurrencyConversionOutput>(result.Response);
                TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion currencyRates = Mapper.Map<CurrencyConversionOutput, TraveLayer.CustomTypes.CurrencyConversion.ViewModels.CurrencyConversion>(rate);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, rate);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
