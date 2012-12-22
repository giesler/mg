using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CamLib;

public partial class GetLogImage : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        Response.Clear();
        Response.ContentType = "image/jpeg";

        int alertId = int.Parse(Request.QueryString["a"]);
        string height = Request.QueryString["h"];

        CamAlertManager mgr = new CamAlertManager();
        Alert alert = mgr.GetAlert(alertId);

        string fileName = Path.Combine(@"\\kenny.sp.msn2.net\camarchive\images", alert.Filename);

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