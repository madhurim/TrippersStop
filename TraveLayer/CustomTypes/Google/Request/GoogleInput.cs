using System.Collections.Generic;
namespace TraveLayer.CustomTypes.Google.Request
{
    public class GoogleInput
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string NextPageToken { get; set; }
        public string Types { get; set; }
        public string Keywords { get; set; }
        public string[] ExcludeTypes { get; set; }
    }
}
