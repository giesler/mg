namespace msn2.net.Pictures.Controls
{
    partial class Slideshow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Slideshow));
            this.toolStip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.averageLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolClose = new System.Windows.Forms.ToolStripButton();
            this.toolPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolNext = new System.Windows.Forms.ToolStripButton();
            this.toolProperties = new System.Windows.Forms.ToolStripButton();
            this.toolPeople = new System.Windows.Forms.ToolStripButton();
            this.toolCategories = new System.Windows.Forms.ToolStripButton();
            this.toolAddToCategory = new System.Windows.Forms.ToolStripButton();
            this.toolGroups = new System.Windows.Forms.ToolStripButton();
            this.star1 = new System.Windows.Forms.ToolStripButton();
            this.star2 = new System.Windows.Forms.ToolStripButton();
            this.star3 = new System.Windows.Forms.ToolStripButton();
            this.star4 = new System.Windows.Forms.ToolStripButton();
            this.star5 = new System.Windows.Forms.ToolStripButton();
            this.openImage = new System.Windows.Forms.ToolStripButton();
            this.toolStip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStip
            // 
            this.toolStip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolClose,
            this.toolStripSeparator1,
            this.toolPrevious,
            this.toolNext,
            this.toolStripSeparator2,
            this.toolProperties,
            this.toolPeople,
            this.toolCategories,
            this.toolAddToCategory,
            this.toolGroups,
            this.toolStripSeparator3,
            this.star1,
            this.star2,
            this.star3,
            this.star4,
            this.star5,
            this.averageLabel,
            this.toolStripSeparator4,
            this.openImage});
            this.toolStip.Location = new System.Drawing.Point(0, 0);
            this.toolStip.Name = "toolStip";
            this.toolStip.Size = new System.Drawing.Size(988, 25);
            this.toolStip.TabIndex = 0;
            this.toolStip.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // averageLabel
            // 
            this.averageLabel.Name = "averageLabel";
            this.averageLabel.Size = new System.Drawing.Size(46, 22);
            this.averageLabel.Text = "Avg 3.0";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolClose
            // 
            this.toolClose.Image = global::msn2.net.Pictures.Controls.Properties.Resources.xp_close;
            this.toolClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolClose.Name = "toolClose";
            this.toolClose.Size = new System.Drawing.Size(56, 22);
            this.toolClose.Text = "Close";
            this.toolClose.Click += new System.EventHandler(this.toolClose_Click);
            // 
            // toolPrevious
            // 
            this.toolPrevious.Image = global::msn2.net.Pictures.Controls.Properties.Resources.up;
            this.toolPrevious.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPrevious.Name = "toolPrevious";
            this.toolPrevious.Size = new System.Drawing.Size(69, 22);
            this.toolPrevious.Text = "Previous";
            this.toolPrevious.Click += new System.EventHandler(this.toolPrevious_Click);
            // 
            // toolNext
            // 
            this.toolNext.Image = global::msn2.net.Pictures.Controls.Properties.Resources.down;
            this.toolNext.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNext.Name = "toolNext";
            this.toolNext.Size = new System.Drawing.Size(48, 22);
            this.toolNext.Text = "Next";
            this.toolNext.Click += new System.EventHandler(this.toolNext_Click);
            // 
            // toolProperties
            // 
            this.toolProperties.Image = global::msn2.net.Pictures.Controls.Properties.Resources.Properties;
            this.toolProperties.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolProperties.ImageTransparentColor = System.Drawing.Color.White;
            this.toolProperties.Name = "toolProperties";
            this.toolProperties.Size = new System.Drawing.Size(80, 22);
            this.toolProperties.Text = "Properties";
            this.toolProperties.Click += new System.EventHandler(this.toolProperties_Click);
            // 
            // toolPeople
            // 
            this.toolPeople.Image = global::msn2.net.Pictures.Controls.Properties.Resources.user;
            this.toolPeople.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolPeople.ImageTransparentColor = System.Drawing.Color.White;
            this.toolPeople.Name = "toolPeople";
            this.toolPeople.Size = new System.Drawing.Size(63, 22);
            this.toolPeople.Text = "People";
            this.toolPeople.Click += new System.EventHandler(this.toolPeople_Click);
            // 
            // toolCategories
            // 
            this.toolCategories.Image = global::msn2.net.Pictures.Controls.Properties.Resources.Folder;
            this.toolCategories.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolCategories.ImageTransparentColor = System.Drawing.Color.White;
            this.toolCategories.Name = "toolCategories";
            this.toolCategories.Size = new System.Drawing.Size(83, 22);
            this.toolCategories.Text = "Categories";
            this.toolCategories.Click += new System.EventHandler(this.toolCategories_Click);
            // 
            // toolAddToCategory
            // 
            this.toolAddToCategory.Image = ((System.Drawing.Image)(resources.GetObject("toolAddToCategory.Image")));
            this.toolAddToCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAddToCategory.Name = "toolAddToCategory";
            this.toolAddToCategory.Size = new System.Drawing.Size(121, 22);
            this.toolAddToCategory.Text = "Add to category...";
            this.toolAddToCategory.Visible = false;
            this.toolAddToCategory.Click += new System.EventHandler(this.toolAddToCategory_Click);
            // 
            // toolGroups
            // 
            this.toolGroups.Image = global::msn2.net.Pictures.Controls.Properties.Resources.lock1;
            this.toolGroups.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolGroups.ImageTransparentColor = System.Drawing.Color.White;
            this.toolGroups.Name = "toolGroups";
            this.toolGroups.Size = new System.Drawing.Size(91, 22);
            this.toolGroups.Text = "Shared With";
            this.toolGroups.Click += new System.EventHandler(this.toolGroups_Click);
            // 
            // star1
            // 
            this.star1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.star1.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            this.star1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.star1.Name = "star1";
            this.star1.Size = new System.Drawing.Size(23, 22);
            this.star1.Text = "1 star";
            this.star1.MouseLeave += new System.EventHandler(this.star1_MouseLeave);
            this.star1.MouseEnter += new System.EventHandler(this.star1_MouseEnter);
            this.star1.Click += new System.EventHandler(this.star1_Click);
            // 
            // star2
            // 
            this.star2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.star2.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            this.star2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.star2.Name = "star2";
            this.star2.Size = new System.Drawing.Size(23, 22);
            this.star2.Text = "2 stars";
            this.star2.ToolTipText = "2 stars";
            this.star2.MouseLeave += new System.EventHandler(this.star2_MouseLeave);
            this.star2.MouseEnter += new System.EventHandler(this.star2_MouseEnter);
            this.star2.Click += new System.EventHandler(this.star2_Click);
            // 
            // star3
            // 
            this.star3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.star3.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            this.star3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.star3.Name = "star3";
            this.star3.Size = new System.Drawing.Size(23, 22);
            this.star3.Text = "3 stars";
            this.star3.MouseLeave += new System.EventHandler(this.star3_MouseLeave);
            this.star3.MouseEnter += new System.EventHandler(this.star3_MouseEnter);
            this.star3.Click += new System.EventHandler(this.star3_Click);
            // 
            // star4
            // 
            this.star4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.star4.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            this.star4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.star4.Name = "star4";
            this.star4.Size = new System.Drawing.Size(23, 22);
            this.star4.Text = "4 stars";
            this.star4.ToolTipText = "4 stars";
            this.star4.MouseLeave += new System.EventHandler(this.star4_MouseLeave);
            this.star4.MouseEnter += new System.EventHandler(this.star4_MouseEnter);
            this.star4.Click += new System.EventHandler(this.star4_Click);
            // 
            // star5
            // 
            this.star5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.star5.Image = global::msn2.net.Pictures.Controls.Properties.Resources.starf;
            this.star5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.star5.Name = "star5";
            this.star5.Size = new System.Drawing.Size(23, 22);
            this.star5.Text = "5 stars";
            this.star5.MouseLeave += new System.EventHandler(this.star5_MouseLeave);
            this.star5.MouseEnter += new System.EventHandler(this.star5_MouseEnter);
            this.star5.Click += new System.EventHandler(this.star5_Click);
            // 
            // openImage
            // 
            this.openImage.Image = global::msn2.net.Pictures.Controls.Properties.Resources.openfolderHS;
            this.openImage.ImageTransparentColor = System.Drawing.Color.Black;
            this.openImage.Name = "openImage";
            this.openImage.Size = new System.Drawing.Size(92, 22);
            this.openImage.Text = "Open Image";
            this.openImage.Click += new System.EventHandler(this.openImage_Click);
            // 
            // Slideshow
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.toolStip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Slideshow";
            this.Text = "Slideshow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.toolStip.ResumeLayout(false);
            this.toolStip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        protected System.Windows.Forms.ToolStrip toolStip;
        private System.Windows.Forms.ToolStripButton toolClose;
        private System.Windows.Forms.ToolStripButton toolPrevious;
        private System.Windows.Forms.ToolStripButton toolNext;
        private System.Windows.Forms.ToolStripButton toolProperties;
        private System.Windows.Forms.ToolStripButton toolPeople;
        private System.Windows.Forms.ToolStripButton toolCategories;
        private System.Windows.Forms.ToolStripButton toolAddToCategory;
        private System.Windows.Forms.ToolStripButton toolGroups;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton star1;
        private System.Windows.Forms.ToolStripButton star2;
        private System.Windows.Forms.ToolStripButton star3;
        private System.Windows.Forms.ToolStripButton star4;
        private System.Windows.Forms.ToolStripButton star5;
        private System.Windows.Forms.ToolStripLabel averageLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton openImage;

    }
}