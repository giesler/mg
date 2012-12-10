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
    /// Interaction logic for PartyModeMainPage.xaml
    /// </summary>
    public partial class PartyModeMainPage : Page
    {
        public PartyModeMainPage()
        {
            InitializeComponent();

            this.contentFrame.Navigated += new NavigatedEventHandler(contentFrame_Navigated);
        }

        void contentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Page page = e.Content as Page;
            if (page != null)
            {
                this.pageTitle.Content = page.Title.ToLower();
            }
        }
    }
}
