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

namespace giesler.org.lists
{
    public partial class StoreItemList : UserControl
    {
        public StoreItemList()
        {
            InitializeComponent();
        }

        public void Load(IEnumerable<svc1.ShoppingListItem> items)
        {
            foreach (svc1.ShoppingListItem item in items)
            {
                StoreItem display = new StoreItem { Item = item };
                display.DataContext = item;
                display.OnRemove += new EventHandler(display_OnRemove);
                this.list.Items.Add(display);
            }
        }

        void display_OnRemove(object sender, EventArgs e)
        {
            StoreItem item = (StoreItem)sender;

            svc1.ShoppingListServiceClient svc = new svc1.ShoppingListServiceClient();
            svc.DeleteShoppingListItemAsync(item.Item, item);
            svc.DeleteShoppingListItemCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(svc_DeleteShoppingListItemCompleted);
            // BUGBUG: should keep in list until complete

            item.IsEnabled = false;
        }

        void svc_DeleteShoppingListItemCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            svc1.ShoppingListServiceClient svc = (svc1.ShoppingListServiceClient)sender;
            StoreItem item = (StoreItem)e.UserState;

            if (e.Error == null)
            {
                this.list.Items.Remove(item);
            }
            else
            {
                item.IsEnabled = true;
                MessageBox.Show(e.Error.Message, "Delete error", MessageBoxButton.OK);
            }

            svc.CloseAsync();
        }

    }
}
