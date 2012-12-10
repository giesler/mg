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
using Microsoft.Phone.Shell;

namespace RelayControllerPhoneUtility
{
    public partial class MainPage : PhoneApplicationPage
    {
        DispatcherTimer timer = null;
        DateTime endTime;
        bool ctor = true;

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

            this.ctor = false;
            /*
            SystemTray.SetIsVisible(this, true);
            SystemTray.SetOpacity(this, 0.5);
            SystemTray.SetBackgroundColor(this, Colors.Purple);
            SystemTray.SetForegroundColor(this, Colors.Yellow);

            ProgressIndicator prog = new ProgressIndicator();
            prog.IsVisible = true;
            prog.IsIndeterminate = true;
            prog.Text = "Click me...";
            SystemTray.SetProgressIndicator(this, prog);
            */
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

            this.previousRelay.IsEnabled = enable && this.relay.SelectedIndex > 0;
            this.nextRelay.IsEnabled = enable && this.relay.SelectedIndex < this.relay.Items.Count;

            this.previousTime.IsEnabled = enable && this.seconds.SelectedIndex > 0;
            this.nextTime.IsEnabled = enable && this.seconds.SelectedIndex < this.seconds.Items.Count;

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
            if (e.AddedItems.Count > 0 && !this.ctor)
            {
                App.Relay = int.Parse(e.AddedItems[0].ToString());

                if (this.relay.IsEnabled)
                {
                    this.previousRelay.IsEnabled = this.relay.SelectedIndex > 0;
                    this.nextRelay.IsEnabled = this.relay.SelectedIndex < this.relay.Items.Count;
                }
            }
        }

        private void seconds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && !this.ctor)
            {
                App.SecondCount = int.Parse(e.AddedItems[0].ToString());

                if (this.seconds.IsEnabled)
                {
                    this.previousTime.IsEnabled = this.seconds.SelectedIndex > 0;
                    this.nextTime.IsEnabled = this.seconds.SelectedIndex < this.seconds.Items.Count;
                }
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