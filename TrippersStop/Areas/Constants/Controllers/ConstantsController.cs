using DataLayer.Entities;
using DataLayer.Repositories;
using ExpressMapper;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Constants.Response;
using Trippism.APIExtention.Filters;
using Trippism.APIHelper;
using TrippismApi.TraveLayer;
namespace Trippism.Areas.Constants.Controllers
{
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class ConstantsController : ApiController
    {
        ICacheService _cacheService;
        string _expireTime = ConfigurationManager.AppSettings["RedisExpireInMin"].ToString();
        private const string TrippismCurrencySymbolsKey = "Trippism.Constants.CurrencySymbols";
        private const string TrippismAirportsKey = "Trippism.Constants.Airports";
        public ConstantsController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [Route("api/Constants/GetCurrencySymbols")]
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
            string currencySymbolJsonString = string.Empty;
            using (StreamReader readerCurrencySymbolJson = new StreamReader(jsonPath))
            {
                currencySymbolJsonString = readerCurrencySymbolJson.ReadToEnd();
            }

            CurrencySymbols currencySymbols = new CurrencySymbols();
            currencySymbols = ServiceStackSerializer.DeSerialize<CurrencySymbols>(currencySymbolJsonString);
            TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel currencySymbolViewModel = Mapper.Map<CurrencySymbols, TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel>(currencySymbols);
            _cacheService.Save<TraveLayer.CustomTypes.Constants.ViewModels.CurrencySymbolsViewModel>(TrippismCurrencySymbolsKey, currencySymbolViewModel);
            return Request.CreateResponse(HttpStatusCode.OK, currencySymbolViewModel);
        }

        [Route("api/Constants/GetAirports")]
        [ResponseType(typeof(List<TraveLayer.CustomTypes.Constants.Response.AirportsDetail>))]
        public async Task<HttpResponseMessage> GetAirports()
        {
            //var tripCurrencySymbols = _cacheService.GetByKey<TraveLayer.CustomTypes.Constants.Response.AirportRoot>(TrippismAirportsKey);
            //if (tripCurrencySymbols != null)
            //    return Request.CreateResponse(HttpStatusCode.OK, tripCurrencySymbols.AirportsRoot.AirportsDetail);
            string jsonPath = GetFullPath(ConfigurationManager.AppSettings["AirportsJsonPath"].ToString());
            return await Task.Run(() =>
            { return GetAirportsResponse(jsonPath); });
        }

        private HttpResponseMessage GetAirportsResponse(string jsonPath)
        {
            string currencySymbolJsonString = string.Empty;
            using (StreamReader readerCurrencySymbolJson = new StreamReader(jsonPath))
            {
                currencySymbolJsonString = readerCurrencySymbolJson.ReadToEnd();
            }
            var airports = ServiceStackSerializer.DeSerialize<AirportRoot>(currencySymbolJsonString);
            //_cacheService.Save<TraveLayer.CustomTypes.Constants.Response.AirportRoot>(TrippismAirportsKey, airports);
            return Request.CreateResponse(HttpStatusCode.OK, airports.AirportsRoot.AirportsDetail);
        }
        [HttpGet]
        [Route("api/Constants/MissingAirportLog")]
        public async Task MissingAirportLog(string Airportcode)
        {
            await Task.Run(() => { SaveAirportCode(Airportcode); });
        }
        private void SaveAirportCode(string Airportcode)
        {
            DefaultLog defaultLog = new DefaultLog();
            RepositoryDefaultLog objRepositoryDefaultLog = new RepositoryDefaultLog();
            string loggerName = "MissingAirportLogger";
            var data = objRepositoryDefaultLog.FindOne<DefaultLog>(x => x.Message == Airportcode && x.Logger == loggerName);
            if (data == null)
                TrippismNLog.SaveNLogData(Airportcode, loggerName);
        }

        private string GetFullPath(string path)
        {
            return System.Web.HttpContext.Current.Server.MapPath(path);
        }
    }
}
