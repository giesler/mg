using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;
using CamLib;
using System.Text;

public partial class Log : System.Web.UI.Page
{
    List<Alert> alerts = null;
    bool videos = false;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        CamAlertManager mgr = new CamAlertManager();
        this.alerts = mgr.GetAlertsSinceDate(DateTime.Now.AddDays(-7));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["Login"];
        if (cookie == null || cookie.Value != "1")
        {
            Response.Redirect("Login.aspx");
        }

        this.videos = Request.QueryString["v"] == "1";
        this.showHideVideos.Text = this.videos ? "HIDE VIDEOS" : "SHOW VIDEOS";

        List<DayPictures> dayPics = new List<DayPictures>();
        dayPics.Add(new DayPictures("TODAY", DateTime.Now.Date));
        dayPics.Add(new DayPictures("YESTERDAY", DateTime.Now.Date.AddDays(-1)));
        dayPics.Add(new DayPictures(DateTime.Now.Date.AddDays(-2).ToString("dddd MMMM d"), DateTime.Now.Date.AddDays(-2)));
        dayPics.Add(new DayPictures(DateTime.Now.Date.AddDays(-3).ToString("dddd MMMM d"), DateTime.Now.Date.AddDays(-3)));
        dayPics.Add(new DayPictures(DateTime.Now.Date.AddDays(-4).ToString("dddd MMMM d"), DateTime.Now.Date.AddDays(-4)));
        dayPics.Add(new DayPictures(DateTime.Now.Date.AddDays(-5).ToString("dddd MMMM d"), DateTime.Now.Date.AddDays(-5)));
        dayPics.Add(new DayPictures(DateTime.Now.Date.AddDays(-6).ToString("dddd MMMM d"), DateTime.Now.Date.AddDays(-6)));
        dayPics.Add(new DayPictures(DateTime.Now.Date.AddDays(-7).ToString("dddd MMMM d"), DateTime.Now.Date.AddDays(-7)));
        this.data.DataSource = dayPics;
        this.data.DataBind();
    }

    protected string GetPictures(object sender)
    {
        DateTime date = (DateTime)sender;
        StringBuilder html = new StringBuilder();

        var q = this.alerts.Where(i => i.Timestamp > date && i.Timestamp < date.AddDays(1).AddSeconds(-1));
        int server = 1;
        DateTime lastAlertTime = date.Date;
        CamVideoManager mgr = new CamVideoManager();
        List<Alert> alerts = q.OrderBy(i => i.Timestamp).ToList();

        foreach (Alert alert in alerts)
        {
            // Get all videos before this alert timestamp
            if (this.videos)
            {
                AddVideos(mgr, html, lastAlertTime, alert.Timestamp);
                lastAlertTime = alert.Timestamp;
            }

            string getUrl = string.Format("http://cam{0}.msn2.net/GetLogImage.aspx", server);
            if (Request.Url.IsLoopback)
            {
                getUrl = "GetLogImage.aspx";
            }
            html.AppendFormat("<div class=\"panel\"><a href=\"AlertItem.aspx?a={0}\">", alert.Id);
            html.AppendFormat("<img height=\"48\" width=\"64\" src=\"{2}?a={0}&h=48\" title=\"{1}\" border=\"0\" class=\"thumb\" /></a></div>", alert.Id, alert.Timestamp.ToString("h:mm"), getUrl);

            if (server == 3)
            {
                server = 0;
            }

            server++;

            if (this.videos)
            {
                // Add end of day videos
                if (alert.Id == alerts[alerts.Count - 1].Id)
                {
                    AddVideos(mgr, html, alert.Timestamp, alert.Timestamp.Date.AddDays(1));
                }
            }
        }

        return html.ToString();        
    }

    private static void AddVideos(CamVideoManager mgr, StringBuilder html, DateTime startAlertTime, DateTime endAlertTime)
    {
        List<Video> videos = mgr.GetVideos(startAlertTime, endAlertTime);
        foreach (Video video in videos)
        {            
            string name = video.Filename.Substring(29);
            name = name.Substring(0, name.IndexOf(" ")).Trim();
            html.AppendFormat("<div class=\"panel\"><a href=\"getvid.aspx?v={0}\">{1}</a>", video.Id, video.Timestamp.ToString("h:mm"));
            html.AppendFormat("<br /><a href=\"getvid.aspx?v={0}\">{1}</a></div>", video.Id, name);
        }
    }

    protected void showHideVideos_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["v"] == "1")
        {
            Response.Redirect("log.aspx?v=0");
        }
        else
        {
            Response.Redirect("log.aspx?v=1");
        }
    }
}

public class DayPictures
{
    public DayPictures(string day, DateTime date)
    {
        this.Day = day.ToUpper();
        this.Date = date;
    }

    public string Day { get; set; }
    public DateTime Date { get; set; }
}