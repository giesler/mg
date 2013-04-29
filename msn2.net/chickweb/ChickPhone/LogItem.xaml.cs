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
using System.Windows.Data;

namespace ChickPhone
{
    public partial class LogItem : PhoneApplicationPage
    {
        List<CamDataService.LogItem> previousAndNextItems = null;
        static readonly string GetLogImageBaseUri = "http://cams.msn2.net/GetLogImage.aspx?a=";

        public LogItem()
        {
            InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (App.LogItem_LastImageId != null)
            {
                this.LoadItem(App.LogItem_LastImageId, App.LogItem_LastImageTimestamp);
            }
            else
            {
                string itemId = NavigationContext.QueryString["a"];
                DateTime ts = DateTime.Parse(NavigationContext.QueryString["ts"]);
                this.LoadItem(itemId, ts);
            }
        }

        void LoadItem(string itemId, DateTime ts)
        {
            App.LogItem_LastImageId = itemId;
            App.LogItem_LastImageTimestamp = ts;

            this.image.Source = new BitmapImage(new Uri(GetLogImageBaseUri + itemId));
            this.videos.ItemsSource = null;
            
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

            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = false;

            CameraDataClient client = new CameraDataClient();
            client.GetVideosCompleted += OnGetVideosCompleted;
            client.GetVideosAsync(ts.AddSeconds(-30), ts.AddMinutes(5));

            CameraDataClient client2 = new CameraDataClient();
            client2.GetPreviousAndNextLogItemsCompleted += OnGetPreviousAndNextCompleted;
            client2.GetPreviousAndNextLogItemsAsync(itemId);
        }

        void OnGetPreviousAndNextCompleted(object sender, GetPreviousAndNextLogItemsCompletedEventArgs e)
        {
            CameraDataClient client = (CameraDataClient)sender;

            if (e.Error == null)
            {
                this.previousAndNextItems = e.Result.ToList();

                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;
            }

            client.CloseAsync();
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

        private void OnPrevious(object sender, EventArgs e)
        {
            this.LoadItem(this.previousAndNextItems[0].Id, this.previousAndNextItems[0].Timestamp);
        }

        private void OnNext(object sender, EventArgs e)
        {
            this.LoadItem(this.previousAndNextItems[1].Id, this.previousAndNextItems[1].Timestamp);
        }

        private void imageButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/LogItemZoom.xaml?url={0}", GetLogImageBaseUri + App.LogItem_LastImageId), UriKind.Relative));
        }
    }

    public class UpperCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? null : value.ToString().ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}