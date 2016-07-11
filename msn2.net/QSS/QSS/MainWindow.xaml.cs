using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Web;

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
        int interval = 4;
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

            this.Path = @"\\server0\Plex\MSN2";
            this.Path = @"d:\onedrive\pictures\store";
            foreach (string arg in args)
            {
                if (arg.StartsWith("/p:"))
                {
                    this.Path = arg.Substring(3);
                }
            }
            
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
            ThreadPool.QueueUserWorkItem(this.LoadSonosData, null);

            this.insideGrid.Visibility = Visibility.Collapsed;
            this.outsideGrid.Visibility = Visibility.Collapsed;
            this.sonosGrid.Visibility = Visibility.Collapsed;

            this.Cursor = Cursors.None;
            this.ForceCursor = true;

            this.videoTimer = new DispatcherTimer();
            this.videoTimer.Interval = TimeSpan.FromMilliseconds(500);
            this.videoTimer.Tick += new EventHandler(this.OnVideoTimerTick);

            this.loadCompleteCheck = new DispatcherTimer();
            this.loadCompleteCheck.Interval = TimeSpan.FromMilliseconds(200);
            this.loadCompleteCheck.Tick += new EventHandler(this.OnLoadCompleteCheck);
        }

        private void LoadWeatherData(object j)
        {
            NetatmoIntegration netatmo = new NetatmoIntegration();
            netatmo.Init();

            WeatherProvider weather = new WeatherProvider();
            DateTime lastWeatherUpdate = DateTime.MinValue;

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
                    if (lastWeatherUpdate.AddMinutes(10) < DateTime.Now) ;
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

            this.outsideTrend.Text = GetTrend(outside).Trim();

            float outsideMaxF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.MaxTemp);
            this.outsideMax.Text = ((int)outsideMaxF).ToString("0");
            this.outsideMaxDecimal.Text = (outsideMaxF % 1.0 * 10.0).ToString("0");

            float outsideMinF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.MinTemp);
            this.outsideMin.Text = ((int)outsideMinF).ToString("0");
            this.outsideMinDecimal.Text = (outsideMinF % 1.0 * 10.0).ToString("0");
            this.outsideHumidity.Text = outside.DashboardData.Humidity.ToString();

            Module inside = data.Modules.First(i => i.ModuleName == "Bedroom");
            float insideF = NetatmoIntegration.GetFahrenheit(inside.DashboardData.Temperature);
            this.insideTemp.Text = ((int)insideF).ToString("0");
            this.insideTempDecimal.Text = "." + (insideF % 1.0 * 10.0).ToString("0");
            double insideFrac = insideF - ((float)((int)insideF) * 1.0);
            if (insideFrac < 0.1)
            {
                this.insideTempDecimal.Visibility = Visibility.Collapsed;
            }
            this.insideTrend.Text = GetTrend(inside).Trim();

            float insideMaxF = NetatmoIntegration.GetFahrenheit(inside.DashboardData.MaxTemp);
            this.insideMax.Text = ((int)insideMaxF).ToString("0");
            this.insideMaxDecimal.Text = (insideMaxF % 1.0 * 10.0).ToString("0");

            float insideMinF = NetatmoIntegration.GetFahrenheit(inside.DashboardData.MinTemp);
            this.insideMin.Text = ((int)insideMinF).ToString("0");
            this.insideMinDecimal.Text = (insideMinF % 1.0 * 10.0).ToString("0");
            this.insideHumidity.Text = inside.DashboardData.Humidity.ToString();

            this.outsideDriveway.Source = new Uri("http://ddns.msn2.net:8808/getimg.aspx?c=dw1&h=64&id=qss" + new Random().Next(1000000).ToString("00000000"));
        }

        private void ShowWeatherProviderData(object s)
        {
            WeatherResponse wr = (WeatherResponse)s;

            this.feelsLike.Text = ((int)wr.WeatherObservation.FeelLikeF).ToString();
            this.feelslikeDecimal.Text = (wr.WeatherObservation.FeelLikeF % 1.0 * 10.0).ToString("0");
        }

        private void LoadSonosData(object d)
        {
            SonosPlayingData lastData = null;

            while (true)
            {
                try
                {
                    SonosPlayingData data = GetPlayingData();

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

        private SonosPlayingData GetPlayingData()
        {
            string soapRequestTemplate = "<s:Envelope s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body>{0}</s:Body></s:Envelope>";
            XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace uns = "urn:schemas-upnp-org:service:AVTransport:1";

            string zoneName = "Kitchen";
            string zoneIp = "192.168.1.67";
            string coordinatorIp = GetCoordinator(zoneName, zoneIp);

            HttpWebRequest statRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://{0}:1400/MediaRenderer/AVTransport/Control", coordinatorIp));
            statRequest.ContentType = "text/xml";
            statRequest.Method = "POST";
            statRequest.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:AVTransport:1#GetTransportInfo\"");

            string soapStatus = string.Format(soapRequestTemplate, "<u:GetTransportInfo xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID><Channel>Master</Channel></u:GetTransportInfo>");
            byte[] byteStatus = Encoding.UTF8.GetBytes(soapStatus);
            statRequest.ContentLength = byteStatus.Length;
            using (Stream s = statRequest.GetRequestStream())
            {
                s.Write(byteStatus, 0, byteStatus.Length);
                s.Close();
            }

            HttpWebResponse statResponse = (HttpWebResponse)statRequest.GetResponse();
            using (Stream s = statResponse.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    string statusXml = sr.ReadToEnd();
                    XDocument status = XDocument.Parse(statusXml);

                    XElement body = status.Descendants(ns + "Body").First();
                    XElement transportInfo = body.Descendants(uns + "GetTransportInfoResponse").First().Descendants("CurrentTransportState").First();
                    if (transportInfo.Value.ToLower() != "playing" && transportInfo.Value.ToLower() != "paused_playback")
                    {
                        return new SonosPlayingData();
                    }
                }
            }

            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://{0}:1400/MediaRenderer/AVTransport/Control", coordinatorIp));
            webRequest.ContentType = "text/xml";
            webRequest.Method = "POST";
            webRequest.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:AVTransport:1#GetPositionInfo\"");
            
            string soap = string.Format(soapRequestTemplate, "<u:GetPositionInfo xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID><Channel>Master</Channel></u:GetPositionInfo>");

            byte[] byteArray = Encoding.UTF8.GetBytes(soap);
            webRequest.ContentLength = byteArray.Length;
            Stream stream = webRequest.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseString = reader.ReadToEnd();
                XDocument doc = XDocument.Parse(responseString);
                

                XElement body = doc.Descendants(ns + "Body").First();
                XElement trackMetaData = body.Descendants(uns + "GetPositionInfoResponse").First().Descendants("TrackMetaData").First();

                string xml = HttpUtility.UrlDecode(trackMetaData.Value);
                if (trackMetaData.Value == "NOT_IMPLEMENTED")
                {
                    return new SonosPlayingData();
                }
                else
                {
                    XDocument playing = XDocument.Parse(xml.Replace(" & ", " &amp; "));
                    XNamespace nsupnp = "urn:schemas-upnp-org:metadata-1-0/upnp/";
                    XNamespace nsr = "urn:schemas-rinconnetworks-com:metadata-1-0/";
                    XNamespace nsdc = "http://purl.org/dc/elements/1.1/";

                    XElement item = playing.Elements().First();

                    XElement albumArtUri = item.Descendants(nsupnp + "albumArtURI").FirstOrDefault();
                    XElement title = item.Descendants(nsdc + "title").FirstOrDefault();
                    XElement album = item.Descendants(nsupnp + "album").FirstOrDefault();
                    XElement artist = item.Descendants(nsr + "albumArtist").FirstOrDefault();

                    SonosPlayingData data = new SonosPlayingData();
                    if (albumArtUri != null)
                    {
                        data.AlbumArtUri = string.Format("http://{0}:1400{1}", coordinatorIp, albumArtUri.Value);
                    }
                    if (title != null)
                    {
                        data.Title = title.Value;
                    }
                    if (album != null)
                    {
                        data.Album = album.Value;
                    }
                    if (artist != null)
                    {
                        data.Artist = artist.Value;
                    }
                    return data;
                }
            }
        }

        private string GetCoordinator(string zoneName, string zoneIp)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://" + zoneIp + ":1400/status/topology");
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            using (Stream s = res.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    string xml = sr.ReadToEnd();
                    XDocument topo = XDocument.Parse(xml);

                    XElement players = topo.Root.Element("ZonePlayers");
                    var target = players.Elements("ZonePlayer").Where(p => p.Value.ToLower() == zoneName.ToLower());
                    if (target == null)
                    {
                        throw new Exception(string.Format("Unable to find Sonos player '{0}' listed in Sonos topology.", zoneName, zoneIp));
                    }
                    if (target.Attributes("coordinator").Where(a => a.Value == "true").Count() == 0)
                    {
                        string groupName = target.Attributes("group").First().Value;

                        var newTarget = players.Elements("ZonePlayer").Where(p => p.Attribute("group").Value == groupName && p.Attribute("coordinator").Value == "true").First();

                        // Location format: http://192.168.1.69:1400/xml/device_description.xml
                        string location = newTarget.Attribute("location").Value;
                        int startIndex = location.IndexOf("://");
                        int endIndex = location.IndexOf(":1400");
                        zoneIp = location.Substring(startIndex + 3, endIndex - startIndex - 3);
                        return zoneIp;
                    }
                    else
                    {
                        throw new Exception("Unable to find coordinator for Sonos zone " + zoneName);
                    }
                }
            }
        }

        string GetTrend(Module module)
        {
            if (module.DashboardData.TempTrend == "up")
            {
                return "↑";
            }
            else if (module.DashboardData.TempTrend == "down")
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
            }
        }

        protected override void OnKeyUp(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyUp(e);

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
                    Thread.Sleep(5000);
                    File.Delete(sender.ToString());
                }
                catch (Exception)
                {
                }
            }
        }
    }

    public class SonosPlayingData
    {
        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string AlbumArtUri { get; set; }

        public override bool Equals(object obj)
        {
            SonosPlayingData data = obj as SonosPlayingData;
            if (data == null)
            {
                return false;
            }

            return this.Title == data.Title && this.Album == data.Album && this.Artist == data.Artist && this.AlbumArtUri == data.AlbumArtUri;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
