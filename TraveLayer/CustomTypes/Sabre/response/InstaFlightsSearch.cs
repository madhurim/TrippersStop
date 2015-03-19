/// <summary>
///  This class retrives  round-trip flight itineraries with published fares and fare breakdowns available from the Sabre cache for a certain origin, destination, and round-trip travel dates.
/// </summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    public class InstaCabin
    {
        public string Cabin { get; set; }
    }

    public class InstaTPAExtensions
    {
        public InstaCabin InstaCabin { get; set; }
    }

    public class InstaFareInfo
    {
        public string FareReference { get; set; }
        public InstaTPAExtensions TPA_Extensions { get; set; }
    }

    public class FareInfos
    {
        public List<FareInfo> InstaFareInfo { get; set; }
    }

    public class Tax
    {
        public double Amount { get; set; }
        public string TaxCode { get; set; }
        public int DecimalPlaces { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Taxes
    {
        public List<Tax> Tax { get; set; }
    }

    public class TotalFare
    {
        public int Amount { get; set; }
        public int DecimalPlaces { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class EquivFare
    {
        public double Amount { get; set; }
        public int DecimalPlaces { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class BaseFare
    {
        public double Amount { get; set; }
        public int DecimalPlaces { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class ItinTotalFare
    {
        public Taxes Taxes { get; set; }
        public TotalFare TotalFare { get; set; }
        public EquivFare EquivFare { get; set; }
        public BaseFare BaseFare { get; set; }
    }

    public class FareBasisCode
    {
        public string content { get; set; }
        public string DepartureAirportCode { get; set; }
        public string BookingCode { get; set; }
        public string ArrivalAirportCode { get; set; }
    }

    public class FareBasisCodes
    {
        public List<FareBasisCode> FareBasisCode { get; set; }
    }
    
    public class TotalTax
    {
        public double Amount { get; set; }
        public int DecimalPlaces { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Tax2
    {
        public double Amount { get; set; }
        public string TaxCode { get; set; }
        public int DecimalPlaces { get; set; }
        public string CurrencyCode { get; set; }
    }

    //public class Taxes2
    //{
    //    public TotalTax TotalTax { get; set; }
    //    public List<Tax2> Tax { get; set; }
    //}

    //public class TotalFare2
    //{
    //    public int Amount { get; set; }
    //    public string CurrencyCode { get; set; }
    //}

    //public class EquivFare2
    //{
    //    public double Amount { get; set; }
    //    public int DecimalPlaces { get; set; }
    //    public string CurrencyCode { get; set; }
    //}

    //public class BaseFare2
    //{
    //    public double Amount { get; set; }
    //    public string CurrencyCode { get; set; }
    //}

    //public class PassengerFare
    //{
    //    public Taxes2 Taxes { get; set; }
    //    public TotalFare2 TotalFare { get; set; }
    //    public EquivFare2 EquivFare { get; set; }
    //    public BaseFare2 BaseFare { get; set; }
    //}

    //public class PTCFareBreakdown
    //{
    //    public FareBasisCodes FareBasisCodes { get; set; }
    //    public PassengerTypeQuantity PassengerTypeQuantity { get; set; }
    //    public PassengerFare PassengerFare { get; set; }
    //}

    //public class PTCFareBreakdowns
    //{
    //    public PTCFareBreakdown PTC_FareBreakdown { get; set; }
    //}

    //public class DivideInParty
    //{
    //    public bool Indicator { get; set; }
    //}

    //public class TPAExtensions2
    //{
    //    public DivideInParty DivideInParty { get; set; }
    //}

    //public class AirItineraryPricingInfo
    //{
    //    public FareInfos FareInfos { get; set; }
    //    public ItinTotalFare ItinTotalFare { get; set; }
    //    public PTCFareBreakdowns PTC_FareBreakdowns { get; set; }
    //    public TPAExtensions2 TPA_Extensions { get; set; }
    //}

    //public class Equipment
    //{
    //    public object AirEquipType { get; set; }
    //}

    //public class ArrivalTimeZone
    //{
    //    public int GMTOffset { get; set; }
    //}

    //public class DepartureAirport
    //{
    //    public string LocationCode { get; set; }
    //}

    //public class DepartureTimeZone
    //{
    //    public int GMTOffset { get; set; }
    //}

    //public class ETicket
    //{
    //    public bool Ind { get; set; }
    //}

    //public class TPAExtensions3
    //{
    //    public ETicket eTicket { get; set; }
    //}

    //public class OperatingAirline
    //{
    //    public int FlightNumber { get; set; }
    //    public string Code { get; set; }
    //}

    //public class ArrivalAirport
    //{
    //    public string LocationCode { get; set; }
    //}

    //public class MarketingAirline
    //{
    //    public string Code { get; set; }
    //}

    //public class FlightSegment
    //{
    //    public Equipment Equipment { get; set; }
    //    public ArrivalTimeZone ArrivalTimeZone { get; set; }
    //    public string ArrivalDateTime { get; set; }
    //    public string ResBookDesigCode { get; set; }
    //    public DepartureAirport DepartureAirport { get; set; }
    //    public DepartureTimeZone DepartureTimeZone { get; set; }
    //    public string MarriageGrp { get; set; }
    //    public TPAExtensions3 TPA_Extensions { get; set; }
    //    public string DepartureDateTime { get; set; }
    //    public int StopQuantity { get; set; }
    //    public int ElapsedTime { get; set; }
    //    public int FlightNumber { get; set; }
    //    public OperatingAirline OperatingAirline { get; set; }
    //    public ArrivalAirport ArrivalAirport { get; set; }
    //    public MarketingAirline MarketingAirline { get; set; }
    //}

    //public class OriginDestinationOption
    //{
    //    public List<FlightSegment> FlightSegment { get; set; }
    //    public int ElapsedTime { get; set; }
    //}

    //public class OriginDestinationOptions
    //{
    //    public List<OriginDestinationOption> OriginDestinationOption { get; set; }
    //}

    //public class AirItinerary
    //{
    //    public OriginDestinationOptions OriginDestinationOptions { get; set; }
    //    public string DirectionInd { get; set; }
    //}

    //public class ValidatingCarrier
    //{
    //    public string Code { get; set; }
    //}

    //public class TPAExtensions4
    //{
    //    public ValidatingCarrier ValidatingCarrier { get; set; }
    //}

    //public class TicketingInfo
    //{
    //    public string TicketType { get; set; }
    //}

    //public class PricedItinerary
    //{
    //    public AirItineraryPricingInfo AirItineraryPricingInfo { get; set; }
    //    public AirItinerary AirItinerary { get; set; }
    //    public int SequenceNumber { get; set; }
    //    public TPAExtensions4 TPA_Extensions { get; set; }
    //    public TicketingInfo TicketingInfo { get; set; }
    //}

    public class OTA_InstaFlightsSearch
    {
        public string DepartureDateTime { get; set; }
        public List<PricedItinerary> PricedItineraries { get; set; }
        public string DestinationLocation { get; set; }
        public string ReturnDateTime { get; set; }
        public string OriginLocation { get; set; }
        public List<Link> Links { get; set; }
    }

    public class InstaFlightsSearch
    {
        public OTA_InstaFlightsSearch OTA_InstaFlightsSearch { get; set; }
    }

}


//{
//    "DepartureDateTime": "2014-07-03",
//    "PricedItineraries": [
//        {
//            "AirItineraryPricingInfo": {
//                "FareInfos": {
//                    "FareInfo": [
//                        {
//                            "FareReference": "X",
//                            "TPA_Extensions": {
//                                "Cabin": {
//                                    "Cabin": "Y"
//                                }
//                            }
//                        },
//                        {
//                            "FareReference": "X",
//                            "TPA_Extensions": {
//                                "Cabin": {
//                                    "Cabin": "Y"
//                                }
//                            }
//                        },
//                        {
//                            "FareReference": "X",
//                            "TPA_Extensions": {
//                                "Cabin": {
//                                    "Cabin": "Y"
//                                }
//                            }
//                        },
//                        {
//                            "FareReference": "X",
//                            "TPA_Extensions": {
//                                "Cabin": {
//                                    "Cabin": "Y"
//                                }
//                            }
//                        }
//                    ]
//                },
//                "ItinTotalFare": {
//                    "Taxes": {
//                        "Tax": [
//                            {
//                                "Amount": 71.5,
//                                "TaxCode": "TOTALTAX",
//                                "DecimalPlaces": 2,
//                                "CurrencyCode": "USD"
//                            }
//                        ]
//                    },
//                    "TotalFare": {
//                        "Amount": 438,
//                        "DecimalPlaces": 2,
//                        "CurrencyCode": "USD"
//                    },
//                    "EquivFare": {
//                        "Amount": 366.5,
//                        "DecimalPlaces": 2,
//                        "CurrencyCode": "USD"
//                    },
//                    "BaseFare": {
//                        "Amount": 347.9,
//                        "DecimalPlaces": 2,
//                        "CurrencyCode": "USD"
//                    }
//                },
//                "PTC_FareBreakdowns": {
//                    "PTC_FareBreakdown": {
//                        "FareBasisCodes": {
//                            "FareBasisCode": [
//                                {
//                                    "content": "XA07A0QZ",
//                                    "DepartureAirportCode": "DFW",
//                                    "BookingCode": "X",
//                                    "ArrivalAirportCode": "ATL"
//                                },
//                                {
//                                    "content": "XA07A0QZ",
//                                    "DepartureAirportCode": "ATL",
//                                    "BookingCode": "X",
//                                    "ArrivalAirportCode": "CLE"
//                                },
//                                {
//                                    "content": "XA07A0QZ",
//                                    "DepartureAirportCode": "CLE",
//                                    "BookingCode": "X",
//                                    "ArrivalAirportCode": "ATL"
//                                },
//                                {
//                                    "content": "XA07A0QZ",
//                                    "DepartureAirportCode": "ATL",
//                                    "BookingCode": "X",
//                                    "ArrivalAirportCode": "DFW"
//                                }
//                            ]
//                        },
//                        "PassengerTypeQuantity": {
//                            "Quantity": 1,
//                            "Code": "ADT"
//                        },
//                        "PassengerFare": {
//                            "Taxes": {
//                                "TotalTax": {
//                                    "Amount": 71.5,
//                                    "DecimalPlaces": 2,
//                                    "CurrencyCode": "USD"
//                                },
//                                "Tax": [
//                                    {
//                                        "Amount": 27.5,
//                                        "TaxCode": "US1",
//                                        "DecimalPlaces": 2,
//                                        "CurrencyCode": "USD"
//                                    },
//                                    {
//                                        "Amount": 16,
//                                        "TaxCode": "ZP",
//                                        "DecimalPlaces": 2,
//                                        "CurrencyCode": "USD"
//                                    },
//                                    {
//                                        "Amount": 10,
//                                        "TaxCode": "AY",
//                                        "DecimalPlaces": 2,
//                                        "CurrencyCode": "USD"
//                                    },
//                                    {
//                                        "Amount": 18,
//                                        "TaxCode": "XF",
//                                        "DecimalPlaces": 2,
//                                        "CurrencyCode": "USD"
//                                    }
//                                ]
//                            },
//                            "TotalFare": {
//                                "Amount": 438,
//                                "CurrencyCode": "USD"
//                            },
//                            "EquivFare": {
//                                "Amount": 366.5,
//                                "DecimalPlaces": 2,
//                                "CurrencyCode": "USD"
//                            },
//                            "BaseFare": {
//                                "Amount": 347.9,
//                                "CurrencyCode": "USD"
//                            }
//                        }
//                    }
//                },
//                "TPA_Extensions": {
//                    "DivideInParty": {
//                        "Indicator": false
//                    }
//                }
//            },
//            "AirItinerary": {
//                "OriginDestinationOptions": {
//                    "OriginDestinationOption": [
//                        {
//                            "FlightSegment": [
//                                {
//                                    "Equipment": {
//                                        "AirEquipType": "M88"
//                                    },
//                                    "ArrivalTimeZone": {
//                                        "GMTOffset": -4
//                                    },
//                                    "ArrivalDateTime": "2014-07-03T08:55:00",
//                                    "ResBookDesigCode": "X",
//                                    "DepartureAirport": {
//                                        "LocationCode": "DFW"
//                                    },
//                                    "DepartureTimeZone": {
//                                        "GMTOffset": -5
//                                    },
//                                    "MarriageGrp": "O",
//                                    "TPA_Extensions": {
//                                        "eTicket": {
//                                            "Ind": false
//                                        }
//                                    },
//                                    "DepartureDateTime": "2014-07-03T05:45:00",
//                                    "StopQuantity": 0,
//                                    "ElapsedTime": 130,
//                                    "FlightNumber": 1890,
//                                    "OperatingAirline": {
//                                        "FlightNumber": 1890,
//                                        "Code": "DL"
//                                    },
//                                    "ArrivalAirport": {
//                                        "LocationCode": "ATL"
//                                    },
//                                    "MarketingAirline": {
//                                        "Code": "DL"
//                                    }
//                                },
//                                {
//                                    "Equipment": {
//                                        "AirEquipType": 717
//                                    },
//                                    "ArrivalTimeZone": {
//                                        "GMTOffset": -4
//                                    },
//                                    "ArrivalDateTime": "2014-07-03T13:52:00",
//                                    "ResBookDesigCode": "X",
//                                    "DepartureAirport": {
//                                        "LocationCode": "ATL"
//                                    },
//                                    "DepartureTimeZone": {
//                                        "GMTOffset": -4
//                                    },
//                                    "MarriageGrp": "I",
//                                    "TPA_Extensions": {
//                                        "eTicket": {
//                                            "Ind": false
//                                        }
//                                    },
//                                    "DepartureDateTime": "2014-07-03T12:05:00",
//                                    "StopQuantity": 0,
//                                    "ElapsedTime": 107,
//                                    "FlightNumber": 2528,
//                                    "OperatingAirline": {
//                                        "FlightNumber": 2528,
//                                        "Code": "DL"
//                                    },
//                                    "ArrivalAirport": {
//                                        "LocationCode": "CLE"
//                                    },
//                                    "MarketingAirline": {
//                                        "Code": "DL"
//                                    }
//                                }
//                            ],
//                            "ElapsedTime": 427
//                        },
//                        {
//                            "FlightSegment": [
//                                {
//                                    "Equipment": {
//                                        "AirEquipType": 717
//                                    },
//                                    "ArrivalTimeZone": {
//                                        "GMTOffset": -4
//                                    },
//                                    "ArrivalDateTime": "2014-07-12T10:07:00",
//                                    "ResBookDesigCode": "X",
//                                    "DepartureAirport": {
//                                        "LocationCode": "CLE"
//                                    },
//                                    "DepartureTimeZone": {
//                                        "GMTOffset": -4
//                                    },
//                                    "MarriageGrp": "O",
//                                    "TPA_Extensions": {
//                                        "eTicket": {
//                                            "Ind": false
//                                        }
//                                    },
//                                    "DepartureDateTime": "2014-07-12T08:10:00",
//                                    "StopQuantity": 0,
//                                    "ElapsedTime": 117,
//                                    "FlightNumber": 1755,
//                                    "OperatingAirline": {
//                                        "FlightNumber": 1755,
//                                        "Code": "DL"
//                                    },
//                                    "ArrivalAirport": {
//                                        "LocationCode": "ATL"
//                                    },
//                                    "MarketingAirline": {
//                                        "Code": "DL"
//                                    }
//                                },
//                                {
//                                    "Equipment": {
//                                        "AirEquipType": "M88"
//                                    },
//                                    "ArrivalTimeZone": {
//                                        "GMTOffset": -5
//                                    },
//                                    "ArrivalDateTime": "2014-07-12T15:10:00",
//                                    "ResBookDesigCode": "X",
//                                    "DepartureAirport": {
//                                        "LocationCode": "ATL"
//                                    },
//                                    "DepartureTimeZone": {
//                                        "GMTOffset": -4
//                                    },
//                                    "MarriageGrp": "I",
//                                    "TPA_Extensions": {
//                                        "eTicket": {
//                                            "Ind": false
//                                        }
//                                    },
//                                    "DepartureDateTime": "2014-07-12T13:50:00",
//                                    "StopQuantity": 0,
//                                    "ElapsedTime": 140,
//                                    "FlightNumber": 1910,
//                                    "OperatingAirline": {
//                                        "FlightNumber": 1910,
//                                        "Code": "DL"
//                                    },
//                                    "ArrivalAirport": {
//                                        "LocationCode": "DFW"
//                                    },
//                                    "MarketingAirline": {
//                                        "Code": "DL"
//                                    }
//                                }
//                            ],
//                            "ElapsedTime": 480
//                        }
//                    ]
//                },
//                "DirectionInd": "Return"
//            },
//            "SequenceNumber": 1,
//            "TPA_Extensions": {
//                "ValidatingCarrier": {
//                    "Code": "DL"
//                }
//            },
//            "TicketingInfo": {
//                "TicketType": "eTicket"
//            }
//        }
//    ],
//    "DestinationLocation": "CLE",
//    "ReturnDateTime": "2014-07-12",
//    "OriginLocation": "DFW",
//    "Links": [
//        {
//            "rel": "nextResults",
//            "href": "https://api.sabre.com/v1/shop/flights?origin=DFW&destination=CLE&departuredate=2014-07-03&returndate=2014-07-12"
//        },
//        {
//            "rel": "self",
//            "href": "https://api.sabre.com/v1/shop/flights?origin=DFW&destination=CLE&departuredate=2014-07-03&returndate=2014-07-12"
//        },
//        {
//            "rel": "linkTemplate",
//            "href": "https://api.sabre.com/v1/shop/flights?origin=<origin>&destination=<destination>&departuredate=<departuredate>&returndate=<returndate>&offset=<offset>&limit=<limit>&sortby=<sortby>&order=<order>&sortby2=<sortby2>&order2=<order2>&minfare=<minfare>&maxfare=<maxfare>&includedcarriers=<includedcarriers>&excludedcarriers=<excludedcarriers>&outboundflightstops=<outboundflightstops>&inboundflightstops=<inboundflightstops>&outboundstopduration=<outboundstopduration>&inboundstopduration=<inboundstopduration>&outbounddeparturewindow=<outbounddeparturewindow>&outboundarrivalwindow=<outboundarrivalwindow>&inbounddeparturewindow=<inbounddeparturewindow>&inboundarrivalwindow=<inboundarrivalwindow>&onlineitinerariesonly=<onlineitinerariesonly>&eticketsonly=<eticketsonly>&<includedconnectpoints>=<includedconnectpoints>&excludedconnectpoints=<excludedconnectpoints>&pointofsalecountry=<pointofsalecountry>&passengercount=<passengercount>"
//        }
//    ]
//}
