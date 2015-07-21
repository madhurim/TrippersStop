using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippismApi.TraveLayer;
using ServiceStack;
using System.Reflection;
using System.Text;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using AutoMapper;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Configuration;
using System.Web.Http.Description;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Weather;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using JetBrains.Profiler.Windows;
using Trippism.APIHelper;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// Return the current nonstop lead fare and an overall lead fare available to destinations
    /// </summary>
    public class DestinationsController : ApiController
    {

        IAsyncSabreAPICaller _apiCaller;
        IAsyncWeatherAPICaller _weatherApiCaller;
        ICacheService _cacheService;
        const string _destinationKey = "TrippismApi.Destinations.All";
        string _expireTime = ConfigurationManager.AppSettings["RedisExpireInMin"].ToString();

        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public DestinationsController(IAsyncSabreAPICaller apiCaller, IAsyncWeatherAPICaller weatherApiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
            _weatherApiCaller = weatherApiCaller;
        }

        // GET api/DestinationFinder
        /// <summary>
        /// Return the current nonstop lead fare and an overall lead fare available to destinations from a specific origin on roundtrip travel dates 
        /// </summary>
        /// <param name="destinationsRequest">
        /// Return record based on destinations Request Type</param>
        [ResponseType(typeof(Fares))]
        public HttpResponseMessage Get([FromUri]Destinations destinationsRequest)
        {
            string url = GetURL(destinationsRequest);
            return GetResponse(url);
        }

        // GET api/DestinationFinder
        /// <summary>
        /// Filters the response for destination airport codes associated with a travel theme
        /// </summary>
        [Route("api/destinations/theme/{theme}")]
        [Route("api/sabre/destinations/theme/{theme}")]
        [HttpGet]
        [ResponseType(typeof(Fares))]
        public HttpResponseMessage GetDestinationsByTheme(string theme, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&lengthofstay={3}&theme={4}", origin, departuredate, returndate, lengthofstay, theme);
            return GetResponse(url);
        }
        // GET api/DestinationFinder
        /// <summary>
        /// Return the cheapest detinations 
        /// </summary>
        [Route("api/destinations/cheapest/{count}")]
        [Route("api/sabre/destinations/cheapest/{count}")]
        [HttpGet]
        [ResponseType(typeof(Fares))]
        public HttpResponseMessage GetTopCheapestDestinations(int count, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&lengthofstay={3}", origin, departuredate, returndate, lengthofstay);
            return GetResponse(url, count);
        }
        // GET api/DestinationFinder
        /// <summary>
        /// Filters the response for overall lead fares that are equal to or less than the maximum, and returns the value in LowestFare
        /// </summary>
        [Route("api/destinations/maxfare/{maxfare}")]
        [Route("api/sabre/destinations/maxfare/{maxfare}")]
        [HttpGet]
        [ResponseType(typeof(Fares))]
        public HttpResponseMessage GetDestinationsByMaxFare(double maxfare, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&maxfare={3}&lengthofstay={4}", origin, departuredate, returndate, maxfare, lengthofstay);
            return GetResponse(url);
        }
        // GET api/DestinationFinder
        /// <summary>
        /// Filters the response for destinations in the country or countries you specify
        /// </summary>
        [Route("api/sabre/destinations/country/{country}")]
        [HttpGet]
        [ResponseType(typeof(Fares))]
        public HttpResponseMessage GetDestinationsByCountry(string country, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&maxfare={3}&lengthofstay={4}&location={4}", origin, departuredate, returndate, lengthofstay, country);
            return GetResponse(url);
        }



        /// <summary>
        /// Filters the response for destinations in the country or countries you specify
        /// </summary>
        [Route("api/sabre/destinations/insights")]
        [HttpGet]
        public HttpResponseMessage Insights([FromUri]TripInput tripInput)
        {
            string lengthOfStay = (Convert.ToDateTime(tripInput.ReturnDate) - Convert.ToDateTime(tripInput.DepartureDate)).Days.ToString();
            string destinationUrl = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&lengthofstay={3}", tripInput.Origin, tripInput.DepartureDate, tripInput.ReturnDate, lengthOfStay);
            string fareForecast = string.Format("v1/forecast/flights/fares?origin={0}&destination={1}&departuredate={2}&returndate={3}", tripInput.Origin, tripInput.Destination, tripInput.DepartureDate, tripInput.ReturnDate);
            string fareRange = string.Format("v1/historical/flights/fares?origin={0}&destination={1}&earliestdeparturedate={2}&latestdeparturedate={3}&lengthofstay={4}", tripInput.Origin, tripInput.Destination, tripInput.DepartureDate, tripInput.ReturnDate, lengthOfStay);
            string seasonality = string.Format("v1/historical/flights/{0}/seasonality", tripInput.Destination);

            string fromDate = Convert.ToDateTime(tripInput.DepartureDate).ToString("MMdd");
            string toDate = Convert.ToDateTime(tripInput.ReturnDate).ToString("MMdd");

            string weather = string.Format("planner_{0}{1}/q/{2}/{3}.json", fromDate, toDate, tripInput.State, tripInput.City);
            return GetResponse(destinationUrl, fareForecast, fareRange, seasonality, weather);
        }



        /// <summary>
        /// Format url based on request.
        /// </summary>
        private string GetURL(Destinations destinationsRequest)
        {
            StringBuilder url = new StringBuilder();
            url.Append("v1/shop/flights/fares?");
            //url.Append("v1/shop/flights?");
            Type type = destinationsRequest.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string propertyName = string.Empty;
            string propertyValue = string.Empty;
            string separator = string.Empty;
            foreach (PropertyInfo property in properties)
            {
                propertyName = property.Name;
                var result = property.GetValue(destinationsRequest, null);
                if (result != null)
                {
                    propertyValue = result.ToString();
                    if (!string.IsNullOrWhiteSpace(propertyValue))
                    {
                        url.Append(separator);
                        url.Append(string.Join("=", propertyName.ToLower(), propertyValue));
                        //url.Append(string.Join("=", propertyName, propertyValue));
                        separator = "&";
                    }
                }
            }
            return url.ToString();
        }
        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url, int count = 0)
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
                OTA_DestinationFinder cities = new OTA_DestinationFinder();
                cities = ServiceStackSerializer.DeSerialize<OTA_DestinationFinder>(result.Response);
                Mapper.CreateMap<OTA_DestinationFinder, Fares>();
                Fares fares = Mapper.Map<OTA_DestinationFinder, Fares>(cities);
                if (count != 0)
                {
                    fares.FareInfo = fares.FareInfo.OrderBy(f => f.LowestFare).Take(count).ToList();
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fares);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }

        private  HttpResponseMessage GetResponse(string destinationUrl, string fareForecast, string fareRange, string seasonality,string weather)
        {
            SabreApiTokenHelper.SetApiToken(_apiCaller, _cacheService);
            var responses =  Task.WhenAll(
            new[]
            {
              destinationUrl,
              fareForecast,
              fareRange,
              seasonality
            }.Select(url => _apiCaller.Get(url)));
            var result = responses.Result;
            //if (PerformanceProfiler.IsActive)
            //{
            //    PerformanceProfiler.Begin();
            //    PerformanceProfiler.Start();
            //}

            TripOutput tripOutput = new TripOutput();
            tripOutput.TripWeather=GetWeatherResponse(weather);
            if(result[0].StatusCode==HttpStatusCode.OK )
            tripOutput.Fares = GetDestinationResponse(result[0].Response);
            if (result[1].StatusCode == HttpStatusCode.OK)
                tripOutput.LowFareForecast = GetFareForecastResponse(result[1].Response);
            if (result[2].StatusCode == HttpStatusCode.OK)
                tripOutput.FareRange = GetFareRangeResponse(result[2].Response);
            if (result[3].StatusCode == HttpStatusCode.OK)
                tripOutput.TravelSeasonality = GetTravelSeasonalityResponse(result[3].Response);
            //if (PerformanceProfiler.IsActive)
            //{
            //    PerformanceProfiler.Stop();
            //    PerformanceProfiler.EndSave();
            //}
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tripOutput);
            
            return response;
        }

        private VM.TravelSeasonality GetTravelSeasonalityResponse(string jsonResponse)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            OTA_TravelSeasonality seasonality = new OTA_TravelSeasonality();
            seasonality = ServiceStackSerializer.DeSerialize<OTA_TravelSeasonality>(jsonResponse);
            watch.Stop();
            TripperLog.LogMethodTime("TravelSeasonality Response-DeSerialize ", watch.ElapsedMilliseconds);
            watch = System.Diagnostics.Stopwatch.StartNew();
            Mapper.CreateMap<OTA_TravelSeasonality, VM.TravelSeasonality>();
            VM.TravelSeasonality travelSeasonality = Mapper.Map<OTA_TravelSeasonality, VM.TravelSeasonality>(seasonality);
            watch.Stop();
            TripperLog.LogMethodTime("TravelSeasonality Response-Mapping ", watch.ElapsedMilliseconds);
            return travelSeasonality;
        }

        private LowFareForecast GetFareForecastResponse(string jsonResponse)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            OTA_LowFareForecast fares = new OTA_LowFareForecast();
            fares = ServiceStackSerializer.DeSerialize<OTA_LowFareForecast>(jsonResponse);
            watch.Stop();
            TripperLog.LogMethodTime("FareForecast Response-DeSerialize ", watch.ElapsedMilliseconds);
            watch = System.Diagnostics.Stopwatch.StartNew();
            Mapper.CreateMap<OTA_LowFareForecast, LowFareForecast>();
            LowFareForecast lowFareForecast = Mapper.Map<OTA_LowFareForecast, LowFareForecast>(fares);
            watch.Stop();
            TripperLog.LogMethodTime("FareForecast Response-Mapping ", watch.ElapsedMilliseconds);
            return lowFareForecast;
        }

        private VM.FareRange GetFareRangeResponse(string jsonResponse)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            OTA_FareRange fares = new OTA_FareRange();
            fares = ServiceStackSerializer.DeSerialize<OTA_FareRange>(jsonResponse);
            watch.Stop();
            TripperLog.LogMethodTime("FareRange Response-DeSerialize ", watch.ElapsedMilliseconds);
            watch = System.Diagnostics.Stopwatch.StartNew();
            Mapper.CreateMap<OTA_FareRange, VM.FareRange>();
            VM.FareRange fareRange = Mapper.Map<OTA_FareRange, VM.FareRange>(fares);
            watch.Stop();
            TripperLog.LogMethodTime("FareRange Response-Mapping ", watch.ElapsedMilliseconds);
            return fareRange;
        }

        private Fares GetDestinationResponse(string jsonResponse)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            OTA_DestinationFinder cities = new OTA_DestinationFinder();     
            cities = ServiceStackSerializer.DeSerialize<OTA_DestinationFinder>(jsonResponse);
            watch.Stop();
            TripperLog.LogMethodTime("Destination Response-DeSerialize ", watch.ElapsedMilliseconds);
            watch = System.Diagnostics.Stopwatch.StartNew();
            Mapper.CreateMap<OTA_DestinationFinder, Fares>();
            Fares fares = Mapper.Map<OTA_DestinationFinder, Fares>(cities);
            watch.Stop();
            TripperLog.LogMethodTime("Destination Response-Mapping ", watch.ElapsedMilliseconds);
            return fares;
        }

        private TripWeather GetWeatherResponse(string weatherUrl)
        {
            _weatherApiCaller.Accept = "application/json";
            _weatherApiCaller.ContentType = "application/json";
            APIResponse result = _weatherApiCaller.Get(weatherUrl).Result;
            //watch.Stop();
            //TripperLog.LogMethodTime("GetWeatherResponse-API Call ", watch.ElapsedMilliseconds);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                HistoryOutput weather = new HistoryOutput();
                watch = System.Diagnostics.Stopwatch.StartNew();
                weather = ServiceStackSerializer.DeSerialize<HistoryOutput>(result.Response);
                watch.Stop();
                TripperLog.LogMethodTime("GetWeatherResponse-DeSerialize ", watch.ElapsedMilliseconds);
                watch = System.Diagnostics.Stopwatch.StartNew();
                Trip trip = weather.trip;
                Mapper.CreateMap<Trip, TripWeather>()
                   .ForMember(h => h.TempHighAvg, m => m.MapFrom(s => s.temp_high))
                   .ForMember(h => h.TempLowAvg, m => m.MapFrom(s => s.temp_low))
                   .ForMember(h => h.ChanceOf, m => m.MapFrom(s => s.chance_of))
                   .ForMember(h => h.CloudCover, m => m.MapFrom(s => s.cloud_cover));
                Mapper.CreateMap<TempHigh, TempHighAvg>()
                    .ForMember(h => h.Avg, m => m.MapFrom(s => s.avg));
                Mapper.CreateMap<TempLow, TempLowAvg>()
                   .ForMember(h => h.Avg, m => m.MapFrom(s => s.avg));
                TripWeather tripWeather = Mapper.Map<Trip, TripWeather>(trip);
                watch.Stop();
                TripperLog.LogMethodTime("GetWeatherResponse-Mapping ", watch.ElapsedMilliseconds);
                return tripWeather;
            }
            return null;
        }


    }
}