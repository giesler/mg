using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HomeControl81
{
    public partial class SelectTime : PhoneApplicationPage
    {
        public SelectTime()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            TimeSpan duration = TimeSpan.Parse(tb.Tag.ToString());

            App.Duration = duration;

            NavigationService.GoBack();
        }
    }
}