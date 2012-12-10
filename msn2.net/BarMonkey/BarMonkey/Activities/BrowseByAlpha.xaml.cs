﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace msn2.net.BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for BrowseByAlpha.xaml
    /// </summary>
    public partial class BrowseByAlpha : Page
    {
        public BrowseByAlpha()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.drinkList.ItemsSource = BarMonkeyContext.Current.Drinks.GetDrinks("");

            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
        }
    }
}
