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
using System.Diagnostics;
using System.Threading;

namespace giesler.org.lists
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool backgroundOperationActive = false;
        private bool loadedFromStorage = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            this.main.SelectionChanged += new SelectionChangedEventHandler(main_SelectionChanged);
        }

        void main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PivotItem item = this.main.SelectedItem as PivotItem;
            List list = item.Tag as List;

            if (list != null)
            {
                App.SelectedList = list.UniqueId;
                App.Current.SaveSettings();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.Current.LoadAll();
        }

        private void DisplayLoadedLists(List<List> lists)
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

                if (list.UniqueId == App.SelectedList)
                {
                    selectIndex = cur;
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

            this.LoadItems(App.Items);

            if (this.loadedFromStorage == true)
            {
                this.refreshButton_Click(null, null);
                this.loadedFromStorage = false;
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.SaveLists), new object());
            }
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
            ListDataServiceClient svc = App.DataProvider;
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

            ListDataServiceClient client = (ListDataServiceClient)sender;
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

            this.pbar.Visibility = loading ? Visibility.Visible : Visibility.Collapsed;

            if (this.ApplicationBar != null && this.ApplicationBar.Buttons.Count > 0)
            {
                ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = !loading;
                ((IApplicationBarIconButton)this.ApplicationBar.Buttons[1]).IsEnabled = this.main.Items.Count > 1;
                ((IApplicationBarIconButton)this.ApplicationBar.Buttons[2]).IsEnabled = !loading;
                //((IApplicationBarIconButton)this.ApplicationBar.Buttons[3]).IsEnabled = !loading;
            }

            foreach (PivotItem item in this.main.Items)
            {
                StoreItemList list = item.Content as StoreItemList;
                if (list != null)
                {
                    list.ToggleEnabled(loading == false);                    
                }
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            string msg = string.Format("MainPage_Loaded: App.Lists=null? {0}, App.AttemptedStorageLoad={1}", App.Lists == null, App.AttemptedStorageLoad);
            Debug.WriteLine(msg);

            this.updatingMessage.Visibility = System.Windows.Visibility.Visible;
            
            if (!App.AttemptedStorageLoad)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadFromStorage), new object());
                this.ToggleBackgroundOperationStatus(true);

                this.updatingMessage.Text = "Loading...";

#if DEBUG
                this.updatingMessage.Text = "Loading from storage...";
#endif
            }
            else if (App.AuthData == null || App.AuthData.PersonUniqueId == Guid.Empty || App.AuthData.DeviceUniqueId == Guid.Empty)
            {
                NavigationService.Navigate(new Uri("/GetLiveIdPage.xaml", UriKind.Relative));
            }
            else if (App.Lists == null)
            {
                this.ReloadAll();

#if DEBUG
                this.updatingMessage.Text = "Loading from service...";
#endif
            }
            else
            {
                this.DisplayLoadedLists(App.Lists);
                this.updatingMessage.Visibility = System.Windows.Visibility.Collapsed;
            }

            this.UpdateControls();
        }

        void ReloadAll()
        {
            ListDataServiceClient svc = App.DataProvider;
            svc.GetAllCompleted += new EventHandler<GetAllCompletedEventArgs>(svc_GetAllCompleted);
            svc.GetAllAsync(App.AuthDataList);

            this.ToggleBackgroundOperationStatus(true);

            this.updatingMessage.Text = "Updating...";
            this.updatingMessage.Visibility = System.Windows.Visibility.Visible;
        }

        void svc_GetAllCompleted(object sender, GetAllCompletedEventArgs e)
        {
            this.updatingMessage.Visibility = System.Windows.Visibility.Collapsed;

            if (e.Error == null)
            {
                List<Guid> uniqueLists = new List<Guid>();
                List<List> lists = new List<List>();
                List<ListItemEx> items = new List<ListItemEx>();
                foreach (var i in e.Result)
                {
                    if (uniqueLists.Contains(i.UniqueId) == false)
                    {
                        List list = new List { Name = i.Name, UniqueId = i.UniqueId };
                        uniqueLists.Add(list.UniqueId);
                        lists.Add(list);
                    }

                    if (i.ItemUniqueId.HasValue)
                    {
                        ListItemEx item = new ListItemEx { Name = i.ItemName, UniqueId = i.ItemUniqueId.Value, ListUniqueId = i.UniqueId };
                        items.Add(item);
                    }
                }

                App.Lists = lists;
                App.Items = items;

                DisplayLoadedLists(lists);
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }

            this.ToggleBackgroundOperationStatus(false);

            ListDataServiceClient svc = (ListDataServiceClient)sender;
            svc.CloseAsync();
        }

        void LoadFromStorage(object sender)
        {
            try
            {
                App.Current.LoadAll();
                this.loadedFromStorage = true;
            }
            catch (Exception ex)
            {
                // todo: log
                App.Lists = null;
                App.Items = null;
                Debug.WriteLine(ex.ToString());
            }

            App.AttemptedStorageLoad = true;

            Dispatcher.BeginInvoke(new RoutedEventHandler(this.MainPage_Loaded), null, null);
        }

        void SaveLists(object sender)
        {
            App.Current.SaveAll();
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

            this.ReloadAll();
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