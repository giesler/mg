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
            this.categoryList = new msn2.net.Pictures.Controls.CategoryFlowLayoutPanel();
            this.dateTaken = new msn2.net.Pictures.Controls.UserControls.DateTimePicker();
            this.description = new msn2.net.Pictures.Controls.UserControls.MultiItemTextBox();
            this.title = new msn2.net.Pictures.Controls.UserControls.MultiItemTextBox();
            this.differentCategoriesLabel = new System.Windows.Forms.Label();
            this.categoryList.SuspendLayout();
            this.SuspendLayout();
// 
// titleLabel
// 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(4, 4);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(29, 14);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Title:";
// 
// descriptionLabel
// 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(4, 46);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(64, 14);
            this.descriptionLabel.TabIndex = 2;
            this.descriptionLabel.Text = "Description:";
// 
// labelDateTaken
// 
            this.labelDateTaken.AutoSize = true;
            this.labelDateTaken.Location = new System.Drawing.Point(4, 25);
            this.labelDateTaken.Name = "labelDateTaken";
            this.labelDateTaken.Size = new System.Drawing.Size(66, 14);
            this.labelDateTaken.TabIndex = 5;
            this.labelDateTaken.Text = "Date Taken:";
// 
// categoryLabel
// 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Location = new System.Drawing.Point(8, 94);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(62, 14);
            this.categoryLabel.TabIndex = 7;
            this.categoryLabel.Text = "Categories:";
// 
// categoryList
// 
            this.categoryList.Controls.Add(this.differentCategoriesLabel);
            this.categoryList.Location = new System.Drawing.Point(75, 91);
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
// differentCategoriesLabel
// 
            this.differentCategoriesLabel.AutoSize = true;
            this.differentCategoriesLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.differentCategoriesLabel.Location = new System.Drawing.Point(3, 3);
            this.differentCategoriesLabel.Name = "differentCategoriesLabel";
            this.differentCategoriesLabel.Size = new System.Drawing.Size(181, 14);
            this.differentCategoriesLabel.TabIndex = 9;
            this.differentCategoriesLabel.Text = "<pictures are in differnt categories>";
            this.differentCategoriesLabel.Visible = false;
// 
// MultiPictureEdit
// 
            this.Controls.Add(this.categoryList);
            this.Controls.Add(this.categoryLabel);
            this.Controls.Add(this.dateTaken);
            this.Controls.Add(this.labelDateTaken);
            this.Controls.Add(this.description);
            this.Controls.Add(this.title);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "MultiPictureEdit";
            this.Size = new System.Drawing.Size(391, 112);
            this.categoryList.ResumeLayout(false);
            this.categoryList.PerformLayout();
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
        private CategoryFlowLayoutPanel categoryList;
        private System.Windows.Forms.Label differentCategoriesLabel;
    }
}
