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

        private Slideshow()
        {
            InitializeComponent();
        }

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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (e.Cancel == false)
            {
                if (this.editor != null)
                {
                    this.editor.Close();
                }

                if (this.sourceForm != null)
                {
                    this.sourceForm.Focus();
                }
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

        private void UpdateControls()
        {
            toolPrevious.Enabled = false;
            toolNext.Enabled = false;

            if (null != getPreviousId && getPreviousId(item.PictureId) != null)
            {
                toolPrevious.Enabled = true;
            }
            if (null != getNextId && getNextId(item.PictureId) != null)
            {
                toolNext.Enabled = true;
            }
        }

        private void toolClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolPrevious_Click(object sender, EventArgs e)
        {
            PictureData picture = getPreviousId(item.PictureId);
            SetPicture(picture);
        }

        private void toolNext_Click(object sender, EventArgs e)
        {
            PictureData picture = getNextId(item.PictureId);
            SetPicture(picture);
        }

        private void toolProperties_Click(object sender, EventArgs e)
        {
            if (null == this.editor)
            {
                this.editor = new PictureProperties();
                this.editor.SetPicture(this.picture);
                this.editor.Opacity = 0.75f;
                this.editor.Left = this.Left + this.Width - 100 - this.editor.Width;
                this.editor.Top = this.Top + this.Height - 100 - this.editor.Height;
            }

            toolProperties.Checked = !toolProperties.Checked;
            if (toolProperties.Checked)
            {
                this.editor.Show(this);
            }
            else
            {
                this.editor.Hide();
            }

        }

        private void toolAddToCategory_Click(object sender, EventArgs e)
        {
            fSelectCategory selCat = fSelectCategory.GetSelectCategoryDialog();
            if (selCat.ShowDialog(this) == DialogResult.OK)
            {
                PicContext.Current.PictureManager.AddToCategory(
                    this.picture.Id, 
                    selCat.SelectedCategory.CategoryId);
            }
        }

    }

    public delegate PictureData GetNextItemIdDelegate(int currentItem);
    public delegate PictureData GetPreviousItemIdDelegate(int currentItem);
}