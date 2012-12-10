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
    /// Interaction logic for UserHome.xaml
    /// </summary>
    public partial class UserHome : Page
    {
        public UserHome()
        {
            InitializeComponent();

            this.activityList.Items.Add(new Activity { Name = "Search drinks" });
            this.activityList.Items.Add(new Activity { Name = "Make a drink for..." });
            this.activityList.Items.Add(new Activity { Name = "Custom drink" });
            this.activityList.Items.Add(new Activity { Name = "Admin" });
            this.activityList.Items.Add(new Activity { Name = "My Settings" });
        }
    }
}
