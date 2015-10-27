using System.Collections.Generic;

namespace TraveLayer.CustomTypes.Constants.Response
{
    public class CurrencySymbols
    {
        public Currencies Currencies { get; set; }
    }
    public class Currency
    {
        public string symbol { get; set; }
        public string code { get; set; }
    }

    public class Currencies
    {
        public List<Currency> Currency { get; set; }
    }
}
