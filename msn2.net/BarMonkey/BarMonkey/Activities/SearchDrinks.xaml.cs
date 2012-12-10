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
using System.Diagnostics;

namespace BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for SearchDrinks.xaml
    /// </summary>
    public partial class SearchDrinks : Page
    {
        public SearchDrinks()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.numPad.KeyPressed += new EventHandler<NumericKeypadPressedEventArgs>(keyPad_KeyPressed);
            this.kb.KeyboardButtonPressed += new KeyboardButtonPressedEvent(kb_KeyboardButtonPressed);

            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
        }

        private Key lastKey = Key.End;
        private DateTime lastKeyPress = DateTime.MinValue;

        void keyPad_KeyPressed(object sender, NumericKeypadPressedEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (this.searchText.Text.Length > 0)
                {
                    this.searchText.Text = this.searchText.Text.Substring(0, this.searchText.Text.Length - 1);
                }
            }
            else
            {
                string addKey = null;
                TimeSpan lastPressTime = DateTime.Now - lastKeyPress;
                Trace.WriteLine(e.Key.ToString() + ": " + lastPressTime.TotalMilliseconds.ToString() + " (last: " + lastKey.ToString() + ")");

                if (lastKey == e.Key && lastPressTime.TotalMilliseconds < 2000)
                {
                    string lastChar = null;
                    if (this.searchText.Text.Length > 0)
                    {
                        lastChar = this.searchText.Text.Substring(this.searchText.Text.Length - 1, 1);
                    }

                    addKey = GetStringForNumericKey(e.Key, lastChar);

                    if (addKey != null)
                    {
                        this.searchText.Text = this.searchText.Text.Substring(0, this.searchText.Text.Length-1) + addKey;
                    }
                }
                else
                {
                    addKey = GetStringForNumericKey(e.Key, null);

                    if (addKey != null)
                    {
                        this.searchText.Text += addKey;
                    }
                }
            }

            this.UpdateResultList();

            lastKey = e.Key;
            lastKeyPress = DateTime.Now;
        }

        void kb_KeyboardButtonPressed(string content)
        {
            if (content == "DEL")
            {
                if (this.searchText.Text.Length > 0)
                {
                    this.searchText.Text = this.searchText.Text.Substring(0, this.searchText.Text.Length - 1);
                }
            }
            else
            {
                this.searchText.Text += content;
            }

            this.UpdateResultList();
        }

        void selectDrink(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            int drinkId = (int)b.Tag;

            ConfirmDrink confirm = new ConfirmDrink();
            Drink drink = BarMonkeyContext.Current.Drinks.GetDrink(drinkId);
            confirm.SetDrink(drink);
            base.NavigationService.Navigate(confirm);
        }

        void UpdateResultList()
        {
            this.statusLabel.Content = "Searching...";

            this.resultList.Items.Clear();

            List<Drink> list = BarMonkeyContext.Current.Drinks.GetDrinks(this.searchText.Text);
            foreach (Drink drink in list)
            {
                this.resultList.Items.Add(drink);
            }

            this.statusLabel.Content = string.Format("Found {0} drinks.", list.Count);                    
        }

        string GetStringForNumericKey(Key key, string lastChar)
        {
            NumericKeyMapping match = null;
            foreach (NumericKeyMapping mapping in this.GetKeyMappings())
            {
                if (mapping.Key == key)
                {
                    match = mapping;
                    break;
                }
            }

            string returnChar = null;
            if (match != null)
            {
                if (lastChar == null)
                {
                    returnChar = match.Chars[0];
                }
                else
                {
                    int position = -1;
                    for (int i = 0; i < match.Chars.Length; i++)
                    {
                        if (lastChar == match.Chars[i])
                        {
                            position = i;
                            break;
                        }
                    }

                    if (position > -1)
                    {
                        if (position == match.Chars.Length - 1)
                        {
                            position = -1;
                        }
                    }

                    position++;

                    returnChar = match.Chars[position];
                }
            }

            return returnChar;
        }

        private class NumericKeyMapping { public Key Key { get; set; } public string[] Chars { get; set; } }

        private List<NumericKeyMapping> GetKeyMappings()
        {
            List<NumericKeyMapping> list = new List<NumericKeyMapping>();
            list.Add(new NumericKeyMapping() { Key = Key.NumPad0, Chars = new string[] { " ", "0" } });
            list.Add(new NumericKeyMapping() { Key = Key.NumPad1, Chars = new string[] { "1" } });
            list.Add(new NumericKeyMapping() { Key = Key.NumPad2, Chars = new string[] { "a", "b", "c", "2" } });
            list.Add(new NumericKeyMapping() { Key = Key.NumPad3, Chars = new string[] { "d", "e", "f", "3" } });
            list.Add(new NumericKeyMapping() { Key = Key.NumPad4, Chars = new string[] { "g", "h", "i", "4" } });
            list.Add(new NumericKeyMapping() { Key = Key.NumPad5, Chars = new string[] { "j", "k", "l", "5" } });
            list.Add(new NumericKeyMapping() { Key = Key.NumPad6, Chars = new string[] { "m", "n", "o", "6" } });
            list.Add(new NumericKeyMapping() { Key = Key.NumPad7, Chars = new string[] { "p", "q", "r", "s", "7" } });
            list.Add(new NumericKeyMapping() { Key = Key.NumPad8, Chars = new string[] { "t", "u", "v", "8" } });
            list.Add(new NumericKeyMapping() { Key = Key.NumPad9, Chars = new string[] { "w", "x", "y", "z", "9" } });
            return list;
        }

        private void ToggleNumPad_Click(object sender, RoutedEventArgs e)
        {
            this.toggleKb.IsChecked = !this.toggleNumPad.IsChecked;
            this.UpdateInputPanel();
        }

        private void ToggleKeyboard_Click(object sender, RoutedEventArgs e)
        {
            this.toggleNumPad.IsChecked = !this.toggleKb.IsChecked;
            this.UpdateInputPanel();
        }

        private void UpdateInputPanel()
        {
            bool showNumPad = (this.toggleNumPad.IsChecked == true);

            this.kb.Visibility = showNumPad == true ? Visibility.Hidden : Visibility.Visible;
            this.numPad.Visibility = showNumPad == true ? Visibility.Visible : Visibility.Hidden;
        }

    }


}
