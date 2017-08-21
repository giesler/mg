using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLExpressControls
{
    public partial class ItemEditor : UserControl
    {
        public ItemEditor()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Cancelled = true;
            this.Visibility = Visibility.Collapsed;
        }

        public bool Cancelled { get; private set; }
    }
}
