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

namespace BarMonkey
{
    /// <summary>
    /// Interaction logic for UserHome.xaml
    /// </summary>
    public partial class UserHome : Page
    {
        public UserHome()
        {
            InitializeComponent();
        }

        private List<Activity> GetActivities()
        {
            List<Activity> list = new List<Activity>();
            list.Add(new Activity { Name = "Search Drinks", PageUrl = "Activities/SearchDrinks.xaml" });
            list.Add(new Activity { Name = "Make a drink for...", IsEnabled = false });
            list.Add(new Activity { Name = "Custom Drink", IsEnabled = false });
            list.Add(new Activity { Name = "Admin", IsEnabled = false });
            return list;
        }


        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.activityList.ItemsSource = this.GetActivities();

            this.favoriteList.ItemsSource = BarMonkeyContext.Current.Drinks.GetFavorites();
        }

        private void selectActivity_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            Activity match = null;

            foreach (Activity a in this.GetActivities())
            {
                if (a.Name == b.Content.ToString())
                {
                    match = a;
                    break;
                }
            }

            if (match != null && match.PageUrl != null)
            {
                Uri uri = new Uri(string.Format(
                    "pack://application:,,,/{0}",
                    match.PageUrl));
                base.NavigationService.Navigate(uri);
            }

        }

        private void favorite_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int drinkId = (int)button.Tag;

            Drink drink = BarMonkeyContext.Current.Drinks.GetDrink(drinkId);
            Activities.ConfirmDrink cd = new BarMonkey.Activities.ConfirmDrink();
            cd.SetDrink(drink);
            base.NavigationService.Navigate(cd);
        }
    }

    public class Activity
    {
        public string Name { get; set; }

        public string PageUrl { get; set; }

        public bool IsEnabled { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
