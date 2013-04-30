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
using giesler.org.lists.ListData;
using Microsoft.Phone.Shell;
using System.Threading;
using Windows.Phone.Speech.Recognition;

namespace giesler.org.lists
{
    public partial class Add : PhoneApplicationPage
    {
        private SpeechRecognizerUI reco;

        public Add()
        {
            InitializeComponent();

            App.Current.LoadAll();

            Guid listId = App.SelectedList;
            List list = App.Lists.FirstOrDefault(l => l.UniqueId == listId);

            //this.PageTitle.Text = list.Name.ToLower();

            this.Loaded += new RoutedEventHandler(OnPageLoaded);
        }

        void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            if (!NavigationContext.QueryString.ContainsKey("voice"))
            {
                this.text.Focus();
            }
        }

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("voice"))
            {
                this.reco = new SpeechRecognizerUI();
                this.reco.Settings.ReadoutEnabled = false;
                this.reco.Settings.ShowConfirmation = false;
                SpeechRecognitionUIResult result = await this.reco.RecognizeWithUIAsync();
                this.text.Text = result.RecognitionResult.Text;
                this.reco.Dispose();
                this.text.Focus();
            }
        }

        private void text_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = this.text.Text.Trim().Length > 0;
        }

        private void OnAdd(object sender, EventArgs e)
        {
            ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = false;
            this.text.IsEnabled = false;
            this.pbar.Visibility = System.Windows.Visibility.Visible;
            this.status.Visibility = System.Windows.Visibility.Visible;

            string[] items = this.text.Text.Trim().Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

            if (items.Length == 1)
            {
                this.AddItem(items[0]);
            }
            else
            {
                this.AddItems(items);
            }
        }

        private void AddItem(string item)
        {
            Guid listUniqueId = new Guid(NavigationContext.QueryString["listUniqueId"]);

            ListEx list = App.Lists.FirstOrDefault(l => l.UniqueId == listUniqueId);
            ListItemEx newItem = new ListItemEx { Id = -1, Name = item.Trim(), ListUniqueId = listUniqueId, UniqueId = Guid.NewGuid() };
            list.Items.Add(newItem);
            list.Items = list.Items.OrderBy(i => i.Name).ToList();

            ListDataServiceClient svc = App.DataProvider;
            svc.AddListItemWithIdCompleted += new EventHandler<AddListItemWithIdCompletedEventArgs>(svc_AddListItemCompleted);
            svc.AddListItemWithIdAsync(App.AuthDataList, listUniqueId, newItem.UniqueId, item.Trim(), newItem.UniqueId);
        }

        void svc_AddListItemCompleted(object sender, AddListItemWithIdCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.OnAddError(e.Error);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.SaveLists), new object());
                NavigationService.GoBack();
            }

            ListDataServiceClient svc = (ListDataServiceClient)sender;
            svc.CloseAsync();
        }

        void OnAddError(Exception ex)
        {
            ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = false;
            this.text.IsEnabled = false;
            this.pbar.Visibility = System.Windows.Visibility.Visible;
            this.status.Visibility = System.Windows.Visibility.Visible;

            MessageBox.Show(ex.Message);
        }

        private void AddItems(string[] items)
        {
            Guid listUniqueId = new Guid(NavigationContext.QueryString["listUniqueId"]);

            ListEx list = App.Lists.FirstOrDefault(l => l.UniqueId == listUniqueId);
            List<ListItem> addList = new List<ListItem>();

            foreach (string item in items)
            {
                ListItemEx newItem = new ListItemEx { Id = -1, Name = item.Trim(), ListUniqueId = listUniqueId, UniqueId = Guid.NewGuid() };
                list.Items.Add(newItem);
                list.Items = list.Items.OrderBy(i => i.Name).ToList();

                addList.Add(new ListItem{ UniqueId = newItem.UniqueId, Name = item.Trim()});
            }

            ListDataServiceClient svc = App.DataProvider;
            svc.AddListItemsAsync(App.AuthDataList, listUniqueId, addList);
            svc.AddListItemsCompleted += OnAddListItemsCompleted;
        }

        void OnAddListItemsCompleted(object sender, AddListItemsCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.OnAddError(e.Error);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.SaveLists), new object());
                NavigationService.GoBack();
            }

            ListDataServiceClient svc = (ListDataServiceClient)sender;
            svc.CloseAsync();
        }

        void SaveLists(object sender)
        {
            App.Current.SaveAll();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}