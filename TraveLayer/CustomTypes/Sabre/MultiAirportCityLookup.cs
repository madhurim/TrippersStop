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

    //public class MultiAirportCityLookup
    //{
    //    public OTA_MultiAirportCityLookup OTA_MultiAirportCityLookup { get; set; }
    //}

}
