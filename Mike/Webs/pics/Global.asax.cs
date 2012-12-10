using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Configuration;

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

		public static string PictureCacheLocation;
		public static string PictureLocation;
		
		protected void Application_Start(Object sender, EventArgs e)
		{
            PictureCacheLocation		= ConfigurationSettings.AppSettings["PictureCacheLocation"];
			PictureLocation				= ConfigurationSettings.AppSettings["PictureLocation"];
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
			// check if the user is authenticated
			if (Request.IsAuthenticated) 
			{
				// load the user info into the session
				PersonInfo pi = new PersonInfo(Convert.ToInt32(User.Identity.Name));
				Session["PersonInfo"] = pi;

			}

			Session["MySelectedList"]		= new pics.Controls.PictureIdCollection();

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

