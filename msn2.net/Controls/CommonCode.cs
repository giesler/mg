using System;
using System.Threading;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for CommonCode.
	/// </summary>
	public class CommonCode
	{
		public CommonCode()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}

    public class ShellTabPage: TabPage
    {
        private Control control = null;

        public ShellTabPage(string title): base(title)
        {
        }

        public ShellTabPage(string title, Control control): base(title)
        {
            control.Dock = DockStyle.Fill;
            this.Controls.Add(control);
        }

        public Control Control
        {
            get
            {
                return this.control;
            }
        }
    }

    public class WebLinkPage : ShellTabPage
	{
		private string key;
		private WebBrowserControl browser;

		public WebLinkPage(string title, string key): this(title, key, new WebBrowserControl())
		{}

		public WebLinkPage(string title, string key, WebBrowserControl browser): base(title, browser)
		{
			this.key   = key;
			this.browser = browser;
			browser.Visible = true;
		}

		public string Key
		{
			get { return key; }
			set { key = value; }
		}

		public void Navigate(string url)
		{
            browser.Navigate(String.Format(url, key));			
		}
		
	}

}
