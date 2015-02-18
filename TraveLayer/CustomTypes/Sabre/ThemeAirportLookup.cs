using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    //class ThemeAirportLookup
    //{
    //}


    //public class Link
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    public class Destination
    {
        public string Destinations { get; set; }
        public string Type { get; set; }
        public List<Link> Links { get; set; }
    }

    //public class Link2
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    public class OTA_ThemeAirportLookup
    {
        public List<Destination> Destinations { get; set; }
       // public List<Link2> Links { get; set; }
    }

    public class ThemeAirportLookup : ICustomType
    {
        public OTA_ThemeAirportLookup OTA_ThemeAirportLookup { get; set; }
    }

}
