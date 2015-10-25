using System;

namespace TraveLayer.CustomTypes.Weather
{
    public class HistoryInput
    {
        public string State { get; set; }
        public string City { get; set; }
        public DateTime DepartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
