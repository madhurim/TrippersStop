using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.ViewModels
{
    public class Location : Attraction
    {
       // public ReviewRatingCount ReviewRatingCount { get; set; }
        public object ReviewRatingCount { get; set; }
        public List<Subrating> SubRatings { get; set; }
        public string PhotoCount { get; set; }
        public string LocationDetail { get; set; }
        public List<TripType> TripTypes { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Award> Awards { get; set; }
        public string GooglePlaceId { get; set; }
    }
}
