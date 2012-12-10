using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BarMonkeyPhone.BarMonkeyClientService;

namespace BarMonkeyPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        Point leftMouseDownPoint;
        TextBlock leftMouseDownBlock;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            BarMonkeyClientServiceClient client1 = App.GetClient();
            client1.GetTopDrinksCompleted += new EventHandler<BarMonkeyClientService.GetTopDrinksCompletedEventArgs>(client_GetTopDrinksCompleted);
            client1.GetTopDrinksAsync(10);
        }

        void client_GetTopDrinksCompleted(object sender, BarMonkeyClientService.GetTopDrinksCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.top10List.ItemsSource = e.Result;
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }

            this.pbarTop10.Visibility = System.Windows.Visibility.Collapsed;

            BarMonkeyClientServiceClient client = (BarMonkeyClientServiceClient)sender;
            client.CloseAsync();

            BarMonkeyClientServiceClient client2 = App.GetClient();
            client2.GetAllDrinksCompleted += new EventHandler<GetAllDrinksCompletedEventArgs>(client2_GetAllDrinksCompleted);
            client2.GetAllDrinksAsync();
        }

        void client2_GetAllDrinksCompleted(object sender, GetAllDrinksCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.allList.ItemsSource = e.Result;
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }

            this.pbarAll.Visibility = System.Windows.Visibility.Collapsed;

            BarMonkeyClientServiceClient client = (BarMonkeyClientServiceClient)sender;
            client.CloseAsync();
        }

        private void OnSelectDrink(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);

            TextBlock tb = (TextBlock)sender;
            if (tb == this.leftMouseDownBlock)
            {
                double xdiff = p.X - this.leftMouseDownPoint.X;
                double ydiff = p.Y - this.leftMouseDownPoint.Y;
                if (Math.Abs(xdiff) < 5 && Math.Abs(ydiff) < 5)
                {
                    Drink d = tb.DataContext as Drink;

                    string uriText = string.Format("/PourDrink.xaml?name={0}&id={1}", tb.Text, d.Id);
                    Uri uri = new Uri(uriText, UriKind.Relative);
                    NavigationService.Navigate(uri);
                }
            }
        }

        private void OnLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.leftMouseDownPoint = e.GetPosition(this);
            this.leftMouseDownBlock = (TextBlock)sender;
        }
    }
}