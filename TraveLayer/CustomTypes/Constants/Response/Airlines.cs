using System.Collections.Generic;

namespace TraveLayer.CustomTypes.Constants.Response
{
    public class Airlines
    {
        public List<Response> response { get; set; }
    }
    public class Response
    {
        public string code { get; set; }
        public string name { get; set; }
    }
}
