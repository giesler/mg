using System;
using System.Collections;
using System.Collections.Generic;
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
using msn2.net.Pictures;

namespace pics
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public partial class Categories : Page
	{
		#region Declares

		protected int rootCategoryId;
		protected int currentCategoryId;
		protected int startRecord;
		protected bool showEditControls;
		protected System.Web.UI.WebControls.HyperLink lnkSlideshow;
		protected System.Web.UI.WebControls.Label currentCategory;
		protected System.Web.UI.WebControls.DataList subCategories;
		protected System.Web.UI.WebControls.Label categoriesInLabel;
		protected pics.Controls.ContentPanel pictureTaskPanel;
		protected pics.Controls.PngImage slideshowImage;
		protected System.Web.UI.WebControls.HyperLink addToFolder;
		protected System.Web.UI.WebControls.HyperLink setCategoryPic;
		protected bool editMode;
        private DropDownList sortByField;
        private DropDownList sortOrder;

		#endregion

        private int GetCurrentSortFieldId()
        {
            return this.sortByField.SelectedIndex;
        }

		private void Page_Load(object sender, System.EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["sf"] != null)
                {
                    int sortFieldIndex = int.Parse(Request.QueryString["sf"]);
                    this.sortByField.SelectedIndex = sortFieldIndex;
                }

                if (Request.QueryString["so"] != null)
                {
                    int sortOrderIndex = int.Parse(Request.QueryString["so"]);
                    this.sortOrder.SelectedIndex = sortOrderIndex;
                }
            }

            showEditControls = Global.AdminMode;

			// Figure out what to use as the root ID
			if (Request.QueryString["r"] == null)
			{
				rootCategoryId	= 1;
			}
			else
			{
				rootCategoryId	= int.Parse(Request.QueryString["r"]);
			}
			if (Request.QueryString["c"] == null)
			{
				currentCategoryId	= rootCategoryId;
			}
			else
			{
				currentCategoryId	= int.Parse(Request.QueryString["c"]);
			}

			// Load the details of this category
			CategoryManager catManager		= PicContext.Current.CategoryManager;
			CategoryInfo currentCategory 		= catManager.GetCategory(currentCategoryId);
			if (currentCategory == null)
			{
				throw new ApplicationException("The category specified in the querystring does not exist or you do not have permission to view it.");
			}

			// Find out if in edit mode
			editMode = (Request.QueryString["mode"] != null && Request.QueryString["mode"] == "edit");

			// Get the category table
            List<CategoryInfo> categories = PicContext.Current.CategoryManager.GetCategories(currentCategoryId, 1);

			int columns				= 2;

			Table t = new Table();
			t.CellPadding	= 2;
			t.CellSpacing	= 2;
			t.Width			= Unit.Percentage(100);
			childCategoryList.Controls.Add(t);

			TableRow catRow			= null;
            
			// fill the child control list with links
			int i = 0;
			foreach (CategoryInfo category in new EricUtility.Iterators.IterReverse(categories))
			{
				//BreakGroup( t);

				if (i % columns == 0)
				{
					catRow		= new TableRow();
					t.Rows.Add(catRow);
				}

				string catNavUrl = "Categories.aspx?r=" + rootCategoryId + "&c=" + category.CategoryId.ToString();
				
				pics.Controls.CategoryListViewItem lvi = new pics.Controls.CategoryListViewItem(category.CategoryId, catNavUrl);
				lvi.FolderWidth		= 150;
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
                picTasks.Visible = true;
            }
            else
            {
                toolbarPanel.Visible = false;
            }

			// Now load the 'you are here' control
            List<CategoryInfo> cc = new List<CategoryInfo>();
			CategoryInfo parentCategory = currentCategory;
			while (parentCategory.CategoryId != rootCategoryId) 
			{
				parentCategory	= catManager.GetCategory(parentCategory.ParentId);

				// create a view to find the parent
				cc.Add(parentCategory);
			}
			
			Table tr = new Table();
			tr.CellPadding = 0;
			tr.Width	= Unit.Percentage(100);
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
			foreach (CategoryInfo category in new EricUtility.Iterators.IterReverse(cc))
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
				link.NavigateUrl		= "Categories.aspx?r=" + rootCategoryId.ToString() + "&c=" + category.CategoryId.ToString();
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
			t2.Width	= Unit.Percentage(100);
			currentCategoryCell.Controls.Add(t2);

			TableRow t2r1 = new TableRow();
			t2.Rows.Add(t2r1);

			TableCell t2r1c1 = new TableCell();
			t2r1c1.Width		= Unit.Pixel(10);
			t2r1c1.RowSpan   = 2;
			t2r1c1.VerticalAlign	= VerticalAlign.Top;
			t2r1.Cells.Add(t2r1c1);

			if (currentCategory.PictureId != 0)
			{
				//t2r1.Controls.Add(new HtmlLiteral("pic: " + currentCategory.PictureId.ToString()));
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

			// Add edit link
			TableCell t2r2c3	= new TableCell();
			t2r2c3.HorizontalAlign	= HorizontalAlign.Right;
			t2r1.Cells.Add(t2r2c3);

			if (showEditControls)
			{
				int personId						= PicContext.Current.CurrentUser.Id;
				t2r2c3.Width						= Unit.Pixel(50);
				CategoryEditFormLink editCat		= new CategoryEditFormLink(currentCategory.CategoryId, personId);
				t2r2c3.Controls.Add(editCat);
			}

			TableRow t2r2 = new TableRow();
			t2.Rows.Add(t2r2);

			// Add the description
			TableCell t2r1c3	= new TableCell();
			t2r1c3.CssClass		= "categoryDesc";
			t2r1c3.ColumnSpan	= 2;
			t2r2.Cells.Add(t2r1c3);
			if (currentCategory.Description != null)
			{
				t2r1c3.Controls.Add(new HtmlLiteral(currentCategory.Description));
			}

			if (showEditControls)
			{
				string groups = "";
				string [] groupNames = PicContext.Current.CategoryManager.GetCategoryGroups(
                    currentCategory.CategoryId);
				foreach (string groupName in groupNames)
				{
					if (groups.Length > 0) groups += ", ";
					groups += groupName;
				}
				groups = "Groups: " + groups + "<<br /> />";
				if (t2r1c3.Controls.Count > 0) 
				{
					groups = "<<br /> />" + groups;
				}
				t2r1c3.Controls.Add(new HtmlLiteral(groups));
			}

			// Add the tasklist
//			TaskListControl tl	= (TaskListControl) LoadControl(@"Controls/TaskListControl.ascx");
//			Sidebar1.Controls.Add(tl);
				

        }

        private void AddToolbarControls()
        {
            this.toolbarPanel.Controls.Add(new HtmlLiteral("Sort by: "));

            this.sortByField = new DropDownList();
            this.sortByField.ID = "sortByField";
            this.sortByField.CssClass = "toolbarItem";
            this.sortByField.AutoPostBack = true;
            this.toolbarPanel.Controls.Add(this.sortByField);

            this.sortByField.Items.Add(new ListItem("Picture Date", "0"));
            this.sortByField.Items.Add(new ListItem("Date Picture Added", "1"));

            this.sortByField.Items[0].Selected = true;

            this.sortByField.SelectedIndexChanged += new EventHandler(this.OnSortByFieldChanged);

            this.sortOrder = new DropDownList();
            this.sortOrder.CssClass = "toolbarItem";
            this.sortOrder.ID = "sortOrder";
            this.sortOrder.AutoPostBack = true;
            this.toolbarPanel.Controls.Add(this.sortOrder);

            this.sortOrder.Items.Add(new ListItem("Ascending", "0"));
            this.sortOrder.Items.Add(new ListItem("Descending", "1"));

            this.sortOrder.Items[0].Selected = true;

            this.sortOrder.SelectedIndexChanged += new EventHandler(this.OnSortOrderChanged);
        }


        private void Page_Init(object sender, EventArgs e)
		{
			InitializeComponent();

            // Add toolbar controls
            this.AddToolbarControls();

            //Sidebar1.SelectedWindow.OnSelectAll += new EventHandler(SelectedWindow_OnSelectAll);

        }

		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion

        public static PictureSortField GetSortFieldById(int id)
        {
            // also in picview.aspx.cs


            PictureSortField sortField = PictureSortField.DatePictureTaken;
            switch (id)
            {
                case 0:
                    sortField = PictureSortField.DatePictureTaken;
                    break;
                case 1:
                    sortField = PictureSortField.DatePictureAdded;
                    break;
                case 2:
                    sortField = PictureSortField.DatePictureUpdated;
                    break;
                default:
                    throw new ApplicationException("The sort order querystring variable value for 'sf' was not recognized");
            }

            return sortField;
        }

		private void LoadPictures()
		{
			// see if a start record is set in QS - this would override anything passed
			if (Request.QueryString["sr"] != null)
				startRecord = Convert.ToInt32(Request.QueryString["sr"]);

            PictureSortField sortField = Categories.GetSortFieldById(this.GetCurrentSortFieldId());

            PictureSortOrder sortOrder = PictureSortOrder.SortAscending;
            if (this.sortOrder.SelectedIndex != 0)
            {
                sortOrder = PictureSortOrder.SortDescending;
            }

            // make sure intStartRecord is at least 1
			if (startRecord < 1) startRecord = 1;

			int categoryId		= Convert.ToInt32(currentCategoryId);
			int count			= 0;
			int pageSize		= 20;
			DataSet dsPics		= PicContext.Current.PictureManager.GetPictures(categoryId, startRecord, 
                pageSize, 125, 125, sortField, sortOrder, ref count); 
            
			// create new control
			PagedThumbnailList thumbs		= new PagedThumbnailList();
			thumbs.ShowRecordNumber			= false;
			thumbs.ShowCheckBox				= showEditControls;
			thumbs.PictureCheckBoxClicked	+= new PictureCheckBoxClickedEventHandler(PictureCheckBoxClicked);
			thumbs.PageReturnURL			= PicPageURL;
			thumbs.ThumbsDataSource			= dsPics.Tables["Pictures"].DefaultView;
			thumbs.TotalRecords				= count;
			thumbs.StartRecord				= startRecord;
			thumbs.RecordsPerPage			= pageSize;

			// if there are children categories, point to that if no pics here
            if (childCategoryList.Visible)
            {
                thumbs.Visible = false; //.NoPictureMessage	= "<b>Please select a category from the left.</b>";
            }
            else
            {
                thumbs.NoPictureMessage = "<b>There are no pictures currently available in this category.</b><<br />>"
                    + "We may have added the category and not published pictures yet, so please check back later.";
            }                
             
            thumbs.PageNavUrl = Request.Path + "?r=" + rootCategoryId + "&c=" + currentCategoryId + "&sr={0}"
                                                        + "&sf=" + this.sortByField.SelectedIndex.ToString()
                                                        + "&so=" + this.sortOrder.SelectedIndex.ToString();
            pnlthumbs.Controls.Add(thumbs);

			// Show the slideshow link if there are pictures
			if (thumbs.HasPictures) 
			{
				picTasks.Visible			= true;
				picTaskList.SetSlideshowUrl(String.Format(PicPageURL + "&ss=1#title", startRecord.ToString()));
			}

		}

		private void PictureCheckBoxClicked(object sender, PictureCheckBoxEventArgs e)
		{
			// Ensure selected list visible
			Sidebar1.SelectedWindow.Visible	= true;
            
			Sidebar1.SelectedWindow.ContentPanel.AddContent(new HtmlLiteral("<hr style=\"border:1px\">"));

			PictureIdCollection mySelectedList	= Global.SelectedPictures;
			if (e.Checked)
			{
				mySelectedList.Add(e.PicId);
				Label added = new Label();
				added.Text	= "- Added 1 pic<<br />>";
				added.Font.Size = FontUnit.Parse("8pt");
				Sidebar1.SelectedWindow.ContentPanel.AddContent(added);
			}
			else
			{
				mySelectedList.Remove(e.PicId);
				Label removed = new Label();
				removed.Text	= "- Removed 1 pic<<br />>";
				removed.Font.Size = FontUnit.Parse("8pt");
				Sidebar1.SelectedWindow.ContentPanel.AddContent(removed);
			}

            PictureImageControl pic = new PictureImageControl();
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

		private void picTasks_SlideshowTask(object sender, System.EventArgs e)
		{
		
		}

		public String PicPageURL 
		{
			get 
			{

				String strURL = "picview.aspx?r={0}&c=" + currentCategoryId + "&type=category"
                                                        + "&sf=" + this.sortByField.SelectedIndex.ToString()
                                                        + "&so=" + this.sortOrder.SelectedIndex.ToString()
                 + "&RefURL=" + Server.UrlEncode(Request.FilePath + "?r=" + rootCategoryId 
														+ "&c=" + currentCategoryId + "&sr=" + startRecord.ToString()
                                                        + "&sf=" + this.sortByField.SelectedIndex.ToString()
                                                        + "&so=" + this.sortOrder.SelectedIndex.ToString());
				return strURL;
			}

		}

		private void SelectedWindow_OnSelectAll(object sender, EventArgs e)
		{
			int categoryId		= Convert.ToInt32(currentCategoryId);
			int count			= 0;
			PictureIdCollection mySelectedList	= Global.SelectedPictures;

			// Get current category pic ids
			DataSet dsPics		= PicContext.Current.PictureManager.GetPictures(categoryId, startRecord, 
                10000, 125, 125, PictureSortField.DatePictureTaken, PictureSortOrder.SortAscending, ref count); 

			foreach (DataRow dr in dsPics.Tables[0].Rows)
			{
				int pictureId = (int) dr["PictureId"];
				mySelectedList.Add(pictureId);
			}

			Label added = new Label();
			added.Text	= "- Added all pic<<br />>";
			added.Font.Size = FontUnit.Parse("8pt");
			Sidebar1.SelectedWindow.ContentPanel.AddContent(added);

		}

        void OnSortByFieldChanged(object sender, EventArgs e)
        {
        }

        void OnSortOrderChanged(object sender, EventArgs e)
        {
        }
    }
}
