//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
using System;
//using DataLayer.Interfaces;


namespace TrippismEntities
{
    public class ViewYoutubeVideo : EntityBase
    {
        public Guid RefGuid { get; set; }
        public string VideoID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
    }
}
