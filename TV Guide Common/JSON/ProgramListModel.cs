using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_Guide.JSONModels
{
    public class ProgramListModel
    {
        public string bannerad { get; set; }
        public List<Dailylink> dailylinks { get; set; }
        public List<Program> programs { get; set; }
    }

    public class Dailylink
    {
        public string title { get; set; }
        public string link { get; set; }
    }

    public class Program
    {
        public string programtitle { get; set; }
        public string programtime { get; set; }
        public int repeat { get; set; }
        public int subtitles { get; set; }
        public string description { get; set; }
        public int duration { get; set; }
        private string _url;

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