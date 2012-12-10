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

            string address = "192.168.1.1:8080";
            if (cam == "2")
            {
                address = "192.168.1.5:8080";
            }
            
            string url = string.Format("http://{0}/cam_1.jpg?{1}",
                address, DateTime.Now.ToString("yymmddhhmmsstt"));
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
                            img.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                            img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                            Response.End();
                        }
                    }
                }
            }
        }
    }
}