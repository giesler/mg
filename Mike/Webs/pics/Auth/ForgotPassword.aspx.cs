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
using System.Web.Mail;
using System.Data.SqlClient;
using System.Configuration;

namespace pics.Auth
{
	/// <summary>
	/// Summary description for ForgotPassword.
	/// </summary>
	public class ForgotPassword : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.Panel pnlConfirm;
		protected System.Web.UI.WebControls.Label lblEmail;
		protected System.Web.UI.WebControls.Panel pnlSent;
		protected System.Web.UI.WebControls.Button btnConfirm;
	
		public ForgotPassword()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// set the email based on querystring
			if (Request.QueryString["email"] == null)
				Response.Redirect("Login.aspx");
			else
				txtEmail.Text = Request.QueryString["email"];
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
			this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnConfirm_Click(object sender, System.EventArgs e)
		{

			// make sure email is valid, get guid
			SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("dbo.sp_ForgotPassword", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;
			cmd.Parameters.Add("@email", SqlDbType.NVarChar, 150);
			cmd.Parameters["@email"].Value = txtEmail.Text;
			cmd.Parameters.Add("@guid", SqlDbType.NVarChar, 50);
			cmd.Parameters["@guid"].Direction = ParameterDirection.Output;
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();
		
			// build message
			String sGUID = cmd.Parameters["@guid"].Value.ToString().Substring(2);
			System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);
			sb.Append("<html><body><p>A request has been made to reset your MSN2 password.</p>");
			sb.Append("<p>If you made this request, click <a href=\"http://" + Request.Url.Host);
			sb.Append(Request.ApplicationPath + "/Auth/ResetPassword.aspx?id=" + sGUID);
			sb.Append("&email=" + Server.UrlEncode(txtEmail.Text) + "\">here</a>.</p>");
			sb.Append("<p>If you did not make this request, you can ignore this email message.</p>");
			sb.Append("</body></html>");

			// Create the message object
			MailMessage msg = new MailMessage();
			msg.To		= txtEmail.Text;
			msg.From	= "MSN2 Pictures <pictures@msn2.net>";
			msg.Subject	= "Reset MSN2 Pictures password";
			msg.Body	= sb.ToString();
			msg.BodyFormat = MailFormat.Html;

			// Send the email message
			SmtpMail.SmtpServer = "kyle";
			SmtpMail.Send(msg);

			// display confirmation
			pnlConfirm.Visible  = false;
			pnlSent.Visible		= true;
			lblEmail.Text		= txtEmail.Text;
		}
	}
}
