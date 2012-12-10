
namespace pics.Controls
{

	using System;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;
	using System.IO;
	using System.Drawing.Imaging;
	using System.Collections;
	using System.Text;
	using msn2.net.Pictures;

	#region Picture Display control
	/// <summary>
	///		Outputs an image tag referring to an image file created with max height and width
	/// </summary>
	public class Picture : System.Web.UI.Control, System.Web.UI.INamingContainer
	{
		private int width;
		private int height;
		private string filename;
		private string cssClass;
		private int pictureId;
		private int maxHeight;
		private int maxWidth;

		/// <summary>
		/// Creates a new Picture object
		/// </summary>
		public Picture()
		{}

		public void SetPictureById(int id, int maxHeight, int maxWidth)
		{
			this.pictureId		= id;
			this.maxHeight		= maxHeight;
			this.maxWidth		= maxWidth;

//			// load the person's info
//			PersonInfo pi = (PersonInfo) HttpContext.Current.Session["PersonInfo"];
//
//			SqlConnection cn = new SqlConnection(Config.ConnectionString);
//
//			// Set up SP to retreive picture
//			SqlCommand cmdPic    = new SqlCommand();
//			cmdPic.CommandText = "p_GetPicture";
//			cmdPic.CommandType   = CommandType.StoredProcedure;
//			cmdPic.Connection    = cn;
//			SqlDataAdapter daPic = new SqlDataAdapter(cmdPic);
//
//			// set up params on the SP
//			cmdPic.Parameters.Add("@PictureID", id);
//			cmdPic.Parameters.Add("@StartRecord", 0);
//			cmdPic.Parameters.Add("@ReturnCount", 1);
//			cmdPic.Parameters.Add("@MaxHeight", 125);
//			cmdPic.Parameters.Add("@MaxWidth", 125);
//			cmdPic.Parameters.Add("@PersonID", pi.PersonID);
//			cmdPic.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
//			cmdPic.Parameters["@TotalCount"].Direction = ParameterDirection.Output;
//
//			// run the SP, set datasource to the picture list
//			cn.Open();
//			DataSet ds		= new DataSet();
//			daPic.Fill(ds, "Picture");
//			DataRow dr		= ds.Tables[0].Rows[0];
//			filename		= dr["Filename"].ToString();

		}

		protected override void CreateChildControls() 
		{

			String strAppPath = HttpContext.Current.Request.ApplicationPath;
			if (!strAppPath.Equals("/"))  
				strAppPath = strAppPath + "/";

			System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
			
			// Create the image object
			image.BorderWidth = Unit.Pixel(0);
			image.BorderStyle = BorderStyle.None;

			// see if file wasn't set
			if (filename == null)
			{
				image.ImageUrl = strAppPath + "GetImage.axd?p=" + pictureId.ToString() + "&mw=" + maxWidth.ToString() + "&mh=" + maxHeight.ToString();
			}
			else
			{
				image.ImageUrl = strAppPath + "piccache/" + filename.Replace(@"\", @"/");
			}

			// If we have height / width, set them
			if (height > 0)
				image.Height = height;
			if (width > 0)
				image.Width = width;
			this.Controls.Add(image);

		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write("<div id=\"" + this.ID + "\"");
			if (cssClass != null)
			{
				writer.Write(" class=\"" + cssClass + "\"");
			}
			writer.Write(">");

			base.Render(writer);

			writer.Write("</div>");
		}
	
		/// <summary>
		/// Source filename of the image
		/// </summary>
		public string Filename 
		{
			get 
			{
				return filename;
			}
			set 
			{
				filename = value;
			}
		}

		public string CssClass
		{
			get 
			{
				return cssClass;
			}
			set
			{
				cssClass = value;
			}
		}

		/// <summary>
		/// Picture image actual width
		/// </summary>
		public int Width 
		{
			get 
			{
				return width;
			}
			set 
			{
				width = value;
			}
		}

		/// <summary>
		/// Picture image actual height
		/// </summary>
		public int Height 
		{
			get 
			{
				return height;
			}
			set 
			{
				height = value;
			}
		}

	}

	#endregion

	#region ThumbnailList Control
	/// <summary>
	///		Creates a DataList view of the passed Picture list
	/// </summary>
	public class ThumbnailList : System.Web.UI.Control, System.Web.UI.INamingContainer
	{
		#region Declares
		protected System.Web.UI.WebControls.DataList thumbList;
		protected String pageReturnURL;
		protected String noPicturesMessage;
		protected bool showRecordNumber;
		protected bool showCheckBox;
		#endregion

		/// <summary>
		/// Basic constructor
		/// </summary>
		public ThumbnailList()
		{

			// set up dlPicture
			thumbList = new DataList();
			thumbList.ItemDataBound += new DataListItemEventHandler(PictureItemDataBind);
			thumbList.Visible = true;

			// Set basic props of DataList
			thumbList.RepeatDirection	= RepeatDirection.Horizontal;
			thumbList.HorizontalAlign	= HorizontalAlign.Center;
			thumbList.Width				= Unit.Percentage(100);
			thumbList.RepeatColumns		= 4;
			thumbList.BorderColor		= Color.FromArgb(204, 204, 204);
			thumbList.BorderStyle		= BorderStyle.None;
			thumbList.CellPadding		= 3;
			thumbList.GridLines			= GridLines.Both;
			thumbList.BorderWidth		= Unit.Pixel(0);

		}


		protected override void CreateChildControls() 
		{
			// Add this list to this objects controls
			this.Controls.Add(thumbList);

			// see if there are any records in the data source
			if (!HasPictures) 
			{
                Table t = new Table();
				t.HorizontalAlign = HorizontalAlign.Center;
				this.Controls.Add(t);

				// add a blank row for spacing
				TableRow tr = new TableRow();
				tr.Height   = Unit.Pixel(50);
				t.Rows.Add(tr);

				// fill blank row with a blank space
				TableCell tc  = new TableCell();
				tc.ColumnSpan = 3;
				tc.Text		  = "&nbsp;";
				tr.Cells.Add(tc);

				// second row with everything in it
				tr = new TableRow();
				t.Rows.Add(tr);

				// blank cell
				tc = new TableCell();
				tc.Width	= Unit.Pixel(50);
				tc.Text		= "&nbsp;";
				tr.Cells.Add(tc);

				// cell with 'i' image
				tc = new TableCell();
				tc.VerticalAlign	= VerticalAlign.Middle;
				tc.HorizontalAlign	= HorizontalAlign.Center;
				tr.Cells.Add(tc);

				// 'i' image
				PngImage img	= new PngImage("Images/info_icon.png", 16, 16);
				tc.Controls.Add(img);

				// text cell
				tc = new TableCell();
				tr.Cells.Add(tc);

				// text contents
                Literal lt = new Literal();
				if (noPicturesMessage == null)
					noPicturesMessage = "<b>There are no pictures in the current area.</b><br>Please select another area.";
				lt.Text	= noPicturesMessage;
				tc.Controls.Add(lt);

			}

		}
		
		private void PictureItemDataBind(object sender, DataListItemEventArgs e) 
		{
			ListItemType itemType = e.Item.ItemType;

			if ((itemType != ListItemType.Header) &&
				(itemType != ListItemType.Footer) &&
				(itemType != ListItemType.Separator))
			{
				// Create new picture view control 
				string zoomUrl	= System.Web.UI.DataBinder.Eval(e.Item.DataItem, "RecNumber", pageReturnURL);
				int pictureId	= Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "PictureId"));
				string caption	= DataBinder.Eval(e.Item.DataItem, "Title").ToString();
                int recNumber	= Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "RecNumber").ToString());

				PictureViewer viewer = new PictureViewer(pictureId, zoomUrl, caption, recNumber, showCheckBox, showRecordNumber, false);
				viewer.PictureCheckBoxClicked += new PictureCheckBoxClickedEventHandler(viewer_PictureCheckBoxClicked);

				
				// finally, add table to this dataitem
				e.Item.Controls.Add(viewer);
			}

		}

		#region Properties
		/// <summary>
		/// Sets the datasource to display pictures
		/// </summary>
		public Object ThumbsDataSource
		{
			set 
			{
				thumbList.DataSource = value;
				thumbList.DataBind();
			}

		}

		/// <summary>
		/// Sets the page to return to after clicking a picture to zoom in
		/// </summary>
		public String PageReturnURL 
		{
			set 
			{
				pageReturnURL = value;
			}
		}

		/// <summary>
		/// Message to display when there are no pictures in the data source
		/// </summary>
		public String NoPictureMessage 
		{
			set 
			{
				noPicturesMessage = value;
			}
		}

		/// <summary>
		/// Returns true if the current control has pictures - note it will only return 
		/// a valid value after the control has been added to a page.
		/// </summary>
		public bool HasPictures 
		{
			get 
			{
				return (thumbList.Items.Count != 0);
			}
		}

		/// <summary>
		/// Shows the record number in the corner of each thumbnail
		/// </summary>
		public bool ShowRecordNumber 
		{
			set 
			{
				showRecordNumber = value;
			}
		}

		public bool ShowCheckBox
		{
			get
			{
				return showCheckBox;
			}
			set
			{
				showCheckBox = value;
			}
		}
		#endregion
		#region Event Handlers

		private void viewer_PictureCheckBoxClicked(object sender, PictureCheckBoxEventArgs e)
		{
			if (PictureCheckBoxClicked != null)
			{
				PictureCheckBoxClicked(sender, e);
			}
		}

		#endregion
		#region Events
		public event PictureCheckBoxClickedEventHandler PictureCheckBoxClicked;
		#endregion
	}



	#endregion

	#region CategoryList Control
	/// <summary>
	///		Creates a DataList view of the passed Picture list
	/// </summary>
	public class CategoryList: System.Web.UI.Control, System.Web.UI.INamingContainer
	{
		#region Declares
		protected System.Web.UI.WebControls.DataList thumbList;
		protected String pageReturnURL;
		protected String noPicturesMessage;
		protected bool showRecordNumber;
		protected bool showCheckBox;
		#endregion

		/// <summary>
		/// Basic constructor
		/// </summary>
		public CategoryList()
		{

			// set up dlPicture
			thumbList = new DataList();
			thumbList.ItemDataBound += new DataListItemEventHandler(PictureItemDataBind);
			thumbList.Visible = true;

			// Set basic props of DataList
			thumbList.RepeatDirection	= RepeatDirection.Horizontal;
			thumbList.HorizontalAlign	= HorizontalAlign.Center;
			thumbList.Width				= Unit.Percentage(100);
			thumbList.RepeatColumns		= 3;
			thumbList.BorderColor		= Color.FromArgb(204, 204, 204);
			thumbList.BorderStyle		= BorderStyle.None;
			thumbList.CellPadding		= 3;
			thumbList.GridLines			= GridLines.Both;
			thumbList.BorderWidth		= Unit.Pixel(0);

		}


		protected override void CreateChildControls() 
		{
			// Add this list to this objects controls
			this.Controls.Add(thumbList);

			// see if there are any records in the data source
			if (!HasPictures) 
			{
				Table t = new Table();
				t.HorizontalAlign = HorizontalAlign.Center;
				this.Controls.Add(t);

				// add a blank row for spacing
				TableRow tr = new TableRow();
				tr.Height   = Unit.Pixel(50);
				t.Rows.Add(tr);

				// fill blank row with a blank space
				TableCell tc  = new TableCell();
				tc.ColumnSpan = 3;
				tc.Text		  = "&nbsp;";
				tr.Cells.Add(tc);

				// second row with everything in it
				tr = new TableRow();
				t.Rows.Add(tr);

				// blank cell
				tc = new TableCell();
				tc.Width	= Unit.Pixel(50);
				tc.Text		= "&nbsp;";
				tr.Cells.Add(tc);

				// cell with 'i' image
				tc = new TableCell();
				tc.VerticalAlign	= VerticalAlign.Middle;
				tc.HorizontalAlign	= HorizontalAlign.Center;
				tr.Cells.Add(tc);

				// 'i' image
				HtmlImage img = new HtmlImage();
				img.Src	= "Images/info.jpg";
				tc.Controls.Add(img);

				// text cell
				tc = new TableCell();
				tr.Cells.Add(tc);

				// text contents
				Literal lt = new Literal();
				if (noPicturesMessage == null)
					noPicturesMessage = "<b>There are no pictures in the current area.</b><br>Please select another area.";
				lt.Text	= noPicturesMessage;
				tc.Controls.Add(lt);

			}

		}
		
		private void PictureItemDataBind(object sender, DataListItemEventArgs e) 
		{
			ListItemType itemType = e.Item.ItemType;

			if ((itemType != ListItemType.Header) &&
				(itemType != ListItemType.Footer) &&
				(itemType != ListItemType.Separator))
			{

				// Create table to contain thumb
				Table t = new Table();
				t.CellPadding = 3;
				t.CellSpacing = 0;

				// Only row in the table
				TableRow tr = new TableRow();
				t.Rows.Add(tr);

				// Add cell with picture
				TableCell tcPic = new TableCell();
				tr.Cells.Add(tcPic);

				// Add a container table for the picture
				Table tPic = new Table();
				tPic.Height			= 145;
				tPic.Width			= 145;
				tPic.CellPadding	= 5;
				tPic.CellSpacing	= 0;
				tPic.CssClass		= "pictureFrame";
				//				tPic.BorderColor	= Color.Black;
				//				tPic.BorderStyle	= BorderStyle.Solid;
				//				tPic.BorderWidth	= 1;
				//				tPic.BackColor		= Color.Black;
				tPic.HorizontalAlign	= HorizontalAlign.Center;
				tcPic.Controls.Add(tPic);

				tcPic.Style.Add("filter", "progid:DXImageTransform.Microsoft.Shadow(color='#666666', Direction=135, Strength=8)");

				// container table row
				TableRow trtPic = new TableRow();
				tPic.Controls.Add(trtPic);

				// container table cell
				TableCell tctPic = new TableCell();
				tctPic.ColumnSpan		= 2;
				tctPic.HorizontalAlign	= HorizontalAlign.Center;
				tctPic.VerticalAlign	= VerticalAlign.Middle;
				tctPic.Height			= 145;
				tctPic.Width			= 145;
				tctPic.CssClass			= "folderImageCell";
				tctPic.Style.Add("POSITION", "relative");
				trtPic.Controls.Add(tctPic);

				if (showCheckBox)
				{
					Panel panel = new Panel();
					panel.CssClass	= "checkboxPanel";
					tctPic.Controls.Add(panel);

					int pictureId	= (int) System.Web.UI.DataBinder.Eval(e.Item.DataItem, "PictureId");

					PictureCheckBox checkBox		= new PictureCheckBox(pictureId);
					checkBox.AutoPostBack			= true;
					panel.Controls.Add(checkBox);

					// See if we need to set the checkbox state
					PictureIdCollection mySelectedList	= Global.SelectedPictures;
					if (mySelectedList.Contains(pictureId))
					{
						checkBox.Checked			= true;
					}
			
				}

				// Add link to cell for clicking on picture
				HyperLink lnkPicZoomPic	= new HyperLink();
				lnkPicZoomPic.NavigateUrl	= System.Web.UI.DataBinder.Eval(e.Item.DataItem, 
					"RecNumber", pageReturnURL);
				tctPic.Controls.Add(lnkPicZoomPic);

				Picture curPic = new Picture();
				curPic.Filename = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "FileName").ToString();
				curPic.Height	= Convert.ToInt32(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Height"));
				curPic.Width	= Convert.ToInt32(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Width"));
				lnkPicZoomPic.Controls.Add(curPic);

				TableRow noteRow = new TableRow();
				tPic.Rows.Add(noteRow);

				TableCell noteCell = new TableCell();
				noteCell.Font.Name = "Arial";
				noteCell.BackColor = Color.White;
				noteCell.ForeColor = Color.Black;
				noteRow.Cells.Add(noteCell);

				HyperLink lnkPicZoom	= new HyperLink();
				lnkPicZoom.NavigateUrl	= System.Web.UI.DataBinder.Eval(e.Item.DataItem, 
					"RecNumber", pageReturnURL);
				lnkPicZoom.CssClass		= "whitenote";
				lnkPicZoom.Text			= System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Title").ToString();
				if (lnkPicZoom.Text.Length > 15)
					lnkPicZoom.Text = lnkPicZoom.Text.Substring(0, 13) + "...";
				noteCell.Controls.Add(lnkPicZoom);

				if (lnkPicZoom.Text.Length == 0) 
				{
					Literal lit = new Literal();
					lit.Text = "&nbsp;";
					noteCell.Controls.Add(lit);
				}

				TableCell tcCounter = new TableCell();
				tcCounter.BackColor = Color.White;
				tcCounter.ForeColor = Color.Black;
				tcCounter.VerticalAlign = VerticalAlign.Bottom;
				tcCounter.HorizontalAlign = HorizontalAlign.Right;
				noteRow.Cells.Add(tcCounter);

				if (showRecordNumber) 
				{
					HyperLink countLink = new HyperLink();
					countLink.Text = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "RecNumber").ToString();
					countLink.CssClass = "recnum";
					countLink.NavigateUrl = System.Web.UI.DataBinder.Eval(e.Item.DataItem, 
						"RecNumber", pageReturnURL);
					tcCounter.Controls.Add(countLink);
				}

				// finally, add table to this dataitem
				e.Item.Controls.Add(t);
			}

		}

		#region Properties
		/// <summary>
		/// Sets the datasource to display pictures
		/// </summary>
		public Object ThumbsDataSource
		{
			set 
			{
				thumbList.DataSource = value;
				thumbList.DataBind();
			}

		}

		/// <summary>
		/// Sets the page to return to after clicking a picture to zoom in
		/// </summary>
		public String PageReturnURL 
		{
			set 
			{
				pageReturnURL = value;
			}
		}

		/// <summary>
		/// Message to display when there are no pictures in the data source
		/// </summary>
		public String NoPictureMessage 
		{
			set 
			{
				noPicturesMessage = value;
			}
		}

		/// <summary>
		/// Returns true if the current control has pictures - note it will only return 
		/// a valid value after the control has been added to a page.
		/// </summary>
		public bool HasPictures 
		{
			get 
			{
				return (thumbList.Items.Count != 0);
			}
		}

		/// <summary>
		/// Shows the record number in the corner of each thumbnail
		/// </summary>
		public bool ShowRecordNumber 
		{
			set 
			{
				showRecordNumber = value;
			}
		}

		public bool ShowCheckBox
		{
			get
			{
				return showCheckBox;
			}
			set
			{
				showCheckBox = value;
			}
		}
		#endregion
	}

	#endregion

	#region CategoryListViewItem control

	public class CategoryListViewItem: Control, INamingContainer
	{
		#region Declares
		private int categoryId;
		private string navigateUrl;
		private Category category;
		protected int folderWidth = 64;
		private string folderImage = @"Images/folder.png";
		private bool adminMode;
		#endregion
		#region Constructors
		public CategoryListViewItem()
		{
		}

		public CategoryListViewItem(int categoryId, string navigateUrl)
		{
			this.categoryId			= categoryId;
			this.navigateUrl		= navigateUrl;
		}

		public CategoryListViewItem(Category category)
		{
			this.category			= category;
		}
		#endregion
		#region Properties
		public int CategoryId
		{
			get
			{
				return categoryId;
			}
			set
			{
				categoryId = value;
			}
		}
		public string NavigateUrl
		{
			get
			{
				return navigateUrl;
			}
			set
			{
				navigateUrl = value;
			}
		}
		public int FolderWidth
		{
			get
			{
				return folderWidth;
			}
			set
			{
				folderWidth = value;
			}
		}
		public bool AdminMode
		{
			get
			{
				return adminMode;
			}
			set
			{
				adminMode = value;
			}
		}

		public string FolderImage
		{
			get
			{
				return folderImage;
			}
			set
			{
				folderImage = value;
			}
		}
		#endregion
		#region Private Methods
		protected override void CreateChildControls()
		{
			if (HttpContext.Current != null)
			{
                adminMode = Global.AdminMode;
			}

			Table t					= new Table();
			t.Width					= Unit.Percentage(100);
			this.Controls.Add(t);

			TableRow tr				= new TableRow();
			tr.Attributes.Add("height", "20");
			t.Rows.Add(tr);

			// Load the category if we need to
			CategoryManager catManager	= PicContext.Current.CategoryManager;
			if (category == null)
			{
				category					= catManager.GetCategory(categoryId);
			}

			// Get counts of sub items
            int picCount = this.GetRecursivePictureCount(category.CategoryId, false);
            int catCount = this.GetRecursiveCategoryCount(category.CategoryId, false);
            int recursivePicCount = this.GetRecursivePictureCount(category.CategoryId, true);
            int recursiveCatCount = this.GetRecursiveCategoryCount(category.CategoryId, true);

			TableCell catCell		= new TableCell();
			catCell.Width			= Unit.Pixel(folderWidth);
			catCell.VerticalAlign	= VerticalAlign.Top;
			catCell.RowSpan			= 3;
			tr.Cells.Add(catCell);

			Panel p					= new Panel();
			p.CssClass				= "categoryFolder";
			catCell.Controls.Add(p);

			// Background folder image
			string imageUrl			= (recursivePicCount != 0 ? folderImage : "Images/folderEmpty.png");
			if (folderWidth == 150 && category.PictureId != 0)
			{
				PictureViewer pic		= new PictureViewer(category.PictureId, navigateUrl, "", 0, false, false, false);
				pic.CssClass			= "categoryPicture";
				catCell.Controls.Add(pic);
			}
			else
			{
				PngImage pngImage		= new PngImage(imageUrl, folderWidth, folderWidth);
				if (navigateUrl != null)
				{
					pngImage.OnClickScript	= "location.href='" + navigateUrl + "';";
				}
				p.Controls.Add(pngImage);
			}

			// Text cell
			catCell					= new TableCell();
			catCell.CssClass		= "categoryTextCell";
			catCell.VerticalAlign	= VerticalAlign.Top;
			catCell.Attributes.Add("height", "20");
			tr.Cells.Add(catCell);

			if (folderWidth > 60)
			{
				catCell.Controls.Add(new HtmlLiteral("<br>"));
			}
		
			// Link to category
			HyperLink lnk			= new HyperLink();
			lnk.Text				= category.Name;
			lnk.CssClass			= "categoryLink";
			if (navigateUrl != null)
			{
				lnk.NavigateUrl		= navigateUrl; 
			}
			catCell.Controls.Add(lnk);

			bool prevText = false;

			// Date/time, if we have something
			string dateFormatString	= "ddd, MMM d \"'\"yy";
			if (category.FromDate == DateTime.MinValue)
			{
			}
			else 
			{
				catCell.Controls.Add(new HtmlLiteral("<br>"));

				if (prevText)
				{
					catCell.Controls.Add(new HtmlLiteral(" from "));
				}
//				prevText = true;
				if (category.FromDate.Date == category.ToDate.Date)
				{
					catCell.Controls.Add(new HtmlLiteral(category.FromDate.ToString(dateFormatString)));
				}
				else
				{
					catCell.Controls.Add(new HtmlLiteral(category.FromDate.ToString(dateFormatString)));
					catCell.Controls.Add(new HtmlLiteral(" to "));
					catCell.Controls.Add(new HtmlLiteral(category.ToDate.ToString(dateFormatString)));
				}
				catCell.Controls.Add(new HtmlLiteral("<br>"));
			}
	
			// Description
			if (category.Description != null && category.Description.Length > 0)
			{
				Label lbl			= new Label();
				lbl.Text			= category.Description;
				lbl.CssClass		= "categoryDescription";

				catCell.Controls.Add(lbl);

			}

			// Contents
			//catCell.Controls.Add(new HtmlLiteral("<i>Contains:</i><br>"));
			tr						= new TableRow();
			tr.Attributes.Add("height", "10");
			t.Rows.Add(tr);

			catCell					= new TableCell();
			catCell.CssClass		= "categoryTextCell";
			catCell.VerticalAlign	= VerticalAlign.Bottom;
			catCell.Attributes.Add("height", "10");
			tr.Cells.Add(catCell);

			if (adminMode)
			{
				string groups = "";
				string [] groupNames = PicContext.Current.CategoryManager.GetCategoryGroups(categoryId);
				foreach (string groupName in groupNames)
				{
					if (groups.Length > 0) groups += ", ";
					groups += groupName;
				}
				groups = "Groups: " + groups + "<br />";
				catCell.Controls.Add(new HtmlLiteral(groups));
			}

			string splitText = ", ";

			if (picCount > 0)
			{	
				if (prevText)
				{
					catCell.Controls.Add(new HtmlLiteral(splitText));
				}
				if (picCount == 1)
				{
					catCell.Controls.Add(new HtmlLiteral("Contains " + picCount.ToString("#,##0") + " picture"));
				}
				else
				{
					catCell.Controls.Add(new HtmlLiteral("Contains " + picCount.ToString("#,##0") + " pictures"));
				}
//				prevText = true;
			}

			// Recursive picture count
			if (picCount == 0)
			{
				if (prevText)
				{
					catCell.Controls.Add(new HtmlLiteral(splitText));
				}
				prevText = true;

				if (recursivePicCount == 0)
				{
					catCell.Controls.Add(new HtmlLiteral("There are no pictures in this folder or subfolders.<br>"));
				}
				else if (recursivePicCount == 1)
				{
					catCell.Controls.Add(new HtmlLiteral("Contains " + recursivePicCount.ToString("#,##0") + " pictures in " + recursiveCatCount.ToString() + " subfolders"));
				}
				else
				{
					catCell.Controls.Add(new HtmlLiteral("Contains " + recursivePicCount.ToString("#,##0") + " pictures in " + recursiveCatCount.ToString() + " subfolders"));
				}
			}

//			catCell.Controls.Add(new HtmlLiteral("Latest Updates<br>"));

		}
		#endregion

        private int GetRecursivePictureCount(int categoryId, bool recursive)
        {
            int pictureCount = -1;
            HttpContext context = HttpContext.Current;
            string cacheKey = string.Format("PictureCount.PersonId.{0}.CategoryId.{1}.Pictures.Recursive.{2}", 
                PicContext.Current.CurrentUser.Id,
                categoryId,
                recursive);

            if (context != null)
            {
                object cachedObject = context.Cache[cacheKey];
                if (cachedObject != null)
                {
                    pictureCount = Convert.ToInt32(cachedObject.ToString());
                    return pictureCount;
                }
            }

            pictureCount = PicContext.Current.CategoryManager.PictureCount(category.CategoryId, recursive);

            if (context != null)
            {
                context.Cache.Add(cacheKey, pictureCount, null, DateTime.MaxValue, TimeSpan.FromMinutes(10), System.Web.Caching.CacheItemPriority.Normal, null);
            }

            return pictureCount;
        }


        private int GetRecursiveCategoryCount(int categoryId, bool recursive)
        {
            int categoryCount = -1;
            HttpContext context = HttpContext.Current;
            string cacheKey = string.Format("PictureCount.PersonId.{0}.CategoryId.{1}.Categories.Recursive.{2}", 
                PicContext.Current.CurrentUser.Id,
                categoryId,
                recursive);

            if (context != null)
            {
                object cachedObject = context.Cache[cacheKey];
                if (cachedObject != null)
                {
                    categoryCount = Convert.ToInt32(cachedObject.ToString());
                    return categoryCount;
                }
            }

            categoryCount = PicContext.Current.CategoryManager.CategoryCount(category.CategoryId, recursive);

            if (context != null)
            {
                context.Cache.Add(cacheKey, categoryCount, null, DateTime.MaxValue, TimeSpan.FromMinutes(10), System.Web.Caching.CacheItemPriority.Normal, null);
            }

            return categoryCount;
        }


	}

	#endregion

	#region Paged Thumbnail Control

	public class PagedThumbnailList: ThumbnailList 
	{

		protected int totalRecords;
		protected int startRecord;
		protected int recordsPerPage;
		protected string pageNavUrl;

		public PagedThumbnailList (): base()
		{}


		protected override void CreateChildControls() 
		{
			if (totalRecords > recordsPerPage)
			{
				this.Controls.Add(new ThumbnailListPagingControl(pageNavUrl, recordsPerPage, startRecord, totalRecords));
			}

			// Add the thumbnail list from the base class to this object
			base.CreateChildControls();

			if (totalRecords > recordsPerPage)
			{
				this.Controls.Add(new ThumbnailListPagingControl(pageNavUrl, recordsPerPage, startRecord, totalRecords));
			}
		}

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

		public String PageNavUrl 
		{
			set 
			{
				pageNavUrl = value;
			}
			get 
			{
				return pageNavUrl;
			}
		}
		#endregion
	}

	#endregion

	#region Person Picker

	/// <summary>
	/// Allows the user to find and select a person
	/// </summary>
	public class PersonPicker : System.Web.UI.Control, System.Web.UI.INamingContainer
	{
		protected System.Web.UI.WebControls.Button btnFind;
		protected System.Web.UI.WebControls.DataGrid dgPeople;
		protected System.Web.UI.WebControls.TextBox txtName;

		public event EventHandler PersonSelected;

		public PersonPicker()
		{

			// Create items
			
			// Find button
			btnFind = new Button();
			btnFind.Text = " Find ";
			btnFind.CssClass = "btn";

			// Find field
			txtName = new TextBox();
			
			// Data grid
			dgPeople = new DataGrid();
			dgPeople.ItemCommand	+= new DataGridCommandEventHandler(dgPeople_ItemCommand);
			dgPeople.GridLines		= GridLines.Vertical;
			dgPeople.BorderWidth	= Unit.Pixel(1);
			dgPeople.BorderColor	= Color.FromName("#999999");
			dgPeople.BackColor		= Color.White;
			dgPeople.CellPadding	= 3;
			dgPeople.ShowHeader		= false;
			dgPeople.ShowFooter		= false;
			dgPeople.AutoGenerateColumns			= false;
			dgPeople.Width							= Unit.Percentage(100);
			
			dgPeople.SelectedItemStyle.Font.Bold	= true;
			dgPeople.SelectedItemStyle.ForeColor	= Color.Black;
			dgPeople.SelectedItemStyle.BackColor	= Color.SlateGray;

			dgPeople.ItemStyle.ForeColor			= Color.Black;
			dgPeople.ItemStyle.BackColor			= Color.Gainsboro;

			dgPeople.AlternatingItemStyle.BackColor	= Color.Silver;
			dgPeople.AlternatingItemStyle.ForeColor	= Color.Black;

			ButtonColumn bc = new ButtonColumn();
			bc.DataTextField	= "Name";
			bc.HeaderText		= "Name";
			bc.CommandName		= "Select";

			dgPeople.Columns.Add(bc);

		}
		
		public int SelectedPerson 
		{
			get 
			{
				if (dgPeople.SelectedItem != null)
					return Convert.ToInt32(dgPeople.DataKeys[dgPeople.SelectedIndex]);
			else
					return -1;
			}

		}

		protected override void CreateChildControls() 
		{
			this.Controls.Add(new LiteralControl("Name: "));
			this.Controls.Add(txtName);
			this.Controls.Add(btnFind);

			// Add a sized panel
			Panel p		= new Panel();
			p.CssClass	= "fixed300pxFrame";
			this.Controls.Add(p);

			p.Controls.Add(dgPeople);

			// Set up events
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			this.dgPeople.SelectedIndexChanged += new System.EventHandler(this.dgPeople_SelectedIndexChanged);

		}

		private void btnFind_Click (Object sender, EventArgs e) 
		{
			// set up connection and command
			PersonInfoCollection col	= PicContext.Current.UserManager.Find(txtName.Text);
			dgPeople.DataSource			= col;
			dgPeople.DataKeyField		= "Id";
			dgPeople.DataBind();
		}

		private void dgPeople_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (PersonSelected != null)
				PersonSelected(sender, e);
		}

		private void dgPeople_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			dgPeople.SelectedIndex = e.Item.ItemIndex;
			btnFind_Click(null, e);
		}
	}

	#endregion

	#region People Selector
	public class PeopleSelector : System.Web.UI.Control, System.Web.UI.INamingContainer
	{
        protected TextBox searchEntry;
		protected Button search;
		protected ListBox availablePeople;
		protected ListBox selectedPeople;
		protected Button addPerson;
		protected Button removePerson;
		protected Label errorMessage;
		protected Color backColor;
		protected Color foreColor;

		public PeopleSelector() 
		{
			// Initialize stuff
			searchEntry = new TextBox();
			backColor	= Color.Transparent;
			foreColor	= Color.Black;

			search = new Button();
			search.Text = "Find";
			search.Click += new System.EventHandler(search_Click);
			search.CssClass = "btn";

			availablePeople = new ListBox();
			availablePeople.Width = Unit.Pixel(250);
			availablePeople.SelectionMode = ListSelectionMode.Multiple;
			selectedPeople  = new ListBox();
			selectedPeople.Width = Unit.Pixel(250);
			selectedPeople.SelectionMode = ListSelectionMode.Multiple;

			addPerson = new Button();
			addPerson.Text = "Add";
			addPerson.Click += new System.EventHandler(addPerson_Click);
			addPerson.Width = Unit.Pixel(100);
			addPerson.CssClass = "btn";
			
			removePerson = new Button();
			removePerson.Text = "Remove";
			removePerson.Click += new System.EventHandler(removePerson_Click);
			removePerson.Width = Unit.Pixel(100);
			removePerson.CssClass = "btn";
            
			errorMessage = new Label();
			errorMessage.CssClass = "err";
		}

		protected override void CreateChildControls() 
		{
			// Main table
			Table t = new Table();
			t.BackColor = backColor;
			t.ForeColor = foreColor;
			this.Controls.Add(t);

			// TOP ROW - with find box
			TableRow tr = new TableRow();
			t.Rows.Add(tr);

			// Find cell
			TableCell tc = new TableCell();
			tc.ColumnSpan = 3;
			tr.Cells.Add(tc);

			// Find text
			Literal lit = new Literal();
			lit.Text = "Find: ";
			tc.Controls.Add(lit);

			// Find textbox
            tc.Controls.Add(searchEntry);

			// Find button
			tc.Controls.Add(search);

			// Spacer
			lit = new Literal();
			lit.Text = "&nbsp;&nbsp;&nbsp;";
			tc.Controls.Add(lit);

			// Error (if any)
			tc.Controls.Add(errorMessage);

			// SECOND ROW - main stuff
			tr = new TableRow();
			t.Rows.Add(tr);

			// Cell with found people
			tc = new TableCell();
			tc.Width = Unit.Percentage(40);
			tr.Cells.Add(tc);

			// Found people list
			lit = new Literal();
			lit.Text = "Matching names<br>";
			tc.Controls.Add(lit);
			tc.Controls.Add(availablePeople);

			// Cell with buttons
			tc = new TableCell();
			tc.Width = Unit.Percentage(20);
			tc.HorizontalAlign = HorizontalAlign.Center;
			tr.Cells.Add(tc);

			// Add buttons
			tc.Controls.Add(addPerson);
			lit = new Literal();
			lit.Text = "<br>";
			tc.Controls.Add(lit);
			tc.Controls.Add(removePerson);

			// Cell with selected people
			tc = new TableCell();
			tc.Width = Unit.Percentage(40);
			tr.Cells.Add(tc);

			// Selected people list
			lit = new Literal();
			lit.Text = "Selected People<br>";
			tc.Controls.Add(lit);
			tc.Controls.Add(selectedPeople);

		}

		private void search_Click(object sender, System.EventArgs e) 
		{
			// clear the current list if anything is there
			errorMessage.Text = "";
			availablePeople.Items.Clear();

			// set up connection and command
			PersonInfoCollection people		= PicContext.Current.UserManager.Find(searchEntry.Text);
			availablePeople.DataSource		= people;
			availablePeople.DataTextField	= "Name";
			availablePeople.DataValueField	= "Id";
			availablePeople.DataBind();

			// check if any recs found
			if (availablePeople.Items.Count == 0)
				errorMessage.Text = String.Format("There were no people found with '{0}' in their name.", searchEntry.Text);

		}

		private void addPerson_Click(object sender, System.EventArgs e) 
		{
			// clear any error displayed
			errorMessage.Text = "";

			// loop through list adding item to second list if not already present
			foreach (ListItem item in availablePeople.Items) 
			{
				if (item.Selected) 
				{
					ListItem li = new ListItem(item.Text, item.Value);
					if (!selectedPeople.Items.Contains(li))
						selectedPeople.Items.Add(li);
				}
			}

			// loop through removing each selected item
			while (availablePeople.SelectedIndex >= 0) 
			{
				availablePeople.Items.Remove(availablePeople.SelectedItem);
			}

		}

		private void removePerson_Click(object sender, System.EventArgs e) 
		{
			// clear any error displayed
			errorMessage.Text = "";

			ListItem item;

			// loop through selected items removing each one
			while (selectedPeople.SelectedIndex >= 0) 
			{
				item = selectedPeople.SelectedItem;
				if (!availablePeople.Items.Contains(item))
					availablePeople.Items.Add(item);

				item = selectedPeople.SelectedItem;
				selectedPeople.Items.Remove(item);
			}

		}


		public void AddPerson(int personId, string personName) 
		{
            // add item to list of selectedpeople
			ListItem item = new ListItem(personName, personId.ToString());
			selectedPeople.Items.Add(item);
		}

		public ListItemCollection SelectedPeople 
		{
			get 
			{
				return selectedPeople.Items;
			}

			set 
			{
				foreach(ListItem li in value) 
				{
					selectedPeople.Items.Add(li);
				}
			}
		}

		public Color BackColor 
		{
			set { backColor = value; }
			get { return backColor;  }
		}

		public Color ForeColor 
		{
			set { foreColor = value; }
			get { return foreColor;  }
		}

	}
	#endregion

	#region Error Message Panel

	[ParseChildrenAttribute(ChildrenAsProperties = false)]
	public class ErrorMessagePanel: Control, INamingContainer
	{
		#region Declares
		private ArrayList	childControls;
		private string		title;
		#endregion

		#region Constructors
		public ErrorMessagePanel()
		{
			childControls			= new ArrayList();
		}
		#endregion

		#region Private Methods
		protected override void AddParsedSubObject(object obj)
		{
			childControls.Add(obj);
		}

		protected override void CreateChildControls()
		{
			Table t					= new Table();
			t.CssClass				= "panelBadLogin";
			t.CellPadding			= 0;
			t.CellSpacing			= 0;
			this.Controls.Add(t);
            
			#region Title Row

			if (title != null)
			{
				TableRow titleRow		= new TableRow();
				t.Rows.Add(titleRow);

				TableCell titleCell		= new TableCell();
				titleCell.ColumnSpan	= 2;
				titleCell.CssClass		= "panelBadLoginTitle";
				titleRow.Cells.Add(titleCell);

				titleCell.Controls.Add(new HtmlLiteral(title));
			}

			#endregion

			#region Content Row

			TableRow contentRow			= new TableRow();
			t.Rows.Add(contentRow);

			TableCell imageCell			= new TableCell();
			imageCell.CssClass			= "panelBadLoginImageCell";
			imageCell.VerticalAlign		= VerticalAlign.Top;
			contentRow.Cells.Add(imageCell);

			imageCell.Controls.Add(new PngImage(@"../Images/stop_icon.png", 24, 24));

			TableCell messageCell		= new TableCell();
			messageCell.CssClass		= "pnlBadLoginTextCell";
			contentRow.Cells.Add(messageCell);

			foreach (Control c in childControls)
			{
				messageCell.Controls.Add(c);
			}

			#endregion

		}

		protected override void Render(HtmlTextWriter output)
		{
            output.Write("<div id=\"" + this.ID + "\" style=\"position: absolute\"");

			output.Write(">");

			base.Render(output);

			output.Write("</div>");
		}

		#endregion

		#region Properties
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

	#region PictureEditFormLink

	public class PictureEditFormLink: Control, INamingContainer
	{
		private int pictureId;

		public PictureEditFormLink(int pictureId)
		{
			this.pictureId		= pictureId;
			this.ID				= "pictureTasks";
		}

		public int PictureId
		{
			get
			{
				return pictureId;
			}
		}

		protected override void Render(HtmlTextWriter output)
		{
			output.Write("<object id=\"" + this.ID + "\" ");
			output.Write("classid=\"http:msn2.net.Pictures.Controls.dll#msn2.net.Pictures.Controls.EditPictureLink\" ");
			output.Write("height=\"20\" ");
			output.Write("width=\"50\" ");
			output.Write(">");

			output.Write("<param name=\"PictureId\" ");
			output.Write("value=\"" + pictureId.ToString() + "\" />");
			output.Write("</object>");
		}
	}
/*

	<param name="PictureId" value="1646">
</object>
*/

	#endregion

	#region CategoryEditFormLink

	public class CategoryEditFormLink: Control, INamingContainer
	{
		private int categoryId;
		private int personId;

		public CategoryEditFormLink(int categoryId, int personId)
		{
			this.categoryId		= categoryId;
			this.personId		= personId;
			this.ID				= "categoryTasks";
		}

		public int CategoryId
		{
			get
			{
				return categoryId;
			}
		}

		public int PersonId
		{
			get
			{
				return personId;
			}
		}

		protected override void Render(HtmlTextWriter output)
		{
			output.WriteLine("<object id=\"" + this.ID + "\" ");
			output.WriteLine("classid=\"http:msn2.net.Pictures.Controls.dll#msn2.net.Pictures.Controls.EditCategoryLink\" ");
			output.WriteLine("height=\"15\" ");
			output.WriteLine("width=\"50\" ");
			output.WriteLine(">");

			output.WriteLine("<param name=\"CategoryId\" ");
			output.WriteLine("value=\"" + categoryId.ToString() + "\" />");
			output.WriteLine("<param name=\"PersonId\" ");
			output.WriteLine("value=\"" + personId.ToString() + "\" />");
			output.WriteLine("<param name=\"SignalRefresh\" value=\"0\" />");
			output.WriteLine("</object>");

//			output.WriteLine("<script language=\"JavaScript\">");
//			output.WriteLine("function checkRefresh() { ");
//			output.WriteLine("  if (document.all." + this.UniqueID + ")  {");
//			output.WriteLine("    if (document.all." + this.UniqueID + ".CategoryId)");
//			//output.WriteLine("      document.all." + this.UniqueID + ".SignalRefresh = 0; ");
//			output.WriteLine("      alert(document.all." + this.UniqueID + ".CategoryId);");
//			output.WriteLine("    }");
//			output.WriteLine("    else alert('no catid');");
//			output.WriteLine("}");
//			output.WriteLine("setInterval('checkRefresh()', 10000)");
//			output.WriteLine("</script>");
		}
	}

	#endregion

	#region MainFormEditLink

	public class OpenMainFormLink: Control, INamingContainer
	{
		protected override void Render(HtmlTextWriter output)
		{
			output.Write("<object id'\"" + this.UniqueID + "\" ");
			output.Write("classid=\"http:msn2.net.Pictures.Controls.dll#msn2.net.Pictures.Controls.OpenMainFormLink\" ");
			output.Write("height=\"15\" ");
			output.Write("width=\"50\" ");
			output.Write("></object>");
		}
	}
    
	#endregion


	#region PngImage

		public class PngImage: Control, INamingContainer
		{
			private string imageSrc;
			private int width;
			private int height;
			private string		onClickScript;
			public HorizontalAlign HorizontalAlign = HorizontalAlign.NotSet;
			

			public PngImage()
			{
			}

		#region Properties
			public PngImage(string imageSrc, int width, int height)
			{
				this.ImageSrc		= imageSrc;
				this.width			= width;
				this.height			= height;
			}			

			public string ImageSrc
			{
				get
				{
					return imageSrc;
				}
				set
				{
					imageSrc = value;
				}
			}

			public int Height
			{
				get
				{
					return height;
				}
				set
				{
					height = value;
				}
			}

			public int Width
			{
				get
				{
					return width;
				}
				set
				{
					width = value;
				}
			}

			public string OnClickScript
			{
				get
				{
					return onClickScript;
				}
				set
				{
					onClickScript = value;
				}
			}

		#endregion

			protected override void Render(HtmlTextWriter output)
			{
				output.Write("<table><tr><td>");
				output.Write("<div style=\"");
				output.Write("FILTER: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + imageSrc + "'); ");
				output.Write("WIDTH: " + width.ToString() + "px;");
				output.Write("HEIGHT: " + height.ToString() + "px;");
				if (onClickScript != null)
				{
					output.Write("CURSOR: hand;");
				}
				output.Write("\"");

				// Check if we have an onClick= script
				if (onClickScript != null)
				{
					output.Write(" onClick=\"" + onClickScript + "\"");
				}
			
				output.Write("></div>");
				output.Write("</td></tr></table>");
			}

		}

	#endregion

	#region Content Panel

		[ParseChildren(false)]
			public class ContentPanel: System.Web.UI.Control
		{
		#region Declares
			private ArrayList parsedControls;
			public	Unit				Width = Unit.Empty;
			public	HorizontalAlign		Align = HorizontalAlign.NotSet;
			private Control		titleControl;
		#endregion

		#region Constructors
			public ContentPanel()
			{
				parsedControls			= new ArrayList();
			}
			public ContentPanel(string title)
			{
				parsedControls			= new ArrayList();
				this.Title				= title;
			}
		#endregion

		#region Private Methods
			protected override void AddParsedSubObject(object obj)
			{
				if (obj is Control)
				{
					parsedControls.Add(obj);
				}
			}

			protected override void CreateChildControls()
			{

				HttpContext.Current.Trace.Write("ContentPanel", "CreateChildControls: " + this.ID);

				foreach (Control c in parsedControls)
				{
					this.Controls.Add(c);
				}

			}

			protected override void Render(HtmlTextWriter output)
			{
				// Move all child controls
				output.Write("<span id=\"" + this.ID + "\"");
				output.Write(">");

#if DEBUG
			output.WriteLine("<!-- " + this.ID + " - Window table -->");
#endif
				output.Write("<table cellpadding=\"0\" cellspacing=\"0\"");
				if (Width != Unit.Empty)
				{
					output.Write(" width=\"" + Width.ToString() + "\"");
				}
				output.Write(" class=\"contentPanel\">");
			
#if DEBUG
			output.WriteLine("<!-- " + this.ID + " - Title Row -->");
#endif
				if (titleControl != null)
				{
					output.Write("<tr><td class=\"contentPanelTitleCell\">");
					titleControl.RenderControl(output);
					output.Write("</td></tr>");
				}
            
#if DEBUG
			output.WriteLine("<!-- " + this.ID + " - Content Row -->");
#endif

				// Content row
				output.Write("<tr><td class=\"contentPanelContentCell\" valign=\"top\"");
				if (Align != HorizontalAlign.NotSet)
				{
					output.Write(" align=\"" + Align.ToString() + "\"");
				}
				output.Write(">");

				foreach (Control c in this.Controls)
				{
					c.RenderControl(output);
				}

				output.Write("</td></tr></table>");

				output.Write("</span>");

#if DEBUG
			output.WriteLine("<!-- " + this.ID + " - End -->");
#endif

			}

		#endregion

			public void AddContent(Control c)
			{
				this.Controls.Add(c);
			}

			public void AddContent(Control c, int position)
			{
				this.Controls.AddAt(position, c);
			}
		#region Properties
			public string Title
			{
				get
				{
					if (titleControl != null)
					{
						return titleControl.ToString();
					}
					else
					{
						return null;
					}
				}
				set
				{
					titleControl = new HtmlLiteral(value);
				}
			}
		#endregion

		}

	#endregion

	#region Sidebar

		public class Sidebar: Control, INamingContainer
		{
		#region Declares
			private ArrayList	childControls;
			private SelectedWindow selectedWindow;
		#endregion

		#region Constructors
			public Sidebar()
			{
				// Get HTTP context
				HttpContext httpContext	= HttpContext.Current;
				if (httpContext == null)
				{
					this.Controls.Add(new HtmlLiteral("[Sidebar]"));
					return;
				}

				childControls			= new ArrayList();
            
				// If logged in
				if (httpContext.Request.IsAuthenticated) 
				{
                    bool adminMode = Global.AdminMode;
                    if (adminMode)
					{
						selectedWindow = new SelectedWindow();
						childControls.Add(selectedWindow);
					}

					// Add go task list
					ContentPanel goPanel	= new ContentPanel("Go to...");
					goPanel.Width			= Unit.Percentage(100);
					childControls.Add(goPanel);

					GoTaskList goTasks		= new GoTaskList();
					goPanel.Controls.Add(goTasks);

					childControls.Add(new HtmlLiteral("<br>"));
				}			
			}

		
		#endregion

		#region Private Methods
			protected override void AddParsedSubObject(object obj)
			{
				childControls.Add(obj);
			}

			protected override void CreateChildControls()
			{
				HttpContext context			= HttpContext.Current;
				if (context != null)
				{
					context.Trace.Write("Sidebar", "CreatingChildControls");
				}

				// Just copy the child controls into output collection
				foreach (Control c in childControls)
				{
					this.Controls.Add(c);
				}

				// Add selected window
				if (selectedWindow != null && context != null)
				{
					context.Trace.Write("Sidebar", "Adding selected window");
					this.Controls.Add(selectedWindow);
				}


			}

			protected override void Render(HtmlTextWriter output)
			{
				HttpContext httpContext			= HttpContext.Current;

				output.Write("<div id=\"" + this.ID + "\" style=\"POSITION: relative\">");

				if (httpContext != null)
				{
					// Image div
					StringBuilder sb = new StringBuilder();
					sb.Append("<div style=\"Z-INDEX: 1; LEFT: 18px; POSITION: absolute; TOP: 350px;\">");
					sb.Append("  <img src=\"" + httpContext.Request.ApplicationPath);
					if (!httpContext.Request.ApplicationPath.EndsWith(@"/"))
					{
						sb.Append(@"/");
					}
					sb.Append("Images/msn2needlewash.gif\">");
					sb.Append("</div>");
					output.Write(sb.ToString());
				}

				// Content div
				output.Write("<div style=\"Z-INDEX: 2; POSITION: absolute\" class=\"sidebar\">");
            
				base.Render(output);

				output.Write("</div>");

				output.Write("</div>");
			}

		#endregion

		#region Properties
			public SelectedWindow SelectedWindow
			{
				get 
				{
					return selectedWindow;
				}
			}
			public string CurrentTask
			{
				get
				{
					return ViewState["t"].ToString();
				}
				set
				{
					ViewState["t"] = value;
				}
			}

		#endregion

		}

	#endregion

	#region PictureCheckBox
		public class PictureCheckBox: CheckBox
		{
		#region Constructor
			public PictureCheckBox(int picId)
			{
				this.PicId = picId;
			}
		#endregion
		#region Properties
			public int PicId
			{
				get
				{
					if (ViewState["p"] != null)
					{
						return Convert.ToInt32(ViewState["p"]);
					}
					else
					{
						return 0;
					}
				}
				set
				{
					ViewState["p"] = value;
				}
			}
		#endregion
		}
	#endregion

	#region SelectedWindow

	public class SelectedWindow: TaskPanel
	{
		#region Declares
		private PictureIdCollection selectedList;
		private LinkButton clearBasket;
		private LinkButton selectAll;
		private HtmlLiteral noPicturesMessage;
		private HtmlLiteral picCountMessage;
		private Panel taskPanel;
		#endregion
		#region Constructors
		public SelectedWindow(): base("Picture Basket")
		{
			clearBasket = new LinkButton();
			clearBasket.Text = "Clear Basket";
			clearBasket.Click += new EventHandler(clearBasket_Click);
			this.Controls.Add(clearBasket);

			selectAll			= new LinkButton();
			selectAll.Text		= "Select All";
			selectAll.Click		+= new EventHandler(selectAll_Click);
			this.Controls.Add(selectAll);
		}
		#endregion
		#region Overrides

		protected override void CreateChildControls()
		{
			noPicturesMessage		= new HtmlLiteral("Check any picture to see a list of tasks.");
			noPicturesMessage.ID	= "nopicsselected";
			contentPanel.AddContent(noPicturesMessage, 0);

			taskPanel = new Panel();

			picCountMessage			= new HtmlLiteral("# pics");
			taskPanel.Controls.Add(picCountMessage);

			StringBuilder sb = new StringBuilder();
			sb.Append("<br>");

			sb.Append("<b>Tasks</b><br>");
			sb.Append("<a href=\"javascript:AddPicsToCat()\">Add to category</a><br />");
			sb.Append("<a href=\"javascript:AddGroups()\">Add groups</a><br />");

			taskPanel.Controls.Add(new HtmlLiteral(sb.ToString()));

			taskPanel.Controls.Add(selectAll);
			taskPanel.Controls.Add(clearBasket);

			contentPanel.AddContent(taskPanel);

			this.Controls.Add(contentPanel);

			base.CreateChildControls ();
		}

		protected override void OnPreRender(EventArgs e)
		{
			// if we logged off, bail
			if (PicContext.Current.CurrentUser == null)
				return;

			// Retreive the list of seleted items
			selectedList = Global.SelectedPictures;
			Page.RegisterHiddenField("selectedPicIds", selectedList.GetListAsString());

			// Show certain content for no selected
			if (selectedList.Count == 0)
			{
				noPicturesMessage.Visible	= true; 
				taskPanel.Visible			= false;
			}
			else
			{
				noPicturesMessage.Visible	= false;
				taskPanel.Visible			= true;
				
				string temp					= selectedList.Count.ToString() 
					+ (selectedList.Count == 1 ? " pic" : " pics");
				picCountMessage.Text		= temp;
			}

			// Set up JavaScript
			StringBuilder sb2	= new StringBuilder();
			int personId		= PicContext.Current.CurrentUser.Id;
			sb2.Append("<script language=\"javascript\">");
			sb2.Append("function AddPicsToCat() { ");
			sb2.Append(" if (document.all['categoryTasks'] != null) {");
			sb2.Append("   var pics = document.all.selectedPicIds.value;");
			sb2.Append("   var pId  = " + personId.ToString() + ";");
			sb2.Append("   var t    = document.all['categoryTasks'];");
			sb2.Append("   t.AddPicsToCategory(pics, pId);");
			sb2.Append(" }");
			sb2.Append("}");
			sb2.Append("function AddGroups() { ");
			sb2.Append(" if (document.all['categoryTasks'] != null) {");
			sb2.Append("   var pics = document.all.selectedPicIds.value;");
			sb2.Append("   var pId  = " + personId.ToString() + ";");
			sb2.Append("   var t    = document.all['categoryTasks'];");
			sb2.Append("   t.AddGroupsToPics(pics, pId);");
			sb2.Append(" }");
			sb2.Append("}");
			sb2.Append("// </script>");
			Page.RegisterClientScriptBlock("selectedWindowScript", sb2.ToString());

			
			base.OnPreRender (e);

		}

		#endregion

		#region Event Handlers

		private void clearBasket_Click(object sender, EventArgs e)
		{
            selectedList = Global.SelectedPictures;
			selectedList.Clear();
		}

		private void selectAll_Click(object sender, EventArgs e)
		{
			if (OnSelectAll != null)
			{
				OnSelectAll(this, EventArgs.Empty);
			}
		}

		#endregion

		public event EventHandler OnSelectAll;

	}

	#endregion

	#region HtmlLiteral

		public class HtmlLiteral: Literal
		{
		#region Constructors
			public HtmlLiteral(string text)
			{
				base.Text			= text;
			}
		#endregion
		}

	#endregion

	#region TaskPanel
		public class TaskPanel: Control, INamingContainer
		{
		#region Declares
			protected ContentPanel contentPanel;
		#endregion

		#region Constructor
			public TaskPanel(string title)
			{
				contentPanel			= new ContentPanel();
				contentPanel.Width		= Unit.Percentage(100);
				contentPanel.Title		= title;
			}
		#endregion

		#region Private Methods
			protected override void Render(HtmlTextWriter output)
			{
				output.Write("<div style=\"width: 100%\">");

				base.Render(output);

				output.Write("</div>");
			}
		#endregion
		#region Properties
			public ContentPanel ContentPanel
			{
				get 
				{
					return contentPanel;
				}
			}
		#endregion
			public void AddToSession()
			{

			}
			public void RemoveFromSession()
			{

			}
		}
	#endregion

	#region GoTaskList
	
		public class GoTaskList: TaskList
		{
			public GoTaskList()
			{
				string homeUrl				= "default.aspx";

				TaskItem goHome				= new TaskItem("Picture Home", @"images/msn2_home.gif", 16, 16, homeUrl);
				this.Controls.Add(goHome);

				TaskItem goSearch			= new TaskItem("Search", @"images/search.gif", 16, 16, "SearchCriteria.aspx");
				this.Controls.Add(goSearch);

			}
		}

	#endregion

	#region TaskList
		public class TaskList: Control, INamingContainer
		{
		#region Private Methods
			protected override void Render(HtmlTextWriter output)
			{
				output.Write("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"1\">");
			
				foreach (Control c in Controls)
				{
					c.RenderControl(output);
				}

				output.Write("</table>");
			}
		#endregion
		}
	#endregion

	#region PictureTasks

		public class PictureTasks: TaskList
		{
			public PictureTasks()
			{
//				TaskItem addToCat			= new TaskItem("Add to category...", "Images/add.png", 9, 12);
//				this.Controls.Add(addToCat);
			}

			public void SetSlideshowUrl(string url)
			{
				TaskItem slideshow			= new TaskItem("View Slideshow", "Images/slideshow12x9.png", 9, 12, url);
				this.Controls.Add(slideshow);
			}

		}

	#endregion

	#region TaskItem

		public class TaskItem: Control, INamingContainer
		{
			private Control image;
			private LinkButton link;
			private HyperLink hlink;

			public TaskItem(string text, string imageUrl, int width, int height)
			{
				image = CreateImage(imageUrl, width, height);

				link						= new LinkButton();
				link.Text					= text;
				link.CssClass				= "sidebarTaskLink";
				link.Click					+= new EventHandler(link_Click);
			}

			public TaskItem(string text, string imageUrl, int width, int height, string navigateUrl)
			{
				image = CreateImage(imageUrl, width, height);

				hlink						= new HyperLink();
				hlink.Text					= text;
				hlink.CssClass				= "sidebarTaskLink";
				hlink.NavigateUrl			= navigateUrl;
			}

			private Control CreateImage(string imageUrl, int width, int height)
			{
				if (imageUrl.IndexOf("png") > 0)
				{
					return new PngImage(imageUrl, width, height);
				}
				else
				{
					System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
					img.Height		= height;
					img.Width		= width;
					img.ImageUrl	= imageUrl;
					return img;
				}
			}

			protected override void Render(HtmlTextWriter output)
			{
				output.Write("<tr>");
				output.Write("<td>");
				image.RenderControl(output);
				output.Write("</td><td class=\"sidebarTaskLink\">");
				if (link != null)
				{
					link.RenderControl(output);
				}
				else
				{
					hlink.RenderControl(output);
				}
				output.Write("</td></tr>");
			}
			protected override void CreateChildControls()
			{
				this.Controls.Add(image);
				if (link != null)
				{
					this.Controls.Add(link);
				}
				else
				{
					this.Controls.Add(hlink);
				}
				this.Controls.Add(new HtmlLiteral("<br>"));
			}

			private void link_Click(object sender, EventArgs e)
			{
				if (Click != null)
				{
					Click(this, EventArgs.Empty);
				}
			}

			public event EventHandler Click;
		}

	#endregion
		/*
				<TR>
					<TD width="9"></TD>
					<TD class="sidebarTaskLink">
						<asp:HyperLink id="addToFolder" Runat="server" CssClass="sidebarTaskLink">Add to folder</asp:HyperLink></TD>
				</TR>
				<TR>
					<TD width="9"></TD>
					<TD class="sidebarTaskLink">
						<asp:HyperLink id="setCategoryPic" Runat="server" CssClass="sidebarTaskLink">Set folder pic</asp:HyperLink></TD>
				</TR>
			</TABLE>
		</picctls:contentpanel>
		*/

	}
