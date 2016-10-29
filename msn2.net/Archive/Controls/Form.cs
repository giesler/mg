using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for Form.
	/// </summary>
	public class Form : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panelMain;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Button buttonViewTabs;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private bool showTabs = false;
		private Crownwood.Magic.Controls.TabControl tabControlPages;
		private Crownwood.Magic.Docking.DockingManagerIDE dockManager;
		private int tabHeight = 200;

		private Crownwood.Magic.Controls.TabControl tabsRight;
		private Crownwood.Magic.Docking.Content rightContent;

		public Form()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			dockManager = new Crownwood.Magic.Docking.DockingManagerIDE(this);

			tabsRight = new Crownwood.Magic.Controls.TabControl();
			rightContent = dockManager.CreateContent(tabsRight, "Web");
            
			dockManager.AddSingleContent(rightContent, Crownwood.Magic.Docking.State.DockBottom);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelMain = new System.Windows.Forms.Panel();
			this.buttonViewTabs = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.tabControlPages = new Crownwood.Magic.Controls.TabControl();
			this.panelMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelMain
			// 
			this.panelMain.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.buttonViewTabs});
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(440, 80);
			this.panelMain.TabIndex = 1;
			// 
			// buttonViewTabs
			// 
			this.buttonViewTabs.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonViewTabs.Location = new System.Drawing.Point(8, 56);
			this.buttonViewTabs.Name = "buttonViewTabs";
			this.buttonViewTabs.Size = new System.Drawing.Size(24, 16);
			this.buttonViewTabs.TabIndex = 0;
			this.buttonViewTabs.Text = "/\\";
			this.buttonViewTabs.Click += new System.EventHandler(this.buttonViewTabs_Click);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 80);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(440, 3);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// tabControlPages
			// 
			this.tabControlPages.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm;
			this.tabControlPages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlPages.HotTextColor = System.Drawing.SystemColors.ActiveCaption;
			this.tabControlPages.HotTrack = false;
			this.tabControlPages.ImageList = null;
			this.tabControlPages.Location = new System.Drawing.Point(0, 83);
			this.tabControlPages.Name = "tabControlPages";
			this.tabControlPages.PositionTop = true;
			this.tabControlPages.SelectedIndex = -1;
			this.tabControlPages.ShowArrows = true;
			this.tabControlPages.ShowClose = false;
			this.tabControlPages.ShrinkPagesToFit = false;
			this.tabControlPages.Size = new System.Drawing.Size(440, 243);
			this.tabControlPages.TabIndex = 3;
			this.tabControlPages.TextColor = System.Drawing.SystemColors.MenuText;
			// 
			// Form
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 326);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControlPages,
																		  this.splitter1,
																		  this.panelMain});
			this.Name = "Form";
			this.Text = "Form";
			this.panelMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonViewTabs_Click(object sender, System.EventArgs e)
		{
			if (showTabs)
			{
				tabHeight = tabControlPages.Height;
				tabControlPages.Visible = false;
				this.ClientSize = new Size(this.ClientSize.Width, tabControlPages.Top);
				buttonViewTabs.Text = @"\/";
			} 
			else 
			{
				tabControlPages.Visible = true;
				tabControlPages.Height = tabHeight;
				this.ClientSize = new Size(this.ClientSize.Width, tabControlPages.Top + tabHeight);
				buttonViewTabs.Text = @"/\";
			}
			showTabs = !showTabs;
		}

		protected void AddPage(string title, ShellForm page)
		{
			Crownwood.Magic.Controls.TabPage tabPage = 
				new Crownwood.Magic.Controls.TabPage(title, page);
			tabControlPages.TabPages.Add(tabPage);
		}

		protected void AddRightPage(string title, ShellForm page)
		{
			Crownwood.Magic.Controls.TabPage tabPage = 
				new Crownwood.Magic.Controls.TabPage(title, page);
			tabsRight.TabPages.Add(tabPage);
		}

		protected void AddFloat(string title, ShellForm page)
		{
			Crownwood.Magic.Docking.Content content = 
				new Crownwood.Magic.Docking.Content(page, title);

			page.Visible = true;

			if (page.FixedSize.Height > 0)
				content.FloatingSize = new Size(content.FloatingSize.Width, page.FixedSize.Height);

			if (page.FixedSize.Height > 0)
				content.FloatingSize = new Size(page.FixedSize.Width, content.FloatingSize.Height);

            dockManager.AddSingleContent(content, Crownwood.Magic.Docking.State.Floating);			
		}

		protected void AddDock(string title, ShellForm page)
		{
			Crownwood.Magic.Docking.Content content = 
				new Crownwood.Magic.Docking.Content(page, title);

			if (page.FixedSize.Height > 0)
				content.DockingSize = new Size(content.FloatingSize.Width, page.FixedSize.Height);

			if (page.FixedSize.Height > 0)
				content.DockingSize = new Size(page.FixedSize.Width, content.FloatingSize.Height);

			dockManager.AddSingleContent(content, Crownwood.Magic.Docking.State.DockRight);			
		}

		protected void AddFloat(string title, System.Windows.Forms.Form page)
		{
			Crownwood.Magic.Docking.Content content = 
				new Crownwood.Magic.Docking.Content(page, title);

			page.Visible = true;

			dockManager.AddSingleContent(content, Crownwood.Magic.Docking.State.Floating);			
		}
	
	}
}
