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
using giesler.org.lists.svc1;
using giesler.org.lists.ListData;

namespace giesler.org.lists
{
    public partial class Add : PhoneApplicationPage
    {
        public Add()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            Guid listUniqueId = new Guid(NavigationContext.QueryString["listUniqueId"]);

            ListDataServiceClient svc = new ListDataServiceClient();
            svc.AddListItemCompleted += new EventHandler<AddListItemCompletedEventArgs>(svc_AddListItemCompleted);
            svc.AddListItemAsync(App.AuthDataList, listUniqueId, this.text.Text.Trim());

            List<ListItemEx> items = App.Items;
            items.Add(new ListItemEx { Id = -1, Name = this.text.Text.Trim(), ListUniqueId = listUniqueId });
            App.Items = items.OrderBy(i => i.Name).ToList();

            NavigationService.Navigate(new Uri("/MainPage.xaml?listUniqueId=" + listUniqueId.ToString(), UriKind.Relative));
        }

        void svc_AddListItemCompleted(object sender, AddListItemCompletedEventArgs e)
        {
            ListDataServiceClient svc = (ListDataServiceClient)sender;
            svc.CloseAsync();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void text_TextChanged(object sender, TextChangedEventArgs e)
        {
// BUGBUG:            this.addButton.IsEnabled = this.text.Text.Trim().Length > 0;
        }
    }
}