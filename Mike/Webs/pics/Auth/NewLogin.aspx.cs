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
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Web.Mail;

namespace pics.Auth
{
	/// <summary>
	/// Summary description for NewLogin.
	/// </summary>
	public class NewLogin : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.TextBox txtConfirmPassword;
		protected System.Web.UI.WebControls.DropDownList lstRequest;
		protected System.Web.UI.WebControls.Panel pnlNewLogin;
		protected System.Web.UI.WebControls.RequiredFieldValidator NameValidator;
		protected System.Web.UI.WebControls.RequiredFieldValidator EmailValidator;
		protected System.Web.UI.WebControls.RegularExpressionValidator EmailRegexVal;
		protected System.Web.UI.WebControls.RequiredFieldValidator PasswordValidator;
		protected System.Web.UI.WebControls.CompareValidator CompareValidator1;
		protected System.Web.UI.WebControls.ValidationSummary ValidSummary;
		protected System.Web.UI.WebControls.RequiredFieldValidator ConfirmPasswordValidator;
		protected System.Web.UI.WebControls.Button btnSend;
		protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator2;
		protected System.Web.UI.WebControls.RegularExpressionValidator Regularexpressionvalidator1;
		protected System.Web.UI.WebControls.TextBox txtLookupEmail;
		protected System.Web.UI.WebControls.ValidationSummary Validationsummary1;
		protected System.Web.UI.WebControls.Panel pnlEmailLookup;
		protected System.Web.UI.WebControls.Panel pnlEmailFound;
		protected System.Web.UI.WebControls.Button btnEmailLookup;
		protected System.Web.UI.WebControls.Label lblEmail;
		protected System.Web.UI.WebControls.Panel pnlInfo;
	
		public NewLogin()
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

		private void btnSend_Click(object sender, System.EventArgs e)
		{
			// encrypt the password
			MD5 md5 = MD5.Create();
			byte[] bPassword = md5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(txtPassword.Text));

			// add new login request
			SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
			SqlCommand cmd   = new SqlCommand("sp_LoginRequest_Add", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;

			// add params
			cmd.Parameters.Add("@name", txtName.Text);
			cmd.Parameters.Add("@email", txtLookupEmail.Text);
			cmd.Parameters.Add("@password", System.Text.ASCIIEncoding.ASCII.GetString(bPassword));
			cmd.Parameters.Add("@id", SqlDbType.Int);
			cmd.Parameters["@id"].Direction = ParameterDirection.Output;

			// open connection and execute
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			// figure out who to send to
			String strSendTo;
			strSendTo = lstRequest.SelectedItem.Value + "@msn2.net";

			// create the message body
			System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);
			sb.Append("<html><body><p>Someone used your name to request access to the pic site.  ");
			sb.Append("The submitted details are below, and a link to activate the user is below as well.</p>");
			sb.Append("<p>Name: <b>" + txtName.Text + "</b></p>");
			sb.Append("<p>Email: <b>" + txtLookupEmail.Text + "</b></p>");
			sb.Append("<p>&nbsp;</p>");
			sb.Append("<a href=\"http://" + Request.Url.Host + Request.ApplicationPath + "/Admin/AuthNewLogin.aspx");
			sb.Append("?id=" + cmd.Parameters["@id"].Value.ToString() + "\">Click here to authorize</a>");
			sb.Append("</body></html>");

			// Send an email to correct person
			MailMessage msg = new MailMessage();
			msg.From	= txtName.Text + "<" + txtLookupEmail.Text + ">";
			msg.To		= strSendTo;
			msg.Subject = "New User on pics.msn2.net";
			msg.Body	= sb.ToString();
			msg.BodyFormat = MailFormat.Html;

			// Send the email message
			SmtpMail.SmtpServer = "kyle";
			SmtpMail.Send(msg);

			// show user a brief message
			pnlInfo.Visible		= true;
			pnlNewLogin.Visible = false;
		}

		private void btnEmailLookup_Click(object sender, System.EventArgs e)
		{
			this.Validate();

			if (!Page.IsValid) return;

			// connect to db and see if email is found
			SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
			SqlCommand cmd   = new SqlCommand("sp_ForgotPassword", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;
			cmd.Parameters.Add("@Email", txtLookupEmail.Text);
			cmd.Parameters.Add("@guid", SqlDbType.NVarChar, 50);
			cmd.Parameters["@guid"].Direction = ParameterDirection.Output;

			// run command
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

			// see if output was null, if so we don't know the email
			if (cmd.Parameters["@guid"].Value.ToString().Length > 1) 
			{
				// build message
				String sGUID = cmd.Parameters["@guid"].Value.ToString().Substring(2);
				System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);
				sb.Append("<html><body><p>Welcome to MSN2 pictures.</p>");
				sb.Append("<p>Click <a href=\"http://" + Request.Url.Host);
				sb.Append(Request.ApplicationPath + "/Auth/ResetPassword.aspx?id=" + sGUID);
				sb.Append("&email=" + Server.UrlEncode(txtLookupEmail.Text) + "\">here</a> to set your password and log on.</p>");
				sb.Append("<p>If you did not make this request, you can ignore this email message.</p>");
				sb.Append("</body></html>");

				// Create the message object
				MailMessage msg = new MailMessage();
				msg.To		= txtLookupEmail.Text;
				msg.From	= "MSN2 Pictures <pictures@msn2.net>";
				msg.Subject	= "MSN2 Pictures - Login";
				msg.Body	= sb.ToString();
				msg.BodyFormat = MailFormat.Html;

				// Send message
				SmtpMail.SmtpServer = pics.Config.SMTPServer;
				SmtpMail.Send(msg);

				// show correct panes for email found and message sent
				pnlEmailLookup.Visible = false;
				pnlEmailFound.Visible = true;
			} 
			else 
			{
				lblEmail.Text = txtLookupEmail.Text;

				pnlNewLogin.Visible = true;
				pnlEmailLookup.Visible = false;
			}
			
			cn.Close();
		}

		private void btnEmailFoundContinue_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("ResetPassword.aspx?email=" + txtLookupEmail.Text + ViewState["emailfound"].ToString());
		}

	}
}
