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
            this.toolClose = new System.Windows.Forms.ToolStripButton();
            this.toolPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolNext = new System.Windows.Forms.ToolStripButton();
            this.toolProperties = new System.Windows.Forms.ToolStripButton();
            this.toolPeople = new System.Windows.Forms.ToolStripButton();
            this.toolCategories = new System.Windows.Forms.ToolStripButton();
            this.toolAddToCategory = new System.Windows.Forms.ToolStripButton();
            this.toolGroups = new System.Windows.Forms.ToolStripButton();
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
            this.toolGroups});
            this.toolStip.Location = new System.Drawing.Point(0, 0);
            this.toolStip.Name = "toolStip";
            this.toolStip.Size = new System.Drawing.Size(703, 25);
            this.toolStip.TabIndex = 0;
            this.toolStip.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // toolClose
            // 
            this.toolClose.Image = msn2.net.Pictures.Controls.Properties.Resources.xp_close;
            this.toolClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolClose.Name = "toolClose";
            this.toolClose.Text = "Close";
            this.toolClose.Click += new System.EventHandler(this.toolClose_Click);
            // 
            // toolPrevious
            // 
            this.toolPrevious.Image = msn2.net.Pictures.Controls.Properties.Resources.up;
            this.toolPrevious.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPrevious.Name = "toolPrevious";
            this.toolPrevious.Text = "Previous";
            this.toolPrevious.Click += new System.EventHandler(this.toolPrevious_Click);
            // 
            // toolNext
            // 
            this.toolNext.Image = msn2.net.Pictures.Controls.Properties.Resources.down;
            this.toolNext.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNext.Name = "toolNext";
            this.toolNext.Text = "Next";
            this.toolNext.Click += new System.EventHandler(this.toolNext_Click);
            // 
            // toolProperties
            // 
            this.toolProperties.Image = msn2.net.Pictures.Controls.Properties.Resources.Properties;
            this.toolProperties.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolProperties.ImageTransparentColor = System.Drawing.Color.White;
            this.toolProperties.Name = "toolProperties";
            this.toolProperties.Text = "Properties";
            this.toolProperties.Click += new System.EventHandler(this.toolProperties_Click);
            // 
            // toolPeople
            // 
            this.toolPeople.Image = msn2.net.Pictures.Controls.Properties.Resources.user;
            this.toolPeople.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolPeople.ImageTransparentColor = System.Drawing.Color.White;
            this.toolPeople.Name = "toolPeople";
            this.toolPeople.Text = "People";
            this.toolPeople.Click += new System.EventHandler(this.toolPeople_Click);
            // 
            // toolCategories
            // 
            this.toolCategories.Image = msn2.net.Pictures.Controls.Properties.Resources.Folder;
            this.toolCategories.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolCategories.ImageTransparentColor = System.Drawing.Color.White;
            this.toolCategories.Name = "toolCategories";
            this.toolCategories.Text = "Categories";
            this.toolCategories.Click += new System.EventHandler(this.toolCategories_Click);
            // 
            // toolAddToCategory
            // 
            this.toolAddToCategory.Image = ((System.Drawing.Image)(resources.GetObject("toolAddToCategory.Image")));
            this.toolAddToCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAddToCategory.Name = "toolAddToCategory";
            this.toolAddToCategory.Text = "Add to category...";
            this.toolAddToCategory.Visible = false;
            this.toolAddToCategory.Click += new System.EventHandler(this.toolAddToCategory_Click);
            // 
            // toolGroups
            // 
            this.toolGroups.Image = msn2.net.Pictures.Controls.Properties.Resources.lock1;
            this.toolGroups.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolGroups.ImageTransparentColor = System.Drawing.Color.White;
            this.toolGroups.Name = "toolGroups";
            this.toolGroups.Text = "Shared With";
            this.toolGroups.Click += new System.EventHandler(this.toolGroups_Click);
            // 
            // Slideshow
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(703, 606);
            this.Controls.Add(this.toolStip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Slideshow";
            this.Text = "Slideshow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Slideshow_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Slideshow_KeyDown);
            this.toolStip.ResumeLayout(false);
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

    }
}