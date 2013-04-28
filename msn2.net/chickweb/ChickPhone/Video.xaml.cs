using Microsoft.Phone.Controls;
using System;
using System.Windows.Navigation;

namespace ChickPhone
{
    public partial class Video : PhoneApplicationPage
    {
        public Video()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string video = NavigationContext.QueryString["v"];

            this.player.Source = new Uri("http://cams.msn2.net/GetVid.aspx?v=" + video);
        }
    }
}