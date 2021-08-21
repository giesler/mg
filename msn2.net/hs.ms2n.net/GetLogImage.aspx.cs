using HomeServices.CamData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeServices
{
    public partial class GetLogImage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Response.Clear();
            Response.ContentType = "image/jpeg";

            int alertId = int.Parse(Request.QueryString["a"]);
            string height = Request.QueryString["h"];

            CameraDataClient data = new CameraDataClient();
            string name = data.GetItemFilename(alertId);
            data.Close();

            string fileName = Path.Combine(@"\\kenny.sp.msn2.net\camarchive\images", name);

            using (Bitmap bmp = new Bitmap(fileName))
            {
                if (string.IsNullOrEmpty(height))
                {
                    bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
                }
                else
                {
                    int h = int.Parse(height);
                    int w = (int)((double)bmp.Width / (double)bmp.Height * (double)h);

                    using (Bitmap thumb = new Bitmap(bmp, new Size(w, h)))
                    {
                        thumb.Save(Response.OutputStream, ImageFormat.Jpeg);
                    }
                }

                Response.End();
            }
        }
    }
}