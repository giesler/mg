using HomeServices.CamData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeServices
{
    public partial class getvid : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Response.Clear();

            string id = Request.QueryString["v"];
            if (string.IsNullOrEmpty(id))
            {
                Response.Redirect("Log.aspx");
            }

            CameraDataClient data = new CameraDataClient();
            string name = data.GetVideoFilename(int.Parse(id));
            data.Close();

            if (name == null)
            {
                Response.Redirect("Log.aspx");
            }

            string fileName = Path.GetFileName(name);

            fileName = Path.Combine(@"\\kenny.sp.msn2.net\camarchive\videos", fileName);

            if (File.Exists(fileName))
            {
                Response.AddHeader("content-disposition", "inline; filename=" + fileName);
                Response.WriteFile(fileName);
                Response.ContentType = "video/mpeg";
            }
            else
            {
                Response.Write("Unable to read the video file " + fileName);
            }

            Response.End();
        }
    }
}