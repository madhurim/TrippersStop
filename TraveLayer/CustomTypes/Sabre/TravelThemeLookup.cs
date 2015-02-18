using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    //class TravelThemeLookup
    //{
    //}

    //public class Link
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    public class Theme
    {
        public string Themes { get; set; }
    //    public List<Link> Links { get; set; }
    }

    //public class Link2
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    public class OTA_TravelThemeLookup
    {
        public List<Theme> Themes { get; set; }
       // public List<Link2> Links { get; set; }
    }

    public class TravelThemeLookup : ICustomType
    {
        public OTA_TravelThemeLookup OTA_TravelThemeLookup { get; set; }
    }
}
