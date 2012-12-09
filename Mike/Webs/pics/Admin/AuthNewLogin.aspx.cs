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

namespace pics.Admin
{
	/// <summary>
	/// Summary description for AuthNewLogin.
	/// </summary>
	public class AuthNewLogin : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblName;
		protected System.Web.UI.WebControls.Panel pnlNewLoginInfo;
		protected System.Web.UI.WebControls.HyperLink lnkNewLogin;
		protected pics.Controls.PersonPicker PersonPicker;
		protected System.Web.UI.WebControls.Panel pnlPerson;
		protected System.Web.UI.WebControls.Button btnContinue;
		protected System.Web.UI.WebControls.Panel pnlDone;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label lblEmail;
	
		public AuthNewLogin()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{

				// make sure we have an ID
				if (Request.QueryString["id"] == null)
					Response.Redirect("../");

				// set up objects to get info
				SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
				SqlCommand cmd   = new SqlCommand("sp_LoginRequest_Retreive", cn);
				cmd.CommandType  = CommandType.StoredProcedure;
				cmd.Parameters.Add("@id", Request.QueryString["id"]);

				// retreive info on this id
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);

				// attempt to read record
				if (dr.Read()) 
				{
					lblName.Text = dr["Name"].ToString();
					lblEmail.Text = dr["Email"].ToString();
				} 
				else 
				{
					lblName.Text = "Unable to read user information.";
				}

				// close objects
				dr.Close();
				cn.Close();

				// set up link to new user
				lnkNewLogin.NavigateUrl = "AuthNewLoginNew.aspx?id=" + Request.QueryString["id"].ToString();

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
			this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void FindPersonSelected(object sender, EventArgs e) 
		{
			lblEmail.Text = "a person was selected.";

		}

		private void btnContinue_Click(object sender, System.EventArgs e)
		{
			// make sure someone was selected
			if (PersonPicker.SelectedPerson == -1)
			{
				lblError.Text = "You must select a person first.";
				return;
			}

			// create conneciton, command
			SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("dbo.sp_LoginRequest_Associate", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@RequestID", Request.QueryString["id"]);
			cmd.Parameters.Add("@PersonID", PersonPicker.SelectedPerson);

			// run command
			cn.Open();
			cmd.ExecuteNonQuery();
			cn.Close();

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
			SmtpMail.SmtpServer = pics.Config.SMTPServer;
			SmtpMail.Send(msg);

			// show message
			pnlPerson.Visible = false;
			pnlDone.Visible = true;
		}


	
	}
}
