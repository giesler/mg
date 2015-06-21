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

namespace QSS
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
        int interval = 6;
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

        public MainWindow(string[] args)
        {
            InitializeComponent();

            this.Path = @"c:\nr";

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

            this.Cursor = Cursors.None;
            this.ForceCursor = true;

            this.videoTimer = new DispatcherTimer();
            this.videoTimer.Interval = TimeSpan.FromMilliseconds(500);
            this.videoTimer.Tick += new EventHandler(this.OnVideoTimerTick);

            this.loadCompleteCheck = new DispatcherTimer();
            this.loadCompleteCheck.Interval = TimeSpan.FromMilliseconds(200);
            this.loadCompleteCheck.Tick += new EventHandler(this.OnLoadCompleteCheck);
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
}
