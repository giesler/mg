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
using msn2.net.Pictures;

namespace pics.Auth
{
	/// <summary>
	/// Summary description for ForgotPassword.
	/// </summary>
	public partial class ForgotPassword 
	{
		#region Declares

	
		#endregion

// 		public ForgotPassword()
// 		{
// 			Page.Init += new System.EventHandler(Page_Init);
// 		}

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

		}
		#endregion

        protected void btnConfirm_Click(object sender, System.EventArgs e)
		{

			// make sure email is valid, get guid
			Guid userId		= PicContext.Current.UserManager.GetPasswordResetKey(txtEmail.Text);
		
			// build message
			System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);
			sb.Append("<html><body><p>A request has been made to reset your MSN2 password.</p>");
			sb.Append("<p>If you made this request, click <a href=\"http://" + Request.Url.Host);
			sb.Append(Request.ApplicationPath + "/Auth/ResetPassword.aspx?id=" + userId.ToString());
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
			SmtpMail.SmtpServer = PicContext.Current.Config.SmtpServer;
			SmtpMail.Send(msg);

			// display confirmation
			pnlConfirm.Visible  = false;
			pnlSent.Visible		= true;
			lblEmail.Text		= txtEmail.Text;
		}
	}
}
