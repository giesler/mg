using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLExpressControls.msn2.net.ShoppingList.ShoppingListService;

namespace SLExpressControls
{
    public partial class Page : UserControl
    {
        ShoppingListServiceClient client = new ShoppingListServiceClient();
        List<string> stores = null;
        bool loadingItems = false;

        public Page()
        {
            InitializeComponent();

            client.GetStoresCompleted += new EventHandler<GetStoresCompletedEventArgs>(client_GetStoresCompleted);
            client.GetStoresAsync();
        }

        void client_GetStoresCompleted(object sender, GetStoresCompletedEventArgs e)
        {
            client.GetStoresCompleted -= new EventHandler<GetStoresCompletedEventArgs>(client_GetStoresCompleted);

            var q = from i in e.Result select new StoreListItem { Store = i };

            this.stores = e.Result;
            this.storeList.ItemsSource = q;
            this.storeList.SelectedItem = this.storeList.Items[0];

            this.LoadItems();
        }

        private void storeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadItems();
        }

        void LoadItems()
        {
            if (this.storeList.SelectedIndex >= 0)
            {
                if (this.loadingItems == false)
                {
                    StoreListItem item = (StoreListItem)this.storeList.SelectedItem;
                    this.Log("Loading {0}", item.Store);

                    this.client.GetShoppingListItemsCompleted += new EventHandler<GetShoppingListItemsCompletedEventArgs>(client_GetShoppingListItemsCompleted);
                    this.client.GetShoppingListItemsAsync(item.Store);

                    this.items.Visibility = Visibility.Collapsed;
                    this.loading.Visibility = Visibility.Visible;
                }
            }
        }

        void client_GetShoppingListItemsCompleted(object sender, GetShoppingListItemsCompletedEventArgs e)
        {
            this.client.GetShoppingListItemsCompleted -= new EventHandler<GetShoppingListItemsCompletedEventArgs>(client_GetShoppingListItemsCompleted);

            StoreListItem selectedStore = (StoreListItem)this.storeList.SelectedItem;
            string loadedStore = e.UserState.ToString();

            this.Log("Load complete for " + loadedStore);

            if (loadedStore.Equals(selectedStore.Store, StringComparison.InvariantCultureIgnoreCase))
            {
                this.loadingItems = true;

                this.items.ItemsSource = e.Result.Where(r => r.Store.Equals(selectedStore.Store, StringComparison.InvariantCultureIgnoreCase));

                int selectedIndex = this.storeList.SelectedIndex;

                List<StoreListItem> itemList = new List<StoreListItem>();
                foreach (string store in this.stores)
                {
                    var q = from i in e.Result
                            where i.Store.Equals(store, StringComparison.InvariantCultureIgnoreCase)
                            select i;

                    itemList.Add(new StoreListItem { Store = store, ItemCount = q.Count() });
                }
                
                this.storeList.ItemsSource = itemList;
                this.storeList.SelectedIndex = selectedIndex;

                this.items.Visibility = Visibility.Visible;
                this.loading.Visibility = Visibility.Collapsed;

                this.loadingItems = false;
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            client.AddShoppingListItemAsync(this.storeList.SelectedItem.ToString(), this.addText.Text.Trim(), this.storeList.SelectedItem);
            client.AddShoppingListItemCompleted += new EventHandler<AddShoppingListItemCompletedEventArgs>(client_AddShoppingListItemCompleted);
        }

        void client_AddShoppingListItemCompleted(object sender, AddShoppingListItemCompletedEventArgs e)
        {
            client.AddShoppingListItemCompleted -= new EventHandler<AddShoppingListItemCompletedEventArgs>(client_AddShoppingListItemCompleted);

            if (e.Cancelled == false)
            {
                if (e.UserState.ToString() == this.storeList.SelectedItem.ToString())
                {
                    // ADD TO LIST
                }
            }
        }

        private void addText_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.add.IsEnabled = this.addText.Text.Trim().Length > 0;
        }

        private void bulkadd_Click(object sender, RoutedEventArgs e)
        {

        }

        public void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            this.debug.Text += string.Format("{0:00}:{1:00}:{2:00}: {3}{4}", 
                DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                message, Environment.NewLine);
            this.debug.SelectionStart = this.debug.Text.Length;
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton link = (HyperlinkButton)sender;
            this.editor.DataContext = link.DataContext;

            this.editor.Visibility = Visibility.Visible;
        }
    }

    public class StoreListItem
    {
        public string Store { get; set; }
        public int ItemCount { get; set; }

        public override string ToString()
        {
            if (this.ItemCount > 0)
            {
                return string.Format("{0} ({1})", this.Store, this.ItemCount);
            }
            else
            {
                return this.Store;
            }
        }
    }
}
