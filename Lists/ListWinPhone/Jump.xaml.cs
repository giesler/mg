﻿using System;
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
    public partial class Jump : PhoneApplicationPage
    {
        public Jump()
        {
            InitializeComponent();

            App.Current.LoadAll();

            this.storeList.ItemsSource = App.Lists.OrderBy(l => l.Name);
        }

        public ListEx SelectedList { get; private set; }
        
        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            this.SelectedList = (ListEx)tb.Tag;
            
            App.SelectedList = this.SelectedList.UniqueId;
            App.Current.SaveSettings();
            App.IsJumpNavigation = true;

            NavigationService.GoBack();
        }
    }
}