using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModel
{
    public class TravelSeasonality
    {
        public string DestinationLocation { get; set; }
        public List<Seasonality> Seasonality { get; set; }
    }
}
