﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    public class FareData
    {
        public double MaximumFare { get; set; }
        public double MinimumFare { get; set; }
        public double MedianFare { get; set; }
        public string CurrencyCode { get; set; }
        public string Count { get; set; }
        public string DepartureDateTime { get; set; }
        public string ReturnDateTime { get; set; }
        public List<Link> Links { get; set; }
    }

   
    public class OTA_FareRange
    {
        public string OriginLocation { get; set; }
        public string DestinationLocation { get; set; }
        public List<FareData> FareData { get; set; }
        public List<Link2> Links { get; set; }
    }

    public class FareRange
    {
        public OTA_FareRange OTA_FareRange { get; set; }
    }

}
