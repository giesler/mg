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
    /// Interaction logic for IngredientSettings.xaml
    /// </summary>
    public partial class IngredientSettings : Page
    {
        private RelayControllerClient relayClient = new RelayControllerClient();
        private bool changingRelay = false;
        private bool changingIngredient = false;
        private bool changingFlowRate = false;
        private bool changingRemaining = false;
        private bool isAdmin = true;

        public IngredientSettings()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.EnableControls();

            //this.isAdmin = BarMonkeyContext.Current.CurrentUser.IsAdmin;

            this.ingredient.ItemsSource = BarMonkeyContext.Current.Ingredients.GetIngredients();
            this.relay.ItemsSource = BarMonkeyContext.Current.Relays.GetRelays();

            foreach (Ingredient i in this.ingredient.Items)
            {
                if (i.Name.ToString() == "Water")
                {
                    this.ingredient.SelectedItem = i;
                    break;
                }
            }

            List<decimal> ouncesItems = new List<decimal>();
            for (decimal i = 0.0M; i < 200.0M; i++)
            {
                ouncesItems.Add(i * 0.1M);
            }
            this.ouncesPerSecond.ItemsSource = ouncesItems;

            List<int> ounces = new List<int>();
            for (int i = 0; i < 256; i++)
            {
                ounces.Add(i);
            }
            this.ouncesRemaining.ItemsSource = ounces;

            this.ingredient.SelectedIndex = 0;

            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
        }

        private void relay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.changingRelay == false && this.changingIngredient == false)
            {
                this.changingRelay = true;

                Relay relay = this.relay.SelectedItem as Relay;
                Ingredient ingredient = this.ingredient.SelectedItem as Ingredient;

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
                            BarMonkeyContext.Current.Clone().Relays.SetIngredient(relay, ingredient);
                        }
                        else
                        {
                            if (e.RemovedItems.Count > 0)
                            {
                                this.relay.SelectedItem = e.RemovedItems[0];
                            }
                            else
                            {
                                this.relay.SelectedIndex = -1;
                            }
                        }
                    }
                }
                else
                {
                    BarMonkeyContext.Current.Relays.SetIngredient(relay, ingredient);
                }

                this.changingIngredient = false;
            }
        }

        private void ingredient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ingredient ingredient = (Ingredient)this.ingredient.SelectedItem;

            this.changingIngredient = true;

            this.changingRelay = true;
            if (ingredient.Relay != null)
            {
                this.relay.SelectedItem = ingredient.Relay;
            }
            else
            {
                this.relay.SelectedIndex = -1;
            }
            this.changingRelay = false;

            this.changingFlowRate = true;
            this.ouncesPerSecond.SelectedItem = ingredient.OuncesPerSecond;
            this.changingFlowRate = false;

            if (ingredient.RemainingOunces >= 0)
            {
                this.changingRemaining = true;
                this.ouncesRemaining.SelectedIndex = (int)ingredient.RemainingOunces;
                this.changingRemaining = false;
            }

            this.changingIngredient = false;
        }

        private void ouncesPerSecond_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.changingFlowRate == false)
            {
                this.changingFlowRate = true;

                Ingredient ingredient = this.ingredient.SelectedItem as Ingredient;
                ingredient.OuncesPerSecond = (decimal)this.ouncesPerSecond.SelectedItem;
                BarMonkeyContext.Current.Ingredients.UpdateIngredient(ingredient);

                this.changingFlowRate = false;
            }
        }

        private void ouncesRemaining_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.changingRemaining == false)
            {
                this.changingRemaining = true;

                Ingredient ingredient = this.ingredient.SelectedItem as Ingredient;
                ingredient.RemainingOunces = (decimal)(int)this.ouncesRemaining.SelectedItem;
                BarMonkeyContext.Current.Ingredients.UpdateIngredient(ingredient);

                this.changingRemaining = false;
            }
        }

        private void openRelay_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(this.Connect, true);

            this.DisableControls();

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

            try
            {
                double seconds = double.Parse(this.seconds.Text);
                Relay relay = (Relay)this.relay.SelectedItem;

                List<BatchItem> items = new List<BatchItem>();
                items.Add(new BatchItem() { RelayNumber = relay.Id, Seconds = seconds, Group = 1 });

                relayClient.BeginSendBatch(items.ToArray<BatchItem>(), pourComplete, null);

                this.statusLabel.Text = "pouring...";
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new WaitCallback(displayException), ex);
            }
        }

        private void pourComplete(IAsyncResult ar)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AsyncCallback(onPourCompleted), ar);
        }

        private void onPourCompleted(IAsyncResult ar)
        {
            this.statusLabel.Text = "done";

            this.EnableControls();
        }

        private void EnableControls()
        {
            this.relay.IsEnabled = this.isAdmin;
            this.ingredient.IsEnabled = true;
            this.ouncesPerSecond.IsEnabled = this.isAdmin;
            this.ouncesRemaining.IsEnabled = this.isAdmin;
            this.seconds.IsEnabled = true;
            this.plus.IsEnabled = true;
            this.minus.IsEnabled = true;
            this.openRelay.IsEnabled = true;
            this.navBar.IsEnabled = true;
            this.allOff.IsEnabled = true;
        }


        private void displayException(object o)
        {
            Exception exception = (Exception)o;
            this.statusLabel.Text = exception.ToString();

            this.EnableControls();
        }

        private void allOff_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(this.Connect, false);

            this.DisableControls();

            this.statusLabel.Text = "connecting...";
        }

        private void DisableControls()
        {
            this.relay.IsEnabled = false;
            this.ingredient.IsEnabled = false;
            this.ouncesPerSecond.IsEnabled = false;
            this.ouncesRemaining.IsEnabled = false;
            this.seconds.IsEnabled = false;
            this.plus.IsEnabled = false;
            this.minus.IsEnabled = false;
            this.openRelay.IsEnabled = false;
            this.navBar.IsEnabled = false;
            this.allOff.IsEnabled = false;
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

        private void minus_Click(object sender, RoutedEventArgs e)
        {
            double secs = double.Parse(this.seconds.Text);
            secs -= 0.5;
            if (secs > 0)
            {
                this.seconds.Text = secs.ToString();
            }
        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            double secs = double.Parse(this.seconds.Text);
            secs += 0.5;
            this.seconds.Text = secs.ToString();
        }
    }
}
