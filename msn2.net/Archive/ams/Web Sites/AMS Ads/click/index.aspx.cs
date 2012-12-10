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

namespace AMS_Ads.click
{
	/// <summary>
	/// Summary description for index.
	/// </summary>
	public class index : System.Web.UI.Page
	{
		protected String strTargetURL = "default";

		public String TargetURL
		{
			get
			{
				return strTargetURL;
			}
		}

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

			SqlConnection cn = new SqlConnection(ConfigurationSettings.AppSettings["strConn"]);
			SqlCommand cmd   = new SqlCommand("sp_Ads_Click", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@AdID", SqlDbType.NVarChar, 10));
			cmd.Parameters["@AdID"].Value = Request.QueryString["id"].ToString();
			
			cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 16));
			cmd.Parameters["@ip"].Value = Request.UserHostAddress;

			strTargetURL = "http://www.adultmediaswapper.com/";

			// open connection and read vals
			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
			if (dr.Read()) 
			{
				// read row
				if (!dr.IsDBNull(0))
                    strTargetURL = dr.GetString(0);

			} 
			dr.Close();
			cn.Close();

			// redirect to target URL if we can
			if (strTargetURL.Length > 0)
				Response.Redirect(strTargetURL);
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
