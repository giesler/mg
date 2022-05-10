using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace msn2.net
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Path { get; private set; }
        public List<string> Files { get; private set; }
        public int CurrentIndex { get; private set; }
        public List<string> SortedFiles { get; private set; }
        public int CurrentSortedIndex { get; private set; }

        Timer timer = null;
        bool showInfo = false;
        int interval = 15;
        Point? lastMousePoint = null;
        bool randomMode = true;
        bool paused = false;
        DispatcherTimer videoTimer = null;
        bool videoPlaying = false;
        MediaElement activeMedia = null;
        MediaElement inactiveMedia = null;
        bool loadingImage = false;
        DispatcherTimer loadCompleteCheck = null;
        Storyboard storyboard = null;
        bool weatherShown = false;

        public MainWindow(string[] args)
        {
            InitializeComponent();
            
            foreach (string arg in args)
            {
                if (arg.StartsWith("/p:"))
                {
                    this.Path = arg.Substring(3);
                }
            }

            this.Path = @"c:\xmp";

            if (!Directory.Exists(this.Path))
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (Directory.Exists(drive.Name + "nr"))
                    {
                        this.Path = drive.Name + "nr";
                        break;
                    }
                }
            }

            ThreadPool.QueueUserWorkItem(this.LoadPics, null);
            ThreadPool.QueueUserWorkItem(this.LoadWeatherData, null);
// bugbug - broken with latest sonos version            ThreadPool.QueueUserWorkItem(this.LoadSonosData, null);

            this.insideGrid.Visibility = Visibility.Collapsed;
            this.outsideGrid.Visibility = Visibility.Collapsed;
            this.sonosGrid.Visibility = Visibility.Collapsed;
            this.forecastGrid.Visibility = Visibility.Collapsed;

            this.Cursor = Cursors.None;
            this.ForceCursor = true;

            this.videoTimer = new DispatcherTimer();
            this.videoTimer.Interval = TimeSpan.FromMilliseconds(500);
            this.videoTimer.Tick += new EventHandler(this.OnVideoTimerTick);

            this.loadCompleteCheck = new DispatcherTimer();
            this.loadCompleteCheck.Interval = TimeSpan.FromMilliseconds(200);
            this.loadCompleteCheck.Tick += new EventHandler(this.OnLoadCompleteCheck);

            Trace.WriteLine("MainWindow ctor complete");

            this.timeLabel.Content = DateTime.Now.ToString("t");
            this.dayLabel.Content = DateTime.Now.ToString("dddd");
            this.dateLabel.Content = DateTime.Now.ToString("M");
        }

        private void LoadWeatherData(object j)
        {
            NetatmoIntegration netatmo = new NetatmoIntegration();
            netatmo.Init();

            WeatherProvider weather = new WeatherProvider();
            DateTime lastWeatherUpdate = DateTime.MinValue;
            var lastForecastUpdate = DateTime.MinValue;

            Thread.Sleep(TimeSpan.FromSeconds(1));

            while (true)
            {
                try
                {
                    Device data = netatmo.GetWeatherData();
                    this.Dispatcher.Invoke(new TimerCallback(this.ShowWeatherData), data);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Error loading netatmo data: " + ex.ToString());
                }

                try
                {
                    if (lastWeatherUpdate.AddMinutes(10) < DateTime.Now) 
                    {
                        lastWeatherUpdate = DateTime.Now;

                        WeatherResponse wr = weather.GetWeatherData();
                        this.Dispatcher.Invoke(new TimerCallback(this.ShowWeatherProviderData), wr);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Error loading weather provider data: " + ex.ToString());
                }

                try
                {
                    if (lastForecastUpdate.AddMinutes(60) < DateTime.Now)
                    {
                        var wr = weather.GetForecastData();
                        this.Dispatcher.Invoke(new TimerCallback(this.ShowForecastData), wr);
                        lastForecastUpdate = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Error loading weather forecast data: " + ex.ToString());
                }

                Trace.WriteLine("Weather data load complete");

                Thread.Sleep(TimeSpan.FromMinutes(3));
            }
        }

        private void ShowWeatherData(object d)
        {
            if (this.insideGrid.Visibility != Visibility.Visible && !this.weatherShown)
            {
                this.insideGrid.Visibility = Visibility.Visible;
                this.outsideGrid.Visibility = Visibility.Visible;

                this.weatherShown = true;
            }

            Device data = (Device)d;

            Module outside = data.Modules.First(i => i.ModuleName == "Outdoor");
            float outsideF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.Temperature);
            this.outsideTemp.Text = ((int)outsideF).ToString("0");
            this.outsideTempDecimal.Text = "." + (outsideF % 1.0 * 10.0).ToString("0");
            double outsideFrac = outsideF - ((float)((int)outsideF) * 1.0);
            if (outsideFrac < 0.1)
            {
                this.outsideTempDecimal.Visibility = Visibility.Collapsed;
            }

            this.outsideTrend.Text = GetTrend(outside.DashboardData).Trim();

            float outsideMaxF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.MaxTemp);
            this.outsideMax.Text = ((int)outsideMaxF).ToString("0");
            this.outsideMaxDecimal.Text = (outsideMaxF % 1.0 * 10.0).ToString("0");

            float outsideMinF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.MinTemp);
            this.outsideMin.Text = ((int)outsideMinF).ToString("0");
            this.outsideMinDecimal.Text = (outsideMinF % 1.0 * 10.0).ToString("0");
            this.outsideHumidity.Text = outside.DashboardData.Humidity.ToString();
            
            float insideF = NetatmoIntegration.GetFahrenheit(data.DashboardData.Temperature);
            this.insideTemp.Text = ((int)insideF).ToString("0");
            this.insideTempDecimal.Text = "." + (insideF % 1.0 * 10.0).ToString("0");
            double insideFrac = insideF - ((float)((int)insideF) * 1.0);
            if (insideFrac < 0.1)
            {
                this.insideTempDecimal.Visibility = Visibility.Collapsed;
            }
            this.insideTrend.Text = GetTrend(data.DashboardData).Trim();

            float insideMaxF = NetatmoIntegration.GetFahrenheit(data.DashboardData.MaxTemp);
            this.insideMax.Text = ((int)insideMaxF).ToString("0");
            this.insideMaxDecimal.Text = (insideMaxF % 1.0 * 10.0).ToString("0");

            float insideMinF = NetatmoIntegration.GetFahrenheit(data.DashboardData.MinTemp);
            this.insideMin.Text = ((int)insideMinF).ToString("0");
            this.insideMinDecimal.Text = (insideMinF % 1.0 * 10.0).ToString("0");
            this.insideHumidity.Text = data.DashboardData.Humidity.ToString();

            string address = string.Format("https://svcs.giesler.org:8443/getimg.aspx?c=gdw&h=64&ts={0}", DateTime.Now.ToString("yymmddhhmmsstt"));

//            var camUri = new Uri("https://cams.ms2n.net/getimg.aspx?c=gdw&h=64&id=qss&ts=" + new Random().Next(1000000).ToString("00000000"));
//            this.outsideDriveway.Source =  new Uri(address);
        }

        private void ShowWeatherProviderData(object s)
        {
            WeatherResponse wr = (WeatherResponse)s;

            if (wr.WeatherObservation != null)
            {
                this.feelsLike.Text = ((int)wr.WeatherObservation.FeelLikeF).ToString();
                this.feelslikeDecimal.Text = (wr.WeatherObservation.FeelLikeF % 1.0 * 10.0).ToString("0");
            }
            else
            {
                this.feelsLike.Text = "error";
            }
        }

        private void ShowForecastData(object s)
        {
            var wr = (FiveDayForecast)s;

            this.day1Image.Source = GetForecastImage(wr.PartialForecast[0].IconCode[0]);
            this.day1Hi.Text = wr.MaxTemperature[0];
            this.day1Low.Text = wr.MinTemperature[0];

            this.day2Image.Source = GetForecastImage(wr.PartialForecast[0].IconCode[1]);
            this.day2Name.Text = wr.DayOfWeek[1].Substring(0, 3).ToUpper();
            this.day2Hi.Text = wr.MaxTemperature[1] == null ? "-" : wr.MaxTemperature[1];
            this.day2Low.Text = wr.MinTemperature[1];

            this.day3Image.Source = GetForecastImage(wr.PartialForecast[0].IconCode[2]);
            this.day3Name.Text = wr.DayOfWeek[2].Substring(0, 3).ToUpper();
            this.day3Hi.Text = wr.MaxTemperature[2];
            this.day3Low.Text = wr.MinTemperature[2];

            this.day4Image.Source = GetForecastImage(wr.PartialForecast[0].IconCode[3]);
            this.day4Name.Text = wr.DayOfWeek[3].Substring(0, 3).ToUpper();
            this.day4Hi.Text = wr.MaxTemperature[3];
            this.day4Low.Text = wr.MinTemperature[3];

    //    this.forecastGrid.Visibility = Visibility.Visible;
        }

        BitmapImage GetForecastImage(int? icon)
        {
            if (icon.HasValue == false)
            {
                return null;
            }

            return new BitmapImage(new Uri("https://icons.wxug.com/i/c/v4/" + icon.Value.ToString() + ".svg"));
        }

        private void LoadSonosData(object d)
        {
            SonosPlayingData lastData = null;

            while (true)
            {
                try
                {
                    SonosPlayingData data = SonosIntegration.GetPlayingData();

                    if (lastData == null || !lastData.Equals(data))
                    {
                        this.Dispatcher.Invoke(new TimerCallback(this.UpdateSonosData), data);
                    }

                    lastData = data;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Error loading sonos data: " + ex.ToString());
                }
                
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
        
        private void UpdateSonosData(object s)
        {
            SonosPlayingData data = (SonosPlayingData)s;
            if (data.Title == null)
            {
                this.sonosGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                this.sonosGrid.Visibility = Visibility.Visible;
                this.title.Text = data.Title;
                this.album.Text = data.Album;
                this.artist.Text = data.Artist;
                if (data.AlbumArtUri != null)
                {
                    this.albumArt.Source = new Uri(data.AlbumArtUri);
                    this.albumArtColumn.Width = (GridLength)new GridLengthConverter().ConvertFromString("120px");
                }
                else
                {
                    this.albumArt.Source = null;
                    this.albumArtColumn.Width = (GridLength)new GridLengthConverter().ConvertFromString("10px");
                }
            }
        }

        string GetTrend(DashboardData data)
        {
            if (data.TempTrend == "up")
            {
                return "↑";
            }
            else if (data.TempTrend == "down")
            {
                return "↓";
            }
            else
            {
                return "";
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.WindowState = System.Windows.WindowState.Maximized;
        }

        void OnVideoTimerTick(object sender, EventArgs e)
        {
            if (this.activeMedia != null && this.activeMedia.NaturalDuration.HasTimeSpan && this.videoTimer.IsEnabled)
            {
                this.pbar.Maximum = this.activeMedia.NaturalDuration.TimeSpan.TotalSeconds;

                if (this.activeMedia.Position.TotalSeconds >= this.activeMedia.NaturalDuration.TimeSpan.TotalSeconds)
                {
                    this.videoTimer.IsEnabled = false;
                    this.videoPlaying = false;
                    this.DisplayNextPicture(null);
                }
                else
                {
                    this.pbar.Value = this.activeMedia.Position.TotalSeconds;
                }
            }
        }

        void OnLoadCompleteCheck(object sender, EventArgs e)
        {
            if (this.activeMedia.DownloadProgress >= 1)
            {
                this.loadCompleteCheck.IsEnabled = false;
                this.loadingImage = false;

                this.activeMedia.Opacity = 1;
                this.activeMedia.Visibility = System.Windows.Visibility.Visible;
                this.inactiveMedia.Opacity = 0;
                this.inactiveMedia.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void LoadPics(object sender)
        {
            string[] files = Directory.GetFiles(this.Path, "*.jpg", SearchOption.AllDirectories);
            string[] avis = Directory.GetFiles(this.Path, "*.avi", SearchOption.AllDirectories);
            string[] mpgs = Directory.GetFiles(this.Path, "*.mpg", SearchOption.AllDirectories);

            List<string> all = new List<string>();
            files.ToList().ForEach(i => all.Add(i));
            avis.ToList().ForEach(i => all.Add(i));
            mpgs.ToList().ForEach(i => all.Add(i));

            this.Files = new List<string>();
            this.SortedFiles = new List<string>();

            List<string> random = new List<string>();
            foreach (string file in all.OrderBy(s => s))
            {
                random.Add(file);
                this.SortedFiles.Add(file);
            }

            Random r = new Random();
            while (random.Count > 0)
            {
                int index = r.Next(random.Count);
                this.Files.Add(random[index]);
                random.RemoveAt(index);
            }

            this.CreateTimer(true);
        }

        void CreateTimer(bool display)
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
            }

            this.timer = new Timer(this.OnTimer, null, this.interval * 1000, this.interval * 1000);

            if (display)
            {
                this.OnTimer(null);
            }
        }

        void OnTimer(object state)
        {
            if (!this.videoPlaying)
            {
                this.Dispatcher.Invoke(new TimerCallback(this.DisplayNextPicture), new object());
            }
        }

        void DisplayNextPicture(object sender)
        {
            if (!this.paused)
            {
                this.status.Visibility = Visibility.Collapsed;

                try
                {
                    if (this.randomMode)
                    {
                        if (this.CurrentIndex + 1 >= this.Files.Count)
                        {
                            this.CurrentIndex = 0;
                        }
                        else
                        {
                            this.CurrentIndex++;
                        }

                        Uri uri = new Uri(this.Files[this.CurrentIndex]);
                        this.SetItem(uri);
                    }
                    else
                    {
                        if (this.CurrentSortedIndex + 1 >= this.SortedFiles.Count)
                        {
                            this.CurrentSortedIndex = 0;
                        }
                        else
                        {
                            this.CurrentSortedIndex++;
                        }

                        Uri uri = new Uri(this.SortedFiles[this.CurrentSortedIndex]);
                        this.SetItem(uri);
                    }
                }
                catch (Exception ex)
                {
                    this.status.Visibility = Visibility.Visible;
                    Trace.WriteLine("Error displaying pic: " + ex.ToString());
                    this.status.Content = ex.Message;
                }
            }
        }

        void SetItem(Uri uri)
        {
            if (uri.ToString().ToLower().EndsWith("jpg"))
            {
                this.videoPlaying = false;
                this.videoTimer.IsEnabled = false;
                this.pbar.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.videoPlaying = true;
                this.videoTimer.IsEnabled = true;
                this.pbar.Visibility = System.Windows.Visibility.Visible;
                this.pbar.Value = 0;
            }

            this.info.Content = uri.ToString();

            try
            {
                Trace.WriteLine("Setting item: " + uri.ToString());
                Storyboard sb = null;
                Storyboard cur = null;

                if (this.activeMedia == this.item2)
                {
                    this.item2.Source = this.activeMedia.Source;

                    this.activeMedia = this.item1;
                    this.inactiveMedia = this.item2;

                    sb = (Storyboard)this.grid.FindResource("itemFadeTo1");
                    cur = (Storyboard)this.grid.FindResource("itemFadeTo2");
                }
                else
                {
                    if (this.activeMedia != null)
                    {
                        this.item1.Source = this.activeMedia.Source;
                    }

                    this.activeMedia = this.item2;
                    this.inactiveMedia = this.item1;

                    sb = (Storyboard)this.grid.FindResource("itemFadeTo2");
                    cur = (Storyboard)this.grid.FindResource("itemFadeTo1");
                }

                this.activeMedia.Source = uri;
                this.loadingImage = true;
                this.loadCompleteCheck.IsEnabled = true;
                this.storyboard = sb;
            }
            catch (Exception ex)
            {
                this.info.Content = "Error loading " + uri.ToString() + ": " + ex.Message;
                Trace.WriteLine($"Unable to load {uri} - {ex}");
            }
        }

        protected override void OnKeyUp(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyUp(e);

            Trace.WriteLine($"OnKeyUp: {e.Key} - loading {this.loadingImage}");

            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == System.Windows.Input.Key.Right && !this.loadingImage)
            {
                this.videoPlaying = false;
                this.CreateTimer(true);
            }
            else if (e.Key == System.Windows.Input.Key.Left && !this.loadingImage)
            {
                this.videoPlaying = false;
                if (this.randomMode)
                {
                    if (this.CurrentIndex > 0)
                    {
                        this.CurrentIndex--;
                        this.CurrentIndex--;
                    }
                }
                else
                {
                    if (this.CurrentSortedIndex > 0)
                    {
                        this.CurrentSortedIndex--;
                        this.CurrentSortedIndex--;
                    }
                }
                this.CreateTimer(true);
            }
            else if (e.Key == System.Windows.Input.Key.Up)
            {
                this.interval++;

                if (!this.loadingImage)
                {
                    this.CreateTimer(false);
                }
            }
            else if (e.Key == System.Windows.Input.Key.Down && this.interval > 1)
            {
                this.interval--;

                if (!this.loadingImage)
                {
                    this.CreateTimer(false);
                }
            }
            else if (e.Key == System.Windows.Input.Key.Delete && this.CurrentIndex >= 0 && !this.loadingImage)
            {
                if (this.randomMode)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.DeletePic), this.Files[this.CurrentIndex]);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.DeletePic), this.SortedFiles[this.CurrentSortedIndex]);
                }
                this.DisplayNextPicture(null);
            }
            else if (e.Key == Key.S || e.Key == Key.R)
            {
                if (this.randomMode)
                {
                    string currentFile = this.Files[this.CurrentIndex];
                    this.CurrentSortedIndex = this.SortedFiles.IndexOf(currentFile);
                }
                else
                {
                    string currentFile = this.SortedFiles[this.CurrentIndex];
                    this.CurrentIndex = this.Files.IndexOf(currentFile);
                }
                this.randomMode = !this.randomMode;
            }
            else if (e.Key == Key.Space)
            {
                this.paused = !this.paused;

                if (!this.paused && !this.loadingImage)
                {
                    this.DisplayNextPicture(null);
                }
            }
            else if (e.Key == Key.I)
            {
                this.showInfo = !this.showInfo;
                this.info.Visibility = this.showInfo ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (e.Key == Key.E)
            {
                string args = string.Format("/select,{0}", this.info.Content.ToString());
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("explorer.exe", args);
                p.Start();
                this.Close();
            }
            else if (e.Key == Key.W)
            {
                if (this.insideGrid.Visibility == Visibility.Visible)
                {
                    this.insideGrid.Visibility = Visibility.Collapsed;
                    this.outsideGrid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.insideGrid.Visibility = Visibility.Visible;
                    this.outsideGrid.Visibility = Visibility.Visible;
                }
            }
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!this.lastMousePoint.HasValue)
            {
                this.lastMousePoint = e.GetPosition(this);
            }

            Point cur = e.GetPosition(this);

            if (Math.Abs(cur.X - this.lastMousePoint.Value.X) > 0
                || Math.Abs(cur.Y - this.lastMousePoint.Value.Y) > 0)
            {
                this.Close();
            }
        }

        void DeletePic(object sender)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    Trace.WriteLine($"Waiting to delete {sender}");
                    Thread.Sleep(5000);
                    File.Delete(sender.ToString());
                    Trace.WriteLine($"Deleted {sender}");
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Failed to delete {sender}: {e}");
                }
            }
        }
    }
}
