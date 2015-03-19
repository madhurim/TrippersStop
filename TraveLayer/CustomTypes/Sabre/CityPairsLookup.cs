using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
   
    public class OriginLocation2
    {
        public string AirportCode { get; set; }
        public string AirportName { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
    }

   
    public class OriginDestinationLocation
    {
        public string OriginDestinationLocations { get; set; }
        public OriginLocation OriginLocation { get; set; }
        public DestinationLocation DestinationLocation { get; set; }
    }

    
    public class OTA_CityPairsLookup
    {
        public List<OriginDestinationLocation> OriginDestinationLocations { get; set; }
        public List<Link> Links { get; set; }
    }

    //public class CityPairsLookup
    //{
    //    public OTA_CityPairsLookup OTA_CityPairsLookup { get; set; }
    //}
}
