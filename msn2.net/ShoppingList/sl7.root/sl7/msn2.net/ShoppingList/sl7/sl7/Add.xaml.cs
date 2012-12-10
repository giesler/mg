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

namespace giesler.org.lists
{
    public partial class Add : PhoneApplicationPage
    {
        public Add()
        {
            InitializeComponent();
        }

        void svc_AddShoppingListItemCompleted(object sender, svc1.AddShoppingListItemCompletedEventArgs e)
        {
            svc1.ShoppingListServiceClient svc = (svc1.ShoppingListServiceClient)sender;
            svc.CloseAsync();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            string store = NavigationContext.QueryString["store"];

            svc1.ShoppingListServiceClient svc = new svc1.ShoppingListServiceClient();
            svc.AddShoppingListItemCompleted += new EventHandler<svc1.AddShoppingListItemCompletedEventArgs>(svc_AddShoppingListItemCompleted);
            svc.AddShoppingListItemAsync(store, this.text.Text.Trim());

            List<ShoppingListItem> items = App.Items;
            items.Add(new svc1.ShoppingListItem { Id = -1, ListItem = this.text.Text.Trim(), Store = store });
            App.Items = items.OrderBy(i => i.ListItem).ToList();

            NavigationService.Navigate(new Uri("/MainPage.xaml?s=" + store, UriKind.Relative));
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