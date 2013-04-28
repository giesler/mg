using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace ChickPhone
{
    public partial class Log : PhoneApplicationPage
    {
        public Log()
        {
            InitializeComponent();

            List<DateItem> dates = new List<DateItem>();
            dates.Add(new DateItem("TODAY", DateTime.Now.Date));
            dates.Add(new DateItem("YESTERDAY", DateTime.Now.Date.AddDays(-1)));
            DateTime temp = DateTime.Now.Date.AddDays(-2);
            for (int i = 0; i < 6; i++)
            {
                dates.Add(new DateItem(temp.ToString("dddd MMMM d").ToUpper(), temp));
                temp = temp.Date.AddDays(-1);
            }
            this.dateSelect.ItemsSource = dates;
            this.dateSelect.SelectedIndex = 0;
        }

        void OnGetItemsCompleted(object sender, CamDataService.GetItemsCompletedEventArgs e)
        {
            CamDataService.CameraDataClient camClient = (CamDataService.CameraDataClient)sender;

            if (e.Error == null)
            {
                this.logPictures.ItemHeight = 96;
                this.logPictures.ItemWidth = 128;

                var items = e.Result;
                foreach (var item in items)
                {
                    Button button = new Button();
                    button.Click += OnButtonClick;
                    button.BorderBrush = null;
                    button.DataContext = item;
                    button.Margin = new Thickness(0);
                    button.Padding = new Thickness(0);

                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(item.Url));
                    image.Height = 96;
                    image.Width = 128;
                    image.Margin = new Thickness(0);
                    button.Content = image;

                    this.logPictures.Children.Add(button);
                }
            }

            this.pblog.Visibility = Visibility.Collapsed;

            camClient.CloseAsync();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Image image = (Image)button.Content;

            CamDataService.LogItem item = (CamDataService.LogItem)button.DataContext;

            NavigationService.Navigate(new Uri(string.Format("/LogItem.xaml?a={0}&ts={1}", item.Id, item.Timestamp), UriKind.Relative));
        }

        private void ListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                DateItem item = e.AddedItems[0] as DateItem;

                CamDataService.CameraDataClient camClient = new CamDataService.CameraDataClient();
                camClient.GetItemsCompleted += OnGetItemsCompleted;
                camClient.GetItemsAsync(item.Time);

                this.logPictures.Children.Clear();
                this.pblog.Visibility = System.Windows.Visibility.Visible;
            }
        }

    }

    class DateItem
    {
        public DateItem(string name, DateTime time)
        {
            this.DisplayText = name;
            this.Time = time;
        }

        public string DisplayText { get; private set; }
        public DateTime Time { get; private set; }
    }
}