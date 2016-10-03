using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.CurrencyConversion.ViewModels
{
    public class CurrencyConversion
    {
        public string Base { get; set; }
        public string Target{ get; set; }
        public double Rate { get; set; }
    }
}
