using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using msn2.net.Configuration;

namespace msn2.net.Controls
{
	public class WebBrowser : msn2.net.Controls.ShellForm
	{
		#region Declares

		public msn2.net.Controls.WebBrowserControl webBrowserControl1;
		private System.ComponentModel.IContainer components = null;
		private Crownwood.Magic.Controls.TabControl tabControl;
		private Crownwood.Magic.Controls.TabPage resultsPage = null;
		private int defaultWidth = 800;
		private int defaultHeight = 700;
		private Panel resultPanel = null;

		#endregion

		#region Constructors

		public WebBrowser(string title, int width, int height)
		{
			InitializeComponent();

			defaultWidth = width;
			defaultHeight = height;

			InternalConstructor(title);
		}

		public WebBrowser(string title, string url)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			InternalConstructor(title);

			AddNewTab(title, url);
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

			this.webBrowserControl1.IsPopup = popup;

			InternalConstructor(title);

		}

		public WebBrowser(GoogleSearchSettings settings)
		{
			InternalConstructor("Search results: '" + settings.SearchString + "'");
			
			GoogleSearchService googleSearch = new GoogleSearchService();

			//Instantiate an AsyncCallback delegate to use as a parameter
			//in the BeginFactorize method.
			//AsyncCallback cb = new AsyncCallback(GoogleSearchResultCallback);

			// Begin the Async call to Factorize, passing in our
			// AsyncCalback delegate and a reference
			// to our instance of PrimeFactorizer.
			//IAsyncResult ar = googleSearch.BegindoGoogleSearch(settings.Key, settings.SearchString, 50, 1000, false, "", false, "", "", "", cb, googleSearch);

			GoogleSearchResult result = googleSearch.doGoogleSearch(settings.Key, settings.SearchString, 50, 10, false, "", false, "", "", "");

			// create new results tab
			resultPanel = new Panel();
			resultPanel.AutoScroll = true;
			resultsPage = new Crownwood.Magic.Controls.TabPage("'" + settings.SearchString + "'", resultPanel);
			tabControl.TabPages.Add(resultsPage);

			foreach (ResultElement resultElement in result.resultElements)
			{
				GoogleResultControl resultControl = new GoogleResultControl(resultElement);
				resultControl.Width			= this.Width;
				resultControl.Dock			= DockStyle.Top;
				resultPanel.Controls.Add(resultControl);
			}
			
		}

		private void GoogleSearchResultCallback(IAsyncResult ar)
		{
			
			GoogleSearchService googleSearch	= (GoogleSearchService) ar.AsyncState;
			GoogleSearchResult result					=  googleSearch.EnddoGoogleSearch(ar);

			foreach (ResultElement resultElement in result.resultElements)
			{
				GoogleResultControl resultControl = new GoogleResultControl(resultElement);
				resultControl.Width			= this.Width;
				resultControl.Dock			= DockStyle.Top;
				resultPanel.Controls.Add(resultControl);
			}
			// TODO: Should history be added to a data object?  so constructors here need data objects...
		}

		private void InternalConstructor(string title)
		{
			// Create tabbed window with browsers
			tabControl = new Crownwood.Magic.Controls.TabControl();
			tabControl.Dock = DockStyle.Fill;
			tabControl.ShowClose		= true;
			tabControl.ShowArrows		= true;
			tabControl.PositionTop		= true;
			tabControl.ClosePressed		+= new EventHandler(ClosePressed);
			this.Controls.Add(tabControl);

			tabControl.Visible = true;

			this.Text = title;

			this.Width = defaultWidth;
			this.Height = defaultHeight;
		}

		#endregion

		#region Methods

		public void AddNewTab(string title, string url)
		{
			msn2.net.Controls.WebBrowserControl browser = new msn2.net.Controls.WebBrowserControl();

			browser.TitleChange += new msn2.net.Controls.TitleChangeDelegate(this.webBrowserControl1_TitleChange);
			browser.NavigateComplete	+= new NavigateCompleteDelegate(webBrowserControl_NavigateComplete);
			browser.OpenNewTabEvent += new OpenNewTabDelegate(OpenNewTab);
			browser.Navigate(url);

			// Add new tab
			Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage(title, browser);
			browser.TabPage = page;
			tabControl.TabPages.Add(page);
		}

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
			this.Text = "Web Browser";
			this.TitleVisible = true;
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void webBrowserControl1_TitleChange(object sender, msn2.net.Controls.TitleChangeEventArgs e)
		{
			WebBrowserControl browser = (WebBrowserControl) sender;

			if (browser.TabPage != null)
				browser.TabPage.Title = e.Title;

			if (tabControl.SelectedTab == browser.TabPage)
			{
				this.Text = e.Title;
			}
		}

		private void webBrowserControl_NavigateComplete(object sender, NavigateCompleteEventArgs e)
		{
			
		}
	}

	#region GoogleSearchSettings

	public class GoogleSearchSettings
	{
		private string searchString;
		private string key;

		public GoogleSearchSettings(string key, string searchString)
		{
			this.searchString		= searchString;
			this.key				= key;
		}

		public string SearchString
		{
			get { return searchString; }
			set { searchString = value; }
		}

		public string Key
		{
			get { return key; }
			set { key = value; }
		}
	}

	#endregion
}

