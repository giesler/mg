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
		protected System.Web.UI.WebControls.Label lblCurrentCategory;
		protected System.Web.UI.WebControls.Label lblCategoryDesc;

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

		public Categories()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{

			// Figure out what to use as the root ID
			rootCategoryId = Request.QueryString["r"];
			currentCategoryId = Request.QueryString["c"];

			if (rootCategoryId == null) 
				rootCategoryId = "1";
			if (currentCategoryId == null)
				currentCategoryId = rootCategoryId;

			if (!Page.IsPostBack) 
			{
				// Get the category table
				DataSetCategory dsCat = GetCategories();

				// find the current node's info
				DataView currentNodeInfo = new DataView(dsCat.Category);
				currentNodeInfo.RowFilter = "CategoryID = " + currentCategoryId;
				DataSetCategory.CategoryRow categoryRow = (DataSetCategory.CategoryRow) currentNodeInfo[0].Row;

				// set this info on the page
				
				currentCategory.Text = categoryRow.CategoryName;
				lblCurrentCategory.Text = categoryRow.CategoryName;
				if (!categoryRow.IsCategoryDescriptionNull())
					lblCategoryDesc.Text = categoryRow.CategoryDescription;
                

				// child areas
				DataView childNodeInfo  = new DataView(dsCat.Category);
				childNodeInfo.RowFilter = "CategoryParentID = " + currentCategoryId + " And CategoryID <> CategoryParentID";
				childNodeInfo.Sort	    = "CategoryName";

				// fill the child control list with links
				for (int i = 0; i < childNodeInfo.Count; i++) 
				{
					DataSetCategory.CategoryRow childRow = (DataSetCategory.CategoryRow) childNodeInfo[i].Row;
					
					HyperLink lnk = new HyperLink();
					lnk.Text		= childRow.CategoryName;
					lnk.NavigateUrl = "Categories.aspx?r=" + rootCategoryId + "&c=" + childRow.CategoryID.ToString();
					lnk.CssClass	= "note";
					childCategoryList.Controls.Add(lnk);

					Literal lit = new Literal();
					lit.Text = "<br><br>";
					childCategoryList.Controls.Add(lit);

				}

				// if no children, hide this control
				if (childNodeInfo.Count == 0)
					childCategoryList.Visible = false;

				LoadPictures();

				// Now load the 'you are here' control
				ArrayList arrayHere = new ArrayList();
				DataSetCategory.CategoryRow row = categoryRow;
				while (row.CategoryID != Convert.ToInt32(rootCategoryId)) 
				{
					// create a view to find the parent
                    DataView parentView = new DataView(dsCat.Category);
					parentView.RowFilter = "CategoryID = " + row.CategoryParentID.ToString();

					// add the parent to the array
					row = (DataSetCategory.CategoryRow) parentView[0].Row;
					arrayHere.Add(row.CategoryID);
				}

				// Now output in reverse order
				for (int i = arrayHere.Count-1; i >= 0; i--) 
				{
					// find the title of this node
					DataView itemView = new DataView(dsCat.Category);
					itemView.RowFilter = "CategoryID = " + arrayHere[i].ToString();
					DataSetCategory.CategoryRow itemRow = (DataSetCategory.CategoryRow) itemView[0].Row;

					// create a link to this page
					HyperLink link = new HyperLink();
					link.Text = itemRow.CategoryName;
					link.NavigateUrl = "Categories.aspx?r=" + rootCategoryId + "&c=" + itemRow.CategoryID.ToString();
					youAreHerePanel.Controls.Add(link);

					// add a divider
					Literal lit = new Literal();
					lit.Text = " \\ ";
					youAreHerePanel.Controls.Add(lit);
				}

				// And add the current category
				Label curCategory = new Label();
				curCategory.Text = categoryRow.CategoryName;
				curCategory.Font.Bold = true;
				youAreHerePanel.Controls.Add(curCategory);
				
			}

		}

		private DataSetCategory GetCategories() 
		{
			// load the person's info
			PersonInfo pi = (PersonInfo) Session["PersonInfo"];

			// get a dataset with categories
			SqlConnection cn  = new SqlConnection("data source=kyle;initial catalog=picdb;user id=sa;password=too;persist security info=False");
			SqlDataAdapter da = new SqlDataAdapter("dbo.sp_Category_GetCategories", cn);
			da.SelectCommand.Parameters.Add("@PersonID", pi.PersonID);
			da.SelectCommand.CommandType = CommandType.StoredProcedure;
			DataSetCategory dsCat = new DataSetCategory();
			da.Fill(dsCat, "Category");				
			return dsCat;
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
			SqlConnection cn  = new SqlConnection("data source=kyle;initial catalog=picdb;user id=sa;password=too;persist security info=False");

			// Set up SP to retreive pictures
			SqlDataAdapter daPics = new SqlDataAdapter("dbo.sp_Category_GetPictures", cn);
			daPics.SelectCommand.CommandType = CommandType.StoredProcedure;

			// set up params on the SP
			daPics.SelectCommand.Parameters.Add("@CategoryID", Convert.ToInt32(currentCategoryId));
			daPics.SelectCommand.Parameters.Add("@StartRecord", startRecord);
			daPics.SelectCommand.Parameters.Add("@ReturnCount", 15);
			daPics.SelectCommand.Parameters.Add("@PersonID", pi.PersonID);
			daPics.SelectCommand.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
			daPics.SelectCommand.Parameters["@TotalCount"].Direction = ParameterDirection.Output;

            // run the SP, set datasource to the picture list
			cn.Open();
			DataSet dsPics = new DataSet();
			daPics.Fill(dsPics, "Pictures");
			cn.Close();

			// create new control
			PagedThumbnailList thumbs = new PagedThumbnailList();
			thumbs.ShowRecordNumber	= true;
			thumbs.PageReturnURL	= PicPageURL;
			thumbs.ThumbsDataSource = dsPics.Tables["Pictures"].DefaultView;
			thumbs.TotalRecords		= Convert.ToInt32(daPics.SelectCommand.Parameters["@TotalCount"].Value);
			thumbs.StartRecord		= startRecord;
			thumbs.RecordsPerPage	= 15;
			thumbs.NoPictureMessage	= "<b>There are no pictures in this category.</b><br>Please select another category from the left.";
			thumbs.PageNavURL		= Request.Path + "?r=" + rootCategoryId	+ "&c=" + currentCategoryId + "&sr={0}";
			pnlthumbs.Controls.Add(thumbs);

			// Show the slideshow link if there are pictures
			if (thumbs.HasPictures) 
			{
				lnkSlideshow.Visible = true;
				lnkSlideshow.NavigateUrl = String.Format(PicPageURL + "&ss=1#title", startRecord.ToString());
			}
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
