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
using System.Windows.Threading;
using msn2.net.BarMonkey.RelayControllerService;
using System.Threading;

namespace BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for PourDrink.xaml
    /// </summary>
    public partial class PourDrink : Page
    {
        private Drink drink;
        private Container container;
        private RelayControllerClient relayClient = new RelayControllerClient();

        public PourDrink()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // TODO: Close client

            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
        }

        public void SetDrink(Drink drink, Container container)
        {
            this.drink = drink;
            this.container = container;

            this.drinkName.Content = this.drink.Name;

            this.repeat.Visibility = Visibility.Hidden;
            this.navBar.IsEnabled = false;

            this.statusLabel.Content = "connecting...";

            ThreadPool.QueueUserWorkItem(new WaitCallback(Connect));
        }

        private void Connect(object foo)
        {
            try
            {
                relayClient.ConnectTest();
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AsyncCallback(onConnected), null);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new WaitCallback(displayException), ex);
            }
        }

        private void displayException(object o)
        {
            Exception exception = (Exception)o;
            this.statusLabel.Content = exception.ToString();
            this.navBar.IsEnabled = true;
        }

        private void onConnected(IAsyncResult ar)
        {
            this.statusLabel.Content = "sending...";

            var q = from di in this.drink.DrinkIngredients
                    orderby di.Group, di.Sequence
                    select di;

            decimal totalOunces = (from di in this.drink.DrinkIngredients select di.AmountOunces).Sum();
            decimal offset = this.container.Size / totalOunces;

            List<BatchItem> items = new List<BatchItem>();

            decimal fullDuration = 0;
            decimal currentStageDuration = 0;
            int currentGroup = -1;
            foreach (DrinkIngredient di in q)
            {
                if (currentGroup != di.Group)
                {
                    fullDuration += currentStageDuration;
                    currentGroup = di.Group;
                    currentStageDuration = 0;
                }

                decimal duration = GetOutputDuration(offset, di);
                if (duration > currentStageDuration)
                {
                    currentStageDuration = duration;
                }
            }
            fullDuration += currentStageDuration;

            int lightRelayNumber = 35;
            int soundRelayNumber = 34;
            items.Add(new BatchItem { Group = 0, RelayNumber = lightRelayNumber, Seconds = (double)fullDuration });
            items.Add(new BatchItem { Group = 0, RelayNumber = soundRelayNumber, Seconds = (double)fullDuration });

            foreach (DrinkIngredient di in q)
            {
                decimal duration = GetOutputDuration(offset, di);

                int relayNumber = (int)di.Ingredient.RelayId;
                
                items.Add(new BatchItem { Group = di.Group, RelayNumber = relayNumber, Seconds=(double)duration});
            }

            if (this.container.WaterFlushOunces > 0)
            {
                Ingredient waterIngredient = BarMonkeyContext.Current.Ingredients.GetIngredient("Water");
                decimal duration = this.container.WaterFlushOunces * waterIngredient.OuncesPerSecond;

                items.Add(new BatchItem { Group = 999, RelayNumber = (int)waterIngredient.RelayId, Seconds = (double)duration });
            }

            bool pouring = false;
            try
            {
                relayClient.BeginSendBatch(items.ToArray<BatchItem>(), pourComplete, null);
                pouring = true;
            }
            catch (Exception ex)
            {
                this.displayException(ex);
            }

            if (pouring == true)
            {
                this.statusLabel.Content = "pouring...";

                if (BarMonkeyContext.Current.ImpersonateUser != null)
                {
                    BarMonkeyContext.Current.Drinks.LogDrink(drink, offset, BarMonkeyContext.Current.ImpersonateUser.Id);
                }
                else
                {
                    BarMonkeyContext.Current.Drinks.LogDrink(drink, offset);
                }
            }
        }

        private static decimal GetOutputDuration(decimal offset, DrinkIngredient di)
        {
            decimal outputAmount = di.AmountOunces * offset;
            decimal duration = outputAmount * di.Ingredient.OuncesPerSecond;
            return duration;
        }

        private void pourComplete(IAsyncResult ar)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AsyncCallback(onPourCompleted), ar);
        }

        private void onPourCompleted(IAsyncResult ar)
        {
            this.statusLabel.Content = "Cheers!";

            this.repeat.Visibility = Visibility.Visible;
            this.navBar.IsEnabled = true;
        }

        private void repeat_Click(object sender, RoutedEventArgs e)
        {
            onConnected(null);

            this.repeat.Visibility = Visibility.Hidden;
            this.navBar.IsEnabled = false;
        }

        private void gohome_Click(object sender, RoutedEventArgs e)
        {
            if (BarMonkeyContext.Current.CurrentUser.IsGuest == false)
            {
                this.NavigationService.Navigate(new UserHome());
            }
            else
            {
                PartyModeMainPage partyPage = new PartyModeMainPage();
                this.NavigationService.Navigate(partyPage);
            }            
        }        

    }
}
