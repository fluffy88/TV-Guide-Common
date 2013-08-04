using TV_Guide;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml;
using TV_Guide.JSONModels;

namespace TV_Guide.ViewModels
{
    public class ChannelViewModel : INotifyPropertyChanged
    {
        private bool _isLoading = false;

        public ObservableCollection<Channel> ChannelCollection { get; private set; }

        public ChannelViewModel()
        {
            this.ChannelCollection = new ObservableCollection<Channel>();
            this.IsLoading = false;
        }

        public void LoadPage()
        {
            IsLoading = true;

            HttpWebRequest request = HttpUtils.GetHttpRequest("http://api.entertainment.ie/tvguide/listings.asp");
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            rawHtml = HttpUtility.HtmlDecode(rawHtml);

            var jsonObject = JsonConvert.DeserializeObject<ChannelListModel>(rawHtml);

            foreach (Channel c in jsonObject.channels)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { ChannelCollection.Add(c); });
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