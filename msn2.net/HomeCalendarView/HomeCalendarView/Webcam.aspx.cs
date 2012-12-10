using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;

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

            using (HttpWebResponse response = (HttpWebResponse) req.GetResponse())
            {
                using (Stream s = response.GetResponseStream())
                {
                    System.Drawing.Image i = System.Drawing.Image.FromStream(s);
                    s.Close();

                    Response.ContentType = "image/jpeg";
                    i.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Response.End();
                }

                response.Close();
            }            
        }
    }
}
