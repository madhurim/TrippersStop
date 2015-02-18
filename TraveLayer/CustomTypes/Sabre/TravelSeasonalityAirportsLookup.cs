using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    //class TravelSeasonalityAirportsLookup
    //{
    //}


    public class DestinationLocation2
    {
        public string AirportCode { get; set; }
        public string AirportName { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
    }

    //public class DestinationLocation
    //{
    //    public DestinationLocation2 DestinationLocation { get; set; }
    //}

    //public class Link
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    public class OTA_TravelSeasonalityAirportsLookup
    {
        public List<DestinationLocation> DestinationLocations { get; set; }
       // public List<Link> Links { get; set; }
    }

    public class TravelSeasonalityAirportsLookup : ICustomType
    {
        public OTA_TravelSeasonalityAirportsLookup OTA_TravelSeasonalityAirportsLookup { get; set; }
    }
}
