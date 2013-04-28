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
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Net.NetworkInformation;
using System.Diagnostics;

namespace ChickPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        DispatcherTimer timer;
        DispatcherTimer hideConnectMessage;
        bool inGet1 = false;
        bool inGet2 = false;
        bool inGet3 = false;
        bool inGet4 = false;
        bool inGet5 = false;
        bool inGet6 = false;
        DateTime lastRefresh1 = DateTime.MinValue;
        DateTime lastRefresh2 = DateTime.MinValue;
        DateTime lastRefresh3 = DateTime.MinValue;
        DateTime lastRefresh4 = DateTime.MinValue;
        DateTime lastRefresh5 = DateTime.MinValue;
        DateTime lastRefresh6 = DateTime.MinValue;

        string appRoot = "http://cams.msn2.net/";
        bool stop = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.timer.Tick += new EventHandler(this.OnTick);
            this.timer.Start();

            this.OnTick(null, null);

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ConnectToNetwork), null);
        }

        void ConnectToNetwork(object sender)
        {
            NetworkInterfaceType netType = NetworkInterface.NetworkInterfaceType;
            Dispatcher.BeginInvoke(() => { this.ConnectedToNetwork(netType); });
        }

        void ConnectedToNetwork(NetworkInterfaceType netType)
        {
            this.info.Text = GetNetType(netType);
            this.hideConnectMessage = new DispatcherTimer();
            this.hideConnectMessage.Interval = new TimeSpan(0, 0, 5);
            this.hideConnectMessage.Tick += new EventHandler(this.OnHideNetworkMessage);
            this.hideConnectMessage.Start();
        }

        string GetNetType(NetworkInterfaceType netType)
        {
            switch (netType)
            {
                case NetworkInterfaceType.Ethernet:
                    return "WIRED";
                case NetworkInterfaceType.MobileBroadbandCdma:
                    return "CDMA";
                case NetworkInterfaceType.Wireless80211:
                    return "WIFI";
                case NetworkInterfaceType.MobileBroadbandGsm:
                    return "GSM";
                default:
                    return netType.ToString().ToUpper();
            }
        }

        void OnHideNetworkMessage(object sender, EventArgs e)
        {
            this.hideConnectMessage.Stop();
            this.hideConnectMessage = null;

            this.info.Visibility = System.Windows.Visibility.Collapsed;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.cam1Zoom.Visibility = System.Windows.Visibility.Visible;
            this.cam2Zoom.Visibility = System.Windows.Visibility.Visible;
            this.cam3Zoom.Visibility = System.Windows.Visibility.Visible;
            this.cam4Zoom.Visibility = System.Windows.Visibility.Visible;
            this.cam5Zoom.Visibility = System.Windows.Visibility.Visible;
            this.cam6Zoom.Visibility = System.Windows.Visibility.Visible;

            this.stop = false;
        }

        void OnTick(object sender, EventArgs e)
        {
            if (!this.stop)
            {
                this.GetImage();
            }
        }

        void GetImage()
        {
            if (!this.inGet1 && (this.panorama.SelectedItem == this.cam1 || this.lastRefresh1.AddSeconds(15) < DateTime.Now))
            {
                this.inGet1 = true;
                WebClient imgLoader = new WebClient();
                imgLoader.OpenReadCompleted += new OpenReadCompletedEventHandler(imgLoader1_OpenReadCompleted);

                string url = string.Format("{0}getimg.aspx?c={1}&id={2}", this.appRoot, "1", Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);

                this.lastRefresh1 = DateTime.Now;
            }

            if (!this.inGet2 && (this.panorama.SelectedItem == this.cam2 || this.lastRefresh2.AddSeconds(15) < DateTime.Now))
            {
                this.inGet2 = true;
                WebClient imgLoader = new WebClient();
                imgLoader.OpenReadCompleted += new OpenReadCompletedEventHandler(imgLoader2_OpenReadCompleted);

                string url = string.Format("{0}getimg.aspx?c={1}&id={2}", this.appRoot, "2", Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);

                this.lastRefresh2 = DateTime.Now;
            }

            if (!this.inGet3 && (this.panorama.SelectedItem == this.cam3 || this.lastRefresh3.AddSeconds(15) < DateTime.Now))
            {
                this.inGet3 = true;
                WebClient imgLoader = new WebClient();
                imgLoader.OpenReadCompleted += new OpenReadCompletedEventHandler(imgLoader3_OpenReadCompleted);

                string url = string.Format("{0}getimg.aspx?c={1}&id={2}&r=1", this.appRoot, "3", Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);

                this.lastRefresh3 = DateTime.Now;
            }

            if (!this.inGet4 && (this.panorama.SelectedItem == this.cam4 || this.lastRefresh4.AddSeconds(15) < DateTime.Now))
            {
                this.inGet4 = true;
                WebClient imgLoader = new WebClient();
                imgLoader.OpenReadCompleted += new OpenReadCompletedEventHandler(imgLoader4_OpenReadCompleted);

                string url = string.Format("{0}getimg.aspx?c={1}&id={2}&r=1", this.appRoot, "front", Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);

                this.lastRefresh4 = DateTime.Now;
            }

            if (!this.inGet5 && (this.panorama.SelectedItem == this.cam5 || this.lastRefresh5.AddSeconds(15) < DateTime.Now))
            {
                this.inGet5 = true;
                WebClient imgLoader = new WebClient();
                imgLoader.OpenReadCompleted += new OpenReadCompletedEventHandler(imgLoader5_OpenReadCompleted);

                string url = string.Format("{0}getimg.aspx?c={1}&id={2}&r=1", this.appRoot, "dw1", Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);

                this.lastRefresh5 = DateTime.Now;
            }

            if (!this.inGet6 && (this.panorama.SelectedItem == this.cam6 || this.lastRefresh6.AddSeconds(16) < DateTime.Now))
            {
                this.inGet6 = true;
                WebClient imgLoader = new WebClient();
                imgLoader.OpenReadCompleted += new OpenReadCompletedEventHandler(imgLoader6_OpenReadCompleted);

                string url = string.Format("{0}getimg.aspx?c={1}&id={2}&r=1", this.appRoot, "side", Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);

                this.lastRefresh6 = DateTime.Now;
            }
        }

        void imgLoader1_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {            
            if (e.Error != null)
            {
                this.SetError(this.img1txt, e.Error.Message);
            }
            else
            {
                CompleteRead(this.img1a, this.imgFadeTo1a, this.img1b, this.imgFadeTo1b, this.img1txt, this.pb1, e);
                this.inGet1 = false;
            }
        }

        void imgLoader2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.SetError(this.img2txt, e.Error.Message);
            }
            else
            {
                CompleteRead(this.img2a, this.imgFadeTo2a, this.img2b, this.imgFadeTo2b, this.img2txt, this.pb2, e);
                this.inGet2 = false;
            }
        }

        void imgLoader3_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.SetError(this.img3txt, e.Error.Message);
            }
            else
            {
                CompleteRead(this.img3a, this.imgFadeTo3a, this.img3b, this.imgFadeTo3b, this.img3txt, this.pb3, e);
                this.inGet3 = false;
            }
        }
        
        void imgLoader4_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.SetError(this.img4txt, e.Error.Message);
            }
            else
            {
                CompleteRead(this.img4a, this.imgFadeTo4a, this.img4b, this.imgFadeTo4b, this.img4txt, this.pb4, e);
                this.inGet4 = false;
            }
        }
        
        void imgLoader5_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.SetError(this.img5txt, e.Error.Message);
            }
            else
            {
                CompleteRead(this.img5a, this.imgFadeTo5a, this.img5b, this.imgFadeTo5b, this.img5txt, this.pb5, e);
                this.inGet5 = false;
            }
        }
        
        void imgLoader6_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.SetError(this.img6txt, e.Error.Message);
            }
            else
            {
                CompleteRead(this.img6a, this.imgFadeTo6a, this.img6b, this.imgFadeTo6b, this.img6txt, this.pb6, e);
                this.inGet6 = false;
            }
        }

        void CompleteRead(Image i1, Storyboard s1, Image i2, Storyboard s2, TextBlock txt, ProgressBar pb, OpenReadCompletedEventArgs e)
        {
            pb.Visibility = System.Windows.Visibility.Collapsed;
            
            if (e.Error == null && e.Cancelled == false && !this.stop)
            {
                if (e.Result.Length == 0)
                {
                    this.SetError(this.img2txt, "cannot connect to camera");
                }
                else
                {

                    try
                    {
                        BitmapImage bmp = new BitmapImage();
                        bmp.SetSource(e.Result);

                        if (s1.GetCurrentState() == ClockState.Active)
                        {
                            s1.Stop();
                        }
                        i1.Source = i2.Source;
                        i2.Source = bmp;
                        s2.Begin();

                        txt.Visibility = System.Windows.Visibility.Collapsed;                        
                    }
                    catch (Exception ex)
                    {
                        this.SetError(txt, ex.Message);
                    }
                }
            }
        }


        private void SetError(TextBlock textBlock, string p)
        {
            textBlock.Text = "loading..." + Environment.NewLine + p;
            textBlock.Visibility = System.Windows.Visibility.Visible;
        }

        void OpenCam(ImageSource lastImage, string camera)
        {
            App.LastImage = lastImage;
            NavigationService.Navigate(new Uri(string.Format("/SingleCam.xaml?c={0}", camera), UriKind.Relative));
        }

        void OpenCam(ImageSource lastImage, int camera)
        {
            App.LastImage = lastImage;
            NavigationService.Navigate(new Uri(string.Format("/SingleCam.xaml?c={0}", camera), UriKind.Relative));
        }

        private void cam1Zoom_Click(object sender, RoutedEventArgs e)
        {
            this.stop = true;
            this.cam1Zoom.Visibility = System.Windows.Visibility.Collapsed;
            this.OpenCam(this.img1a.Source, 1);
        }

        private void cam2Zoom_Click(object sender, RoutedEventArgs e)
        {
            this.stop = true;
            this.cam2Zoom.Visibility = System.Windows.Visibility.Collapsed;
            this.OpenCam(this.img2a.Source, 2);
        }

        private void cam3Zoom_Click(object sender, RoutedEventArgs e)
        {
            this.stop = true;
            this.cam3Zoom.Visibility = System.Windows.Visibility.Collapsed;
            this.OpenCam(this.img3a.Source, 3);
        }

        private void cam4Zoom_Click(object sender, RoutedEventArgs e)
        {
            this.stop = true;
            this.cam4Zoom.Visibility = System.Windows.Visibility.Collapsed;
            this.OpenCam(this.img4a.Source, "front");
        }

        private void cam5Zoom_Click(object sender, RoutedEventArgs e)
        {
            this.stop = true;
            this.cam5Zoom.Visibility = System.Windows.Visibility.Collapsed;
            this.OpenCam(this.img5a.Source, "dw1");
        }
        
        private void cam6Zoom_Click(object sender, RoutedEventArgs e)
        {
            this.stop = true;
            this.cam6Zoom.Visibility = System.Windows.Visibility.Collapsed;
            this.OpenCam(this.img6a.Source, "side");
        }

        private void OnLogMenuItemClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/Log.xaml"), UriKind.Relative));
        }
    }
}