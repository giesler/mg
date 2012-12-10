using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using msn2.net.Pictures.Controls;

namespace msn2.net.Pictures
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();

            this.interval.Value = Properties.Settings.Default.SlideshowInterval;
            this.path.Text = Properties.Settings.Default.Path;

            this.group.Items.Add("<current user>");
            this.group.SelectedIndex = 0;

            var q = from g in PicContext.Current.GroupManager.GetGroups()
                    orderby g.GroupName
                    select g;

            foreach (var i in q)
            {
                int index = this.group.Items.Add(i);
                if (i.Id == Properties.Settings.Default.Group)
                {
                    this.group.SelectedIndex = index;
                }
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SlideshowInterval = (int)this.interval.Value;
            Properties.Settings.Default.Path = this.path.Text;
            
            int groupId = 0;
            if (this.group.SelectedIndex > 0)
            {
                Group group = (Group)this.group.SelectedItem;
                groupId = group.Id;
            }
            Properties.Settings.Default.Group = groupId;

            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void changePath_Click(object sender, EventArgs e)
        {
            fSelectCategory dialog = new fSelectCategory();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.path.Text = dialog.SelectedCategory.Path;
            }
        }
    }
}
