using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class getimg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Clear();
        Response.ContentType = "image/jpeg";

        string cam = "gdw";
        if (Request.QueryString["c"] != null)
        {
            cam = Request.QueryString["c"];
        }

        bool rotate = false;
        if (Request.QueryString["r"] == "1")
        {
            rotate = true;
        }

        int random = new Random().Next(1, 5);
        string address = string.Format("https://cam{0}.ms2n.net:8443/getimg.aspx?c={1}&r={2}&ts={3}", random, cam, rotate ? "1" : "0", DateTime.Now.ToString("yymmddhhmmsstt"));
        
        int maxHeight = 0;
        if (Request.QueryString["h"] != null)
        {
            maxHeight = int.Parse(Request.QueryString["h"]);
        }

        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(address);
        req.Method = WebRequestMethods.Http.Get;
//        req.Credentials = new NetworkCredential("home", "4362");

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
                            img.Save(Response.OutputStream, ImageFormat.Jpeg);
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