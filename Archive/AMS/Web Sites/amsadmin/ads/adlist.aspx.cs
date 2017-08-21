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

namespace amsadmin.ads
{
	/// <summary>
	/// Summary description for adlist.
	/// </summary>
	public class adlist : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblTitle;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.TextBox txtURL;
		protected System.Web.UI.WebControls.DataGrid dgAds;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.CheckBox chkActive;
	
		protected int mintCompanyID;

		public adlist()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			if (!Page.IsPostBack) 
			{
				mintCompanyID = Int32.Parse(Request.QueryString["companyid"]);

				SqlConnection cn = new SqlConnection( System.Configuration.ConfigurationSettings.AppSettings["strConn"] );
				SqlCommand cmd   = new SqlCommand("select * from Ad where companyid = " + mintCompanyID.ToString(), cn);
			
				cn.Open();

				SqlDataReader dr = cmd.ExecuteReader();

				dgAds.DataSource = dr;
				dgAds.DataBind();

				dr.Close();

				// load company details
				SqlCommand cmd2 = new SqlCommand( "select CompanyName, CompanyURL from AdCompany where CompanyID = " + mintCompanyID.ToString(), cn);
				SqlDataReader dr2 = cmd2.ExecuteReader();
				if (dr2.Read()) 
				{
					txtName.Text = dr2.GetString(0);
					txtURL.Text  = dr2.GetString(1);
				}
				else 
				{
					txtName.Text = "No data.";
				}

				cn.Close();
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			String sql;
			sql = "update AdCompany set CompanyName='" + txtName.Text + "'";
			sql += ", CompanyURL = '" + txtURL.Text + "' where CompanyID = " + mintCompanyID.ToString();

			Trace.Write("info", sql);

			SqlConnection cn = new SqlConnection( System.Configuration.ConfigurationSettings.AppSettings["strConn"] );
			SqlCommand cmd   = new SqlCommand(sql, cn);
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		}

	}
}
