using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using msn2.net.Configuration;
using System.Text;
using System.IO;

namespace msn2.net.Controls
{
	public class WebBrowser : msn2.net.Controls.ShellForm
	{
		#region Declares

		public msn2.net.Controls.WebBrowserControl webBrowserControl1;
		private System.ComponentModel.IContainer components = null;
		private TabControl tabControl;
		private int defaultWidth = 800;
		private int defaultHeight = 700;
		protected TitleBarControl [] buttons = null;

		#endregion
		#region Constructors

		public WebBrowser(Data data): base(data)
		{
			InitializeComponent();
			InternalConstructor(data.Text);
			
			// If we have config data to use, then init based on it
			if (Data.ConfigData != null)
			{
				// Check if a search config object
				if (Data.ConfigData.GetType().IsSubclassOf(typeof(CustomWebSearchConfigData)))
				{
					CustomWebSearchConfigData searchData = (CustomWebSearchConfigData) data.ConfigData;
					TabPage page = AddNewTab(Data.Text, searchData.DefaultClickBehavior);
					searchData.Run(page);					
				}
				else if (Data.ConfigData.GetType().IsSubclassOf(typeof(SearchConfigData)))
				{
					SearchConfigData searchData = (SearchConfigData) data.ConfigData;
                    
					TabPage page = AddNewTab(Data.Text, searchData.DefaultClickBehavior);
					
					searchData.Run(page);					
				}
			}

			if (Screen.PrimaryScreen.WorkingArea.Width < 800)
			{
				this.Width	= Screen.PrimaryScreen.WorkingArea.Width;
				this.Left	= Screen.PrimaryScreen.WorkingArea.Left;
				this.Height = Screen.PrimaryScreen.WorkingArea.Height;
				this.Top	= Screen.PrimaryScreen.WorkingArea.Top;
			}

			this.FadeInEnabled = false;
		}

		public WebBrowser(Data data, string title, int width, int height): base(data)
		{
			InitializeComponent();

			defaultWidth = width;
			defaultHeight = height;

			this.MaximumSize = new Size(width, height);

			InternalConstructor(title);
		}

		public WebBrowser(string title, string url): base()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			InternalConstructor(title);

			AddNewTab(title, url);
		}

		public WebBrowser(string title, string url, WebBrowserControl.DefaultClickBehavior defaultClickBehavior): base()
		{
			InitializeComponent();

			InternalConstructor(title);

			AddNewTab(title, url, defaultClickBehavior);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="title"></param>
		/// <param name="popup"></param>
		/// <remarks>Used by web browser for new windows</remarks>
		public WebBrowser(string title, bool popup)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			if (webBrowserControl1 != null)
			{
				this.webBrowserControl1.IsPopup = popup;
			}

			InternalConstructor(title);

		}

		private void InternalConstructor(string title)
		{
			// Create tabbed window with browsers
			tabControl = new TabControl();
			tabControl.Dock = DockStyle.Fill;
			this.Controls.Add(tabControl);

			tabControl.Visible = true;

			this.Text = title;

			this.Width = defaultWidth;
			this.Height = defaultHeight;

			// Create and add title bar controls
			buttons = new TitleBarControl[3];
			buttons[0]		= new TitleBarControl("buttonRefresh", "r", new EventHandler(Refresh_Clicked));
			buttons[1]		= new TitleBarControl("buttonSaveTo", "+", new EventHandler(SaveTo_Clicked));
			buttons[2]		= new TitleBarControl("buttonBack", "<", new EventHandler(Back_Clicked));

			this.AddButtons(buttons, true);

		}

		#endregion
		#region Methods

		public void HideTitlebarButtons()
		{
			foreach (TitleBarControl c in buttons)
			{
				c.Visible = false;
			}
		}

		#region AddNewTab methods
		public TabPage AddNewTab(string title, WebBrowserControl.DefaultClickBehavior defaultClickBehavior)
		{
			msn2.net.Controls.WebBrowserControl browser = new msn2.net.Controls.WebBrowserControl(defaultClickBehavior);

			browser.TitleChange += new msn2.net.Controls.TitleChangeDelegate(this.webBrowserControl1_TitleChange);
			browser.NavigateComplete	+= new NavigateCompleteDelegate(webBrowserControl_NavigateComplete);
			browser.OpenNewTabEvent += new OpenNewTabDelegate(OpenNewTab);
			browser.ShowStatus("searching...");
            browser.Dock = DockStyle.Fill;

			// Add new tab
			TabPage page = new TabPage(title);
            page.Controls.Add(browser);
			browser.TabPage = page;
			tabControl.TabPages.Add(page);

			return page;
		}

		public TabPage AddNewTab(ShellForm form)
		{
			TabPage page = new TabPage(form.Text);
            form.Dock = DockStyle.Fill;
            page.Controls.Add(form);
			tabControl.TabPages.Add(page);

			return page;
		}

		public TabPage AddNewTab(string title, string url)
		{
			return AddNewTab(title, url, WebBrowserControl.DefaultClickBehavior.OpenLink);
		}

		public TabPage AddNewTab(string title, string url, WebBrowserControl.DefaultClickBehavior defaultClickBehavior)
		{
			msn2.net.Controls.WebBrowserControl browser = new msn2.net.Controls.WebBrowserControl(defaultClickBehavior);

			browser.TitleChange += new msn2.net.Controls.TitleChangeDelegate(this.webBrowserControl1_TitleChange);
			browser.NavigateComplete	+= new NavigateCompleteDelegate(webBrowserControl_NavigateComplete);
			browser.OpenNewTabEvent += new OpenNewTabDelegate(OpenNewTab);
			browser.Navigate(url);

			// Add new tab
			TabPage page = new TabPage(title);
            browser.Dock = DockStyle.Fill;
            page.Controls.Add(browser);
			tabControl.TabPages.Add(page);

			return page;
		}

		#endregion
		#region AddStaticTab methods
		public void AddStaticTab(string title, string url)
		{
			AddStaticTab(title, url, new TimeSpan(0));
		}

		public void AddStaticTab(string title, string url, TimeSpan refresh)
		{
			msn2.net.Controls.WebBrowserControl browser = new msn2.net.Controls.WebBrowserControl(WebBrowserControl.DefaultClickBehavior.OpenInNewWindow, refresh);

			browser.NavigateComplete	+= new NavigateCompleteDelegate(webBrowserControl_NavigateComplete);
			browser.OpenNewTabEvent		+= new OpenNewTabDelegate(OpenNewTab);
			browser.Navigate(url);

			// Add new tab
			TabPage page = new TabPage(title);
            browser.Dock = DockStyle.Fill;
            page.Controls.Add(browser);
            tabControl.TabPages.Add(page);
		}

		#endregion

		public void OpenNewTab(object sender, NavigateEventArgs e)
		{
			AddNewTab(e.Title, e.Url);
		}

		#endregion
		#region Disposal

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion
		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WebBrowser));
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// timerFadeOut
			// 
			this.timerFadeOut.Enabled = false;
			// 
			// timerFadeIn
			// 
			this.timerFadeIn.Enabled = false;
			// 
			// WebBrowser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 264);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "WebBrowser";
			this.ShowInTaskbar = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Web Browser";
			this.TitleVisible = true;
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		#region Event Handlers

		#region Title Bar buttons

		private void Refresh_Clicked(object sender, EventArgs e)
		{
			if (tabControl.TabPages.Count == 0)
				return;

			// Get the current page
			TabPage page = tabControl.SelectedTab;
			WebBrowserControl browser = (WebBrowserControl) page.Controls[0];

			if (browser != null)
			{
				browser.RefreshPage();
			}
		}

		private void SaveTo_Clicked(object sender, EventArgs e)
		{
			if (tabControl.TabPages.Count == 0)
				return;

			// Get the current page
			TabPage page = tabControl.SelectedTab;
			WebBrowserControl browser = (WebBrowserControl) page.Controls[0];

			if (browser != null)
			{
				ShellSave shellSave	= new ShellSave();
				shellSave.Visible = false;
				
				if (shellSave.ShowShellDialog(this) == DialogResult.OK)
				{
					shellSave.Data.Get(page.Text, browser.Url, new FavoriteConfigData(), typeof(FavoriteConfigData));
				}
				shellSave.Dispose();
			}
		}

		private void Back_Clicked(object sender, EventArgs e)
		{
			if (tabControl.TabPages.Count == 0)
				return;

			// Get the current page
			TabPage page = tabControl.SelectedTab;
			WebBrowserControl browser = (WebBrowserControl) page.Controls[0];

			if (browser != null)
			{
				browser.Back();
			}
			
		}

		#endregion

		private void webBrowserControl1_TitleChange(object sender, msn2.net.Controls.TitleChangeEventArgs e)
		{
			WebBrowserControl browser = (WebBrowserControl) sender;

			if (browser.TabPage != null)
				browser.TabPage.Text = e.Title;
		}

		private void webBrowserControl_NavigateComplete(object sender, NavigateCompleteEventArgs e)
		{
			
		}

		#endregion
	}

	[Serializable]
	public class WebBrowserConfigData: msn2.net.Configuration.ConfigData
	{
		#region Declares
		private ArrayList pages;
		#endregion
		#region Constructors
		public WebBrowserConfigData()
		{
		}
		#endregion
        
	}

}

