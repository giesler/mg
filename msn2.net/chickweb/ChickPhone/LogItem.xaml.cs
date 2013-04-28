using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChickPhone.CamDataService;

namespace ChickPhone
{
    public partial class LogItem : PhoneApplicationPage
    {
        public LogItem()
        {
            InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string itemId = NavigationContext.QueryString["a"];
            DateTime ts = DateTime.Parse(NavigationContext.QueryString["ts"]);

            this.image.Source = new BitmapImage(new Uri("http://cams.msn2.net/GetLogImage.aspx?a=" + itemId));

            if (ts.Date == DateTime.Today.Date)
            {
                this.timestamp.Text = "TODAY " + ts.ToString("h:mm tt");
            }
            else if (ts.Date.AddDays(1) == DateTime.Today)
            {
                this.timestamp.Text = "YESTERDAY " + ts.ToString("h:mm tt");
            }
            else
            {
                this.timestamp.Text = ts.ToString("ddd MMM d h:mm tt").ToUpper();
            }

            CameraDataClient client = new CameraDataClient();
            client.GetVideosCompleted += OnGetVideosCompleted;
            client.GetVideosAsync(ts.AddSeconds(-30), ts.AddMinutes(5));
        }

        void OnGetVideosCompleted(object sender, GetVideosCompletedEventArgs e)
        {
            CameraDataClient client = (CameraDataClient)sender;

            if (e.Error == null)
            {
                this.videos.ItemsSource = e.Result;
            }

            client.CloseAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.DataContext != null)
            {
                VideoItem video = (VideoItem)button.DataContext;
                NavigationService.Navigate(new Uri(string.Format("/Video.xaml?v={0}", video.Id), UriKind.Relative));
            }
        }
    }
}