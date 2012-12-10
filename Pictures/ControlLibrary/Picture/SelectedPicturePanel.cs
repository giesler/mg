#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

#endregion

namespace msn2.net.Pictures.Controls
{
    public partial class SelectedPicturePanel : UserControl
    {
        private bool multiSelectMode = false;

        public SelectedPicturePanel()
        {
            InitializeComponent();
            UpdateControls();
        }

        public void AddPicture(PictureData picture)
        {
            this.pictureStack1.AddPicture(picture);
            this.pictureDetailEditor.AddPicture(picture);

            if (this.multiSelectMode == false)
            {
                UpdateControls();
            }
        }

        public void RemovePicture(int pictureId)
        {
            this.pictureStack1.RemovePicture(pictureId);
            this.pictureDetailEditor.RemovePicture(pictureId);

            if (this.multiSelectMode == false)
            {
                UpdateControls();
            }
        }

        public void ClearPictures()
        {
            List<int> pictures = new List<int>();
            foreach (PictureItem item in this.pictureStack1.Pictures)
            {
                pictures.Add(item.PictureId);
            }

            try
            {
                this.MultiSelectStart();
                foreach (int pictureId in pictures)
                {
                    this.RemovePicture(pictureId);
                }
            }
            finally
            {
                this.MultiSelectEnd();
            }
        }

        public void MultiSelectStart()
        {
            this.pictureStack1.SuspendPaint = true;
            this.multiSelectMode = true;
        }

        public void MultiSelectEnd()
        {
            this.multiSelectMode = false;
            this.pictureStack1.SuspendPaint = false;
            this.pictureStack1.Refresh();
            this.UpdateControls();
        }

        private void UpdateControls()
        {
            int count = this.pictureStack1.Pictures.Count;

            Color labelColor = (count > 0 ? SystemColors.ControlText : SystemColors.GrayText);
            this.taskLabel.ForeColor = labelColor;

            if (count == 0)
            {
                taskList.Controls.Clear();
            }
            else 
            {
                AddTasks();
            }
        }

        private void AddTasks()
        {
            taskList.Controls.Clear();

            LinkLabel addSecurityGroup = new LinkLabel();
            addSecurityGroup.Text = "Share with...";
            addSecurityGroup.LinkClicked += new LinkLabelLinkClickedEventHandler(addSecurityGroup_LinkClicked);
            addSecurityGroup.Dock = DockStyle.Top;
            taskList.Controls.Add(addSecurityGroup);

            LinkLabel removeFromCategory = new LinkLabel();
            removeFromCategory.Text = "Remove from category";
            removeFromCategory.LinkClicked += new LinkLabelLinkClickedEventHandler(removeFromCategory_LinkClicked);
            removeFromCategory.Dock = DockStyle.Top;
            taskList.Controls.Add(removeFromCategory);

            LinkLabel addToCategory = new LinkLabel();
            addToCategory.Text = "Add to category";
            addToCategory.LinkClicked += new LinkLabelLinkClickedEventHandler(addToCategory_LinkClicked);
            addToCategory.Dock = DockStyle.Top;
            taskList.Controls.Add(addToCategory);

            LinkLabel setCategoryIndexPic = new LinkLabel();
            setCategoryIndexPic.Text = "Set as index pic";
            setCategoryIndexPic.LinkClicked += new LinkLabelLinkClickedEventHandler(this.setCategoryIndexPic_LinkClicked);
            setCategoryIndexPic.Dock = DockStyle.Top;
            taskList.Controls.Add(setCategoryIndexPic);

        }

        void addToCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fSelectCategory selCat = fSelectCategory.GetSelectCategoryDialog();
            if (selCat.ShowDialog(this) == DialogResult.OK)
            {
                foreach (PictureItem item in this.pictureStack1.Pictures)
                {
                    PicContext.Current.PictureManager.AddToCategory(item.PictureId, selCat.SelectedCategory.CategoryId);
                }
                this.pictureDetailEditor.AddCategory(selCat.SelectedCategory);
            }
        }

        void setCategoryIndexPic_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int pictureCount = this.pictureStack1.Pictures.Count;
            if (pictureCount > 1)
            {
                MessageBox.Show("You can only set one picture as the category index picture.", "Set Index", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                List<CategoryInfo> categories = this.pictureDetailEditor.GetCurrentCategories();

                if (categories.Count == 1)
                {
                    PicContext.Current.CategoryManager.SetCategoryPictureId(
                        categories[0].CategoryId,
                        this.pictureStack1.Pictures[0].PictureId);
                }
                else
                {
                    CategoryListDialog dialog = new CategoryListDialog(categories);
                    dialog.SelectionMode = SelectionMode.One;
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        CategoryInfo category = dialog.GetSelectedCategory();
                        PicContext.Current.CategoryManager.SetCategoryPictureId(
                            category.CategoryId,
                            this.pictureStack1.Pictures[0].PictureId);
                    }
                }
            }
        }

        void removeFromCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            List<CategoryInfo> categories = this.pictureDetailEditor.GetAllCurrentCategories();

            CategoryListDialog dialog = new CategoryListDialog(categories);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                List<CategoryInfo> removeList = dialog.GetSelectedCategories();
                foreach (CategoryInfo category in removeList)
                {
                    foreach (PictureItem item in this.pictureStack1.Pictures)
                    {
                        PicContext.Current.PictureManager.RemoveFromCategory(item.PictureId, category.CategoryId);
                     }
                    this.pictureDetailEditor.RemoveCategory(category);
                }
            }
        }

        void addSecurityGroup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fGroupSelect group = new fGroupSelect();
            if (group.ShowDialog(this) == DialogResult.OK)
            {
                foreach (PictureItem item in this.pictureStack1.Pictures)
                {
                    PicContext.Current.PictureManager.AddToSecurityGroup(item.PictureId, group.SelectedGroup.GroupID);
                }
            }
        }

        void removeSecurityGroup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fGroupSelect group = new fGroupSelect();
            if (group.ShowDialog(this) == DialogResult.OK)
            {
                foreach (PictureItem item in this.pictureStack1.Pictures)
                {
                    PicContext.Current.PictureManager.RemoveFromSecurityGroup(item.PictureId, group.SelectedGroup.GroupID);
                }
            }
        }
    }
}
