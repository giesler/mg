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
	/// Summary description for picview.
	/// </summary>
	public partial class picviewss 
	{
		#region Declares
		protected System.Web.UI.WebControls.Panel pnlDescription;
		protected System.Web.UI.WebControls.Panel pnlPeople;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.HtmlControls.HtmlTableRow titleRow;
		protected String m_HttpRefreshURL;
		#endregion

// 		public picviewss()
// 		{
// 			Page.Init += new System.EventHandler(Page_Init);
// 		}

		private void Page_Load(object sender, System.EventArgs e)
		{

			// Set link to return to list
			if (Request.QueryString["RefURL"] != null) 
			{
				//lnkReturn.Text = "Return<br>to list";
				lnkReturn.NavigateUrl = Request.QueryString["RefURL"];
			}

			// make sure we have a picid
			if (Request.QueryString["r"] == null && Request.QueryString["p"] == null) 
			{
				lblTitle.Text = "There was no record number passed to the page.";
			} 
			else 
			{
				// load the person's info

				// figure out the source type
				string sourceType = Request.QueryString["type"];

				// init connection and command
				SqlConnection cn  = new SqlConnection(PicContext.Current.Config.ConnectionString);

				// Set up SP to retreive pictures
				SqlCommand cmdPic    = new SqlCommand();
				if (sourceType.Equals("category"))
				{
					cmdPic.CommandText = "p_Category_GetPictures";	/// switch based on type
					SetCategory(Convert.ToInt32(Request.QueryString["c"]));
				}
				else if (sourceType.Equals("search"))
				{
					cmdPic.CommandText = "p_Search_GetPictures";	/// switch based on type
				}
				else if (sourceType.Equals("random"))
				{
					cmdPic.CommandText = "p_GetPicture";
				}
				cmdPic.CommandType   = CommandType.StoredProcedure;
				cmdPic.Connection    = cn;
				SqlDataAdapter daPic = new SqlDataAdapter(cmdPic);

				if (sourceType.Equals("category"))
				{
					//lblCategory.Text	= "toget: category";
				}
				else if (sourceType.Equals("search"))
				{
					lblCategory.Text	= "Search Results";
				}
				else if (sourceType.Equals("random"))
				{
					lblCategory.Text	= "Random Picture";
				}

				// set up params on the SP
				if (sourceType.Equals("category")) 
				{
					cmdPic.Parameters.Add("@CategoryID", Convert.ToInt32(Request.QueryString["c"]));
				}
				else if (sourceType.Equals("search")) 
				{
					Guid id		= new Guid(Request.QueryString["id"]);
					cmdPic.Parameters.Add("@SearchID", id);
				} 
				else if (sourceType.Equals("random")) 
				{
					cmdPic.Parameters.Add("@PictureID", Convert.ToInt32(Request.QueryString["p"]));
				}
				cmdPic.Parameters.Add("@StartRecord", Convert.ToInt32(Request.QueryString["r"]));
				cmdPic.Parameters.Add("@ReturnCount", 1);
				cmdPic.Parameters.Add("@MaxHeight", 700);
				cmdPic.Parameters.Add("@MaxWidth", 750);
				cmdPic.Parameters.Add("@PersonID", 1);
				cmdPic.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
				cmdPic.Parameters["@TotalCount"].Direction = ParameterDirection.Output;

				// run the SP, set datasource to the picture list
				cn.Open();
				DataSet ds = new DataSet();
				daPic.Fill(ds, "Picture");
				DataRow dr = ds.Tables[0].Rows[0];

				// now set the controls on the page
				if (!dr.IsNull("Title") && dr["Title"].ToString().Length > 0)
					lblTitle.Text = dr["Title"].ToString() + "<br>";
				else
					lblTitle.Visible = false;
				if (!dr.IsNull("PictureDate"))
					lblPictureDate.Text = Convert.ToDateTime(dr["PictureDate"]).ToLongDateString();
				else
					lblPictureDate.Visible = false;
				if (!dr.IsNull("Description"))
					lblPictureDesc.Text = dr["Description"].ToString();
				else
					lblPictureDesc.Text = "";

				// now create the picture
				Picture curPic = new Picture();
				//curPic.Filename = dr["Filename"].ToString();
				curPic.SetPictureById((int) dr["PictureId"], 700, 750);
				curPic.Height   = Convert.ToInt32(dr["Height"]);
				curPic.Width	= Convert.ToInt32(dr["Width"]);
				curPic.ID		= "currentPicture";
				tdPicture.Controls.Add(curPic);

				// now read people
                SqlCommand cmdPerson = new SqlCommand();
				cmdPerson.CommandText = "sp_Picture_GetPeople";
				cmdPerson.CommandType = CommandType.StoredProcedure;
				cmdPerson.Connection  = cn;

				// set up params to SP
				cmdPerson.Parameters.Add("@PictureID", Convert.ToInt32(dr["PictureID"]));

				// now read the data
                SqlDataReader drPerson = cmdPerson.ExecuteReader();
				dlPerson.DataSource = drPerson;
				dlPerson.DataBind();

//				if (dlPerson.Items.Count == 0)
//					pnlPeople.Visible = false;

				// close people reader
                drPerson.Close();

				// close connection
				cn.Close();


				// Now set page controls
				int intCurRec = Convert.ToInt32(Request.QueryString["r"]);
				int intTotalCount = Convert.ToInt32(cmdPic.Parameters["@TotalCount"].Value);

				String strURL = Request.Path + "?" + Request.ServerVariables["QUERY_STRING"];
				int intRecPos = strURL.IndexOf("r=");
				if (intRecPos > 0) 
					strURL = strURL.Substring(0, intRecPos) + "r={0}" 
						+ strURL.Substring(strURL.IndexOf("&", intRecPos));

				// check if in slideshow mode
				if (Request.QueryString["ss"] != null) 
				{

					if (intCurRec == intTotalCount) 
					{
						Control c = LoadControl("Controls//AutoTimer.ascx");
						AutoTimer at			= (AutoTimer) c;
						at.NavigateUrl			= lnkReturn.NavigateUrl;
						at.Seconds				= 15;
						at.Visible				= true;
//						m_HttpRefreshURL = "10;URL=" + lnkReturn.NavigateUrl;
						panelNext.Controls.Add(at);

						lnkNext.Visible			= true;
						lnkNext.NavigateUrl		= lnkReturn.NavigateUrl;
					} 
					else 
					{
						Control c = LoadControl("Controls//AutoTimer.ascx");
						AutoTimer at			= (AutoTimer) c;
						at.NavigateUrl			= strURL.Replace("{0}", Convert.ToString(intCurRec+1)) + "#title";
						at.Seconds				= 15;
						at.Visible				= true;
						panelNext.Controls.Add(at);

						lnkNext.Visible			= true;
						lnkNext.NavigateUrl		= lnkReturn.NavigateUrl;
					}

					nextBarNote.Visible			= true;

				} 
				else 
				{

					// make sure not on first picture
					if (intCurRec > 1) 
					{
						lnkPrevious.Visible = true;
						lnkPrevious.NavigateUrl = strURL.Replace("{0}", Convert.ToString(intCurRec-1));
					}

					// make sure not on last picture
					if (intCurRec < intTotalCount && !sourceType.Equals("random")) 
					{
						lnkNext.Visible = true;
						lnkNext.NavigateUrl = strURL.Replace("{0}", Convert.ToString(intCurRec+1));
					}
				}

				if (sourceType.Equals("random"))
				{
					pictureLocation.Visible = false;
				}
				else
				{
					lblPicture.Text  = intCurRec.ToString();
					lblPictures.Text = intTotalCount.ToString();
				}

				// if in random mode, hide page controls
//				if (sourceType.Equals("random"))
//					pnlPageControls.Visible = false;

			}

		}

		private void Page_Init(object sender, EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}

		public String HttpRefreshURL 
		{
			get 
			{
				return m_HttpRefreshURL;
			}
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
		#region Private Methods
		private void SetCategory(int categoryId)
		{
			CategoryManager catMan		= PicContext.Current.CategoryManager;
			Category cat				= catMan.GetCategory(categoryId);
			lblCategory.Text			= cat.Name;

		}
		#endregion
	}
}
