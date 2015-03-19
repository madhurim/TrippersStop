/// <summary>
///  This class rates weekly traffic volumes to certain destination airports.
///  Looks up the traffic volume booked via the Sabre GDS to the requested destination airport.
/// </summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    public class Seasonality
    {
        public int YearWeekNumber { get; set; }
        public string SeasonalityIndicator { get; set; }
        public string WeekEndDate { get; set; }
        public string NumberOfObservations { get; set; }
        public string WeekStartDate { get; set; }
    }

    public class OTA_TravelSeasonality
    {
        public string DestinationLocation { get; set; }
        public List<Seasonality> Seasonality { get; set; }
        public List<Link> Links { get; set; }
    }

    public class TravelSeasonality
    {
        public OTA_TravelSeasonality OTA_TravelSeasonality { get; set; }
    }
}
