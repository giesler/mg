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

namespace BarMonkeyPhone
{
    public partial class ManualPour : PhoneApplicationPage
    {
        public ManualPour()
        {
            InitializeComponent();

            this.relaySelector.DataSource = new IntLoopingDataSource { MinValue = 0, MaxValue = 48 };
            this.secondsSelector.DataSource = new IntLoopingDataSource { MinValue = 1, MaxValue = 120 };

            this.secondsSelector.DataSource.SelectedItem = 15;

            BarMonkeyClientService.BarMonkeyClientServiceClient client = new BarMonkeyClientService.BarMonkeyClientServiceClient();
            client.ConnectTestCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_ConnectTestCompleted);
            client.ConnectTestAsync();
        }

        void client_ConnectTestCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                this.status.Visibility = System.Windows.Visibility.Collapsed;
                this.progressBar.Visibility = System.Windows.Visibility.Collapsed;
                this.pour.IsEnabled = true;
                this.allOff.IsEnabled = true;
            }
        }

        private void pour_Click(object sender, RoutedEventArgs e)
        {
            this.allOff.IsEnabled = true;
        }

        private void allOff_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}