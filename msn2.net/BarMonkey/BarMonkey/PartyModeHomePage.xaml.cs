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
using BarMonkey;

namespace msn2.net.BarMonkey
{
    /// <summary>
    /// Interaction logic for PartyModeHomePage.xaml
    /// </summary>
    public partial class PartyModeHomePage : Page
    {
        public PartyModeHomePage()
        {
            InitializeComponent();
        }

        private void newDrink_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("pack://application:,,,/Activities/NewDrink.xaml");
            NavigationService.Navigate(uri);
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("pack://application:,,,/Activities/Settings.xaml");
            NavigationService.Navigate(uri);
        }
    }
}
