using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.ViewModels
{
    public class Review
    {
        public string Id { get; set; }
        public string Lang { get; set; }
        public string LocationId { get; set; }
        public string PublishedDate { get; set; }
        public int Rating { get; set; }
        public string HelpfulVotes { get; set; }
        public string RatingImageUrl { get; set; }
        public string Url { get; set; }
        public string TripType { get; set; }
        public string TravelDate { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public bool IsMachineTranslated { get; set; }
    }
}
