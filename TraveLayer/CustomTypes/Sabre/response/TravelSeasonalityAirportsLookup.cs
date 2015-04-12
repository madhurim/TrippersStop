/// <summary>
///  This class retrieves airports that we support as a destination airport. 
/// </summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    public class DestinationLocation2
    {
        public string AirportCode { get; set; }
        public string AirportName { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
    }
   
    public class OTA_TravelSeasonalityAirportsLookup
    {
        public List<DestinationLocation> DestinationLocations { get; set; }
        public List<Link> Links { get; set; }
    }
    //TBD: Need to be clean 
    public class TravelSeasonalityAirportsLookup
    {
        public OTA_TravelSeasonalityAirportsLookup OTA_TravelSeasonalityAirportsLookup { get; set; }
    }
}
