using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net.Mail;
using CamLib;
using CamAlertService.Properties;
using System.Net;
using System.Diagnostics;
using System.ServiceModel;

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
                                CamAlertManager alerts = new CamAlertManager();
                                alerts.InsertAlert(fileName);
                                this.Log("{0} Alert inserted in db", fileName);
                            }
                            else if (extension == ".mp4")
                            {
                                CamVideoManager video = new CamVideoManager();
                                video.InsertVideo(fileName);
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

        void PurgeFiles()
        {
            CamAlertManager alertManager = new CamAlertManager();
            List<Alert> alerts = alertManager.GetAlertsBeforeDate(DateTime.Now.AddDays(-60));
            foreach (Alert alert in alerts)
            {
                string fileName = Path.GetFileName(alert.Filename);
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
                    alertManager.DeleteAlert(alert.Id);
                }
                catch (Exception ex)
                {
                    this.SendError(fileName, ex);
                }
            }

            CamVideoManager videoManager = new CamVideoManager();
            List<Video> videos = videoManager.GetVideos(DateTime.Now.AddYears(-10), DateTime.Now.AddDays(-20));
            foreach (Video video in videos.OrderBy(v => v.Id))
            {
                string fileName = Path.GetFileName(video.Filename);
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

                    if (File.Exists(video.Filename))
                    {
                        File.Delete(video.Filename);
                    }

                    videoManager.DeleteVideo(video.Id);
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

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                this.Log("{0} FTP delete complete: {1} - {2}", fileName, response.StatusCode, response.StatusDescription.Trim());
                response.Close();
            }
        }

        void Log(string message, params object[] args)
        {
            Trace.WriteLine(string.Format("{0} {1}: {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), string.Format(message, args)));
        }

        void SendError(string fileName, Exception ex)
        {
            string messageText = string.Format("{0}{1}{2}", fileName, Environment.NewLine, ex);
            SmtpClient smtp = new SmtpClient(Settings.Default.SmtpServer);
            smtp.Send(Settings.Default.ErrorMailFrom, Settings.Default.ErrorMailTo, "InsertAlert error: " + ex.Message, messageText);
        }
    }
}
