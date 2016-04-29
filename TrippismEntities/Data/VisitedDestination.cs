//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
using System;
//using DataLayer.Interfaces;

namespace TrippismEntities
{
    public class VisitedDestination : EntityBase
    {
        public Guid RefGuid { get; set; }
        public string Origin_airprot_CityCode { get; set; }
        public string Origin_airport_CityName { get; set; }
        public string Origin_airport_Code { get; set; }
        public string Origin_airport_CountryCode { get; set; }
        public string Origin_airport_RegionName { get; set; }
        public string Destination_airport_CityCode { get; set; }
        public string Destination_airport_CityName { get; set; }
        public string Destination_airport_Code { get; set; }
        public string Destination_airport_CountryCode { get; set; }
        public string Destination_airport_RegionName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
