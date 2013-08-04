using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_Guide.JSONModels
{
    public class ProgramModel
    {
        public string bannerad { get; set; }
        public DetailedProgram program { get; set; }
    }

    public class ProgramDetails : IComparable
    {
        public string channel { get; set; }
        public int channelid { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        private string _duration;
        private DateTime _programtime;
        public int subtitles { get; set; }
        public string description { get; set; }
        public string channelurl { get; set; }
        public string image { get; set; }

        public string duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    _duration = " - " + value + "mins";
                }
            }
        }

        public DateTime programtime
        {
            get { return _programtime; }
            set
            {
                if (_programtime != value)
                {
                    _programtime = Convert.ToDateTime(value);
                }
            }
        }

        public string starttime
        {
            get
            {
                return String.Format("{0:D2}:{1:D2}", _programtime.Hour, _programtime.Minute);
            }
        }

        public int CompareTo(object obj)
        {
            return programtime.CompareTo(((ProgramDetails)obj).programtime);
        }
    }

    public class Link
    {
        public string title { get; set; }
        public string url { get; set; }
        public string type { get; set; }
    }

    public class DetailedProgram
    {
        public ProgramDetails details { get; set; }
        public List<Link> links { get; set; }
    }
}