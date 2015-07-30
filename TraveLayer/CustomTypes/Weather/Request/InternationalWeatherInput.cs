using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Weather.Request
{
    public class InternationalWeatherInput
    {
        public string CountryCode { get; set; }
        public string AirportCode { get; set; }
        public DateTime DepartDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
