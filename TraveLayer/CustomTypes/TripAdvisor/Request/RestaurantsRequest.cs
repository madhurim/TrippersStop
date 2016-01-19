

namespace TraveLayer.CustomTypes.TripAdvisor.Request
{
    public class RestaurantsRequest : PropertiesRequest 
    {
        public string SubCategory { get; set; }
        public string Cuisines { get; set; }
        public string Prices { get; set; }
    }
}