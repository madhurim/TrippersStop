//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
using System;
//using DataLayer.Interfaces;

namespace TrippismEntities
{
    public class ViewAttractionPlace : EntityBase
    {
        public Guid RefGuid { get; set; }
        public string Logn_Name { get; set; }
        public string Short_name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Formatted_Address { get; set; }
        public decimal? Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
    }
}
