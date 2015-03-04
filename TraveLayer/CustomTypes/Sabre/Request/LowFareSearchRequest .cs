using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    //https://developer.sabre.com/docs/read/rest_apis/air/intelligence/top_destinations
    public class LowFareSearchRequest 
    {
        public string DepartureDateTime  { get; set; }
        public string DestinationLocation { get; set; }
        public string DestinationType { get; set; }
        public int TopDestinations { get; set; }
        public string DestinationCountry { get; set; }
        public string Theme { get; set; }
        public string Region { get; set; }        
    }
}
