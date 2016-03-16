using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Yelp.Response
{
    public class YelpOutput
    {
        public Region region { get; set; }
        public string total { get; set; }
        public List<Businesses> businesses { get; set; }
    }
    public class Businesses
    {
        public string is_claimed { get; set; }
        public string distance { get; set; }
        public string mobile_url { get; set; }
        public string rating_img_url { get; set; }
        public string review_count { get; set; }
        public string name { get; set; }
        public string snippet_image_url { get; set; }
        public string rating { get; set; }
        public string url { get; set; }
        public string phone { get; set; }
        public string snippet_text { get; set; }
        public string image_url { get; set; }
        public string display_phone { get; set; }
        public string rating_img_url_large { get; set; }
        public string id { get; set; }
        public string is_closed { get; set; }
        public string rating_img_url_small { get; set; }
        public string categories { get; set; }
        public Location location { get; set; }
    }
    public class Location
    {
        public string city { get; set; }
        public string display_address { get; set; }
        public string geo_accuracy { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string address { get; set; }
        public string coordinate { get; set; }
        public string state_code { get; set; }
    }
    public class Region
    {
        public Span span { get; set; }
        public Center center { get; set; }
    }
    public class Span
    {
        public string latitude_delta { get; set; }
        public string longitude_delta { get; set; }
    }
    public class Center
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

}


