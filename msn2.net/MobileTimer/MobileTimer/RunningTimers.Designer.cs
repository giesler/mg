namespace MobileTimer
{
    partial class RunningTimers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuAdd = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuUpdateApp = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.debugText = new System.Windows.Forms.TextBox();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuDebug = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuAdd);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuAdd
            // 
            this.menuAdd.Text = "&Add";
            this.menuAdd.Click += new System.EventHandler(this.menuAdd_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.MenuItems.Add(this.menuDebug);
            this.menuItem2.MenuItems.Add(this.menuItem1);
            this.menuItem2.MenuItems.Add(this.menuUpdateApp);
            this.menuItem2.MenuItems.Add(this.menuItem4);
            this.menuItem2.MenuItems.Add(this.menuExit);
            this.menuItem2.Text = "Menu";
            // 
            // menuUpdateApp
            // 
            this.menuUpdateApp.Text = "Update App";
            this.menuUpdateApp.Click += new System.EventHandler(this.menuUpdateApp_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Text = "-";
            // 
            // menuExit
            // 
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // debugText
            // 
            this.debugText.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.debugText.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.debugText.Location = new System.Drawing.Point(0, 204);
            this.debugText.Multiline = true;
            this.debugText.Name = "debugText";
            this.debugText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugText.Size = new System.Drawing.Size(240, 64);
            this.debugText.TabIndex = 0;
            this.debugText.Visible = false;
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "-";
            // 
            // menuDebug
            // 
            this.menuDebug.Text = "&Debug WIndow";
            this.menuDebug.Click += new System.EventHandler(this.menuDebug_Click);
            // 
            // RunningTimers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.debugText);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "RunningTimers";
            this.Text = "Timers";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuAdd;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuUpdateApp;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuExit;
        private System.Windows.Forms.TextBox debugText;
        private System.Windows.Forms.MenuItem menuDebug;
        private System.Windows.Forms.MenuItem menuItem1;

    }
}