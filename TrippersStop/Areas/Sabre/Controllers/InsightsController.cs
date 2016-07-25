using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippismApi.TraveLayer;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using System.Configuration;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Weather;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using ExpressMapper;
using Trippism.APIExtention.Filters;
using BusinessLogic;

namespace TrippismApi.Areas.Sabre.Controllers
{
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class InsightsController : ApiController
    {
        public string SabreDestinationsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreDestinationsUrl"];
            }
        }
        public string SabreFareForecastUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreFareForecastUrl"];
            }
        }
        public string SabreFareRangeUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreFareRangeUrl"];
            }
        }
        public string SabreSeasonalityUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSeasonalityUrl"];
            }
        }

        public string WeatherHistoryUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WeatherHistoryUrl"];
            }
        }
        public string SabreCountriesUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSaleCountryUrl"];
            }
        }

        // GET api/destination
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/destination/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/destination
        public void Post([FromBody]string value)
        {
        }

        // PUT api/destination/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/destination/5
        public void Delete(int id)
        {
        }
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        /// 

        IAsyncSabreAPICaller _apiCaller;
        IAsyncWeatherAPICaller _weatherApiCaller;
        ICacheService _cacheService;
        IBusinessLayer<Trip, TripWeather> _weatherBusinessLayer;

        public InsightsController(IAsyncSabreAPICaller apiCaller, IAsyncWeatherAPICaller weatherApiCaller, ICacheService cacheService, IBusinessLayer<Trip, TripWeather> weatherBusinessLayer)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
            _weatherApiCaller = weatherApiCaller;
            _weatherBusinessLayer = weatherBusinessLayer;
        }
        /// <summary>
        /// Filters the response for destinations in the country or countries you specify
        /// </summary>
        [Route("api/sabre/destination/insights")]
        [HttpGet]
        public async Task<HttpResponseMessage> Insights([FromUri]TripInput tripInput)
        {
            string lengthOfStay = (Convert.ToDateTime(tripInput.ReturnDate) - Convert.ToDateTime(tripInput.DepartureDate)).Days.ToString();
            //  string destinationUrl = string.Format(SabreDestinationsUrl + "?origin={0}&departuredate={1}&returndate={2}&lengthofstay={3}", tripInput.Origin, tripInput.DepartureDate, tripInput.ReturnDate, lengthOfStay);
            string fareForecast = string.Format(SabreFareForecastUrl + "?origin={0}&destination={1}&departuredate={2}&returndate={3}", tripInput.Origin, tripInput.Destination, tripInput.DepartureDate, tripInput.ReturnDate);
            string fareRange = string.Format(SabreFareRangeUrl + "?origin={0}&destination={1}&earliestdeparturedate={2}&latestdeparturedate={3}&lengthofstay={4}", tripInput.Origin, tripInput.Destination, tripInput.DepartureDate, tripInput.ReturnDate, lengthOfStay);
            string seasonality = string.Format(SabreSeasonalityUrl, tripInput.Destination);

            string fromDate = Convert.ToDateTime(tripInput.DepartureDate).ToString("MMdd");
            string toDate = Convert.ToDateTime(tripInput.ReturnDate).ToString("MMdd");

            string weather = string.Format(WeatherHistoryUrl, fromDate, toDate, tripInput.State, tripInput.City);
            return await Task.Run(() =>
             { return GetResponse(fareForecast, fareRange, seasonality, weather); });

        }

        /// <summary>
        /// Fet farerange and fare forcast based on dates and destinations
        /// </summary>
        [Route("api/sabre/destination/insights/fares")]
        [HttpGet]
        public async Task<HttpResponseMessage> Fares([FromUri]TravelInfo tripInput)
        {
            string lengthOfStay = (Convert.ToDateTime(tripInput.ReturnDate) - Convert.ToDateTime(tripInput.DepartureDate)).Days.ToString();
            string fareForecast = string.Format(SabreFareForecastUrl + "?origin={0}&destination={1}&departuredate={2}&returndate={3}", tripInput.Origin, tripInput.Destination, tripInput.DepartureDate, tripInput.ReturnDate);
            string fareRange = string.Format(SabreFareRangeUrl + "?origin={0}&destination={1}&earliestdeparturedate={2}&latestdeparturedate={3}&lengthofstay={4}", tripInput.Origin, tripInput.Destination, tripInput.DepartureDate, tripInput.ReturnDate, lengthOfStay);
            return await Task.Run(() =>
            { return GetFareResponse(fareForecast, fareRange); });

        }
        public HttpResponseMessage GetFareResponse(string fareForecast, string fareRange)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            var responses = Task.WhenAll(
            new[]
            {
              fareForecast,
              fareRange
            }.Select(url => _apiCaller.Get(url)));
            var result = responses.Result;
            FareOutput fareOutput = new FareOutput();
            if (result[0].StatusCode == HttpStatusCode.OK)
                fareOutput.LowFareForecast = GetFareForecastResponse(result[0].Response);
            if (result[1].StatusCode == HttpStatusCode.OK)
                fareOutput.FareRange = GetFareRangeResponse(result[1].Response);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fareOutput);
            return response;
        }



        public HttpResponseMessage GetResponse(string fareForecast, string fareRange, string seasonality, string weather)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            var responses = Task.WhenAll(
            new[]
            {
              weather,
              fareForecast,
              fareRange,
              seasonality
            }.Select(url => _apiCaller.Get(url)));
            var result = responses.Result;

            TripOutput tripOutput = new TripOutput();


            if (result[0].StatusCode == HttpStatusCode.OK)
                tripOutput.TripWeather = GetWeatherResponse(result[0].Response);
            if (result[1].StatusCode == HttpStatusCode.OK)
                tripOutput.LowFareForecast = GetFareForecastResponse(result[1].Response);
            if (result[2].StatusCode == HttpStatusCode.OK)
                tripOutput.FareRange = GetFareRangeResponse(result[2].Response);
            if (result[3].StatusCode == HttpStatusCode.OK)
                tripOutput.TravelSeasonality = GetTravelSeasonalityResponse(result[3].Response);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tripOutput);

            return response;
        }

        private VM.TravelSeasonality GetTravelSeasonalityResponse(string jsonResponse)
        {

            OTA_TravelSeasonality seasonality = new OTA_TravelSeasonality();
            seasonality = ServiceStackSerializer.DeSerialize<OTA_TravelSeasonality>(jsonResponse);
            VM.TravelSeasonality travelSeasonality = Mapper.Map<OTA_TravelSeasonality, VM.TravelSeasonality>(seasonality);
            return travelSeasonality;
        }

        public LowFareForecast GetFareForecastResponse(string jsonResponse)
        {
            OTA_LowFareForecast fares = new OTA_LowFareForecast();
            fares = ServiceStackSerializer.DeSerialize<OTA_LowFareForecast>(jsonResponse);

            LowFareForecast lowFareForecast = Mapper.Map<OTA_LowFareForecast, LowFareForecast>(fares);
            return lowFareForecast;
        }

        private VM.FareRange GetFareRangeResponse(string jsonResponse)
        {

            OTA_FareRange fares = new OTA_FareRange();
            fares = ServiceStackSerializer.DeSerialize<OTA_FareRange>(jsonResponse);
            VM.FareRange fareRange = Mapper.Map<OTA_FareRange, VM.FareRange>(fares);
            return fareRange;
        }


        private TripWeather GetWeatherResponse(string weatherResp)
        {
            HistoryOutput weather = new HistoryOutput();
            weather = ServiceStackSerializer.DeSerialize<HistoryOutput>(weatherResp);
            if (weather != null && weather.trip != null)
            {
                Trip trip = weather.trip;
                TripWeather tripWeather = Mapper.Map<Trip, TripWeather>(trip);
                tripWeather.WeatherChances = new List<WeatherChance>();
                if (trip.chance_of != null)
                {
                    TripWeather chances = _weatherBusinessLayer.Process(trip);
                    tripWeather.WeatherChances = chances.WeatherChances;
                }
                return tripWeather;
            }
            return null;
        }
    }


}

