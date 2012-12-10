using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace chickweb
{
    public partial class getimg : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Response.Clear();
            Response.ContentType = "image/jpeg";

            string cam = "1";
            if (Request.QueryString["c"] != null)
            {
                cam = Request.QueryString["c"];
            }

            string address = string.Format("http://randy:5050{0}/webcam/jpg", cam);
            bool rotate = true;

            if (cam == "3")
            {
                rotate = false;
                address = string.Format("http://ned:50503/webcam/jpg", cam);
            }
            else if (cam == "dw1")
            {
                rotate = false;
                address = "http://cams/image/dw1";
            }
            else if (cam.ToLower() == "front")
            {
                rotate = false;
                address = "http://cams/image/Front";
            }

            if (Request.QueryString["r"] == "1")
            {
                rotate = true;
            }

            int maxHeight = 0;
            if (Request.QueryString["h"] != null)
            {
                maxHeight = int.Parse(Request.QueryString["h"]);
            }

            string url = string.Format("{0}?{1}",
                address, cam, DateTime.Now.ToString("yymmddhhmmsstt"));
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = WebRequestMethods.Http.Get;

            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                if (response.ContentLength > 0)
                {
                    using (Stream s = response.GetResponseStream())
                    {
                        using (System.Drawing.Image img = System.Drawing.Image.FromStream(s))
                        {
                            if (rotate)
                            {
                                img.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                            }

                            if (maxHeight == 0 || maxHeight > img.Height)
                            {
                                img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                            else
                            {
                                int w = (int)((double)img.Width / (double)img.Height * (double)maxHeight);

                                using (Bitmap thumb = new Bitmap(img, new Size(w, maxHeight)))
                                {
                                    thumb.Save(Response.OutputStream, ImageFormat.Jpeg);
                                }

                            }

                            Response.End();
                        }
                    }
                }
            }
        }
    }
}