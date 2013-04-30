using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using giesler.org.lists.ListData;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Shell;
using Windows.Phone.Speech.VoiceCommands;

namespace giesler.org.lists
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool backgroundOperationActive = false;
        private bool loadedFromStorage = false;
        private Timer lastRefreshDisplayTimer = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

                public bool AttemptedStorageLoad { get; set; }
        private bool isLoading = false;
        private StoreItemList listControl;

        void main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoading)
            {
                PivotItemEx item = this.main.Items[this.main.SelectedIndex] as PivotItemEx;
                if (item != null)
                {
                    ListEx list = item.Tag as ListEx;

                    if (list != null)
                    {
                        App.SelectedList = list.UniqueId;
                        App.Current.SaveSettings();
                    }

                    if (!item.IsLoaded)
                    {
                        item.IsLoaded = true;

                        ThreadPool.QueueUserWorkItem(new WaitCallback(DelayControlUpdateThread), item);
                    }
                }
            }
        }

        private void DelayControlUpdateThread(object sender)
        {
            Thread.Sleep(100);

            Dispatcher.BeginInvoke(new WaitCallback(this.AddListItems), sender);
        }

        private void AddListItems(object sender)
        {
            PivotItemEx item = (PivotItemEx)sender;
            ListEx list = (ListEx)item.Tag;
            this.listControl = new StoreItemList();
            item.Content = this.listControl;
            this.listControl.DeleteListItem += new DeleteListItemEventHandler(listControl_DeleteListItem);

            var q = list.Items.OrderBy(i => i.Name);
            listControl.Load(q);
        }

        private void DisplayLoadedLists(List<ListEx> lists)
        {
            App.Lists = lists;

            this.isLoading = true;
            this.main.Items.Clear();

            ListEx selected = null;
            if (App.SelectedList != Guid.Empty)
            {
                selected = App.Lists.FirstOrDefault(l => l.UniqueId == App.SelectedList);
                if (selected != null)
                {
                    int index = App.Lists.OrderBy(l => l.Name).ToList().IndexOf(selected);

                    App.Lists = new List<ListEx>();
                    lists.OrderBy(l => l.Name).Skip(index).ToList().ForEach(l => App.Lists.Add(l));
                    lists.OrderBy(l => l.Name).Take(index).ToList().ForEach(l => App.Lists.Add(l));
                }
            }

            foreach (ListEx list in App.Lists)
            {
                PivotItemEx item = new PivotItemEx();
                item.Tag = list;
                item.Header = list.Name.ToLowerInvariant();
                this.main.Items.Add(item);
            }

            this.isLoading = false;

            // Adding items sets the selectedindex, but doens't trigger selection
            this.main.SelectedIndex = 0;
            this.main.SelectedItem = this.main.Items[0];

            this.main_SelectionChanged(null, null);

            this.UpdateControls();

            TimeSpan refreshDuration = DateTime.Now - App.LastRefreshTime;
            if (refreshDuration.TotalSeconds > 90)
            {
                if (App.IsJumpNavigation)
                {
                    App.IsJumpNavigation = false;
                }
                else
                {
                    this.updatingMessage.Text = "Last updated " + GetDurationString(refreshDuration);
                    this.updatingMessage.Visibility = System.Windows.Visibility.Visible;
                    this.lastRefreshDisplayTimer = new Timer(new TimerCallback(OnLastRefreshHide), new object(), 1000 * 5, Timeout.Infinite);
                }
            }
            else
            {
                this.updatingMessage.Visibility = System.Windows.Visibility.Collapsed;
            }

            List<string> listNames = new List<string>();
            lists.ForEach(i => listNames.Add(i.Name));

            VoiceCommandSet widgetVcs = VoiceCommandService.InstalledCommandSets["en-US"];
            widgetVcs.UpdatePhraseListAsync("list", listNames);
        }

        static string GetDurationString(TimeSpan duration)
        {
            string durationText = string.Empty;
            if (duration.TotalMinutes < 1)
            {
                durationText = "under one minute ago";
            }
            else if (duration.TotalSeconds < 120)
            {
                durationText = string.Format("1 minute ago");
            }
            else if (duration.TotalMinutes < 60)
            {
                durationText = string.Format("{0:0} minutes ago", duration.TotalMinutes);
            }
            else if (duration.TotalMinutes < 120)
            {
                durationText = string.Format("1 hour ago");
            }
            else if (duration.TotalHours < 24)
            {
                durationText = string.Format("{0:0} hours ago", duration.TotalHours);
            }
            else if (duration.TotalHours < 48)
            {
                durationText = string.Format("Yesterday");
            }
            else if (duration.TotalHours < 72)
            {
                durationText = "The day before yesterday";
            }
            else if (duration.TotalHours < 96)
            {
                durationText = "The day before the day before yesterday";
            }
            else
            {
                durationText = string.Format("{0:0} days ago", duration.TotalDays);
            }
            return durationText;
        }

        private void OnLastRefreshHide(object sender)
        {
            Dispatcher.BeginInvoke(() =>
            {
                this.updatingMessage.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        void listControl_DeleteListItem(ListItem item)
        {
            if (!App.PendingDeletes.Contains(item.UniqueId))
            {
                App.PendingDeletes.Add(item.UniqueId);
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.SaveLists), new object());
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.QueueDeletes), new object());
            }
        }

        void QueueDeletes(object sender)
        {
            foreach (Guid guid in App.PendingDeletes)
            {
                ListDataServiceClient svc = App.DataProvider;
                svc.DeleteListItemCompleted += new EventHandler<DeleteListItemCompletedEventArgs>(svc_DeleteListItemCompleted);
                svc.DeleteListItemAsync(App.AuthDataList, guid, guid);
            }
        }

        void svc_DeleteListItemCompleted(object sender, DeleteListItemCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ListDataServiceClient client = (ListDataServiceClient)sender;
                client.CloseAsync();

                App.PendingDeletes.Remove((Guid)e.UserState);
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.SaveLists), new object());
            }
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
                ((IApplicationBarIconButton)this.ApplicationBar.Buttons[3]).IsEnabled = !loading;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            App.Current.LoadAll();

            if (NavigationContext.QueryString.ToString().Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                NavigationContext.QueryString.ToList().ForEach(i => sb.AppendFormat("{0}={1}", i.Key, i.Value));

                if (sb.ToString().Trim().Length > 0)
                {
                    MessageBox.Show(sb.ToString());
                }
            }

            //string msg = string.Format("MainPage_Loaded: App.Lists=null? {0}, App.AttemptedStorageLoad={1}", App.Lists == null, App.AttemptedStorageLoad);
            //MessageBox.Show(msg);

            //bool isLoaded = false;
            //if (this.main.Items.Count > 0)
            //{
            //    PivotItemEx item = this.main.SelectedItem as PivotItemEx;
            //    if (item != null)
            //    {
            //        ListEx list = item.Tag as ListEx;
            //        if (list.UniqueId == App.SelectedList)
            //        {
            //            isLoaded = true;

            //            ThreadPool.QueueUserWorkItem(new WaitCallback(DelayControlUpdateThread), item);
            //        }
            //    }
            //}

            //if (!isLoaded)
            {
                if (App.Lists != null && App.Lists.Count > 0)
                {
                    this.DisplayLoadedLists(App.Lists);
                }
                if (App.Lists.Count == 0)
                {
                    this.ReloadAll();

                    this.updatingMessage.Text = "Loading from service...";
                    this.updatingMessage.Visibility = Visibility.Visible;
                    this.pbar.Visibility = System.Windows.Visibility.Visible;
                }
                //else if (App.AuthData == null || App.AuthData.PersonUniqueId == Guid.Empty || App.AuthData.DeviceUniqueId == Guid.Empty)
                //{
                //    NavigationService.Navigate(new Uri("/GetLiveIdPage.xaml", UriKind.Relative));
                //}
            }

            this.UpdateControls();

            if (App.LastRefreshTime.AddMinutes(30) < DateTime.Now)
            {
                this.ReloadAll();
            }

            this.QueueDeletes(null);
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

                    if (resultItem.ItemUniqueId.HasValue && !App.PendingDeletes.Contains(resultItem.ItemUniqueId.Value))
                    {
                        ListItemEx item = new ListItemEx { Name = resultItem.ItemName, UniqueId = resultItem.ItemUniqueId.Value, ListUniqueId = resultItem.UniqueId };
                        lists.FirstOrDefault(l => l.UniqueId == resultItem.UniqueId).Items.Add(item);
                    }
                }

                

                App.Lists = lists;

                App.LastRefreshTime = DateTime.Now;

                ThreadPool.QueueUserWorkItem(new WaitCallback(this.SaveLists), new object());

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
            PivotItem item = (PivotItem)this.main.Items[this.main.SelectedIndex];
            List list = (List)item.Tag;

            NavigationService.Navigate(new Uri("/Add.xaml?listUniqueId=" + list.UniqueId, UriKind.Relative));
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            this.ReloadAll();
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