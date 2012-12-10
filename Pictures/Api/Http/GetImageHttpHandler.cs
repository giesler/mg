using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
            string backColorString = "";
            if (context.Request.QueryString["bc"] != null)
            {
                backColorString = context.Request.QueryString["bc"];
            }
            Color backColor = Color.FromArgb(93, 100, 109);
            if (backColorString.Length > 0)
            {
                backColor = Color.FromName(backColorString);
            }
            if (maxWidth == 0)
            {
                maxWidth = 125;
            }
            if (maxHeight == 0)
            {
                maxHeight = 125;
            }

            int sizeWidth = 0;
            int sizeHeight = 0;
            if (context.Request.QueryString["sw"] != null)
            {
                sizeWidth = int.Parse(context.Request.QueryString["sw"]);
            }
            if (context.Request.QueryString["sh"] != null)
            {
                sizeHeight = int.Parse(context.Request.QueryString["sh"]);
            }

            Picture picture = PicContext.Current.PictureManager.GetPicture(pictureId);

            if (picture != null)
            {
                PictureCache cachedImage = PicContext.Current.PictureManager.GetPictureCache(pictureId, maxWidth, maxHeight);

                if (cachedImage == null)
                {
                    throw new Exception("Unable to locate cached image. (id " + PicContext.Current.CurrentUser.Id + ")");
                }

                if (!string.IsNullOrEmpty(context.Request.QueryString["sb"]))
                {
                    sizeWidth = cachedImage.Width.Value;
                    sizeHeight = cachedImage.Height.Value;
                }

                string strCache = PicContext.Current.Config.CacheDirectory;
                string filename = Path.Combine(strCache, cachedImage.Filename);

                context.Response.ContentType = "image/jpeg";

                if (!File.Exists(filename))
                {
                    throw new Exception("Unable to locate " + filename);
                }

                using (Bitmap img = new Bitmap(filename))
                {
                    if (sizeWidth == 0 && sizeHeight == 0)
                    {
                        using (Bitmap baseBitmap = new Bitmap(maxWidth + 2, img.Height + 2))
                        {
                            Graphics g = Graphics.FromImage(baseBitmap);

                            using (SolidBrush background = new SolidBrush(backColor))
                            {
                                g.FillRectangle(background, 0, 0, maxWidth + 2, img.Height + 2);
                            }

                            int left = 0;
                            int top = 0;

                            if (img.Height > img.Width)
                            {
                                left = (maxWidth - img.Width) / 2;
                            }
                            else
                            {
                             //   top = (maxHeight - img.Height) / 2;
                            }

                            g.DrawImage(img, left + 1, top + 1, img.Width, img.Height);

                            using (Pen pen = new Pen(Color.Silver))
                            {
                                g.DrawRectangle(pen, left, 0, img.Width + 1, img.Height + 1);
                            }

                            baseBitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                        }
                    }
                    else
                    {
                        using (Bitmap smallerBitmap = new Bitmap(img, new Size(sizeWidth, sizeHeight)))
                        {
                            smallerBitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                        }
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
