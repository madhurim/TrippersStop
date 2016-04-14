using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.SabreSoap.ViewModels
{
    public class Hotel
    {
        public string Star { get; set; }
        public decimal MinimumPriceAvg { get; set; }
        public decimal MaximumPriceAvg { get; set; }
        public string CurrencyCode { get; set; }
    }
}
