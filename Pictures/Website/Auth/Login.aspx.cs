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
using msn2.net.Pictures;
#endregion

namespace pics.auth
{
	/// <summary>
	/// Summary description for login.
	/// </summary>
    public partial class Login : Page
	{
		#region Declares
		protected pics.Controls.ErrorMessagePanel pnlBadPassword1;
		#endregion
		#region Constructor
// 		public Login()
// 		{
// 			Page.Init += new System.EventHandler(Page_Init);
// 		}
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
            //if (email.Text.Length > 0 && password.Text.Length > 0)
            //{
            //    btnLogin_Click(this, EventArgs.Empty);
            //}
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

		}
		#endregion
		#region Private Methods
        protected void btnLogin_Click(object sender, System.EventArgs e)
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
            Page.GetPostBackEventReference(this);

            string pwd = UserManager.GetEncryptedPassword(password.Text);

            bool valid = false;
            PersonInfo info = PicContext.Current.UserManager.Login(email.Text, pwd, ref valid);

            // Check if login is valid
			if (info != null) 
			{
//				Session["PersonInfo"] = pi;
//				System.Security.Cryptography.MD5 crypt = System.Security.Cryptography.MD5.Create();
//				byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(password.Text);
//				byte[] bOut = crypt.ComputeHash(b);

                if (Request.QueryString["ReturnURL"] != null)
                {
                    string url = Request.QueryString["ReturnURL"];
                    if (url.IndexOf("Logout.aspx") > 0)
                    {
                        FormsAuthentication.SetAuthCookie(info.Id.ToString(), chkSave.Checked);
                        Response.Redirect("../default.aspx");
                        return;
                    }
                }

				FormsAuthentication.RedirectFromLoginPage(info.Id.ToString(), chkSave.Checked);
			}
			else if (valid)
			{
				pnlBadPassword.Visible		= true;
				pnlBadEmail.Visible			= false;

                lnkForgotPassword.NavigateUrl = "ForgotPassword.aspx?email=" + Server.UrlEncode(email.Text);
			}
			else
			{
				pnlBadPassword.Visible		= false;
				pnlBadEmail.Visible			= true;

                lnkNewLogin.NavigateUrl = "NewLogin.aspx?email=" + Server.UrlEncode(email.Text);
            }
		}

		#endregion
	}

}
