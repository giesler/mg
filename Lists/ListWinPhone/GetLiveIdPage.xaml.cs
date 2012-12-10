using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Live;
using System.Data.Services.Client;
using System.Threading;
using System.Diagnostics;
using giesler.org.lists.ListData;

namespace giesler.org.lists
{
    public partial class GetLiveIdPage : PhoneApplicationPage
    {
        const string ClientId = "0000000048042355";
        const string ClientSecret = "mitXACdPcReJ5TznIYRyihzVwPsDquBl";

        LiveDataContext dataContext = new LiveDataContext();
        AppInformation appInfo;
        AppAuthentication appAuth;
        ClientAuthenticationData listAuth;

        public GetLiveIdPage()
        {
            InitializeComponent();

            this.dataContext.SignInCompleted += new SignInCompletedEventHandler(dataContext_SignInCompleted);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            appInfo = new AppInformation(ClientId, ClientSecret);
            appInfo.RequestedOffers.Add(new Offer()
            {
                Name = "WL_Contacts",
                Action = "View"
            });

            if (string.IsNullOrEmpty(App.LiveIdUserId) || string.IsNullOrEmpty(App.LiveIdAccessToken))
            {
                try
                {
                    this.dataContext.SignInAsync(this.browser, appInfo, null);
                }
                catch (Exception ex)
                {
                    string error = string.Format("Windows Live sign in failed with error '{0}'.  Please try again later.", ex.Message);
                    MessageBox.Show(error, "Sign in error", MessageBoxButton.OK);
                }
            }
            else
            {
                this.browser.Visibility = System.Windows.Visibility.Collapsed;
                this.loading.Visibility = System.Windows.Visibility.Visible;

                this.loading.Text = "signing in...";

                this.appAuth = new AppAuthentication(App.LiveIdAccessToken, App.LiveIdRefreshToken);
                appInfo.AuthInfo = this.appAuth;
                this.appAuth.UserId = App.LiveIdUserId;
                this.dataContext.RefreshAccessTokenAsync(appInfo);
            }
        }

        void dataContext_SignInCompleted(object sender, SignInCompletedEventArgs e)
        {
            if (!e.Succeeded)
            {
                string error = (e.AuthInfo.Error == null) ? "Unknown error" : e.AuthInfo.Error.Message;
                MessageBox.Show(error, "Sign in error", MessageBoxButton.OK);
            }
            else
            {
                this.appAuth = e.AuthInfo;
                this.Dispatcher.BeginInvoke(new WaitCallback(SignInCompleted), new object());
            }
        }

        void SignInCompleted(object sender)
        {
            this.browser.Visibility = System.Windows.Visibility.Collapsed;
            this.loading.Visibility = System.Windows.Visibility.Visible;

            this.loading.Text = "loading profile...";

            ListDataServiceClient authClient = new ListDataServiceClient();
            authClient.GetPersonCompleted += new EventHandler<GetPersonCompletedEventArgs>(authClient_GetPersonCompleted);
            authClient.GetPersonAsync(this.appAuth.UserId, string.Empty);
        }

        void authClient_GetPersonCompleted(object sender, GetPersonCompletedEventArgs e)
        {
            ListDataServiceClient client = (ListDataServiceClient)sender;
            client.GetPersonCompleted -= new EventHandler<GetPersonCompletedEventArgs>(authClient_GetPersonCompleted);

            this.listAuth = new ClientAuthenticationData { PersonUniqueId = e.Result.UniqueId};

            client.AddDeviceCompleted += new EventHandler<AddDeviceCompletedEventArgs>(authClient_AddDeviceCompleted);
            client.AddDeviceAsync(this.listAuth, "WP7");
        }

        void authClient_AddDeviceCompleted(object sender, AddDeviceCompletedEventArgs e)
        {
            this.listAuth.DeviceUniqueId = e.Result.UniqueId;
            App.Current.SetClientAuth(this.listAuth, this.appAuth);

            this.Dispatcher.BeginInvoke(new WaitCallback(CompleteLoad), new object());
        }

        void CompleteLoad(object sender)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}