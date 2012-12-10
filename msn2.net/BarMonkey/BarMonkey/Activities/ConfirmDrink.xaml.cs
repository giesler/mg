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
using msn2.net.BarMonkey;

namespace BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for PourDrink.xaml
    /// </summary>
    public partial class ConfirmDrink : Page
    {
        private Drink drink = null;
        private Container container = null;

        public ConfirmDrink()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            foreach (Container c in BarMonkeyContext.Current.Containers.GetContainers())
            {
                this.containers.Items.Add(c);
            }

            if (this.container == null)
            {
                this.containers.SelectedIndex = this.containers.Items.Count > 2 ? 2 : 0;
                this.container = (Container)this.containers.SelectedItem;
            }

            if (BarMonkeyContext.Current.ImpersonateUser != null
                || BarMonkeyContext.Current.CurrentUser.IsGuest == true)
            {
                this.favorite.Visibility = Visibility.Hidden;
            }

            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
            this.navBar.NextClicked += new EventHandler(navBar_NextClicked);
            this.navBar.NextEnabled = true;
            this.navBar.NextContent = "POUR";
        }

        void navBar_NextClicked(object sender, EventArgs e)
        {
            PourDrink pour = new PourDrink();
            pour.SetDrink(this.drink, this.container);
            this.NavigationService.Navigate(pour);
        }

        public void SetDrink(Drink drink)
        {
            this.drink = drink;

            this.UpdateDrinkDetails();

            this.favorite.IsChecked = BarMonkeyContext.Current.Drinks.IsFavorite(drink.Id);
        }

        private void UpdateDrinkDetails()
        {
            if (this.drink != null)
            {
                this.drinkName.Content = this.drink.Name;
                this.description.Text = this.drink.Description;

                decimal totalSize = (from di in this.drink.DrinkIngredients select di.AmountOunces).Sum();
                decimal offset = this.container.Size / totalSize;

                this.ingredients.Items.Clear();
                foreach (DrinkIngredient di in this.drink.DrinkIngredients)
                {
                    string amount = (di.AmountOunces * offset).ToString("0.0") + " oz";
                    DrinkIngredientAmount a = new DrinkIngredientAmount { Name = di.Ingredient.Name, Amount = amount };
                    this.ingredients.Items.Add(a);
                }
            }
        }

        protected class DrinkIngredientAmount
        {
            public string Name { get; set; }
            public string Amount { get; set; }
        }

        private void addToFavorites_Click(object sender, RoutedEventArgs e)
        {
            BarMonkeyContext.Current.Drinks.SetFavorite(this.drink.Id, this.favorite.IsChecked);
        }

        private void containers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.container = this.containers.SelectedItem as Container;
            this.UpdateDrinkDetails();
        }

    }

    
}