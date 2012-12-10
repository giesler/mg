using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;

namespace msn2.net.Pictures
{
    public class GetImageHttpHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            int pictureId = Convert.ToInt32(context.Request.QueryString["p"]);
            int maxWidth = Convert.ToInt32(context.Request.QueryString["mw"]);
            int maxHeight = Convert.ToInt32(context.Request.QueryString["mh"]);

            DataSet ds = PicContext.Current.PictureManager.GetPicture(pictureId, maxWidth, maxHeight);

            // Check if we got the picture - user may not have access
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string strCache = PicContext.Current.Config.CacheDirectory;
                string filename = dr["FileName"].ToString();

                filename = strCache + filename.Replace(@"\", @"/");

                context.Response.ContentType = "image/jpeg";

                using (Bitmap img = new Bitmap(filename))
                {
                    using (Bitmap baseBitmap = new Bitmap(maxWidth + 2, img.Height + 2))
                    {
                        Graphics g = Graphics.FromImage(baseBitmap);

                        using (SolidBrush background = new SolidBrush(Color.FromArgb(93, 100, 109)))
                        {
                            g.FillRectangle(background, 0, 0, maxWidth + 2, img.Height + 2);
                        }

                        int left = 0;

                        if (img.Height > img.Width)
                        {
                            left = (maxWidth / 2) - (img.Width / 2);
                        }
                        else
                        {
                            left = 0;
                        }

                        g.DrawImage(img, left + 1, 1, img.Width, img.Height);

                        using (Pen pen = new Pen(Color.Silver))
                        {
                            g.DrawRectangle(pen, left, 0, img.Width + 1, img.Height + 1);
                        }

                        baseBitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                    }
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
