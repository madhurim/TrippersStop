﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Google.ViewModels
{
    public class Google
    {
        public List<results> results { get; set; }
    }

    public class results
    {
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public Photos photos { get; set; }
        public string place_id { get; set; }
        public string rating { get; set; }
        public string reference { get; set; }
        public string scope { get; set; }
        public string types { get; set; }
        public string vicinity { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
    }

    public class Location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class Photos
    {
        public string height { get; set; }
        public string html_attributions { get; set; }
        public string photo_reference { get; set; }
        public string width { get; set; }
    }

}
