using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace pics.Controls
{
	/// <summary>
	/// Summary description for ThumbnailListPaging.
	/// </summary>
	public class ThumbnailListPagingControl: Control, INamingContainer
	{
		#region Declares
		protected int totalRecords;
		protected int startRecord;
		protected int recordsPerPage;
		protected String pageNavURL;
		#endregion

		#region Constructor
		public ThumbnailListPagingControl(string pageNavUrl, int recordsPerPage, int startRecord, int totalRecords): base()
		{
			this.pageNavURL		= pageNavUrl;
			this.recordsPerPage	= recordsPerPage;
			this.startRecord	= startRecord;
			this.totalRecords	= totalRecords;
		}
		#endregion

		#region Overriden Methods
		protected override void CreateChildControls()
		{
			
			// figure out number of pages we have
			int pages = totalRecords / recordsPerPage ;
			int page  = (startRecord / recordsPerPage) + 1;	// +1 to compensate for 0 being 1
			if (totalRecords % recordsPerPage != 0) // if we have some odd pics left over, that makes another page
				pages++;

			// display page controls if more then one page
			if (pages > 0) 
			{
				// Page controls table
				Table t					= new Table();
				t.Width					= Unit.Percentage(100);
				t.Height				= Unit.Pixel(25);
				t.CellPadding			= 0;
				t.CellSpacing			= 0;
				this.Controls.Add(t);

				TableRow tr = new TableRow();
				t.Rows.Add(tr);

				// Previous link cell
				TableCell tc			= new TableCell();
				tc.CssClass				= "pipeLeft";
				tc.Width				= Unit.Pixel(25);
				tr.Cells.Add(tc);

				// If not at the beginning, add a previous link
				if (page > 1) 
				{
					HyperLink lnk = new HyperLink();
					lnk.Font.Bold = true;
					lnk.NavigateUrl = String.Format(pageNavURL, (startRecord - recordsPerPage));
                    lnk.BorderWidth = 0;
                    lnk.BorderStyle = BorderStyle.None;
                    tc.Controls.Add(lnk);

					Image img			= new Image();
					img.ImageUrl		= "Images/button_left.gif";
					img.ImageAlign		= ImageAlign.Right;
                    img.BorderStyle = BorderStyle.None;
                    lnk.Controls.Add(img);
				}

				// Now show the page and page count in the middle
				TableCell tcPage		= new TableCell();
				tcPage.CssClass			= "pipeCenter";
				tr.Cells.Add(tcPage);

				// 'Page ' prefix
				Literal litPage = new Literal();
				litPage.Text	= "Page ";
				tcPage.Controls.Add(litPage);

				// Loop through the pages we have
				HttpContext.Current.Trace.Write("PagedThumbnailList: pageNavURL", pageNavURL);
				for (int i = 1; i <= pages; i++) 
				{
					// if we are on the i'th page, show page in bold
					if (page == i) 
					{
						Label lbl = new Label();
						lbl.Text = i.ToString();
						lbl.Font.Bold = true;
						tcPage.Controls.Add(lbl);
					}
					else 
					{
						HyperLink lnk		= new HyperLink();
						lnk.Text			= i.ToString();
						lnk.CssClass		= "pipeCenterLink";
						lnk.NavigateUrl		= String.Format(pageNavURL, (i-1)*recordsPerPage + 1);
						tcPage.Controls.Add(lnk);
					}

					Literal l = new Literal();
					l.Text = "&nbsp;";
					tcPage.Controls.Add(l);
						
				}

				// Right next/prev cell
				tc					= new TableCell();
				tc.CssClass			= "pipeRight";
				tc.Width			= Unit.Pixel(25);
				tr.Cells.Add(tc);

				// If not at the end, add a next link
				if (page < pages) 
				{
					HyperLink lnk = new HyperLink();
					lnk.Font.Bold = true;
					lnk.NavigateUrl = String.Format(pageNavURL, (startRecord + recordsPerPage));
                    tc.Controls.Add(lnk);

					Image img			= new Image();
					img.ImageUrl		= "Images/button_right.gif";
                    img.BorderStyle = BorderStyle.None;
					lnk.Controls.Add(img);

				}

			}		
		}
		#endregion

		#region Public Properties

		public int TotalRecords 
		{
			set 
			{
				totalRecords = value;
			}
			get 
			{
				return totalRecords;
			}
		}

		public int StartRecord 
		{
			set 
			{
				startRecord = value;
			}
			get 
			{
				return startRecord;
			}
		}

		public int RecordsPerPage 
		{
			set 
			{
				recordsPerPage = value;
			}
			get 
			{
				return recordsPerPage;
			}
		}

		public String PageNavURL 
		{
			set 
			{
				pageNavURL = value;
			}
			get 
			{
				return pageNavURL;
			}
		}
		#endregion
	}
}
