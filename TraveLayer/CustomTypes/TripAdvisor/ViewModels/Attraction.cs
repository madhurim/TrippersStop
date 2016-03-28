using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.ViewModels
{
    public class Attraction
    {
        public Address Address { get; set; }
        public string Distance { get; set; }
        //public object percent_recommended { get; set; }
        public string RecommendedPercentage { get; set; }
       // public string Bearing { get; set; }
        public string Latitude { get; set; }
        public string Rating { get; set; }
        public List<object> Cuisine { get; set; }
       // public List<Category> Cuisine { get; set; }
        
        public string LocationId { get; set; }
        public Rank Ranking { get; set; }
        public string ApiDetailUrl { get; set; }
        public string Location { get; set; }
        public string WebUrl { get; set; }
        public string PriceLevel { get; set; }
        public string RatingImageUrl { get; set; }
        //public List<object> awards { get; set; }
        public string Name { get; set; }
        public string NumReviews { get; set; }
       // public string WriteReview { get; set; }
        public Category Category { get; set; }

        //public List<object> subcategory { get; set; }
        //public List<Ancestor> ancestors { get; set; }
        public string SeeAllPhotos { get; set; }
        public string Longitude { get; set; }

        // Added for attractions
        public List<AttractionType> AttractionTypes { get; set; }
        public WikiPediaInfo WikiPediaInfo { get; set; }
    }


}
