using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CamLib
{
    public class CamVideoManager
    {
        object insertLock = new object();

        public void InsertVideo(string fileName)
        {
            using (CamDataDataContext data = new CamDataDataContext())
            {
                int id = (from d in data.Videos
                          where d.Filename == fileName
                          select d.Id).FirstOrDefault();

                if (id == 0)
                {
                    lock (this.insertLock)
                    {                        
                        id = (from d in data.Videos
                              where d.Filename == fileName
                              select d.Id).FirstOrDefault();

                        if (id == 0)
                        {
                            string dir = Path.GetDirectoryName(fileName);
                            string name = Path.GetFileNameWithoutExtension(fileName);

                            // Extract month/day/year
                            int yearStart = dir.IndexOf("201");
                            string year = dir.Substring(yearStart, 4);
                            string month = dir.Substring(yearStart + 5, 2);
                            string day = dir.Substring(yearStart + 8, 2);

                            // Extract time
                            int timeStart = name.IndexOf("S");
                            string time = name.Substring(timeStart + 1, 2) + ":" + name.Substring(timeStart + 3, 2) + ":" + name.Substring(timeStart + 5, 2);

                            DateTime timeStamp = DateTime.Parse(string.Format(@"{0}/{1}/{2} {3}", month, day, year, time));

                            // Extract duration
                            int durationStart = name.IndexOf("D");
                            int durationEnd = name.IndexOf(" ", durationStart);
                            int duration = int.Parse(name.Substring(durationStart + 1, durationEnd - durationStart - 1));

                            // Extract motion
                            int motionStart = name.IndexOf("A");
                            int motionEnd = name.IndexOf(" ", motionStart);
                            int motion = int.Parse(name.Substring(motionStart + 1, motionEnd - motionStart - 1));

                            FileInfo fileInfo = new FileInfo(fileName);

                            Video video = new Video
                            {
                                Filename = fileName,
                                Timestamp = timeStamp,
                                Motion = motion,
                                Duration = duration,
                                Size = (int)fileInfo.Length % 1024
                            };

                            data.Videos.InsertOnSubmit(video);
                            data.SubmitChanges();
                        }
                    }
                }
            }
        }

        public List<Video> GetVideos(DateTime startTime, DateTime endTime)
        {
            List<Video> list = null;

            using (CamDataDataContext data = new CamDataDataContext())
            {
                list = data.Videos.Where(v => v.Timestamp > startTime && v.Timestamp < endTime).OrderBy(v => v.Timestamp).ToList();
            }

            return list;
        }

        public Video GetVideo(int id)
        {
            Video video = null;

            using (CamDataDataContext data = new CamDataDataContext())
            {
                video = data.Videos.FirstOrDefault(v => v.Id == id);
            }

            return video;
        }
    }
}
