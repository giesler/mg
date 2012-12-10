using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mn2.net.ShoppingList
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();

            this.deviceName.Text = System.Net.Dns.GetHostName();
        }

        public string PIN
        {
            get
            {
                return this.pin.Text.Trim();
            }
            set
            {
                this.pin.Text = value;
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void pin_TextChanged(object sender, EventArgs e)
        {
            this.ok.Enabled = this.pin.Text.Trim().Length > 3;
        }
    }
}