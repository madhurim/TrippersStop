using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels
{
    public class HotelDetail
    {
        public string[] Address { get; set; }
        public HotelRating[] HotelRating { get; set; }
        //public string HotelRating { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string HotelName { get; set; }
        public string HotelCode { get; set; }
        public string HotelCityCode { get; set; }
        public RateRange RateRange { get; set; }
    }
}
