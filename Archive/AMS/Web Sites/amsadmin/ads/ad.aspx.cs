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
using System.Configuration;
using System.Data.SqlClient;

namespace amsadmin.ads
{
	/// <summary>
	/// Summary description for ad.
	/// </summary>
	public class ad : System.Web.UI.Page
	{
		private int mintAdID;
		private int mintCompanyID;

		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.CheckBox chkActive;
		protected System.Web.UI.WebControls.TextBox txtComments;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList DropDownList1;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox TextBox2;
		protected System.Web.UI.WebControls.TextBox TextBox3;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox TextBox4;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.DropDownList DropDownList2;
		protected System.Web.UI.WebControls.HyperLink lnkCompany;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.TextBox TextBox5;
		protected System.Web.UI.WebControls.TextBox TextBox6;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.Label Label1;
	
		public ad()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			if (!Page.IsPostBack) 
			{
			
				mintAdID      = Int32.Parse(Request.QueryString["adid"]);

				SqlConnection cn = new SqlConnection( System.Configuration.ConfigurationSettings.AppSettings["strConn"] );
				SqlCommand cmd   = new SqlCommand("select a.AdTitle, c.CompanyID, c.CompanyName from ad a inner join adcompany c on c.CompanyID = a.CompanyID where AdID = " + Request.QueryString["AdID"], cn);

				cn.Open();
			
				SqlDataReader dr = cmd.ExecuteReader();
				
				if (dr.Read()) 
				{
					// basic company information
					lnkCompany.Text = dr.GetString(2);
					mintCompanyID = dr.GetInt32(1);
					txtName.Text = dr.GetString(0);
				} 
				else 
				{
					txtName.Text = "error";
				}

				// fill in the company name too
				lnkCompany.Text = "company name here";
				lnkCompany.NavigateUrl = "adlist.aspx?companyid=" + mintCompanyID.ToString();

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

		}
		#endregion
	}
}
