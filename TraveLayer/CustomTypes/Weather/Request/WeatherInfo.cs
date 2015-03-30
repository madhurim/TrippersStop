using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Weather
{
   public class WeatherInfo
    {
         public string State { get; set; }
         public string City { get; set; }
         public DateTime DepartDate { get; set; }
         public DateTime ReturnDate { get; set; }

    }
}
