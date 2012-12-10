using System;
using QuartzTypeLib;
using System.Timers;
using msn2.net.QueuePlayer.Shared;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace msn2.net.QueuePlayer
{
	/// <summary>
	/// Summary description for DXPlayer.
	/// </summary>
	public class DirectXPlayer: IServerPlayer, IDisposable
	{
		private FilgraphManagerClass filegraphManager;
		private System.Timers.Timer timer;
		private bool initialized = false;
		private MediaSourceDictionary mediaCache = new MediaSourceDictionary();
		private Queue preloadQueue = new Queue();
		private Thread preloadThread;

		// defaults
		private int	   mediaVolume  = 0;
		private int    mediaBalance = 0;
		private double mediaRate	= 1;

		public DirectXPlayer()
		{
			timer = new System.Timers.Timer(1000);
			timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);

			preloadThread = new Thread(new ThreadStart(PreloadMediaThread));
			preloadThread.Start();
		}

		#region Disposal

		private bool disposed = false;

		// Implement Idisposable.
		// Do not make this method virtual.
		// A derived class should not be able to override this method.
		public void Dispose()
		{
			Dispose(true);
			// Take yourself off of the Finalization queue 
			// to prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		// Dispose(bool disposing) executes in two distinct scenarios.
		// If disposing equals true, the method has been called directly
		// or indirectly by a user's code. Managed and unmanaged resources
		// can be disposed.
		// If disposing equals false, the method has been called by the 
		// runtime from inside the finalizer and you should not reference 
		// other objects. Only unmanaged resources can be disposed.
		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if(!this.disposed)
			{
				// If disposing equals true, dispose all managed 
				// and unmanaged resources.
				if(disposing)
				{
					// Dispose managed resources.
				}
				// Release unmanaged resources. If disposing is false, 
				// only the following code is executed.
				if (filegraphManager != null)
				{
					System.Runtime.InteropServices.Marshal.ReleaseComObject(filegraphManager);
				}
			}
			disposed = true;         
		}

		#endregion

		# region Control operations

		public override event Server_PlayingEventHandler PlayingEvent;

		public override void Play() 
		{
			if (disposed || !initialized)
				return;

			try
			{
				filegraphManager.Run();
			}
			catch (Exception ex)
			{
				throw new MediaErrorException("Unable to play selected media.", ex);
			}

			timer.Enabled = true;
			if (PlayingEvent != null) 
			{
				PlayingEvent(this, new EventArgs());
			}
		}

		public override event Server_PausedEventHandler PausedEvent;

		public override void Pause() 
		{
			if (disposed || !initialized)
				return;

			try
			{
				filegraphManager.Pause();
			}
			catch (Exception ex)
			{
				throw new MediaErrorException("Unable to pause selected media.", ex);
			}

			timer.Enabled = false;
			if (PausedEvent != null) 
			{
				PausedEvent(this, new EventArgs());
			}
		}

		public override event Server_StoppedEventHandler StoppedEvent;

		public override void Stop() 
		{
			if (!disposed && initialized)
			{
				try
				{
					filegraphManager.Stop();
					filegraphManager.CurrentPosition = 0;
				}
				catch (Exception ex)
				{
					throw new MediaErrorException("Unable to stop selected media.", ex);
				}

				timer.Enabled = false;
				if (StoppedEvent != null) 
				{
					StoppedEvent(this, new EventArgs());
					ProgressEvent(this, new Server_ProgressEventArgs(0.0));
				}
			}
			
		}

		# endregion

		# region Playing operations

		public override event Server_EndOfStreamEventHandler EndOfStreamEvent;
		public override event Server_ProgressEventHandler ProgressEvent;

		private void Timer_Elapsed(object sender, ElapsedEventArgs e) 
		{
			if (disposed || !initialized)
				return;

			try
			{
				// Check if the media is done playing
				if (filegraphManager.CurrentPosition == filegraphManager.Duration) 
				{
					timer.Enabled = false;
					if (EndOfStreamEvent != null) 
					{
						EndOfStreamEvent(this, new EventArgs());
					}				
				}
					// otherwise, raise progress event
				else 
				{
					if (ProgressEvent != null) 
					{
						ProgressEvent(this, new Server_ProgressEventArgs(filegraphManager.CurrentPosition));
					}
				}
			}
			catch (Exception ex)
			{
				throw new MediaErrorException("Unable to determine media position.", ex);
			}

		}

		# endregion
	
		#region Properties

		public override string MediaFile 
		{
			set 
			{
				if (filegraphManager != null) 
				{
					filegraphManager.Stop();			
					System.Runtime.InteropServices.Marshal.ReleaseComObject(filegraphManager);
				}
				try
				{
					// first check for media already in cache
					if (mediaCache.Contains(value))
					{
						filegraphManager = mediaCache[value];
					}
					else
					{
						filegraphManager = new FilgraphManagerClass();
						filegraphManager.RenderFile(value);
					}
					filegraphManager.Volume  = mediaVolume;
					filegraphManager.Rate	 = mediaRate;
					filegraphManager.Balance = mediaBalance;
					initialized = true;
				}
				catch (Exception ex)
				{
					throw new MediaErrorException("Invalid media file.", ex);
				}

			}
		}

		public override bool IsValid 
		{
			get 
			{
				if (disposed || !initialized || filegraphManager == null) 
				{
					return false;
				} 
				else 
				{
					return true;
				}
			}
		}

		public override double Duration 
		{
			get 
			{
				if (!initialized)
					return 0;
				else
					return filegraphManager.Duration;
			}
		}

		public override event Server_VolumeChangedEventHandler VolumeChanged;

		public override double Volume 
		{
			get 
			{
				// min value = 0, max value = -10,000
				if (!initialized)
					return 1;
				else
					return ( ((double)filegraphManager.Volume + 2000.0) / 2000.0);
			}
			set 
			{
				if (value > 1.0)
					value = 1.0;
				mediaVolume = (int) (0 - ((1.0 - value) * 2000.0));
				if (initialized)
					filegraphManager.Volume = mediaVolume;
				if (VolumeChanged != null) 
					VolumeChanged(this, new Server_VolumeEventArgs(value));
			}
		}

		public override event Server_BalanceChangedEventHandler BalanceChanged;
	
		public override int Balance 
		{
			get 
			{
				if (!initialized)
					return 0;
				else
					return filegraphManager.Balance;
			}
			set 
			{
				mediaBalance = value;
				if (initialized)
					filegraphManager.Balance = value;
				if (BalanceChanged != null)
					BalanceChanged(this, new Server_BalanceEventArgs(value));
			}
		}

		public override event Server_RateChangedEventHandler RateChanged;
	
		public override double Rate 
		{
			get 
			{
				if (!initialized)
					return 1.0;
				else
					return filegraphManager.Rate;
			}
			set 
			{
				if (value > 0 && initialized) 
				{
					mediaRate = value;
					if (initialized)
						filegraphManager.Rate = value;
					if (RateChanged != null)
						RateChanged(this, new Server_RateEventArgs(value));
				}
			}
		}

		/// <summary>
		/// Changes the current position by changeAmount
		/// </summary>
		/// <param name="changeAmount">Amount to change</param>
		public override void MoveTimeByAmount(double changeAmount)
		{
			if (initialized)
			{
				if (changeAmount + filegraphManager.CurrentPosition > filegraphManager.Duration)
				{
					filegraphManager.CurrentPosition = filegraphManager.Duration;
				}
				else if (changeAmount + filegraphManager.CurrentPosition < 0)
				{
					filegraphManager.CurrentPosition = 0;
				}
				else
				{
					filegraphManager.CurrentPosition = filegraphManager.CurrentPosition + changeAmount;
				}
			}
		}

		public override double CurrentPosition 
		{
			get 
			{
				if (initialized)
					return filegraphManager.CurrentPosition;
				else	
					return 0;
			}
			set 
			{
				if (initialized)
				{
					if (value > filegraphManager.Duration)
					{
						filegraphManager.CurrentPosition = filegraphManager.Duration;
					}
					else
					{
						filegraphManager.CurrentPosition = value;
					}
				}
			}
		}

		#endregion

		#region Preloading

		public void PreloadMedia(string filename)
		{
			preloadQueue.Enqueue(filename);

			// if a thread isn't already loading media, kick one off
			if (preloadThread.ThreadState == System.Threading.ThreadState.Suspended)
			{
				preloadThread.Resume();
			}
		}

		// Loads all media in the queue
		private void PreloadMediaThread()
		{
			Thread.CurrentThread.Name = "DirectX Media Preload Thread";

			while (true)
			{
				while (preloadQueue.Count > 0)
				{
					string filename = preloadQueue.Dequeue().ToString();
					FilgraphManagerClass f = new FilgraphManagerClass();
					Debug.WriteLine("Preloading '" + filename + "'", "DirextXPlayer");
					f.RenderFile(filename);
					mediaCache.Add(filename, f);            
				}

				preloadThread.Suspend();
			}
		}

		public void UnloadMedia(string filename)
		{
			FilgraphManagerClass f = mediaCache[filename];
			System.Runtime.InteropServices.Marshal.ReleaseComObject(f);
			mediaCache.Remove(filename);
		}

		#endregion

	}

	#region MediaErrorException

	public class MediaErrorException: System.Exception
	{
		private string text;
		private Exception MediaException;

		public MediaErrorException(string text)
		{
			this.text = text;
		}

		public MediaErrorException(string text, Exception e)
		{
			this.text = text;
			this.MediaException = e;
		}

		public string Text
		{
			get { return text; }
		}
	}

	#endregion

	internal class MediaSourceDictionary: DictionaryBase
	{
		private Queue filenameQueue = new Queue();

		public void Add(string filename, FilgraphManagerClass filegraph)
		{
			// Check if already in queue - if so, ignore
			if (InnerHashtable.Contains(filename))
			{
				InnerHashtable.Add(filename, filegraph);
			}

			filenameQueue.Enqueue(filename);

			// Also remove any entries over the limit, as long as they aren't still in queue
			while (filenameQueue.Count > 30)
			{
				string f = filenameQueue.Dequeue().ToString();
				if (!filenameQueue.Contains(f))
				{
					Debug.WriteLine("Removing file from media cache: " + f, "DirextXPlayer");
					Remove(f);
				}
			}
		}

		public void Remove(string filename)
		{
			FilgraphManagerClass f = (FilgraphManagerClass) InnerHashtable[filename];
			if (f != null)
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(f);
				InnerHashtable.Remove(filename);
			}
		}

		public bool Contains(string filename)
		{
			return InnerHashtable.ContainsKey(filename);
		}

		public FilgraphManagerClass this[string filename]
		{
			get 
			{
				return (FilgraphManagerClass) InnerHashtable[filename];
			}
		}

	}
}
