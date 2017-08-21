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

namespace AMS_Ads.featured
{
	/// <summary>
	/// Summary description for preview.
	/// </summary>
	public class index : System.Web.UI.Page
	{
		public index()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		const String DEFAULT_BODY_TAG = "scroll=\"no\" bgcolor=\"black\" text=\"#FFFFFF\" link=\"#FFFF00\" vlink=\"#FFFF99\" topmargin=\"0\" leftmargin=\"0\" marginheight=\"0\" marginwidth=\"0\"";
		String strBodyTag, strBodyContents;
		int AdID;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			// init connection and command objects
		}


		public String BodyTag
		{
			get
			{
				return strBodyTag;
			}
		}
	
		public String BodyContents
		{
			get
			{
				return strBodyContents;
			}
		}
		public String AdIDTag
		{
			get
			{
				return "<!--" + AdID + "-->";
			}
		}
		
		private void Page_Init(object sender, EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();

			// make sure we have a valid company id, if not bail out
			if (Request["cid"] == null)
				Response.Redirect("../request/");

			// init connection, sp
			SqlConnection cn = new SqlConnection(ConfigurationSettings.AppSettings["strConn"]);
			SqlCommand cmd   = new SqlCommand("sp_Ads_Featured", cn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 16));
			cmd.Parameters.Add(new SqlParameter("@cid", SqlDbType.Int));
			cmd.Parameters["@ip"].Value = Request.UserHostAddress;
			cmd.Parameters["@cid"].Value = Request["cid"];

			int AdType; bool AdBodyTagDefault;
			String AdImageURL, AdBodyTag, AdBodyContents;
			AdID             = 0;
			AdType           = 1;
			AdImageURL       = "";
			AdBodyTagDefault = true;
			AdBodyTag        = DEFAULT_BODY_TAG;
			AdBodyContents   = "error";

			// open connection and read vals
			try 
			{
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read()) 
				{
					// read row
					AdID             = dr.GetInt32(0);
					AdType           = dr.GetByte(1);
					if (!dr.IsDBNull(2))
						AdImageURL = dr.GetString(2);
					if (!dr.IsDBNull(3))
						AdBodyTagDefault = dr.GetBoolean(3);
					if (!dr.IsDBNull(4))
						AdBodyTag = dr.GetString(4);
					if (!dr.IsDBNull(5))
						AdBodyContents = dr.GetString(5);
				} 
				dr.Close();
				cn.Close();
			}
			catch (Exception excep) 
			{
				// we'll ignore
				Trace.Write(excep.ToString());
			}

			// now set the body tag and contents according to the data read
			if (AdType == 0) 
			{
				// image ad
				strBodyContents = "<a href=\"" + ConfigurationSettings.AppSettings["strClickURL"] + AdID + "\" target=\"_new\">" 
					+ "<img src=\"" + AdImageURL + "\" border=\"0\"></a>";
			}
			else if (AdType == 1)
			{
				// custom ad
				strBodyContents = AdBodyContents;
				strBodyContents = strBodyContents.Replace("%clickurl%", ConfigurationSettings.AppSettings["strClickURL"] + AdID);
			}

			// set the body tag accordingly
			if (AdBodyTagDefault)
				strBodyTag = DEFAULT_BODY_TAG;
			else
				strBodyTag = AdBodyTag;		
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
