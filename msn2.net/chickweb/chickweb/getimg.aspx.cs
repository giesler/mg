using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Drawing;

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

            string address = "192.168.1.25:8888";
            bool rotate = true;

            if (cam == "3")
            {
                cam = "1";
                address = "192.168.1.1:8080";
                rotate = false;
            }

            if (Request.QueryString["r"] == "1")
            {
                rotate = true;
            }

            string url = string.Format("http://{0}/cam_{1}.jpg?{2}",
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

                            img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                            Response.End();
                        }
                    }
                }
            }
        }
    }
}