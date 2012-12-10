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

            this.LoadStores();
        }
        
        public void LoadStores()
        {
            this.list.Items.Clear();

            foreach (List list in App.Lists)
            {
                this.list.Items.Add(list);
            }

            this.list.Items.Add(new List { Name = "add..." });
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.list.SelectedIndex > 0)
            {
                NavigationService.Navigate(new Uri("/EditStorePage.xaml?s=" + this.list.SelectedItem.ToString(), UriKind.Relative));
            }
        }
    }
}