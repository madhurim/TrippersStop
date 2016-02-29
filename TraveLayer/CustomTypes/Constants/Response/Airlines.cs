using System.Collections.Generic;

namespace TraveLayer.CustomTypes.Constants.Response
{
    public class Airlines
    {
        public Request request { get; set; }
        public List<Response> response { get; set; }
    }


    public class Key
    {
        public string api_key { get; set; }
        public string type { get; set; }
        public object expired { get; set; }
    }

    public class Params
    {
        public string lang { get; set; }
    }

    public class Client
    {
        public string ip { get; set; }
    }

    public class Request
    {
        public string lang { get; set; }
        public string currency { get; set; }
        public int time { get; set; }
        public string id { get; set; }
        public string server { get; set; }
        public int pid { get; set; }
        public Key key { get; set; }
        public Params @params { get; set; }
        public Client client { get; set; }
    }

    public class Response
    {
        public string code { get; set; }
        public string icao { get; set; }
        public string name { get; set; }
        public object alias { get; set; }
        public string callsign { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
    }
}
