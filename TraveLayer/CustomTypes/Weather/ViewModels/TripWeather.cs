using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Weather
{
    public class TripWeather
    {
        public TempHighAvg TempHighAvg { get; set; }
        public TempLowAvg TempLowAvg { get; set; }
        public List<WeatherChance> WeatherChances { get; set; }
        public CloudCover CloudCover { get; set; }
    }

    public class TempHighAvg
    {
        public Avg Avg { get; set; }
    }
    public class TempLowAvg
    {
        public Avg Avg { get; set; }
    }
    public class WeatherChance
    {
        public string ChanceType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Percentage { get; set; }
    }
}
