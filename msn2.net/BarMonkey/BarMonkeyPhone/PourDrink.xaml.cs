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
    public partial class PourDrink : PhoneApplicationPage
    {
        int drinkId;

        public PourDrink()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PourDrink_Loaded);

            this.status.Text = "";
        }

        void PourDrink_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("name") == false)
            {
                NavigationService.GoBack();
            }

            this.drinkName.Header = NavigationContext.QueryString["name"];
            this.drinkId = int.Parse(NavigationContext.QueryString["id"]);

            this.pour.IsEnabled = true;
        }

        private void pour_Click(object sender, RoutedEventArgs e)
        {
            this.pour.IsEnabled = false;
            this.pbar.Visibility = System.Windows.Visibility.Visible;

            this.status.Text = "connecting...";

            BarMonkeyClientServiceClient client = App.GetClient();
            client.ConnectTestCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_ConnectTestCompleted);
            client.ConnectTestAsync();
        }

        void client_ConnectTestCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.status.Text = "pouring...";

                BarMonkeyClientServiceClient client = App.GetClient();
                client.PourDrinkCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_PourDrinkCompleted);
                client.PourDrinkAsync(drinkId, 4);
            }
            else
            {
                this.pbar.Visibility = System.Windows.Visibility.Collapsed;
                this.pour.IsEnabled = true;
                this.status.Text = e.Error.Message;
            }

            BarMonkeyClientServiceClient c = (BarMonkeyClientServiceClient)sender;
            c.CloseAsync();
        }

        void client_PourDrinkCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.status.Text = "Cheers!";
            }
            else
            {
                this.status.Text = e.Error.Message;
            }

            this.pour.IsEnabled = true;
            this.pbar.Visibility = System.Windows.Visibility.Collapsed;

            BarMonkeyClientServiceClient c = (BarMonkeyClientServiceClient)sender;
            c.CloseAsync();
        }

        
    }
}