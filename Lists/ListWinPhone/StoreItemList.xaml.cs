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
    public partial class StoreItemList : UserControl
    {
        public event DeleteListItemEventHandler DeleteListItem;

        public StoreItemList()
        {
            InitializeComponent();
        }

        public void Load(IEnumerable<ListItemEx> items)
        {
            foreach (ListItemEx item in items)
            {
                StoreItem display = new StoreItem { Item = item };
                display.DataContext = item;
                display.OnRemove += new EventHandler(display_OnRemove);
                this.list.Items.Add(display);
            }
        }

        public void ToggleEnabled(bool isEnabled)
        {
            foreach (StoreItem item in this.list.Items)
            {
                item.IsEnabled = isEnabled;
            }
        }

        void display_OnRemove(object sender, EventArgs e)
        {
            StoreItem item = (StoreItem)sender;

            if (this.DeleteListItem != null)
            {
                this.DeleteListItem(item.Item);
                this.list.Items.Remove(item);
            }
        }
    }

    public delegate void DeleteListItemEventHandler(ListItem item);
}
