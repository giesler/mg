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
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;

namespace giesler.org.lists
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void svc_GetStoresCompleted(object sender, svc1.GetStoresCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                LoadStores(e.Result);

                //this.jumpButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }

            svc1.ShoppingListServiceClient svc = (svc1.ShoppingListServiceClient)sender;
            svc.CloseAsync();
        }

        private void LoadStores(List<string> stores)
        {
            App.Stores = stores;
            int clearCount = this.main.Items.Count;

            PivotItem select = null;
            foreach (string store in stores)
            {
                PivotItem item = new PivotItem();
                item.Tag = store;
                item.Header = store.ToLowerInvariant();

                TextBlock blk = new TextBlock() { Text = "loading..." };
                item.Content = blk;

                this.main.Items.Add(item);

                if (NavigationContext.QueryString.ContainsKey("s"))
                {
                    if (item.Tag.ToString() == NavigationContext.QueryString["s"])
                    {
                        select = item;
                    }
                }
            }

            while (clearCount-- > 0)
            {
                this.main.Items.RemoveAt(clearCount);
            }

            if (select != null)
            {
                this.main.SelectedItem = select;
            }

            this.loading.Visibility = System.Windows.Visibility.Collapsed;

            if (App.Items == null)
            {
                svc1.ShoppingListServiceClient svc2 = new svc1.ShoppingListServiceClient();
                svc2.GetShoppingListItemsCompleted += new EventHandler<svc1.GetShoppingListItemsCompletedEventArgs>(svc2_GetShoppingListItemsCompleted);
                svc2.GetShoppingListItemsAsync();
            }
            else
            {
                this.LoadItems(App.Items);
            }
        }

        void svc2_GetShoppingListItemsCompleted(object sender, svc1.GetShoppingListItemsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                App.Items = e.Result;
                this.LoadItems(App.Items);
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }

            svc1.ShoppingListServiceClient svc = (svc1.ShoppingListServiceClient)sender;
            svc.CloseAsync();
        }

        private void LoadItems(List<svc1.ShoppingListItem> items)
        {
            foreach (PivotItem item in this.main.Items)
            {
                StoreItemList list = new StoreItemList();
                item.Content = list;

                var q = items.Where(i => i.Store.ToString() == item.Tag.ToString());
                list.Load(q);// t.DataContext = q;
            }

            foreach (ApplicationBarIconButton b in this.ApplicationBar.Buttons)
            {
                b.IsEnabled = true;
            }
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            if (App.Stores == null)
            {
                svc1.ShoppingListServiceClient svc = new svc1.ShoppingListServiceClient();
                svc.GetStoresAsync();
                svc.GetStoresCompleted += new EventHandler<svc1.GetStoresCompletedEventArgs>(svc_GetStoresCompleted);
            }
            else
            {
                this.LoadStores(App.Stores);
            }

        }

        private void jumpButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/jump.xaml", UriKind.Relative));
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            PivotItem item = (PivotItem)this.main.SelectedItem;
            string store = item.Tag.ToString();
            NavigationService.Navigate(new Uri("/Add.xaml?store=" + store, UriKind.Relative));
        }
    }
}