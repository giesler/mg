using Microsoft.Phone.Controls;
using System;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ChickPhone
{
    public partial class Video : PhoneApplicationPage
    {
        DispatcherTimer timer = new DispatcherTimer();

        public Video()
        {
            InitializeComponent();

            this.timer.Interval = TimeSpan.FromMilliseconds(250);
            this.timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.progress.Value = this.player.Position.TotalMilliseconds;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string video = NavigationContext.QueryString["v"];

            this.player.Source = new Uri("http://cam1.msn2.net:8808/GetVid.aspx?v=" + video);
        }

        private void player_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            this.pb.Visibility = System.Windows.Visibility.Collapsed;
            this.progress.Visibility = System.Windows.Visibility.Visible;
            this.progress.Maximum = this.player.NaturalDuration.TimeSpan.TotalMilliseconds;
            this.timer.Start();
        }

        private void player_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}