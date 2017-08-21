using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace msn2.net.Pictures
{
	public class PictureWatcher
	{
		private FileSystemWatcher fileSystemWatcher;
		private string directoryName;
		private object lockObject = new object();
		private Queue<string> fileQueue = new Queue<string>();
		private bool exiting = false;
		private Dictionary<string, DateTime> lastFileAccessTimes = new Dictionary<string,DateTime>();
				
		public PictureWatcher(
			string directoryName,
			string filter)
		{
			this.directoryName = directoryName;

			//
			// Create the file watcher
			//

			this.fileSystemWatcher = new FileSystemWatcher(
				directoryName,
				filter);
			this.fileSystemWatcher.Created
				+= new FileSystemEventHandler(this.OnFileCreated);
			this.fileSystemWatcher.EnableRaisingEvents = true;

			//
			// Start file processing thread
			//

			ThreadPool.QueueUserWorkItem(
				new WaitCallback(ProcessFileThread));

		}

		void OnFileCreated(object sender, FileSystemEventArgs e)
		{
			lock (this.lockObject)
			{
				lastFileAccessTimes.Add(e.FullPath, DateTime.Now);
				fileQueue.Enqueue(e.FullPath);
				Trace.WriteLine("PictureWatcher.QueueFile: " + e.FullPath);
			}
		}

		void ProcessFileThread(object notUsed)
		{
			while (!exiting)
			{
				//
				// Dequeue at most one item per second
				//

				lock (this.lockObject)
				{
					if (this.fileQueue.Count > 0)
					{
						string fileName = this.fileQueue.Peek();

						//
						// Top item in queue will have oldest time - make sure it is 10 seconds old
						//

						DateTime lastCheckTime = this.lastFileAccessTimes[fileName];
						if (lastCheckTime.AddSeconds(10) < DateTime.Now)
						{
							fileName = this.fileQueue.Dequeue();
							ThreadPool.QueueUserWorkItem(
								new WaitCallback(
									this.ProcessNewFileThread),
									fileName);
						}
					}
				}


				Thread.Sleep(1000);
			}
		}

		void ProcessNewFileThread(object fileNameObject)
		{
			string fileName = fileNameObject.ToString();

			// Try to get exclusive access
			bool opened = false;
			FileStream fileStream = null;
			try
			{
				fileStream = File.OpenWrite(fileName);
				opened = true;
			}
			catch (IOException)
			{
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
			
			// Import file if we got access, otherwise requeue

			if (opened)
			{
				this.ImportFile(fileName);

				lock (this.lockObject)
				{
					this.lastFileAccessTimes.Remove(fileName);
				}
			}
			else
			{
				lock (this.lockObject)
				{
					lastFileAccessTimes[fileName] = DateTime.Now;
					this.fileQueue.Enqueue(fileName);
				}
			}
		
		}

		void ImportFile(string fileName)
		{
			Trace.WriteLine("PictureWatcher.ImportFile: " + fileName);
		}		
	}
}
