using CamLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class CameraDataService : ICameraData
{
    public List<LogItem> GetItems(DateTime date)
    {
        CamAlertManager mgr = new CamAlertManager();
        var q = mgr.GetAlertsOnDate(date);
        return ConvertAlerts(q);
    }

    private static List<LogItem> ConvertAlerts(List<Alert> q)
    {
        List<LogItem> items = new List<LogItem>();
        int server = 1;
        foreach (Alert alert in q)
        {
            string getUrl = string.Format("http://cam{0}.msn2.net/GetLogImage.aspx?a={1}", server, alert.Id);

            items.Add(new LogItem { Id = alert.Id.ToString(), Timestamp = alert.Timestamp, Url = getUrl });

            server++;
            if (server > 4)
            {
                server = 1;
            }
        }
        return items;
    }

    public List<VideoItem> GetVideos(DateTime startTime, DateTime endTime)
    {
        CamVideoManager mgr = new CamVideoManager();
        var list = mgr.GetVideos(startTime, endTime);

        List<VideoItem> items = CreateVideoItems(list);

        return items;
    }

    private static List<VideoItem> CreateVideoItems(List<Video> list)
    {
        List<VideoItem> items = new List<VideoItem>();

        foreach (var video in list)
        {
            string name = video.Filename.Substring((@"C:\LOGITECH ALERT RECORDINGS\").Length);
            name = name.Substring(0, name.IndexOf(" ")).Trim();

            VideoItem item = new VideoItem();
            item.Name = name;
            item.Timestamp = video.Timestamp;
            item.MotionPercentage = (double)video.Motion / 32375.0 * 100.0;
            item.Duration = video.Duration / 1000;
            item.Id = video.Id.ToString();
            items.Add(item);
        }
        return items;
    }

    public PreviousAndNextLogItems GetPreviousAndNextLogItems(string id)
    {
        CamAlertManager alerts = new CamAlertManager();
        int idValue = int.Parse(id);
        PreviousAndNextLogItems items = new PreviousAndNextLogItems();

        int previousId = alerts.GetPreviousAlertIdById(idValue);
        if (previousId > 0)
        {
            var previous = alerts.GetAlert(previousId);
            items.PreviousItem = new LogItem { Id = previous.Id.ToString(), Timestamp = previous.Timestamp, Url = "http://cam1.msn2.net/GetLogImage.aspx?a=" + previous.Id.ToString() };
        }

        int nextId = alerts.GetNextAlertIdById(idValue);
        if (nextId > 0)
        {
            var next = alerts.GetAlert(nextId);
            items.NextItem = new LogItem { Id = next.Id.ToString(), Timestamp = next.Timestamp, Url = "http://cam2.msn2.net/GetLogImage.aspx?a=" + next.Id.ToString() };
        }

        return items;
    }

    public void AddVideo(DateTime timestamp, string fileName, int duration, int motion, int size)
    {
        CamVideoManager videos = new CamVideoManager();
        videos.AddVideo(timestamp, fileName, duration, motion, size);
    }

    public void AddAlert(DateTime timestamp, string fileName, DateTime receiveTime)
    {
        CamAlertManager alerts = new CamAlertManager();
        alerts.AddAlert(timestamp, fileName, receiveTime);
    }

    public List<LogItem> GetAlertsBeforeDate(DateTime timestamp)
    {
        CamAlertManager alerts = new CamAlertManager();
        List<Alert> list = alerts.GetAlertsBeforeDate(timestamp);
        return ConvertAlerts(list);
    }

    public void DeleteAlert(int id)
    {
        CamAlertManager alerts = new CamAlertManager();
        alerts.DeleteAlert(id);
    }

    public void DeleteVideo(int id)
    {
        CamVideoManager videos = new CamVideoManager();
        videos.DeleteVideo(id);
    }
}
