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

namespace msn2.net.BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for DrinkDisplayOptions.xaml
    /// </summary>
    public partial class DrinkDisplayOptions : Page
    {
        public DrinkDisplayOptions()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.navStack.AddCommands(GetActivities());
            this.navStack.NavigateToUri += delegate(Uri u) { this.NavigationService.Navigate(u); };

            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
        }

        private List<Activity> GetActivities()
        {
            List<Activity> list = new List<Activity>();
            list.Add(new Activity { Name = "Search", PageUrl = "Activities/SearchDrinks.xaml" });
            list.Add(new Activity { Name = "Browse A-Z", PageUrl = "Activities/BrowseByAlpha.xaml" });
            list.Add(new Activity { Name = "Categories", PageUrl = "Activities/DrinkByCategory.xaml" });
            return list;
        }
    }
}
