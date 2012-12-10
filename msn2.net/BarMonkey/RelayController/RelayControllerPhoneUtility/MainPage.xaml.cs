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
using RelayControllerPhoneUtility.RelayController;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace RelayControllerPhoneUtility
{
    public partial class MainPage : PhoneApplicationPage
    {
        DispatcherTimer timer = null;
        DateTime endTime;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this.Toggle(false);

            RelayControllerClient client = new RelayControllerClient();
            client.ConnectTestCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_ConnectTestCompleted);
            client.ConnectTestAsync();

            this.relay.ItemsSource = App.Relays;
            this.seconds.ItemsSource = App.Seconds;

            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(100);
            this.timer.Tick += new EventHandler(this.OnTimerTick);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.seconds.SelectedItem = App.SecondCount;
            this.relay.SelectedItem = App.Relay;
        }

        void client_ConnectTestCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this.status.Visibility = System.Windows.Visibility.Collapsed;

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Connect Error", MessageBoxButton.OK);
            }
            else
            {
                this.Toggle(true);
            }
        }
 
        private void allOff_Click(object sender, RoutedEventArgs e)
        {
            RelayControllerClient client = new RelayControllerClient();
            client.TurnAllOffCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_TurnAllOffCompleted);
            client.TurnAllOffAsync();

            this.Toggle(false);

            this.pbar.IsIndeterminate = true;
        }

        void Toggle(bool enable)
        {
            this.allOff.IsEnabled = enable;
            this.flush.IsEnabled = enable;

            this.relay.IsEnabled = enable;
            this.seconds.IsEnabled = enable;

            this.previousRelay.IsEnabled = enable;
            this.nextRelay.IsEnabled = enable;

            this.previousTime.IsEnabled = enable;
            this.nextTime.IsEnabled = enable;

            this.pbar.Visibility = enable ? Visibility.Collapsed : System.Windows.Visibility.Visible;
        }

        void client_TurnAllOffCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this.Toggle(true);

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Turn Off Error", MessageBoxButton.OK);
            }
        }

        private void flush_Click(object sender, RoutedEventArgs e)
        {
            int seconds = int.Parse(this.seconds.SelectedItem.ToString());

            ObservableCollection<BatchItem> batch = new ObservableCollection<BatchItem>();
            batch.Add(new BatchItem { Group = 0, RelayNumber = int.Parse(this.relay.SelectedItem.ToString()), Seconds = seconds });
            
            RelayControllerClient client = new RelayControllerClient();
            client.SendBatchCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_SendBatchCompleted);
            client.SendBatchAsync(batch);

            this.Toggle(false);

            this.pbar.IsIndeterminate = false;
            this.pbar.Maximum = seconds * 1000;
            this.pbar.Value = 0;

            this.endTime = DateTime.Now.AddSeconds(seconds);

            this.timer.Start();
        }

        void client_SendBatchCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this.Toggle(true);
                        
            this.timer.Stop();
            this.pbar.IsIndeterminate = true;
        }
        
        private void relay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                App.Relay = int.Parse(e.AddedItems[0].ToString());

                ListPicker picker = (ListPicker)sender;
                this.previousRelay.IsEnabled = picker.SelectedIndex > 0;
                this.nextRelay.IsEnabled = picker.SelectedIndex < picker.Items.Count;
            }
        }

        private void seconds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                App.SecondCount = int.Parse(e.AddedItems[0].ToString());

                ListPicker picker = (ListPicker)sender;
                this.previousTime.IsEnabled = picker.SelectedIndex > 0;
                this.nextRelay.IsEnabled = picker.SelectedIndex < picker.Items.Count;
            }
        }

        void OnTimerTick(object sender, EventArgs e)
        {
            TimeSpan ts = this.endTime - DateTime.Now;

            if (ts.TotalMilliseconds > 0)
            {
                this.pbar.Value = this.pbar.Maximum - ts.TotalMilliseconds;
            }
            else
            {
                this.timer.Stop();
            }
        }

        private void previousRelay_Click(object sender, RoutedEventArgs e)
        {
            this.relay.SelectedItem = this.relay.Items[this.relay.SelectedIndex - 1];
        }

        private void nextRelay_Click(object sender, RoutedEventArgs e)
        {
            this.relay.SelectedItem = this.relay.Items[this.relay.SelectedIndex + 1];
        }

        private void previousTime_Click(object sender, RoutedEventArgs e)
        {
            this.seconds.SelectedItem = this.seconds.Items[this.seconds.SelectedIndex - 1];
        }

        private void nextTime_Click(object sender, RoutedEventArgs e)
        {
            this.seconds.SelectedItem = this.seconds.Items[this.seconds.SelectedIndex + 1];
        }
    }
}