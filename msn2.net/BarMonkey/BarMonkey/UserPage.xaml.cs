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
using BarMonkeyControls;
using msn2.net.BarMonkey;

namespace BarMonkey
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : System.Windows.Controls.Page
    {
        public UserPage()
        {
            InitializeComponent();

            this.UpdateUserName();
            this.pageTitle.Content = this.Title;

            this.contentFrame.Navigated += new NavigatedEventHandler(contentFrame_Navigated);
        }

        void contentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Page page = e.Content as Page;
            if (page != null)
            {
                this.pageTitle.Content = page.Title.ToLower();
                this.UpdateUserName();
            }
        }

        protected void logout_click(object sender, EventArgs e)
        {
            BarMonkeyContext.Current.ImpersonateUser = null;
            this.NavigationService.Navigate(new Welcome());
        }

        protected void home_click(object sender, EventArgs e)
        {
            BarMonkeyContext.Current.ImpersonateUser = null;
            this.contentFrame.Navigate(new UserHome());
        }

        private void UpdateUserName()
        {
            if (BarMonkeyContext.Current.ImpersonateUser == null)
            {
                this.userName.Content = BarMonkeyContext.Current.CurrentUser.Name;
            }
            else
            {
                this.userName.Content = BarMonkeyContext.Current.CurrentUser.Name 
                    + " choosing for " + BarMonkeyContext.Current.ImpersonateUser.Name;
            }
        }
    }
}
