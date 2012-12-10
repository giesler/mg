﻿using System;
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
        private bool changingFlowRate = false;
        private bool changingRemaining = false;
        private bool isAdmin = true;

        public RelaySettings()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //this.isAdmin = BarMonkeyContext.Current.CurrentUser.IsAdmin;
            this.EnableControls();

            this.LoadDatabaseDropdowns();

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

            this.relay.SelectedIndex = 0;
            
            this.navBar.BackClicked += delegate(object o, EventArgs a) { base.NavigationService.GoBack(); };
            this.navBar.HomeClicked += delegate(object o, EventArgs a) { base.NavigationService.Navigate(new PartyModeHomePage()); };
        }

        void LoadDatabaseDropdowns()
        {
            int relaySelectedIndex = this.relay.SelectedIndex;
            this.changingRelay = true;
            this.relay.ItemsSource = BarMonkeyContext.Current.Relays.GetRelays();
            if (relaySelectedIndex >= 0)
            {
                this.relay.SelectedIndex = relaySelectedIndex;
            }
            this.changingRelay = false;

            int ingredientSelectedIndex = this.ingredient.SelectedIndex;
            this.changingIngredient = true;
            this.ingredient.ItemsSource = BarMonkeyContext.Current.Ingredients.GetIngredients();
            if (ingredientSelectedIndex >= 0)
            {
                this.ingredient.SelectedIndex = ingredientSelectedIndex;
            }
            this.changingIngredient = false;
        }

        private void relay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.changingRelay)
            {
                this.changingRelay = true;
                this.changingFlowRate = true;
                this.changingRemaining = true;

                Relay relay = this.relay.SelectedItem as Relay;

                BarMonkeyContext.Current.Reload();
                this.LoadDatabaseDropdowns();

                Ingredient ingredient = BarMonkeyContext.Current.Ingredients.GetIngredientOnRelay(relay);
                if (ingredient != null)
                {
                    this.ingredient.SelectedItem = ingredient;
                    this.ouncesPerSecond.SelectedItem = ingredient.OuncesPerSecond;
                    this.ouncesRemaining.SelectedIndex = (int)ingredient.RemainingOunces;
                }
                else
                {
                    this.ingredient.SelectedIndex = -1;
                    this.ouncesPerSecond.SelectedItem = -1;
                    this.ouncesRemaining.SelectedIndex = -1;
                }

                this.changingFlowRate = false;
                this.changingRemaining = false;
                this.changingRelay = false;
            }
        }

        private void ingredient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.changingIngredient)
            {
                this.changingIngredient = true;

                Relay relay = this.relay.SelectedItem as Relay;
                Ingredient newIngredient = this.ingredient.SelectedItem as Ingredient;
                
                if (newIngredient != null)
                {
                    if (newIngredient.Relay != null)
                    {
                        // Are we replacing an assignment?
                        Ingredient current = BarMonkeyContext.Current.Ingredients.GetIngredientOnRelay(relay);
                        if (current == null)
                        {
                            this.SetIngredient(relay, newIngredient, e);
                        }
                        else if (current.Id != newIngredient.Id)
                        {
                            string message = string.Format(
                                "Would you like to change '{0}' from relay {1} to relay {2} and remove '{3}'?",
                                newIngredient.Name,
                                newIngredient.RelayId,
                                relay.Id,
                                current.Name);
                            MessageBoxResult result = System.Windows.MessageBox.Show(
                                message,
                                "Confirm Relay Switch",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                this.SetIngredient(relay, newIngredient, e);
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
                        else
                        {
                            System.Diagnostics.Debug.Write("Matching");

                        }
                    }
                    else
                    {
                        this.SetIngredient(relay, newIngredient, e);
                    }
                }

                this.changingIngredient = false;
            }
        }

        void SetIngredient(Relay relay, Ingredient ingredient, SelectionChangedEventArgs e)
        {
            try
            {
                BarMonkeyContext.Current.Relays.SetIngredient(relay, ingredient);
            }
            catch (Exception ex)
            {
                if (e.RemovedItems.Count > 0)
                {
                    this.relay.SelectedItem = e.RemovedItems[0];
                }

                System.Windows.MessageBox.Show(ex.Message, "Error setting ingredient", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void DisableControls()
        {
            this.relay.IsEnabled = false;
            this.ingredient.IsEnabled = false;
            this.ouncesPerSecond.IsEnabled = false;
            this.ouncesRemaining.IsEnabled = false;
            this.seconds.IsEnabled = false;
            this.openRelay.IsEnabled = false;
            this.navBar.IsEnabled = false;
            this.plus.IsEnabled = false;
            this.minus.IsEnabled = false;
            this.allOff.IsEnabled = false;
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
                this.statusLabel.Text = ex.Message;
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
            this.relay.IsEnabled = true;
            this.ingredient.IsEnabled = this.isAdmin;
            this.ouncesPerSecond.IsEnabled = this.isAdmin;
            this.ouncesRemaining.IsEnabled = this.isAdmin;
            this.seconds.IsEnabled = true;
            this.openRelay.IsEnabled = true;
            this.navBar.IsEnabled = true;
            this.plus.IsEnabled = true;
            this.minus.IsEnabled = true;
            this.allOff.IsEnabled = true;
        }

        private void displayException(object o)
        {
            Exception exception = (Exception)o;
            this.statusLabel.Text = exception.ToString();

            this.EnableControls();

            this.relayClient = new RelayControllerClient();
        }

        private void allOff_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(this.Connect, false);

            this.DisableControls();

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
