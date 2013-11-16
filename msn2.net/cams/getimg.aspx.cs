using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Net;

public partial class getimg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "image/jpeg";

        string cam = "1";
        if (Request.QueryString["c"] != null)
        {
            cam = Request.QueryString["c"];
        }

        string address = string.Format("http://randy.sp.msn2.net:5050{0}/webcam/jpg", cam);
        bool rotate = true;

        if (cam == "1")
        {
            address = "http://cam1.sp.msn2.net/image/cc1";
        }
        else if (cam == "2")
        {
            address = "http://cam2.sp.msn2.net/image/cc2";
        }
        else if (cam == "3")
        {
            rotate = false;
            address = "http://cam3.sp.msn2.net/image/cc3";
        }
        else if (cam == "dw1")
        {
            rotate = false;
            address = "http://cam4.sp.msn2.net/image/dw1";
        }
        else if (cam.ToLower() == "front")
        {
            rotate = false;
            address = "http://cam5.sp.msn2.net/image/Front";
        }
        else if (cam.ToLower() == "side")
        {
            rotate = false;
            address = "http://cam1.sp.msn2.net/image/Side";
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
        req.Credentials = new NetworkCredential("admin", "aaaCharlie", "sp");

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