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
        private const string TrippismHighRankedAirportsKey = "Trippism.Constants.HighRankedAirports";
        private const string TrippismHighRankedAirportsCurrencyKey = "Trippism.Constants.HighRankedAirports.Currency";
        private const string TrippismAirlinesKey = "Trippism.Constants.Airlines";
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
            var tripAirports = _cacheService.GetByKey<TraveLayer.CustomTypes.Constants.Response.AirportRoot>(TrippismAirportsKey);
            if (tripAirports != null)
                return Request.CreateResponse(HttpStatusCode.OK, tripAirports.AirportsRoot.AirportsDetail);

            string jsonPath = GetFullPath(ConfigurationManager.AppSettings["AirportsJsonPath"].ToString());
            return await Task.Run(() =>
            { return GetAirportsResponse(jsonPath, TrippismAirportsKey); });
        }

        [Route("api/Constants/GetHighRankedAirports")]
        [ResponseType(typeof(List<TraveLayer.CustomTypes.Constants.Response.AirportsDetail>))]
        public async Task<HttpResponseMessage> GetHighRankedAirports()
        {
            var tripAirports = _cacheService.GetByKey<TraveLayer.CustomTypes.Constants.Response.AirportRoot>(TrippismHighRankedAirportsKey);
            if (tripAirports != null)
                return Request.CreateResponse(HttpStatusCode.OK, tripAirports.AirportsRoot.AirportsDetail);

            string jsonPath = GetFullPath(ConfigurationManager.AppSettings["HighRankedAirportsJsonPath"].ToString());
            return await Task.Run(() =>
            { return GetAirportsResponse(jsonPath, TrippismHighRankedAirportsKey); });
        }

        private HttpResponseMessage GetAirportsResponse(string jsonPath, string cacheKey)
        {
            string airportJsonString = string.Empty;
            using (StreamReader readerAirportJson = new StreamReader(jsonPath))
            {
                airportJsonString = readerAirportJson.ReadToEnd();
            }
            var airports = ServiceStackSerializer.DeSerialize<AirportRoot>(airportJsonString);
            _cacheService.Save<TraveLayer.CustomTypes.Constants.Response.AirportRoot>(cacheKey, airports);
            return Request.CreateResponse(HttpStatusCode.OK, airports.AirportsRoot.AirportsDetail);
        }

        private HttpResponseMessage GetAirportCurrencyResponse(string jsonPath, string cacheKey)
        {
            string airportJsonString = string.Empty;
            using (StreamReader readerAirportJson = new StreamReader(jsonPath))
            {
                airportJsonString = readerAirportJson.ReadToEnd();
            }
            var airports = ServiceStackSerializer.DeSerialize<List<AirportCurrency>>(airportJsonString);
            AirportCurrencyOutput airportCurrencyOutput = new AirportCurrencyOutput();
            airportCurrencyOutput.AirportCurrencies = airports;
            _cacheService.Save<AirportCurrencyOutput>(cacheKey, airportCurrencyOutput);
            return Request.CreateResponse(HttpStatusCode.OK, airportCurrencyOutput);
        }


        [Route("api/Constants/GetHighRankedAirportsCurrency")]
        [ResponseType(typeof(List<TraveLayer.CustomTypes.Constants.Response.AirportCurrency>))]
        public async Task<HttpResponseMessage> GetHighRankedAirportsCurrency()
        {
            var tripAirports = _cacheService.GetByKey<AirportCurrencyOutput>(TrippismHighRankedAirportsCurrencyKey);
            if (tripAirports != null)
                return Request.CreateResponse(HttpStatusCode.OK, tripAirports);

            string jsonPath = GetFullPath(ConfigurationManager.AppSettings["HighRankedAirportsCurrencyJsonPath"].ToString());
            return await Task.Run(() =>
            { return GetAirportCurrencyResponse(jsonPath, TrippismHighRankedAirportsCurrencyKey); });
        }

        [Route("api/Constants/GetAirlines")]
        [ResponseType(typeof(List<TraveLayer.CustomTypes.Constants.Response.Airlines>))]
        public async Task<HttpResponseMessage> GetAirlines()
        {
            var tripAirlines = _cacheService.GetByKey<TraveLayer.CustomTypes.Constants.Response.Airlines>(TrippismAirlinesKey);
            if (tripAirlines != null)
                return Request.CreateResponse(HttpStatusCode.OK, tripAirlines.response);
            string jsonPath = GetFullPath(ConfigurationManager.AppSettings["AirlinesJsonPath"].ToString());
            return await Task.Run(() =>
            { return GetAirlinesResponse(jsonPath); });
        }

        private HttpResponseMessage GetAirlinesResponse(string jsonPath)
        {
            string airlinesJsonString = string.Empty;
            using (StreamReader readerAirlinesJson = new StreamReader(jsonPath))
            {
                airlinesJsonString = readerAirlinesJson.ReadToEnd();
            }
            var airlines = ServiceStackSerializer.DeSerialize<Airlines>(airlinesJsonString);
            _cacheService.Save<TraveLayer.CustomTypes.Constants.Response.Airlines>(TrippismAirlinesKey, airlines);
            return Request.CreateResponse(HttpStatusCode.OK, airlines.response);
        }

        [HttpGet]
        [Route("api/Constants/MissingAirportLog")]
        public async Task MissingAirportLog(string Airportcode)
        {
            await Task.Run(() => { SaveAirportCode(Airportcode); });
        }
        private void SaveAirportCode(string Airportcode)
        {
            string loggerName = "MissingAirportLogger";
            //DefaultLog defaultLog = new DefaultLog();
            //RepositoryDefaultLog objRepositoryDefaultLog = new RepositoryDefaultLog();
            //var data = objRepositoryDefaultLog.FindOne<DefaultLog>(x => x.Message == Airportcode && x.Logger == loggerName);
            //if (data == null)
            TrippismNLog.SaveNLogData(Airportcode, loggerName);
        }

        private string GetFullPath(string path)
        {
            return System.Web.HttpContext.Current.Server.MapPath(path);
        }
    }
}
