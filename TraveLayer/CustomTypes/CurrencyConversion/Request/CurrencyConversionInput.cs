﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.CurrencyConversion.Request
{
    public class CurrencyConversionInput
    {
        public string CurrencyCode { get; set; }
        public string ExchangeCurrencyCode { get; set; }
    }
}
