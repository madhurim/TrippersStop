using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Google.ViewModels
{
    public class GooglePlaceDetails
    {
        public GooglePlaceDetails()
        {
            result = new List<GooglePlaceresults>();
        }
        public List<GooglePlaceresults> result { get; set; }
    }

    public class GooglePlaceresults
    {
        public GooglePlaceresults()
        {
            photos = new List<GooglePlacePhotos>();
        }

        public string adr_address { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }
        public GooglePlaceGeometry geometry { get; set; }
        public string icon { get; set; }
        public string international_phone_number { get; set; }
        public string name { get; set; }
        public List<GooglePlacePhotos> photos { get; set; }
        public string place_id { get; set; }
        public string rating { get; set; }
        public string vicinity { get; set; }
        public string website { get; set; }

    }

    public class GooglePlaceGeometry
    {
        public GooglePlaceLocation location { get; set; }
    }

    public class GooglePlaceLocation
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class GooglePlacePhotos
    {

        public string height { get; set; }
        public string width { get; set; }
    }

}
