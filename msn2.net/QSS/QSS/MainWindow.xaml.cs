using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Input;

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
        
        Timer timer = null;
        int interval = 6;
        bool loaded = false;
        Point? lastMousePoint = null;

        public MainWindow()
        {
            InitializeComponent();

            this.Path = @"c:\nr";
            ThreadPool.QueueUserWorkItem(this.LoadPics, null);

            this.Cursor = Cursors.None;
            this.ForceCursor = true;
        }

        void LoadPics(object sender)
        {
            string[] files =  Directory.GetFiles(this.Path, "*.jpg", SearchOption.AllDirectories);
            this.Files = new List<string>();

            List<string> random = new List<string>();
            foreach (string file in files)
            {
                random.Add(file);
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

            this.loaded = true;
        }

        void OnTimer(object state)
        {
            this.Dispatcher.Invoke(new TimerCallback(this.DisplayNextPicture), new object());
        }

        void DisplayNextPicture(object sender)
        {
            this.status.Visibility = Visibility.Collapsed;

            this.CurrentIndex++;

            this.img.Source = new BitmapImage(new Uri(this.Files[this.CurrentIndex]));
        }

        protected override void OnKeyUp(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == System.Windows.Input.Key.Right)
            {
                this.CreateTimer(true);
            }
            else if (e.Key == System.Windows.Input.Key.Left)
            {
                if (this.CurrentIndex > 0)
                {
                    this.CurrentIndex--;
                    this.CurrentIndex--;
                }
                this.CreateTimer(true);
            }
            else if (e.Key == System.Windows.Input.Key.Up)
            {
                this.interval++;
                this.CreateTimer(false);
            }
            else if (e.Key == System.Windows.Input.Key.Down && this.interval > 1)
            {
                this.interval--;
                this.CreateTimer(false);
            }
            else if (e.Key == System.Windows.Input.Key.Delete && this.CurrentIndex >= 0)
            {
                this.DeletePic(this.Files[this.CurrentIndex]);
                this.CreateTimer(true);
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
