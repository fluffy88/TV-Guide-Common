using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_Guide.JSONModels
{
    public class MoviesModel
    {
        public string bannerad { get; set; }
        public List<Movie> programs { get; set; }
    }

    public class Movie
    {
        public string channel { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string title { get; set; }
        public int duration { get; set; }
        public string subtitles { get; set; }
        public string url { get; set; }
        public string logo { get; set; }

        public string starttime
        {
            get
            {
                DateTime dt = Convert.ToDateTime(time);
                return String.Format("{0:D2}:{1:D2}", dt.Hour, dt.Minute);
            }
        }

    }

    public class MovieChannelCollection : List<Movie>
    {
        public string channel { get; set; }
    }
}