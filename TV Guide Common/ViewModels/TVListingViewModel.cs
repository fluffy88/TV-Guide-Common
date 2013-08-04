using TV_Guide;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Xml;
using TV_Guide.Classes;
using TV_Guide.JSONModels;

namespace TV_Guide.ViewModels
{
    public class TVListingViewModel : INotifyPropertyChanged
    {
        private bool _isLoading = false;
        private int LoadingThreads;

        public SortedObservableCollection<ProgramDetails> TVListingCollection { get; private set; }

        public TVListingViewModel()
        {
            this.TVListingCollection = new SortedObservableCollection<ProgramDetails>();
            this.IsLoading = false;
            this.LoadingThreads = 0;
        }

        public void LoadPage(string page)
        {
            IsLoading = true;
            HttpWebRequest request = HttpUtils.GetHttpRequest(page);
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            rawHtml = HttpUtility.HtmlDecode(rawHtml);

            var jsonObject = JsonConvert.DeserializeObject<ProgramListModel>(rawHtml);

            foreach (Program p in jsonObject.programs)
            {
                HttpWebRequest request = HttpUtils.GetHttpRequest(Uri.UnescapeDataString(p.url));
                request.BeginGetResponse(new AsyncCallback(DescCallBack), request);
                this.LoadingThreads++;
            }
        }

        private void DescCallBack(IAsyncResult ar)
        {
            string rawHtml = HttpUtils.GetResponse(ar);
            rawHtml = HttpUtility.HtmlDecode(rawHtml);
            var jsonObject = JsonConvert.DeserializeObject<ProgramModel>(rawHtml);

            if (jsonObject.program != null)
            {
                ProgramDetails details = jsonObject.program.details;
                Deployment.Current.Dispatcher.BeginInvoke(() => TVListingCollection.Add(details));
            }

            this.LoadingThreads--;
            if (this.LoadingThreads == 0)
            {
                IsLoading = false;
            }
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