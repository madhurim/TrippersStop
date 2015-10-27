using ExpressMapper;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Constants.Response;
using Trippism.APIExtention.Filters;
using TrippismApi.TraveLayer;
namespace Trippism.Areas.Constants.Controllers
{
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class ConstantsController : ApiController
    {
        ICacheService _cacheService;
        string _expireTime = ConfigurationManager.AppSettings["RedisExpireInMin"].ToString();
        const string TrippismCurrencySymbolsKey = "Trippism.CurrencySymbols";
        public ConstantsController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [ResponseType(typeof(TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel))]
        public async Task<HttpResponseMessage> GetCurrencySymbols()
        {
            var tripCurrencySymbols = _cacheService.GetByKey<TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel>(TrippismCurrencySymbolsKey);
            if (tripCurrencySymbols != null)
                return Request.CreateResponse(HttpStatusCode.OK, tripCurrencySymbols);

            string jsonPath = GetFullPath(ConfigurationManager.AppSettings["CurrencySymbolsJsonPath"].ToString());
            return await Task.Run(() =>
            { return GetCurrencySymbolsResponse(jsonPath); });
        }
        private HttpResponseMessage GetCurrencySymbolsResponse(string jsonPath)
        {
            StreamReader readerCurrencySymbolJson = new StreamReader(jsonPath);
            string currencySymbolJsonString = readerCurrencySymbolJson.ReadToEnd();

            CurrencySymbols currencySymbols = new CurrencySymbols();
            currencySymbols = ServiceStackSerializer.DeSerialize<CurrencySymbols>(currencySymbolJsonString);
            TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel currencySymbolViewModel = Mapper.Map<CurrencySymbols, TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel>(currencySymbols);
            _cacheService.Save<TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel>(TrippismCurrencySymbolsKey, currencySymbolViewModel);
            return Request.CreateResponse(HttpStatusCode.OK, currencySymbolViewModel);
        }
        private string GetFullPath(string path)
        {
            return System.Web.HttpContext.Current.Server.MapPath(path);
        }
    }
}
