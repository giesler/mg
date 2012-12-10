using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using msn2.net.BarMonkey;
using System.Windows.Threading;
using msn2.net.BarMonkey.RelayControllerService;
using System.Threading;
using System.Diagnostics;

namespace BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for PourDrink.xaml
    /// </summary>
    public partial class PourDrink : Page
    {
        private Drink drink;
        private Container container;
        private Dispenser disp;
        private MediaPlayer mediaPlayer;        
        private DispatcherTimer timer;

        public PourDrink()
        {
            InitializeComponent();

            this.disp = new Dispenser();
            this.disp.OnPourStarted += new EventHandler(disp_OnPourStarted);
            this.disp.OnPourConnectCompleted += new EventHandler(disp_OnPourConnectCompleted);
            this.disp.OnPourCompleted += new EventHandler(disp_OnPourCompleted);

            this.mediaPlayer = new MediaPlayer();
            this.mediaPlayer.MediaEnded += new EventHandler(mediaPlayer_MediaEnded);
            this.mediaPlayer.MediaFailed += mediaPlayer_MediaFailed;
            this.mediaPlayer.MediaOpened += mediaPlayer_MediaOpened;
            this.mediaPlayer.IsMuted = true;
            this.mediaPlayer.Open(new Uri("margaritaville.wav", UriKind.Relative));

            this.timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, this.OnTimer, this.Dispatcher);
        }

        void mediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            int totalSeconds = (int) this.mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            int randomStart = new Random().Next(0, totalSeconds-30);
            this.mediaPlayer.Position = new TimeSpan(0, 0, randomStart);
            this.mediaPlayer.IsMuted = false;
        }

        void mediaPlayer_MediaFailed(object sender, ExceptionEventArgs e)
        {
            Trace.WriteLine(e.ErrorException.Message);
        }
                
        void OnTimer(object sender, EventArgs e)
        {
            if (this.statusPanel.Visibility == System.Windows.Visibility.Visible && this.pbar.Value < this.pbar.Maximum)
            {
                this.pbar.Value++;
            }
        }

        void mediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            this.mediaPlayer.Position = new TimeSpan(0, 0, 0);
        }

        void disp_OnPourCompleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new WaitCallback(this.OnCompleted), new object());
        }

        void SetStatusText(object text)
        {
            this.statusLabel.Content = text.ToString();
        }

        void disp_OnPourConnectCompleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new WaitCallback(this.SetStatusText), "connected");
        }

        void OnCompleted(object sender)
        {
            this.statusLabel.Content = "";
            this.repeat.Visibility = Visibility.Visible;
            this.home.Visibility = System.Windows.Visibility.Visible;
            this.statusPanel.Visibility = System.Windows.Visibility.Collapsed;
            this.pbar.Visibility = System.Windows.Visibility.Collapsed;
            if (!string.IsNullOrEmpty(this.garnish.Text))
            {
                this.garnishPanel.Visibility = System.Windows.Visibility.Visible;
            }
            this.mediaPlayer.Stop();
            this.pbar.Value = 0;
            this.Title = "cheers!";
        }

        void disp_OnPourStarted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new WaitCallback(this.OnPourStarted), new object());
        }

        void OnPourStarted(object sender)
        {
            this.statusLabel.Content = "dispensing...";

            this.statusPanel.Visibility = System.Windows.Visibility.Visible;
            this.pbar.Visibility = System.Windows.Visibility.Visible;
            this.pbar.Maximum = this.disp.EstimatedDuration.TotalSeconds + 10;
        }

        public void SetDrink(Drink drink, Container container)
        {
            this.drink = drink;
            this.container = container;

            this.statusPanel.Visibility = System.Windows.Visibility.Visible;
            this.repeat.Visibility = Visibility.Hidden;
            this.home.Visibility = System.Windows.Visibility.Hidden;
            this.pbar.Visibility = System.Windows.Visibility.Collapsed;
            this.garnishPanel.Visibility = System.Windows.Visibility.Collapsed;
            this.garnish.Text = drink.GarnishMessage;

            this.Title = "pouring " + this.drink.Name.ToLower();
            this.statusLabel.Content = "connecting...";

            this.mediaPlayer.Play();

            while (true)
            {
                int random = new Random().Next(0, App.Messages.Length - 1);
                this.motd.Text = App.Messages[random];
                if (this.motd.Text.Length < 300)
                {
                    break;
                }
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(PourThread), new object());
        }

        private void PourThread(object o)
        {
            try
            {
                this.disp.PourDrink(BarMonkeyContext.Current, this.drink, this.container);
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new WaitCallback(this.displayException), ex);
            }
        }

        private void displayException(object o)
        {
            Exception exception = (Exception)o;
            this.statusLabel.Content = exception.ToString();
        }

        private void repeat_Click(object sender, RoutedEventArgs e)
        {
            this.SetDrink(this.drink, this.container);
        }

        private void gohome_Click(object sender, RoutedEventArgs e)
        {
            if (BarMonkeyContext.Current.CurrentUser.IsGuest == false)
            {
                this.NavigationService.Navigate(new UserHome());
            }
            else
            {
                App.GoToNewDrinkPage = true;
                Uri uri = new Uri("pack://application:,,,/PartyModeHomePage.xaml");                
                this.NavigationService.Navigate(uri);
            }            
        }        

    }
}
