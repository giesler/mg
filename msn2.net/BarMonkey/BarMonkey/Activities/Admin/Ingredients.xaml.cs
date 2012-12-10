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
    /// Interaction logic for Ingredients.xaml
    /// </summary>
    public partial class Ingredients : Page
    {
        public Ingredients()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.selectView.Items.Add("by name");
            this.selectView.Items.Add("by amount remaining");

            this.selectView.SelectedIndex = 0;
        }

        private void selectView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.selectView.SelectedIndex == 0)
            {
                this.ingredientList.ItemsSource = from i in BarMonkeyContext.Current.Data.Ingredients
                                                  orderby i.Name ascending
                                                  select i;
            }
            else
            {
                this.ingredientList.ItemsSource = from i in BarMonkeyContext.Current.Data.Ingredients
                                                  orderby i.RemainingOunces ascending
                                                  select i;
            }
        }

        private void ingredient_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
