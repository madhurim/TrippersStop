using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    //class AlternateDate
    //{
    //}
    //public class DestinationLocation
    //{
    //    public string LocationCode { get; set; }
    //}

    //public class OriginLocation
    //{
    //    public string LocationCode { get; set; }
    //}

    //public class OriginDestinationInformation
    //{
    //    public string DepartureDateTime { get; set; }
    //    public DestinationLocation DestinationLocation { get; set; }
    //    public OriginLocation OriginLocation { get; set; }
    //    public int RPH { get; set; }
    //}

    //public class CompanyName
    //{
    //    public string Code { get; set; }
    //}

    //public class RequestorID
    //{
    //    public CompanyName CompanyName { get; set; }
    //    public string ID { get; set; }
    //    public string Type { get; set; }
    //}

    //public class Source
    //{
    //    public RequestorID RequestorID { get; set; }
    //}

    //public class POS
    //{
    //    public List<Source> Source { get; set; }
    //}

    //public class RequestType
    //{
    //    public string Name { get; set; }
    //}

    //public class IntelliSellTransaction
    //{
    //    public RequestType RequestType { get; set; }
    //}

    //public class TPAExtensions
    //{
    //    public IntelliSellTransaction IntelliSellTransaction { get; set; }
    //}

    //public class PassengerTypeQuantity
    //{
    //    public string Code { get; set; }
    //    public int Quantity { get; set; }
    //}

    //public class AirTravelerAvail
    //{
    //    public List<PassengerTypeQuantity> PassengerTypeQuantity { get; set; }
    //}

    //public class TravelerInfoSummary
    //{
    //    public List<AirTravelerAvail> AirTravelerAvail { get; set; }
    //}

    public class OTAAirLowFareSearchRQ
    {
        public List<OriginDestinationInformation> OriginDestinationInformation { get; set; }
        public POS POS { get; set; }
        public TPAExtensions TPA_Extensions { get; set; }
        public TravelerInfoSummary TravelerInfoSummary { get; set; }
        public TravelPreferences TravelPreferences { get; set; }
    }

    public class OTA_AlternateDate
    {
        public OTAAirLowFareSearchRQ OTA_AirLowFareSearchRQ { get; set; }
    }

    public class AlternateDate : ICustomType
    {
        public OTA_AlternateDate OTA_AlternateDate { get; set; }
    }


}



//{
//    "OTA_AirLowFareSearchRQ": {
//        "OriginDestinationInformation": [
//            {
//                "DepartureDateTime": "2014-11-11T00:00:00",
//                "DestinationLocation": {
//                    "LocationCode": "LAX"
//                },
//                "OriginLocation": {
//                    "LocationCode": "DFW"
//                },
//                "RPH": 1
//            },
//            {
//                "DepartureDateTime": "2014-11-12T00:00:00",
//                "DestinationLocation": {
//                    "LocationCode": "DFW"
//                },
//                "OriginLocation": {
//                    "LocationCode": "LAX"
//                },
//                "RPH": 2
//            }
//        ],
//        "POS": {
//            "Source": [
//                {
//                    "RequestorID": {
//                        "CompanyName": {
//                            "Code": "TN"
//                        },
//                        "ID": "REQ.ID",
//                        "Type": "0.AAA.X"
//                    }
//                }
//            ]
//        },
//        "TPA_Extensions": {
//            "IntelliSellTransaction": {
//                "RequestType": {
//                    "Name": "AD1"
//                }
//            }
//        },
//        "TravelerInfoSummary": {
//            "AirTravelerAvail": [
//                {
//                    "PassengerTypeQuantity": [
//                        {
//                            "Code": "ADT",
//                            "Quantity": 1
//                        }
//                    ]
//                }
//            ]
//        }
//    }
//}