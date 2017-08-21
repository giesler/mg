using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;

namespace vbsw 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{

		public Global()
		{
			InitializeComponent();
		}	

		protected void Application_Start(Object sender, EventArgs e)
		{

		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			bool setCookie = false;

			if (Request.QueryString["email"] != null) 
			{
				if (Request.Cookies["email"] == null)
				{
					setCookie = true;
				}
				if (Request.Cookies["email"] != null && Request.Cookies["email"].Value != Request.QueryString["email"])
				{
					setCookie = true;
				}

				if (setCookie)
				{
					Response.Cookies["email"].Value = Request.QueryString["email"];
					Response.Cookies["email"].Expires = DateTime.Now.Add(new TimeSpan(500, 0, 0, 0, 0));
				}
			}
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{

		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}

