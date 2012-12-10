using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Linq;
using msn2.net.Pictures;
using System.Data.Linq;
using System.Configuration;

namespace PictureService
{
    class PictureMonitorService
    {
        EventLog log;
        Queue<string> updateQueue = new Queue<string>();
        Queue<string> addQueue = new Queue<string>();
        FileSystemWatcher updateWatcher;
        FileSystemWatcher addWatcher;
        string addWatchPath;
        string updateWatchPath;
        DateTime lastScan = DateTime.Parse("8/22/2009 8:00 PM");
        bool shutdown = false;
        Thread updateScanThread;
        Thread newScanThread;
        Thread processThread;

        public PictureMonitorService()
        {
            this.addWatchPath = ConfigurationManager.AppSettings["addDirectory"];
            this.updateWatchPath = ConfigurationManager.AppSettings["updateDirectory"];
        }

        public void Start()
        {
            if (EventLog.SourceExists("Picture Monitor") == false)
            {
                EventLog.CreateEventSource("Picture Monitor", "Picture Monitor");
            }
            
            this.log = new EventLog("Picture Monitor");
            this.log.Source = this.log.Log;
            this.log.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 14);

            this.Log(EventLogEntryType.Information, "Starting up...");

            if (File.Exists("lastscan.txt"))
            {
                string lastScanText = File.ReadAllText("lastscan.txt");
                this.lastScan = DateTime.Parse(lastScanText);
            }

            this.addWatcher = new FileSystemWatcher(addWatchPath);
            this.addWatcher.Created += new FileSystemEventHandler(addWatcher_Created);
            this.addWatcher.IncludeSubdirectories = true;
            this.addWatcher.EnableRaisingEvents = true;

            this.updateWatcher = new FileSystemWatcher(updateWatchPath);
            this.updateWatcher.Changed += new FileSystemEventHandler(updateWatcher_Changed);
            this.updateWatcher.IncludeSubdirectories = true;
            this.updateWatcher.EnableRaisingEvents = true;

            this.updateScanThread = new Thread(new ThreadStart(this.GetUpdatedFiles));
            this.updateScanThread.Name = "Update File Scan";
            this.updateScanThread.Start();

            this.newScanThread = new Thread(new ThreadStart(this.GetNewFiles));
            this.newScanThread.Name = "New File Scan";
            this.newScanThread.Start();

            this.processThread = new Thread(new ThreadStart(this.ProcessThread));
            this.processThread.Name = "Processing Thread";
            this.processThread.Start();

            this.Log(EventLogEntryType.Information, "Starting all threads");
        }

        public void Shutdown()
        {
            this.shutdown = true;

            File.WriteAllText("lastscan.txt", DateTime.Now.ToString());

            this.Log(EventLogEntryType.Information, "Shutting join threads");

            this.newScanThread.Join();
            this.updateScanThread.Join();
            this.processThread.Join();

            this.Log(EventLogEntryType.Information, "Shutdown complete");
        }

        void updateWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.Log(EventLogEntryType.Information, "Queued file change: ", e.FullPath);
            this.updateQueue.Enqueue(e.FullPath);
        }

        void addWatcher_Created(object sender, FileSystemEventArgs e)
        {
            this.Log(EventLogEntryType.Information, "Queued file creation: ", e.FullPath);
            this.addQueue.Enqueue(e.FullPath);
        }

        void ProcessThread()
        {
            while (!this.shutdown)
            {
                if (this.addQueue.Count > 0)
                {
                    this.ProcessAdd();
                }
                else if (this.updateQueue.Count > 0)
                {
                    this.ProcessUpdate();
                }

                if (this.addQueue.Count == 0 && this.updateQueue.Count == 0)
                {
                    Thread.Sleep(5000);
                }
            }
        }

        void ProcessAdd()
        {
            string fileName = this.addQueue.Dequeue();

            this.Log(EventLogEntryType.Warning, "Add skipped: {0}", fileName);

            // Move file

            // Add to db
            
            // Update cache
        }

        void ProcessUpdate()
        {
            string fileName = this.updateQueue.Dequeue();
            bool retry = true;
            int retryCount = 20;

            this.Log(EventLogEntryType.Information, "Processing {0}", fileName);

            while (retry && retryCount > 0)
            {
                retry = false;
                try
                {
                    int index = fileName.IndexOf("pics.msn2.net");
                    string relativeFileName = fileName.Substring(index + 14);

                    PicContext context = PicContext.Current.Clone();
                    Picture p = context.PictureManager.GetPictures().FirstOrDefault(i => i.Filename == relativeFileName);
                    if (p != null)
                    {
                        string[] tags = ImageUtilities.GetTags(fileName);
                        if (tags != null && tags.Length > 0)
                        {
                            this.MergeTags(context, p, fileName, tags);
                        }
                        else
                        {
                            this.Log(EventLogEntryType.Information, "No tags: " + fileName);
                        }
                    }
                    else
                    {
                        this.Log(EventLogEntryType.Error, "Unknown file: " + relativeFileName);
                    }
                }
                catch (IOException)
                {
                    retry = true;
                    retryCount--;
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    this.Log(EventLogEntryType.Error, "Error processing {0}: {1}", fileName, ex);
                }
            }

            this.Log(EventLogEntryType.Information, "Processed {0}", fileName);
        }

        void MergeTags(PicContext context, Picture p, string fileName, string[] tags)
        {
            List<PictureCategory> categories = p.PictureCategories.ToList();

            foreach (string tag in tags)
            {
                string tagPath = @"\" + tag.Replace("/", @"\");
                PictureCategory match = categories.FirstOrDefault(pc => pc.Category.Path.ToLower() == tagPath.ToLower());
                if (match == null)
                {
                    PicContext clone = context.Clone();
                    int categoryId = clone.CategoryManager.AddCategoryPath(tagPath);
                    this.Log(EventLogEntryType.Information, "Adding tag {0} to picture {1}", tagPath, p.Id);
                    clone.PictureManager.AddToCategory(p.Id, categoryId);
                }
            }

            foreach (PictureCategory category in categories)
            {
                string tagPath = category.Category.Path.Substring(1).Replace(@"\", "/");
                bool found = false;

                foreach (string tag in tags)
                {
                    if (tag.ToLower() == tagPath.ToLower())
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    this.Log(EventLogEntryType.Information, "Category {0} not found on picture - removing from db", category.Category.Path);
                    context.PictureManager.RemoveFromCategory(p.Id, category.CategoryID);

                    if (context.PictureManager.GetPicturesByCategory(category.CategoryID).Count == 0)
                    {
                        this.Log(EventLogEntryType.Information, "Removing category with no pictures: {0}", category.Category.Path);
                        context.CategoryManager.DeleteCategory(category.CategoryID);
                    }
                }
            }
        }

        void GetNewFiles()
        {
            this.GetNewFiles(this.addWatchPath);
        }
        
        void GetNewFiles(string path)
        {
            foreach (string fileName in Directory.GetFiles(path, "*.jpg"))
            {
                this.addQueue.Enqueue(fileName);
            }

            if (!this.shutdown)
            {
                foreach (string directory in Directory.GetDirectories(path))
                {
                    this.GetNewFiles(directory);
                }
            }
        }

        void GetUpdatedFiles()
        {
            this.GetUpdatedFiles(this.updateWatchPath);
        }

        void GetUpdatedFiles(string path)
        {
            foreach (string fileName in Directory.GetFiles(path, "*.jpg"))
            {
                FileInfo info = new FileInfo(fileName);
                if (info.LastWriteTime > this.lastScan)
                {
                    this.updateQueue.Enqueue(fileName);
                }
            }

            if (!this.shutdown)
            {
                foreach (string directory in Directory.GetDirectories(path))
                {
                    this.GetUpdatedFiles(directory);
                }
            }
        }

        void Log(EventLogEntryType type, string message, params object[] args)
        {
            this.log.WriteEntry(string.Format(message, args), type);
            Console.WriteLine(type.ToString() + ": " + string.Format(message, args));
        }
    }
}
