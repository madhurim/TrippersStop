using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Weather;
using TrippismApi.TraveLayer;
using System.Linq;
using System.Configuration;
namespace Trippism.Areas.Weather.StaticMethods
{
    public static class WeatherAPIResponse
    {
        public static IAsyncWeatherAPICaller _apiCaller { get; set; }
        public static ICacheService _cacheService { get; set; }
        public static string WeatherHistoryUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WeatherHistoryUrl"];
            }
        }
        public static string WeatherInternationalHistoryUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WeatherInternationalHistoryUrl"];
            }
        }
        public static string WeatherInternationalMultiCityHistoryUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WeatherInternationalMultiCityHistoryUrl"];
            }
        }
        public static async Task<APIResponse> GetWeatherAPIResult(string fromDate, string toDate, string stateOrCountryCode, string city)
        {
            List<string> urls = new List<string>();
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";

            city = city.Replace("é", "e");
            urls.Add(string.Format(WeatherHistoryUrl, fromDate, toDate, stateOrCountryCode, city));
            urls.Add(string.Format(WeatherHistoryUrl, fromDate, toDate, stateOrCountryCode, city.Replace(" ", "_")));
            urls.Add(string.Format(WeatherHistoryUrl, fromDate, toDate, stateOrCountryCode, city.Replace(" ", string.Empty)));

            foreach (string item in urls)
            {
                APIResponse result = await Task.Run(() => _apiCaller.Get(item).Result);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    // See if its returning multiple city list
                    InternationalHistoryOutput internationalWeather = ServiceStackSerializer.DeSerialize<InternationalHistoryOutput>(result.Response);
                    if (internationalWeather != null && internationalWeather.response.results != null && internationalWeather.response.results.Any())
                    {
                        // multi city respose
                        string cityCode = internationalWeather.response.results.Where(x => x.country == stateOrCountryCode || x.country_iso3166 == stateOrCountryCode).Select(x => x.l).FirstOrDefault();
                        // if city code is found then make new API call or break loop.
                        if (!string.IsNullOrEmpty(cityCode))
                        {
                            string url = string.Format(WeatherInternationalMultiCityHistoryUrl, fromDate, toDate, cityCode);
                            result = await Task.Run(() => _apiCaller.Get(url).Result);
                            HistoryOutput weather = new HistoryOutput();
                            weather = ServiceStackSerializer.DeSerialize<HistoryOutput>(result.Response);
                            if (weather != null && weather.trip != null)
                                return result;
                        }
                        break;
                    }
                    else
                    {
                        // See if response contains weather data (if not then continue loop).
                        HistoryOutput weather = new HistoryOutput();
                        weather = ServiceStackSerializer.DeSerialize<HistoryOutput>(result.Response);
                        if (weather != null && weather.trip != null)
                            return result;
                    }
                }
            }
            return new APIResponse() { StatusCode = HttpStatusCode.NoContent };
        }
    }
}