using System;
using System.Threading;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

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

	public class WebLinkPage: Crownwood.Magic.Controls.TabPage
	{
		private string key;
		private WebBrowserControl browser;

		public WebLinkPage(string title, string key): this(title, key, new WebBrowserControl())
		{}

		public WebLinkPage(string title, string key, WebBrowserControl browser): base(title, browser)
		{
			this.Title = title;
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
