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
        public SelectedPicturePanel()
        {
            InitializeComponent();
            UpdateControls();
        }

        public void AddPicture(int pictureId)
        {
            this.pictureStack1.AddPicture(pictureId);
            this.pictureDetailEditor.AddPicture(pictureId);

            UpdateControls();
        }

        public void RemovePicture(int pictureId)
        {
            this.pictureStack1.RemovePicture(pictureId);
            this.pictureDetailEditor.RemovePicture(pictureId);

            UpdateControls();
        }

        public void ClearPictures()
        {
            List<int> pictures = new List<int>();
            foreach (PictureItem item in this.pictureStack1.Pictures)
            {
                pictures.Add(item.PictureId);
            }

            this.MultiSelectStart();
            foreach (int pictureId in pictures)
            {
                this.RemovePicture(pictureId);
            }
            this.MultiSelectEnd();
        }

        public void MultiSelectStart()
        {
            this.pictureStack1.SuspendPaint = true;
        }

        public void MultiSelectEnd()
        {
            this.pictureStack1.SuspendPaint = false;
            this.pictureStack1.Refresh();
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

            LinkLabel removeSecurityGroup = new LinkLabel();
            removeSecurityGroup.Text = "Remove security group";
            removeSecurityGroup.LinkClicked += new LinkLabelLinkClickedEventHandler(removeSecurityGroup_LinkClicked);
            removeSecurityGroup.Dock = DockStyle.Top;
            taskList.Controls.Add(removeSecurityGroup);

            LinkLabel addSecurityGroup = new LinkLabel();
            addSecurityGroup.Text = "Add security group";
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

        void removeFromCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Collection<Category> categories = this.pictureDetailEditor.GetCurrentCategories();

            CategoryListDialog dialog = new CategoryListDialog(categories);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                Collection<Category> removeList = dialog.GetSelectedCategories();
                foreach (Category category in removeList)
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
