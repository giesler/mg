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
using msn2.net.BarMonkey;

namespace BarMonkey.Activities
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class SelectUser : Page
    {
        public SelectUser()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            List<User> users = BarMonkeyContext.Current.Users.GetUsers();

            users = (from u in users
                     where u.Id != BarMonkeyContext.Current.CurrentUser.Id
                     select u).ToList<User>();

            this.userList.ItemsSource = users;
        }

        private void selectUser_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            User user = BarMonkeyContext.Current.Users.GetUser((int)b.Tag);
            BarMonkeyContext.Current.ImpersonateUser = user;

            ImpersonateUserHome home = new ImpersonateUserHome();
            base.NavigationService.Navigate(home);
        }
    }
}
