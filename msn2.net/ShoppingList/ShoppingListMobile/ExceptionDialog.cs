using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsMobile.PocketOutlook;

namespace msn2.net.ShoppingList
{
    public partial class ExceptionDialog : Form
    {
        private Exception exception;

        public ExceptionDialog()
        {
            InitializeComponent();
        }

        public ExceptionDialog(Exception ex)
        {
            InitializeComponent();

            this.exception = ex;

            this.label1.Text = ex.GetType().Name + ": " + ex.Message;
            this.textBox1.Text = ex.ToString();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();

            if (this.checkBox1.Checked == true)
            {
                OutlookSession session = new OutlookSession();

                EmailMessage msg = new EmailMessage();
                msg.To.Add(new Recipient("mike@giesler.org"));
                msg.Subject = "ShoppingListMobile: " + this.exception.GetType().Name + " - " + this.exception.Message;
                msg.BodyText = this.exception.ToString();
                msg.Send(session.EmailAccounts[0]);
            }
        }
    }
}