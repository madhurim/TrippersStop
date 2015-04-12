/// <summary>
///  This class retrieves a list of multi-airport city (MAC) codes. A MAC is a city served by multiple major airports.
///  The response is multi-airport cities located in the requested country (or countries), 
///  sorted by city name, in ascending order. If no country is requested, all MAC codes and cities are included in the response.
/// </summary>



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
 
    public class City
    {
        public string code { get; set; }
        public string name { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string regionName { get; set; }
        public List<Link> Links { get; set; }
    }

    public class OTA_MultiAirportCityLookup
    {
        public List<City> Cities { get; set; }
        public List<Link> Links { get; set; }
    }

    //TBD: Need to be clean 
    public class MultiAirportCityLookup
    {
        public OTA_MultiAirportCityLookup OTA_MultiAirportCityLookup { get; set; }
    }

}
