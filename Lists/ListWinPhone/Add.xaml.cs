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

namespace giesler.org.lists
{
    public partial class Add : PhoneApplicationPage
    {
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
            this.text.Focus();
        }

        private void text_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = this.text.Text.Trim().Length > 0;
        }

        private void OnAdd(object sender, EventArgs e)
        {
            Guid listUniqueId = new Guid(NavigationContext.QueryString["listUniqueId"]);

            ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = false;
            this.text.IsEnabled = false;
            this.pbar.Visibility = System.Windows.Visibility.Visible;
            this.status.Visibility = System.Windows.Visibility.Visible;

            ListDataServiceClient svc = App.DataProvider;
            svc.AddListItemCompleted += new EventHandler<AddListItemCompletedEventArgs>(svc_AddListItemCompleted);
            svc.AddListItemAsync(App.AuthDataList, listUniqueId, this.text.Text.Trim());

            ListEx list = App.Lists.FirstOrDefault(l => l.UniqueId == listUniqueId);
            list.Items.Add(new ListItemEx { Id = -1, Name = this.text.Text.Trim(), ListUniqueId = listUniqueId });
            list.Items = list.Items.OrderBy(i => i.Name).ToList();
        }

        void svc_AddListItemCompleted(object sender, AddListItemCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ((IApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = false;
                this.text.IsEnabled = false;
                this.pbar.Visibility = System.Windows.Visibility.Visible;
                this.status.Visibility = System.Windows.Visibility.Visible;
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