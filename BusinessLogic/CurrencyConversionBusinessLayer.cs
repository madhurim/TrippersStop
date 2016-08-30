using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.CurrencyConversion.Response;
using TraveLayer.CustomTypes.CurrencyConversion.ViewModels;

namespace BusinessLogic
{
    public class CurrencyConversionBusinessLayer : IBusinessLayer<CurrencyConversionOutput, CurrencyConversion>
    {
        public CurrencyConversion Process(CurrencyConversionOutput currencyConversionOutput)
        {
            CurrencyConversion currencyConversion = new CurrencyConversion();

            currencyConversion.Base = currencyConversionOutput.Base;
            currencyConversion.Rate = currencyConversionOutput.rates.Where(x => x.Key.Equals(currencyConversionOutput.Target)).Select(y => y.Value).FirstOrDefault();
            currencyConversion.Target = currencyConversionOutput.Target;
            return currencyConversion;
        }
    }
}
