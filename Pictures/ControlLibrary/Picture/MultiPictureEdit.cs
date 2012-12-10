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
        private Dictionary<int, PictureData> pictures;
        private Hashtable pictureCategories = new Hashtable();
        private Hashtable pictureGroups = new Hashtable();
        private Hashtable picturePeople = new Hashtable();

        public MultiPictureEdit()
        {
            InitializeComponent();
            pictures = new Dictionary<int, PictureData>();

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

            this.groups.Clear();
            this.categoryList.Clear();
            this.personList.Clear();

            this.pictureGroups.Clear();
            this.pictureCategories.Clear();
            this.picturePeople.Clear();
        }

        public void AddPicture(PictureData picture)
        {
            pictures.Add(picture.Id, picture);

            title.AddItem(picture.Id, picture.Title);
            description.AddItem(picture.Id, picture.Description);
            dateTaken.AddItem(picture.Id, picture.DateTaken);

            List<CategoryInfo> categories = PicContext.Current.PictureManager.GetPictureCategories(picture.Id);
            this.pictureCategories.Add(picture.Id, categories);

            this.UpdateCategories();

            List<PersonGroupInfo> loadedGroups = PicContext.Current.PictureManager.GetPictureGroups(picture.Id);
            this.pictureGroups.Add(picture.Id, loadedGroups);

            this.UpdateGroups();

            List<PersonInfo> loadedPeople = PicContext.Current.PictureManager.GetPicturePeople(picture.Id);
            this.picturePeople.Add(picture.Id, loadedPeople);

            this.UpdatePeople();

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
                List<CategoryInfo> categories = null;
                foreach (List<CategoryInfo> tempCategories in this.pictureCategories.Values)
                {
                    categories = tempCategories;
                    break;
                }
                if (categories != null)
                {
                    foreach (CategoryInfo category in categories)
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

            foreach (List<CategoryInfo> categories in this.pictureCategories.Values)
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
                foreach (CategoryInfo category in categories)
                {
                    // Make sure in all other picture group hash tables
                    foreach (List<CategoryInfo> tempCategories in this.pictureCategories.Values)
                    {
                        bool found = false;
                        foreach (CategoryInfo tempCategory in tempCategories)
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

        public void AddCategory(CategoryInfo category)
        {
            if (!categoryList.Contains(category))
            {
                categoryList.Add(category);

                foreach (List<CategoryInfo> categories in this.pictureCategories.Values)
                {
                    categories.Add(category);
                }
            }
        }

        public void RemoveCategory(CategoryInfo category)
        {
            categoryList.Remove(category);

            foreach (List<CategoryInfo> categories in this.pictureCategories.Values)
            {
                categories.Remove(category);
            }
        }

        public List<CategoryInfo> GetAllCurrentCategories()
        {
            List<CategoryInfo> categories = new List<CategoryInfo>();

            foreach (PictureData picture in this.pictures.Values)
            {
                List<CategoryInfo> picCategories = PicContext.Current.PictureManager.GetPictureCategories(
                    picture.Id);
                foreach (CategoryInfo category in picCategories)
                {
                    if (categories.Contains(category) == false)
                    {
                        categories.Add(category);
                    }
                }
            }

            return categories;
        }

        public List<CategoryInfo> GetCurrentCategories()
        {
            List<CategoryInfo> categories = new List<CategoryInfo>();

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
                List<PersonGroupInfo> firstPictureGroups = null;
                foreach (List<PersonGroupInfo> tempGroups in this.pictureGroups.Values)
                {
                    firstPictureGroups = tempGroups;
                    break;
                }
                if (firstPictureGroups != null)
                {
                    foreach (PersonGroupInfo group in firstPictureGroups)
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

            foreach (List<PersonGroupInfo> picturePersonGroups in this.pictureGroups.Values)
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
                foreach (PersonGroupInfo group in picturePersonGroups)
                {
                    // Make sure in all other picture group hash tables
                    foreach (List<PersonGroupInfo> tempGroups in this.pictureGroups.Values)
                    {
                        bool found = false;
                        foreach (PersonGroupInfo tempGroup in tempGroups)
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

        public void AddGroup(PersonGroupInfo group)
        {
            if (!groups.Contains(group))
            {
                groups.Add(group);

                foreach (List<PersonGroupInfo> personGroup in this.pictureGroups.Values)
                {
                    personGroup.Add(group);
                }
            }
        }

        public void RemoveGroup(PersonGroupInfo group)
        {
            this.pictureGroups.Remove(group);

            foreach (List<PersonGroupInfo> personGroup in this.pictureGroups.Values)
            {
                personGroup.Remove(group);
            }
        }

        public List<PersonGroupInfo> GetCurrentGroups()
        {
            List<PersonGroupInfo> personGroup = new List<PersonGroupInfo>();

            foreach (GroupLinkItem item in this.groups.Controls)
            {
                personGroup.Add(item.Group);
            }

            return personGroup;
        }

        #endregion

        #region People

        private void UpdatePeople()
        {
            if (this.AllPicturesHaveSamePeople())
            {
                this.personList.Visible = true;
                this.differentPeopleLabel.Visible = false;
                this.personList.Clear();

                List<PersonInfo> firstPicturePersons = null;
                foreach (List<PersonInfo> tempPersons in this.picturePeople.Values)
                {
                    firstPicturePersons = tempPersons;
                    break;
                }
                if (firstPicturePersons != null)
                {
                    foreach (PersonInfo person in firstPicturePersons)
                    {
                        this.personList.Add(person);
                    }
                }
            }
            else
            {
                this.personList.Visible = false;
                this.differentPeopleLabel.Visible = true;
            }
        }

        private bool AllPicturesHaveSamePeople()
        {
            int count = 0;

            foreach (List<PersonInfo> picturePersonInfos in this.picturePeople.Values)
            {
                if (count == 0)
                {
                    count = picturePersonInfos.Count;
                }

                if (picturePersonInfos.Count != count)
                {
                    return false;
                }

                // Check items against each other person collection
                foreach (PersonInfo person in picturePersonInfos)
                {
                    // Make sure in all other picture person hash tables
                    foreach (List<PersonInfo> tempPersons in this.picturePeople.Values)
                    {
                        bool found = false;
                        foreach (PersonInfo tempPerson in tempPersons)
                        {
                            if (tempPerson.Id == person.Id)
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

        public void AddPerson(PersonInfo person)
        {
            if (!this.personList.Contains(person))
            {
                this.personList.Add(person);

                foreach (List<PersonInfo> personPerson in this.picturePeople.Values)
                {
                    personPerson.Add(person);
                }
            }
        }

        public void RemovePerson(PersonInfo person)
        {
            this.personList.Remove(person);

            foreach (List<PersonInfo> personPerson in this.picturePeople.Values)
            {
                personPerson.Remove(person);
            }
        }

        public List<PersonInfo> GetCurrentPersons()
        {
            List<PersonInfo> personPerson = new List<PersonInfo>();

            foreach (PersonLinkItem item in this.personList.Controls)
            {
                personPerson.Add(item.Person);
            }

            return personPerson;
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
            this.picturePeople.Remove(pictureId);

            this.UpdateCategories();
            this.UpdateGroups();
            this.UpdatePeople();

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
            peopleLabel.ForeColor = labelColor;
        }

        private void title_StringItemChanged(object sender, msn2.net.Pictures.Controls.UserControls.StringItemChangedEventArgs e)
        {
            // Update the picture title
            PictureData picture = this.pictures[e.Id];
            picture.Title = e.NewValue;
            PicContext.Current.PictureManager.Save(picture);
        }

        private void description_StringItemChanged(object sender, msn2.net.Pictures.Controls.UserControls.StringItemChangedEventArgs e)
        {
            // Update the description
            PictureData picture = this.pictures[e.Id];
            picture.Description = e.NewValue;
            PicContext.Current.PictureManager.Save(picture);
        }

        private void dateTaken_DateTimeItemChanged(object sender, msn2.net.Pictures.Controls.UserControls.DateTimeItemChangedEventArgs e)
        {
            // Update the date taken
            PictureData picture = this.pictures[e.Id];
            picture.DateTaken = dateTaken.Value;
            PicContext.Current.PictureManager.Save(picture);
        }

    }
}
