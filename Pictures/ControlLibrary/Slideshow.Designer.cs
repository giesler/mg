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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.closeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.sepToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.previousPictureToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.nextPictureToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.sepToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.addtocategoryToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
// 
// toolStrip1
// 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripButton,
            this.sepToolStripSeparator,
            this.previousPictureToolStripButton,
            this.nextPictureToolStripButton,
            this.sepToolStripSeparator1,
            this.propertiesToolStripButton,
            this.addtocategoryToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
// 
// closeToolStripButton
// 
            this.closeToolStripButton.Name = "closeToolStripButton";
            this.closeToolStripButton.SettingsKey = "Slideshow.closeToolStripButton";
            this.closeToolStripButton.Text = "&Close";
            this.closeToolStripButton.Click += new System.EventHandler(this.closeToolStripButton_Click_1);
// 
// sepToolStripSeparator
// 
            this.sepToolStripSeparator.Name = "sepToolStripSeparator";
            this.sepToolStripSeparator.SettingsKey = "Slideshow.sepToolStripSeparator";
// 
// previousPictureToolStripButton
// 
            this.previousPictureToolStripButton.Name = "previousPictureToolStripButton";
            this.previousPictureToolStripButton.SettingsKey = "Slideshow.previousPictureToolStripButton";
            this.previousPictureToolStripButton.Text = "&Previous Picture";
            this.previousPictureToolStripButton.Click += new System.EventHandler(this.previousPictureToolStripButton_Click);
// 
// nextPictureToolStripButton
// 
            this.nextPictureToolStripButton.Name = "nextPictureToolStripButton";
            this.nextPictureToolStripButton.SettingsKey = "Slideshow.nextPictureToolStripButton";
            this.nextPictureToolStripButton.Text = "&Next Picture";
            this.nextPictureToolStripButton.Click += new System.EventHandler(this.nextPictureToolStripButton_Click);
// 
// sepToolStripSeparator1
// 
            this.sepToolStripSeparator1.Name = "sepToolStripSeparator1";
            this.sepToolStripSeparator1.SettingsKey = "Slideshow.sepToolStripSeparator1";
// 
// propertiesToolStripButton
// 
            this.propertiesToolStripButton.Name = "propertiesToolStripButton";
            this.propertiesToolStripButton.SettingsKey = "Slideshow.propertiesToolStripButton";
            this.propertiesToolStripButton.Text = "Properties";
            this.propertiesToolStripButton.Click += new System.EventHandler(this.propertiesToolStripButton_Click);
// 
// addtocategoryToolStripButton
// 
            this.addtocategoryToolStripButton.Name = "addtocategoryToolStripButton";
            this.addtocategoryToolStripButton.SettingsKey = "Slideshow.addtocategoryToolStripButton";
            this.addtocategoryToolStripButton.Text = "&Add to category...";
            this.addtocategoryToolStripButton.Click += new System.EventHandler(this.addtocategoryToolStripButton_Click);
// 
// Slideshow
// 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(703, 606);
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
        private System.Windows.Forms.ToolStripButton closeToolStripButton;
        private System.Windows.Forms.ToolStripSeparator sepToolStripSeparator;
        private System.Windows.Forms.ToolStripButton previousPictureToolStripButton;
        private System.Windows.Forms.ToolStripButton nextPictureToolStripButton;
        private System.Windows.Forms.ToolStripSeparator sepToolStripSeparator1;
        private System.Windows.Forms.ToolStripButton propertiesToolStripButton;
        private System.Windows.Forms.ToolStripButton addtocategoryToolStripButton;
    }
}