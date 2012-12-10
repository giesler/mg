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
	/// Summary description for SearchResults.
	/// </summary>
	public class SearchResults : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.HyperLink ReturnToCriteria;
		protected System.Web.UI.WebControls.HyperLink lnkSlideshow;
		protected System.Web.UI.WebControls.Panel youAreHerePanel;
		protected System.Web.UI.WebControls.Label searchDescription;
		protected pics.Controls.Header header;
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
				ReturnToCriteria.NavigateUrl = "SearchCriteria.aspx?id=" + Request.QueryString["id"].ToString();

				// get the byte array from the guid passed
				XMGuid.Init();
				XMGuid g = new XMGuid(Request.QueryString["id"]);

				// get the search description
				SqlConnection cn = new SqlConnection(pics.Config.ConnectionString);
				SqlCommand cmd   = new SqlCommand("select SearchDescription from Search where SearchID = @SearchID", cn);
				cmd.Parameters.Add("@SearchID", g.Buffer);
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
			// load the person's info
			PersonInfo pi = (PersonInfo) Session["PersonInfo"];

			string id = Request.QueryString["id"];
	
			// get the byte array from the guid passed
			XMGuid.Init();
			XMGuid g = new XMGuid(id);

			// see if a start record is set in QS - this would override anything passed
			if (Request.QueryString["sr"] != null)
				intStartRecord = Convert.ToInt32(Request.QueryString["sr"]);

			// make sure intStartRecord is at least 1
			if (intStartRecord < 1) intStartRecord = 1;

			// init connection and command to get pictures
			SqlConnection cn  = new SqlConnection(pics.Config.ConnectionString);

			// Set up SP to retreive pictures
			SqlDataAdapter daPics = new SqlDataAdapter("dbo.p_Search_GetPictures", cn);
			daPics.SelectCommand.CommandType = CommandType.StoredProcedure;

			// set up params on the SP
			daPics.SelectCommand.Parameters.Add("@SearchID", g.Buffer);
			daPics.SelectCommand.Parameters.Add("@StartRecord", intStartRecord);
			daPics.SelectCommand.Parameters.Add("@ReturnCount", 15);
			daPics.SelectCommand.Parameters.Add("@PersonID", pi.PersonID);
			daPics.SelectCommand.Parameters.Add("@MaxHeight", 125);
			daPics.SelectCommand.Parameters.Add("@MaxWidth", 125);
			daPics.SelectCommand.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
			daPics.SelectCommand.Parameters["@TotalCount"].Direction = ParameterDirection.Output;

			// run the SP, set datasource to the picture list
			cn.Open();
			DataSet dsPics = new DataSet();
			daPics.Fill(dsPics, "Pictures");
			cn.Close();

			// create new control
			PagedThumbnailList thumbs = new PagedThumbnailList();
			thumbs.PageReturnURL	= PicPageURL;
			thumbs.ShowRecordNumber	= true;
			thumbs.ThumbsDataSource = dsPics.Tables["Pictures"].DefaultView;
			thumbs.TotalRecords		= Convert.ToInt32(daPics.SelectCommand.Parameters["@TotalCount"].Value);
			thumbs.StartRecord		= intStartRecord;
			thumbs.RecordsPerPage	= 15;
			thumbs.NoPictureMessage	= "<b>There are no pictures in this category.</b><br>Please select another category from the left tree.";
			thumbs.PageNavUrl		= Request.Path + "?id=" + id + "&sr={0}";
			pnlthumbs.Controls.Add(thumbs);

			// Show the slideshow link if there are pictures
			if (thumbs.HasPictures) 
			{
				lnkSlideshow.Visible = true;
				lnkSlideshow.NavigateUrl = String.Format(PicPageURL + "&ss=1", intStartRecord);

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
