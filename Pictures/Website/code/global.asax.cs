using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using msn2.net.Pictures;

namespace pics 
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

		public static bool AdminMode
		{
			get
			{
				HttpSessionState session = HttpContext.Current.Session;
				if (session["editMode"] == null)
				{
					return false;
				}
				
				return (bool) session["editMode"];
			}
			set
			{
				HttpSessionState session = HttpContext.Current.Session;
				if (session["editMode"] == null)
				{
					session["editMode"] = value;
				}
				else
				{
					session.Add("editMode", value);
				}
			}
		}

		protected void Application_Start(Object sender, EventArgs e)
		{
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
			Session["MySelectedList"]		= new PictureIdCollection();
			Session["editMode"]				= false;

		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{

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

