using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using pics.Controls;

namespace pics
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class Categories : System.Web.UI.Page
	{
		#region Declares

		protected string rootCategoryId;
		protected string currentCategoryId;
		protected int startRecord;
		protected System.Web.UI.WebControls.HyperLink lnkSlideshow;
		protected System.Web.UI.WebControls.Label currentCategory;
		protected System.Web.UI.WebControls.DataList subCategories;
		protected System.Web.UI.WebControls.Panel childCategoryList;
		protected System.Web.UI.WebControls.Label categoriesInLabel;
		protected System.Web.UI.WebControls.Panel youAreHerePanel;
		protected System.Web.UI.WebControls.Panel pnlthumbs;
		protected pics.Controls.Sidebar Sidebar1;
		protected pics.Controls.ContentPanel pictureTaskPanel;
		protected pics.Controls.PngImage slideshowImage;
		protected System.Web.UI.WebControls.HyperLink addToFolder;
		protected System.Web.UI.WebControls.HyperLink setCategoryPic;
		protected bool editMode;
		#endregion

		public Categories()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{

			// Figure out what to use as the root ID
			rootCategoryId = Request.QueryString["r"];
			currentCategoryId = Request.QueryString["c"];

			// Defaults for root and category ID
			if (rootCategoryId == null) 
				rootCategoryId = "1";
			if (currentCategoryId == null)
				currentCategoryId = rootCategoryId;

			// Load the details of this category
			CategoryManager catManager		= new CategoryManager();
			Category currentCategory 		= catManager.GetCategory(Convert.ToInt32(currentCategoryId));
			if (currentCategory == null)
			{
				throw new ApplicationException("The category specified in the querystring does not exist or you do not have permission to view it.");
			}

			// Find out if in edit mode
			editMode = (Request.QueryString["mode"] != null && Request.QueryString["mode"] == "edit");

			// Get the category table
			CategoryCollection categories = catManager.GetCateogies(Convert.ToInt32(currentCategoryId), 1);

			int columns				= 2;

			Table t = new Table();
			t.CellPadding	= 2;
			t.CellSpacing	= 2;
			t.Width			= Unit.Percentage(100);
			childCategoryList.Controls.Add(t);

			TableRow catRow			= null;

            
			// fill the child control list with links
			int i = 0;
			foreach (Category category in categories)
			{
				//BreakGroup( t);

				if (i % columns == 0)
				{
					catRow		= new TableRow();
					t.Rows.Add(catRow);
				}

				string catNavUrl = "Categories.aspx?r=" + rootCategoryId + "&c=" + category.CategoryId.ToString();
				
				pics.Controls.CategoryListViewItem lvi = new pics.Controls.CategoryListViewItem(category.CategoryId, catNavUrl);
				TableCell tc = new TableCell();
				tc.Controls.Add(lvi);
				catRow.Cells.Add(tc);

				i++;
			}

			// if no children, hide this control
			if (categories.Count == 0)
			{
				childCategoryList.Visible = false;
				LoadPictures();
			}

			// Now load the 'you are here' control
			CategoryCollection	cc	= new CategoryCollection();
			Category parentCategory = currentCategory;
			while (parentCategory.CategoryId != Convert.ToInt32(rootCategoryId)) 
			{
				parentCategory	= catManager.GetCategory(parentCategory.ParentCategoryId);

				// create a view to find the parent
				cc.Add(parentCategory);
			}
			
			Table tr = new Table();
			tr.CellPadding = 0;
			tr.CellSpacing = 0;
			youAreHerePanel.Controls.Add(tr);

			TableRow trc = new TableRow();
			tr.Rows.Add(trc);

			TableCell categoryCell = new TableCell();
			trc.Cells.Add(categoryCell);

			Table t1 = new Table();
			t1.CellPadding = 0;
			t1.CellSpacing = 0;
			categoryCell.Controls.Add(t1);

			TableRow r1 = new TableRow();
			t1.Rows.Add(r1);

			// Now output in reverse order
			int currentItem = 0;
			foreach (Category category in new EricUtility.Iterators.IterReverse(cc))
			{
				TableCell tc = new TableCell();
				tc.Width = Unit.Pixel(10);
				r1.Cells.Add(tc);

				PngImage folderImage = new PngImage("Images/folder12x12.png", 8, 8);
				tc.Controls.Add(folderImage);

				tc = new TableCell();
				tc.CssClass		= "categorySmallLink";
				r1.Cells.Add(tc);

				// create a link to this page
				HyperLink link			= new HyperLink();
				link.Text				= category.Name;
				link.CssClass			= "categorySmallLink";
				link.NavigateUrl		= "Categories.aspx?r=" + rootCategoryId + "&c=" + category.CategoryId.ToString();
				tc.Controls.Add(link);

				// add a divider if not at end
				currentItem++;
				if (currentItem != cc.Count)
				{
					Literal lit = new Literal();
					lit.Text = " \\ ";
					tc.Controls.Add(lit);
				}
			}

			trc = new TableRow();
			tr.Rows.Add(trc);

			TableCell currentCategoryCell = new TableCell();
			if (cc.Count > 1)
			{
				currentCategoryCell.ColumnSpan = cc.Count-1;
			}
			trc.Cells.Add(currentCategoryCell);

			Table t2 = new Table();
			currentCategoryCell.Controls.Add(t2);

			TableRow t2r1 = new TableRow();
			t2.Rows.Add(t2r1);

			TableCell t2r1c1 = new TableCell();
			t2r1c1.RowSpan   = 2;
			t2r1c1.VerticalAlign	= VerticalAlign.Top;
			t2r1.Cells.Add(t2r1c1);

			if (currentCategory.PictureId != 0)
			{
				t2r1.Controls.Add(new HtmlLiteral("pic: " + currentCategory.PictureId.ToString()));
			}
			PngImage folderImage1 = new PngImage("Images/folder12x12.png", 10, 10);
			t2r1c1.Controls.Add(folderImage1);

			TableCell t2r1c2	= new TableCell();
			t2r1.Cells.Add(t2r1c2);

			// And add the current category
			Label curCategory = new Label();
			curCategory.Text = currentCategory.Name;
			curCategory.Font.Bold = true;
			t2r1c2.Controls.Add(curCategory);

			TableRow t2r2 = new TableRow();
			t2.Rows.Add(t2r2);

			// Add the description
			TableCell t2r1c3	= new TableCell();
			t2r1c3.CssClass		= "categoryDesc";
			t2r2.Cells.Add(t2r1c3);
			if (currentCategory.Description != null)
			{
				t2r1c3.Controls.Add(new HtmlLiteral(currentCategory.Description));
			}
				
		}


		private void Page_Init(object sender, EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}

		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void LoadPictures()
		{
			// load the person's info
			PersonInfo pi = (PersonInfo) Session["PersonInfo"];

			// see if a start record is set in QS - this would override anything passed
			if (Request.QueryString["sr"] != null)
				startRecord = Convert.ToInt32(Request.QueryString["sr"]);

			// make sure intStartRecord is at least 1
			if (startRecord < 1) startRecord = 1;

			// init connection and command to get pictures
			SqlConnection cn  = new SqlConnection(Config.ConnectionString);

			// Set up SP to retreive pictures
			SqlDataAdapter daPics = new SqlDataAdapter("dbo.p_Category_GetPictures", cn);
			daPics.SelectCommand.CommandType = CommandType.StoredProcedure;

			// set up params on the SP
			daPics.SelectCommand.Parameters.Add("@CategoryID", Convert.ToInt32(currentCategoryId));
			daPics.SelectCommand.Parameters.Add("@StartRecord", startRecord);
			daPics.SelectCommand.Parameters.Add("@ReturnCount", 15);
			daPics.SelectCommand.Parameters.Add("@PersonID", pi.PersonID);
			daPics.SelectCommand.Parameters.Add("@MaxWidth", 125);
			daPics.SelectCommand.Parameters.Add("@MaxHeight", 125);
			daPics.SelectCommand.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
			daPics.SelectCommand.Parameters["@TotalCount"].Direction = ParameterDirection.Output;

            // run the SP, set datasource to the picture list
			cn.Open();
			DataSet dsPics = new DataSet();
			daPics.Fill(dsPics, "Pictures");
			cn.Close();

			// create new control
			PagedThumbnailList thumbs		= new PagedThumbnailList();
			thumbs.ShowRecordNumber			= true;
			thumbs.ShowCheckBox				= true;
			thumbs.PictureCheckBoxClicked	+= new PictureCheckBoxClickedEventHandler(PictureCheckBoxClicked);
			thumbs.PageReturnURL			= PicPageURL;
			thumbs.ThumbsDataSource			= dsPics.Tables["Pictures"].DefaultView;
			thumbs.TotalRecords				= Convert.ToInt32(daPics.SelectCommand.Parameters["@TotalCount"].Value);
			thumbs.StartRecord				= startRecord;
			thumbs.RecordsPerPage			= 15;

			// if there are children categories, point to that if no pics here
			if (childCategoryList.Visible)
				thumbs.Visible				= false; //.NoPictureMessage	= "<b>Please select a category from the left.</b>";
			else
				thumbs.NoPictureMessage = "<b>There are no pictures currently available in this category.</b><br>"
					+ "We may have added the category and not published pictures yet, so please check back later.";
			thumbs.PageNavURL		= Request.Path + "?r=" + rootCategoryId	+ "&c=" + currentCategoryId + "&sr={0}";
			pnlthumbs.Controls.Add(thumbs);

			// Show the slideshow link if there are pictures
			if (thumbs.HasPictures) 
			{
//				pictureTaskPanel.Visible = true;
//				lnkSlideshow.NavigateUrl = String.Format(PicPageURL + "&ss=1#title", startRecord.ToString());
			}

		}

		private void PictureCheckBoxClicked(object sender, PictureCheckBoxEventArgs e)
		{
			// Ensure selected list visible
//			selectedList.Visible		= true;

            
			Sidebar1.SelectedWindow.ContentPanel.AddContent(new HtmlLiteral("<hr style=\"border:1px\">"));

			PictureManager mgr = new PictureManager();

			// Add or remove from selected list
			PictureIdCollection mySelectedList	= (PictureIdCollection) Session["MySelectedList"];
			if (e.Checked)
			{
				mgr.AddToCategory(e.PicId, Sidebar1.SelectedWindow.CategoryId);
				
				mySelectedList.Add(e.PicId);
				Label added = new Label();
				added.Text	= "- Added 1 pic<br>";
				added.Font.Size = FontUnit.Parse("8pt");
				Sidebar1.SelectedWindow.ContentPanel.AddContent(added);
			
			}
			else
			{
				mgr.RemoveFromCategory(e.PicId, Sidebar1.SelectedWindow.CategoryId);

				mySelectedList.Remove(e.PicId);
				Label removed = new Label();
				removed.Text	= "- Removed 1 pic<br>";
				removed.Font.Size = FontUnit.Parse("8pt");
				Sidebar1.SelectedWindow.ContentPanel.AddContent(removed);
			}

			Picture pic = new Picture();
			pic.Width = 80;
			pic.SetPictureById(e.PicId, 125, 125);
			Sidebar1.SelectedWindow.ContentPanel.AddContent(pic);

			// Update caption with count
//			selectedCount.Text			= mySelectedList.Count.ToString();

//			ThumbnailList thumbs = new ThumbnailList();
//			thumbs.PageReturnURL	= "picview.aspx?p=" + e.PicId + "&type=added";
//			thumbs.ThumbsDataSource = dsPics.Tables["Pictures"].DefaultView;
			//			thumbs.PageNavURL		= "picview.aspx?p=" + dsPics.Tables["Pictures"].Rows[0]["PictureID"].ToString();
//			lastAdded.Controls.Add(thumbs);
//			lastAdded.Visible = true;

			

		}

		public String PicPageURL 
		{
			get 
			{

				String strURL = "picview.aspx?r={0}&c=" + currentCategoryId + "&type=category"
					+ "&RefURL=" + Server.UrlEncode(Request.FilePath + "?r=" + rootCategoryId 
														+ "&c=" + currentCategoryId + "&sr=" + startRecord.ToString());
				return strURL;
			}

		}

	}
}
