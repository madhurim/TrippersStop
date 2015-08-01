/// <summary>
///  This class  forecasts the price range into which the lowest published fare that is available 
///  via the Sabre GDS is predicted to fall within the next 7 days.
/// </summary>



using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
     [ProtoContract]
    public class Forecast
    {
        [ProtoMember(1)]
        public int HighestPredictedFare { get; set; }
        [ProtoMember(2)]
        public string CurrencyCode { get; set; }
        [ProtoMember(3)]
        public int LowestPredictedFare { get; set; }
    }

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
    //TBD: Need to be clean 
    public class FareForecast
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
