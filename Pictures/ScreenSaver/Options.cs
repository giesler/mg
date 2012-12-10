using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace msn2.net.Pictures
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();

            this.interval.Value = Properties.Settings.Default.SlideshowInterval;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SlideshowInterval = (int)this.interval.Value;
            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
