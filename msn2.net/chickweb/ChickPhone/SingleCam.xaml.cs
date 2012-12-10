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
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;

namespace ChickPhone
{
    public partial class SingleCam : PhoneApplicationPage
    {
        DispatcherTimer timer;
        string appRoot = "http://cc.msn2.net/";
        bool inGet = false;
        string camera = "1";

        public SingleCam()
        {
            InitializeComponent();

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.timer.Tick += new EventHandler(this.OnTick);
            this.timer.Start();

            if (App.LastImage != null)
            {
                this.imga.Source = App.LastImage;
                this.imgb.Source = App.LastImage;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.camera = NavigationContext.QueryString["c"];

            this.OnTick(null, null);

            SystemTray.IsVisible = false;
        }

        void OnTick(object sender, EventArgs e)
        {
            this.GetImage();
        }

        void GetImage()
        {
            if (!this.inGet)
            {
                this.inGet = true;
                WebClient imgLoader = new WebClient();
                imgLoader.OpenReadCompleted += new OpenReadCompletedEventHandler(imgLoader1_OpenReadCompleted);
                string rotate = this.camera == "3" || this.camera == "dw1" ? "1" : "0";

                string url = string.Format("{0}getimg.aspx?c={1}&r={2}&id={3}", this.appRoot, this.camera, rotate, Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);
            }
        }

        void imgLoader1_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.SetError(this.imgtxt, e.Error.Message);
            }
            else
            {
                CompleteRead(this.imga, this.imgFadeToa, this.imgb, this.imgFadeTob, this.imgtxt, e);
                this.inGet = false;
            }
        }


        void CompleteRead(Image i1, Storyboard s1, Image i2, Storyboard s2, TextBlock txt, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null && e.Cancelled == false)
            {
                if (e.Result.Length == 0)
                {
                    this.SetError(this.imgtxt, "cannot connect to camera");
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

        double initialScale = 0.0;

        private void GestureListener_PinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            initialScale = transform.ScaleX;
        }

        private void GestureListener_PinchDelta(object sender, PinchGestureEventArgs e)
        {
            transform.ScaleX = initialScale * e.DistanceRatio;
            transform.ScaleY = initialScale * e.DistanceRatio;
        }

    }
}