namespace msn2.net.Pictures.Controls
{
    partial class MultiPictureEdit
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.labelDateTaken = new System.Windows.Forms.Label();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.differentCategoriesLabel = new System.Windows.Forms.Label();
            this.groupLabel = new System.Windows.Forms.Label();
            this.peopleLabel = new System.Windows.Forms.Label();
            this.personList = new msn2.net.Pictures.Controls.PersonLinkList();
            this.differentPeopleLabel = new System.Windows.Forms.Label();
            this.groups = new msn2.net.Pictures.Controls.GroupLinkList();
            this.differentGroupsLabel = new System.Windows.Forms.Label();
            this.categoryList = new msn2.net.Pictures.Controls.CategoryLinkList();
            this.dateTaken = new msn2.net.Pictures.Controls.UserControls.DateTimePicker();
            this.description = new msn2.net.Pictures.Controls.UserControls.MultiItemTextBox();
            this.title = new msn2.net.Pictures.Controls.UserControls.MultiItemTextBox();
            this.personList.SuspendLayout();
            this.groups.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(4, 4);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(26, 13);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Title:";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(4, 46);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(59, 13);
            this.descriptionLabel.TabIndex = 2;
            this.descriptionLabel.Text = "Description:";
            // 
            // labelDateTaken
            // 
            this.labelDateTaken.AutoSize = true;
            this.labelDateTaken.Location = new System.Drawing.Point(4, 25);
            this.labelDateTaken.Name = "labelDateTaken";
            this.labelDateTaken.Size = new System.Drawing.Size(63, 13);
            this.labelDateTaken.TabIndex = 5;
            this.labelDateTaken.Text = "Date Taken:";
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Location = new System.Drawing.Point(5, 124);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(56, 13);
            this.categoryLabel.TabIndex = 7;
            this.categoryLabel.Text = "Categories:";
            // 
            // differentCategoriesLabel
            // 
            this.differentCategoriesLabel.AutoSize = true;
            this.differentCategoriesLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.differentCategoriesLabel.Location = new System.Drawing.Point(71, 117);
            this.differentCategoriesLabel.Name = "differentCategoriesLabel";
            this.differentCategoriesLabel.Size = new System.Drawing.Size(168, 13);
            this.differentCategoriesLabel.TabIndex = 9;
            this.differentCategoriesLabel.Text = "<pictures are in differnt categories>";
            this.differentCategoriesLabel.Visible = false;
            // 
            // groupLabel
            // 
            this.groupLabel.AutoSize = true;
            this.groupLabel.Location = new System.Drawing.Point(5, 149);
            this.groupLabel.Name = "groupLabel";
            this.groupLabel.Size = new System.Drawing.Size(62, 13);
            this.groupLabel.TabIndex = 9;
            this.groupLabel.Text = "Shared with:";
            // 
            // peopleLabel
            // 
            this.peopleLabel.AutoSize = true;
            this.peopleLabel.Location = new System.Drawing.Point(5, 94);
            this.peopleLabel.Name = "peopleLabel";
            this.peopleLabel.Size = new System.Drawing.Size(39, 13);
            this.peopleLabel.TabIndex = 11;
            this.peopleLabel.Text = "People:";
            // 
            // personList
            // 
            this.personList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.personList.Controls.Add(this.differentPeopleLabel);
            this.personList.Location = new System.Drawing.Point(72, 90);
            this.personList.Name = "personList";
            this.personList.Size = new System.Drawing.Size(313, 25);
            this.personList.TabIndex = 12;
            // 
            // differentPeopleLabel
            // 
            this.differentPeopleLabel.AutoSize = true;
            this.differentPeopleLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.differentPeopleLabel.Location = new System.Drawing.Point(3, 0);
            this.differentPeopleLabel.Name = "differentPeopleLabel";
            this.differentPeopleLabel.Size = new System.Drawing.Size(165, 13);
            this.differentPeopleLabel.TabIndex = 10;
            this.differentPeopleLabel.Text = "<pictures include different people>";
            this.differentPeopleLabel.Visible = false;
            // 
            // groups
            // 
            this.groups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groups.Controls.Add(this.differentGroupsLabel);
            this.groups.Location = new System.Drawing.Point(72, 145);
            this.groups.Name = "groups";
            this.groups.Size = new System.Drawing.Size(313, 17);
            this.groups.TabIndex = 10;
            // 
            // differentGroupsLabel
            // 
            this.differentGroupsLabel.AutoSize = true;
            this.differentGroupsLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.differentGroupsLabel.Location = new System.Drawing.Point(3, 0);
            this.differentGroupsLabel.Name = "differentGroupsLabel";
            this.differentGroupsLabel.Size = new System.Drawing.Size(203, 13);
            this.differentGroupsLabel.TabIndex = 10;
            this.differentGroupsLabel.Text = "<pictures are shared with different people>";
            this.differentGroupsLabel.Visible = false;
            // 
            // categoryList
            // 
            this.categoryList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryList.Location = new System.Drawing.Point(72, 121);
            this.categoryList.Name = "categoryList";
            this.categoryList.Size = new System.Drawing.Size(313, 17);
            this.categoryList.TabIndex = 8;
            // 
            // dateTaken
            // 
            this.dateTaken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTaken.Location = new System.Drawing.Point(82, 25);
            this.dateTaken.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.dateTaken.Name = "dateTaken";
            this.dateTaken.Size = new System.Drawing.Size(306, 20);
            this.dateTaken.TabIndex = 6;
            this.dateTaken.DateTimeItemChanged += new msn2.net.Pictures.Controls.UserControls.DateTimePicker.DateTimeItemChangedEventHandler(this.dateTaken_DateTimeItemChanged);
            // 
            // description
            // 
            this.description.AcceptsReturn = true;
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.description.DisplayMultipleItems = false;
            this.description.Location = new System.Drawing.Point(82, 46);
            this.description.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.description.MultiLine = true;
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(306, 38);
            this.description.TabIndex = 4;
            this.description.StringItemChanged += new msn2.net.Pictures.Controls.UserControls.MultiItemTextBox.StringItemChangedEventHandler(this.description_StringItemChanged);
            // 
            // title
            // 
            this.title.AcceptsReturn = false;
            this.title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.title.DisplayMultipleItems = true;
            this.title.Location = new System.Drawing.Point(82, 4);
            this.title.MultiLine = false;
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(306, 14);
            this.title.TabIndex = 3;
            this.title.StringItemChanged += new msn2.net.Pictures.Controls.UserControls.MultiItemTextBox.StringItemChangedEventHandler(this.title_StringItemChanged);
            // 
            // MultiPictureEdit
            // 
            this.Controls.Add(this.personList);
            this.Controls.Add(this.peopleLabel);
            this.Controls.Add(this.differentCategoriesLabel);
            this.Controls.Add(this.groups);
            this.Controls.Add(this.groupLabel);
            this.Controls.Add(this.categoryList);
            this.Controls.Add(this.categoryLabel);
            this.Controls.Add(this.dateTaken);
            this.Controls.Add(this.labelDateTaken);
            this.Controls.Add(this.description);
            this.Controls.Add(this.title);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "MultiPictureEdit";
            this.Size = new System.Drawing.Size(391, 191);
            this.personList.ResumeLayout(false);
            this.personList.PerformLayout();
            this.groups.ResumeLayout(false);
            this.groups.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label labelDateTaken;
        private msn2.net.Pictures.Controls.UserControls.MultiItemTextBox description;
        private msn2.net.Pictures.Controls.UserControls.MultiItemTextBox title;
        private msn2.net.Pictures.Controls.UserControls.DateTimePicker dateTaken;
        private System.Windows.Forms.Label categoryLabel;
        private CategoryLinkList categoryList;
        private System.Windows.Forms.Label differentCategoriesLabel;
        private System.Windows.Forms.Label groupLabel;
        private GroupLinkList groups;
        private System.Windows.Forms.Label differentGroupsLabel;
        private PersonLinkList personList;
        private System.Windows.Forms.Label differentPeopleLabel;
        private System.Windows.Forms.Label peopleLabel;
    }
}
