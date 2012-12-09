using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;

namespace UMServer
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class MediaServer: MarshalByRefObject
	{
		private DXPlayer dxPlayer;
		private Thread playerThread;
		private MediaCollection mediaCollection = new MediaCollection();
		private MediaCollection mediaQueue = new MediaCollection();
		private PlayState playState;

		public enum PlayState 
		{
			Playing,
			Stopped,
			Paused
		}

		public MediaServer()
		{
			playerThread = new Thread(new ThreadStart(PlayerThreadStart));
			playerThread.Start();

			LoadMediaCollection();
			
			while (playerThread.IsAlive) 
				Thread.Sleep(100);

#if !DEBUG
			Next();
#endif
		}

		private void PlayerThreadStart() 
		{
			LogDiagnosticEvent("PlayerThreadStart", "Starting player thread.");

			dxPlayer = new DXPlayer();

			// subscribe to events of mediaPlayerHost
			dxPlayer.EndOfStreamEvent	+= new DXEndOfStreamEventHandler(DXEndOfStreamEvent);
			dxPlayer.PlayingEvent		+= new DXPlayingEventHandler(PlayingEvent);
			dxPlayer.StoppedEvent		+= new DXStoppedEventHandler(StoppedEvent);
			dxPlayer.PausedEvent		+= new DXPausedEventHandler(PausedEvent);
			dxPlayer.ProgressEvent		+= new DXProgressEventHandler(DXProgressEvent);
			dxPlayer.VolumeChanged		+= new DXVolumeChangedEventHandler(DXVolumeEvent);
			dxPlayer.RateChanged		+= new DXRateChangedEventHandler(DXRateChanged);
			dxPlayer.BalanceChanged		+= new DXBalanceChangedEventHandler(DXBalanceChanged);
		}

		private void LoadMediaCollection() 
		{
			LogDiagnosticEvent("LoadMediaCollection", "Loading media collection.");

			SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
			SqlCommand cmd = new SqlCommand("select ID, Filename from Media", cn);
			cn.Open();

			SqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read()) 
			{
				MediaCollectionEntry entry = new MediaCollectionEntry();
				entry.MediaId	= Convert.ToInt32(dr["ID"]);
				entry.MediaFile	= dr["Filename"].ToString();
				mediaCollection.AddToCollection(entry);
			}
			dr.Close();
			cn.Close();

			FillQueue();
		}

		// This is to insure that when created as a Singleton, the first instance never dies,
		// regardless of the time between chat users.
		public override object InitializeLifetimeService()
		{
			return null;
		}

		public void LogDiagnosticEvent(string function, string message) 
		{
			if (LogEvent != null)
				LogEvent(this, new LogEventArgs(function, message));
		}

		public event LogEventHandler LogEvent;

		public event PongEventHandler PongEvent;
		public void Ping() 
		{
			LogDiagnosticEvent("Ping", "");

			if (PongEvent != null) 
			{
				PongEvent(this, new EventArgs());
			}
		}

		#region Normal song actions

		protected int currentMediaId;
		public event PlayingSongEventHandler PlayingSongEvent;
		public event PausedSongEventHandler  PausedSongEvent;
		public event StoppedSongEventHandler StoppedSongEvent;
		public event ProgressEventHandler ProgressEvent;
		public event VolumeEventHandler VolumeEvent;

		public PlayState CurrentPlayState 
		{
			get { return playState;	}
		}

		public int CurrentMediaId 
		{
			get { return currentMediaId; }
		}

		public double Duration 
		{
			get { return dxPlayer.Duration; }
		}

		public double Volume 
		{
			get { return dxPlayer.Volume; }
			set { dxPlayer.Volume = value; }
		}

		public double Rate 
		{
			get { return dxPlayer.Rate; }
			set { dxPlayer.Rate = value; }
		}

		public int Balance 
		{
			get { return dxPlayer.Balance; }
			set { dxPlayer.Balance = value; }
		}

		/// <summary>
		/// Plays the currently selected song
		/// </summary>
		public void Play() 
		{
			LogDiagnosticEvent("Play", "Playing song.");
			dxPlayer.Play();
		}

		protected void PlayingEvent(object sender, EventArgs e) 
		{
			LogDiagnosticEvent("PlayingEvent", "Raising play event");
			playState = PlayState.Playing;

			// Notify everyone a song is playing
			if (PlayingSongEvent != null)
				PlayingSongEvent(this, new MediaEventArgs(currentMediaId));
		}

		/// <summary>
		/// Pause the currently playing song
		/// </summary>
		public void Pause() 
		{
			LogDiagnosticEvent("Pause", "Pausing song.");
			playState = PlayState.Paused;

			dxPlayer.Pause();
		}

		protected void PausedEvent(object sender, EventArgs e)
		{
			LogDiagnosticEvent("PausedEvent", "PausedEvent");

			// Notify everyone a song has paused
			if (PausedSongEvent != null)
				PausedSongEvent(this, new MediaEventArgs(currentMediaId));
		}

		/// <summary>
		/// Stop the currently playing song
		/// </summary>
		public void Stop() 
		{
			LogDiagnosticEvent("Stop", "");
			playState = PlayState.Stopped;

			dxPlayer.Stop();
		}

		protected void StoppedEvent(object sender, EventArgs e) 
		{
			LogDiagnosticEvent("StoppedEvent","");

			// Notify everyone a song has stopped
			if (StoppedSongEvent != null)
				StoppedSongEvent(this, new MediaEventArgs(currentMediaId));
		}

		/// <summary>
		/// Starts the next song
		/// </summary>
		public void Next() 
		{
			LogDiagnosticEvent("Next", "");

			// Pop the top queue entry
			MediaCollectionEntry entry = mediaQueue.Dequeue();
			FillQueue();

			dxPlayer.MediaFile = entry.MediaFile;
			currentMediaId = entry.MediaId;

			// Notify everyone a song was removed
			if (RemovedFromQueueEvent != null) 
			{
				LogDiagnosticEvent("Next.RemovedFromQueueEvent", entry.MediaId.ToString());
				RemovedFromQueueEvent(this, new QueueEventArgs(entry.MediaId, 0));
			}

			Play();
		}

		public void ChangePosition(double newPosition) 
		{
			LogDiagnosticEvent("ChangePosition", "newPosition = " + newPosition.ToString());
			dxPlayer.CurrentPosition = newPosition;
		}

		private void DXEndOfStreamEvent(object sender, EventArgs e) 
		{
			LogDiagnosticEvent("EndOfStreamEvent", "");
			Next();
		}

		private void DXProgressEvent(object sender, DXProgressEventArgs e) 
		{
			LogDiagnosticEvent("DXProgressEvent", "progress = " + e.Progress.ToString());
			if (ProgressEvent != null)
				ProgressEvent(this, new MediaProgressEventArgs(e.Progress));
		}

		private void DXVolumeEvent(object sender, DXVolumeEventArgs e) 
		{
			LogDiagnosticEvent("MediaPlayerVolumeEvent", "volume = " + e.Volume.ToString());
			if (VolumeEvent != null) 
				VolumeEvent(this, new MediaVolumeEventArgs(e.Volume));
		}

		#endregion

		#region Queue related activities
		
		/// <summary>
		/// Raised whenever something is added to the queue
		/// </summary>
		public event AddedToQueueEventHandler AddedToQueueEvent;
		/// <summary>
		/// Raised whenever something is removed from the queue
		/// </summary>
		public event RemovedFromQueueEventHandler RemovedFromQueueEvent;
		/// <summary>
		/// Raised whenever something is moved in the queue
		/// </summary>
		public event MoveInQueueEventHandler MoveInQeuueEvent;

		/// <summary>
		/// Add an entry to the queue
		/// </summary>
		/// <param name="mediaId">ID of the media entry to add</param>
		public void AddToQueue(int mediaId) 
		{
			LogDiagnosticEvent("AddToQueue", "mediaId = " + mediaId.ToString());
			MediaCollectionEntry entry = mediaCollection[mediaId];
			mediaQueue.AddToCollection(entry);

			// Notify everyone a song was added
			if (AddedToQueueEvent != null)
				AddedToQueueEvent(this, new QueueEventArgs(mediaId, mediaQueue.Count-1));
		}

		/// <summary>
		/// Add an entry to the queue
		/// </summary>
		/// <param name="mediaId">ID of the media entry to add</param>
		/// <param name="position">Position to place the new entry</param>
		public void AddToQueue(int mediaId, int position) 
		{
			LogDiagnosticEvent("AddToQueue", "mediaId = " + mediaId.ToString() + ", position = " + position.ToString());
			MediaCollectionEntry entry = mediaCollection[mediaId];
			mediaQueue.AddToCollection(entry, position);

			// Notify everyone a song was added
			if (AddedToQueueEvent != null)
				AddedToQueueEvent(this, new QueueEventArgs(mediaId, position));
		}

		/// <summary>
		/// Removes an entry from the queue
		/// </summary>
		/// <param name="mediaId">ID of the media entry to move</param>
		/// <param name="position">Position of the media entry to move</param>
		public void RemoveFromQueue(int mediaId, int position) 
		{
			LogDiagnosticEvent("RemoveToQueue", "mediaId = " + mediaId.ToString() + ", position = " + position.ToString());

			mediaQueue.RemoveFromCollection(mediaId, position);

			// Notify everyone a song was added
			if (RemovedFromQueueEvent != null)
				RemovedFromQueueEvent(this, new QueueEventArgs(mediaId, position));

			FillQueue();
		}

		/// <summary>
		/// Move a media entry within the queue
		/// </summary>
		/// <param name="mediaId">ID of the media entry to move</param>
		/// <param name="oldPosition">Old queue position</param>
		/// <param name="newPosition">New queue position</param>
		public void MoveInQueue(int mediaId, int oldPosition, int newPosition) 
		{
			LogDiagnosticEvent("MoveInQueue", String.Format("mediaId = {0}, oldPosition = {1}, newPosition = {2}",
				mediaId, oldPosition, newPosition));
			
			mediaQueue.RemoveFromCollection(mediaId, oldPosition);
			mediaQueue.AddToCollection(mediaCollection[mediaId], newPosition);

			// Notify everyone a song was moved
			if (MoveInQeuueEvent != null)
				MoveInQeuueEvent(this, new QueueEventArgs(mediaId, oldPosition, newPosition));
		}

		/// <summary>
		/// All songs in the current queue
		/// </summary>
		/// <returns>An ordered list of songs</returns>
		public MediaCollection CurrentQueue() 
		{
			return mediaQueue;
		}

		private void FillQueue() 
		{
			// fill the queue
			while (mediaQueue.Count < 15) 
			{
				MediaCollectionEntry entry = mediaCollection.RandomEntry();
				mediaQueue.AddToCollection(entry);
				if (AddedToQueueEvent != null) 
				{
					AddedToQueueEvent(this, new QueueEventArgs(entry.MediaId, mediaQueue.Count-1));
				}
			}
		}

		#endregion

		#region Media related properties 
		
		public event RateEventHandler RateChanged;

		private void DXRateChanged(object sender, DXRateEventArgs e) 
		{
			if (RateChanged != null)
				RateChanged(this, new MediaRateEventArgs(e.Rate));
		}

		public event BalanceEventHandler BalanceChanged;

		private void DXBalanceChanged(object sender, DXBalanceEventArgs e) 
		{
			if (BalanceChanged != null)
				BalanceChanged(this, new MediaBalanceEventArgs(e.Balance));
		}

		#endregion
	}

	#region Events raised by MediaServer

	/* Basic diagnostic events */
	public delegate void LogEventHandler(object sender, LogEventArgs e);
	public delegate void PongEventHandler(object sender, EventArgs e);
	
	/* Normal song events */
	public delegate void PlayingSongEventHandler(object sender, MediaEventArgs e);
	public delegate void PausedSongEventHandler(object sender, MediaEventArgs e);
	public delegate void StoppedSongEventHandler(object sender, MediaEventArgs e);

	/* Other properties */
	public delegate void ProgressEventHandler(object sender, MediaProgressEventArgs e);
	public delegate void VolumeEventHandler(object sender, MediaVolumeEventArgs e);
	public delegate void RateEventHandler(object sender, MediaRateEventArgs e);
	public delegate void BalanceEventHandler(object sender, MediaBalanceEventArgs e);

	/* Queue related events */
	public delegate void AddedToQueueEventHandler(object sender, QueueEventArgs e);
	public delegate void RemovedFromQueueEventHandler(object sender, QueueEventArgs e);
	public delegate void MoveInQueueEventHandler(object sender, QueueEventArgs e);

	#endregion

	#region EventArg classes used by MediaServer

	/*
	 * EventArgs classes
	 */

	[Serializable]
	public class LogEventArgs: EventArgs 
	{
		private string function;
		private string message;

		public LogEventArgs(string function, string message) 
		{
			this.function = function;
			this.message = message;
		}

		public string Message 
		{
			get { return message; }
		}

		public string Function 
		{
			get { return function; }
		}
	}

	[Serializable]
	public class MediaProgressEventArgs: EventArgs 
	{
		private double progress;

		public MediaProgressEventArgs(double progress) 
		{
			this.progress = progress;
		}
		public double Progress 
		{
			get { return progress; }
		}
	}

	[Serializable]
	public class MediaEventArgs: EventArgs 
	{
		private int mediaId;

		public MediaEventArgs(int mediaId) 
		{
			this.mediaId = mediaId;
		}

		public int MediaId
		{
			get { return mediaId; }
		}
	}

	[Serializable]
	public class QueueEventArgs: EventArgs 
	{
		private int mediaId   = -1;
		private int newPosition = -1;
		private int position = -1;

		public QueueEventArgs(int mediaId, int position) 
		{
			this.mediaId   = mediaId;
			this.position = position;
		}

		public QueueEventArgs(int mediaId, int position, int newPosition) 
		{
			this.mediaId   = mediaId;
			this.position  = position;
			this.newPosition = newPosition;
		}

		public int MediaId
		{
			get { return mediaId; }
		}

		public int Position
		{
			get { return position; }
		}

		public int NewPosition 
		{
			get { return newPosition; }
		}
	}

	[Serializable]
	public class MediaVolumeEventArgs: EventArgs
	{
		private double volume;

		public MediaVolumeEventArgs(double volume) 
		{
			this.volume = volume;
		}

		public double Volume 
		{
			get { return volume; }
		}
	}

	[Serializable]
	public class MediaRateEventArgs: EventArgs
	{
		private double rate;

		public MediaRateEventArgs(double rate) 
		{
			this.rate = rate;
		}

		public double Rate 
		{
			get { return rate; }
		}
	}


	[Serializable]
	public class MediaBalanceEventArgs: EventArgs
	{
		private int balance;

		public MediaBalanceEventArgs(int balance) 
		{
			this.balance = balance;
		}

		public int Balance 
		{
			get { return balance; }
		}
	}
	
	#endregion

	#region Collection related classes

	/// <summary>
	/// Collection of media entries
	/// </summary>
	[Serializable]
	public class MediaCollection: ReadOnlyCollectionBase 
	{
		private Random random = new Random();

		/// <summary>
		/// Add a media collection entry to the list
		/// </summary>
		/// <param name="entry">MediaCollectionEntry pre populated</param>
		public void AddToCollection(MediaCollectionEntry entry) 
		{
			InnerList.Add(entry);
		}

		internal void AddToCollection(MediaCollectionEntry entry, int position) 
		{
			InnerList.Insert(position, entry);
		}

		/// <summary>
		/// Get a random media entry
		/// </summary>
		/// <returns>A random entry</returns>
		internal MediaCollectionEntry RandomEntry() 
		{
			
			MediaCollectionEntry retEntry = new MediaCollectionEntry();
			MediaCollectionEntry randomEntry = (MediaCollectionEntry) InnerList[random.Next(InnerList.Count)];
            
			retEntry.MediaFile = randomEntry.MediaFile;
			retEntry.MediaId   = randomEntry.MediaId;

			return retEntry;			
		}

		/// <summary>
		/// Removes the item with given params from the list
		/// </summary>
		/// <param name="mediaId">ID of the media to remove</param>
		/// <param name="instance">Occurence of the media to remove</param>
		internal void RemoveFromCollection(int mediaId, int position) 
		{
			if (position != -1) 
			{
				InnerList.RemoveAt(position);
			} 
			else 
			{
				// find the first matching item
				foreach (MediaCollectionEntry entry in InnerList) 
				{
					if (entry.MediaId == mediaId) 
					{
						InnerList.Remove(entry);
						return;
					}
				}
			}
		}

		/// <summary>
		/// Pop the top of the queue
		/// </summary>
		/// <returns></returns>
		internal MediaCollectionEntry Dequeue() 
		{
			MediaCollectionEntry entry = (MediaCollectionEntry) InnerList[0];
			InnerList.Remove(entry);
			return entry;
		}

		public MediaCollectionEntry this[int mediaId] 
		{
			get 
			{
				foreach(MediaCollectionEntry entry in InnerList) 
				{
					if (entry.MediaId == mediaId)
						return entry;
				}
				return null;
			}
		}
	}

	public class MediaCollectionEntry 
	{
		private int mediaId;
		private string mediaFile;
		private string name;
		private string artist;
		private double duration;

		public int MediaId 
		{
			get { return mediaId; }
			set { mediaId = value; }
		}

		public string MediaFile 
		{
			get { return mediaFile; }
			set { mediaFile = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string Artist 
		{
			get { return artist; }
			set { artist = value; }
		}

		public double Duration 
		{
			get { return duration; }
			set { duration = value; }
		}
	}

	#endregion
}