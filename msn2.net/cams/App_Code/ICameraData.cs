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
    List<LogItem> GetItems(DateTime date);

    [OperationContract]
    List<VideoItem> GetVideos(DateTime startTime, DateTime endTime);
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

