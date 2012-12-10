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

namespace vbsw
{
	/// <summary>
	/// Summary description for feedback.
	/// </summary>
	public class feedback : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox name;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.TextBox email;
		protected System.Web.UI.WebControls.Button buttonSend;
		protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator2;
		protected System.Web.UI.WebControls.RadioButtonList subject;
		protected System.Web.UI.WebControls.Panel pnlEnterInfo;
		protected System.Web.UI.WebControls.Panel pnlDone;
		protected System.Web.UI.WebControls.TextBox body;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
				// Try to set the email address
				if (Request.Cookies["email"] != null) 
					email.Text = Request.Cookies["email"].Value;
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
			this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void buttonSend_Click(object sender, System.EventArgs e)
		{
			MailMessage message = new MailMessage();
			
			message.From = name.Text + " <" + email.Text + ">";
			message.To		= "help@installassistant.com";
			message.Subject	= "IA Site Feedback: " + subject.SelectedItem.Value;
			message.Body	= body.Text;

			SmtpMail.SmtpServer = Config.SmtpServer;
			SmtpMail.Send(message);

			// Save the email for future use
			Response.Cookies["email"].Value = email.Text;
			Response.Cookies["email"].Expires = DateTime.Now.Add(new TimeSpan(500, 0, 0, 0, 0));
		
			pnlEnterInfo.Visible = false;
			pnlDone.Visible = true;
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("default.aspx");
		}

	}
}
