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

            string url = string.Format("http://192.168.1.1:8080/cam_1.jpg?{0}", DateTime.Now);
            HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create(url);
            req.Method = WebRequestMethods.Http.Get;
            
            using (HttpWebResponse response = (HttpWebResponse) req.GetResponse())
            {
                using (Stream s = response.GetResponseStream())
                {
                    System.Drawing.Image i = System.Drawing.Image.FromStream(s);
                    s.Close();
                    i.RotateFlip(RotateFlipType.Rotate90FlipNone);

                    System.Drawing.Image thumb = new System.Drawing.Bitmap(60, 80, i.PixelFormat);
                    Graphics g = Graphics.FromImage(thumb);
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    
                    Rectangle rect = new Rectangle(0, 0, 60, 80);
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
