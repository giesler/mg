
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

	#region Picture Display control
	/// <summary>
	///		Outputs an image tag referring to an image file created with max height and width
	/// </summary>
	public class Picture : System.Web.UI.Control, System.Web.UI.INamingContainer
	{

		protected int maxWidth;
		protected int maxHeight;
		protected String filename;
		protected System.Web.UI.WebControls.Image currentImage;
		protected HttpServerUtility	server;

		/// <summary>
		/// Creates a new Picture object
		/// </summary>
		/// <param name="srvr">Server object of the current request</param>
		public Picture(HttpServerUtility server)
		{
			this.server = server;
		}

		protected override void CreateChildControls() 
		{
			
			// Create the image object
			currentImage = new System.Web.UI.WebControls.Image();
			currentImage.BorderWidth = Unit.Pixel(0);
			currentImage.BorderStyle = BorderStyle.None;

			// verify max width and height
			if (maxWidth == 0)
				maxWidth = 150;
			if (maxHeight == 0)
				maxHeight = 150;

			// init values
			int newWidth	= maxWidth;
			int newHeight	= maxHeight;

			// see if file wasn't set
			if (filename == null)
				throw new Exception("Filename is blank");

			// build the filename of our desired target file
			String targetImageURL = 
				filename.Substring(0, filename.LastIndexOf("."))	    // get part before .<type>
				+ "h" + newHeight + "w" + newWidth						// add on hxwx
				+ ".jpg";					// always want .jpg   + strFile.Substring(strFile.LastIndexOf("."));			
			
			
			// figure out the filenames on the web server
			String sourceFile = pics.Config.PictureDirectory + "\\" + filename.Replace("/", "\\");
			String targetFile = server.MapPath("/piccache") + "\\" + targetImageURL.Replace("/", "\\");

			// check datestamps - if the source file has changed get rid of the cached image
			if (File.Exists(targetFile)) 
			{
				if (File.GetLastWriteTime(sourceFile) > File.GetLastWriteTime(targetFile))
					File.Delete(targetFile);
			}

			// if the targetFile doesn't exist, we need to create it
			if (!File.Exists(targetFile)) 
			{

				// load the current file
				System.Drawing.Image img = System.Drawing.Image.FromFile(sourceFile);

				// read the current height and width
				newWidth  = img.Width;
				newHeight = img.Height;

				// see if image is wider then we want, if so resize
				if (newWidth > maxWidth) 
				{
					newWidth  = maxWidth;
					newHeight = (int) ( (float) img.Height * ( (float) newWidth / (float) img.Width) );
				}

				// see if image height is greater then we want, if so, resize
				if (newHeight > maxHeight)
				{
					newWidth  = (int) ( (float) newWidth * ( (float) maxHeight / (float) newHeight) );
					newHeight = maxHeight;
				}

				// see if target dir exists, if not, create it
				String targetDirectory = targetFile.Substring(0, targetFile.LastIndexOf("\\"));
				if (!System.IO.Directory.Exists(targetDirectory))
					System.IO.Directory.CreateDirectory(targetDirectory);

				// if new width and height aren't the same as the image, resize, createing a new image, then save
				if (newWidth != img.Width || newHeight != img.Height) 
				{
					System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img, newWidth, newHeight);
					bmp.Save(targetFile, System.Drawing.Imaging.ImageFormat.Jpeg);
					bmp.Dispose();
				} 
				else 
				{
					// simply output the file as it is, since it is already an okay size
					img.Save(targetFile, System.Drawing.Imaging.ImageFormat.Jpeg);
				}

				// since we know these values, might as well set them
				currentImage.Height = newHeight;
				currentImage.Width  = newWidth;

				img.Dispose();

			}

			// we now have an image available, so set the image tag
			currentImage.ImageUrl = "/piccache/" + targetImageURL;

			this.Controls.Add(currentImage);

		}

		/// <summary>
		/// Maximum height of the image to output
		/// </summary>
		public int MaxHeight 
		{
			get 
			{
				return maxHeight;
			}
			set 
			{
				maxHeight = value;
			}
		}
	
		/// <summary>
		/// Maximum width of the image to output
		/// </summary>
		public int MaxWidth 
		{
			get 
			{
				return maxWidth;
			}
			set 
			{
				maxWidth = value;
			}
		}
	
		/// <summary>
		/// Source filename of the image
		/// </summary>
		public String Filename 
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

	}

	#endregion

	#region ThumbnailList Control
	/// <summary>
	///		Creates a DataList view of the passed Picture list
	/// </summary>
	public class ThumbnailList : System.Web.UI.Control, System.Web.UI.INamingContainer
	{
		protected System.Web.UI.WebControls.DataList thumbList;
		protected HttpServerUtility server;
		protected String pageReturnURL;
		protected String noPicturesMessage;
		protected bool showRecordNumber;

		/// <summary>
		/// Creates a thumbnail list based on the DataSet
		/// </summary>
		/// <param name="srvr">Current request server object</param>
		public ThumbnailList(HttpServerUtility server)
		{
            this.server = server;

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
				tPic.BorderColor	= Color.White;
				tPic.BorderStyle	= BorderStyle.Solid;
				tPic.BorderWidth	= 2;
				tPic.BackColor		= Color.Black;
				tPic.HorizontalAlign	= HorizontalAlign.Center;
				tcPic.Controls.Add(tPic);

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
				trtPic.Controls.Add(tctPic);

				// Add link to cell for clicking on picture
				HyperLink lnkPicZoomPic	= new HyperLink();
				lnkPicZoomPic.NavigateUrl	= System.Web.UI.DataBinder.Eval(e.Item.DataItem, 
					"RecNumber", pageReturnURL);
				tctPic.Controls.Add(lnkPicZoomPic);

				// Add picture to cell
				Picture curPic = new Picture(server);
				curPic.Filename = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "FileName").ToString();
				curPic.MaxHeight = 125;
				curPic.MaxWidth  = 125;
				lnkPicZoomPic.Controls.Add(curPic);

				TableRow noteRow = new TableRow();
				tPic.Rows.Add(noteRow);

				TableCell noteCell = new TableCell();
				noteCell.BackColor = Color.White;
				noteCell.ForeColor = Color.Black;
				noteRow.Cells.Add(noteCell);

				HyperLink lnkPicZoom	= new HyperLink();
				lnkPicZoom.NavigateUrl	= System.Web.UI.DataBinder.Eval(e.Item.DataItem, 
					"RecNumber", pageReturnURL);
				lnkPicZoom.CssClass		= "whitenote";
				lnkPicZoom.Text			= System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Title").ToString();
				if (lnkPicZoom.Text.Length > 14)
					lnkPicZoom.Text = lnkPicZoom.Text.Substring(0, 12) + "...";
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
/*				Label lll = new Label();
				lll.CssClass = "note";
				lll.Text = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Title").ToString();
				ttc.Controls.Add(lll);

				// Add cell with link and description
				TableCell tcInfo = new TableCell();
				tr.Cells.Add(tcInfo);

				// Add link to zoom in to picture
				HyperLink lnkPicZoom	= new HyperLink();
				lnkPicZoom.NavigateUrl	= System.Web.UI.DataBinder.Eval(e.Item.DataItem, 
											"RecNumber", pageReturnURL);
				lnkPicZoom.Text			= System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Title").ToString();
				tcInfo.Controls.Add(lnkPicZoom);

				// Add picture date
				Label lblPicDate = new Label();
				lblPicDate.Text  = "<br><br>" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "PictureDate", "{0:d}");
				tcInfo.Controls.Add(lblPicDate);
*/
				// finally, add table to this dataitem
				e.Item.Controls.Add(t);
			}

		}

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
	}
	#endregion

	#region Paged Thumbnail Control

	public class PagedThumbnailList: ThumbnailList 
	{

		protected int totalRecords;
		protected int startRecord;
		protected int recordsPerPage;
		protected String pageNavURL;

		public PagedThumbnailList (HttpServerUtility server): base(server)
		{}


		protected override void CreateChildControls() 
		{
			// Add the thumbnail list from the base class to this object
			base.CreateChildControls();

			
			// figure out number of pages we have
			int pages = totalRecords / recordsPerPage ;
			int page  = (startRecord / recordsPerPage) + 1;	// +1 to compensate for 0 being 1
			if (totalRecords % recordsPerPage != 0) // if we have some odd pics left over, that makes another page
				pages++;

			// display page controls if more then one page
			if (pages > 0) 
			{
				// Page controls table
				Table t = new Table();
				t.Width		= Unit.Percentage(100);
				this.Controls.Add(t);

				TableRow tr = new TableRow();
				t.Rows.Add(tr);

				// Now show the page and page count in the middle
				TableCell tcPage = new TableCell();
				tcPage.HorizontalAlign = HorizontalAlign.Right;
				tr.Cells.Add(tcPage);

				// 'Page ' prefix
				Literal litPage = new Literal();
				litPage.Text	= "Page ";
				tcPage.Controls.Add(litPage);

				// If not at the beginning, add a previous link
				if (page > 1) 
				{
					HyperLink lnk = new HyperLink();
					lnk.Text = "Previous";
					lnk.Font.Bold = true;
					lnk.NavigateUrl = String.Format(pageNavURL, (startRecord - recordsPerPage));
					tcPage.Controls.Add(lnk);

					// add a single space
					Literal l = new Literal();
					l.Text = "&nbsp;";
					tcPage.Controls.Add(l);

				}

				// Loop through the pages we have
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
						HyperLink lnk = new HyperLink();
						lnk.Text = i.ToString();
						lnk.NavigateUrl = String.Format(pageNavURL, (page-1)*recordsPerPage);
						tcPage.Controls.Add(lnk);
					}

					Literal l = new Literal();
					l.Text = "&nbsp;";
					tcPage.Controls.Add(l);
						
				}

				// If not at the end, add a next link
				if (page < pages) 
				{
					HyperLink lnk = new HyperLink();
					lnk.Text = "Next";
					lnk.Font.Bold = true;
					lnk.NavigateUrl = String.Format(pageNavURL, (startRecord + recordsPerPage));
					tcPage.Controls.Add(lnk);

					// add a single space
					Literal l = new Literal();
					l.Text = "&nbsp;";
					tcPage.Controls.Add(l);

				}

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

	#endregion

	#region Person Picker

	/// <summary>
	/// Allows the user to find and select a person
	/// </summary>
	public class PersonPicker : System.Web.UI.Control, System.Web.UI.INamingContainer
	{
		protected HttpServerUtility	_Server;
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
			dgPeople.ItemCommand   += new DataGridCommandEventHandler(PersonItemSelect);
			dgPeople.GridLines		= GridLines.Vertical;
			dgPeople.BorderWidth	= Unit.Pixel(1);
			dgPeople.BorderColor	= Color.FromName("#999999");
			dgPeople.BackColor		= Color.White;
			dgPeople.CellPadding	= 3;
			dgPeople.AutoGenerateColumns = false;
			
			dgPeople.FooterStyle.BackColor	= Color.FromName("#CCCCCC");
			dgPeople.FooterStyle.ForeColor	= Color.Black;

			dgPeople.HeaderStyle.Font.Bold	= true;
			dgPeople.HeaderStyle.ForeColor	= Color.White;
			dgPeople.HeaderStyle.BackColor	= Color.FromName("#000084");

			dgPeople.SelectedItemStyle.Font.Bold	= true;
			dgPeople.SelectedItemStyle.ForeColor	= Color.White;
			dgPeople.SelectedItemStyle.BackColor	= Color.FromName("#FFFFCC");

			dgPeople.AlternatingItemStyle.BackColor	= Color.Gainsboro;
			dgPeople.AlternatingItemStyle.ForeColor	= Color.Black;

			dgPeople.ItemStyle.ForeColor	= Color.FromName("#WhiteSmoke");
			dgPeople.ItemStyle.BackColor	= Color.Black;

			ButtonColumn bc = new ButtonColumn();
			bc.DataTextField	= "FullName";
			bc.HeaderText		= "Name";
			bc.CommandName		= "Select";

			dgPeople.Columns.Add(bc);

		}


		private void PersonItemSelect(object sender, DataGridCommandEventArgs e) 
		{
			dgPeople.SelectedIndex = e.Item.ItemIndex;
			btnFind_Click(sender, e);

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
			this.Controls.Add(dgPeople);

			// Set up events
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			this.dgPeople.SelectedIndexChanged += new System.EventHandler(this.dgPeople_SelectedIndexChanged);

		}

		private void btnFind_Click (Object sender, EventArgs e) 
		{
			// set up connection and command
			SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("dbo.sp_Person_Find", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@name", txtName.Text);

			// Open connection, and display results
			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			dgPeople.DataSource = dr;
			dgPeople.DataKeyField = "PersonID";
			dgPeople.DataBind();
			cn.Close();
		}

		private void dgPeople_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (PersonSelected != null)
				PersonSelected(sender, e);
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
		
		public PeopleSelector() 
		{
			// Initialize stuff
			searchEntry = new TextBox();

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
			t.BackColor = Color.White;
			t.ForeColor = Color.Black;
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
			lit.Text = "Search Results<br>";
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
			SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
			SqlCommand cmd	 = new SqlCommand("dbo.sp_Person_Find", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@name", searchEntry.Text);

			// Open connection, and display results
			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			availablePeople.DataSource = dr;
			availablePeople.DataTextField = "FullName";
			availablePeople.DataValueField = "PersonID";
			availablePeople.DataBind();
			cn.Close();

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
	}
	#endregion

}
