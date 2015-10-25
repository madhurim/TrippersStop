/// <summary>
///  This class retrives  round-trip flight itineraries with published fares and fare breakdowns available from the Sabre cache for a certain origin, destination, and round-trip travel dates.
/// </summary>
using System.Collections.Generic;
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

        //Come from Bargain Finder reponse
        public List<FareInfo> FareInfo { get; set; }
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
        public double Amount { get; set; }
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
    public class InstaFlightsSearchOutput
    {
        public string DepartureDateTime { get; set; }
        public List<PricedItinerary> PricedItineraries { get; set; }
        public string DestinationLocation { get; set; }
        public string ReturnDateTime { get; set; }
        public string OriginLocation { get; set; }
        public List<Link> Links { get; set; }
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

    //public class OTA_InstaFlightsSearch
    //{
    //    public string DepartureDateTime { get; set; }
    //    public List<PricedItinerary> PricedItineraries { get; set; }
    //    public string DestinationLocation { get; set; }
    //    public string ReturnDateTime { get; set; }
    //    public string OriginLocation { get; set; }
    //    public List<Link> Links { get; set; }
    //}


}

