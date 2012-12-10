using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;

namespace mn2.net.ShoppingList
{
    public partial class EditItemForm : Form
    {
        private InputPanel ip;

        public EditItemForm(InputPanel ip)
        {
            InitializeComponent();

            this.ip = ip;
            this.ip.EnabledChanged += new EventHandler(ip_EnabledChanged);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this.ip.EnabledChanged -= new EventHandler(ip_EnabledChanged);
        }

        void ip_EnabledChanged(object sender, EventArgs e)
        {
            int offset = this.ip.Enabled ? this.ip.Bounds.Height : 0;

            this.textBox1.Height = this.ClientSize.Height - this.textBox1.Top * 2 - offset;
        }

        private void menuOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void menuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.menuOK.Enabled = this.textBox1.Text.Trim().Length > 0;
        }

        public string ItemText
        {
            get
            {
                return this.textBox1.Text.Trim();
            }
            set
            {
                this.textBox1.Text = value;
            }
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            if (this.ip.Enabled == false)
            {
                if (this.Height > this.Width)
                {
                    this.ip.Enabled = true;
                }
            }
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            if (this.ip.Enabled == true)
            {
                this.ip.Enabled = false;
            }
        }
    }
}