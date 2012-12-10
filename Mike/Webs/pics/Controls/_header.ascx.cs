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
				headerTable.Height = 30;

				// Add first row
				TableRow tr = new TableRow();
#if DEBUG
				tr.BorderWidth = 1;
#endif
				tr.Height = 30;
				headerTable.Rows.Add(tr);
                
				// left spacing cell
				TableCell tc = new TableCell();
				tc.Width = 17;
				tc.Height = 30;
				tr.Cells.Add(tc);

				// msn2 image cell
				tc = new TableCell();
				tc.Width				= 90;
				tc.Height				= 30;
				tr.Cells.Add(tc);

				// msn2 image
				HyperLink lnkmsn2		= new HyperLink();
				lnkmsn2.NavigateUrl		= "http://www.msn2.net";
				HtmlImage msn2image		= new HtmlImage();
				msn2image.Src			= strAppPath + "images/msn2summer.gif";
				msn2image.Height		= 27;
				msn2image.Width			= 90;
				msn2image.Border		= 0;
				lnkmsn2.Controls.Add(msn2image);
				tc.Controls.Add(lnkmsn2);

				// image right spacing cell
				tc						= new TableCell();
				tc.Width				= 17;
				tc.Height				= 30;
				tr.Cells.Add(tc);

				// Cell with header
				// Add cell with page title if applicable
				TableCell tcTitle = new TableCell();
				tcTitle.Height = 30;
				tcTitle.VerticalAlign = VerticalAlign.Bottom;
				tr.Cells.Add(tcTitle);

				// Create page title label
				if (mstrHeader != null && mstrHeader.Length > 0) 
				{
					Label lbl = new Label();
					lbl.Font.Bold = true;
					lbl.Font.Name	= "Tahoma";
					lbl.Font.Size = FontUnit.Large;
					lbl.ForeColor	= Color.LightGray;
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
#if DEBUG
					tLoginInfo.BorderWidth = 1;
#endif
					tLoginInfo.Height	= Unit.Percentage(100);
					tLoginInfo.Width	= Unit.Percentage(100);
					tc.Controls.Add(tLoginInfo);

					// top row will have nav links
					tr = new TableRow();
#if DEBUG
					tr.BorderWidth = 1;
#endif
					tr.Height = 30;
					tLoginInfo.Rows.Add(tr);
					tc = new TableCell();
					tc.HorizontalAlign = HorizontalAlign.Right;
					tc.Height = 30;
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
					tc					= new TableCell();
					tc.Height			= 30;
					tc.VerticalAlign	= VerticalAlign.Top;
					tc.HorizontalAlign	= HorizontalAlign.Right;
					tr.Cells.Add(tc);

					// create user name
					Label lblUserName	= new Label();
					lblUserName.Font.Size = 8;
					lblUserName.Text	= pi.Name + "<br>";
					lblUserName.ForeColor = Color.LightGray;
					tc.Controls.Add(lblUserName);

					// Create logout link
					HyperLink lnkLogout	= new HyperLink();
					lnkLogout.Text		= "Sign Out";
					lnkLogout.CssClass	= "headerLink";
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
