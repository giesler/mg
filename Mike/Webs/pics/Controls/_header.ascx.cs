namespace pics.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Web.Security;

	/// <summary>
	///		Summary description for C_header.
	/// </summary>
	public abstract class C_header : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label lblHeader;
		protected System.Web.UI.WebControls.Table headerTable;

		protected String mstrSize;
		protected String mstrHeader;
		protected bool _showUserInfo = true;

		/// <summary>
		public C_header()
		{
			this.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			String strAppPath = Request.ApplicationPath;
			if (!strAppPath.Equals("/"))  
				strAppPath = strAppPath + "/";
			
			// Set a default size
			if (mstrSize == null)
				mstrSize = "small";

			if (mstrSize.Equals("small")) 
			{
				headerTable.Height = 75;
				headerTable.Attributes["background"] = strAppPath + "Images/msn2_small_panel.jpg";

				// Add first row
				TableRow tr = new TableRow();
				tr.Height = 37;
				headerTable.Rows.Add(tr);
                
				// left cell
				TableCell tc = new TableCell();
				tc.Attributes["background"] = strAppPath + "Images/msn2_small.jpg";
				tc.Width = 600;
				tc.Height = 75;
				tr.Cells.Add(tc);

				// new table within cell with header
				Table innerTable = new Table();
				innerTable.CellPadding = 0;
				innerTable.CellSpacing = 0;
				innerTable.Width	   = Unit.Percentage(100);
				tc.Controls.Add(innerTable);
				TableRow trInner = new TableRow();
				innerTable.Rows.Add(trInner);
				TableCell tcNeedle = new TableCell();
				tcNeedle.Height = 75;
				tcNeedle.Width = 32;
				tcNeedle.RowSpan = 2;
				trInner.Cells.Add(tcNeedle);
				
				// Create false images in 'needle' cell for linking
				HyperLink lnk = new HyperLink();
				lnk.NavigateUrl = "http://www.msn2.net/";
				HtmlImage htmlImage = new HtmlImage();
				htmlImage.Src    = strAppPath + "images/trans.gif";
				htmlImage.Height = 75;
				htmlImage.Width  = 32;
				htmlImage.Border = 0;
				lnk.Controls.Add(htmlImage);
				tcNeedle.Controls.Add(lnk);

				// new cell for 'msn2' image for clicking
				TableCell tcMSN2 = new TableCell();
				tcMSN2.Height = 35;
				tcMSN2.ColumnSpan = 2;
				trInner.Cells.Add(tcMSN2);
				
				// MSN2 fake link
				lnk = new HyperLink();
				lnk.NavigateUrl = "http://www.msn2.net/";
				htmlImage = new HtmlImage();
				htmlImage.Src    = strAppPath + "images/trans.gif";
				htmlImage.Height = 35;
				htmlImage.Width  = 80;
				htmlImage.Border = 0;
				lnk.Controls.Add(htmlImage);
				tcMSN2.Controls.Add(lnk);

				// Second row
				trInner = new TableRow();
				innerTable.Rows.Add(trInner);

				// make sure this row's height is right
				TableCell tcTemp = new TableCell();
				tcTemp.Height = 40;
				htmlImage = new HtmlImage();
				htmlImage.Height = 40;
				htmlImage.Width  = 2;
				htmlImage.Src    = strAppPath + "Images/trans.gif";
				tcTemp.Controls.Add(htmlImage);
				trInner.Cells.Add(tcTemp);

				// Cell with header
				// Add cell with page title if applicable
				TableCell tcTitle = new TableCell();
				tcTitle.Height = 40;
				tcTitle.VerticalAlign = VerticalAlign.Top;
				trInner.Cells.Add(tcTitle);

				// Create page title label
				if (mstrHeader != null && mstrHeader.Length > 0) 
				{
					Label lbl = new Label();
					lbl.Font.Bold = true;
					lbl.Text = mstrHeader;
					tcTitle.Controls.Add(lbl);
				}

				// Add right cell to main table
				tc = new TableCell();
				tc.Text = "&nbsp;";
				tr.Cells.Add(tc);

				
				
				// Now we want to create the login info in the right cell
				if (_showUserInfo && Request.IsAuthenticated) 
				{
					// load details on the logged in person
					PersonInfo pi = (PersonInfo) Session["PersonInfo"];

                    Table tLoginInfo = new Table();
					tLoginInfo.Height	= Unit.Percentage(100);
					tLoginInfo.Width	= Unit.Percentage(100);
					tc.Controls.Add(tLoginInfo);

					// top row will have nav links
					tr = new TableRow();
					tr.Height = 35;
					tLoginInfo.Rows.Add(tr);
					tc = new TableCell();
					tc.HorizontalAlign = HorizontalAlign.Right;
					tc.Height = 35;
					tc.Text = "&nbsp;";
					tr.Cells.Add(tc);

					// add link to home
                    HyperLink lnkHome = new HyperLink();
					lnkHome.NavigateUrl = "http://" + Request.Url.Host + strAppPath;
					tc.Controls.Add(lnkHome);

					// Add image to the link
					HtmlImage homeImage = new HtmlImage();
                    homeImage.Src		= strAppPath + "images/msn2_home.gif";
					homeImage.Border	= 0;
					homeImage.Alt		= "To Picture home page";
					lnkHome.Controls.Add(homeImage);

					// last cell will have login name and logout link
					tr					= new TableRow();
					tr.Height			= 40;
					tLoginInfo.Rows.Add(tr);
					tc					= new TableCell();
					tc.Height			= 40;
					tc.VerticalAlign	= VerticalAlign.Top;
					tc.HorizontalAlign	= HorizontalAlign.Right;
					tr.Cells.Add(tc);

					// create user name
					Label lblUserName	= new Label();
					lblUserName.Font.Size = 8;
					lblUserName.Text	= pi.Name + "<br>";
					tc.Controls.Add(lblUserName);

					// Create logout link
					HyperLink lnkLogout	= new HyperLink();
					lnkLogout.Text		= "Logout";
					lnkLogout.Font.Size	= 8;
					lnkLogout.NavigateUrl = strAppPath + "Auth/Logout.aspx";
					tc.Controls.Add(lnkLogout);

				}


			}			

		}

		private void Page_Init(object sender, EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}

		public String Header
		{
			set 
			{
				mstrHeader = value;
			}
		}
        
		
		public String Size 
		{
			set 
			{
				mstrSize = value;
			}
		}

		public bool ShowUserInfo 
		{
			set 
			{
				_showUserInfo = value;
			}
		}

		#region Web Form Designer generated code
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
