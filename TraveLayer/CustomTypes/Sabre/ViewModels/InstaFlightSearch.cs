using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    public class InstaFlightSearch
    {

        public string DepartureDateTime { get; set; }
        public List<PricedItinerary> PricedItineraries { get; set; }
        public string DestinationLocation { get; set; }
        public string ReturnDateTime { get; set; }
        public string OriginLocation { get; set; }
        public List<Link> Links { get; set; }
    }

}
