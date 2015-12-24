using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.Response
{
    public class Datum
    {
        public AddressObj address_obj { get; set; }
        public string distance { get; set; }
        public object percent_recommended { get; set; }
        public string bearing { get; set; }
        public string latitude { get; set; }
        public string rating { get; set; }
        public List<object> cuisine { get; set; }
        public string location_id { get; set; }
        public RankingData ranking_data { get; set; }
        public string api_detail_url { get; set; }
        public string location_string { get; set; }
        public string web_url { get; set; }
        public string price_level { get; set; }
        public string rating_image_url { get; set; }
        public List<object> awards { get; set; }
        public string name { get; set; }
        public string num_reviews { get; set; }
        public string write_review { get; set; }
        public Category category { get; set; }
        public List<object> subcategory { get; set; }
        public List<Ancestor> ancestors { get; set; }
        public string see_all_photos { get; set; }
        public string longitude { get; set; }
    }

}
