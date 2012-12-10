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
	/// Summary description for download.
	/// </summary>
	public class downloadbutton : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox email;
		protected System.Web.UI.WebControls.RadioButtonList version;
		protected System.Web.UI.WebControls.Button downloadNow;
		protected System.Web.UI.WebControls.CheckBox subscribe;
		protected System.Web.UI.WebControls.Panel downloadPanel;
		protected System.Web.UI.WebControls.Panel donePanel;

		private string downloadFile = "";

		public string DownloadFile 
		{
			get 
			{
				return downloadFile;
			}
		}
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
				if (Request.Cookies["email"] != null) 
				{
					email.Text = Request.Cookies["email"].Value;
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
			this.downloadNow.Click += new System.EventHandler(this.downloadNow_Click);
			this.ID = "downloadbutton";
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void downloadNow_Click(object sender, System.EventArgs e)
		{
			// Save to database
			SqlConnection cn = new SqlConnection(Config.ConnectionString);
			SqlCommand cmd = new SqlCommand("dbo.sp_VBSWAddDownload", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@IPAddress", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@Browser", SqlDbType.NVarChar, 75);
			cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@Subscribe", SqlDbType.Bit);
			cmd.Parameters.Add("@Version", SqlDbType.NVarChar, 50);
			cmd.Parameters.Add("@ServerIP", SqlDbType.NVarChar, 50);

			cmd.Parameters["@IPAddress"].Value	= Request.UserHostAddress.ToString();
			cmd.Parameters["@Browser"].Value	= Request.UserAgent.ToString();
			cmd.Parameters["@Email"].Value		= email.Text;
			cmd.Parameters["@Subscribe"].Value  = subscribe.Checked;
			cmd.Parameters["@Version"].Value	= version.SelectedItem.Value;
			cmd.Parameters["@ServerIP"].Value	= Request.Url.Host.ToString();

			cn.Open();
			cmd.ExecuteNonQuery();

			// If user subscribed, save
			if (subscribe.Checked) 
			{
				cmd = new SqlCommand("dbo.sp_VBSWMailListAction", cn);
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.Parameters.Add("@Subscribe", SqlDbType.Bit);
				cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150);
				cmd.Parameters.Add("@IPAddress", SqlDbType.NVarChar, 50);

				cmd.Parameters["@Subscribe"].Value  = true;
				cmd.Parameters["@Email"].Value		= email.Text;
				cmd.Parameters["@IPAddress"].Value	= Request.UserHostAddress.ToString();

				cmd.ExecuteNonQuery();
			}
		
			cn.Close();

			// finally, make sure email is saved in cookie
			if (email.Text.Length > 0)
			{
				if (Request.Cookies["email"] != null && Request.Cookies["email"].Value != email.Text) 
				{
					Response.Cookies["email"].Value = email.Text;
					Response.Cookies["email"].Expires = DateTime.Now.Add(new TimeSpan(500, 0, 0, 0, 0));
				}
			}

			string file = "";

			// select the file to redirect to
			switch (version.SelectedItem.Value) 
			{
				case "300":
					file = "ia_3-00.zip";
					break;
				case "200":
					file = "vbsw_2-00.zip";
					break;
				case "101":
					file = "vbsw_1-01.zip";
					break;
				case "100":
					file = "vbsw_1-00.zip";
					break;
			}

			downloadFile = "download/" + file;
			downloadPanel.Visible = false;
			donePanel.Visible = true;
		}
	}
}
