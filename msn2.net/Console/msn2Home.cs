using System;

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
			string baseUrl = "http://home";

			base.AddNewTab("Home", baseUrl, msn2.net.Controls.WebBrowserControl.DefaultClickBehavior.OpenLink);
//			base.AddStaticTab("Home", baseUrl);
		}
		#endregion
	}
}

