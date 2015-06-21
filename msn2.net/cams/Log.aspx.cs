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
        this.alerts = mgr.GetAlertsSinceDate(DateTime.Now.AddDays(-30).ToUniversalTime());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["Login"];
        if (cookie == null || cookie.Value != "1")
        {
            Response.Redirect("http://login.msn2.net/?r=http://cams.msn2.net/log.aspx");
        }

        this.videos = Request.QueryString["v"] == "1";
        this.showHideVideos.Text = this.videos ? "HIDE VIDEOS" : "SHOW VIDEOS";

        List<DayPictures> dayPics = new List<DayPictures>();
        dayPics.Add(new DayPictures("TODAY", DateTime.Now.Date));
        dayPics.Add(new DayPictures("YESTERDAY", DateTime.Now.Date.AddDays(-1)));
        for (int i = 2; i < 31; i++)
        {
            dayPics.Add(new DayPictures(DateTime.Now.Date.AddDays(-1 * i).ToString("dddd MMMM d"), DateTime.Now.Date.AddDays(-1 * i)));
        }
        
        this.data.DataSource = dayPics;
        this.data.DataBind();
    }

    protected string GetPictures(object sender)
    {
        DateTime date = ((DateTime)sender).AddHours(8);
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
            
            string getUrl = string.Format("http://cam{0}.msn2.net:8808/GetLogImage.aspx", server);
            if (Request.Url.IsLoopback)
            {
 //               getUrl = "GetLogImage.aspx";
            }
            DateTime ts = CameraDataService.ToPst(alert.Timestamp);
            html.AppendFormat("<div class=\"panel\"><a href=\"AlertItem.aspx?a={0}\">", alert.Id);
            html.AppendFormat("<img height=\"48\" width=\"64\" src=\"{2}?a={0}&h=48\" title=\"{1}\" border=\"0\" class=\"thumb\" /></a></div>", alert.Id, ts.ToString("h:mm"), getUrl);

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
            DateTime ts = CameraDataService.ToPst(video.Timestamp);
            html.AppendFormat("<div class=\"panel\"><a href=\"{0}\">{1}</a>", GetVideoUrl(video.Id), ts.ToString("h:mm"));
            html.AppendFormat("<br /><a href=\"{0}\">{1}</a></div>", GetVideoUrl(video.Id), name);
        }
    }

    public static string GetVideoUrl(int id)
    {
        int server = new Random().Next(1, 5);
        return string.Format("http://cam{0}.msn2.net:8808/getvid.aspx?v={1}", server, id);
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