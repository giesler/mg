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
	/// Summary description for Cdefault.
	/// </summary>
	public class Cdefault : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Panel randomPicture;
		protected System.Web.UI.WebControls.TextBox searchQuery;
		protected System.Web.UI.WebControls.Button search;
		protected pics.Controls.ContentPanel searchPanel;
		protected pics.Controls.Sidebar Sidebar1;
		protected pics.Controls.ContentPanel welcomeMessage;
		protected pics.Controls.CategoryListViewItem rootCategory;
		protected System.Web.UI.WebControls.DataList dlRecent;
	
		public Cdefault()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			// load the person's info
			PersonInfo pi = (PersonInfo) Session["PersonInfo"];

			// open connection and command
			SqlConnection cn  = new SqlConnection(pics.Config.ConnectionString);
            SqlCommand cmd    = new SqlCommand("dbo.sp_RecentCategories", cn);
			cmd.CommandType   = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PersonID", pi.PersonID);

			Literal br = new Literal();
			br.Text = "<br>";

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();

			dlRecent.DataSource = dr;
			dlRecent.DataBind();

			dr.Close();
			cn.Close();

			// Set up SP to retreive pictures
			SqlDataAdapter daPics = new SqlDataAdapter("dbo.p_RandomPicture", cn);
			daPics.SelectCommand.CommandType = CommandType.StoredProcedure;

			// set up params on the SP
			daPics.SelectCommand.Parameters.Add("@PersonID", pi.PersonID);
			daPics.SelectCommand.Parameters.Add("@MaxWidth", 125);
			daPics.SelectCommand.Parameters.Add("@MaxHeight", 125);

			// run the SP, set datasource to the picture list
			cn.Open();
			DataSet dsPics = new DataSet();
			daPics.Fill(dsPics, "Pictures");
			cn.Close();

			// create new control
			ThumbnailList thumbs = new ThumbnailList();
			thumbs.PageReturnURL	= "picview.aspx?p=" + dsPics.Tables["Pictures"].Rows[0]["PictureID"].ToString()
				+ "&type=random&RefUrl=default.aspx"; //"default.aspx";
			thumbs.ThumbsDataSource = dsPics.Tables["Pictures"].DefaultView;
//			thumbs.PageNavURL		= "picview.aspx?p=" + dsPics.Tables["Pictures"].Rows[0]["PictureID"].ToString();
			randomPicture.Controls.Add(thumbs);

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
	}
}
