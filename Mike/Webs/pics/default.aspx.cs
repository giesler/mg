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
	/// Summary description for Cdefault.
	/// </summary>
	public class Cdefault : System.Web.UI.Page
	{
		#region Declares

		protected System.Web.UI.WebControls.Panel randomPicture;
		protected System.Web.UI.WebControls.TextBox searchQuery;
		protected System.Web.UI.WebControls.Button search;
		protected pics.Controls.ContentPanel searchPanel;
		protected pics.Controls.Sidebar Sidebar1;
		protected pics.Controls.ContentPanel welcomeMessage;
		protected pics.Controls.CategoryListViewItem rootCategory;
		protected pics.Controls.Header header;
		protected System.Web.UI.WebControls.Panel recentPictures;
		protected pics.Controls.ContentPanel contentRecentPictures;
		protected pics.Controls.ContentPanel browsePicturesContent;
		protected pics.Controls.ContentPanel contentRandomPicture;
		protected pics.Controls.OpenMainFormLink mainFormLink;
		protected pics.Controls.ContentPanel searchContent;
		protected System.Web.UI.WebControls.Panel adminMode;
		protected System.Web.UI.WebControls.DataList dlRecent;
	
		#endregion

		#region Constructor

		public Cdefault()
		{
			Page.Init += new System.EventHandler(Page_Init);
		}

		#endregion
        
		private void Page_Load(object sender, System.EventArgs e)
		{			
			Literal br = new Literal();
			br.Text = "<br>";

			DataSet ds = PicContext.Current.CategoryManager.RecentCategorires();

			dlRecent.DataSource = ds;
			dlRecent.DataBind();

			// run the SP, set datasource to the picture list
			DataSetPicture dsPics	= PicContext.Current.PictureManager.RandomImageData();

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
