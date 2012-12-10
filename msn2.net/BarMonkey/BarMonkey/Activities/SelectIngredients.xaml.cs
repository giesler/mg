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
    /// Interaction logic for SelectIngredients.xaml
    /// </summary>
    public partial class SelectIngredients : Page
    {
        public SelectIngredients()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };

            List<Ingredient> ingredients = BarMonkeyContext.Current.Ingredients.GetAvailableIngredients();
            this.available.ItemsSource = ingredients;
        }
    }
}
