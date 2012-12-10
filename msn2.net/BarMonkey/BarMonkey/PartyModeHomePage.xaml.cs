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
using BarMonkey;

namespace msn2.net.BarMonkey
{
    /// <summary>
    /// Interaction logic for PartyModeHomePage.xaml
    /// </summary>
    public partial class PartyModeHomePage : Page
    {
        public PartyModeHomePage()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.navStack.AddCommands(GetActivities());
            this.navStack.NavigateToUri += delegate(Uri u) { this.NavigationService.Navigate(u); };
        }

        private List<Activity> GetActivities()
        {
            List<Activity> list = new List<Activity>();
            list.Add(new Activity { Name = "New Drink", PageUrl = "Activities/NewDrink.xaml" });
            //list.Add(new Activity { Name = "Statistics", PageUrl = "Activities/Statistics.xaml" });
            list.Add(new Activity { Name = "Settings", PageUrl = "Activities/Settings.xaml" });
            return list;
        }
    }
}
