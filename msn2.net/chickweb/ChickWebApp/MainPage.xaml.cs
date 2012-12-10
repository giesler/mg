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
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;
using System.Windows.Interop;

namespace ChickWebApp
{
    public partial class MainPage : UserControl
    {
        DateTime startTime = DateTime.Now;
        DateTime stopTime = DateTime.MinValue;
        DispatcherTimer timer = null;
        int RefreshMinutes = 10;
        bool inGet1 = false;
        bool inGet2 = false;
        App app = null;

        public MainPage(App app)
        {
            InitializeComponent();

            if (App.IsLong)
            {
                this.RefreshMinutes = 200;
            }
            this.progress.Maximum = this.RefreshMinutes * 60;
            this.stopTime = DateTime.Now.AddMinutes(RefreshMinutes);

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.timer.Tick += new EventHandler(this.OnTick);
            this.timer.Start();

            this.OnTick(null, null);

            this.app = app;
            app.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);

            this.startTime = DateTime.Now;
        }

        void Content_FullScreenChanged(object sender, EventArgs e)
        {
            bool fullScreen = this.app.Host.Content.IsFullScreen;

            this.button1.Visibility = fullScreen ? Visibility.Collapsed : Visibility.Visible;
            this.fullScreen.Visibility = fullScreen ? Visibility.Collapsed : Visibility.Visible;

            this.headerText.FontSize = fullScreen ? 8 : 16;
            this.headerText.Margin = fullScreen ? new Thickness(2) : new Thickness(5);
            this.headerRow.Height = fullScreen ? new GridLength(15) : new GridLength(30);

            this.Reset();
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

                string url = string.Format("{0}getimg.aspx?c={1}&id={2}", App.GetAppRoot(), "1", Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);
            }

            if (!this.inGet2)
            {
                this.inGet2 = true;
                WebClient imgLoader = new WebClient();
                imgLoader.OpenReadCompleted += new OpenReadCompletedEventHandler(imgLoader2_OpenReadCompleted);

                string url = string.Format("{0}getimg.aspx?c={1}&id={2}", App.GetAppRoot(), "2", Guid.NewGuid());
                Uri uri = new Uri(url);
                imgLoader.OpenReadAsync(uri);
            }

            TimeSpan ts = this.stopTime - DateTime.Now;
            if (ts.TotalSeconds <= 0)
            {
                this.timer.Stop();

                TimeoutDialog dialog = new TimeoutDialog();
                dialog.Closed += new EventHandler(OnTimeoutDialogClosed);
                dialog.Show();
            }
            else
            {
                this.progress.Value = (this.RefreshMinutes * 60) - ts.TotalSeconds;
            }
        }

        void Reset()
        {
            this.startTime = DateTime.Now;
            this.stopTime = this.startTime.AddMinutes(this.RefreshMinutes);
            this.GetImage();
        }

        void OnTimeoutDialogClosed(object sender, EventArgs e)
        {
            this.stopTime = DateTime.Now.AddMinutes(RefreshMinutes);
            this.timer.Start();
        }

        void imgLoader1_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            CompleteRead(this.img1a, this.imgFadeTo1a, this.img1b, this.imgFadeTo1b, this.img1txt, e);
            this.inGet1 = false;
        }

        void imgLoader2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            CompleteRead(this.img2a, this.imgFadeTo2a, this.img2b, this.imgFadeTo2b, this.img2txt, e);
            this.inGet2 = false;
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AboutDialog dialog = new AboutDialog();
            dialog.Show();
        }

        private void fullScreen_Click(object sender, RoutedEventArgs e)
        {
            app.Host.Content.FullScreenOptions = FullScreenOptions.StaysFullScreenWhenUnfocused;
            app.Host.Content.IsFullScreen = true;
        }

        private void img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Reset();
        }
    }
}
