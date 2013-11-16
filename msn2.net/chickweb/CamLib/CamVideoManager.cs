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

        public void AddVideo(DateTime timestamp, string fileName, int duration, int motion, int size)
        {
            using (CamDataDataContext data = new CamDataDataContext())
            {
                Video current = data.Videos.Where(i => i.Filename == fileName).FirstOrDefault();
                if (current == null)
                {
                    lock (this.insertLock)
                    {
                        current = data.Videos.Where(i => i.Filename == fileName).FirstOrDefault();
                        if (current == null)
                        {
                            Video video = new Video
                            {
                                Timestamp = timestamp,
                                Filename = fileName,
                                Duration = duration,
                                Motion = motion,
                                Size = size
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

        public void DeleteVideo(int id)
        {
            using (CamDataDataContext data = new CamDataDataContext())
            {
                Video video = data.Videos.FirstOrDefault(v => v.Id == id);
                if (video != null)
                {
                    data.Videos.DeleteOnSubmit(video);
                    data.SubmitChanges();
                }
            }
        }
    }
}
