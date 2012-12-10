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

            this.LoadLists();
        }
        
        public void LoadLists()
        {
            this.list.Items.Clear();

            foreach (List list in App.Lists)
            {
                this.list.Items.Add(list);
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.list.SelectedIndex > 0)
            {
                List list = this.list.SelectedItem as List;
                NavigationService.Navigate(new Uri("/EditStorePage.xaml?listUniqueId=" + list.UniqueId.ToString(), UriKind.Relative));
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(@"/AddList.xaml", UriKind.Relative));
        }
    }
}