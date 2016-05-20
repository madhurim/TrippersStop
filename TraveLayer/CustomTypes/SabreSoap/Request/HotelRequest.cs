using System;

namespace TraveLayer.CustomTypes.Sabre.SoapServices.Request
{
    public class HotelRequest
    {
        public string CorporateId { get; set; }
        public string GuestCounts { get; set; }
        public string HotelCityCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
        public string CurrencyCode { get; set; }
    }
}
