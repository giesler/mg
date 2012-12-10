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
using BarMonkey.RelayControllerService;
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
        private RelayControllerService.RelayControllerClient relayClient = new BarMonkey.RelayControllerService.RelayControllerClient();

        public PourDrink()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // TODO: Close client
        }

        public void SetDrink(Drink drink, Container container)
        {
            this.drink = drink;
            this.container = container;

            this.drinkName.Content = this.drink.Name;

            this.repeat.IsEnabled = false;
            this.gohome.IsEnabled = false;

            this.statusLabel.Content = "connecting...";

            ThreadPool.QueueUserWorkItem(new WaitCallback(Connect));
        }

        private void Connect(object foo)
        {
            try
            {
                relayClient.ConnectTest();
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
        }

        private void connectComplete(IAsyncResult ar)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AsyncCallback(onConnected), ar);
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
            foreach (DrinkIngredient di in q)
            {
                decimal outputAmount = di.AmountOunces * offset;
                double duration = (double)outputAmount * BarMonkeyContext.Current.OuncesDispensedPerSecond;

                int relayNumber = (int)di.Ingredient.RelayId;

                items.Add(new BatchItem { Group = di.Group, RelayNumber = relayNumber, Seconds=duration});
            }

            BatchItem[] batch = new BatchItem[2];
            batch[0] = new BatchItem { Group = 1, RelayNumber = 2, Seconds = 2 };
            batch[1] = new BatchItem { Group = 2, RelayNumber = 3, Seconds = 5 };
            relayClient.BeginSendBatch(batch, pourComplete, new object());

            this.statusLabel.Content = "pouring...";

            BarMonkeyContext.Current.Drinks.LogDrink(drink, offset);
        }

        private void pourComplete(IAsyncResult ar)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AsyncCallback(onPourCompleted), ar);
        }

        private void onPourCompleted(IAsyncResult ar)
        {
            this.statusLabel.Content = "Cheers!";

            this.repeat.IsEnabled = true;
            this.gohome.IsEnabled = true;
        }

        private void repeat_Click(object sender, RoutedEventArgs e)
        {
            onConnected(null);
        }

        private void gohome_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserHome());
        }        

    }
}
