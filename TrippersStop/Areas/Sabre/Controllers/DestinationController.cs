﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippismApi.TraveLayer;
using System.Text;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Configuration;
using System.Web.Http.Description;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Weather;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using Trippism.APIHelper;
using ExpressMapper;
using Trippism.APIExtention.Filters;
using NLog;

namespace TrippismApi.Areas.Sabre.Controllers
{

    public class DestinationController : ApiController
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
        public DestinationController(IAsyncSabreAPICaller apiCaller, IAsyncWeatherAPICaller weatherApiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
            _weatherApiCaller = weatherApiCaller;
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

        /// <summary>
        /// Filters the response for destinations in the country or countries you specify
        /// </summary>
        [Route("api/sabre/destinations/insights/seasonality")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetSeasonalWeather([FromUri]TripInput tripInput)
        {
            string seasonality = string.Format(SabreSeasonalityUrl, tripInput.Destination);
            string fromDate = Convert.ToDateTime(tripInput.DepartureDate).ToString("MMdd");
            string toDate = Convert.ToDateTime(tripInput.ReturnDate).ToString("MMdd");
            string weather = string.Format(WeatherHistoryUrl, fromDate, toDate, tripInput.State, tripInput.City);
            return await Task.Run(() =>
             { return GetResponse(seasonality, weather); });
        }

       
        private HttpResponseMessage GetFareResponse(string fareForecast, string fareRange)
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

        /// <summary>
        /// Format url based on request.
        /// </summary>
        private string GetURL(Destinations destinationsRequest)
        {
            StringBuilder url = new StringBuilder();
            url.Append(SabreDestinationsUrl + "?");
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Origin))
            {
                url.Append("origin=" + destinationsRequest.Origin);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Destination))
            {
                url.Append("&destination=" + destinationsRequest.Destination);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.DepartureDate))
            {
                url.Append("&departuredate=" + destinationsRequest.DepartureDate);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.ReturnDate))
            {
                url.Append("&returndate=" + destinationsRequest.ReturnDate);
            }

            if (!string.IsNullOrWhiteSpace(destinationsRequest.Earliestdeparturedate))
            {
                url.Append("&earliestdeparturedate=" + destinationsRequest.Earliestdeparturedate);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Latestdeparturedate))
            {
                url.Append("&latestdeparturedate=" + destinationsRequest.Latestdeparturedate);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Lengthofstay))
            {
                url.Append("&lengthofstay=" + destinationsRequest.Lengthofstay);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Location))
            {
                url.Append("&location=" + destinationsRequest.Location);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Maxfare))
            {
                url.Append("&maxfare=" + destinationsRequest.Maxfare);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Minfare))
            {
                url.Append("&minfare=" + destinationsRequest.Minfare);
            }

            if (!string.IsNullOrWhiteSpace(destinationsRequest.PointOfSaleCountry))
            {
                url.Append("&pointofsalecountry=" + destinationsRequest.PointOfSaleCountry);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Region))
            {
                url.Append("&region=" + destinationsRequest.Region);
            }

            if (!string.IsNullOrWhiteSpace(destinationsRequest.Theme))
            {
                url.Append("&theme=" + destinationsRequest.Theme);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.TopDestinations))
            {
                url.Append("&topdestinations=" + destinationsRequest.TopDestinations);
            }
            return url.ToString();
        }

        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url, int count = 0)
        {
            //TrippismNLog.SaveNLogData(url);
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            string posResponse = "Parameter 'pointofsalecountry' has an unsupported value";
            if (result.StatusCode == HttpStatusCode.BadRequest && result.Response.ToString() == posResponse)
            {
                APIResponse supportedPOSCountries = GetAPIResponse(SabreCountriesUrl);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, supportedPOSCountries.Response);
                return response;
            }
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_DestinationFinder cities = new OTA_DestinationFinder();
                cities = ServiceStackSerializer.DeSerialize<OTA_DestinationFinder>(result.Response);
                Fares fares = Mapper.Map<OTA_DestinationFinder, Fares>(cities);
                if (count != 0)
                {
                    fares.FareInfo = fares.FareInfo.OrderBy(f => f.LowestFare.Fare).Take(count).ToList();
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fares);
                //TrippismNLog.SaveNLogData(result.Response);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
        private APIResponse GetAPIResponse(string url)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            return result;
        }
        private HttpResponseMessage GetResponse(string seasonality, string weather)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            var responses = Task.WhenAll(
            new[]
            {
              seasonality
            }.Select(url => _apiCaller.Get(url)));
            var result = responses.Result;
            SeasonalityOutput seasonalityOutput = new SeasonalityOutput();
            seasonalityOutput.TripWeather = GetWeatherResponse(weather);
            if (result[0].StatusCode == HttpStatusCode.OK)
                seasonalityOutput.TravelSeasonality = GetTravelSeasonalityResponse(result[0].Response);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, seasonalityOutput);
            return response;
        }

        private HttpResponseMessage GetResponse(string fareForecast, string fareRange, string seasonality, string weather)
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
            if (result[0].StatusCode == HttpStatusCode.OK)
                tripOutput.LowFareForecast = GetFareForecastResponse(result[1].Response);
            if (result[1].StatusCode == HttpStatusCode.OK)
                tripOutput.FareRange = GetFareRangeResponse(result[2].Response);
            if (result[2].StatusCode == HttpStatusCode.OK)
                tripOutput.TravelSeasonality = GetTravelSeasonalityResponse(result[3].Response);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tripOutput);

            return response;
        }

        private VM.TravelSeasonality GetTravelSeasonalityResponse(string jsonResponse)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            OTA_TravelSeasonality seasonality = new OTA_TravelSeasonality();
            seasonality = ServiceStackSerializer.DeSerialize<OTA_TravelSeasonality>(jsonResponse);
            watch.Stop();
            //TripperLog.LogMethodTime("TravelSeasonality Response-DeSerialize ", watch.ElapsedMilliseconds);
            watch = System.Diagnostics.Stopwatch.StartNew();
            //Mapper.CreateMap<OTA_TravelSeasonality, VM.TravelSeasonality>();
            VM.TravelSeasonality travelSeasonality = Mapper.Map<OTA_TravelSeasonality, VM.TravelSeasonality>(seasonality);
            watch.Stop();
            //TripperLog.LogMethodTime("TravelSeasonality Response-Mapping ", watch.ElapsedMilliseconds);
            return travelSeasonality;
        }

        private LowFareForecast GetFareForecastResponse(string jsonResponse)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            OTA_LowFareForecast fares = new OTA_LowFareForecast();
            fares = ServiceStackSerializer.DeSerialize<OTA_LowFareForecast>(jsonResponse);
            watch.Stop();
            //TripperLog.LogMethodTime("FareForecast Response-DeSerialize ", watch.ElapsedMilliseconds);
            watch = System.Diagnostics.Stopwatch.StartNew();

            LowFareForecast lowFareForecast = Mapper.Map<OTA_LowFareForecast, LowFareForecast>(fares);
            watch.Stop();
            //TripperLog.LogMethodTime("FareForecast Response-Mapping ", watch.ElapsedMilliseconds);
            return lowFareForecast;
        }

        private VM.FareRange GetFareRangeResponse(string jsonResponse)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            OTA_FareRange fares = new OTA_FareRange();
            fares = ServiceStackSerializer.DeSerialize<OTA_FareRange>(jsonResponse);
            watch.Stop();
            //TripperLog.LogMethodTime("FareRange Response-DeSerialize ", watch.ElapsedMilliseconds);
            watch = System.Diagnostics.Stopwatch.StartNew();
            VM.FareRange fareRange = Mapper.Map<OTA_FareRange, VM.FareRange>(fares);
            watch.Stop();
            //TripperLog.LogMethodTime("FareRange Response-Mapping ", watch.ElapsedMilliseconds);
            return fareRange;
        }

        private Fares GetDestinationResponse(string jsonResponse)
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            OTA_DestinationFinder cities = new OTA_DestinationFinder();
            cities = ServiceStackSerializer.DeSerialize<OTA_DestinationFinder>(jsonResponse);
           // watch.Stop();
            //TripperLog.LogMethodTime("Destination Response-DeSerialize ", watch.ElapsedMilliseconds);
          //  watch = System.Diagnostics.Stopwatch.StartNew();
            Fares fares = Mapper.Map<OTA_DestinationFinder, Fares>(cities);
          //  watch.Stop();
            //TripperLog.LogMethodTime("Destination Response-Mapping ", watch.ElapsedMilliseconds);
            return fares;
        }

        private TripWeather GetWeatherResponse(string weatherResp)
        {
          //  _weatherApiCaller.Accept = "application/json";
          //  _weatherApiCaller.ContentType = "application/json";
          //  APIResponse result = _weatherApiCaller.Get(weatherUrl).Result;
          //  if (result.StatusCode == HttpStatusCode.OK)
          //  {
           //     var watch = System.Diagnostics.Stopwatch.StartNew();
                HistoryOutput weather = new HistoryOutput();
               // watch = System.Diagnostics.Stopwatch.StartNew();
                weather = ServiceStackSerializer.DeSerialize<HistoryOutput>(weatherResp);
               // watch.Stop();
                //TripperLog.LogMethodTime("GetWeatherResponse-DeSerialize ", watch.ElapsedMilliseconds);
                //watch = System.Diagnostics.Stopwatch.StartNew();
                TripWeather tripWeather = Mapper.Map<Trip, TripWeather>(weather.trip);
                tripWeather.WeatherChances = new List<WeatherChance>();
                if (weather != null && weather.trip != null && weather.trip.chance_of != null)
                {
                    ApiHelper.FilterChanceRecord(weather.trip, tripWeather);
                }
               // watch.Stop();
                //TripperLog.LogMethodTime("GetWeatherResponse-Mapping ", watch.ElapsedMilliseconds);
                return tripWeather;
        //    }
            return null;
        }
    }


}
