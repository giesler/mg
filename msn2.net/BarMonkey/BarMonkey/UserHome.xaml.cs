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
            list.Add(new Activity { Name = "Make a drink for..." });
            list.Add(new Activity { Name = "Custom Drink" });
            return list;
        }


        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.activityList.ItemsSource = this.GetActivities();

            var q = BarMonkeyContext.Current.Data.GetUsersFavoriteDrinks(1);

            List<GetUsersFavoriteDrinksResult> drinkList = q.ToList<GetUsersFavoriteDrinksResult>();
            this.favoriteList.DataContext = drinkList;

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

            if (match != null)
            {
                Uri uri = new Uri(string.Format(
                    "pack://application:,,,/{0}",
                    match.PageUrl));
                base.NavigationService.Navigate(uri);
            }

        }
    }

    public class Activity
    {
        public string Name { get; set; }

        public string PageUrl { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
