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
using BarMonkey;
using msn2.net.BarMonkey;

namespace BarMonkeyControls
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : System.Windows.Controls.Page
    {
        public Welcome()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            userList.ItemsSource = BarMonkeyContext.Current.Users.GetUsers();
        }

        private void user_click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int userId = (int) button.Tag;

            BarMonkeyContext.Current.Login(userId);
            UserPage userPage = new UserPage();
            this.NavigationService.Navigate(userPage);
        }

        private void partyMode_Click(object sender, RoutedEventArgs e)
        {
            BarMonkeyContext.Current.Login(9);
            
            PartyModeMainPage partyPage = new PartyModeMainPage();
            this.NavigationService.Navigate(partyPage);
        }


    }
}