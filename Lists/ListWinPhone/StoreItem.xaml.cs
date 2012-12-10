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

namespace giesler.org.lists
{
    public partial class StoreItem : UserControl
    {
        private Timer timer = null;
        
        public event EventHandler OnRemove;
        private DateTime checkTime;

        public StoreItem()
        {
            InitializeComponent();
        }
        
        public ListItem Item { get; set; }

        private void itemCheck_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox itemCheck = (CheckBox)sender;
            StackPanel sp = (StackPanel)itemCheck.Parent;
            TextBlock tb = (TextBlock)sp.Children[1];

            tb.Foreground = new SolidColorBrush(Colors.Gray);
            this.timer = new Timer(new TimerCallback(ItemCheckComplete), itemCheck, 1000 * 5, Timeout.Infinite);
            this.checkTime = DateTime.Now;
        }

        void ItemCheckComplete(object sender)
        {
            this.Dispatcher.BeginInvoke(new WaitCallback(this.ItemCheckCompleteUI), sender);
        }

        void ItemCheckCompleteUI(object sender)
        {
            CheckBox itemCheck = (CheckBox)sender;

            if (itemCheck.IsChecked.Value && checkTime.AddSeconds(5) < DateTime.Now)
            {
                ListItemEx item = (ListItemEx)itemCheck.Tag;
                App.Lists.FirstOrDefault(l => l.UniqueId == item.ListUniqueId).Items.Remove(item);

                if (this.OnRemove != null)
                {
                    this.OnRemove(this, EventArgs.Empty);
                }
            }

            this.timer.Dispose();
        }

        private void itemCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox itemCheck = (CheckBox)sender;
            StackPanel sp = (StackPanel)itemCheck.Parent;
            TextBlock tb = (TextBlock)sp.Children[1];

            tb.Foreground = new SolidColorBrush(Colors.White);
        }
    }
}
