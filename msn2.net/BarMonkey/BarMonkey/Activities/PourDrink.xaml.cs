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

        public PourDrink()
        {
            InitializeComponent();
        }

        public void SetDrink(int id)
        {
            this.drink = BarMonkeyContext.Current.Drinks.GetDrink(id);

            this.drinkName.Content = this.drink.Name;
            this.description.Content = this.drink.Description;

            foreach (DrinkIngredient ingredient in this.drink.DrinkIngredients)
            {
                this.ingredients.Items.Add(ingredient.Ingredient);
            }
        }
    }
}
