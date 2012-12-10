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

namespace BarMonkey.Activities.Admin
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        public Admin()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.activityList.ItemsSource = this.GetActivities();
        }

        private List<Activity> GetActivities()
        {
            List<Activity> list = new List<Activity>();
            list.Add(new Activity { Name = "Users", IsEnabled = false });
            list.Add(new Activity { Name = "Ingredients", PageUrl="Activities/Admin/Ingredients.xaml"});
            list.Add(new Activity { Name = "Drinks", IsEnabled = false });
            list.Add(new Activity { Name = "Settings", IsEnabled = false });
            return list;
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
    }
}
