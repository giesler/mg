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
            this.leftRaftingContainer = new System.Windows.Forms.RaftingContainer();
            this.leftRaftingContainer1 = new System.Windows.Forms.RaftingContainer();
            this.topRaftingContainer = new System.Windows.Forms.RaftingContainer();
            this.bottomRaftingContainer = new System.Windows.Forms.RaftingContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.closeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.sepToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.previousPictureToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.nextPictureToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.sepToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.leftRaftingContainer2 = new System.Windows.Forms.RaftingContainer();
            this.leftRaftingContainer3 = new System.Windows.Forms.RaftingContainer();
            this.topRaftingContainer1 = new System.Windows.Forms.RaftingContainer();
            this.bottomRaftingContainer1 = new System.Windows.Forms.RaftingContainer();
            this.addtocategoryToolStripButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.leftRaftingContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftRaftingContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topRaftingContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomRaftingContainer)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftRaftingContainer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftRaftingContainer3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topRaftingContainer1)).BeginInit();
            this.topRaftingContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomRaftingContainer1)).BeginInit();
            this.SuspendLayout();
// 
// leftRaftingContainer
// 
            this.leftRaftingContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftRaftingContainer.Name = "leftRaftingContainer";
// 
// leftRaftingContainer1
// 
            this.leftRaftingContainer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftRaftingContainer1.Name = "leftRaftingContainer1";
// 
// topRaftingContainer
// 
            this.topRaftingContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.topRaftingContainer.Name = "topRaftingContainer";
// 
// bottomRaftingContainer
// 
            this.bottomRaftingContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomRaftingContainer.Name = "bottomRaftingContainer";
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
            this.toolStrip1.Raft = System.Windows.Forms.RaftingSides.Top;
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
// leftRaftingContainer2
// 
            this.leftRaftingContainer2.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftRaftingContainer2.Name = "leftRaftingContainer2";
// 
// leftRaftingContainer3
// 
            this.leftRaftingContainer3.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftRaftingContainer3.Name = "leftRaftingContainer3";
// 
// topRaftingContainer1
// 
            this.topRaftingContainer1.Controls.Add(this.toolStrip1);
            this.topRaftingContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.topRaftingContainer1.Name = "topRaftingContainer1";
// 
// bottomRaftingContainer1
// 
            this.bottomRaftingContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomRaftingContainer1.Name = "bottomRaftingContainer1";
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
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(703, 606);
            this.Controls.Add(this.leftRaftingContainer2);
            this.Controls.Add(this.leftRaftingContainer3);
            this.Controls.Add(this.topRaftingContainer1);
            this.Controls.Add(this.bottomRaftingContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Slideshow";
            this.Text = "Slideshow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Slideshow_KeyPress);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Slideshow_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.leftRaftingContainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftRaftingContainer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topRaftingContainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomRaftingContainer)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftRaftingContainer2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftRaftingContainer3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topRaftingContainer1)).EndInit();
            this.topRaftingContainer1.ResumeLayout(false);
            this.topRaftingContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomRaftingContainer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RaftingContainer leftRaftingContainer;
        private System.Windows.Forms.RaftingContainer leftRaftingContainer1;
        private System.Windows.Forms.RaftingContainer topRaftingContainer;
        private System.Windows.Forms.RaftingContainer bottomRaftingContainer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.RaftingContainer leftRaftingContainer2;
        private System.Windows.Forms.RaftingContainer leftRaftingContainer3;
        private System.Windows.Forms.RaftingContainer topRaftingContainer1;
        private System.Windows.Forms.RaftingContainer bottomRaftingContainer1;
        private System.Windows.Forms.ToolStripButton closeToolStripButton;
        private System.Windows.Forms.ToolStripSeparator sepToolStripSeparator;
        private System.Windows.Forms.ToolStripButton previousPictureToolStripButton;
        private System.Windows.Forms.ToolStripButton nextPictureToolStripButton;
        private System.Windows.Forms.ToolStripSeparator sepToolStripSeparator1;
        private System.Windows.Forms.ToolStripButton propertiesToolStripButton;
        private System.Windows.Forms.ToolStripButton addtocategoryToolStripButton;
    }
}