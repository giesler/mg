using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CamLib;
using System.IO;

public partial class AlertItem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["Login"];
        if (cookie == null || cookie.Value != "1")
        {
            Response.Redirect("Login.aspx");
        }
        if (Request.QueryString["a"] == null)
        {
            Response.Redirect("Log.aspx");
        }

        int alertId = int.Parse(Request.QueryString["a"]);
        string height = Request.QueryString["h"];

        CamAlertManager mgr = new CamAlertManager();
        Alert alert = mgr.GetAlert(alertId);

        this.name.Text = alert.Timestamp.ToString("dddd MMMM d h:mm tt").ToUpper();
        if (Request.UserAgent.ToLower().Contains("mobile"))
        {
            this.name.Text = alert.Timestamp.ToString("ddd MMM d h:mm tt").ToUpper();
        }
        if (alert.Timestamp.Date == DateTime.Today.Date)
        {
            this.name.Text = "TODAY " + alert.Timestamp.ToString("h:mm tt");
        }
        else if (alert.Timestamp.Date.AddDays(1) == DateTime.Today)
        {
            this.name.Text = "YESTERDAY " + alert.Timestamp.ToString("h:mm tt");
        }

        this.img.ImageUrl = "GetLogImage.aspx?a=" + alert.Id.ToString();

        int nextId = mgr.GetNextAlertId(alert);
        if (nextId > 0)
        {
            this.nextLink.NavigateUrl = "AlertItem.aspx?a=" + nextId.ToString();
        }
        else
        {
            this.nextLink.Enabled = false;
            this.nextLink.CssClass = "disabledLink";
        }

        int previousId = mgr.GetPreviousAlertId(alert);
        if (previousId > 0)
        {
            this.previousLink.NavigateUrl = "AlertItem.aspx?a=" + previousId.ToString();
        }
        else
        {
            this.previousLink.Enabled = false;
            this.previousLink.CssClass = "disabledLink";
        }

        CamVideoManager mgr2 = new CamVideoManager();
        List<Video> videos = mgr2.GetVideos(alert.Timestamp.AddSeconds(-30), alert.Timestamp.AddMinutes(5));

        this.videos.Items.Add(string.Format("- {0} video{1} -", videos.Count, videos.Count == 1 ? "" : "s"));
        this.videos.SelectedIndex = 0;

        foreach (Video video in videos)
        {
            string name = video.Filename.Substring((@"C:\LOGITECH ALERT RECORDINGS\").Length);
            name = name.Substring(0, name.IndexOf(" ")).Trim();
            if (Request.UserAgent.ToLower().IndexOf("mobile") > 0)
            {
                name = name.Substring(0, 1);
            }
            double motionPercent = (double)video.Motion / 32375.0 * 100.0;
            ListItem item = new ListItem
            {
                Text = string.Format("{0} ({1} {2}s) {3:0}%", video.Timestamp.ToString("h:mm:ss"), name, video.Duration / 1000, motionPercent),
                Value = video.Id.ToString()
            };
            this.videos.Items.Add(item);
        }

        if (this.videos.Items.Count == 1)
        {
            this.videos.Visible = false;
        }

        
    }

    protected string GetVideoUrl()
    {
        string output = string.Empty;

        string req = Request.Form["videos"];
        if (!string.IsNullOrEmpty(req))
        {
            int server = new Random().Next(1, 5);
            output = string.Format("http://cam{0}.msn2.net/getvid.aspx?v={1}", server, req);
            img.Visible = false;
        }
        else
        {
            img.Visible = true;
        }

        return output;
    }

    protected string GetScript()
    {
        string output = string.Empty;

        string req = Request.Form["videos"];
        if (!string.IsNullOrEmpty(req))
        {
            int server = new Random().Next(1, 5);
            string url = string.Format("http://cam{0}.msn2.net/getvid.aspx?v={1}", server, req);
            output = "window.open('" + url + "');";
        }

        return output;
    }
}