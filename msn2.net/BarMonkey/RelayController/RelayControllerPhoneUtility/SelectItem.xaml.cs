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

namespace RelayControllerPhoneUtility
{
    public partial class SelectItem : PhoneApplicationPage
    {
        public SelectItem()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationService.CurrentSource.ToString().IndexOf("list=relay") > 0)
            {
                this.listBox.ItemsSource = App.Relays;
            }
            else
            {
                this.listBox.ItemsSource = App.Seconds;
            }
        }

        void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            if (NavigationService.CurrentSource.ToString().IndexOf("list=relay") > 0)
            {
                App.Relay = (int)tb.Tag;
            }
            else
            {
                App.SecondCount = (int)tb.Tag;
            }

            NavigationService.GoBack();
        }
    }
}