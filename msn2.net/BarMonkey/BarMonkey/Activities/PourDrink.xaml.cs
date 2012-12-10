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
using msn2.net.BarMonkey;

namespace BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for PourDrink.xaml
    /// </summary>
    public partial class PourDrink : Page
    {
        private Drink drink = null;
        private Container container = null;

        public PourDrink()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            foreach (Container c in BarMonkeyContext.Current.Containers.GetContainers())
            {
                this.containers.Items.Add(c);

                if (this.container == null)
                {
                    this.container = c;
                    this.containers.SelectedItem = c;
                }
            }
        }

        public void SetDrink(int id)
        {
            this.drink = BarMonkeyContext.Current.Drinks.GetDrink(id);

            this.UpdateDrinkDetails();
        }

        private void UpdateDrinkDetails()
        {
            if (this.drink != null)
            {
                this.drinkName.Content = this.drink.Name;
                this.description.Content = this.drink.Description;

                decimal totalSize = 0.0M;

                foreach (DrinkIngredient ingredient in this.drink.DrinkIngredients)
                {
                    totalSize += ingredient.AmountOunces;
                }

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

        private void containers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.container = this.containers.SelectedItem as Container;
            this.UpdateDrinkDetails();
        }
    }

    
}
