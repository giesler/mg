using System;
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
using BarMonkey.Activities;

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
            this.navBar.NextClicked += new EventHandler(navBar_NextClicked);

            List<Ingredient> ingredients = BarMonkeyContext.Current.Ingredients.GetAvailableIngredients();
            foreach (Ingredient ingredient in ingredients)
            {
                this.available.Items.Add(ingredient);
            }
        }

        private void available_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.select.IsEnabled = (this.available.SelectedItems.Count > 0);
        }

        private void selected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.deselect.IsEnabled = (this.selected.SelectedItems.Count > 0);
        }

        private void select_Click(object sender, RoutedEventArgs e)
        {
            Ingredient ingredient = this.available.SelectedItem as Ingredient;
            if (ingredient != null)
            {
                this.available.Items.Remove(ingredient);
                this.selected.Items.Add(ingredient);
                this.selected.SelectedItem = ingredient;

                UpdateTotalCount();
            }
        }

        private void deselect_Click(object sender, RoutedEventArgs e)
        {
            Ingredient ingredient = this.selected.SelectedItem as Ingredient;
            if (ingredient != null)
            {
                this.selected.Items.Remove(ingredient);
                this.available.Items.Add(ingredient);
                this.available.SelectedItem = ingredient;

                UpdateTotalCount();
            }
        }

        private void UpdateTotalCount()
        {
            List<Drink> drinks = GetMatchingDrinks();

            if (drinks.Count == 0)
            {
                this.match.Content = "No matching drinks were found.";
                this.navBar.NextEnabled = false;
            }
            else
            {
                this.navBar.NextEnabled = true;

                if (drinks.Count == 1)
                {
                    this.match.Content = "1 drink matches";
                }
                else
                {
                    this.match.Content = drinks.Count.ToString() + " drinks match";
                }
            }
        }

        private List<Drink> GetMatchingDrinks()
        {
            List<Ingredient> matchingIngredients = new List<Ingredient>();
            foreach (Ingredient item in this.selected.Items)
            {
                matchingIngredients.Add(item);
            }

            List<Drink> drinks = BarMonkeyContext.Current.Drinks.GetDrinks(matchingIngredients);
            return drinks;
        }

        void navBar_NextClicked(object sender, EventArgs e)
        {
            List<Drink> drinks = GetMatchingDrinks();

            if (drinks.Count == 1)
            {
                ConfirmDrink cd = new ConfirmDrink();
                cd.SetDrink(drinks[0]);
                base.NavigationService.Navigate(cd);
            }
            else
            {
                SelectDrink sd = new SelectDrink();
                sd.LoadDrinks(drinks);
                base.NavigationService.Navigate(sd);
            }
        }

    }
}
