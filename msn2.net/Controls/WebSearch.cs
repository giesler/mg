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

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label labelSearchLocation;
		private System.Windows.Forms.Label labelSearchFor;
		private System.Windows.Forms.ListView listViewSearch;
		private System.Windows.Forms.ImageList imageListSearch;
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

//			this.Left = Screen.PrimaryScreen.Bounds.Right - this.Width - 50;
//			this.Top  = Screen.PrimaryScreen.Bounds.Bottom  - this.Height - 800;

//			this.Left = 0;

			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			//System.Drawing.Icon icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("msn2.net.Controls.Icons.WebSearch.ico"));
			//this.Icon = new System.Drawing.Icon(icon, 16, 16);

			// Add items to listview
			SearchListViewItem item = new SearchListViewItem(new WebSearchConfigData(), listViewSearch.SmallImageList);
			listViewSearch.Items.Add(item);
			item.Selected = true;

			listViewSearch.Items.Add(new SearchListViewItem(new GoogleGroupsSearchConfigData(), listViewSearch.SmallImageList));
			listViewSearch.Items.Add(new SearchListViewItem(new AllRecipesSearchConfigData(), listViewSearch.SmallImageList));
			listViewSearch.Items.Add(new SearchListViewItem(new EpicuriosSearchConfigData(), listViewSearch.SmallImageList));
			listViewSearch.Items.Add(new SearchListViewItem(new YahooSearchConfigData(), listViewSearch.SmallImageList));
			listViewSearch.Items.Add(new SearchListViewItem(new MSDNSearchConfigData(), listViewSearch.SmallImageList));
			listViewSearch.Items.Add(new SearchListViewItem(new MicrosoftKbSearchConfigData(), listViewSearch.SmallImageList));

			listViewSearch.BackColor = msn2.net.Common.Drawing.LightenColor(this.BackColor);
			//			listViewSearch.Items.Add(new SearchListViewItem(new CommunitySearchConfigData(), listViewSearch.SmallImageList));
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WebSearch));
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.labelSearchFor = new System.Windows.Forms.Label();
			this.labelSearchLocation = new System.Windows.Forms.Label();
			this.listViewSearch = new System.Windows.Forms.ListView();
			this.imageListSearch = new System.Windows.Forms.ImageList(this.components);
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
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBox1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(8, 32);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(400, 23);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "<type search string>";
			this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
			// 
			// labelSearchFor
			// 
			this.labelSearchFor.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelSearchFor.Location = new System.Drawing.Point(8, 8);
			this.labelSearchFor.Name = "labelSearchFor";
			this.labelSearchFor.Size = new System.Drawing.Size(376, 24);
			this.labelSearchFor.TabIndex = 5;
			this.labelSearchFor.Text = "What do you want to search for?";
			this.labelSearchFor.Visible = false;
			// 
			// labelSearchLocation
			// 
			this.labelSearchLocation.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelSearchLocation.Location = new System.Drawing.Point(8, 64);
			this.labelSearchLocation.Name = "labelSearchLocation";
			this.labelSearchLocation.Size = new System.Drawing.Size(376, 24);
			this.labelSearchLocation.TabIndex = 7;
			this.labelSearchLocation.Text = "Where do you want to search?";
			this.labelSearchLocation.Visible = false;
			// 
			// listViewSearch
			// 
			this.listViewSearch.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.listViewSearch.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.listViewSearch.FullRowSelect = true;
			this.listViewSearch.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listViewSearch.HideSelection = false;
			this.listViewSearch.HoverSelection = true;
			this.listViewSearch.Location = new System.Drawing.Point(8, 88);
			this.listViewSearch.MultiSelect = false;
			this.listViewSearch.Name = "listViewSearch";
			this.listViewSearch.Size = new System.Drawing.Size(400, 97);
			this.listViewSearch.SmallImageList = this.imageListSearch;
			this.listViewSearch.TabIndex = 8;
			this.listViewSearch.View = System.Windows.Forms.View.List;
			this.listViewSearch.Click += new System.EventHandler(this.shellListView1_Click);
			this.listViewSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listViewSearch_KeyPress);
			// 
			// imageListSearch
			// 
			this.imageListSearch.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListSearch.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListSearch.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// WebSearch
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 190);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listViewSearch,
																		  this.labelSearchLocation,
																		  this.labelSearchFor,
																		  this.textBox1});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "WebSearch";
			this.ShowInTaskbar = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Search";
			this.Load += new System.EventHandler(this.WebSearch_Load);
			this.Activated += new System.EventHandler(this.WebSearch_Activated);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.WebSearch_Paint);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WebSearch_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		#region Methods

		private void WebSearch_Activated(object sender, System.EventArgs e)
		{
			this.textBox1.Focus();
			this.textBox1.SelectAll();
		}

		private void WebSearch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Visible = false;
				e.Handled = true;
			}
		}

		private void WebSearch_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);

			e.Graphics.DrawString(labelSearchFor.Text, labelSearchFor.Font, new SolidBrush(labelSearchFor.ForeColor), new RectangleF(labelSearchFor.Location, labelSearchFor.Size));
			e.Graphics.DrawString(labelSearchLocation.Text, labelSearchLocation.Font, new SolidBrush(labelSearchLocation.ForeColor), new RectangleF(labelSearchLocation.Location, labelSearchLocation.Size));
		}

		private void WebSearch_Load(object sender, System.EventArgs e)
		{
//			this.Left = 100;
//			this.Top = 100;
		}

		#endregion

		private void shellListView1_Click(object sender, System.EventArgs e)
		{
			if (listViewSearch.SelectedItems.Count == 0)
				return;
			
			textBox1.SelectAll();

			WebBrowser browser = null;
			Status status = new Status("Searching...");
			this.Hide();
			status.Show();

			SearchListViewItem item = (SearchListViewItem)listViewSearch.SelectedItems[0];
			SearchConfigData searchConfigData = item.SearchConfigData;
			
			searchConfigData.SearchString = textBox1.Text;

			Data searchData = this.Data.Get(String.Format(searchConfigData.Name + " Results: {0}", textBox1.Text), searchConfigData, searchConfigData.GetType());

			browser = new WebBrowser(searchData);

			// Set the browser icon if we have one
			if (searchConfigData.icon != null)
			{
				browser.Icon = searchConfigData.icon;
			}

			browser.Show();

			status.Hide();
			status.Dispose();
		}

		private void listViewSearch_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == (char) 13)
			{
				shellListView1_Click(sender, e);
			}
		}

		private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == (char) 13)
			{
				shellListView1_Click(sender, e);
			}
		}
	}

	#region SearchListViewItem
	public class SearchListViewItem: ListViewItem
	{
		#region Constructor
		public SearchListViewItem(SearchConfigData searchConfigData, ImageList imageList)
		{
            this.Text				= searchConfigData.Name;
			this.searchConfigData	= searchConfigData;

			if (searchConfigData.icon != null)
			{
				imageList.Images.Add(searchConfigData.icon);
				this.ImageIndex = imageList.Images.Count - 1;
			}
		}
		#endregion

		#region Declares
		private SearchConfigData searchConfigData;
		#endregion

		#region Properties
		public SearchConfigData SearchConfigData
		{
			get 
			{
				return searchConfigData;
			}
		}
		#endregion
	}						
	#endregion		 
	#region SearchConfigData

	[Serializable]
	public class SearchConfigData: msn2.net.Configuration.ConfigData
	{
		#region Declares

		private string searchString;
		protected string title;
		protected string name;
		[NonSerialized()]
		[System.Xml.Serialization.XmlIgnore()]
		public System.Drawing.Icon icon;

		#endregion
		#region Constructors
		public SearchConfigData(): this("")
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			this.icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("msn2.net.Controls.Icons.WebSearch.ico"));		
		}

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

		/// <summary>
		/// Don't need this method, remove it and var
		/// </summary>
		private string Title
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

		public string Name
		{
			get 
			{
				return name;
			}
			set
			{
				name = value;
			}
		}
//		public System.Drawing.Icon Icon
//		{
//			get
//			{
//				return icon;
//			}
//			set
//			{
//				this.icon = value;
//			}
//		}
		#endregion
	}

	#endregion
	#region WebSearchConfigData

	public class WebSearchConfigData: SearchConfigData
	{
		#region Declares

		private GoogleSearchService googleSearch;
		private bool initialSearchComplete = false;

		#endregion
		#region Constructors
		public WebSearchConfigData(): this("")
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			this.icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("msn2.net.Controls.Icons.Google.ico"));
		}

		public WebSearchConfigData(string t): base(t)
		{
			title = String.Format("Google Search Results: '{0}'", t);
			name	= "Google";
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

			// If initial search, we may want to open first result
			if (!initialSearchComplete)
			{
				initialSearchComplete = true;
                
				if (result.resultElements.Length > 0)
				{
					ResultElement first = result.resultElements[0];
					
				}

			}
			
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

		public CustomWebSearchConfigData(): this("")
		{
		}

		public CustomWebSearchConfigData(string searchString): base(searchString)
		{
			name = "Custom Search";
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
		public GoogleGroupsSearchConfigData(): this("")
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			this.icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("msn2.net.Controls.Icons.Google.ico"));
		}

		public GoogleGroupsSearchConfigData(string searchString): base(searchString)
		{
			title					= String.Format("Google Groups Results: '{0}'", searchString);
			name					= "Google Groups";
			
			this.searchUrl			= "http://groups.google.com/groups?hl=en&q={0}";
			this.resultUrlRegEx		= @"(http://groups.google.com/groups\?(.)*((selm=(.)*)|(threadm=(.)*))(.)*)";
			this.moreDataUrlRegEx	= @"(http://groups.google.com/groups\?(.)*((start=(.)*)|(sa=(.)*)|(group=(.)*))(.)*)";
		}
	}

	#endregion
	#region AllRecipesSearchConfigData
	
	public class AllRecipesSearchConfigData: CustomWebSearchConfigData
	{
		public AllRecipesSearchConfigData(): this("")
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			this.icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("msn2.net.Controls.Icons.allrecipes.ico"));
		}

		public AllRecipesSearchConfigData(string searchString): base(searchString)
		{
			title					= String.Format("allrecipes.com Search Results: '{0}'", searchString);
			name					= "allrecipes.com recipes";
			this.searchUrl			= "http://search.allrecipes.com/SearchResults.asp?site=allrecipes&allrecipes=allrecipes&q1={0}&Search+Allrecipes%21.x=2&Search+Allrecipes%21.y=8";
			this.moreDataUrlRegEx	= @"(http://search.allrecipes.com/searchresults.asp)";
		}
	}

	#endregion
	#region EpicuriosSearchConfigData
	
	public class EpicuriosSearchConfigData: CustomWebSearchConfigData
	{
		public EpicuriosSearchConfigData(): this("")
		{}

		public EpicuriosSearchConfigData(string searchString): base(searchString)
		{
			title					= String.Format("Epicurious Search Results: '{0}'", searchString);
			name					= "Epicurious";
			
			this.searchUrl			= "http://www.epicurious.com/s97is.vts?action=filtersearch&filter=recipe-filter.hts&collection=Recipes&ResultTemplate=recipe-results.hts&queryType=and&keyword={0}";
			this.moreDataUrlRegEx	= @"(http://www.epicurious.com/s97is.vts\?)(.)*(ResultStart=)(.)*";
		}
	}

	#endregion
	#region YahooSearchConfigData
	
	public class YahooSearchConfigData: CustomWebSearchConfigData
	{
		public YahooSearchConfigData(): this("")
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			this.icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("msn2.net.Controls.Icons.Yahoo.ico"));            		
		}

		public YahooSearchConfigData(string searchString): base(searchString)
		{
			name					= "Yahoo";
			
			this.searchUrl			= "http://search.yahoo.com/bin/search?p={0}";
			this.moreDataUrlRegEx	= @"(http://google.yahoo.com/bin/query\?p=)(.)*(hs=)(.)*";
		}
	}

	#endregion
	#region MSDNSearchConfigData
	
	public class MSDNSearchConfigData: CustomWebSearchConfigData
	{
		public MSDNSearchConfigData(): this("")
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			this.icon = new System.Drawing.Icon(assembly.GetManifestResourceStream("msn2.net.Controls.Icons.MSDN.ico"));            		
		}

		public MSDNSearchConfigData(string searchString): base(searchString)
		{
			name					= "MSDN Online";
			
			this.searchUrl			= "http://search.microsoft.com/default.asp?qu={0}&boolean=ALL&nq=NEW&so=RECCNT&p=1&ig=01&ig=02&ig=03&ig=04&ig=05&ig=06&i=00&i=01&i=02&i=03&i=04&i=05&i=06&i=07&i=08&i=09&i=10&i=11&i=12&i=13&i=14&i=15&i=16&i=17&i=18&i=19&i=20&i=21&i=22&i=23&i=24&i=25&i=26&i=27&i=28&i=29&i=30&i=31&i=32&i=33&i=34&i=35&i=36&i=37&i=38&i=39&i=40&i=41&i=42&i=43&i=44&i=45&i=46&i=47&i=48&i=49&i=50&i=51&siteid=us/dev";
			this.moreDataUrlRegEx	= @"(http://search.microsoft.com/gomsuri.asp\?)(.)*(c=rp_NextResults)(.)*";
		}
	}

	#endregion
	#region MicrosoftKbSearchConfigData
	
	public class MicrosoftKbSearchConfigData: CustomWebSearchConfigData
	{
		public MicrosoftKbSearchConfigData(): this("")
		{}

		public MicrosoftKbSearchConfigData(string searchString): base(searchString)
		{
			name					= "Microsoft Knowledge Base";
			
			this.searchUrl			= "http://search.support.microsoft.com/search/default.aspx?Catalog=LCID%3D1033%26CDID%3DEN-US-KB%26PRODLISTSRC%3DON&Product=msall&Query={0}&Queryc={0}&REF=false&srchstep=0&KeywordType=ALL&Titles=false&numDays=&maxResults=50";
			this.resultUrlRegEx		= @"(http://support.microsoft.com/default.aspx\?)(.)*(scid=kb;en-us;Q([0-9])*)";

			//this.moreDataUrlRegEx	= @"(http://search.microsoft.com/gomsuri.asp\?)(.)*(c=rp_NextResults)(.)*";
		}
	}

	#endregion
	#region CommunitySearchConfigData

	public class CommunitySearchConfigData: SearchConfigData
	{
		public CommunitySearchConfigData(): this("")
		{}

		public CommunitySearchConfigData(string t): base(t)
		{
			name	= "Community";
		}
		
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

