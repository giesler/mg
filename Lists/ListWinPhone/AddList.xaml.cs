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
    public partial class AddList : PhoneApplicationPage
    {
        public AddList()
        {
            InitializeComponent();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            this.add.IsEnabled = false;
            this.adding.Visibility = System.Windows.Visibility.Visible;

            IListDataProvider svc = App.DataProvider;
            svc.AddListCompleted += new EventHandler<AddListCompletedEventArgs>(svc_AddListCompleted);
            svc.AddListAsync(App.AuthDataList, this.name.Text.Trim());
        }

        void svc_AddListCompleted(object sender, AddListCompletedEventArgs e)
        {
            IListDataProvider svc = (IListDataProvider)sender;
            svc.CloseAsync();

            NavigationService.GoBack();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.name.Text == @"<name>")
            {
                this.name.Text = string.Empty;
            }
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.add.IsEnabled = this.name.Text.Trim().Length > 1;
        }
    }
}