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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BarMonkey;
using BarMonkey.Activities;

namespace msn2.net.BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for BrowseByAlpha.xaml
    /// </summary>
    public partial class BrowseByAlpha : Page
    {
        private Container container = null;
        private DispatcherTimer timer = null;
        private DateTime lastInput = DateTime.Now;
        private string filter = "Margarita";

        public BrowseByAlpha()
        {
            InitializeComponent();
        }



        public bool IsItemSelected
        {
            get { return (bool)GetValue(IsItemSelectedProperty); }
            set { SetValue(IsItemSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsItemSelectedProperty =
            DependencyProperty.Register("IsItemSelected", typeof(bool), typeof(BrowseByAlpha), new UIPropertyMetadata(false));

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.LoadDrinks();

            this.containers.ItemsSource = App.Containers;
            this.containers.SelectedIndex = this.containers.Items.Count - 1;

            this.timer = new DispatcherTimer(TimeSpan.FromSeconds(10), DispatcherPriority.Background, this.OnTimer, this.Dispatcher);
        }

        void LoadDrinks()
        {
            if (this.drinkList.ItemsSource == null && this.drinkList.Items.Count > 0)
            {
                this.drinkList.Items.Clear();
            }

            if (string.IsNullOrEmpty(this.filter))
            {
                this.drinkList.ItemsSource = App.Drinks;
            }
            else if (this.filter.ToLower() == "top")
            {
                this.drinkList.ItemsSource = App.TopDrinks;
            }
            else
            {
                this.drinkList.ItemsSource = App.Drinks.Where(d => d.Category.ToLower() == this.filter.ToLower());
            }
            this.drinkList.IsEnabled = true;
        }

        void OnTimer(object sender, EventArgs e)
        {
            if (this.lastInput.AddSeconds(120) < DateTime.Now)
            {
                this.timer.IsEnabled = false;
                this.timer.Stop();

                if (this.NavigationService != null)
                {
                    NavigationService.GoBack();
                }
            }
        }

        void OnPourDrink(object sender, EventArgs e)
        {
            if (this.drinkList.SelectedIndex >= 0)
            {
                PourDrink pour = new PourDrink();
                pour.SetDrink(this.drinkList.SelectedItem as Drink, this.containers.SelectedItem as Container);
                this.NavigationService.Navigate(pour);

                this.drinkList.SelectedIndex = -1;
            }
        }

        private void drinkList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.IsItemSelected = this.drinkList.SelectedIndex >= 0;

            this.pourDrink.IsEnabled = this.IsItemSelected;
            this.lastInput = DateTime.Now;

            GradientStopCollection stops = new GradientStopCollection();
            stops.Add(new GradientStop(Colors.ForestGreen, 0.25));
            stops.Add(new GradientStop(Colors.White, 0));
            stops.Add(new GradientStop(Colors.White, 0.5));
            stops.Add(new GradientStop(Colors.ForestGreen, 0.75));
            stops.Add(new GradientStop(Colors.White, 1));

            if (this.IsItemSelected)
            {
                LinearGradientBrush brush = new LinearGradientBrush(stops, new Point(0.5, 0), new Point(0.5, 1));

                TransformGroup transform = new TransformGroup();
                transform.Children.Add(new ScaleTransform { CenterX = 0.5, CenterY = 0.5 });
                transform.Children.Add(new SkewTransform { CenterX = 0.5, CenterY = 0.5 });
                transform.Children.Add(new RotateTransform { CenterX = 0.5, CenterY = 0.5, Angle = 90 });
                transform.Children.Add(new TranslateTransform());
                brush.RelativeTransform = transform;

                this.pourDrink.Foreground = brush;
            }

            /*
             *                    <LinearGradientBrush EndPoint="0.5,1" StartPoint="0.5,0">
                        <LinearGradientBrush.RelativeTransform>
                            <TransformGroup>
                                <ScaleTransform CenterY="0.5" CenterX="0.5"/>
                                <SkewTransform CenterY="0.5" CenterX="0.5"/>
                                <RotateTransform Angle="90" CenterY="0.5" CenterX="0.5"/>
                                <TranslateTransform/>
                            </TransformGroup>
                        </LinearGradientBrush.RelativeTransform>
                        <GradientStop Color="#FF03A203" Offset="0.274"/>
                        <GradientStop Color="White"/>
                        <GradientStop Color="#FFBCBDBC" Offset="0.513"/>
                        <GradientStop Color="#FF19A819" Offset="0.726"/>
                        <GradientStop Color="#FFBEBFBE" Offset="1"/>
                    </LinearGradientBrush>
            */

        }

        private void containers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.container = this.containers.SelectedItem as Container;
            this.lastInput = DateTime.Now;
        }

        private void OnFilterMargaritas(object sender, MouseButtonEventArgs e)
        {
            this.SetFilter("Margarita");
            this.filterMargaritas.FontWeight = FontWeights.Bold;
            this.filterMargaritas.Foreground = new SolidColorBrush(Colors.LightBlue);
        }

        private void OnFilterCocktails(object sender, MouseButtonEventArgs e)
        {
            this.SetFilter("Cocktail");
            this.filterCocktails.FontWeight = FontWeights.Bold;
            this.filterCocktails.Foreground = new SolidColorBrush(Colors.LightBlue);
        }

        private void OnFilterTop(object sender, MouseButtonEventArgs e)
        {
            this.SetFilter("Top");
            this.filterTop.FontWeight = FontWeights.Bold;
            this.filterTop.Foreground = new SolidColorBrush(Colors.LightBlue);
        }

        private void OnFilterAll(object sender, MouseButtonEventArgs e)
        {
            this.SetFilter("");
            this.filterAll.FontWeight = FontWeights.Bold;
            this.filterAll.Foreground = new SolidColorBrush(Colors.LightBlue);
        }

        void SetFilter(string filter)
        {
            this.filter = filter;
            this.filterAll.FontWeight = FontWeights.Normal;
            this.filterCocktails.FontWeight = FontWeights.Normal;
            this.filterMargaritas.FontWeight = FontWeights.Normal;

            this.filterAll.Foreground = new SolidColorBrush(Colors.White);
            this.filterCocktails.Foreground = new SolidColorBrush(Colors.White);
            this.filterMargaritas.Foreground = new SolidColorBrush(Colors.White);

            this.LoadDrinks();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            base.NavigationService.GoBack();
        }
    }
}
