using System;
using msn2.net.Common;
using msn2.net.Configuration;
using msn2.net.Controls;
using System.Text.RegularExpressions;

namespace msn2.net.home
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Calander: msn2.net.Controls.WebBrowser
	{
		#region Constructors
		public Calander(msn2.net.Configuration.Data data): base(data, "msn2.net Calander", 400, 300)
		{
			InternalConstructor();
		}		
		private void InternalConstructor()
		{
			string baseUrl = "http://home/_vti_bin/owssvr.dll?Using=Lists%2fEvents%2fShellFormView%2ehtm";

			Crownwood.Magic.Controls.TabPage page = base.AddNewTab("Calander", baseUrl, msn2.net.Controls.WebBrowserControl.DefaultClickBehavior.OpenLink);

			// Hook into navigate event
			WebBrowserControl browser = (WebBrowserControl) page.Control;
			browser.BeforeNavigateEvent += new BeforeNavigateDelegate(OnBeforeNavigate);

			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			System.Drawing.Icon icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("msn2.net.ProjectF.Icons.Calander.ico"));
			this.Icon = new System.Drawing.Icon(icon, 16, 16);

		}
		#endregion

		#region OnBeforeNavigate

		public void OnBeforeNavigate(object sender, BeforeNavigateEventArgs e)
		{
			e.ClickBehavior = msn2.net.Controls.WebBrowserControl.DefaultClickBehavior.OpenInNewWindow;
		}

		#endregion
	}
}
