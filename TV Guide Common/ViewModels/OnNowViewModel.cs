using TV_Guide;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows;
using TV_Guide.JSONModels;

namespace TV_Guide.ViewModels
{
    public class OnNowViewModel : INotifyPropertyChanged
    {
        private bool _isLoading = true;

        public ObservableCollection<OnNowChannelCollection> ProgramCollection { get; private set; }

        public OnNowViewModel()
        {
            this.ProgramCollection = new ObservableCollection<OnNowChannelCollection>();
            this.LoadPage();
        }

        public void LoadPage()
        {
            HttpWebRequest request = HttpUtils.GetHttpRequest("http://api.entertainment.ie/tvguide/tvnow.asp");
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            rawHtml = HttpUtility.HtmlDecode(rawHtml);

            var jsonObject = JsonConvert.DeserializeObject<OnNowModel>(rawHtml);

            foreach (OnNowChannel c in jsonObject.channels)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { ProgramCollection.Add(c.convertToCollection()); });
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