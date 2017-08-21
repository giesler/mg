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
	///		Summary description for AMSStats.
	/// </summary>
	public abstract class AMSStats : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataList dlScores;
		protected System.Web.UI.WebControls.Label lblTotalUniqueFiles;
		protected System.Web.UI.WebControls.Repeater rpScores;
		protected System.Web.UI.WebControls.Label lblUpdateTime;

		public String PositionsToShow;

		/// <summary>
		public AMSStats()
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

				// Make sure some positions will be shown
				if (PositionsToShow == null || PositionsToShow.Length == 0)
					PositionsToShow = "3";

				// Current users's stats
				SqlCommand cmdAMSStats = new SqlCommand("sp_Welcome_AMSStats " + PositionsToShow, cn);
				SqlDataReader drAMSStats = cmdAMSStats.ExecuteReader();

				rpScores.DataSource = drAMSStats;
				rpScores.DataBind();
				
				// move to unique files rs
				if (drAMSStats.NextResult()) 
				{
					// first set the unique files count
					if (drAMSStats.Read())
						if (!drAMSStats.IsDBNull(0))
							lblTotalUniqueFiles.Text = "<b>" + 
								drAMSStats.GetInt32(0).ToString("###,##0") + "</b>";
				} else 
					lblTotalUniqueFiles.Text = "no more data";

				drAMSStats.Close();
			
				// close connection
				cn.Close();

			}

			catch (Exception excep) 
			{
				Trace.Write("AMSStats Exception", excep.ToString());
				lblTotalUniqueFiles.Text = "<!--" + excep + "-->";
			}

			// update the time on the control
			System.DateTime dt = System.DateTime.Now;
			dt = dt.AddHours(2);
			lblUpdateTime.Text = dt.ToShortTimeString();
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
