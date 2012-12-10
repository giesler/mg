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
	public class WebBrowserForm : System.Windows.Forms.Form
	{
		#region Declares

		public msn2.net.Controls.WebBrowserControl webBrowserControl1;
		private System.ComponentModel.IContainer components = null;
		private Crownwood.Magic.Controls.TabControl tabControl;
		private int defaultWidth = 800;
		private int defaultHeight = 700;
		protected Crownwood.Magic.Docking.DockingManager dockManager = null;
		protected TitleBarControl [] buttons = null;

		#endregion
		#region Constructors

		public WebBrowserForm(string title, int width, int height)
		{
			InitializeComponent();

			defaultWidth = width;
			defaultHeight = height;

			this.MaximumSize = new Size(width, height);

			InternalConstructor(title);
		}

		public WebBrowserForm(string title, string url): base()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			InternalConstructor(title);

			AddNewTab(title, url);
		}

		public WebBrowserForm(string title, string url, WebBrowserControl.DefaultClickBehavior defaultClickBehavior): base()
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
		public WebBrowserForm(string title, bool popup)
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
			dockManager = new Crownwood.Magic.Docking.DockingManager(this, Crownwood.Magic.Common.VisualStyle.IDE);
			
			// Create tabbed window with browsers
			tabControl = new Crownwood.Magic.Controls.TabControl();
			tabControl.Dock = DockStyle.Fill;
			tabControl.ShowClose		= true;
			tabControl.ShowArrows		= true;
			tabControl.PositionTop		= true;
			tabControl.ClosePressed		+= new EventHandler(ClosePressed);
			tabControl.HideTabsUsingLogic = true;
			this.Controls.Add(tabControl);

			tabControl.Visible = true;

			this.Text = title;

			this.Width = defaultWidth;
			this.Height = defaultHeight;

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
		public Crownwood.Magic.Controls.TabPage AddNewTab(string title, WebBrowserControl.DefaultClickBehavior defaultClickBehavior)
		{
			msn2.net.Controls.WebBrowserControl browser = new msn2.net.Controls.WebBrowserControl(defaultClickBehavior);

			browser.TitleChange += new msn2.net.Controls.TitleChangeDelegate(this.webBrowserControl1_TitleChange);
			browser.NavigateComplete	+= new NavigateCompleteDelegate(webBrowserControl_NavigateComplete);
			browser.OpenNewTabEvent += new OpenNewTabDelegate(OpenNewTab);
			browser.ShowStatus("searching...");

			// Add new tab
			Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage(title, browser);
			browser.TabPage = page;
			tabControl.TabPages.Add(page);

			return page;
		}

		public Crownwood.Magic.Controls.TabPage AddNewTab(ShellForm form)
		{
			Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage(form.Text, form);
			form.TabPage = page;
			tabControl.TabPages.Add(page);

			return page;
		}

		public Crownwood.Magic.Controls.TabPage AddNewTab(string title, string url)
		{
			return AddNewTab(title, url, WebBrowserControl.DefaultClickBehavior.OpenLink);
		}

		public Crownwood.Magic.Controls.TabPage AddNewTab(string title, string url, WebBrowserControl.DefaultClickBehavior defaultClickBehavior)
		{
			msn2.net.Controls.WebBrowserControl browser = new msn2.net.Controls.WebBrowserControl(defaultClickBehavior);

			browser.TitleChange += new msn2.net.Controls.TitleChangeDelegate(this.webBrowserControl1_TitleChange);
			browser.NavigateComplete	+= new NavigateCompleteDelegate(webBrowserControl_NavigateComplete);
			browser.OpenNewTabEvent += new OpenNewTabDelegate(OpenNewTab);
			browser.Navigate(url);

			// Add new tab
			Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage(title, browser);
			browser.TabPage = page;
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
			Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage(title, browser);
			browser.TabPage = page;
			tabControl.TabPages.Add(page);
		}

		#endregion
		public void ClosePressed(object sender, System.EventArgs e)
		{
			Crownwood.Magic.Controls.TabControl tabControl = (Crownwood.Magic.Controls.TabControl) sender;

			// Remove the reference to the TabPage on the browser object
			WebBrowserControl browser = (WebBrowserControl) tabControl.SelectedTab.Control;
			if (browser != null)
			{
				browser.TabPage = null;
			}

			tabControl.TabPages.Remove(tabControl.SelectedTab);

		}

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
			this.SuspendLayout();
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
			Crownwood.Magic.Controls.TabPage page = tabControl.SelectedTab;
			WebBrowserControl browser = (WebBrowserControl) page.Control;

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
			Crownwood.Magic.Controls.TabPage page = tabControl.SelectedTab;
			WebBrowserControl browser = (WebBrowserControl) page.Control;

			if (browser != null)
			{
				ShellSave shellSave	= new ShellSave();
				shellSave.Visible = false;
				
				if (shellSave.ShowShellDialog(this) == DialogResult.OK)
				{
					shellSave.Data.Get(page.Title, browser.Url, new FavoriteConfigData(), typeof(FavoriteConfigData));
				}
				shellSave.Dispose();
			}
		}

		private void Back_Clicked(object sender, EventArgs e)
		{
			if (tabControl.TabPages.Count == 0)
				return;

			// Get the current page
			Crownwood.Magic.Controls.TabPage page = tabControl.SelectedTab;
			WebBrowserControl browser = (WebBrowserControl) page.Control;

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
				browser.TabPage.Title = e.Title;
		}

		private void webBrowserControl_NavigateComplete(object sender, NavigateCompleteEventArgs e)
		{
			
		}

		#endregion
		#region Properties

		public bool ShowClose
		{
			get
			{
				return tabControl.ShowClose;
			}
			set
			{
				tabControl.ShowClose = value;
			}
		}

		public bool ShowArrows
		{
			get
			{
				return tabControl.ShowArrows;
			}
			set
			{
				tabControl.ShowArrows = value;
			}
		}

		public Crownwood.Magic.Controls.TabControl.VisualAppearance TabAppearance
		{
			get
			{
				return tabControl.Appearance;
			}
			set
			{
				tabControl.Appearance = value;
			}
		}

		#endregion
	}


}

