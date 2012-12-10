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

            this.blocks = new List<TextBlock>();
            this.blocks.Add(this.blockA);
            this.blocks.Add(this.blockD);
            this.blocks.Add(this.blockH);
            this.blocks.Add(this.blockM);
            this.blocks.Add(this.blockR);
            this.blocks.Add(this.blockU);

            SelectBlock(this.blockA);

            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
            this.navBar.NextClicked += new EventHandler(navBar_NextClicked);
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

        private void previousLetters_Click(object sender, RoutedEventArgs e)
        {
            int index = this.blocks.IndexOf(this.selectedBlock);
            if (index > 0)
            {
                SelectBlock(this.blocks[index - 1]);
            }
        }

        private void nextLetters_Click(object sender, RoutedEventArgs e)
        {
            int index = this.blocks.IndexOf(this.selectedBlock);
            if (index + 1 < this.blocks.Count)
            {
                SelectBlock(this.blocks[index + 1]);
            }
        }

        private void SelectBlock(TextBlock block)
        {
            foreach (TextBlock temp in this.blocks)
            {
                temp.Foreground = new SolidColorBrush(Colors.Gray);
                temp.FontSize = 16;
                temp.Padding = new Thickness(10.0);
                temp.FontWeight = FontWeights.Normal;
                temp.VerticalAlignment = VerticalAlignment.Center;
            }

            this.selectedBlock = block;
            if (selectedBlock != null)
            {
                this.selectedBlock.FontSize = 22;
                this.selectedBlock.Foreground = new SolidColorBrush(Colors.White);
                this.selectedBlock.FontWeight = FontWeights.Bold;
            }

            string[] searchStrings = this.selectedBlock.Tag.ToString().Split(',');
            char[] searchChars = new char[searchStrings.Length];
            for (int i = 0; i < searchStrings.Length; i++)
            {
                searchChars[i] = searchStrings[i][0];
            }
            this.drinkList.ItemsSource = BarMonkeyContext.Current.Drinks.GetDrinks(searchChars);
            if (this.drinkList.Items.Count > 0)
            {
                this.drinkList.SelectedIndex = 0;
            }

            int index = this.blocks.IndexOf(this.selectedBlock);
            this.nextLetters.IsEnabled = index + 1 < this.blocks.Count;
            this.previousLetters.IsEnabled = index > 0;
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
