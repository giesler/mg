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
using msn2.net.Pictures;

namespace pics
{
	/// <summary>
	/// Summary description for SearchResults.
	/// </summary>
	public class SearchResults : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.HyperLink ReturnToCriteria;
		protected System.Web.UI.WebControls.HyperLink lnkSlideshow;
		protected System.Web.UI.WebControls.Panel youAreHerePanel;
		protected System.Web.UI.WebControls.Label searchDescription;
		protected pics.Controls.Header header;
		protected pics.Controls.PictureTasks picTaskList;
		protected pics.Controls.ContentPanel picTasks;
		protected pics.Controls.ContentPanel Contentpanel1;
		protected pics.Controls.Sidebar Sidebar1;
		protected System.Web.UI.WebControls.Panel childCategoryList;
		protected System.Web.UI.WebControls.Label resultCount;
		protected System.Web.UI.WebControls.Panel pnlthumbs;
	
		public SearchResults()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
                // make sure we were passed a search id
				if (Request.QueryString["id"] == null)
					Response.Redirect("SearchCriteria.aspx");
				
				// set the URL to return to search criteria page
				// TODO: ReturnToCriteria.NavigateUrl = "SearchCriteria.aspx?id=" + Request.QueryString["id"].ToString();

				// get the byte array from the guid passed
				Guid g = new Guid(Request.QueryString["id"]);

				// get the search description
				SqlConnection cn = new SqlConnection(PicContext.Current.Config.ConnectionString);
				SqlCommand cmd   = new SqlCommand("select SearchDescription from Search where SearchID = @SearchID", cn);
				cmd.Parameters.Add("@SearchID", g);
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				
				// attempt to read search descirption
				if (dr.Read()) 
				{
                    searchDescription.Text = dr["SearchDescription"].ToString();
				}

				dr.Close();
				cn.Close();

				LoadPictures(1);
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

		
		private void LoadPictures(int intStartRecord)
		{
			string id = Request.QueryString["id"];

			int pageSize = 20;
	
			// get the byte array from the guid passed
			Guid g = new Guid(id);

			// see if a start record is set in QS - this would override anything passed
			if (Request.QueryString["sr"] != null)
				intStartRecord = Convert.ToInt32(Request.QueryString["sr"]);

			// make sure intStartRecord is at least 1
			if (intStartRecord < 1) intStartRecord = 1;

			// init connection and command to get pictures
			SqlConnection cn  = new SqlConnection(PicContext.Current.Config.ConnectionString);

			// Set up SP to retreive pictures
			SqlDataAdapter daPics = new SqlDataAdapter("dbo.p_Search_GetPictures", cn);
			daPics.SelectCommand.CommandType = CommandType.StoredProcedure;

			// set up params on the SP
			daPics.SelectCommand.Parameters.Add("@SearchID", g);
			daPics.SelectCommand.Parameters.Add("@StartRecord", intStartRecord);
			daPics.SelectCommand.Parameters.Add("@ReturnCount", pageSize);
			daPics.SelectCommand.Parameters.Add("@PersonID", PicContext.Current.CurrentUser.Id);
			daPics.SelectCommand.Parameters.Add("@MaxHeight", 125);
			daPics.SelectCommand.Parameters.Add("@MaxWidth", 125);
			daPics.SelectCommand.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
			daPics.SelectCommand.Parameters["@TotalCount"].Direction = ParameterDirection.Output;

			// run the SP, set datasource to the picture list
			cn.Open();
			DataSet dsPics = new DataSet();
			daPics.Fill(dsPics, "Pictures");
			cn.Close();

			int totalCount			= Convert.ToInt32(daPics.SelectCommand.Parameters["@TotalCount"].Value);

			// create new control
			PagedThumbnailList thumbs = new PagedThumbnailList();
			thumbs.PageReturnURL	= PicPageURL;
			thumbs.ShowRecordNumber	= false;
			thumbs.ThumbsDataSource = dsPics.Tables["Pictures"].DefaultView;
			thumbs.TotalRecords		= totalCount;
			thumbs.StartRecord		= intStartRecord;
			thumbs.RecordsPerPage	= pageSize;
			thumbs.NoPictureMessage	= "<b>There are no pictures in this category.</b><br>Please select another category from the left tree.";
			thumbs.PageNavUrl		= Request.Path + "?id=" + id + "&sr={0}";
			pnlthumbs.Controls.Add(thumbs);

			// Show the slideshow link if there are pictures
			if (thumbs.HasPictures) 
			{
				picTasks.Visible			= true;
				picTaskList.SetSlideshowUrl(string.Format(PicPageURL + "&ss=1", intStartRecord));

				if (totalCount == 1)
				{
					resultCount.Text	= "1 picture found";
				}
				else
				{
					resultCount.Text	= string.Format("{0} pictures found", totalCount);
				}
			}
		}

		public String PicPageURL 
		{
			get 
			{
				// get the searchid
				string id = Request.QueryString["id"];

				String strURL = "picview.aspx?r={0}&id=" + id
					+ "&RefURL=" + Server.UrlEncode(Request.FilePath + "?id=" + id) + "&type=search";
				return strURL;
			}

		}

	}
}
