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

namespace CamAlertService
{
    class CamAlertMonitor
    {
        FileSystemWatcher watcher = new FileSystemWatcher();
        bool exit = false;
        string basePath;
        string serverPath;
        object insertLock = new object();

        public void Start()
        {
            this.basePath = Settings.Default.LocalMonitorPath;
            this.serverPath = Settings.Default.ServerPath;

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
            
            this.watcher = new FileSystemWatcher(this.basePath, "*.jpg");
            this.watcher.IncludeSubdirectories = false;
            this.watcher.Created += new FileSystemEventHandler(OnFileCreated);
            this.watcher.Renamed += new RenamedEventHandler(OnFileRenamed);            
            this.watcher.EnableRaisingEvents = true;

            this.ScanForFiles();

            DateTime lastScan = DateTime.Now;

            while (!this.exit)
            {
                Thread.Sleep(1000);

                if (DateTime.Now.AddMinutes(5) < DateTime.Now)
                {
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

        void ScanForFiles()
        {
            foreach (string fileName in Directory.GetFiles(this.basePath, "*.jpg"))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessFile), fileName);
            }
        }

        void ProcessFile(object sender)
        {
            string fileName = sender.ToString();

            try
            {
                this.Log("Processing {0}", fileName);

                int retryCount = 0;
                bool logged = false;

                lock (this.insertLock)
                {
                    while (retryCount < 50)
                    {
                        try
                        {
                            if (File.Exists(fileName))
                            {
                                CamAlertManager alerts = new CamAlertManager();

                                alerts.InsertAlert(fileName);
                                logged = true;
                                this.Log("{0} Alert inserted in db", fileName);
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.SendError(fileName, ex);
                            this.Log("{0} InsertAlert: {1}", fileName, ex);
                        }

                        Thread.Sleep(2000);
                        retryCount++;
                    }

                    if (logged)
                    {
                        retryCount = 0;
                        Exception lastDeleteException = null;
                        string destFileName = string.Format(@"ftp://192.168.1.25/{0}", Path.GetFileName(fileName));

                        while (retryCount < 50)
                        {
                            try
                            {
                                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(destFileName);
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
                                this.Log("{0} FTP complete: {1} - {2}", fileName, response.StatusCode, response.StatusDescription);

                                File.Delete(fileName);

                                lastDeleteException = null;
                                break;
                            }
                            catch (Exception ex)
                            {
                                lastDeleteException = ex;

                                this.Log("{0} Upload&delete: {1}", fileName, ex);
                            }

                            Thread.Sleep(2000);
                            retryCount++;
                        }

                        if (lastDeleteException != null)
                        {
                            SendError(fileName, lastDeleteException);
                        }

                        this.Log("Done with {0}", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                SendError(fileName, ex);
                this.Log("{0} - Exception - {1}", fileName, ex);
            }

            Trace.Flush();
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
