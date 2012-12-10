using System;

namespace MSNBC
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Headlines: msn2.net.Controls.WebBrowser
	{
		#region Constructors
		public Headlines(msn2.net.Configuration.Data data): base(data, "MSNBC Headlines", 400, 225)
		{
			InternalConstructor();
		}		
		private void InternalConstructor()
		{
			string baseUrl = "http://www.msnbc.com/modules/story_stage/stages.asp?0st=S{0}&nstage={0}&scss=e";

			base.ShowClose	= false;
			base.ShowArrows	= false;
			base.AddStaticTab("Cover", String.Format(baseUrl, 1), new TimeSpan(0, 25, 0));
			base.AddStaticTab("News", String.Format(baseUrl, 2), new TimeSpan(1, 0, 2));
			base.AddStaticTab("Business", String.Format(baseUrl, 3, new TimeSpan(1, 0, 4)));
			base.AddStaticTab("Health", String.Format(baseUrl, 4, new TimeSpan(1, 0, 6)));
			base.AddStaticTab("Technology", String.Format(baseUrl, 5, new TimeSpan(1, 0, 8)));
			base.AddStaticTab("TV News", String.Format(baseUrl, 6, new TimeSpan(1, 0, 10)));
			base.AddStaticTab("Opinions", String.Format(baseUrl, 7, new TimeSpan(3, 0, 0)));
		}
		#endregion
	}
}
