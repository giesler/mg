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

namespace BarMonkey.Activities.Admin.Editors
{
    /// <summary>
    /// Interaction logic for IngredientEditor.xaml
    /// </summary>
    public partial class IngredientEditor : UserControl
    {
        public IngredientEditor()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            int[] thresholds = new int[] { 5, 10, 15, 20, 25, 30 };

            
        }
    }
}
