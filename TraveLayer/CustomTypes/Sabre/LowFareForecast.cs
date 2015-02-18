using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    //class LowFareForecast
    //{
    //}


    public class Forecast
    {
        public int HighestPredictedFare { get; set; }
        public string CurrencyCode { get; set; }
        public int LowestPredictedFare { get; set; }
    }

    //public class Link
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    public class OTA_LowFareForecast
    {
        public string OriginLocation { get; set; }
        public string DestinationLocation { get; set; }
        public string DepartureDateTime { get; set; }
        public string ReturnDateTime { get; set; }
        public Forecast Forecast { get; set; }
        public string Recommendation { get; set; }
        public double LowestFare { get; set; }
        public string CurrencyCode { get; set; }
        public List<Link> Links { get; set; }
    }

    public class LowFareForecast : ICustomType
    {
        public OTA_LowFareForecast OTA_LowFareForecast { get; set; }
    }
}


//{
//  "OriginLocation": "ATL",
//  "DestinationLocation": "LAS",
//  "DepartureDateTime": "2013-08-25T00:00:00",
//  "ReturnDateTime": "2013-08-28T00:00:00",
//  "Forecast": {
//    "HighestPredictedFare": 885,
//    "CurrencyCode": "USD",
//    "LowestPredictedFare": 511
//  },
//  "Recommendation": "buy",
//  "LowestFare": 409.1,
//  "CurrencyCode": "USD",
//  "Links": [{
//    "rel": "self",
//    "href": "https://api.sabre.com/v1/forecast/flights/fares?origin=ATL&destination=LAS&departuredate=2013-08-25&returndate=2013-08-28"
//  },
//  {
//    "rel": "linkTemplate",
//    "href": "https://api.sabre.com/v1/forecast/flights/fares?origin=<origin>&destination=<destination>&departuredate=<departuredate>&returndate=<returndate>"
//  },
//  {
//    "rel": "shop",
//    "href": "https://api.sabre.com/v1/shop/flights?origin=ATL&destination=LAS&departuredate=2013-08-25&returndate=2013-08-28&pointofsalecountry=US"
//  }]
//}
