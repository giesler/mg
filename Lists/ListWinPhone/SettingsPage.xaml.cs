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
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            App.Current.LoadAll();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.LoadLists();

            this.list.Focus();
        }

        public void LoadLists()
        {
            this.list.Items.Clear();

            foreach (List list in App.Lists.OrderBy(i => i.Name))
            {
                this.list.Items.Add(list);
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            ListEx list = tb.Tag as ListEx;
            NavigationService.Navigate(new Uri("/EditStorePage.xaml?listUniqueId=" + list.UniqueId.ToString(), UriKind.Relative));
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(@"/AddList.xaml", UriKind.Relative));
        }

        private void aboutMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

    }
}