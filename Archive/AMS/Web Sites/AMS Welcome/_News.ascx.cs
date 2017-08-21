namespace AMS_Welcome
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Data.SqlClient;
	using System.Configuration;

	/// <summary>
	///		Summary description for AMSNews.
	/// </summary>
	public abstract class AMSNews : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Repeater rpNews;

		public String NewsItems;

		/// <summary>
		public AMSNews()
		{
			this.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{

			try 
			{
				// Open connection
				SqlConnection cn = new SqlConnection(ConfigurationSettings.AppSettings["strConn"]);
				cn.Open();

				// Make sure newsitems will be shown
				if (NewsItems == null || NewsItems.Length == 0)
					NewsItems = "3";

				SqlCommand cmdNews = new SqlCommand("sp_Web_GetNews " + NewsItems, cn);
				SqlDataReader drNews = cmdNews.ExecuteReader();

				rpNews.DataSource = drNews;
				rpNews.DataBind();
				
				drNews.Close();
			
				// close connection
				cn.Close();

			}

			catch (Exception excep) 
			{
				Trace.Write("AMSNews Exception", excep.ToString());
			}

		}

		protected String ShowTitle(Object objTitle) 
		{
			String strTitle = objTitle.ToString();
			if (strTitle.Length == 0) 
			{
				return "";
			}
			return "<b>" + strTitle + "</b><br>";

		}

		protected String ShowTime(Object objTime)
		{
			DateTime dt = Convert.ToDateTime(objTime);
			dt = dt.AddHours(2);	// offset for PST
			return dt.ToLongDateString() + " " + dt.ToShortTimeString() + " CST";
		}

		private void Page_Init(object sender, EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}

		#region Web Form Designer generated code
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
