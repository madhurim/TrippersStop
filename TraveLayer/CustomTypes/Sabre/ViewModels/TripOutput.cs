using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Weather;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    public class TripOutput
    {
        public TripWeather TripWeather { get; set; }
        public Fares Fares { get; set; }
        public FareRange FareRange { get; set; }
        public TravelSeasonality TravelSeasonality { get; set; }
        public LowFareForecast LowFareForecast { get; set; }
    }
    public class FareOutput
    {
        public FareRange FareRange { get; set; }
        public LowFareForecast LowFareForecast { get; set; }
    }
    public class SeasonalityOutput
    {
        public TripWeather TripWeather { get; set; }
        public TravelSeasonality TravelSeasonality { get; set; }
    }
}
