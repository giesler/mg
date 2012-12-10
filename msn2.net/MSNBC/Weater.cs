using System;

namespace MSNBC
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Weather: msn2.net.Controls.WebBrowser
	{
		#region Constructors
		public Weather(msn2.net.Configuration.Data data): base(data, "MSNBC Weather", 397, 200)
		{
			InternalConstructor();
		}		
		private void InternalConstructor()
		{
			string baseUrl = "http://home.msn2.net/weather.aspx?aid={0}";
			base.ShowClose	= false;
			base.ShowArrows	= false;
			base.AddStaticTab("Kirkland", String.Format(baseUrl, "WAKI", new TimeSpan(0, 30, 1)));
			base.AddStaticTab("Seattle", String.Format(baseUrl, "SEA"), new TimeSpan(2, 0, 0));
			base.AddStaticTab("Madison", String.Format(baseUrl, "MSN"), new TimeSpan(2, 0, 2));
		}
		#endregion
	}
}
