using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    public class Airport
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class RailStation
    {
        public string code { get; set; }
        public string name { get; set; }
    }

   
    public class OTA_AirportsAtCitiesLookup
    {
        public List<Airport> Airports { get; set; }
        public List<RailStation> __invalid_name__Railstations { get; set; }
        public List<object> Others { get; set; }
        public List<Link> Links { get; set; }
    }
    //public class AirportsAtCitiesLookup
    //{
    //    public OTA_AirportsAtCitiesLookup OTA_AirportsAtCitiesLookup { get; set; }
    //}
}
