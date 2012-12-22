using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CamLib;
using System.IO;

public partial class getvid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "video/mpeg";
        
        string id = Request.QueryString["v"];
        if (string.IsNullOrEmpty(id))
        {
            Response.Redirect("Log.aspx");
        }

        CamVideoManager mgr = new CamVideoManager();
        Video video = mgr.GetVideo(int.Parse(id));
        if (video == null)
        {
            Response.Redirect("Log.aspx");
        }

        Response.AddHeader("content-disposition", "inline; filename=" + Path.GetFileNameWithoutExtension(video.Filename));

        CamVideoService.CamVideoStorageClient storage = new CamVideoService.CamVideoStorageClient();
        byte[] file = storage.GetFile(video.Filename);

        Response.BinaryWrite(file);
        Response.End();
    }
}