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
    public partial class EditItemForm : Form
    {
        public EditItemForm()
        {
            InitializeComponent();
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
    }
}