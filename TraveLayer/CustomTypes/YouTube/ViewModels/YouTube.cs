using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.YouTube.ViewModels
{
    public class YouTube
    {
        public string nextPageToken { get; set; }
        public string prevPageToken { get; set; }
        public pageInfo pageInfo { get; set; }
        public List<Items> items { get; set; }
    }

    public class Items
    {
        public Id id { get; set; }
        public Snippet snippet { get; set; }
    }
    public class pageInfo
    {
        public string totalResults { get; set; }
        public string resultsPerPage { get; set; }
    }
    public class Snippet
    {
        public string publishedat { get; set; }
        public string channelid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
    }
    public class Id
    {
        public string kind { get; set; }
        public string videoId { get; set; }
    }
    public class Thumbnails
    {
        public Medium medium { get; set; }
        public High high { get; set; }
        public Default @default { get; set; }

    }
    public class Default
    {
        public string url { get; set; }
    }
    public class Medium
    {
        public string url { get; set; }
    }
    public class High
    {
        public string url { get; set; }
    }

}
