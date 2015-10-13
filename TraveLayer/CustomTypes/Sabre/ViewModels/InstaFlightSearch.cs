using System.Collections.Generic;
namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    //public class InstaFlightSearch
    //{
    //public string DepartureDateTime { get; set; }
    //public List<PricedItinerary> PricedItineraries { get; set; }
    //public string DestinationLocation { get; set; }
    //public string ReturnDateTime { get; set; }
    //public string OriginLocation { get; set; }
    //public List<Link> Links { get; set; }
    //}

    public class InstaFlightSearch
    {
        public string DepartureDateTime { get; set; }
        public List<PricedItineraryViewModel> PricedItineraries { get; set; }
        public string DestinationLocation { get; set; }
        public string ReturnDateTime { get; set; }
        public string OriginLocation { get; set; }
    }

    public class PricedItineraryViewModel
    {
        //public AirItineraryViewModel AirItinerary { get; set; }
        public List<OriginDestinationOptionViewModel> OriginDestinationOption { get; set; }
        public List<AirItineraryPricingInfoViewModel> AirItineraryPricingInfo { get; set; }
        //public TotalFare TotalFare { get; set; }
    }

    public class AirItineraryViewModel
    {
        public OriginDestinationOptionsViewModel OriginDestinationOptions { get; set; }
        public string DirectionInd { get; set; }
    }

    public class OriginDestinationOptionsViewModel
    {
        public List<OriginDestinationOptionViewModel> OriginDestinationOption { get; set; }
    }

    public class OriginDestinationOptionViewModel
    {
        public List<FlightSegmentViewModel> FlightSegment { get; set; }
        public int ElapsedTime { get; set; }
    }

    public class AirItineraryPricingInfoViewModel
    {
        //public ItinTotalFareViewModel ItinTotalFare { get; set; }
        public TotalFare TotalFare { get; set; }
        //public PTCFareBreakdownsViewModel PTC_FareBreakdowns { get; set; }
        //public List<PTCFareBreakdownViewModel> PTC_FareBreakdown { get; set; }
        ////public FareInfosViewModel FareInfos { get; set; }
        //public List<FareInfoViewModel> FareInfo { get; set; }
    }

    //public class PTCFareBreakdownsViewModel
    //{
    //    public List<PTCFareBreakdownViewModel> PTC_FareBreakdown { get; set; }
    //}

    public class PTCFareBreakdownViewModel
    {
        public PassengerTypeQuantity PassengerTypeQuantity { get; set; }
        public PassengerFare PassengerFare { get; set; }
    }

    public class ItinTotalFareViewModel
    {
        //public TotalTax TotalTax { get; set; }
        public TotalFare TotalFare { get; set; }
    }

    //public class FareInfosViewModel
    //{
    //    public List<FareInfoViewModel> FareInfo { get; set; }
    //}

    public class FareInfoViewModel
    {
        public LowestFare LowestFare { get; set; }
        public string CurrencyCode { get; set; }
        public LowestNonStopFare LowestNonStopFare { get; set; }
        public string DepartureDateTime { get; set; }
        public string ReturnDateTime { get; set; }
        public List<Link> Links { get; set; }
        public string DestinationLocation { get; set; }
        public string FareReference { get; set; }
        public TPAExtensions4 TPA_Extensions { get; set; }
    }

    public class FlightSegmentViewModel
    {
        public DepartureAirport DepartureAirport { get; set; }
        public ArrivalAirport ArrivalAirport { get; set; }
        public OperatingAirline OperatingAirline { get; set; }
        //public DepartureTimeZone DepartureTimeZone { get; set; }
        //public ArrivalTimeZone ArrivalTimeZone { get; set; }
        public List<StopAirport> StopAirport { get; set; }
        public string DepartureDateTime { get; set; }
        public string ArrivalDateTime { get; set; }
        public int StopQuantity { get; set; }
        public string FlightNumber { get; set; }
        public int ElapsedTime { get; set; }
    }
    public class StopAirport
    {
        public string DepartureDateTime { get; set; }
        public string ArrivalDateTime { get; set; }
        public string LocationCode { get; set; }
        public int ElapsedTime { get; set; }
        public int Duration { get; set; }
    }
}
