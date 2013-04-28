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
}
