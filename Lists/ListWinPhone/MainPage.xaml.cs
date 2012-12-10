﻿using System;
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
using System.Reflection;
using System.Text;

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

        public bool AttemptedStorageLoad { get; set; }

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

        private void DisplayLoadedLists(List<ListEx> lists)
        {
            App.Lists = lists;
            int clearCount = this.main.Items.Count;

            int cur = -1;
            int selectIndex = -1;
            foreach (ListEx list in lists)
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

            foreach (PivotItem item in this.main.Items)
            {
                StoreItemList listControl = new StoreItemList();
                item.Content = listControl;
                listControl.DeleteListItem += new DeleteListItemEventHandler(listControl_DeleteListItem);

                ListEx list = item.Tag as ListEx;

                var q = list.Items.OrderBy(i => i.Name);
                listControl.Load(q);
            }

            this.UpdateControls();

            if (this.loadedFromStorage == true && App.LastRefreshTime.AddMinutes(30) < DateTime.Now)
            {
                this.refreshButton_Click(null, null);
                this.loadedFromStorage = false;
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.SaveLists), new object());
            }
        }

        void listControl_DeleteListItem(ListItem item)
        {
            ListDataServiceClient svc = App.DataProvider;
            svc.DeleteListItemCompleted += new EventHandler<DeleteListItemCompletedEventArgs>(svc_DeleteListItemCompleted);
            svc.DeleteListItemAsync(App.AuthDataList, item.UniqueId);

            this.updatingMessage.Text = "Deleting...";
            this.updatingMessage.Visibility = Visibility.Visible;

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
            bool loading = App.Lists == null || this.backgroundOperationActive == true;

            this.pbar.Visibility = loading ? Visibility.Visible : Visibility.Collapsed;
            if (loading && this.updatingMessage.Visibility == System.Windows.Visibility.Collapsed)
            {
                // TODO: This shouldn't run...
                //ShowStack();
            }

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

        private static void ShowStack()
        {
            StackTrace st = new StackTrace();
            StackFrame[] sf = st.GetFrames();
            StringBuilder sb = new StringBuilder();
            foreach (var s in sf)
            {
                sb.AppendLine(s.GetMethod().DeclaringType.FullName + "." + s.GetMethod().Name);
            }
            MessageBox.Show(sb.ToString());
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.Current.LoadAll();

            //string msg = string.Format("MainPage_Loaded: App.Lists=null? {0}, App.AttemptedStorageLoad={1}", App.Lists == null, App.AttemptedStorageLoad);
            //MessageBox.Show(msg);

            if (App.Lists != null && App.Lists.Count > 0)
            {
                this.DisplayLoadedLists(App.Lists);
            }
            if (App.Lists.Count == 0)
            {
                this.ReloadAll();

                this.updatingMessage.Text = "Loading from service...";
                this.updatingMessage.Visibility = Visibility.Visible;
            }
            //else if (App.AuthData == null || App.AuthData.PersonUniqueId == Guid.Empty || App.AuthData.DeviceUniqueId == Guid.Empty)
            //{
            //    NavigationService.Navigate(new Uri("/GetLiveIdPage.xaml", UriKind.Relative));
            //}
            else
            {
                this.updatingMessage.Visibility = Visibility.Collapsed;
            }

            this.UpdateControls();
        }

        void ReloadAll()
        {
            //ShowStack();

            ListDataServiceClient svc = App.DataProvider;
            svc.GetAllCompleted += new EventHandler<GetAllCompletedEventArgs>(svc_GetAllCompleted);
            svc.GetAllAsync(App.AuthDataList);

            this.updatingMessage.Text = "Updating...";
            this.updatingMessage.Visibility = System.Windows.Visibility.Visible;

            this.ToggleBackgroundOperationStatus(true);
        }

        void svc_GetAllCompleted(object sender, GetAllCompletedEventArgs e)
        {
            this.updatingMessage.Visibility = System.Windows.Visibility.Collapsed;
            this.ToggleBackgroundOperationStatus(false);

            if (e.Error == null)
            {
                List<Guid> uniqueLists = new List<Guid>();
                List<ListEx> lists = new List<ListEx>();
                foreach (var resultItem in e.Result)
                {
                    if (uniqueLists.Contains(resultItem.UniqueId) == false)
                    {
                        ListEx list = new ListEx { Name = resultItem.Name, UniqueId = resultItem.UniqueId };
                        uniqueLists.Add(list.UniqueId);
                        lists.Add(list);
                    }

                    if (resultItem.ItemUniqueId.HasValue)
                    {
                        ListItemEx item = new ListItemEx { Name = resultItem.ItemName, UniqueId = resultItem.ItemUniqueId.Value, ListUniqueId = resultItem.UniqueId };
                        lists.FirstOrDefault(l => l.UniqueId == resultItem.UniqueId).Items.Add(item);
                    }
                }

                App.Lists = lists;
                
                App.LastRefreshTime = DateTime.Now;
                App.Current.SaveSettings();

                DisplayLoadedLists(lists);
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }

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
                App.Lists = new List<ListEx>();
                Debug.WriteLine(ex.ToString());
            }

            Dispatcher.BeginInvoke(new RoutedEventHandler(this.OnLoadedFromStorage), null, null);
        }

        void OnLoadedFromStorage(object sender, EventArgs e)
        {
            this.DisplayLoadedLists(App.Lists);
            this.UpdateControls();
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
//            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));

            this.LoadFromStorage(null);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
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