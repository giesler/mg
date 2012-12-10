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
    /// Interaction logic for BrowseByAlpha.xaml
    /// </summary>
    public partial class BrowseByAlpha : Page
    {
        private List<TextBlock> blocks;
        private TextBlock selectedBlock = null;

        public BrowseByAlpha()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.description.Text = string.Empty;

            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
            this.navBar.NextClicked += new EventHandler(navBar_NextClicked);

            this.drinkList.ItemsSource = BarMonkeyContext.Current.Drinks.GetDrinks();
            if (this.drinkList.Items.Count > 0)
            {
                this.drinkList.SelectedIndex = 0;
            }
        }

        void navBar_NextClicked(object sender, EventArgs e)
        {
            if (this.drinkList.SelectedIndex >= 0)
            {
                ConfirmDrink confirm = new ConfirmDrink();
                Drink drink = this.drinkList.SelectedItem as Drink;
                confirm.SetDrink(drink);
                base.NavigationService.Navigate(confirm);
            }
        }

        private void drinkList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Drink drink = this.drinkList.SelectedItem as Drink;
            if (drink == null)
            {
                this.description.Text = string.Empty;
                this.navBar.NextEnabled = false;
            }
            else
            {
                this.description.Text = drink.Description;
                this.navBar.NextEnabled = true;
            }
        }

    }
}
