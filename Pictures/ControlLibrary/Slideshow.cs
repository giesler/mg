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
        private PictureData picture;
        private PictureProperties editor;
        private Form sourceForm = null;

        public Slideshow(GetPreviousItemIdDelegate getPreviousId, GetNextItemIdDelegate getNextId)
        {
            this.getPreviousId = getPreviousId;
            this.getNextId = getNextId;

            InitializeComponent();

            this.KeyPreview = true;

        }

        public void SetSourceForm(Form sourceForm)
        {
            this.sourceForm = sourceForm;
        }

        private void closeToolStripButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public void SetPicture(PictureData picture)
        {
            if (null != item)
            {
                this.Controls.Remove(item);
                item.Dispose();
            }
            this.picture = picture;

            item = new PictureItem(picture);
            item.DrawShadow = false;
            item.Dock = DockStyle.Fill;
            this.Controls.Add(item);

            if (null != editor)
            {
                editor.SetPicture(this.picture);
            }

            UpdateControls();
        }

        private void closeToolStripButton_Click_1(object sender, EventArgs e)
        {
            this.Close();

            if (this.sourceForm != null)
            {
                this.sourceForm.Focus();
            }
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
            PictureData picture = getPreviousId(item.PictureId);
            SetPicture(picture);
        }

        private void nextPictureToolStripButton_Click(object sender, EventArgs e)
        {
            PictureData picture = getNextId(item.PictureId);
            SetPicture(picture);
        }

        private void UpdateControls()
        {
            previousPictureToolStripButton.Enabled = false;
            nextPictureToolStripButton.Enabled = false;

            if (null != getPreviousId && getPreviousId(item.PictureId) != null)
            {
                previousPictureToolStripButton.Enabled = true;
            }
            if (null != getNextId && getNextId(item.PictureId) != null)
            {
                nextPictureToolStripButton.Enabled = true;
            }
        }

        private void propertiesToolStripButton_Click(object sender, EventArgs e)
        {
            if (null == this.editor)
            {
                this.editor = new PictureProperties();
                this.editor.SetPicture(this.picture);
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

        private void addtocategoryToolStripButton_Click(object sender, EventArgs e)
        {
            fSelectCategory selCat = fSelectCategory.GetSelectCategoryDialog();
            if (selCat.ShowDialog(this) == DialogResult.OK)
            {
                PicContext.Current.PictureManager.AddToCategory(this.picture.Id, selCat.SelectedCategory.CategoryId);
            }
        }

    }

    public delegate PictureData GetNextItemIdDelegate(int currentItem);
    public delegate PictureData GetPreviousItemIdDelegate(int currentItem);
}