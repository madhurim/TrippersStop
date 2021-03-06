﻿/// <summary>
///  This class  retrieves current nonstop lead fare and overall lead fare available on future calendar dates
/// </summary>



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

    public class LowestFare
    {
        public List<string> AirlineCodes { get; set; }
        public double Fare { get; set; }
    }
    public class LowestNonStopFare
    {
        public List<string> AirlineCodes { get; set; }
        public double Fare { get; set; }
    }
    // Move to bargain finder response
    public class FareInfo
    {
        public LowestFare LowestFare { get; set; }
        public string CurrencyCode { get; set; }
        public LowestNonStopFare LowestNonStopFare { get; set; }
        public string DepartureDateTime { get; set; }
        public string ReturnDateTime { get; set; }
        //public List<Link> Links { get; set; }
        public string DestinationLocation { get; set; }

        // Copying from BF REsponse.
        public string FareReference { get; set; }
        public TPAExtensions4 TPA_Extensions { get; set; }
    }
    
    public class LeadPriceCalendarOutput
    {
        public string OriginLocation { get; set; }
        public string DestinationLocation { get; set; }
        public List<FareInfo> FareInfo { get; set; }
       
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


