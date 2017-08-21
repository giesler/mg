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
	///		Summary description for AMSFeatured.
	/// </summary>
	public abstract class AMSFeatured : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label lblFeaturedAdTitle;
		protected System.Web.UI.WebControls.Label lblUpdateTime;
		protected System.Web.UI.WebControls.Image imgFeatured;


		/// <summary>
		public AMSFeatured()
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

				// Get the featured ad
				SqlCommand cmdAd = new SqlCommand("sp_Ads_FeaturedSite '" + Request.UserHostAddress + "'", cn);
				
				SqlDataReader drAd = cmdAd.ExecuteReader(CommandBehavior.SingleRow);
				if (drAd.Read()) 
				{
					imgFeatured.ImageUrl    = drAd.GetString(2);
					lblFeaturedAdTitle.Text = drAd.GetString(6);
				}
				else 
				{
					imgFeatured.ImageUrl    = "";
					lblFeaturedAdTitle.Text = "Unable to retreive featured site information.";
				}
				drAd.Close();

				// close connection
				cn.Close();
			}

			catch (Exception excep) 
			{
				// set defaults...
				lblFeaturedAdTitle.Text = "<!--" + excep + "-->";

			}
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
