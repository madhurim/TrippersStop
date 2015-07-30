using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.YouTube.ViewModels
{
    public class YouTube
    {
        public string ChannelId { get; set; }

        public string Title { get; set; }

        public string VideoId { get; set; }

        public string Description { get; set; }

        public string DefaultURL { get; set; }

        public string MediumURL { get; set; }

        public string HighURL { get; set; }
    }
}
