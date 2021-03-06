﻿/// <summary>
///  This class  retrieves current nonstop lead fare and an overall lead fare available to destinations from a specific origin on round-trip travel dates from the Sabre cache.
/// </summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre  // All the properties same as LeadPriceCalendar.
{
   
    public class OTA_DestinationFinder
    {
        public string OriginLocation { get; set; }
        public List<FareInfo> FareInfo { get; set; }
        public List<Link> Links { get; set; }
    }

    public class DestinationFinder
    {
        public OTA_DestinationFinder OTA_DestinationFinder { get; set; }
    }
 
}
