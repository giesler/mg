using System;
using msn2.net.Controls;
using System.Text;
using System.Text.RegularExpressions;

namespace msn2.net.ProjectF
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Msn2Home: msn2.net.Controls.WebBrowser
	{
		#region Constructors
		public Msn2Home(msn2.net.Configuration.Data data): base(data, "msn2.net Home", 800, 600)
		{
			InternalConstructor();
		}		
		private void InternalConstructor()
		{
			string baseUrl = "http://home/_vti_bin/owssvr.dll?Using=Default%2ehtm";

			Crownwood.Magic.Controls.TabPage page = base.AddNewTab("Home", baseUrl, msn2.net.Controls.WebBrowserControl.DefaultClickBehavior.OpenLink);

			// Hook into navigate event
			WebBrowserControl browser = (WebBrowserControl) page.Control;
			browser.BeforeNavigateEvent += new BeforeNavigateDelegate(OnBeforeNavigate);

			// Bottom border controls
//			WebSearch webSearch = new WebSearch(this.Data);
//			webSearch.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
//			Crownwood.Magic.Docking.Content searchContent = dockManager.Contents.Add(webSearch, "Search");
//			Crownwood.Magic.Docking.WindowContent bottomWc = dockManager.AddContentWithState(searchContent, Crownwood.Magic.Docking.State.DockBottom);
//            
//			MSNBC.Headlines headlines = new MSNBC.Headlines(this.Data);
//			Crownwood.Magic.Docking.Content headlinesContent = dockManager.Contents.Add(headlines, "MSNBC Headlines");
//			dockManager.AddContentToZone(headlinesContent, bottomWc.ParentZone, 0);	

		}
		#endregion

		#region OnBeforeNavigate

		public void OnBeforeNavigate(object sender, BeforeNavigateEventArgs e)
		{
			WebBrowserControl browser = (WebBrowserControl) sender;

			System.Uri uri = new System.Uri(e.Url);

			// Check if we want base URL
			if (uri.PathAndQuery.Length == 0 && uri.Host.StartsWith("home"))
			{
				e.ClickBehavior = msn2.net.Controls.WebBrowserControl.DefaultClickBehavior.OpenLink;
			}
				// Ignore DispForm urls
			else if (Regex.IsMatch(e.Url, "http://home(.)*/(.)*DispForm.htm(.)*"))
			{
				e.ClickBehavior = msn2.net.Controls.WebBrowserControl.DefaultClickBehavior.OpenLink;
			}
				// Ignore EditForm urls
			else if (Regex.IsMatch(e.Url, "http://home(.)*/(.)*EditForm.htm(.)*"))
			{
				e.ClickBehavior = msn2.net.Controls.WebBrowserControl.DefaultClickBehavior.OpenLink;
			}
				// Check if URL is on this site, and if we should 'SPS'ize it
			else if (Regex.IsMatch(e.Url, "http://home(.)*/_vti_bin/owssvr.dll"))
			{
				e.ClickBehavior = msn2.net.Controls.WebBrowserControl.DefaultClickBehavior.OpenLink;
			}
				// Check if we need to add owssvr.dll
			else if (Regex.IsMatch(e.Url, "http://home(.)*/(.)*(htm|htm?(.)*)"))
			{
				// Need to add owssvr.dll
				string path = System.Web.HttpUtility.UrlDecode(uri.PathAndQuery.Substring(1));
				e.Url = uri.Scheme + Uri.SchemeDelimiter + uri.Host 
					+ "/_vti_bin/owssvr.dll?Using=" + System.Web.HttpUtility.UrlEncode(path);
			}
				// Otherwise open in new window
			else
			{
				e.ClickBehavior = msn2.net.Controls.WebBrowserControl.DefaultClickBehavior.OpenInNewWindow;
			}
		}

		#endregion
	}
}

