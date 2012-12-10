using System;
using System.Collections;
using System.Collections.Generic;
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

    public class PictureRecord 
    {
        public PictureRecord(msn2.net.Pictures.Picture picture, int record)
        {
            this.RecNumber = record;
            this.Picture = picture;
            this.PictureId = picture.Id;
            this.Title = picture.Title;
        }

        public int RecNumber { get; private set; }
        public msn2.net.Pictures.Picture Picture { get; private set; }
        public string Title { get; private set; }
        public int PictureId { get; private set; }
    }

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

			List<Category> recentCategories = PicContext.Current.CategoryManager.RecentCategorires();

            dlRecent.DataSource = recentCategories;
			dlRecent.DataBind();

			// run the SP, set datasource to the picture list
			msn2.net.Pictures.Picture pic = PicContext.Current.PictureManager.GetRandomPicture();
            List<PictureRecord> pics = new List<PictureRecord>();
            pics.Add(new PictureRecord(pic, 1));

			// create new control
            if (pic != null)
            {
                ThumbnailList thumbs = new ThumbnailList();
                thumbs.PageReturnURL = BuildRandomPageUrl(pic.Id, "default.aspx");
                thumbs.ThumbsDataSource = pics;
                //			thumbs.PageNavURL		= "picview.aspx?p=" + dsPics.Tables["Pictures"].Rows[0]["PictureID"].ToString();
                randomPicture.Controls.Add(thumbs);
            }
            else
            {
                Label label = new Label();
                label.Text = "You do not have permission to view any pictures.";
                contentRecentPictures.Controls.Add(label);
            }
		}

        public static string BuildRandomPageUrl(int pictureId, string refUrl)
        {
            string url = "picview.aspx?p=" + pictureId.ToString()
                + "&type=random&RefUrl=" + HttpContext.Current.Server.UrlEncode(refUrl);

            return url;
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
