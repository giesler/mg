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
	public partial class Cdefault : Page
	{
		#region Declares

	
		#endregion

		#region Constructor

// 		public Cdefault()
// 		{
// 			Page.Init += new System.EventHandler(Page_Init);
// 		}

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
            int pictureId = Convert.ToInt32(dsPics.Tables["Pictures"].Rows[0]["PictureID"].ToString());
			ThumbnailList thumbs = new ThumbnailList();
			thumbs.PageReturnURL	= pics.picview.BuildRandomPageUrl(pictureId, "default.aspx");
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

		}
		#endregion

	}
}
