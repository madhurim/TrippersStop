/// <summary>
///  This class retrieves a list of destination airport codes that are associated with the theme in the request.
///  A theme is similar to a travel category, and is based on geography, points of interest, or recreational activities
/// </summary>



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    public class Destination
    {
        public string Destinations { get; set; }
        public string Type { get; set; }
        public List<Link> Links { get; set; }

        // Come from Top destinations
      

          //public string DestinationLocation{ get; set; }
          //public string  CountryCode{ get; set; }
          //public string   CountryName{ get; set; }
          //public string  RegionName{ get; set; }
          //public string   MetropolitanAreaName{ get; set; }

    }

    public class OTA_ThemeAirportLookup
    {
        public List<Destination> Destinations { get; set; }
        public List<Link> Links { get; set; }
    }
    //TBD: Need to be clean 
    public class ThemeAirportLookup
    {
        public OTA_ThemeAirportLookup OTA_ThemeAirportLookup { get; set; }
    }

}
