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
using System.Security.Cryptography;

namespace pics.Auth
{
	/// <summary>
	/// Summary description for ResetPassword.
	/// </summary>
	public class ResetPassword : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox txtNewPassword;
		protected System.Web.UI.WebControls.TextBox txtConfirmNewPassword;
		protected System.Web.UI.WebControls.Button btnOK;
		protected System.Web.UI.WebControls.Panel pnlPassword;
		protected System.Web.UI.WebControls.Label lblEmail;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.HyperLink loginLink;
		protected System.Web.UI.WebControls.Panel pnlChanged;
	
		public ResetPassword()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				// make sure an ID is passed
				if (Request.QueryString["id"] == null || Request.QueryString["email"] == null)
					Response.Redirect("../");

				PersonInfo pi = new PersonInfo(Request.QueryString["email"], "");

				// show the email address
				if (pi != null)
				{
					lblEmail.Text = Request.QueryString["email"];
				}
				else
				{
					Response.Write("Invalid email address passed.");
					Response.End();
					return;
				}
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
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{

			// make sure fields match
			if (!txtNewPassword.Text.Equals(txtConfirmNewPassword.Text)) 
			{
				lblError.Text = "Your passwords must match!<br><br>";
				return;
			}

			// get the byte array from the guid in the QS
			XMGuid.Init();
			XMGuid g = new XMGuid(Request.QueryString["id"]);

			// Encode the password
			MD5 md5 = MD5.Create();
			byte[] bPassword = md5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(txtNewPassword.Text));

			// set up a connection and command to update password
			SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("dbo.sp_ResetPassword", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@email", lblEmail.Text);
			cmd.Parameters.Add("@guid", g.Buffer);
			cmd.Parameters.Add("@password", System.Text.ASCIIEncoding.ASCII.GetString(bPassword));
			cmd.Parameters.Add("@success", SqlDbType.Bit);
			cmd.Parameters["@success"].Direction = ParameterDirection.Output;

			// execute the command
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			// check result to make sure password changed
			if (Convert.ToInt16(cmd.Parameters["@success"].Value) == 1) 
			{
				loginLink.NavigateUrl = "Login.aspx?email=" + Server.UrlEncode(lblEmail.Text);
				pnlPassword.Visible = false;
				pnlChanged.Visible	= true;
			} 
			else 
			{
				lblError.Text = "There was a problem changing your password.  Please try again or contact webmaster@msn2.net with this problem";
			}


		}
	}
}
