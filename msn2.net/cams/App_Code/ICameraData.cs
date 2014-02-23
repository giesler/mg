using CamLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

[ServiceContract]
public interface ICameraData
{
    [OperationContract]
    string GetItemFilename(int id);

    [OperationContract]
    List<LogItem> GetItems(DateTime date);

    [OperationContract]
    List<LogItem> GetItemsUtc(DateTime dateTimeStartUtc, DateTime dateTimeEndUtc);

    [OperationContract]
    PreviousAndNextLogItems GetPreviousAndNextLogItems(string id);

    [OperationContract]
    string GetVideoFilename(int id);

    [OperationContract]
    List<VideoItem> GetVideos(DateTime startTime, DateTime endTime);

    [OperationContract]
    void AddVideo(DateTime timestamp, string fileName, int duration, int motion, int size);

    [OperationContract]
    void AddAlert(DateTime timestamp, string fileName, DateTime receiveTime);

    [OperationContract]
    List<LogItem> GetAlertsBeforeDate(DateTime timestamp);

    [OperationContract]
    void DeleteAlert(int id);

    [OperationContract]
    void DeleteVideo(int id);
}

[DataContract]
public class LogItem
{
    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public DateTime Timestamp { get; set; }

    [DataMember]
    public string Url { get; set; }
}

[DataContract]
public class VideoItem
{
    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public double MotionPercentage { get; set; }

    [DataMember]
    public DateTime Timestamp { get; set; }

    [DataMember]
    public double Duration { get; set; }
}

[DataContract]
public class PreviousAndNextLogItems
{
    [DataMember]
    public LogItem PreviousItem { get; set; }

    [DataMember]
    public LogItem NextItem { get; set; }
}
