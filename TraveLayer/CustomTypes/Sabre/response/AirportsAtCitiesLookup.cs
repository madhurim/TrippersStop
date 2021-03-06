
﻿/// <summary>
///  This class retrieves our list of major airport, rail station and other codes that are associated with 
///  a single multi-airport city (MAC) code in the request.
/// </summary>


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

    //TBD: Need to be clean 
    public class AirportsAtCitiesLookup
    {
        public OTA_AirportsAtCitiesLookup OTA_AirportsAtCitiesLookup { get; set; }
    }
}
