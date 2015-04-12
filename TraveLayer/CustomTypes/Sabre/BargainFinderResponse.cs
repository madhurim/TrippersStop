using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    public class Success
    {
    }

    public class Warning
    {
        public string Type { get; set; }
        public string ShortText { get; set; }
        public string Code { get; set; }
        public string MessageClass { get; set; }
    }

    public class Warnings
    {
        public List<Warning> Warning { get; set; }
    }

    public class DepartureAirport
    {
        public string LocationCode { get; set; }
        public string TerminalID { get; set; }
    }

    public class ArrivalAirport
    {
        public string LocationCode { get; set; }
        public string TerminalID { get; set; }
    }

    public class OperatingAirline
    {
        public string Code { get; set; }
        public string FlightNumber { get; set; }
    }

    public class Equipment
    {
        public string AirEquipType { get; set; }
    }

    public class MarketingAirline
    {
        public string Code { get; set; }
    }

    public class DepartureTimeZone
    {
        public double GMTOffset { get; set; }
    }

    public class ArrivalTimeZone
    {
        public double GMTOffset { get; set; }
    }

    public class OnTimePerformance
    {
        public string Level { get; set; }
    }

    public class ETicket
    {
        public bool Ind { get; set; }
    }

    //public class TPAExtensions
    //{
    //    public ETicket eTicket { get; set; }
    //}

    public class FlightSegment
    {
        public DepartureAirport DepartureAirport { get; set; }
        public ArrivalAirport ArrivalAirport { get; set; }
        public OperatingAirline OperatingAirline { get; set; }
        public List<Equipment> Equipment { get; set; }
        public MarketingAirline MarketingAirline { get; set; }
        public string MarriageGrp { get; set; }
        public DepartureTimeZone DepartureTimeZone { get; set; }
        public ArrivalTimeZone ArrivalTimeZone { get; set; }
        public OnTimePerformance OnTimePerformance { get; set; }
        public TPAExtensions TPA_Extensions { get; set; }
        public string DepartureDateTime { get; set; }
        public string ArrivalDateTime { get; set; }
        public int StopQuantity { get; set; }
        public string FlightNumber { get; set; }
        public string ResBookDesigCode { get; set; }
        public int ElapsedTime { get; set; }
    }

    public class OriginDestinationOption
    {
        public List<FlightSegment> FlightSegment { get; set; }
        public int ElapsedTime { get; set; }
    }

    public class OriginDestinationOptions
    {
        public List<OriginDestinationOption> OriginDestinationOption { get; set; }
    }

    public class AirItinerary
    {
        public OriginDestinationOptions OriginDestinationOptions { get; set; }
        public string DirectionInd { get; set; }
    }

    //public class BaseFare
    //{
    //    public double Amount { get; set; }
    //    public string CurrencyCode { get; set; }
    //    public int DecimalPlaces { get; set; }
    //}

    public class FareConstruction
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPlaces { get; set; }
    }

    //public class EquivFare
    //{
    //    public double Amount { get; set; }
    //    public string CurrencyCode { get; set; }
    //    public int DecimalPlaces { get; set; }
    //}

    //public class Tax
    //{
    //    public string TaxCode { get; set; }
    //    public double Amount { get; set; }
    //    public string CurrencyCode { get; set; }
    //    public int DecimalPlaces { get; set; }
    //}

    //public class Taxes
    //{
    //    public List<Tax> Tax { get; set; }
    //}

    //public class TotalFare
    //{
    //    public double Amount { get; set; }
    //    public string CurrencyCode { get; set; }
    //    public int DecimalPlaces { get; set; }
    //}

    //public class ItinTotalFare
    //{
    //    public BaseFare BaseFare { get; set; }
    //    public FareConstruction FareConstruction { get; set; }
    //    public EquivFare EquivFare { get; set; }
    //    public Taxes Taxes { get; set; }
    //    public TotalFare TotalFare { get; set; }
    //}

    //public class PassengerTypeQuantity
    //{
    //    public string Code { get; set; }
    //    public int Quantity { get; set; }
    //}

    //public class FareBasisCode
    //{
    //    public string content { get; set; }
    //    public string BookingCode { get; set; }
    //    public bool AvailabilityBreak { get; set; }
    //    public string DepartureAirportCode { get; set; }
    //    public string ArrivalAirportCode { get; set; }
    //}

    //public class FareBasisCodes
    //{
    //    public List<FareBasisCode> FareBasisCode { get; set; }
    //}

    public class BaseFare2
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class FareConstruction2
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPlaces { get; set; }
    }

    public class EquivFare2
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPlaces { get; set; }
    }

    //public class Tax2
    //{
    //    public string TaxCode { get; set; }
    //    public double Amount { get; set; }
    //    public string CurrencyCode { get; set; }
    //    public int DecimalPlaces { get; set; }
    //}

    //public class TotalTax
    //{
    //    public double Amount { get; set; }
    //    public string CurrencyCode { get; set; }
    //    public int DecimalPlaces { get; set; }
    //}

    public class Taxes2
    {
        public List<Tax2> Tax { get; set; }
        public TotalTax TotalTax { get; set; }
    }

    public class TotalFare2
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Message
    {
        public string AirlineCode { get; set; }
        public string Type { get; set; }
        public int FailCode { get; set; }
        public string Info { get; set; }
    }

    public class Messages
    {
        public List<Message> Message { get; set; }
    }

    public class Segment
    {
        public int Id { get; set; }
    }

    public class Allowance
    {
        public int Pieces { get; set; }
    }

    public class BaggageInformation
    {
        public List<Segment> Segment { get; set; }
        public Allowance Allowance { get; set; }
    }

    public class BaggageInformationList
    {
        public List<BaggageInformation> BaggageInformation { get; set; }
    }

    public class TPAExtensions2
    {
        public Messages Messages { get; set; }
        public BaggageInformationList BaggageInformationList { get; set; }
    }

    public class PassengerFare
    {
        public BaseFare2 BaseFare { get; set; }
        public FareConstruction2 FareConstruction { get; set; }
        public EquivFare2 EquivFare { get; set; }
        public Taxes2 Taxes { get; set; }
        public TotalFare2 TotalFare { get; set; }
        public TPAExtensions2 TPA_Extensions { get; set; }
    }

    public class Endorsements
    {
        public bool NonRefundableIndicator { get; set; }
    }

    public class FareCalcLine
    {
        public string Info { get; set; }
    }

    public class TPAExtensions3
    {
        public FareCalcLine FareCalcLine { get; set; }
    }

    public class PTCFareBreakdown
    {
        public PassengerTypeQuantity PassengerTypeQuantity { get; set; }
        public FareBasisCodes FareBasisCodes { get; set; }
        public PassengerFare PassengerFare { get; set; }
        public Endorsements Endorsements { get; set; }
        public TPAExtensions3 TPA_Extensions { get; set; }
    }

    public class PTCFareBreakdowns
    {
        public List<PTCFareBreakdown> PTC_FareBreakdown { get; set; }
    }

    public class SeatsRemaining
    {
        public int Number { get; set; }
        public bool BelowMin { get; set; }
    }

    //public class Cabin
    //{
    //    public string Cabin { get; set; }
    //}

    public class TPAExtensions4
    {
        public SeatsRemaining SeatsRemaining { get; set; }
        public string Cabin { get; set; }
    }

    public class FareInfo : FareInfoLeadPriceCalendar
    {
        public string FareReference { get; set; }
        public TPAExtensions4 TPA_Extensions { get; set; }

    }
    public class FareInfoLeadPriceCalendar
    {
        public object LowestFare { get; set; }
        public string CurrencyCode { get; set; }
        public object LowestNonStopFare { get; set; }
        public string DepartureDateTime { get; set; }
        public string ReturnDateTime { get; set; }
        public List<Link> Links { get; set; }
        public string DestinationLocation { get; set; } // Copying from DestinationFinder.
    }
    //public class FareInfos
    //{
    //    public List<FareInfo> FareInfo { get; set; }
    //}

    public class DivideInParty
    {
        public bool Indicator { get; set; }
    }

    public class ValidatingCarrier
    {
        public string Default { get; set; }
        public string SettlementMethod { get; set; }
        public bool NewVcxProcess { get; set; }
    }

    public class TPAExtensions5
    {
        public DivideInParty DivideInParty { get; set; }
        public ValidatingCarrier ValidatingCarrier { get; set; }
    }

    public class AirItineraryPricingInfo
    {
        public ItinTotalFare ItinTotalFare { get; set; }
        public PTCFareBreakdowns PTC_FareBreakdowns { get; set; }
        public List<FareInfo> FareInfo { get; set; }
        public TPAExtensions5 TPA_Extensions { get; set; }
        public string LastTicketDate { get; set; }
        public string PricingSource { get; set; }
        public string PricingSubSource { get; set; }
    }

    public class TicketingInfo
    {
        public string TicketType { get; set; }
        public string ValidInterline { get; set; }
    }

    public class ValidatingCarrier2
    {
        public string Code { get; set; }
    }

    public class TPAExtensions6
    {
        public ValidatingCarrier2 ValidatingCarrier { get; set; }
    }

    public class PricedItinerary
    {
        public AirItinerary AirItinerary { get; set; }
        public List<AirItineraryPricingInfo> AirItineraryPricingInfo { get; set; }
        public TicketingInfo TicketingInfo { get; set; }
        public TPAExtensions6 TPA_Extensions { get; set; }
        public int SequenceNumber { get; set; }
    }

    public class PricedItineraries
    {
        public List<PricedItinerary> PricedItinerary { get; set; }
    }

    public class OTAAirLowFareSearchRS
    {
        public Success Success { get; set; }
        public Warnings Warnings { get; set; }
        public PricedItineraries PricedItineraries { get; set; }
        public int PricedItinCount { get; set; }
        public int BrandedOneWayItinCount { get; set; }
        public int SimpleOneWayItinCount { get; set; }
        public int DepartedItinCount { get; set; }
        public int SoldOutItinCount { get; set; }
        public int AvailableItinCount { get; set; }
        public string Version { get; set; }
    }



    public class RootObject
    {
        public OTAAirLowFareSearchRS OTA_AirLowFareSearchRS { get; set; }
        public List<Link> Links { get; set; }
    }
}
