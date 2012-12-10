#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

#endregion

namespace msn2.net.Pictures.Controls
{
    partial class Slideshow : Form
    {
        private PictureItem item;
        private GetPreviousItemIdDelegate getPreviousId;
        private GetNextItemIdDelegate getNextId;
        private int pictureId;
        private PictureProperties editor;

        public Slideshow(GetPreviousItemIdDelegate getPreviousId, GetNextItemIdDelegate getNextId)
        {
            this.getPreviousId = getPreviousId;
            this.getNextId = getNextId;

            InitializeComponent();

            this.KeyPreview = true;

        }

        private void closeToolStripButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public void SetPicture(int pictureId)
        {
            if (null != item)
            {
                this.Controls.Remove(item);
                item.Dispose();
            }
            this.pictureId = pictureId;

            item = new PictureItem(pictureId);
            item.DrawShadow = false;
            item.Dock = DockStyle.Fill;
            this.Controls.Add(item);

            if (null != editor)
            {
                editor.SetPicture(this.pictureId);
            }

            UpdateControls();
        }

        private void closeToolStripButton_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Slideshow_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Slideshow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                this.Close();
            }
        }

        private void previousPictureToolStripButton_Click(object sender, EventArgs e)
        {
            int id = getPreviousId(item.PictureId);
            SetPicture(id);
        }

        private void nextPictureToolStripButton_Click(object sender, EventArgs e)
        {
            int id = getNextId(item.PictureId);
            SetPicture(id);
        }

        private void UpdateControls()
        {
            previousPictureToolStripButton.Enabled = false;
            nextPictureToolStripButton.Enabled = false;

            if (null != getPreviousId && getPreviousId(item.PictureId) > 0)
            {
                previousPictureToolStripButton.Enabled = true;
            }
            if (null != getNextId && getNextId(item.PictureId) > 0)
            {
                nextPictureToolStripButton.Enabled = true;
            }
        }

        private void propertiesToolStripButton_Click(object sender, EventArgs e)
        {
            if (null == this.editor)
            {
                this.editor = new PictureProperties();
                this.editor.SetPicture(this.pictureId);
                this.editor.Opacity = 0.75f;
                this.editor.Left = this.Left + this.Width - 100 - this.editor.Width;
                this.editor.Top = this.Top + this.Height - 100 - this.editor.Height;
            }

            propertiesToolStripButton.Checked = !propertiesToolStripButton.Checked;
            if (propertiesToolStripButton.Checked)
            {
                this.editor.Show(this);
            }
            else
            {
                this.editor.Hide();
            }
        }

    }

    public delegate int GetNextItemIdDelegate(int currentItem);
    public delegate int GetPreviousItemIdDelegate(int currentItem);
}