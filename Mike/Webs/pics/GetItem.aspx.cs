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
using System.Drawing.Imaging;
using msn2.net.Pictures;

namespace pics
{
	/// <summary>
	/// Summary description for GetItem.
	/// </summary>
	public class GetItem : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			int pictureId		= Convert.ToInt32(Request.QueryString["p"]);
			int maxWidth		= Convert.ToInt32(Request.QueryString["mw"]);
			int maxHeight		= Convert.ToInt32(Request.QueryString["mh"]);
			
			DataSet ds			= PicContext.Current.PictureManager.GetPicture(pictureId, maxWidth, maxHeight);
			DataRow dr			= ds.Tables[0].Rows[0];
            
			string strCache		= PicContext.Current.Config.CacheDirectory;

			string filename = dr["FileName"].ToString();

			filename = strCache + filename.Replace(@"\", @"/");

			Response.ContentType	 = "image/jpeg";
			using (Bitmap img = new Bitmap(filename))
			{
				img.Save(Response.OutputStream, ImageFormat.Jpeg);
			}

			Response.End();

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
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

