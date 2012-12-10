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
using msn2.net.BarMonkey.BarMonkeyClientService;

namespace msn2.net.BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for Top10Drinks.xaml
    /// </summary>
    public partial class Top10Drinks : Page
    {
        public Top10Drinks()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            BarMonkeyClientServiceClient svc = new BarMonkeyClientServiceClient();
            svc.GetTopDrinksCompleted += new EventHandler<GetTopDrinksCompletedEventArgs>(svc_GetTopDrinksCompleted);
            svc.GetTopDrinksAsync(10);
        }

        void svc_GetTopDrinksCompleted(object sender, GetTopDrinksCompletedEventArgs e)
        {
            List<Drink> drinks = e.Result;
            
            this.top5.ItemsSource = drinks.Take<Drink>(5);
            this.next5.ItemsSource = drinks.Skip<Drink>(5);

            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            int drinkId = (int)b.Tag;

            ConfirmDrink confirm = new ConfirmDrink();
            Drink drink = BarMonkeyContext.Current.Drinks.GetDrink(drinkId);
            confirm.SetDrink(drink);
            base.NavigationService.Navigate(confirm);
        }
    }
}
