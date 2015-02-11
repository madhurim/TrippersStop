using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{

    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class FareInfo
    {
        public object LowestFare { get; set; }
        public string CurrencyCode { get; set; }
        public object LowestNonStopFare { get; set; }
        public string DepartureDateTime { get; set; }
        public string ReturnDateTime { get; set; }
        public List<Link> Links { get; set; }
    }

    public class Link2
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class LeadPriceCalendar
    {
        public string OriginLocation { get; set; }
        public string DestinationLocation { get; set; }
        public List<FareInfo> FareInfo { get; set; }
        public List<Link2> Links { get; set; }
    }
}

//    {
//    "OriginLocation": "ATL",
//    "DestinationLocation": "LAS",
//    "FareInfo": [
//        {
//            "LowestFare": "342.0",
//            "CurrencyCode": "USD",
//            "LowestNonStopFare": "N/A",
//            "DepartureDateTime": "2013-02-05",
//            "ReturnDateTime": "2013-02-08",
//            "Links": [
//                {
//                    "rel": "shop",
//                    "href": "https://api.sabre.com/v1/shop/flights?origin=ATL&destination=LAS&departuredate=2013-02-05&returndate=2013-02-08&offset=<offset>&limit=<limit>&sortby=<sortby>&order=<order>&sortby2=<sortby2>&order2=<order2>"
//                }
//            ]
//        },
//        {
//            "LowestFare": 347.6,
//            "CurrencyCode": "USD",
//            "LowestNonStopFare": 349.8,
//            "DepartureDateTime": "2013-02-06",
//            "ReturnDateTime": "2013-02-09",
//            "Links": [
//                {
//                    "rel": "shop",
//                    "href": "https://api.sabre.com/v1/shop/flights?origin=ATL&destination=LAS&departuredate=2013-02-10&returndate=2013-02-13&pointofsalecountry=US"
//                }
//            ]
//        },
//        {
//            "LowestFare": "N/A",
//            "CurrencyCode": "N/A",
//            "LowestNonStopFare": "N/A",
//            "DepartureDateTime": "2013-08-07",
//            "ReturnDateTime": "2013-08-10",
//            "Links": [
//                {
//                    "rel": "shop",
//                    "href": "https://api.sabre.com/v1/shop/flights?origin=ATL&destination=LAS&departuredate=2013-08-02&returndate=2013-08-05&pointofsalecountry=US"
//                }
//            ]
//        }
//    ],
//    "Links": [
//        {
//            "rel": "self",
//            "href": "https://api.sabre.com/v1/shop/flights/fares?origin=ATL&destination=LAS&lengthofstay=3"
//        },
//        {
//            "rel": "linkTemplate",
//            "href": "https://api.sabre.com/v1/shop/flights/fares?origin=<origin>&destination=<destination>&lengthofstay=<lengthofstay>&departuredate=<departuredate>&minfare=<minfare>&maxfare=<maxfare>"
//        }
//    ]
//}
    

