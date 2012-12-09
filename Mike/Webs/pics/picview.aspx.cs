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
	/// Summary description for picview.
	/// </summary>
	public class picview : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblTitle;
		protected System.Web.UI.WebControls.Label lblPictureDate;
		protected System.Web.UI.WebControls.Label lblPictureDesc;
		protected System.Web.UI.WebControls.HyperLink lnkPrevious;
		protected System.Web.UI.WebControls.Label lblPicture;
		protected System.Web.UI.WebControls.Label lblPictures;
		protected System.Web.UI.WebControls.HyperLink lnkNext;
		protected System.Web.UI.WebControls.Panel pnlPageControls;
		protected System.Web.UI.WebControls.HyperLink lnkReturn;
		protected System.Web.UI.WebControls.DataList dlPerson;
		protected System.Web.UI.HtmlControls.HtmlTableCell tdPicture;
		protected System.Web.UI.WebControls.Panel pnlDescription;
		protected System.Web.UI.WebControls.Panel pnlPeople;
	
		protected String m_HttpRefreshURL;

		public picview()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{

			// Set link to return to list
			if (Request.QueryString["RefURL"] != null) 
			{
				lnkReturn.Text = "Return to list of pictures";
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
				PersonInfo pi = (PersonInfo) Session["PersonInfo"];

				// figure out the source type
				string sourceType = Request.QueryString["type"];

				// init connection and command
				SqlConnection cn  = new SqlConnection(pics.Config.ConnectionString);

				// Set up SP to retreive pictures
				SqlCommand cmdPic    = new SqlCommand();
				if (sourceType.Equals("category"))
					cmdPic.CommandText = "sp_Category_GetPictures";	/// switch based on type
				else if (sourceType.Equals("search"))
					cmdPic.CommandText = "sp_Search_GetPictures";	/// switch based on type
				else if (sourceType.Equals("random"))
					cmdPic.CommandText = "sp_GetPicture";
				cmdPic.CommandType   = CommandType.StoredProcedure;
				cmdPic.Connection    = cn;
				SqlDataAdapter daPic = new SqlDataAdapter(cmdPic);

				// set up params on the SP
				if (sourceType.Equals("category")) 
				{
					cmdPic.Parameters.Add("@CategoryID", Convert.ToInt32(Request.QueryString["c"]));
				}
				else if (sourceType.Equals("search")) 
				{
					XMGuid.Init();
					XMGuid g = new XMGuid(Request.QueryString["id"]);

					cmdPic.Parameters.Add("@SearchID", g.Buffer);
				} 
				else if (sourceType.Equals("random")) 
				{
					cmdPic.Parameters.Add("@PictureID", Convert.ToInt32(Request.QueryString["p"]));
				}
				cmdPic.Parameters.Add("@StartRecord", Convert.ToInt32(Request.QueryString["r"]));
				cmdPic.Parameters.Add("@ReturnCount", 1);
				cmdPic.Parameters.Add("@PersonID", pi.PersonID);
				cmdPic.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
				cmdPic.Parameters["@TotalCount"].Direction = ParameterDirection.Output;

				// run the SP, set datasource to the picture list
				cn.Open();
				DataSetPicture dsPicture = new DataSetPicture();
				daPic.Fill(dsPicture, "Picture");

				// now set the controls on the page
				DataSetPicture.PictureRow pr = (DataSetPicture.PictureRow) dsPicture.Picture.Rows[0];
				if (!pr.IsTitleNull())
					lblTitle.Text = pr.Title;
				else
					lblTitle.Visible = false;
				if (!pr.IsPictureDateNull())
					lblPictureDate.Text = pr.PictureDate.ToShortDateString();
				else
					lblPictureDate.Visible = false;
				if (!pr.IsDescriptionNull())
					lblPictureDesc.Text = pr.Description;
				else
					pnlDescription.Visible = false;

				// now create the picture
				Picture curPic = new Picture(Server);
				curPic.Filename = pr.Filename;
				curPic.MaxHeight = 700;
				curPic.MaxWidth  = 750;
				tdPicture.Controls.Add(curPic);

				// now read people
                SqlCommand cmdPerson = new SqlCommand();
				cmdPerson.CommandText = "sp_Picture_GetPeople";
				cmdPerson.CommandType = CommandType.StoredProcedure;
				cmdPerson.Connection  = cn;

				// set up params to SP
				cmdPerson.Parameters.Add("@PictureID", pr.PictureID );

				// now read the data
                SqlDataReader drPerson = cmdPerson.ExecuteReader();
				dlPerson.DataSource = drPerson;
				dlPerson.DataBind();

				if (dlPerson.Items.Count == 0)
					pnlPeople.Visible = false;

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
						m_HttpRefreshURL = "5;URL=" + lnkReturn.NavigateUrl;
					} 
					else 
					{
						m_HttpRefreshURL = "5;URL=" 
							+ strURL.Replace("{0}", Convert.ToString(intCurRec+1));
					}

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
					if (intCurRec < intTotalCount) 
					{
						lnkNext.Visible = true;
						lnkNext.NavigateUrl = strURL.Replace("{0}", Convert.ToString(intCurRec+1));
					}
				}

				lblPicture.Text  = intCurRec.ToString();
				lblPictures.Text = intTotalCount.ToString();

				// if in random mode, hide page controls
				if (sourceType.Equals("random"))
					pnlPageControls.Visible = false;

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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
