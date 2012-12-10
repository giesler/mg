using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BarMonkey;
using BarMonkey.Activities;

namespace msn2.net.BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        string input = string.Empty;
        DispatcherTimer timer = null;
        const string PIN = "1212";

        public Settings()
        {
            InitializeComponent();

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.Reload), new object());
        }

        void Reload(object sender)
        {
            BarMonkeyContext.Current.Reload();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.navBar.BackClicked += delegate(object o, EventArgs a) {base.NavigationService.GoBack();};
            this.navBar.HomeClicked += delegate(object o, EventArgs a) {base.NavigationService.Navigate(new PartyModeHomePage());};

            this.navStack.AddCommands(this.GetActivities());
            this.navStack.NavigateToUri += delegate(Uri u) { this.NavigationService.Navigate(u); };

            this.timer = new DispatcherTimer(TimeSpan.FromSeconds(3), DispatcherPriority.Normal, this.timer_Tick, this.Dispatcher);
            this.timer.IsEnabled = false;

            if (App.IsPinValid)
            {                
                this.input = PIN;
                this.Button_Click_1(null, null);
            }            
        }        

        private List<Activity> GetActivities()
        {
            List<Activity> list = new List<Activity>();
            list.Add(new Activity { Name = "INGREDIENTS", PageUrl = "Activities/IngredientSettings.xaml" });
            list.Add(new Activity { Name = "RELAYS", PageUrl = "Activities/RelaySettings.xaml" });

            return list;
        }

        private void byName_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new SearchDrinks());
        }

        private void relay_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.Navigate(new RelaySettings());
        }

        private void exit_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            if (b != null)
            {
                this.input += b.Content.ToString();
            }

            if (this.input == PIN)
            {
                this.navStack.Visibility = System.Windows.Visibility.Visible;
                this.numberPad.Visibility = System.Windows.Visibility.Collapsed;
                this.exit.Visibility = System.Windows.Visibility.Visible;
                this.view.Visibility = System.Windows.Visibility.Collapsed;
                App.IsPinValid = true;
            }
            else if (this.input.Length > 3)
            {
                this.timer.IsEnabled = true;
                this.numberPad.IsEnabled = false;
                this.input = string.Empty;
                this.view.Content = "INCORRECT PIN";
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.timer.IsEnabled = false;
            this.numberPad.IsEnabled = true;
            this.view.Content = "ENTER PIN";

            this.timer.Interval = TimeSpan.FromSeconds(this.timer.Interval.TotalSeconds * 2);

            if (this.timer.Interval.TotalSeconds > 20)
            {
                base.NavigationService.GoBack();
            }
        }

    }
}
