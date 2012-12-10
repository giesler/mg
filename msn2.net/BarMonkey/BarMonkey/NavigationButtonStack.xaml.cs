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

namespace msn2.net.BarMonkey
{
    /// <summary>
    /// Interaction logic for NavigationButtonStack.xaml
    /// </summary>
    public partial class NavigationButtonStack : UserControl
    {
        public NavigationButtonStack()
        {
            InitializeComponent();
        }

        public void AddCommands(List<Activity> activities)
        {
            this.activityList.ItemsSource = activities;
        }

        private void selectActivity_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string url = b.Tag.ToString();

            if (url != null)
            {
                Uri uri = new Uri(string.Format(
                        "pack://application:,,,/{0}",
                        url));
                if (this.NavigateToUri != null)
                {
                    this.NavigateToUri(uri);
                }
            }
        }

        public event NavigateToUriEventHandler NavigateToUri;
    }

    public delegate void NavigateToUriEventHandler(Uri uri);

    public class Activity
    {
        public string Name { get; set; }

        public string PageUrl { get; set; }

        private bool isEnabled = true;
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                this.isEnabled = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
