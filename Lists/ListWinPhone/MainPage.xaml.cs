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
using giesler.org.lists.ListData;
using Microsoft.Phone.Info;

namespace giesler.org.lists
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool backgroundOperationActive = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void LoadLists(List<List> lists)
        {
            App.Lists = lists;
            int clearCount = this.main.Items.Count;

            int cur = -1;
            int selectIndex = -1;
            foreach (List list in lists)
            {
                cur++;

                PivotItem item = new PivotItem();
                item.Tag = list;
                item.Header = list.Name.ToLowerInvariant();

                TextBlock blk = new TextBlock() { Text = "loading..." };
                blk.Margin = new Thickness(12, 12, 0, 0);
                item.Content = blk;

                this.main.Items.Add(item);

                if (NavigationContext.QueryString.ContainsKey("listUniqueId"))
                {
                    if (list.UniqueId.ToString() == NavigationContext.QueryString["listUniqueId"])
                    {
                        selectIndex = cur;
                    }
                }
            }

            while (clearCount-- > 0)
            {
                this.main.Items.RemoveAt(clearCount);
            }

            if (selectIndex >= 0)
            {
                this.main.SelectedIndex = selectIndex;
            }

            this.loading.Visibility = System.Windows.Visibility.Collapsed;

            if (App.Items == null)
            {
                IListDataProvider svc2 = App.DataProvider;
                svc2.GetAllListItemsCompleted += new EventHandler<GetAllListItemsCompletedEventArgs>(svc2_GetAllListItemsCompleted);
                svc2.GetAllListItemsAsync(App.AuthDataList);
                this.ToggleBackgroundOperationStatus(true);
            }
            else
            {
                this.LoadItems(App.Items);
            }
        }

        void svc2_GetAllListItemsCompleted(object sender, GetAllListItemsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<ListItemEx> items = new List<ListItemEx>();
                foreach (var i in e.Result)
                {
                    items.Add(new ListItemEx { UniqueId = i.UniqueId, Name = i.Name, ListUniqueId = i.ListUniqueId });
                }

                App.Items = items;
                this.LoadItems(App.Items);
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }

            this.ToggleBackgroundOperationStatus(false);

            IListDataProvider svc = (IListDataProvider)sender;
            svc.CloseAsync();
        }

        private void LoadItems(List<ListItemEx> items)
        {
            foreach (PivotItem item in this.main.Items)
            {
                StoreItemList listControl = new StoreItemList();
                item.Content = listControl;
                listControl.DeleteListItem += new DeleteListItemEventHandler(listControl_DeleteListItem);
                
                List list = item.Tag as List;

                var q = items.Where(i => i.ListUniqueId == list.UniqueId).OrderBy(i => i.Name);
                listControl.Load(q);
            }

            this.UpdateControls();
        }

        void listControl_DeleteListItem(ListItem item)
        {
            IListDataProvider svc = App.DataProvider;
            svc.DeleteListItemCompleted += new EventHandler<DeleteListItemCompletedEventArgs>(svc_DeleteListItemCompleted);
            svc.DeleteListItemAsync(App.AuthDataList, item.UniqueId);

            this.ToggleBackgroundOperationStatus(true);
        }

        void svc_DeleteListItemCompleted(object sender, DeleteListItemCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Delete Error", MessageBoxButton.OK);
            }

            this.ToggleBackgroundOperationStatus(false);

            IListDataProvider client = (IListDataProvider)sender;
            client.CloseAsync();
        }

        void ToggleBackgroundOperationStatus(bool isActive)
        {
            this.backgroundOperationActive = isActive;
            this.UpdateControls();
        }

        void UpdateControls()
        {
            bool loading = App.Items == null || App.Lists == null || this.backgroundOperationActive == true;

            if (this.ApplicationBar != null && this.ApplicationBar.Buttons.Count > 0)
            {
                ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = !loading;
                ((IApplicationBarIconButton)this.ApplicationBar.Buttons[1]).IsEnabled = !loading;
                ((IApplicationBarIconButton)this.ApplicationBar.Buttons[2]).IsEnabled = !loading;
                //((IApplicationBarIconButton)this.ApplicationBar.Buttons[3]).IsEnabled = !loading;
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.AuthData == null || App.AuthData.PersonUniqueId == Guid.Empty || App.AuthData.DeviceUniqueId == Guid.Empty)
            {
                NavigationService.Navigate(new Uri("/GetLiveIdPage.xaml", UriKind.Relative));
            }
            else if (App.Lists == null)
            {
                IListDataProvider svc = App.DataProvider;
                svc.GetListsCompleted += new EventHandler<GetListsCompletedEventArgs>(svc_GetListsCompleted);
                svc.GetListsAsync(App.AuthDataList);

                this.ToggleBackgroundOperationStatus(true);
            }
            else
            {
                this.LoadLists(App.Lists);
            }

            this.UpdateControls();
        }

        void svc_GetListsCompleted(object sender, GetListsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<List> lists = new List<List>();
                foreach (var i in e.Result)
                {
                    lists.Add(new List { Name = i.Name, UniqueId = i.UniqueId });
                }
                LoadLists(lists);
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }

            this.ToggleBackgroundOperationStatus(false);

            IListDataProvider svc = (IListDataProvider)sender;
            svc.CloseAsync();
        }

        private void jumpButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/jump.xaml", UriKind.Relative));
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            PivotItem item = (PivotItem)this.main.SelectedItem;
            List list = (List)item.Tag;
            
            NavigationService.Navigate(new Uri("/Add.xaml?listUniqueId=" + list.UniqueId, UriKind.Relative));
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            App.Lists = null;
            App.Items = null;

            this.MainPage_Loaded(null, null);
        }

        private void aboutMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void debug_Click(object sender, EventArgs e)
        {
            long deviceTotalMemory = (long)DeviceExtendedProperties.GetValue("DeviceTotalMemory");
            long applicationCurrentMemoryUsage = (long)DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage");
            long applicationPeakMemoryUsage = (long)DeviceExtendedProperties.GetValue("ApplicationPeakMemoryUsage");

            string msg = string.Format("Device: {0}{1}Current: {2}{1}Peak: {3}",
                deviceTotalMemory / 1024 / 1024, Environment.NewLine, applicationCurrentMemoryUsage / 1024 / 1024, applicationPeakMemoryUsage / 1024 / 1024);
            MessageBox.Show(msg, "Debug Info", MessageBoxButton.OK);
        }
    }
}