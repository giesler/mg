#region Using...
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
using System.Web.Security;
using System.Security.Cryptography;
#endregion

namespace pics.auth
{
	/// <summary>
	/// Summary description for login.
	/// </summary>
	public class Login : System.Web.UI.Page
	{
		#region Declares
		protected System.Web.UI.WebControls.CheckBox chkSave;
		protected System.Web.UI.WebControls.HyperLink lnkForgotPassword;
		protected System.Web.UI.WebControls.TextBox password;
		protected System.Web.UI.WebControls.RadioButton radioNewLogin;
		protected System.Web.UI.WebControls.TextBox email;
		protected System.Web.UI.WebControls.RadioButton radioHelpMe;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.Panel panelMessage;
		protected System.Web.UI.WebControls.RadioButton radioPassword;
		protected pics.Controls.ErrorMessagePanel pnlBadPassword;
		protected pics.Controls.ErrorMessagePanel pnlBadPassword1;
		protected pics.Controls.ErrorMessagePanel pnlBadEmail;
		protected System.Web.UI.WebControls.HyperLink lnkNewLogin;
		protected pics.Controls.Header header;
		protected pics.Controls.Sidebar Sidebar1;
		protected System.Web.UI.WebControls.Button btnLogin;
		#endregion
		#region Constructor
		public Login()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}
		#endregion
		#region Page Initialization/Loading
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (Request.QueryString["email"] != null)
				{
					email.Text = Request.QueryString["email"];
				}
			}

			// Check if we have username and pw
			if (email.Text.Length > 0 && password.Text.Length > 0)
			{
				btnLogin_Click(this, EventArgs.Empty);
			}
		}

		private void Page_Init(object sender, EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}
		#endregion
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		#region Private Methods
		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			// Check if a new user - if so redirect to NewLogin.aspx
			if (radioNewLogin.Checked)
			{
				Response.Redirect("NewLogin.aspx?ref=2&email=" + Server.UrlEncode(email.Text));
			}

			// Check if unknown
			if (radioHelpMe.Checked)
			{
				Response.Redirect("NewLogin.aspx?ref=3&email=" + Server.UrlEncode(email.Text));
			}

			// encrypt the password
			MD5 md5 = MD5.Create();
			byte[] bPassword = md5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(password.Text));
			
			// attempt to login
			PersonInfo pi = new PersonInfo(email.Text, System.Text.ASCIIEncoding.ASCII.GetString(bPassword));

			// Check if login is valid
			if (pi.Valid) 
			{
				Session["PersonInfo"] = pi;
				System.Security.Cryptography.MD5 crypt = System.Security.Cryptography.MD5.Create();
				byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(password.Text);
				byte[] bOut = crypt.ComputeHash(b);

				FormsAuthentication.RedirectFromLoginPage(pi.PersonID.ToString(), chkSave.Checked);
			}
			else if (pi.ValidEmail)
			{
				pnlBadPassword.Visible		= false;
				pnlBadEmail.Visible			= true;

				lnkNewLogin.NavigateUrl		= "NewLogin.aspx?email=" + Server.UrlEncode(email.Text);
			}
			else
			{
				pnlBadPassword.Visible		= true;
				pnlBadEmail.Visible			= false;

				lnkForgotPassword.NavigateUrl = "ForgotPassword.aspx?email=" + Server.UrlEncode(email.Text);
			}
		}
		#endregion
	}
}
