using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;
using msn2.net.Pictures;

namespace pics.Auth
{
	public enum PasswordRequestType
	{
		Unknown,
		NoPassword,
		ForgotPassword
	}
	/// <summary>
	/// Summary description for NewLogin.
	/// </summary>
    public partial class NewLogin : Page
	{
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.RequiredFieldValidator EmailValidator;
		protected System.Web.UI.WebControls.RegularExpressionValidator EmailRegexVal;
	
// 		public NewLogin()
// 		{
// 			Page.Init += new System.EventHandler(Page_Init);
// 		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (Request.QueryString["email"] != null)
				{
					Trace.Write("found email in qs");
					txtLookupEmail.Text = Server.HtmlEncode(Request.QueryString["email"]);
					btnEmailLookup_Click(this, EventArgs.Empty);
				}
			}

		}


		public PasswordRequestType PageRequestType
		{
			get
			{
				if (Request.QueryString["ref"] != null)
				{
					if (Request.QueryString["ref"] == "2")
					{
						return PasswordRequestType.NoPassword;
					}
					else if (Request.QueryString["ref"] == "3")
					{
						return PasswordRequestType.ForgotPassword;
					}			
				}

				return PasswordRequestType.Unknown;
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

        protected void btnSend_Click(object sender, System.EventArgs e)
		{
			// encrypt the password
			MD5 md5 = MD5.Create();
			byte[] bPassword = md5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(txtPassword.Text));

			// Add a new user request
			int id = PicContext.Current.UserManager.AddNewUserRequest(txtName.Text, txtLookupEmail.Text, 
				System.Text.ASCIIEncoding.ASCII.GetString(bPassword));
            
			// figure out who to send to
			String strSendTo;
			strSendTo = lstRequest.SelectedItem.Value + "@msn2.net";

			// create the message body
			System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);

			sb.Append("<p>Someone used your name to request a new login to msn2.net.  Wow, cool, huh!?!?  ");
			sb.Append("</p>");
			sb.Append("<p>Name: <b>" + txtName.Text + "</b></<br />>");
			sb.Append("Email: <a href=\"mailto:" + txtLookupEmail.Text + "\"><b>" + Server.HtmlEncode(txtLookupEmail.Text) + "</a></b></p>");
			sb.Append("<a href=\"http://" + Request.Url.Host + Request.ApplicationPath + "/Admin/AuthNewLogin.aspx");
			sb.Append("?id=" + id.ToString() + "\">Click here to authorize</a>");
			sb.Append("<p>If you don't want to authorize them, just ignore this message.  They know who it was sent to, though.</p>");

			// Send an email to correct person
			MailMessage msg = new MailMessage();
			msg.From	= new MailAddress("MSN2 Login System <login@msn2.net>");
			msg.To.Add(new MailAddress(strSendTo));
			msg.Headers.Add("Reply-To", txtName.Text + "<" + txtLookupEmail.Text + ">");
			msg.Subject = "New User on pics.msn2.net";
			msg.Body	= Msn2Mail.BuildMessage(sb.ToString());
            msg.IsBodyHtml = true;

			// Send the email message
			SmtpClient client = new SmtpClient(PicContext.Current.Config.SmtpServer);
            client.Send(msg);

			// show user a brief message
			pnlInfo.Visible		= true;
			pnlNewLogin.Visible = false;
		}

        protected void btnEmailLookup_Click(object sender, System.EventArgs e)
		{
			Trace.Write("email lookup button start.");

			this.Validate();

			if (!Page.IsValid) return;

			Trace.Write("page is valid.");


            Guid resetKey = PicContext.Current.UserManager.GetPasswordResetKey(txtLookupEmail.Text);

			// see if output was null, if so we don't know the email
			if (resetKey != Guid.Empty) 
			{
				// build message
				System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);

				Trace.Write("EmailLookup", "PasswordRequestType = " + PageRequestType.ToString());

				if (PageRequestType == PasswordRequestType.Unknown || PageRequestType == PasswordRequestType.NoPassword)
				{
					sb.Append("<p>Welcome to MSN2 pictures.</p>");
					sb.Append("<p>Click <a href=\"http://" + Request.Url.Host);
					sb.Append(Request.ApplicationPath + "/Auth/ResetPassword.aspx?id=" + resetKey.ToString());
					sb.Append("&email=" + Server.UrlEncode(txtLookupEmail.Text) + "\">here</a> to set your password and log on.</p>");
					sb.Append("<p>If you did not make this request, you can ignore this email message.</p>");
				}
				else
				{
					sb.Append("<p>This email allows you to reset your MSN2 password.</p>");
					sb.Append("<p>Click <a href=\"http://" + Request.Url.Host);
					sb.Append(Request.ApplicationPath + "/Auth/ResetPassword.aspx?id=" + resetKey.ToString());
					sb.Append("&email=" + Server.UrlEncode(txtLookupEmail.Text) + "\">here</a> to set your password and log on.</p>");
					sb.Append("<p>If you did not make this request, you can ignore this email message.</p>");
				}

				// Create the message object
				MailMessage msg = new MailMessage();
				msg.To.Add(new MailAddress(txtLookupEmail.Text));
				msg.From	= new MailAddress( "MSN2 Pictures <pictures@msn2.net>");
				msg.Subject	= "MSN2 Pictures - Login";
				msg.Body	= Msn2Mail.BuildMessage(sb.ToString());
                msg.IsBodyHtml = true;

				// Send message
                SmtpClient client = new SmtpClient(PicContext.Current.Config.SmtpServer);
				try
				{
					client.Send(msg);
				}
				catch (Exception ex)
				{
					Label errLabel = new Label();
					errLabel.Text = "There was an error sending your email.  Please try again later.  If the problem continues please contact Mike.";
					errLabel.Text += errLabel.Text + "<<br /> /><<br /> />Technical error details:<<br /> />";
					errLabel.Text += ex.Message + "<<br /> />";
					Exception ex1 = ex.InnerException;
					if (ex1 != null)
					{
						errLabel.Text += "(InnerException: " + ex1.Message + ")<<br /> />";

						Exception ex2 = ex1.InnerException;
						if (ex2 != null)
						{
							errLabel.Text += "(InnerException2: " + ex2.Message + ")<<br /> />";
						}
					}
					errLabel.Text += "<<br /> />Server: " + client.Host;
					pnlEmailFound.Controls.Add(errLabel);
				}


				// show correct panes for email found and message sent
				pnlEmailLookup.Visible = false;
				pnlEmailFound.Visible = true;

				// Set email text
				foundEmail.Text		= txtLookupEmail.Text;
			} 
			else 
			{
				lblEmail.Text = txtLookupEmail.Text;

				pnlNewLogin.Visible = true;
				pnlEmailLookup.Visible = false;
			}
			
		}

        protected void btnEmailFoundContinue_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("ResetPassword.aspx?email=" + txtLookupEmail.Text + ViewState["emailfound"].ToString());
		}

	}
}
