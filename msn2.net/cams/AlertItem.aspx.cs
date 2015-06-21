﻿using System;
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
            Response.Redirect("http://login.msn2.net/?r=http://cams.msn2.net/log.aspx");
        }
        if (Request.QueryString["a"] == null)
        {
            Response.Redirect("Log.aspx");
        }

        int alertId = int.Parse(Request.QueryString["a"]);
        string height = Request.QueryString["h"];

        CamAlertManager mgr = new CamAlertManager();
        Alert alert = mgr.GetAlert(alertId);

        DateTime ts = CameraDataService.ToPst(alert.Timestamp);
        this.name.Text = ts.ToString("dddd MMMM d h:mm tt").ToUpper();
        if (Request.UserAgent.ToLower().Contains("mobile"))
        {
            this.name.Text = ts.ToString("ddd MMM d h:mm tt").ToUpper();
        }
        if (ts.Date == DateTime.Today.Date)
        {
            this.name.Text = "TODAY " + ts.ToString("h:mm tt");
        }
        else if (ts.Date.AddDays(1) == DateTime.Today)
        {
            this.name.Text = "YESTERDAY " + ts.ToString("h:mm tt");
        }

        int server = new Random().Next(1, 5);
        this.img.ImageUrl = string.Format("http://cam{0}.msn2.net:8808/GetLogImage.aspx?a={1}", server, alert.Id);
        
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
        List<Video> videos = mgr2.GetVideos(alert.Timestamp.ToLocalTime().AddSeconds(-30), alert.Timestamp.ToLocalTime().AddMinutes(5));

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
            ts = CameraDataService.ToPst(video.Timestamp);
            ListItem item = new ListItem
            {
                Text = string.Format("{0} ({1} {2}s) {3:0}%", ts.ToString("h:mm:ss"), name, video.Duration / 1000, motionPercent),
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
            output = string.Format("http://cam{0}.msn2.net:8808/getvid.aspx?v={1}", server, req);
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
            string url = string.Format("http://cam{0}.msn2.net:8808/getvid.aspx?v={1}", server, req);
            output = "window.open('" + url + "');";
        }

        return output;
    }
}