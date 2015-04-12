/// <summary>
///  This class retrieves a list of themes. A theme is similar to a travel category, 
///  and is based on geography, points of interest, and recreational activities
/// </summary>



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
  
    public class Theme
    {
        public string Themes { get; set; }
        public List<Link> Links { get; set; }
    }

    public class OTA_TravelThemeLookup
    {
        public List<Theme> Themes { get; set; }
        public List<Link> Links { get; set; }
    }
    //TBD: Need to be clean 
    public class TravelThemeLookup
    {
        public OTA_TravelThemeLookup OTA_TravelThemeLookup { get; set; }
    }
}
