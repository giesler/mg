using System;
using System.Web;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using msn2.net.Pictures;

namespace pics.Controls
{
	/// <summary>
	/// Summary description for Header.
/*
 * 
<asp:Table Runat="server" ID="headerTable" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" CssClass="msn2headerTable"></asp:Table>
<table width="100%" height="4" cellpadding="0" cellspacing="0" border="0">
	<tr>
		<td height="4" width="100%" class="msn2headerbottom"><img src="Images/blank.gif" width="1" height="3"></td>
	</tr>
</table>
*/
	/// </summary>
	public class Header: System.Web.UI.Control, System.Web.UI.INamingContainer
	{
		#region Declares
		protected System.Web.UI.WebControls.Label lblHeader;
		protected System.Web.UI.WebControls.Table headerTable;

		protected String mstrSize;
		protected String mstrHeader;
		protected bool _showUserInfo = true;
		#endregion
		public Header()
		{
			// Get HTTP context
			HttpContext httpContext		= HttpContext.Current;
			
			// If in design mode, just bail
			if (httpContext == null)
			{
				this.Controls.Add(new HtmlLiteral("This is the MSN2 header..."));
				return;
			}
			// If in 'cd' mode, do not show user info
			if (httpContext.Session["mode"] != null)
			{
				if (httpContext.Session["mode"].ToString() == "cd")
				{
					_showUserInfo = false;
				}
			}

			// Set up header table
			headerTable				= new Table();
			headerTable.CellPadding = 0;
			headerTable.CellSpacing	= 0;
			headerTable.Width		= Unit.Percentage(100);
			headerTable.CssClass	= "msn2headerTable";
			this.Controls.Add(headerTable);

			String strAppPath = httpContext.Request.ApplicationPath;
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
				tr.BorderWidth = 0;
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
				if (_showUserInfo && PicContext.Current.CurrentUser != null) 
				{
                    Table tLoginInfo = new Table();
					tLoginInfo.Height	= Unit.Percentage(100);
					tLoginInfo.Width	= Unit.Percentage(100);
					tc.Controls.Add(tLoginInfo);

					// top row will have nav links
					tr = new TableRow();
					tr.Height = 30;
					tLoginInfo.Rows.Add(tr);
					tc = new TableCell();
					tc.HorizontalAlign = HorizontalAlign.Right;
					tc.Height = 30;
					tc.Text = "&nbsp;";
					tr.Cells.Add(tc);

					// add link to home
//                    HyperLink lnkHome = new HyperLink();
//					lnkHome.NavigateUrl = "http://" + httpContext.Request.Url.Host + strAppPath;
//					tc.Controls.Add(lnkHome);

					// Add image to the link
//					HtmlImage homeImage = new HtmlImage();
//                    homeImage.Src		= strAppPath + "images/msn2_home.gif";
//					homeImage.Border	= 0;
//					homeImage.Alt		= "To Picture home page";
//					lnkHome.Controls.Add(homeImage);

					// last cell will have login name and logout link
					tc					= new TableCell();
					tc.Height			= 30;
					tc.VerticalAlign	= VerticalAlign.Top;
					tc.HorizontalAlign	= HorizontalAlign.Right;
					tr.Cells.Add(tc);

					// create user name
					Label lblUserName	= new Label();
					lblUserName.Font.Size = 8;
					lblUserName.Text	= PicContext.Current.CurrentUser.Name + "<br>";
					lblUserName.ForeColor = Color.LightGray;
					tc.Controls.Add(lblUserName);

					// Create logout link
					HyperLink lnkLogout	= new HyperLink();
					lnkLogout.Text		= "Sign Out";
					lnkLogout.CssClass	= "headerLink";
					lnkLogout.Font.Size	= 8;
					lnkLogout.NavigateUrl = strAppPath + "Auth/Logout.aspx";
					tc.Controls.Add(lnkLogout);

					bool showEditControls	= (bool) httpContext.Session["editMode"];
					if (PicContext.Current.CurrentUser.Id < 3 || PicContext.Current.CurrentUser.Id == 4)
					{
						tc.Controls.Add(new HtmlLiteral("&nbsp;|&nbsp;"));
						LinkButton adminMode	= new LinkButton();
						adminMode.Text			= (showEditControls ? "Edit Mode Off" : "Edit Mode On");
						adminMode.Click			+= new EventHandler(adminMode_Click);
						adminMode.CssClass		= "headerLink";
						adminMode.Font.Size		= 8;
						tc.Controls.Add(adminMode);
					}

					// Add impersonate link
					if (showEditControls)
					{
//						tc.Controls.Add(new HtmlLiteral("&nbsp;|&nbsp;"));
//						HyperLink lnkImper	= new HyperLink();
//						lnkImper.Text		= "Impersonate";
//						lnkImper.CssClass	= "headerLink";
//						lnkImper.Font.Size	= 8;
//						lnkImper.NavigateUrl = strAppPath + "Impersonate.aspx";
//						tc.Controls.Add(lnkImper);
					}
					


				}


			}			
		
		}
		#region Properties

		public string Text
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
		#endregion

		private void adminMode_Click(object sender, EventArgs e)
		{
			Global.AdminMode		= !Global.AdminMode;
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString());
		}
	}
}
