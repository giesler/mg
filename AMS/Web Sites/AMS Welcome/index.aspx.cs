using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;

namespace AMS_Welcome
{
	/// <summary>
	/// Summary description for index.
	/// </summary>
	public class index : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblSharing;
		protected System.Web.UI.WebControls.Label lblUserName;
		public int featureAdID;
		public String featureAdImageURL;
		protected System.Web.UI.WebControls.Label lblScore;
		protected System.Web.UI.WebControls.Label lblTopIndexScore;
		protected System.Web.UI.WebControls.Label lblTotalUniqueFiles;
		protected System.Web.UI.WebControls.HyperLink lnkRefresh;
		public String featureAdTitle;

		public index()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here


		}

		private void Page_Init(object sender, EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();

			try 
			{
				// Open connection
				SqlConnection cn = new SqlConnection(ConfigurationSettings.AppSettings["strConn"]);
				cn.Open();

				// Current users's stats
				SqlCommand cmdUserData = new SqlCommand("sp_Welcome_UserInfo 0x" + Request.QueryString["at"] + "", cn);
				SqlDataReader drUserData = cmdUserData.ExecuteReader(CommandBehavior.SingleRow);
				if (drUserData.Read() && !drUserData.IsDBNull(0)) 
				{
					lblUserName.Text = drUserData.GetString(0);

					// get the number of shared files
					if (drUserData.IsDBNull(1) || drUserData.GetInt32(1) == 0)
						lblSharing.Text = "You are not currently sharing any files.  For help on sharing files, visit the 'Tutorial' below.";
					else if (drUserData.GetInt32(3) == 0)
						lblSharing.Text = "You have files to share, however, a firewall or network device is preventing other users from connecting to you.";
					else	// they have shared files and they are online
						lblSharing.Text = "You are currently sharing <b>" + drUserData.GetInt32(1).ToString("###,##0") + "</b> files.";

					// get the user score
					if (drUserData.IsDBNull(1) )
						lblScore.Text = "<i>Temporarily unavailable</i>";
					else	// there is a score to report
						lblScore.Text = "<b>" + drUserData.GetInt32(2).ToString("###,##0") + "</b>";

					// get the top score
					if (drUserData.IsDBNull(4) )
						lblTopIndexScore.Text = "<i>Temporarily unavailable</i>";
					else	// there is a score to report
						lblTopIndexScore.Text = "<b>" + drUserData.GetInt32(4).ToString("###,##0") + "</b>";
					

					// get the top score
					if (drUserData.IsDBNull(5) )
						lblTotalUniqueFiles.Text = "<i>Temporarily unavailable</i>";
					else	// there is a score to report
						lblTotalUniqueFiles.Text = "<b>" + drUserData.GetInt32(5).ToString("###,##0") + "</b>";

				}
				else 
				{
					lblSharing.Text = "Your login session was not recognized.  Please close and reopen AMS to view your statistics.";	
					lblScore.Text   = "<i>Unavailable</i>";
				}
				drUserData.Close();
			
				// Get the featured ad
				SqlCommand cmdAd = new SqlCommand("sp_Ads_FeaturedSite '" + Request.UserHostAddress + "'", cn);
				
				SqlDataReader drAd = cmdAd.ExecuteReader(CommandBehavior.SingleRow);
				if (drAd.Read()) 
				{
					featureAdImageURL = drAd.GetString(2);
					featureAdTitle    = drAd.GetString(6);
				}
				else 
				{
					featureAdImageURL = "";
					featureAdTitle    = "Unable to retreive featured site information.";
				}
				drAd.Close();

				// close connection
				cn.Close();

				// update link
				lnkRefresh.NavigateUrl = Request.RawUrl;
			}

			catch (Exception excep) 
			{
				// set defaults...
				System.Net.NetworkCredential nc = new System.Net.NetworkCredential();
				lblSharing.Text = "We were unable to retreive your login information.  Please try again later. <!--" + excep + "-->";

			}
		}

		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
