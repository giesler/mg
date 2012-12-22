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
        foreach (Alert alert in q.OrderBy(i => i.Timestamp))
        {
            bool skip = false;
            if (alert.Timestamp < DateTime.Parse("6/30/12") && (alert.Timestamp.Hour > 22 || alert.Timestamp.Hour < 5))
            {
                skip = true;
            }

            if (!skip)
            {
                string getUrl = string.Format("http://cam{0}.msn2.net/GetLogImage.aspx", server);
                html.AppendFormat("<a href=\"AlertItem.aspx?a={0}\">", alert.Id);
                html.AppendFormat("<img height=\"48\" width=\"64\" src=\"{2}?a={0}&h=48\" title=\"{1}\" border=\"0\" class=\"thumb\" /></a>", alert.Id, alert.Timestamp.ToString("h:mm"), getUrl);

                if (server == 3)
                {
                    server = 0;
                }

                server++;
            }
        }

        return html.ToString();        
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