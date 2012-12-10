#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

#endregion

namespace msn2.net.Pictures.Controls
{
    public partial class MultiPictureEdit : UserControl
    {
        private List<int> pictures;
        private Hashtable pictureCategories = new Hashtable();
        private Hashtable pictureGroups = new Hashtable();

        public MultiPictureEdit()
        {
            InitializeComponent();
            pictures = new List<int>();

            description.DisplayMultipleItems = false;
            description.MultiLine = true;
            description.AcceptsReturn = true;

            UpdateControls();
        }

        public void ClearPictures()
        {
            title.FinishEdit();
            description.FinishEdit();
            dateTaken.FinishEdit();

            pictures.Clear();
            title.ClearItems();
            description.ClearItems();
            dateTaken.ClearItems();

            UpdateControls();

            this.pictureCategories.Clear();
            this.groups.Clear();
        }

        public void AddPicture(PictureData picture)
        {
            pictures.Add(picture.Id);

            title.AddItem(picture.Id, picture.Title);
            description.AddItem(picture.Id, picture.Description);
            dateTaken.AddItem(picture.Id, picture.DateTaken);

            List<Category> categories = PicContext.Current.PictureManager.GetPictureCategories(picture.Id);
            this.pictureCategories.Add(picture.Id, categories);

            this.UpdateCategories();

            List<PersonGroup> loadedGroups = PicContext.Current.PictureManager.GetPictureGroups(picture.Id);
            this.pictureGroups.Add(picture.Id, loadedGroups);

            this.UpdateGroups();

            UpdateControls();
        }

        #region Categories

        private void UpdateCategories()
        {
            if (this.AllPicturesHaveSameCategories())
            {
                this.categoryList.Visible = true;
                this.differentCategoriesLabel.Visible = false;
                categoryList.Clear();
                List<Category> categories = null;
                foreach (List<Category> tempCategories in this.pictureCategories.Values)
                {
                    categories = tempCategories;
                    break;
                }
                if (categories != null)
                {
                    foreach (Category category in categories)
                    {
                        categoryList.Add(category);
                    }
                }
            }
            else
            {
                this.categoryList.Visible = false;
                this.differentCategoriesLabel.Visible = true;
            }
        }

        private bool AllPicturesHaveSameCategories()
        {
            int count = 0;

            foreach (List<Category> categories in this.pictureCategories.Values)
            {
                if (count == 0)
                {
                    count = categories.Count;
                }

                if (categories.Count != count)
                {
                    return false;
                }

                // Check items against each other group collection
                foreach (Category category in categories)
                {
                    // Make sure in all other picture group hash tables
                    foreach (List<Category> tempCategories in this.pictureCategories.Values)
                    {
                        bool found = false;
                        foreach (Category tempCategory in tempCategories)
                        {
                            if (tempCategory.CategoryId == category.CategoryId)
                            {
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            return false;
                        }
                    }
                }

            }

            return true;
        }

        public void AddCategory(Category category)
        {
            if (!categoryList.Contains(category))
            {
                categoryList.Add(category);

                foreach (List<Category> categories in this.pictureCategories.Values)
                {
                    categories.Add(category);
                }
            }
        }

        public void RemoveCategory(Category category)
        {
            categoryList.Remove(category);

            foreach (List<Category> categories in this.pictureCategories.Values)
            {
                categories.Remove(category);
            }
        }

        public List<Category> GetCurrentCategories()
        {
            List<Category> categories = new List<Category>();

            foreach (CategoryLinkItem item in this.categoryList.Controls)
            {
                categories.Add(item.Category);
            }

            return categories;
        }

        #endregion

        #region Groups



        private void UpdateGroups()
        {
            if (this.AllPicturesHaveSameGroups())
            {
                this.groups.Visible = true;
                this.differentGroupsLabel.Visible = false;
                groups.Clear();
                List<PersonGroup> firstPictureGroups = null;
                foreach (List<PersonGroup> tempGroups in this.pictureGroups.Values)
                {
                    firstPictureGroups = tempGroups;
                    break;
                }
                if (firstPictureGroups != null)
                {
                    foreach (PersonGroup group in firstPictureGroups)
                    {
                        groups.Add(group);
                    }
                }
            }
            else
            {
                this.groups.Visible = false;
                this.differentGroupsLabel.Visible = true;
            }
        }

        private bool AllPicturesHaveSameGroups()
        {
            int count = 0;

            foreach (List<PersonGroup> picturePersonGroups in this.pictureGroups.Values)
            {
                if (count == 0)
                {
                    count = picturePersonGroups.Count;
                }

                if (picturePersonGroups.Count != count)
                {
                    return false;
                }

                // Check items against each other group collection
                foreach (PersonGroup group in picturePersonGroups)
                {
                    // Make sure in all other picture group hash tables
                    foreach (List<PersonGroup> tempGroups in this.pictureGroups.Values)
                    {
                        bool found = false;
                        foreach (PersonGroup tempGroup in tempGroups)
                        {
                            if (tempGroup.Id == group.Id)
                            {
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            return false;
                        }
                    }
                }

            }

            return true;
        }

        public void AddGroup(PersonGroup group)
        {
            if (!groups.Contains(group))
            {
                groups.Add(group);

                foreach (List<PersonGroup> personGroup in this.pictureGroups.Values)
                {
                    personGroup.Add(group);
                }
            }
        }

        public void RemoveGroup(PersonGroup group)
        {
            this.pictureGroups.Remove(group);

            foreach (List<PersonGroup> personGroup in this.pictureGroups.Values)
            {
                personGroup.Remove(group);
            }
        }

        public List<PersonGroup> GetCurrentGroups()
        {
            List<PersonGroup> personGroup = new List<PersonGroup>();

            foreach (GroupLinkItem item in this.groups.Controls)
            {
                personGroup.Add(item.Group);
            }

            return personGroup;
        }

        #endregion

        public void RemovePicture(int pictureId)
        {
            pictures.Remove(pictureId);
            title.RemoveItem(pictureId);
            description.RemoveItem(pictureId);
            dateTaken.RemoveItem(pictureId);
            
            this.pictureCategories.Remove(pictureId);
            this.pictureGroups.Remove(pictureId);

            this.UpdateCategories();
            this.UpdateGroups();

            UpdateControls();
        }

        private void UpdateControls()
        {
            Color labelColor = (pictures.Count > 0 ? SystemColors.ControlText : SystemColors.GrayText);
            titleLabel.ForeColor = labelColor;
            descriptionLabel.ForeColor = labelColor;
            labelDateTaken.ForeColor = labelColor;
            categoryLabel.ForeColor = labelColor;
            groupLabel.ForeColor = labelColor;
        }

        private void title_StringItemChanged(object sender, msn2.net.Pictures.Controls.UserControls.StringItemChangedEventArgs e)
        {
            // Update the picture title
            PictureData data = PicContext.Current.PictureManager.GetPicture(e.Id);
            data.Title = e.NewValue;
            PicContext.Current.PictureManager.Save(data);
        }

        private void description_StringItemChanged(object sender, msn2.net.Pictures.Controls.UserControls.StringItemChangedEventArgs e)
        {
            // Update the description
            PictureData data = PicContext.Current.PictureManager.GetPicture(e.Id);
            data.Description = e.NewValue;
            PicContext.Current.PictureManager.Save(data);
        }

        private void dateTaken_DateTimeItemChanged(object sender, msn2.net.Pictures.Controls.UserControls.DateTimeItemChangedEventArgs e)
        {
            // Update the date taken
            PictureData data = PicContext.Current.PictureManager.GetPicture(e.Id);
            data.DateTaken = dateTaken.Value;
            PicContext.Current.PictureManager.Save(data);
        }

    }
}
