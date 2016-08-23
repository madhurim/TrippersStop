using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.CurrencyConversion.Response
{
    public class CurrencyConversionOutput
    {
        public string Base{ get; set; }
        public Dictionary<string, double> rates { get; set; }        
    }
}
