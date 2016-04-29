//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
using System;
//using DataLayer.Interfaces;
namespace TrippismEntities
{
    public class SearchCriteria : EntityBase
    {
        public Guid RefGuid { get; set; }
        //public int SearchedTypeID { get; set; }
        public string OriginAirPort { get; set; }
        public string DestinationAirPort { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int LengthOfStay { get; set; }
        public decimal? MaxFare { get; set; }
        public decimal? MinFate { get; set; }
        public string Theme { get; set; }
        public string Region { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }

        
    }
}
