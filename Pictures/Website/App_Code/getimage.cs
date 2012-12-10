using System;
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

namespace msn2.net.Pictures.Website
{
	public class GetImage: IHttpHandler
	{
		#region IHttpHandler Members

		public void ProcessRequest(HttpContext context)
		{
			int pictureId		= Convert.ToInt32(context.Request.QueryString["p"]);
			int maxWidth		= Convert.ToInt32(context.Request.QueryString["mw"]);
			int maxHeight		= Convert.ToInt32(context.Request.QueryString["mh"]);
			
			DataSet ds			= PicContext.Current.PictureManager.GetPicture(pictureId, maxWidth, maxHeight);

			// Check if we got the picture - user may not have access
			if (ds.Tables[0].Rows.Count == 1)
			{
				DataRow dr			= ds.Tables[0].Rows[0];
				string strCache		= PicContext.Current.Config.CacheDirectory;
				string filename		= dr["FileName"].ToString();

				filename = strCache + filename.Replace(@"\", @"/");

				context.Response.ContentType	 = "image/jpeg";
				using (Bitmap img = new Bitmap(filename))
				{
					img.Save(context.Response.OutputStream, ImageFormat.Jpeg);
				}
			}
			else
			{
				context.Response.ContentType	= "image/gif";
				string filename			= context.Server.MapPath("images/folder.gif");
				using (Bitmap img = new Bitmap(filename))
				{
					Bitmap destBitmap = new Bitmap(maxWidth, maxHeight, PixelFormat.Format24bppRgb);
					Graphics g			= Graphics.FromImage(destBitmap);
					g.DrawImage(img, 0, 0, maxWidth, maxHeight);
					g.Dispose();

					destBitmap.Save(context.Response.OutputStream, ImageFormat.Gif);
				}
			}
		}

		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		#endregion
	}
}
