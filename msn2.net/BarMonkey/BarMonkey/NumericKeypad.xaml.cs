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
    /// Interaction logic for NumericKeypad.xaml
    /// </summary>
    public partial class NumericKeypad : UserControl
    {
        public NumericKeypad()
        {
            InitializeComponent();
        }

        public event EventHandler<NumericKeypadPressedEventArgs> KeyPressed;

        private void buttonPressed(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            Key key = Key.NumPad0;

            switch (button.Tag.ToString())
            {
                case "DEL":
                    key = Key.Delete;
                    break;
                case "1":
                    key = Key.NumPad1;
                    break;
                case "2":
                    key = Key.NumPad2;
                    break;
                case "3":
                    key = Key.NumPad3;
                    break;
                case "4":
                    key = Key.NumPad4;
                    break;
                case "5":
                    key = Key.NumPad5;
                    break;
                case "6":
                    key = Key.NumPad6;
                    break;
                case "7":
                    key = Key.NumPad7;
                    break;
                case "8":
                    key = Key.NumPad8;
                    break;
                case "9":
                    key = Key.NumPad9;
                    break;
                case "0":
                    key = Key.NumPad0;
                    break;
            }

            if (this.KeyPressed != null)
            {
                this.KeyPressed(this, new NumericKeypadPressedEventArgs() { Key = key });
            }
        }
    }

    public class NumericKeypadPressedEventArgs : EventArgs
    {
        public Key Key { get; set; }
    }
}
