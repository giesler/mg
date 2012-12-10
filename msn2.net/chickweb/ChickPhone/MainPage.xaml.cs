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

namespace ChickPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        DispatcherTimer timer;
        DispatcherTimer hideConnectMessage;
        bool inGet1 = false;
        bool inGet2 = false;
        string appRoot = "http://cc.msn2.net/";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
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
            this.info.Text = string.Format("{0}", netType.ToString().ToLower());
            this.hideConnectMessage = new DispatcherTimer();
            this.hideConnectMessage.Interval = new TimeSpan(0, 0, 5);
            this.hideConnectMessage.Tick += new EventHandler(this.OnHideNetworkMessage);
            this.hideConnectMessage.Start();
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
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }


        void OnTick(object sender, EventArgs e)
        {
            this.GetImage();
        }

        void GetImage()
        {
            if (!this.inGet1)
            {
                this.inGet1 = true;
                WebClient imgLoader = new WebClient();
                imgLoader.OpenReadCompleted += new OpenReadCompletedEventHandler(imgLoader1_OpenReadCompleted);


                string url = string.Format("{0}getimg.aspx?c={1}&id={2}", this.appRoot, "1", Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);
            }

            if (!this.inGet2)
            {
                this.inGet2 = true;
                WebClient imgLoader = new WebClient();
                imgLoader.OpenReadCompleted += new OpenReadCompletedEventHandler(imgLoader2_OpenReadCompleted);

                string url = string.Format("{0}getimg.aspx?c={1}&id={2}", this.appRoot, "2", Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);
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
                CompleteRead(this.img1a, this.imgFadeTo1a, this.img1b, this.imgFadeTo1b, this.img1txt, e);
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
                CompleteRead(this.img2a, this.imgFadeTo2a, this.img2b, this.imgFadeTo2b, this.img2txt, e);
                this.inGet2 = false;
            }
        }

        void CompleteRead(Image i1, Storyboard s1, Image i2, Storyboard s2, TextBlock txt, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null && e.Cancelled == false)
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

    }
}