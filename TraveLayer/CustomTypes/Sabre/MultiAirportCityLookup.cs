using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    //class MultiAirportCityLookup
    //{
    //}

    //public class Link
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    public class City
    {
        public string code { get; set; }
        public string name { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string regionName { get; set; }
        public List<Link> Links { get; set; }
    }

    //public class Link2
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    public class OTA_MultiAirportCityLookup
    {
        public List<City> Cities { get; set; }
        public List<Link> Links { get; set; }
    }

    public class MultiAirportCityLookup : ICustomType
    {
        public OTA_MultiAirportCityLookup OTA_MultiAirportCityLookup { get; set; }
    }

}
