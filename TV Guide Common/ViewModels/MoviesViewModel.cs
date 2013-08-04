using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using TV_Guide.JSONModels;

namespace TV_Guide.ViewModels
{
    public class MoviesViewModel : INotifyPropertyChanged
    {
        private bool _isLoading = true;
        private Dictionary<string, MovieChannelCollection> channels = new Dictionary<string, MovieChannelCollection>();

        public ObservableCollection<MovieChannelCollection> MoviesCollection { get; private set; }

        public MoviesViewModel()
        {
            this.MoviesCollection = new ObservableCollection<MovieChannelCollection>();
            this.LoadPage();
        }

        public void LoadPage()
        {
            String url = String.Format("http://api.entertainment.ie/tvguide/categories.asp?cat=Movies&date={0}", DateTime.Today.ToString("dd/MM/yyyy"));
            HttpWebRequest request = HttpUtils.GetHttpRequest(url);
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            rawHtml = HttpUtility.HtmlDecode(rawHtml);

            var jsonObject = JsonConvert.DeserializeObject<MoviesModel>(rawHtml);

            foreach (Movie m in jsonObject.programs)
            {
                if (!channels.ContainsKey(m.channel))
                {
                    channels[m.channel] = new MovieChannelCollection() { channel = m.channel };
                }
                channels[m.channel].Add(m);
            }

            var sortedChannels = channels.Values.OrderBy((m) => {return m.channel;});
            foreach (MovieChannelCollection mc in sortedChannels)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { MoviesCollection.Add(mc); });
            }
            IsLoading = false;
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => handler(this, new PropertyChangedEventArgs(propertyName)));
            }
        }
    }
}