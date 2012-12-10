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
        int lastStoreIndex = 0;

        public Page()
        {
            InitializeComponent();

            client.GetStoresCompleted += new EventHandler<GetStoresCompletedEventArgs>(client_GetStoresCompleted);
            client.GetStoresAsync();

            this.debug.Visibility = Visibility.Collapsed;
            this.debugRow.Height = GridLength.Auto;
        }

        void client_GetStoresCompleted(object sender, GetStoresCompletedEventArgs e)
        {
            client.GetStoresCompleted -= new EventHandler<GetStoresCompletedEventArgs>(client_GetStoresCompleted);

            var q = from i in e.Result select new StoreListItem { Store = i };

            this.stores = e.Result;
            this.storeList.ItemsSource = q;
            this.storeList.SelectedIndex = 0;

            this.LoadItems();
        }

        private void storeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Log("StoreSelectionChanged: " + this.storeList.SelectedIndex.ToString());

            if (this.storeList.SelectedIndex >= 0)
            {
                this.lastStoreIndex = this.storeList.SelectedIndex;
                this.LoadItems();
            }
        }

        void LoadItems()
        {
            if (this.lastStoreIndex == 0)
            {
                this.Log("Defaulting to store 0");
                this.storeList.SelectedIndex = 0;
            }
            if (this.lastStoreIndex >= 0)
            {
                if (this.loadingItems == false)
                {
                    StoreListItem item = (StoreListItem)this.storeList.Items[this.lastStoreIndex];
                    this.Log("Loading {0}", item.Store);

                    this.client.GetShoppingListItemsCompleted += new EventHandler<GetShoppingListItemsCompletedEventArgs>(client_GetShoppingListItemsCompleted);
                    this.client.GetShoppingListItemsAsync(item.Store);

                    this.items.Visibility = Visibility.Collapsed;
                    this.loading.Visibility = Visibility.Visible;
                }
            }
            else
            {
                this.Log("No store selected????");
            }
        }

        void client_GetShoppingListItemsCompleted(object sender, GetShoppingListItemsCompletedEventArgs e)
        {
            this.client.GetShoppingListItemsCompleted -= new EventHandler<GetShoppingListItemsCompletedEventArgs>(client_GetShoppingListItemsCompleted);

            StoreListItem item = (StoreListItem)this.storeList.Items[this.lastStoreIndex]; 
            string loadedStore = e.UserState.ToString();

            this.Log("Load complete for " + loadedStore);

            if (loadedStore.Equals(item.Store, StringComparison.InvariantCultureIgnoreCase))
            {
                this.loadingItems = true;

                this.items.ItemsSource = e.Result.Where(r => r.Store.Equals(item.Store, StringComparison.InvariantCultureIgnoreCase));

                int selectedIndex = this.storeList.SelectedIndex;
                this.Log("Saved index: " + selectedIndex.ToString());

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

                this.Log("Restored index: " + this.storeList.SelectedIndex.ToString());

                this.items.Visibility = Visibility.Visible;
                this.loading.Visibility = Visibility.Collapsed;

                this.loadingItems = false;
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            StoreListItem item = (StoreListItem)this.storeList.Items[this.lastStoreIndex]; 
            
            client.AddShoppingListItemAsync(item.Store, this.addText.Text.Trim(), item);
            client.AddShoppingListItemCompleted += new EventHandler<AddShoppingListItemCompletedEventArgs>(client_AddShoppingListItemCompleted);

            this.Log("Adding " + this.addText.Text);

            this.addText.Text = string.Empty;
        }

        void client_AddShoppingListItemCompleted(object sender, AddShoppingListItemCompletedEventArgs e)
        {
            this.Log("Add completed for " + e.Result.ListItem);
            client.AddShoppingListItemCompleted -= new EventHandler<AddShoppingListItemCompletedEventArgs>(client_AddShoppingListItemCompleted);

            if (e.Cancelled == false)
            {
                this.LoadItems();
            }
        }

        private void addText_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.add.IsEnabled = this.addText.Text.Trim().Length > 0;
        }

        private void bulkadd_Click(object sender, RoutedEventArgs e)
        {
        }

        void client_DeleteShoppingListItemCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this.client.DeleteShoppingListItemCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_DeleteShoppingListItemCompleted);
            ShoppingListItem item = (ShoppingListItem)e.UserState;

            this.LoadItems();
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

        private void addText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.add_Click(null, null);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton checkBox = (HyperlinkButton)sender;
            ShoppingListItem item = (ShoppingListItem)checkBox.DataContext;
            checkBox.IsEnabled = false;

            this.client.DeleteShoppingListItemCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_DeleteShoppingListItemCompleted);
            this.client.DeleteShoppingListItemAsync(item, item);
        }

        bool alternate = false;

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = (Grid)sender;

            if (alternate)
            {
                //GradientStopCollection stops = new GradientStopCollection();
                //stops.Add(new GradientStop() { Offset = 0, Color = Color.FromArgb(0, 232, 247, 247) });
                //stops.Add(new GradientStop() { Offset = 100, Color = Color.FromArgb(0, 200, 200, 200) });
                //LinearGradientBrush brush = new LinearGradientBrush(stops, 90);
                grid.Background = new SolidColorBrush(Color.FromArgb(0, 232, 247, 247));
            }
            else
            {
            }

            alternate = !alternate;
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
