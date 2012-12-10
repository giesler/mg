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
using System.Threading;
using msn2.net.BarMonkey.RelayControllerService;
using System.Windows.Threading;
using System.Windows.Forms;

namespace msn2.net.BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for RelaySettings.xaml
    /// </summary>
    public partial class RelaySettings : Page
    {
        private RelayControllerClient relayClient = new RelayControllerClient();
        private bool changingRelay = false;
        private bool changingIngredient = false;

        public RelaySettings()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.relay.ItemsSource = BarMonkeyContext.Current.Relays.GetRelays();
            this.ingredient.ItemsSource = BarMonkeyContext.Current.Ingredients.GetIngredients();
            this.seconds.ItemsSource = new int[] { 1, 2, 5, 10, 15, 20 };

            this.relay.SelectedIndex = 0;
            this.seconds.SelectedIndex = 0;
            
            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
        }

        private void relay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Relay relay = this.relay.SelectedItem as Relay;

            this.changingRelay = true;

            Ingredient ingredient = BarMonkeyContext.Current.Ingredients.GetIngredientOnRelay(relay);
            if (ingredient != null)
            {
                this.ingredient.SelectedItem = ingredient;
            }
            else
            {
                this.ingredient.SelectedIndex = -1;
            }

            this.changingRelay = false;
        }

        private void ingredient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.changingRelay == false && this.changingIngredient == false)
            {
                this.changingIngredient = true;

                Relay relay = this.relay.SelectedItem as Relay;
                Ingredient ingredient = this.ingredient.SelectedItem as Ingredient;

                if (ingredient != null)
                {
                    if (ingredient.Relay != null)
                    {
                        Ingredient current = BarMonkeyContext.Current.Ingredients.GetIngredientOnRelay(relay);
                        if (current == null)
                        {
                            BarMonkeyContext.Current.Relays.SetIngredient(relay, ingredient);
                        }
                        else
                        {
                            string message = string.Format(
                                "Would you like to change '{0}' from relay {1} to relay {2} and remove '{3}'?",
                                ingredient.Name,
                                ingredient.RelayId,
                                relay.Id,
                                current.Name);
                            MessageBoxResult result = System.Windows.MessageBox.Show(
                                message,
                                "Confirm Relay Switch",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                BarMonkeyContext.Current.Relays.SetIngredient(relay, ingredient);
                            }
                            else
                            {
                                if (e.RemovedItems.Count > 0)
                                {
                                    this.ingredient.SelectedItem = e.RemovedItems[0];
                                }
                                else
                                {
                                    this.ingredient.SelectedIndex = -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        BarMonkeyContext.Current.Relays.SetIngredient(relay, ingredient);
                    }
                }

                this.changingIngredient = false;
            }
        }

        private void openRelay_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(this.Connect, true);

            this.relay.IsEnabled = false;
            this.ingredient.IsEnabled = false;
            this.seconds.IsEnabled = false;
            this.openRelay.IsEnabled = false;
            this.navBar.IsEnabled = false;

            this.statusLabel.Text = "connecting...";
        }

        private void Connect(object openOnConnect)
        {
            try
            {
                relayClient.ConnectTest();
                
                if ((bool)openOnConnect == true)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new MethodInvoker(this.onConnected));
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new MethodInvoker(this.connectForAllOff));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new WaitCallback(displayException), ex);
            }
        }

        private void onConnected()
        {
            this.statusLabel.Text = "sending...";

            int seconds = int.Parse(this.seconds.SelectedItem.ToString());
            Relay relay = (Relay)this.relay.SelectedItem;

            List<BatchItem> items = new List<BatchItem>();
            items.Add(new BatchItem() { RelayNumber = relay.Id, Seconds = seconds });

            relayClient.BeginSendBatch(items.ToArray<BatchItem>(), pourComplete, null);

            this.statusLabel.Text = "pouring...";
        }

        private void pourComplete(IAsyncResult ar)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AsyncCallback(onPourCompleted), ar);
        }

        private void onPourCompleted(IAsyncResult ar)
        {
            this.statusLabel.Text = "done";

            this.relay.IsEnabled = true;
            this.ingredient.IsEnabled = true;
            this.seconds.IsEnabled = true;
            this.openRelay.IsEnabled = true;
            this.navBar.IsEnabled = true;
        }


        private void displayException(object o)
        {
            Exception exception = (Exception)o;
            this.statusLabel.Text = exception.ToString();

            this.relay.IsEnabled = true;
            this.ingredient.IsEnabled = true;
            this.seconds.IsEnabled = true;
            this.openRelay.IsEnabled = true;
            this.navBar.IsEnabled = true;

            this.relayClient = new RelayControllerClient();
        }

        private void allOff_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(this.Connect, false);

            this.relay.IsEnabled = false;
            this.ingredient.IsEnabled = false;
            this.seconds.IsEnabled = false;
            this.openRelay.IsEnabled = false;
            this.navBar.IsEnabled = false;

            this.statusLabel.Text = "connecting...";
        }

        private void connectForAllOff()
        {
            this.statusLabel.Text = "sending all off...";

            this.relayClient.BeginTurnAllOff(new AsyncCallback(this.OnConnectedForAllOff), new object());
        }

        private void OnConnectedForAllOff(IAsyncResult result)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AsyncCallback(this.onPourCompleted), null); 
        }
    }
}
