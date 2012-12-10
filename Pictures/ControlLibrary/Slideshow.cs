#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

#endregion

namespace msn2.net.Pictures.Controls
{
    public partial class Slideshow : Form
    {
        private PictureItem item;
        protected GetPreviousItemIdDelegate getPreviousId;
        protected GetNextItemIdDelegate getNextId;
        private PictureData picture;
        private PictureProperties editor;
        private PeopleSelect peopleSelect;
        private GroupSelect groupSelect;
        private CategorySelect categorySelect;
        private Form sourceForm = null;
        private PictureControlSettings settings;
        private bool loading = false;

        private Slideshow()
        {
            this.settings = new PictureControlSettings();
            InitializeComponent();
        }

        public Slideshow(
            PictureControlSettings settings,
            GetPreviousItemIdDelegate getPreviousId, 
            GetNextItemIdDelegate getNextId)
        {
            this.getPreviousId = getPreviousId;
            this.getNextId = getNextId;
            this.settings = settings;

            InitializeComponent();

            this.KeyPreview = true;
        }

        protected PictureData CurrentPicture
        {
            get
            {
                return this.picture;
            }
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
            item.DrawBorder = false;
            item.Dock = DockStyle.Fill;
            this.Controls.Add(item);

            if (null != editor)
            {
                editor.SetPicture(this.picture);
            }
            if (this.peopleSelect != null)
            {
                LoadPeople();
            }
            if (this.groupSelect != null)
            {
                LoadGroups();
            }
            if (this.categorySelect != null)
            {
                LoadCategories();
            }

            UpdateControls();
        }

        private void LoadPeople()
        {
            List<PersonInfo> people = PicContext.Current.PictureManager.GetPicturePeople(this.picture.Id);

            this.loading = true;
            peopleSelect.PersonPicker.ClearSelectedPeople();
            foreach (PersonInfo person in people)
            {
                peopleSelect.PersonPicker.AddSelectedPerson(person.Id);
            }
            this.loading = false;
        }

        private void LoadGroups()
        {
            List<PersonGroup> groups = PicContext.Current.PictureManager.GetPictureGroups(this.picture.Id);

            this.loading = true;
            groupSelect.GroupPicker.ClearSelectedGroups();
            foreach (PersonGroup group in groups)
            {
                groupSelect.GroupPicker.AddSelectedGroup(group.Id);
            }
            this.loading = false;
        }

        private void LoadCategories()
        {
            List<Category> categories = PicContext.Current.PictureManager.GetPictureCategories(this.picture.Id);

            this.loading = true;
            categorySelect.CategoryPicker.ClearSelectedCategories();
            foreach (Category category in categories)
            {
                categorySelect.CategoryPicker.AddSelectedCategory(category.CategoryId);
            }
            this.loading = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (settings.Slideshow_Editor_IsOpen == true)
            {
                toolProperties_Click(this, EventArgs.Empty);
            }

            if (settings.Slideshow_People_IsOpen == true)
            {
                toolPeople_Click(this, EventArgs.Empty);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (e.Cancel == false)
            {
                if (this.editor != null)
                {
                    this.settings.Slideshow_Editor_IsOpen = this.editor.Visible;
                    this.settings.Slideshow_Editor_Left = this.editor.Left;
                    this.settings.Slideshow_Editor_Top = this.editor.Top;
                    this.editor.Close();
                }

                if (this.peopleSelect != null)
                {
                    this.settings.Slideshow_People_IsOpen = this.peopleSelect.Visible;
                    this.settings.Slideshow_People_Left = this.peopleSelect.Left;
                    this.settings.Slideshow_People_Top = this.peopleSelect.Top;
                    this.peopleSelect.Close();
                }

                if (this.groupSelect != null)
                {
                    this.settings.Slideshow_Group_IsOpen = this.groupSelect.Visible;
                    this.settings.Slideshow_Group_Left = this.groupSelect.Left;
                    this.settings.Slideshow_People_Top = this.groupSelect.Top;
                    this.groupSelect.Close();
                }
                
                if (this.categorySelect != null)
                {
                    this.settings.Slideshow_Category_IsOpen = this.categorySelect.Visible;
                    this.settings.Slideshow_Category_Left = this.categorySelect.Left;
                    this.settings.Slideshow_Category_Top = this.categorySelect.Top;
                    this.categorySelect.Close();
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
                if (this.picture != null)
                {
                    this.editor.SetPicture(this.picture);
                }
                this.editor.Opacity = 0.75f;
                this.editor.Left = PictureControlSettings.GetSafeLeft(this.editor, this.settings.Slideshow_Editor_Left);
                this.editor.Top = PictureControlSettings.GetSafeTop(this.editor, this.settings.Slideshow_Editor_Top);
                this.editor.FormClosed += new FormClosedEventHandler(this.OnPropertiesClosed);
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

        void OnPropertiesClosed(object sender, FormClosedEventArgs e)
        {
            this.toolProperties.Checked = false;
            this.editor = null;
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

        #region People

        private void toolPeople_Click(object sender, EventArgs e)
        {
            if (this.peopleSelect == null)
            {
                this.peopleSelect = new PeopleSelect();
                this.peopleSelect.Opacity = 0.75f;
                this.peopleSelect.Left = PictureControlSettings.GetSafeLeft(this.peopleSelect, this.settings.Slideshow_People_Left);
                this.peopleSelect.Top = PictureControlSettings.GetSafeTop(this.peopleSelect, this.settings.Slideshow_People_Top);
                this.peopleSelect.FormClosed += new FormClosedEventHandler(peopleSelect_FormClosed);
                this.peopleSelect.PersonPicker.AddedPerson += new AddedPersonEventHandler(PersonPicker_AddedPerson);
                this.peopleSelect.PersonPicker.RemovedPerson += new RemovedPersonEventHandler(PersonPicker_RemovedPerson);

                if (this.picture != null)
                {
                    LoadPeople();
                }
            }

            toolPeople.Checked = !toolPeople.Checked;
            if (toolPeople.Checked == true)
            {
                this.peopleSelect.Show(this);
            }
            else
            {
                this.peopleSelect.Hide();
            }
        }

        void PersonPicker_AddedPerson(object sender, PersonPickerEventArgs e)
        {
            if (this.loading == false)
            {
                PicContext.Current.PictureManager.AddPerson(
                    this.picture.Id,
                    e.PersonID);

                if (this.editor != null)
                {
                    PersonInfo person = PicContext.Current.UserManager.GetPerson(e.PersonID);
                    this.editor.Editor.AddPerson(person);
                }
            }
        }

        void PersonPicker_RemovedPerson(object sender, PersonPickerEventArgs e)
        {
            if (this.loading == false)
            {
                PicContext.Current.PictureManager.RemovePerson(
                    this.picture.Id,
                    e.PersonID);

                if (this.editor != null)
                {
                    PersonInfo person = PicContext.Current.UserManager.GetPerson(e.PersonID);
                    this.editor.Editor.RemovePerson(person);
                }
            }
        }

        void peopleSelect_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.toolPeople.Checked = false;
            this.peopleSelect = null;
        }

        #endregion

        #region Groups

        private void toolGroups_Click(object sender, EventArgs e)
        {
            if (this.groupSelect == null)
            {
                this.groupSelect = new GroupSelect();
                this.groupSelect.Opacity = 0.75f;
                this.groupSelect.Left = PictureControlSettings.GetSafeLeft(this.groupSelect, this.settings.Slideshow_Group_Left);
                this.groupSelect.Top = PictureControlSettings.GetSafeTop(this.groupSelect, this.settings.Slideshow_Group_Top);
                this.groupSelect.FormClosed += new FormClosedEventHandler(groupSelect_FormClosed);
                this.groupSelect.GroupPicker.AddedGroup += new AddedGroupEventHandler(GroupPicker_AddedGroup);
                this.groupSelect.GroupPicker.RemovedGroup += new RemovedGroupEventHandler(GroupPicker_RemovedGroup);

                if (this.picture != null)
                {
                    LoadGroups();
                }
            }

            toolGroups.Checked = !toolGroups.Checked;
            if (toolGroups.Checked == true)
            {
                this.groupSelect.Show(this);
            }
            else
            {
                this.groupSelect.Hide();
            }
        }

        void GroupPicker_AddedGroup(object sender, GroupPickerEventArgs e)
        {
            if (loading == false)
            {
                PicContext.Current.PictureManager.AddToSecurityGroup(
                    this.picture.Id,
                    e.GroupID);

                if (this.editor != null)
                {
                    PersonGroup group = PicContext.Current.GroupManager.GetGroup(e.GroupID);
                    this.editor.Editor.AddGroup(group);
                }
            }
        }

        void GroupPicker_RemovedGroup(object sender, GroupPickerEventArgs e)
        {
            if (loading == false)
            {
                PicContext.Current.PictureManager.RemoveFromSecurityGroup(
                    this.picture.Id,
                    e.GroupID);

                if (this.editor != null)
                {
                    PersonGroup group = PicContext.Current.GroupManager.GetGroup(e.GroupID);
                    this.editor.Editor.RemoveGroup(group);
                }
            }
        }

        void groupSelect_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.toolGroups.Checked = false;
            this.groupSelect = null;
        }

        #endregion

        #region Categories

        private void toolCategories_Click(object sender, EventArgs e)
        {
            if (this.categorySelect == null)
            {
                this.categorySelect = new CategorySelect();
                this.categorySelect.Opacity = 0.75f;
                this.categorySelect.Left = PictureControlSettings.GetSafeLeft(this.categorySelect, this.settings.Slideshow_Category_Left);
                this.categorySelect.Top = PictureControlSettings.GetSafeTop(this.categorySelect, this.settings.Slideshow_Category_Top);
                this.categorySelect.FormClosed += new FormClosedEventHandler(categorySelect_FormClosed);
                this.categorySelect.CategoryPicker.AddedCategory += new AddedCategoryEventHandler(CategoryPicker_AddedCategory);
                this.categorySelect.CategoryPicker.RemovedCategory += new RemovedCategoryEventHandler(CategoryPicker_RemovedCategory);

                if (this.picture != null)
                {
                    LoadCategories();
                }
            }

            toolCategories.Checked = !toolCategories.Checked;
            if (toolCategories.Checked == true)
            {
                this.categorySelect.Show(this);
            }
            else
            {
                this.categorySelect.Hide();
            }
        }

        void CategoryPicker_AddedCategory(object sender, CategoryPickerEventArgs e)
        {
            if (loading == false)
            {
                PicContext.Current.PictureManager.AddToCategory(
                    this.picture.Id,
                    e.Category.CategoryId);

                if (this.editor != null)
                {
                    this.editor.Editor.AddCategory(e.Category);
                }
            }
        }

        void CategoryPicker_RemovedCategory(object sender, CategoryPickerEventArgs e)
        {
            if (loading == false)
            {
                PicContext.Current.PictureManager.RemoveFromCategory(
                    this.picture.Id,
                    e.Category.CategoryId);

                if (this.editor != null)
                {
                    this.editor.Editor.RemoveCategory(e.Category);
                }
            }
        }

        void categorySelect_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.toolCategories.Checked = false;
            this.categorySelect = null;
        }

        #endregion

    }

    public delegate PictureData GetNextItemIdDelegate(int currentItem);
    public delegate PictureData GetPreviousItemIdDelegate(int currentItem);
}