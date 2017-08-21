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
using System.Web.Mail;
using msn2.net.Pictures;

namespace pics.Admin
{
	/// <summary>
	/// Summary description for AuthNewLogin.
	/// </summary>
	public partial class AuthNewLogin : Page
	{
		#region Declares

	
		#endregion

		#region Constructor

// 		public AuthNewLogin()
// 		{
// 			Page.Init += new System.EventHandler(Page_Init);
// 		}

		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
				// make sure we have an ID
				if (Request.QueryString["id"] == null)
					Response.Redirect("../");

				int id = int.Parse(Request.QueryString["id"]);
				PersonInfo info = PicHttpContext.Current.UserManager.GetNewUserRequest(id);
				if (info != null)
				{
					lblName.Text	= info.Name;
					lblEmail.Text	= info.Email;
				}
				else 
				{
					lblName.Text = "Unable to read user information.";
				}

				// set up link to new user
				lnkNewLogin.NavigateUrl = "AuthNewCreateLogin.aspx?id=" + Request.QueryString["id"].ToString();

				// catch the person picker picking a person
				PersonPicker.PersonSelected += new System.EventHandler(this.FindPersonSelected);

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

		private void FindPersonSelected(object sender, EventArgs e) 
		{
			lblEmail.Text = "a person was selected.";

		}

        protected void btnContinue_Click(object sender, System.EventArgs e)
		{
			// make sure someone was selected
			if (PersonPicker.SelectedPerson == -1)
			{
				lblError.Text = "You must select a person first.";
				return;
			}

			int requestId = int.Parse(Request.QueryString["id"]);
			PicHttpContext.Current.UserManager.AssociateRequestWithPerson(requestId, PersonPicker.SelectedPerson);

			// create message body
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("<html><body><p>Your MSN2 pictures login has been activated.  Click ");
			sb.Append("<a href=\"http://" + Request.Url.Host + Request.ApplicationPath + "\">here</a>");
			sb.Append(" to go to the picture site.</p>");
			sb.Append("<p>Your email address (used to sign in): <b>" + lblEmail.Text + "</b>");
			sb.Append("</body></html>");

			// create mail message
			MailMessage msg = new MailMessage();
			msg.From	= "MSN2 Pictures <pictures@msn2.net";
			msg.To		= lblEmail.Text;
			msg.Subject = "MSN2 Pictures login activated.";
			msg.Body	= sb.ToString();
			msg.BodyFormat = MailFormat.Html;

			// send message
			SmtpMail.SmtpServer = PicHttpContext.Current.Config.SmtpServer;
			SmtpMail.Send(msg);

			// show message
			pnlPerson.Visible = false;
			pnlDone.Visible = true;
		}

		protected void PersonPicker_PersonSelected(object sender, System.EventArgs e)
		{
			afterPersonSelectContent.Visible	= true;
			
		}

	
	}
}
