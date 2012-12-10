using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{
    public partial class LoginDialog : Form
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.email.Text.Length > 0)
            {
                this.password.Focus();
            }
        }

        private void login_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Visible = false;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Visible = false;
        }

        public string Email
        {
            get
            {
                return this.email.Text;
            }
            set
            {
                this.email.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return this.password.Text;
            }
            set
            {
                this.password.Text = value;
            }
        }

    }
}