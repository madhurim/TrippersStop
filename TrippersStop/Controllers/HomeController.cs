using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json.Linq;
using ServiceStack.Text;

namespace TrippismApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public void GenerateAirports()
        {
            var _jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DateFormatHandling = DateFormatHandling.IsoDateFormat };

            StreamReader _readerairport = new StreamReader(Server.MapPath("~/iataairports.json"));
            string _airportjson = _readerairport.ReadToEnd();
            var _allAirports = JsonConvert.DeserializeObject<Airports>(_airportjson, _jsonSettings);


            StreamReader _readercountries = new StreamReader(Server.MapPath("~/countries.json"));
            string _countriesjson = _readercountries.ReadToEnd();
            var _allcountries = JsonConvert.DeserializeObject<Countries>(_countriesjson, _jsonSettings);

            StreamReader _readercities = new StreamReader(Server.MapPath("~/cities.json"));
            string _citiesjson = _readercities.ReadToEnd();
            var _allcities = JsonConvert.DeserializeObject<Cities>(_citiesjson, _jsonSettings);

            List<CityWithAirports> _objCitywithairports = new List<CityWithAirports>();

            //To be uncomment later
            //var requiredCities = new string[] {"PAR", "LON", "NYC", "QSF", "MOW", "CHI", "SHA", "BOM"};
            //foreach (var grps in _allcities.response.Where(i=> requiredCities.Contains(i.code)).GroupBy(i => i.code))
            foreach (var grps in _allcities.response.GroupBy(i => i.code))
            {
                var airportsinCity = _allAirports.response.Find(x => x.city_code == grps.Key);
                CityWithAirports _obj = new CityWithAirports();
                _obj.code = grps.Key;
                _obj.countryCode = grps.First().country_code;
                _obj.countryName = _allcountries.response.Find(x => x.code == grps.First().country_code).name;
                _obj.regionName = grps.First().timezone;
                _obj.name = grps.First().name;

                List<AirportInfo> airportinfo = new List<AirportInfo>();

                var airportsinthecity = _allAirports.response.Where(x => x.city_code == grps.Key).ToList();
                if (airportsinthecity.Any())
                {
                    for (int i = 0; i < airportsinthecity.Count; i++)
                    {
                        AirportInfo ainfo = new AirportInfo();
                        ainfo.code = airportsinthecity[i].code;
                        ainfo.fullname = airportsinthecity[i].name;
                        ainfo.lat = airportsinthecity[i].lat;
                        ainfo.lng = airportsinthecity[i].lng;
                        airportinfo.Add(ainfo);
                    }
                    _obj.Airports = airportinfo;
                }
                _objCitywithairports.Add(_obj);
            }

            string json = JsonConvert.SerializeObject(_objCitywithairports, Formatting.Indented);
            var path = Server.MapPath(@"~/airports.json");
            System.IO.File.WriteAllText(path, json);

            //var result = from p in _allcities.response
            //             group p by p.code into grps
            //             select new CityWithAirports
            //             {
            //                 code = grps.Key,
            //                 countryCode = grps.First().country_code,
            //                 countryName = _allcountries.response.Find(x => x.code == grps.First().country_code).name,
            //                 regionName = grps.First().timezone,
            //                 name = grps.First().name,
            //                 //Airports = new AirportInfo() {
            //                 //    code = _allcountries.response.Find(x => x.code == grps.First().country_code).name,
            //                 //}
            //             };

        }



        private class Airports
        {
            public List<Airport> response { get; set; }
        }
        public class Airport
        {
            [JsonProperty("code")]
            public string code { get; set; }

            [JsonProperty("city_code")]
            public string city_code { get; set; }

            [JsonProperty("country_code")]
            public string country_code { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("alternatenames")]
            public string alternatenames { get; set; }

            [JsonProperty("lat")]
            public string lat { get; set; }

            [JsonProperty("lng")]
            public string lng { get; set; }

            [JsonProperty("timezone")]
            public string timezone { get; set; }

            [JsonProperty("gmt")]
            public string gmt { get; set; }

            [JsonProperty("is_rail_road")]
            public string is_rail_road { get; set; }

            [JsonProperty("is_bus_station")]
            public string is_bus_station { get; set; }

            [JsonProperty("icao")]
            public string icao { get; set; }

        }
        private class Cities
        {
            public List<City> response { get; set; }
        }
        private class City
        {
            public string code { get; set; }
            public string country_code { get; set; }
            public string name { get; set; }
            public string alternatenames { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public string timezone { get; set; }
            public string gmt { get; set; }
        }
        private class Countries
        {
            public List<Country> response { get; set; }
        }
        private class Country
        {
            public string code { get; set; }
            public string code3 { get; set; }
            public string iso_numeric { get; set; }
            public string name { get; set; }
            public string alternatenames { get; set; }
            public string currency_code { get; set; }
            public string currency_name { get; set; }
            public string continent { get; set; }
            public string[] languages { get; set; }
            public string fips_code { get; set; }
            public string population { get; set; }
        }

        private class CityWithAirports
        {
            public string code { get; set; }
            public string name { get; set; }
            public string countryCode { get; set; }
            public string countryName { get; set; }
            public string regionName { get; set; }
            public List<AirportInfo> Airports { get; set; }
        }

        private class AirportInfo
        {


            public string code { get; set; }
            public string fullname { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
        }




    }
}
