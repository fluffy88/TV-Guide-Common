using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_Guide.JSONModels
{
    public class ChannelListModel
    {
        public string bannerad { get; set; }
        public List<Channel> channels { get; set; }
    }

    public class Channel
    {
        public string channel { get; set; }
        public int channelid { get; set; }
        private string _logo;
        private string _url;

        public string logo
        {
            get { return _logo; }
            set
            {
                if (_logo != value)
                {
                    _logo = Uri.EscapeUriString(value);
                }
            }
        }

        public string url
        {
            get { return _url; }
            set
            {
                if (_url != value)
                {
                    _url = Uri.EscapeUriString(value);
                }
            }
        }
    }
}