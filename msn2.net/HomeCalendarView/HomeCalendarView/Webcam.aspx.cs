using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Drawing;

namespace HomeCalendarView
{
    public partial class Webcam : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string url = "http://192.168.1.114/image.jpg?cidx=" + DateTime.Now.ToString();
            HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create(url);
            req.Method = WebRequestMethods.Http.Get;
            req.Credentials = new NetworkCredential("admin", "camping");

            using (HttpWebResponse response = (HttpWebResponse) req.GetResponse())
            {
                using (Stream s = response.GetResponseStream())
                {
                    System.Drawing.Image i = System.Drawing.Image.FromStream(s);
                    s.Close();

                    System.Drawing.Image thumb = new System.Drawing.Bitmap(80, 60, i.PixelFormat);
                    Graphics g = Graphics.FromImage(thumb);
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    Rectangle rect = new Rectangle(0, 0, 80, 60);
                    g.DrawImage(i, rect);

                    Response.ContentType = "image/jpeg";
                    thumb.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Response.End();

                    i.Dispose();
                    thumb.Dispose();
                }

                response.Close();
            }            
        }
    }
}
