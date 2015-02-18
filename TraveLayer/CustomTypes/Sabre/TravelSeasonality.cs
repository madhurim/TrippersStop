using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    //class TravelSeasonality
    //{
    //}

    public class Seasonality
    {
        public int YearWeekNumber { get; set; }
        public string SeasonalityIndicator { get; set; }
        public string WeekEndDate { get; set; }
        public string NumberOfObservations { get; set; }
        public string WeekStartDate { get; set; }
    }

    //public class Link
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    public class OTA_TravelSeasonality
    {
        public string DestinationLocation { get; set; }
        public List<Seasonality> Seasonality { get; set; }
       // public List<Link> Links { get; set; }
    }

    public class TravelSeasonality : ICustomType
    {
        public OTA_TravelSeasonality OTA_TravelSeasonality { get; set; }
    }
}
