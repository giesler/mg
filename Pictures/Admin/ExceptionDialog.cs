#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Diagnostics;
using System.IO;

#endregion

namespace msn2.net.Pictures.Controls
{
    partial class ExceptionDialog : Form
    {
        private Exception exception;

        public ExceptionDialog(Exception ex)
        {
            this.exception = ex;

            InitializeComponent();

            labelTitle.Text = "An unhandled error has occurred.";
            labelDetails.Text = ex.GetType().ToString() + ": " + ex.Message;

            button1.Click += new EventHandler(button1_Click);
        }

        void button1_Click(object sender, EventArgs e)
        {
            string exceptionName = exception.GetType().FullName + ": " + exception.Message;
            string message = ExceptionToString(exception);

            string loc = Assembly.GetExecutingAssembly().Location;
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(loc);

            MailMessage mail = new MailMessage();
            mail.To.Add("mike@giesler.org");
            mail.From = new MailAddress(Environment.UserName + "@msn2.net");
            mail.Subject = string.Format(
                "Picture Admin v{0} - {1}",
                fvi.FileVersion,
                exceptionName);
            mail.Body = message;

            SmtpClient client = new SmtpClient(PictureConfig.Load().SmtpServer);
            client.UseDefaultCredentials = true;
            client.Send(mail);

            this.Close();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {

        }

        public static string ExceptionToString(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ex.GetType().FullName);
            sb.Append(": ");
            sb.AppendLine(ex.Message);
            sb.Append("Stack trace: ");
            sb.AppendLine(ex.StackTrace);

            Exception innerException = ex.InnerException;
            while (innerException != null)
            {
                sb.AppendLine("------  Inner Exception -------------------------");
                sb.Append(innerException.GetType().FullName);
                sb.Append(": ");
                sb.AppendLine(innerException.Message);
                sb.Append("Stack trace: ");
                sb.AppendLine(innerException.StackTrace);

                innerException = innerException.InnerException;
            }

            return sb.ToString();
        }
    }
}