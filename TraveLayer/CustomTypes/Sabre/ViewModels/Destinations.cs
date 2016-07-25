using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    /// <summary>
    /// Sabre DestinationFinder Request
    /// </summary> 

    public class Destinations : TravelInfo
    {
        public string Earliestdeparturedate { get; set; }
        public string Latestdeparturedate { get; set; }
        public string Lengthofstay { get; set; }
        public string Theme { get; set; }
        public string Location { get; set; }
        public string Minfare { get; set; }
        public string Maxfare { get; set; }
        public string PointOfSaleCountry { get; set; }
        public string Region { get; set; }
        public string TopDestinations { get; set; }
        public Destinations ShallowCopy()
        {
            return (Destinations)this.MemberwiseClone();
        }
    }
}
