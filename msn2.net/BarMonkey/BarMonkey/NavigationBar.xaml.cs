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
    /// Interaction logic for NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        public NavigationBar()
        {
            InitializeComponent();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (this.BackClicked != null)
            {
                this.BackClicked(this, EventArgs.Empty);
            }
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            if (this.HomeClicked != null)
            {
                this.HomeClicked(this, EventArgs.Empty);
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (this.NextClicked != null)
            {
                this.NextClicked(this, EventArgs.Empty);
            }
        }

        public event EventHandler BackClicked;
        public event EventHandler HomeClicked;
        public event EventHandler NextClicked;

        public bool BackEnabled
        {
            get
            {
                return this.back.IsEnabled;
            }
            set
            {
                this.back.IsEnabled = value;
            }
        }

        public bool HomeEnabled
        {
            get
            {
                return this.home.IsEnabled;
            }
            set
            {
                this.home.IsEnabled = value;
            }
        }

        public bool NextEnabled
        {
            get
            {
                return this.next.IsEnabled;
            }
            set
            {
                this.next.IsEnabled = value;
            }
        }
    }
}
