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

namespace BarMonkey
{
    /// <summary>
    /// Interaction logic for KeyboardControl.xaml
    /// </summary>
    public partial class KeyboardControl : UserControl
    {
        public KeyboardControl()
        {
            InitializeComponent();
        }

        public event KeyboardButtonPressedEvent KeyboardButtonPressed;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (this.KeyboardButtonPressed != null)
            {
                this.KeyboardButtonPressed(b.Content.ToString());
            }
        }
    }

    public delegate void KeyboardButtonPressedEvent(string content);
}
