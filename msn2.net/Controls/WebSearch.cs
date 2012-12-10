using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using msn2.net.Configuration;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace msn2.net.Controls
{
	public class WebSearch : msn2.net.Controls.ShellForm
	{
		#region Declares

		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		private msn2.net.Controls.ShellButton buttonGo;
		private System.ComponentModel.IContainer components = null;

		#endregion

		#region Constructors

		public WebSearch()
		{
			InternalConstructor();
		}

		public WebSearch(Data data): base(data)
		{
			this.Text = "Search";
			InternalConstructor();
		}

		private void InternalConstructor()
		{
			InitializeComponent();

			comboBox1.SelectedIndex = 0;

			this.Left = Screen.PrimaryScreen.Bounds.Right - this.Width - 50;
			this.Top  = Screen.PrimaryScreen.Bounds.Bottom  - this.Height - 800;

			this.Left = 0;
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
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.buttonGo = new msn2.net.Controls.ShellButton();
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
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.Items.AddRange(new object[] {
														   "Google",
														   "Google Groups",
														   "allreceipes.com",
														   "Epicurius",
														   "Yahoo",
														   "MSDN",
														   "Microsoft Knowledge Base"});
			this.comboBox1.Location = new System.Drawing.Point(8, 8);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(288, 21);
			this.comboBox1.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBox1.Location = new System.Drawing.Point(8, 32);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(240, 20);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "<search>";
			// 
			// buttonGo
			// 
			this.buttonGo.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonGo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonGo.Location = new System.Drawing.Point(256, 32);
			this.buttonGo.Name = "buttonGo";
			this.buttonGo.Size = new System.Drawing.Size(40, 24);
			this.buttonGo.StartColor = System.Drawing.Color.LightGray;
			this.buttonGo.TabIndex = 3;
			this.buttonGo.Text = "go";
			this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
			// 
			// WebSearch
			// 
			this.AcceptButton = this.buttonGo;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 62);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonGo,
																		  this.textBox1,
																		  this.comboBox1});
			this.KeyPreview = true;
			this.Name = "WebSearch";
			this.ShowInTaskbar = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Search";
			this.TitleVisible = true;
			this.Load += new System.EventHandler(this.WebSearch_Load);
			this.Activated += new System.EventHandler(this.WebSearch_Activated);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.WebSearch_Paint);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WebSearch_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void WebSearch_Activated(object sender, System.EventArgs e)
		{
			this.textBox1.Focus();
			this.textBox1.SelectAll();
		}

		private void WebSearch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				textBox1.Text = "";
				textBox1.Focus();
				e.Handled = true;
			}
		}

		private void buttonGo_Click(object sender, System.EventArgs e)
		{
			WebBrowser browser = null;

			SearchConfigData searchConfigData = null;

			switch (comboBox1.SelectedIndex)
			{
				case 0:
					searchConfigData = new WebSearchConfigData(textBox1.Text);
					break;
				case 1:
					searchConfigData = new GoogleGroupsSearchConfigData(textBox1.Text);
					break;
				case 2:
					searchConfigData = new AllRecipesSearchConfigData(textBox1.Text);
					break;
				case 3:
					searchConfigData = new EpicuriosSearchConfigData(textBox1.Text);
					break;
				case 4:
					searchConfigData = new YahooSearchConfigData(textBox1.Text);
					break;
				case 5:
					searchConfigData = new MSDNSearchConfigData(textBox1.Text);
					break;
				case 6:
					searchConfigData = new MicrosoftKbSearchConfigData(textBox1.Text);
					break;
			}
            
			Data searchData = this.Data.Get(String.Format(searchConfigData.Title, textBox1.Text), searchConfigData, searchConfigData.GetType());

			browser = new WebBrowser(searchData);
			browser.Show();

		}

		private void WebSearch_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);
		}

		private void WebSearch_Load(object sender, System.EventArgs e)
		{
			this.Left = 100;
			this.Top = 100;
		}
	}

	#region SearchConfigData

	public class SearchConfigData: msn2.net.Configuration.ConfigData
	{
		#region Declares

		private string searchString;
		protected string title;

		#endregion

		#region Constructors

		public SearchConfigData()
		{}

		public SearchConfigData(string searchString)
		{
			this.searchString = searchString;

			title = String.Format("Search Results: '{0}'", searchString);
		}

		#endregion

		#region Methods

		public virtual void Run(Crownwood.Magic.Controls.TabPage page)
		{
		}

		#endregion

		#region Properties

		public string SearchString
		{
			get
			{
				return searchString;
			}
			set
			{
				searchString = value;
			}
		}

		public WebBrowserControl.DefaultClickBehavior DefaultClickBehavior
		{
			get
			{
				return WebBrowserControl.DefaultClickBehavior.OpenInNewTab;
			}
		}

		public string Title
		{
			get 
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		#endregion
	}

	#endregion

	#region WebSearchConfigData

	public class WebSearchConfigData: SearchConfigData
	{
		#region Declares

		private GoogleSearchService googleSearch;

		#endregion

		#region Constructors

		public WebSearchConfigData()
		{
		}

		public WebSearchConfigData(string t): base(t)
		{
			title = String.Format("Google Search Results: '{0}'", t);
		}

		#endregion

		#region Methods

		public override void Run(Crownwood.Magic.Controls.TabPage page)
		{
			googleSearch = new GoogleSearchService();

			WebBrowserControl browser = (WebBrowserControl) page.Control;
			browser.BeforeNavigateEvent += new BeforeNavigateDelegate(OnBeforeNavigate);
			browser.ShowStatus("searching...");
			browser.Navigate("next:1");
		}

		public void OnBeforeNavigate(object sender, BeforeNavigateEventArgs e)
		{
			WebBrowserControl browser = (WebBrowserControl) sender;

			int startItem	= 0; 
			int endItem		= 0;

			if (e.Url.Length > 5 && (e.Url.Substring(0, 5).Equals("next:") || e.Url.Substring(0, 5).Equals("prev:")))
			{
				startItem	= Convert.ToInt32(e.Url.Substring(5)) - 1;
				endItem		= startItem + 10;
			}
			else
			{
				// We don't care about this navigation
				return;
			}

			browser.ShowStatus("searching...");
			GoogleSearchResult result = googleSearch.doGoogleSearch(
				"YFpgsNEe9BfAzm5QcAp+82eYDgyGSWh0", this.SearchString, startItem, 10, false, "", false, "", "", "");

			e.Url = BuildResultPage(result, this.SearchString + ": " + (startItem+1) + " - " + endItem);
			e.ClickBehavior = WebBrowserControl.DefaultClickBehavior.OpenLink;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Builds results and returns the temp file name
		/// </summary>
		/// <param name="result">Result element</param>
		/// <returns>Temp file name</returns>
		private string BuildResultPage(GoogleSearchResult result, string searchString)
		{
			StringBuilder sb	= new StringBuilder();
			int currentIndex	= result.startIndex;

			sb.Append("<html><head><title>'" + searchString + "'</title></head>");
			sb.Append("<body>");

			// Append results
			foreach (ResultElement resultElement in result.resultElements)
			{
				sb.Append("<table width=\"100%\"><tr><td valign=\"top\">" + currentIndex.ToString() + ".</td>");
				sb.Append("<td><b>" + resultElement.title + "</b><br>");
				sb.Append("<a href=\"" + resultElement.URL + "\">" + resultElement.URL + "</a><br>");
				sb.Append("<font size=\"-1\">" + resultElement.snippet + "</font>");
				sb.Append("</td></tr></table>");

				currentIndex++;
			}

			sb.Append("<table width=\"100%\"><tr><td align=\"right\">");
            
			// Add prev/next links
			if (result.startIndex > 1)
			{
				int previousStart = result.startIndex - 11;
                sb.Append("<a href=\"prev:" + previousStart + "\">&lt; &lt; - - previous</a> | ");
			}

			if (result.endIndex < result.estimatedTotalResultsCount)
			{
                int nextStart		= result.startIndex + 10;
				sb.Append("<a href=\"next:" + nextStart.ToString() + "\"> next - - &gt; &gt;</a> | ");
			}
			sb.Append("</td></tr></table>");

			sb.Append("</body>");
			sb.Append("</html>");

			// Write to a file
			string filename = Path.GetTempFileName();
			TextWriter tw = File.AppendText(filename);
			tw.Write(sb.ToString());
			tw.Flush();
			tw.Close();

			return filename;
		}

		#endregion
	}

	#endregion

	#region CustomSearchConfigData

	public class CustomWebSearchConfigData: SearchConfigData
	{
		#region Declares

		protected string siteName			= "";
		protected string searchUrl			= "";
		protected string resultUrlRegEx		= "";
		protected string moreDataUrlRegEx	= "";

		#endregion

		#region Constructor

		public CustomWebSearchConfigData()
		{
		}

		public CustomWebSearchConfigData(string searchString): base(searchString)
		{
		}

		#endregion

		#region Methods

		public override void Run(Crownwood.Magic.Controls.TabPage page)
		{
            string url	= searchUrl;
			url			= String.Format(url, System.Web.HttpUtility.UrlEncode(this.SearchString));

			WebBrowserControl browser = (WebBrowserControl) page.Control;
			browser.BeforeNavigateEvent	 += new BeforeNavigateDelegate(OnBeforeNavigate);
			browser.Navigate(url);
		}

		public void OnBeforeNavigate(object sender, BeforeNavigateEventArgs e)
		{
			WebBrowserControl browser = (WebBrowserControl) sender;
			
			Regex moreRegEx = new Regex(moreDataUrlRegEx);
			Regex rsltRegEx = new Regex(resultUrlRegEx);

			// Check if 'more' page is in Url string
			if (moreDataUrlRegEx.Length > 0 && moreRegEx.Match(e.Url).Success)
			{
				e.ClickBehavior = WebBrowserControl.DefaultClickBehavior.OpenLink;
			}
			// Check if clicking a result item
			else if (resultUrlRegEx.Length > 0 && rsltRegEx.Match(e.Url).Success)
			{
				e.ClickBehavior	= WebBrowserControl.DefaultClickBehavior.OpenInNewTab;
			}
			// We're not sure, so default to a new tab
			else
			{
				e.ClickBehavior = WebBrowserControl.DefaultClickBehavior.OpenInNewTab;
			}
			
		}
		
		#endregion

		#region Properties

		public string SearchUrl
		{
			get 
			{
				return searchUrl;
			}
			set
			{
				searchUrl = value;
			}
		}

		public string ResultUrlRegEx
		{
			get
			{
				return resultUrlRegEx;
			}
			set
			{
				resultUrlRegEx = value;
			}
		}

		public string MoreDataUrlRegEx
		{
			get
			{
				return moreDataUrlRegEx;
			}
			set
			{
				moreDataUrlRegEx = value;
			}
		}

		#endregion
	}

	#endregion

	#region GoogleGroupsSearchConfigData

	public class GoogleGroupsSearchConfigData: CustomWebSearchConfigData
	{
		public GoogleGroupsSearchConfigData()
		{}

		public GoogleGroupsSearchConfigData(string searchString): base(searchString)
		{
			title = String.Format("Google Groups Results: '{0}'", searchString);
			
			this.searchUrl			= "http://groups.google.com/groups?hl=en&q={0}";
			this.resultUrlRegEx		= @"(http://groups.google.com/groups\?(.)*((selm=(.)*)|(threadm=(.)*))(.)*)";
			this.moreDataUrlRegEx	= @"(http://groups.google.com/groups\?(.)*((start=(.)*)|(sa=(.)*)|(group=(.)*))(.)*)";
		}
	}

	#endregion

	#region AllRecipesSearchConfigData
	
	public class AllRecipesSearchConfigData: CustomWebSearchConfigData
	{
		public AllRecipesSearchConfigData()
		{}

		public AllRecipesSearchConfigData(string searchString): base(searchString)
		{
			title = String.Format("allrecipes.com Search Results: '{0}'", searchString);
			
			this.searchUrl			= "http://search.allrecipes.com/SearchResults.asp?site=allrecipes&allrecipes=allrecipes&q1={0}&Search+Allrecipes%21.x=2&Search+Allrecipes%21.y=8";
			this.moreDataUrlRegEx		= @"(http://search.allrecipes.com/searchresults.asp)";
		}
	}

	#endregion

	#region EpicuriosSearchConfigData
	
	public class EpicuriosSearchConfigData: CustomWebSearchConfigData
	{
		public EpicuriosSearchConfigData()
		{}

		public EpicuriosSearchConfigData(string searchString): base(searchString)
		{
			title = String.Format("Epicurious Search Results: '{0}'", searchString);
			
			this.searchUrl			= "http://www.epicurious.com/s97is.vts?action=filtersearch&filter=recipe-filter.hts&collection=Recipes&ResultTemplate=recipe-results.hts&queryType=and&keyword={0}";
			this.moreDataUrlRegEx	= @"(http://www.epicurious.com/s97is.vts\?)(.)*(ResultStart=)(.)*";
		}
	}

	#endregion

	#region YahooSearchConfigData
	
	public class YahooSearchConfigData: CustomWebSearchConfigData
	{
		public YahooSearchConfigData()
		{}

		public YahooSearchConfigData(string searchString): base(searchString)
		{
			title = String.Format("Yahoo Search Results: '{0}'", searchString);
			
			this.searchUrl			= "http://search.yahoo.com/bin/search?p={0}";
			this.moreDataUrlRegEx	= @"(http://google.yahoo.com/bin/query\?p=)(.)*(hs=)(.)*";
		}
	}

	#endregion

	#region MSDNSearchConfigData
	
	public class MSDNSearchConfigData: CustomWebSearchConfigData
	{
		public MSDNSearchConfigData()
		{}

		public MSDNSearchConfigData(string searchString): base(searchString)
		{
			title = String.Format("MSDN Search Results: '{0}'", searchString);
			
			this.searchUrl			= "http://search.microsoft.com/default.asp?qu={0}&boolean=ALL&nq=NEW&so=RECCNT&p=1&ig=01&ig=02&ig=03&ig=04&ig=05&ig=06&i=00&i=01&i=02&i=03&i=04&i=05&i=06&i=07&i=08&i=09&i=10&i=11&i=12&i=13&i=14&i=15&i=16&i=17&i=18&i=19&i=20&i=21&i=22&i=23&i=24&i=25&i=26&i=27&i=28&i=29&i=30&i=31&i=32&i=33&i=34&i=35&i=36&i=37&i=38&i=39&i=40&i=41&i=42&i=43&i=44&i=45&i=46&i=47&i=48&i=49&i=50&i=51&siteid=us/dev";
			this.moreDataUrlRegEx	= @"(http://search.microsoft.com/gomsuri.asp\?)(.)*(c=rp_NextResults)(.)*";
		}
	}

	#endregion

	#region MicrosoftKbSearchConfigData
	
	public class MicrosoftKbSearchConfigData: CustomWebSearchConfigData
	{
		public MicrosoftKbSearchConfigData()
		{}

		public MicrosoftKbSearchConfigData(string searchString): base(searchString)
		{
			title = String.Format("Microsoft KB Search Results: '{0}'", searchString);
			
			this.searchUrl			= "http://search.support.microsoft.com/search/default.aspx?Catalog=LCID%3D1033%26CDID%3DEN-US-KB%26PRODLISTSRC%3DON&Product=msall&Query={0}&Queryc={0}&REF=false&srchstep=0&KeywordType=ALL&Titles=false&numDays=&maxResults=50";
			this.resultUrlRegEx		= @"(http://support.microsoft.com/default.aspx\?)(.)*(scid=kb;en-us;Q([0-9])*)";

			//this.moreDataUrlRegEx	= @"(http://search.microsoft.com/gomsuri.asp\?)(.)*(c=rp_NextResults)(.)*";
		}
	}

	#endregion

	#region CommunitySearchConfigData

	public class CommunitySearchConfigData: SearchConfigData
	{
		public CommunitySearchConfigData()
		{}

		public CommunitySearchConfigData(string t): base(t)
		{}
		
		public override void Run(Crownwood.Magic.Controls.TabPage page)
		{
			string url = "http://groups.google.com/groups?hl=en&q={0}";
			url = String.Format(url, System.Web.HttpUtility.UrlEncode(this.SearchString));

			WebBrowserControl browser = (WebBrowserControl) page.Control;
			browser.Navigate(url);
		}
	}

	#endregion

}

