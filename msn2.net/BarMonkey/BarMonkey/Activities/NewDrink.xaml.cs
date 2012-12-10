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
    /// Interaction logic for NewDrink.xaml
    /// </summary>
    public partial class NewDrink : Page
    {
        public NewDrink()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.navBar.BackClicked += delegate(object o, EventArgs a) {base.NavigationService.GoBack();};
            this.navBar.HomeClicked += delegate(object o, EventArgs a) {base.NavigationService.Navigate(new PartyModeHomePage());};

            this.navStack.AddCommands(this.GetActivities());
            this.navStack.NavigateToUri += delegate(Uri u) { this.NavigationService.Navigate(u); };
        }

        private List<Activity> GetActivities()
        {
            List<Activity> list = new List<Activity>();
            list.Add(new Activity { Name = "Name", PageUrl = "Activities/DrinkDisplayOptions.xaml" });
            list.Add(new Activity { Name = "Ingredient", PageUrl = "Activities/SelectIngredients.xaml" });
            list.Add(new Activity { Name = "Top 10", PageUrl = "Activities/Top10Drinks.xaml" });
            return list;
        }
    }
}
