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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolProperties = new System.Windows.Forms.ToolStripButton();
            this.toolAddToCategory = new System.Windows.Forms.ToolStripButton();
            this.toolPeople = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolClose,
            this.toolStripSeparator1,
            this.toolPrevious,
            this.toolNext,
            this.toolStripSeparator2,
            this.toolProperties,
            this.toolPeople,
            this.toolAddToCategory});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(703, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolClose
            // 
            this.toolClose.Image = msn2.net.Pictures.Controls.Properties.Resources.xp_close;
            this.toolClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolClose.Name = "toolClose";
            this.toolClose.Text = "Close";
            this.toolClose.Click += new System.EventHandler(this.toolClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolPrevious
            // 
            this.toolPrevious.Image = ((System.Drawing.Image)(resources.GetObject("toolPrevious.Image")));
            this.toolPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPrevious.Name = "toolPrevious";
            this.toolPrevious.Text = "Previous";
            this.toolPrevious.Click += new System.EventHandler(this.toolPrevious_Click);
            // 
            // toolNext
            // 
            this.toolNext.Image = ((System.Drawing.Image)(resources.GetObject("toolNext.Image")));
            this.toolNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNext.Name = "toolNext";
            this.toolNext.Text = "Next";
            this.toolNext.Click += new System.EventHandler(this.toolNext_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // toolProperties
            // 
            this.toolProperties.Image = ((System.Drawing.Image)(resources.GetObject("toolProperties.Image")));
            this.toolProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolProperties.Name = "toolProperties";
            this.toolProperties.Text = "Properties";
            this.toolProperties.Click += new System.EventHandler(this.toolProperties_Click);
            // 
            // toolAddToCategory
            // 
            this.toolAddToCategory.Image = ((System.Drawing.Image)(resources.GetObject("toolAddToCategory.Image")));
            this.toolAddToCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAddToCategory.Name = "toolAddToCategory";
            this.toolAddToCategory.Text = "Add to category...";
            this.toolAddToCategory.Click += new System.EventHandler(this.toolAddToCategory_Click);
            // 
            // toolPeople
            // 
            this.toolPeople.Image = ((System.Drawing.Image)(resources.GetObject("toolPeople.Image")));
            this.toolPeople.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPeople.Name = "toolPeople";
            this.toolPeople.Text = "People";
            this.toolPeople.Click += new System.EventHandler(this.toolPeople_Click);
            // 
            // Slideshow
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(703, 606);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Slideshow";
            this.Text = "Slideshow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Slideshow_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Slideshow_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolPrevious;
        private System.Windows.Forms.ToolStripButton toolNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolProperties;
        private System.Windows.Forms.ToolStripButton toolAddToCategory;
        private System.Windows.Forms.ToolStripButton toolPeople;

    }
}