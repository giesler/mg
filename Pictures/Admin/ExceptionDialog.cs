#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web.Mail;
using System.Net;

#endregion

namespace msn2.net.Pictures
{
    partial class ExceptionDialog : Form
    {
        private Exception exception;

        public ExceptionDialog(Exception ex)
        {
            this.exception = ex;

            InitializeComponent();

            label1.Text = "An unhandled error has occurred.";
            label2.Text = ex.GetType().ToString() + ": " + ex.Message;

            button1.Text = "&Send to Mike";
            button1.Click += new EventHandler(button1_Click);
        }

        void button1_Click(object sender, EventArgs e)
        {
            string exceptionName = "Exception: " + exception;
            string message = exceptionName + Environment.NewLine + "Stack Trace: " + Environment.NewLine;
            message += exception.StackTrace.ToString();

            MailMessage mail = new MailMessage();
            mail.To = "mike@giesler.org";
            mail.Subject = "Picture Admin Exception: " + exceptionName;
            mail.Body = message;
            mail.From = Environment.UserName + "@msn2.net";

            SmtpMail.SmtpServer = Msn2Config.Load().SmtpServer;
            SmtpMail.Send(mail);

            this.Close();
        }
    }
}