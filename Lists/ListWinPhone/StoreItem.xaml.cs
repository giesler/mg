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
using System.Threading;
using giesler.org.lists.ListData;
using System.Diagnostics;

namespace giesler.org.lists
{
    public partial class StoreItem : UserControl
    {
        private Timer timer = null;

        public event EventHandler OnRemove;
        private DateTime checkTime;
        private Point mouseDownPosition;

        public StoreItem()
        {
            InitializeComponent();
        }
        
        public ListItem Item { get; set; }

        private void itemCheck_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox itemCheck = (CheckBox)sender;
            Grid grid = (Grid)itemCheck.Parent;
            TextBlock tb = (TextBlock)grid.Children[1];

            tb.Foreground = (SolidColorBrush)Application.Current.Resources["PhoneDisabledBrush"];
            this.timer = new Timer(new TimerCallback(OnTimer), itemCheck, 250, 250);
            this.checkTime = DateTime.Now;

            this.progressBar1.Visibility = Visibility.Visible;
        }

        void OnTimer(object sender)
        {
            this.Dispatcher.BeginInvoke(new WaitCallback(this.OnTimerUI), sender);
        }

        void OnTimerUI(object sender)
        {
            if (this.itemCheck.IsChecked.Value)
            {
                TimeSpan ts = DateTime.Now - this.checkTime;
                if (ts.TotalSeconds < 5)
                {
                    double progress = ts.TotalMilliseconds / 100.0;
                    this.progressBar1.Value = progress;
                }
                else
                {
                    this.progressBar1.Visibility = Visibility.Collapsed;

                    CheckBox itemCheck = (CheckBox)sender;

                    ListItemEx item = (ListItemEx)itemCheck.Tag;
                    App.Lists.FirstOrDefault(l => l.UniqueId == item.ListUniqueId).Items.Remove(item);

                    if (this.OnRemove != null)
                    {
                        this.OnRemove(this, EventArgs.Empty);
                    }

                    if (this.timer != null)
                    {
                        this.timer.Dispose();
                        this.timer = null;
                    }
                }
            }
        }

        private void itemCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox itemCheck = (CheckBox)sender;
            Grid grid = (Grid)itemCheck.Parent;
            TextBlock tb = (TextBlock)grid.Children[1];

            tb.Foreground = (SolidColorBrush)Application.Current.Resources["PhoneForegroundBrush"];

            if (this.timer != null)
            {
                this.timer.Dispose();
                this.timer = null;
            }

            this.progressBar1.Visibility = System.Windows.Visibility.Collapsed;
            this.progressBar1.Value = 0;
        }

        private void OnLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.mouseDownPosition = e.GetPosition(this);
        }

        private void OnLeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.itemCheck.IsEnabled)
            {
                Point mouseUpPosition = e.GetPosition(this);
                double xdiff = Math.Abs((double)mouseDownPosition.X - (double)mouseUpPosition.X);
                double ydiff = Math.Abs((double)mouseDownPosition.Y - (double)mouseUpPosition.Y);

                if (xdiff < 5 && ydiff < 5)
                {
                    this.itemCheck.IsChecked = !this.itemCheck.IsChecked.Value;
                }
                else
                {
                    Debug.WriteLine("Skipped: " + xdiff.ToString() + ", " + ydiff.ToString());
                }
            }
        }
    }
}
