using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net.Mail;
using CamAlertService.Properties;
using System.Net;
using System.Diagnostics;
using System.ServiceModel;
using CamDataService;

namespace CamAlertService
{
    class CamAlertMonitor
    {
        FileSystemWatcher imageWatcher = new FileSystemWatcher();
        FileSystemWatcher videoWatcher = new FileSystemWatcher();
        bool exit = false;
        string basePath;
        string serverPath;
        string videoPath;
        Dictionary<string, int> alertTracking = new Dictionary<string, int>();
        object alertTrackingLock = new object();

        public void Start()
        {
            this.basePath = Settings.Default.LocalMonitorPath;
            this.serverPath = Settings.Default.ServerPath;
            this.videoPath = Settings.Default.VideoPath;

            Thread t = new Thread(new ThreadStart(this.Main));
            t.Name = "Main Processing Thread";
            t.Start();
        }

        public void Stop()
        {
            this.exit = true;

            while (!this.exit)
            {
                Thread.Sleep(100);
            }
        }

        void Main()
        {
            Thread.Sleep(1000 * 15);
            
            this.imageWatcher = new FileSystemWatcher(this.basePath, "*.jpg");
            this.imageWatcher.IncludeSubdirectories = false;
            this.imageWatcher.Created += new FileSystemEventHandler(OnFileCreated);
            this.imageWatcher.Renamed += new RenamedEventHandler(OnFileRenamed);
            this.imageWatcher.EnableRaisingEvents = true;

            this.videoWatcher = new FileSystemWatcher(this.videoPath, "*.mp4");
            this.videoWatcher.IncludeSubdirectories = true;
            this.videoWatcher.Created += new FileSystemEventHandler(OnVideoCreated);
            this.videoWatcher.EnableRaisingEvents = true;
            /*
            this.ScanForFiles();
            this.ScanForVideos(this.videoPath);
            */
            DateTime lastScan = DateTime.MinValue;
            DateTime lastPurge = DateTime.MinValue;

            while (!this.exit)
            {
                Thread.Sleep(1000);

                if (lastPurge.AddHours(1) < DateTime.Now)
                {
                    lastPurge = DateTime.Now;
                    this.PurgeFiles();
                }

                if (lastScan.AddMinutes(5) < DateTime.Now)
                {
                    lastScan = DateTime.Now;
                    this.ScanForFiles();
                }
            }
        }

        void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            this.Log("OnFileCreated: {0}", e.FullPath);
            Thread.Sleep(2000);
            this.ProcessFile(e.FullPath);
        }

        void OnFileRenamed(object sender, FileSystemEventArgs e)
        {
            this.Log("OnFileRenamed: {0}", e.FullPath);
            Thread.Sleep(2000);
            this.ProcessFile(e.FullPath);
        }

        void OnVideoCreated(object sender, FileSystemEventArgs e)
        {
            this.Log("OnVideoCreated: {0}", e.FullPath);
            Thread.Sleep(2000);
            this.ProcessFile(e.FullPath);
        }


        void ScanForFiles()
        {
            foreach (string fileName in Directory.GetFiles(this.basePath, "*.jpg"))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessFile), fileName);
            }
        }

        void ScanForVideos(string directory)
        {
            foreach (string fileName in Directory.GetFiles(directory, "*.mp4"))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessFile), fileName);
            }

            foreach (string subDir in Directory.GetDirectories(directory))
            {
                this.ScanForVideos(subDir);
            }
        }

        void ProcessFile(object sender)
        {
            string fileName = sender.ToString();

            try
            {
                this.Log("Processing {0}", fileName);

                int retryCount = 0;
                string subDirectory = "Images";

                while (retryCount < 50)
                {
                    try
                    {
                        if (File.Exists(fileName))
                        {
                            string extension = Path.GetExtension(fileName).ToLower();

                            if (extension == ".jpg")
                            {
                                AddAlert(fileName);
                                this.Log("{0} Alert inserted in db", fileName);
                            }
                            else if (extension == ".mp4")
                            {
                                AddVideo(fileName);
                                this.Log("{0} video inserted in db", fileName);
                                subDirectory = "Videos";
                            }
                            else
                            {
                                this.Log("Unknown file type: {0}", extension);
                            }
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.SendError(fileName, ex);
                        this.Log("{0} Insert: {1}", fileName, ex);
                    }

                    Thread.Sleep(2000);
                    retryCount++;
                }

                retryCount = 0;
                Exception lastDeleteException = null;
                string destFileName = string.Format(@"ftp://192.168.1.25/{0}/{1}", subDirectory, Path.GetFileName(fileName));

                while (retryCount < 50)
                {
                    try
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(destFileName.Replace(@"\", "/"));
                        request.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
                        request.Credentials = new NetworkCredential("home", "4362");
                        request.UseBinary = true;
                        request.Proxy = null;

                        byte[] contents = File.ReadAllBytes(fileName);

                        using (Stream requestStream = request.GetRequestStream())
                        {
                            requestStream.Write(contents, 0, contents.Length);
                            requestStream.Close();
                        }

                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        response.Close();
                        this.Log("{0} FTP complete: {1} - {2}", fileName, response.StatusCode, response.StatusDescription.Trim());

                        File.Delete(fileName);

                        lastDeleteException = null;
                        break;
                    }
                    catch (FileNotFoundException ex)
                    {
                        this.Log("{0} Upload&delete: {1}", fileName, ex);
                        break;
                    }
                    catch (Exception ex)
                    {
                        lastDeleteException = ex;

                        this.Log("{0} Upload&delete: {1}", fileName, ex);
                    }

                    Thread.Sleep(2000);
                    retryCount++;

                    if (lastDeleteException != null)
                    {
                        SendError(fileName, lastDeleteException);
                    }

                    this.Log("Done with {0}", fileName);
                }
            }
            catch (Exception ex)
            {
                SendError(fileName, ex);
                this.Log("{0} - Exception - {1}", fileName, ex);
            }

            Trace.Flush();
        }

        void AddVideo(string fileName)
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

            DateTime timestamp = DateTime.Parse(string.Format(@"{0}/{1}/{2} {3}", month, day, year, time));

            // Extract duration
            int durationStart = name.IndexOf("D");
            int durationEnd = name.IndexOf(" ", durationStart);
            int duration = int.Parse(name.Substring(durationStart + 1, durationEnd - durationStart - 1));

            // Extract motion
            int motionStart = name.IndexOf("A");
            int motionEnd = name.IndexOf(" ", motionStart);
            int motion = int.Parse(name.Substring(motionStart + 1, motionEnd - motionStart - 1));

            FileInfo fileInfo = new FileInfo(fileName);

            CamDataService.CameraDataService data = new CameraDataService();
            data.AddVideo(timestamp.ToUniversalTime(), true, fileName, duration, true, motion, true, (int)fileInfo.Length % 1024, true);
        }

        void AddAlert(string fileName)
        {
            string name = Path.GetFileName(fileName);

            // format: do-not-reply@logitech.com Driveway - Home - 2012-06-18 12.34 pm.jpg
            string dateStamp = name.Substring(name.IndexOf("201")).Trim().Replace(".jpg", "").Replace(".", ":");

            int indexAm = dateStamp.ToLower().IndexOf("am");
            int indexPm = dateStamp.ToLower().IndexOf("pm");

            if (indexAm + 2 != dateStamp.Length || indexPm + 2 != dateStamp.Length)
            {
                if (indexAm > 0)
                {
                    dateStamp = dateStamp.Substring(0, indexAm + 2);
                }
                else if (indexPm > 0)
                {
                    dateStamp = dateStamp.Substring(0, indexPm + 2);
                }
            }

            using (CameraDataService client = new  CameraDataService())
            {
                client.AddAlert(DateTime.Parse(dateStamp).ToUniversalTime(), true, Path.GetFileName(fileName), DateTime.UtcNow, true);
            }
        }

        void PurgeFiles()
        {
            CameraDataService data = new CameraDataService();
            LogItem[] alerts = data.GetAlertsBeforeDate(DateTime.UtcNow.AddDays(-60), true);
            foreach (LogItem alert in alerts)
            {
                string fileName = Path.GetFileName(data.GetItemFilename(int.Parse(alert.Id), true));
                try
                {
                    try
                    {
                        DeleteFile("Images/" + fileName);
                    }
                    catch (WebException ex)
                    {
                        this.Log("Error deleting {0}: {1}", fileName, ex.Message);
                    }
                    data.DeleteAlert(int.Parse(alert.Id), true);
                }
                catch (Exception ex)
                {
                    this.SendError(fileName, ex);
                }
            }

            var videos = data.GetVideos(DateTime.Now.AddYears(-10), true, DateTime.Now.AddDays(-20), true);
            foreach (VideoItem video in videos.ToList<VideoItem>().OrderBy(v => v.Id))
            {
                string fileName = Path.GetFileName(video.Name);
                try
                {
                    try
                    {
                        DeleteFile("Videos/" + fileName);
                    }
                    catch (WebException ex)
                    {
                        this.Log("Error deleting {0}: {1}", fileName, ex.Message);
                    }

                    if (File.Exists(video.Name))
                    {
                        File.Delete(video.Name);
                    }

                    data.DeleteVideo(int.Parse(video.Id), true);
                }
                catch (Exception ex)
                {
                    this.SendError(fileName, ex);
                }
            }
        }

        void DeleteFile(string fileName)
        {
            string url = string.Format("ftp://192.168.1.25/{0}", fileName);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = System.Net.WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential("home", "4362");
            request.UseBinary = true;
            request.Proxy = null;

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    this.Log("{0} FTP delete complete: {1} - {2}", fileName, response.StatusCode, response.StatusDescription.Trim());
                    response.Close();
                }
            }
            catch (WebException ex)
            {
                if (ex.Message.IndexOf("File unavilable") < 0 && ex.Message.IndexOf("file not found") < 0)
                {
                    throw;
                }
            }
        }

        void Log(string message, params object[] args)
        {
            Trace.WriteLine(string.Format("{0} {1}: {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), string.Format(message, args)));
        }

        void SendError(string fileName, Exception ex)
        {
            bool send = true;
            lock (this.alertTrackingLock)
            {
                if (this.alertTracking.ContainsKey(fileName))
                {
                    send = false;
                    this.alertTracking[fileName]++;
                }
                else
                {
                    this.alertTracking.Add(fileName, 1);
                }
            }

            if (send)
            {
                string messageText = string.Format("{0}{1}{2}", fileName, Environment.NewLine, ex);
                SmtpClient smtp = new SmtpClient(Settings.Default.SmtpServer);
                smtp.Send(Settings.Default.ErrorMailFrom, Settings.Default.ErrorMailTo, "InsertAlert error: " + ex.Message, messageText);
            }
            else
            {
                this.Log("Skipping send for {0} - {1} errors: {2}", fileName, this.alertTracking[fileName], ex.Message);
            }
        }
    }
}
