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

namespace vbsw
{
	/// <summary>
	/// Summary description for maillist.
	/// </summary>
	public class maillist : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox email;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.RadioButtonList action;
		protected System.Web.UI.WebControls.Label lblNote;
		protected System.Web.UI.WebControls.Panel pnlMain;
		protected System.Web.UI.WebControls.Label actionResult;
		protected System.Web.UI.WebControls.Panel pnlDone;
		protected System.Web.UI.WebControls.Button OK;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
                // Initial state, we need to see if cookie is set
				if (Request.Cookies["email"] != null) 
				{
					email.Text = Request.Cookies["email"].Value;

                    SqlConnection cn = new SqlConnection(Config.ConnectionString);
					SqlCommand cmd = new SqlCommand("select * from VBSWMaillist where email = @Email", cn);
					cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 40);
					cmd.Parameters["@Email"].Value = Request.Cookies["email"].Value;

					cn.Open();
					SqlDataReader dr = cmd.ExecuteReader();
					if (dr.Read()) 
					{
						if (Convert.ToBoolean(dr["Subscribed"])) 
						{
							lblNote.Text = email.Text + " is currently subscribed.";
							action.Items[1].Selected = true;
						} 
						else 
						{
							lblNote.Text = email.Text + " is not currently subscribed.";
							action.Items[0].Selected = true;
						}

					} 
					else 
					{
						lblNote.Text = email.Text + " is not currently subscribed.";
						action.Items[0].Selected = true;
					}
					dr.Close();
					cn.Close();
				}
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.OK.Click += new System.EventHandler(this.OK_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void OK_Click(object sender, System.EventArgs e)
		{
			// Save the email address
			Response.Cookies["email"].Value = email.Text;
			Response.Cookies["email"].Expires = DateTime.Now.Add(new TimeSpan(500, 0, 0, 0, 0));

			// Save to database
			SqlConnection cn = new SqlConnection(Config.ConnectionString);
			SqlCommand cmd = new SqlCommand("dbo.sp_VBSWMailListAction", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@Subscribe", SqlDbType.Bit);
			cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@IPAddress", SqlDbType.NVarChar, 50);

			cmd.Parameters["@Subscribe"].Value = action.Items[0].Selected;
			cmd.Parameters["@Email"].Value = email.Text;
			cmd.Parameters["@IPAddress"].Value = Request.UserHostAddress.ToString();

			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			pnlMain.Visible = false;
			pnlDone.Visible = true;

			if (action.Items[0].Selected) 
			{
				actionResult.Text = email.Text + " has been subscribed to the mailling list.";
			} 
			else 
			{
				actionResult.Text = email.Text + " has been unsubscribed from the mailling list.";
			}
		}
	}
}
