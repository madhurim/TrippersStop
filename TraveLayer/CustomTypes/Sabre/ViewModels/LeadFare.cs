using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    public class LeadFare
    {
        public string OriginLocation { get; set; }
        public string DestinationLocation { get; set; }
        public string CurrencyCode { get; set; }
        public List<Price> Price { get; set; }
    }
}
