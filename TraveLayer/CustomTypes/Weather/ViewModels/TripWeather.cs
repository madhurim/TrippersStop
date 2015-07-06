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
        public ChanceOf ChanceOf { get; set; }

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
}
