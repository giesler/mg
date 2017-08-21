using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Forms;
using msn2.net.BarMonkey;

namespace BarMonkey
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            if (Screen.PrimaryScreen.Bounds.Width < 1025)
            {
                this.WindowState = WindowState.Maximized;
            }

            BarMonkeyContext.Current.Login(9);

            if (Environment.MachineName.ToLowerInvariant() == "chef")
            {
                this.frame.Height = 768;
                this.frame.Width = 1024;
            }
        }
    }
}
