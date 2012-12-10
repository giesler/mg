namespace Microsoft.Live
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;

    public class SignInDialog //: System.Windows.Controls.Panel
    {
        public event EventHandler SignInComplete;

        private AppInformation appInfo;
        //private WebBrowser browser;
        //private const double DialogHeight = 432.0;
        //private const double DialogWidth = 484.0;
        //private Grid layoutRoot;

        public WebBrowser Browser { get; set; }

        public bool? DialogResult { get; set; }

        public SignInDialog(WebBrowser host, AppInformation appInfo)
        {
            if (appInfo == null)
            {
                throw new ArgumentNullException("appInfo");
            }
            this.appInfo = appInfo;
            this.InitializeComponent();
            this.Browser = host;
            this.Browser.LoadCompleted += new LoadCompletedEventHandler(this.BrowserLoadCompleted);
            this.Browser.Source = new Uri(WrapProtocolHelper.BuildConsentUrl(this.appInfo));
        }

        private void BrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            if (WrapProtocolHelper.ParseConsentResponseHTML(this.Browser.SaveToString(), this.appInfo))
            {
                DialogResult = true;
                if (SignInComplete != null)
                {
                    SignInComplete(this, EventArgs.Empty);
                }
                this.Browser.LoadCompleted -= new LoadCompletedEventHandler(this.BrowserLoadCompleted);
                
                //base.set_DialogResult(true);
                //base.Close();
            }
        }

        private void InitializeComponent()
        {
            ////base.set_Title(StringResource.SignInDialogTitle);
            //base.Width = 484.0;
            //base.Height = 432.0;
            ////base.Margin = new Thickness(0.0);
            //this.layoutRoot = new Grid();
            //this.layoutRoot.HorizontalAlignment = HorizontalAlignment.Stretch;
            //this.layoutRoot.VerticalAlignment = VerticalAlignment.Stretch;
            ////this.layoutRoot.Name = "LayoutRoot";
            ////this.layoutRoot.Margin = new Thickness(0.0);
            ////this.Children.Add(this.layoutRoot);
            ////base.Content = this.layoutRoot;
            //this.browser = new WebBrowser();
            //this.browser.IsScriptEnabled = true;
            //this.browser.Width = 484.0;
            //this.browser.Height = 432.0;
            ////this.browser.HorizontalAlignment = HorizontalAlignment.Stretch;
            ////this.browser.VerticalAlignment = VerticalAlignment.Stretch;
            ////this.browser.Margin = new Thickness(0.0);
            //this.Children.Add(this.browser);
        }
    }
}

