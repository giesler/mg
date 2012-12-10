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
using System.Threading;
using Microsoft.Win32;

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
            : this(new PictureControlSettings(), null, null)
        {
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

            item = new PictureItem(picture);
            item.DrawShadow = false;
            item.DrawBorder = false;
            item.PaintBackground = false;
            item.PaintFullControlArea = true;
            item.Dock = DockStyle.Fill;
            item.Padding = new Padding(0);
            this.Controls.Add(item);

            this.toolStip.ImageScalingSize = new Size(32, 32);
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
            this.SetPicture(picture.Id);
        }

        public void SetPicture(int id)
        {
            this.picture = PicContext.Current.PictureManager.GetPicture(id);

            this.item.SetPicture(picture);

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

            this.DisplayStarRating(this.picture.UserRating);
            this.DisplayAverageRating();

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
            List<PersonGroupInfo> groups = PicContext.Current.PictureManager.GetPictureGroups(this.picture.Id);

            this.loading = true;
            groupSelect.GroupPicker.ClearSelectedGroups();
            foreach (PersonGroupInfo group in groups)
            {
                groupSelect.GroupPicker.AddSelectedGroup(group.Id);
            }
            this.loading = false;
        }

        private void LoadCategories()
        {
            List<CategoryInfo> categories = PicContext.Current.PictureManager.GetPictureCategories(this.picture.Id);

            this.loading = true;
            categorySelect.CategoryPicker.ClearSelectedCategories();
            foreach (CategoryInfo category in categories)
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

            if (settings.Slideshow_Category_IsOpen == true)
            {
                toolCategories_Click(this, EventArgs.Empty);
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
                    this.settings.Slideshow_Editor_Height = this.editor.Height;
                    this.settings.Slideshow_Editor_Width = this.editor.Width;
                    this.editor.Close();
                }

                if (this.peopleSelect != null)
                {
                    this.settings.Slideshow_People_IsOpen = this.peopleSelect.Visible;
                    this.settings.Slideshow_People_Left = this.peopleSelect.Left;
                    this.settings.Slideshow_People_Top = this.peopleSelect.Top;
                    this.settings.Slideshow_People_Height = this.peopleSelect.Height;
                    this.settings.Slideshow_People_Width = this.peopleSelect.Width;
                    this.peopleSelect.Close();
                }

                if (this.groupSelect != null)
                {
                    this.settings.Slideshow_Group_IsOpen = this.groupSelect.Visible;
                    this.settings.Slideshow_Group_Left = this.groupSelect.Left;
                    this.settings.Slideshow_Group_Top = this.groupSelect.Top;
                    this.settings.Slideshow_Group_Width = this.groupSelect.Width;
                    this.settings.Slideshow_Group_Height = this.groupSelect.Height;
                    this.groupSelect.Close();
                }

                if (this.categorySelect != null)
                {
                    this.settings.Slideshow_Category_IsOpen = this.categorySelect.Visible;
                    this.settings.Slideshow_Category_Left = this.categorySelect.Left;
                    this.settings.Slideshow_Category_Top = this.categorySelect.Top;
                    this.settings.Slideshow_Category_Width = this.settings.Slideshow_Category_Width;
                    this.settings.Slideshow_Category_Height = this.settings.Slideshow_Category_Height;
                    this.categorySelect.Close();
                }

                if (this.sourceForm != null)
                {
                    this.sourceForm.Focus();
                }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            Trace.WriteLine("OnKeyPress: " + e.KeyChar.ToString());
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Up)
            {
                this.Previous();
                return false;
            }
            else if (keyData == Keys.Right || keyData == Keys.Down)
            {
                this.Next();
                return false;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override bool IsInputKey(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Left:
                    return true;
            }

            return base.IsInputKey(key);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Trace.WriteLine("OnKeyDown: " + e.KeyCode.ToString());
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                this.Close();
            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.PageUp)
            {
                this.Previous();
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down || e.KeyCode == Keys.PageDown)
            {
                this.Next();
            }
            else if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
            {
                this.SaveStarRating(1);
            }
            else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
            {
                this.SaveStarRating(2);
            }
            else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
            {
                this.SaveStarRating(3);
            }
            else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
            {
                this.SaveStarRating(4);
            }
            else if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
            {
                this.SaveStarRating(5);
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        private void UpdateControls()
        {
            toolPrevious.Enabled = false;
            toolNext.Enabled = false;
            toolPrevious.Image = global::msn2.net.Pictures.Controls.Properties.Resources.up;
            toolNext.Image = global::msn2.net.Pictures.Controls.Properties.Resources.down;

            if (null != getPreviousId)
            {
                PictureData previousItem = getPreviousId(item.PictureId);

                if (previousItem != null)
                {
                    toolPrevious.Enabled = true;

                    if (this.toolPrevious.Visible == true)
                    {
                        using (Image previousImage = PicContext.Current.PictureCache.GetImage(
                            previousItem,
                            32,
                            32))
                        {
                            Bitmap bmp = new Bitmap(previousImage, new Size(32, 32));
                            toolPrevious.Image = bmp;
                        }
                    }
                }
            }

            if (null != getNextId)
            {
                PictureData nextItem = getNextId(this.picture.Id);

                if (nextItem != null)
                {
                    toolNext.Enabled = true;

                    if (this.toolNext.Visible == true)
                    {
                        using (Image nextImage = PicContext.Current.PictureCache.GetImage(
                                nextItem,
                                32,
                                32))
                        {
                            Bitmap bmp = new Bitmap(nextImage, new Size(32, 32));
                            toolNext.Image = bmp;
                        }
                    }
                }
            }
        }

        private void toolClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolPrevious_Click(object sender, EventArgs e)
        {
            this.Previous();
        }

        private void toolNext_Click(object sender, EventArgs e)
        {
            this.Next();
        }

        protected void Next()
        {
            PictureData picture = getNextId(item.PictureId);
            if (picture != null)
            {
                SetPicture(picture);
            }
            UpdateControls();
        }

        protected void Previous()
        {
            PictureData picture = getPreviousId(item.PictureId);
            if (picture != null)
            {
                SetPicture(picture);
            }
            UpdateControls();
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
                this.editor.Left = PictureControlSettings.GetSafeLeft(this.editor, this.settings.Slideshow_Editor_Left);
                this.editor.Top = PictureControlSettings.GetSafeTop(this.editor, this.settings.Slideshow_Editor_Top);
                this.editor.Width = this.settings.Slideshow_Editor_Width;
                this.editor.Height = this.settings.Slideshow_Editor_Height;

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
                this.peopleSelect.Left = PictureControlSettings.GetSafeLeft(this.peopleSelect, this.settings.Slideshow_People_Left);
                this.peopleSelect.Top = PictureControlSettings.GetSafeTop(this.peopleSelect, this.settings.Slideshow_People_Top);
                this.peopleSelect.Width = this.settings.Slideshow_People_Width;
                this.peopleSelect.Height = this.settings.Slideshow_People_Width;
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
                this.Focus();
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
                this.groupSelect.Left = PictureControlSettings.GetSafeLeft(this.groupSelect, this.settings.Slideshow_Group_Left);
                this.groupSelect.Top = PictureControlSettings.GetSafeTop(this.groupSelect, this.settings.Slideshow_Group_Top);
                this.groupSelect.Height = this.settings.Slideshow_Group_Height;
                this.groupSelect.Width = this.settings.Slideshow_Group_Width;
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
                this.Focus();
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
                    PersonGroupInfo group = PicContext.Current.GroupManager.GetGroup(e.GroupID);
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
                    PersonGroupInfo group = PicContext.Current.GroupManager.GetGroup(e.GroupID);
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
                this.categorySelect.Left = PictureControlSettings.GetSafeLeft(this.categorySelect, this.settings.Slideshow_Category_Left);
                this.categorySelect.Top = PictureControlSettings.GetSafeTop(this.categorySelect, this.settings.Slideshow_Category_Top);
                this.categorySelect.Width = this.settings.Slideshow_Category_Width;
                this.categorySelect.Height = this.settings.Slideshow_Category_Height;
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
                this.Focus();
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

        protected ToolStrip ToolStip
        {
            get
            {
                return this.toolStip;
            }
        }

        protected PictureItem PictureItem
        {
            get
            {
                return this.item;
            }
        }

        private void star1_Click(object sender, EventArgs e)
        {
            SaveStarRating(1);
        }

        private void star2_Click(object sender, EventArgs e)
        {
            SaveStarRating(2);
        }

        private void star3_Click(object sender, EventArgs e)
        {
            SaveStarRating(3);
        }

        private void star4_Click(object sender, EventArgs e)
        {
            SaveStarRating(4);
        }

        private void star5_Click(object sender, EventArgs e)
        {
            SaveStarRating(5);
        }

        private void star1_MouseEnter(object sender, EventArgs e)
        {
            this.DisplayStarRating(1);
        }

        private void star1_MouseLeave(object sender, EventArgs e)
        {
            this.DisplayCurrentStarRating();
        }

        private void star2_MouseEnter(object sender, EventArgs e)
        {
            this.DisplayStarRating(2);
        }

        private void star2_MouseLeave(object sender, EventArgs e)
        {
            this.DisplayCurrentStarRating();
        }

        private void star3_MouseEnter(object sender, EventArgs e)
        {
            this.DisplayStarRating(3);
        }

        private void star3_MouseLeave(object sender, EventArgs e)
        {
            this.DisplayCurrentStarRating();
        }

        private void star4_MouseEnter(object sender, EventArgs e)
        {
            this.DisplayStarRating(4);
        }

        private void star4_MouseLeave(object sender, EventArgs e)
        {
            this.DisplayCurrentStarRating();
        }

        private void star5_MouseEnter(object sender, EventArgs e)
        {
            this.DisplayStarRating(5);
        }

        private void star5_MouseLeave(object sender, EventArgs e)
        {
            this.DisplayCurrentStarRating();
        }

        private void DisplayCurrentStarRating()
        {
            if (this.picture != null)
            {
                this.DisplayStarRating(this.picture.UserRating);
            }
            else
            {
                this.DisplayStarRating(0);
            }
        }

        private void DisplayStarRating(int rating)
        {
            star1.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            star2.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            star3.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            star4.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            star5.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;

            if (rating > 0)
            {
                star1.Image = global::msn2.net.Pictures.Controls.Properties.Resources.star;

                if (rating > 1)
                {
                    star2.Image = global::msn2.net.Pictures.Controls.Properties.Resources.star;

                    if (rating > 2)
                    {
                        star3.Image = global::msn2.net.Pictures.Controls.Properties.Resources.star;

                        if (rating > 3)
                        {
                            star4.Image = global::msn2.net.Pictures.Controls.Properties.Resources.star;

                            if (rating > 4)
                            {
                                star5.Image = global::msn2.net.Pictures.Controls.Properties.Resources.star;
                            }
                        }
                    }
                }
            }
        }

        private void DisplayAverageRating()
        {
            averageLabel.Visible = false;

            // Only display if user has rated
            if (this.picture != null && this.picture.UserRating > 0)
            {
                averageLabel.Text = string.Format("Avg {0:0.0}", this.picture.AverageRating);
                averageLabel.Visible = true;
            }
        }

        private void ClearHoverState()
        {
            star1.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            star2.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            star3.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            star4.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            star5.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;

            if (this.picture != null)
            {
                DisplayStarRating(this.picture.UserRating);
                DisplayAverageRating();
            }
        }

        private void SaveStarRating(int stars)
        {
            if (this.picture != null)
            {
                PicContext.Current.PictureManager.RatePicture(
                    this.picture.Id,
                    stars);

                this.picture = PicContext.Current.PictureManager.GetPicture(this.picture.Id);

                DisplayStarRating(stars);
                DisplayAverageRating();
            }
        }

        private void openImage_Click(object sender, EventArgs e)
        {
            if (this.picture != null)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(OpenFileThread), this.picture);
            }
        }

        private void OpenFileThread(object oPic)
        {
            PictureData picture = (PictureData)oPic;

            string path = System.IO.Path.Combine(
                PicContext.Current.Config.PictureDirectory,
                picture.Filename);

            if (System.IO.File.Exists(path) == true)
            {
                string editCommand = Registry.GetValue(@"HKEY_CLASSES_ROOT\jpegfile\shell\edit\command", null, "iexplore.exe").ToString();
                
                Process p = new Process();
                if (editCommand.IndexOf("%1") > 0)
                {
                    if (editCommand.IndexOf("rundll32.exe") > 0)
                    {
                        string dll = editCommand.Substring(0, editCommand.IndexOf("rundll32.exe") + 12).Trim();
                        string file = editCommand.Substring(editCommand.IndexOf("rundll32.exe") + 13).Trim().Replace("%1", path);
                        p.StartInfo = new ProcessStartInfo(dll, file);
                        p.StartInfo.UseShellExecute = true;
                    }
                    else
                    {
                        p.StartInfo = new ProcessStartInfo("cmd", "/c " + editCommand.Replace("%1", path));
                        p.StartInfo.UseShellExecute = true;
                    }
                }
                else
                {
                    p.StartInfo = new ProcessStartInfo(editCommand, path);
                }
                p.Start();

                Thread.Sleep(500);
                while (p.HasExited == false)
                {
                    Thread.Sleep(100);

                    if (this.IsDisposed == true)
                    {
                        break;
                    }
                }

                if (this.IsDisposed == false && this.picture != null)
                {
                    ImageUtilities util = new ImageUtilities();
                    util.CreateUpdateCache(picture.Id);
                    this.BeginInvoke(new ReloadImageDelegate(this.ReloadImage), picture);
                }
            }
            else
            {
                MessageBox.Show(
                    "The file '" + path + "' does not exist or you do not have permissions to access it.",
                    "Fiel not found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private delegate void ReloadImageDelegate(PictureData picture);

        private void ReloadImage(PictureData picture)
        {
            if (this.picture != null && picture.Id == this.picture.Id)
            {
                SetPicture(picture);
            }
        }

        public delegate PictureData GetNextItemIdDelegate(int currentItem);
        public delegate PictureData GetPreviousItemIdDelegate(int currentItem);
    }
}