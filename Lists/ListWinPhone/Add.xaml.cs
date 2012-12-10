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

namespace giesler.org.lists
{
    public partial class Add : PhoneApplicationPage
    {
        public Add()
        {
            InitializeComponent();

            Guid listId = App.SelectedList;
            List list = App.Lists.FirstOrDefault(l => l.UniqueId == listId);

            this.PageTitle.Text = list.Name;
        }

        void svc_AddListItemCompleted(object sender, AddListItemCompletedEventArgs e)
        {
            ListDataServiceClient svc = (ListDataServiceClient)sender;
            svc.CloseAsync();
        }

        private void text_TextChanged(object sender, TextChangedEventArgs e)
        {
// BUGBUG:            this.addButton.IsEnabled = this.text.Text.Trim().Length > 0;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Guid listUniqueId = new Guid(NavigationContext.QueryString["listUniqueId"]);

            ListDataServiceClient svc = App.DataProvider;
            svc.AddListItemCompleted += new EventHandler<AddListItemCompletedEventArgs>(svc_AddListItemCompleted);
            svc.AddListItemAsync(App.AuthDataList, listUniqueId, this.text.Text.Trim());

            ListEx list = App.Lists.FirstOrDefault(l => l.UniqueId == listUniqueId);
            list.Items.Add(new ListItemEx { Id = -1, Name = this.text.Text.Trim(), ListUniqueId = listUniqueId });
            list.Items  = list.Items.OrderBy(i => i.Name).ToList();

            NavigationService.GoBack();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}