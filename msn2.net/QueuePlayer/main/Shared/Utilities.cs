using System;
using System.Collections;
using System.Collections.Specialized;
using msn2.net.Common;

namespace msn2.net.QueuePlayer.Shared
{
	#region Events raised by MediaServer

	/* Basic diagnostic events */
	public delegate void LogEventHandler(object sender, LogEventArgs e);
	public delegate void PongEventHandler(object sender, EventArgs e);
	public delegate void MediaErrorEventHandler(object sender, MediaErrorEventArgs e);
	public delegate void ShutdownEventHandler(object sender, EventArgs e);

	/* Normal song events */
	public delegate void PlayingSongEventHandler(object sender, MediaEventArgs e);
	public delegate void PausedSongEventHandler(object sender, MediaEventArgs e);
	public delegate void StoppedSongEventHandler(object sender, MediaEventArgs e);
	public delegate void SetSongEventHandler(object sender, MediaEventArgs e);

	/* Other properties */
	public delegate void ProgressEventHandler(object sender, MediaProgressEventArgs e);
	public delegate void VolumeEventHandler(object sender, MediaVolumeEventArgs e);
	public delegate void RateEventHandler(object sender, MediaRateEventArgs e);
	public delegate void BalanceEventHandler(object sender, MediaBalanceEventArgs e);
	public delegate void MediaCollectionReloadedEventHandler(object sender, EventArgs e);
	public delegate void MediaItemUpdateEventHandler(object sender, MediaItemUpdateEventArgs e);
	public delegate void MediaFileChangedEventHandler(object sender, MediaFileChangedEventArgs e);

	/* Queue related events */
	public delegate void AddedToQueueEventHandler(object sender, QueueEventArgs e);
	public delegate void RemovedFromQueueEventHandler(object sender, QueueEventArgs e);
	public delegate void MoveInQueueEventHandler(object sender, QueueEventArgs e);

	/* History related events */
	public delegate void AddToHistoryEventHandler(object sender, HistoryEventArgs e);
	public delegate void RemoveFromHistoryEventHandler(object sender, HistoryEventArgs e);

	#endregion

	#region EventArg classes used by MediaServer

	/*
	 * EventArgs classes
	 */

	/// <summary>
	/// Logging event arguments
	/// </summary>
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

	/// <summary>
	/// Media item progress event arguments
	/// </summary>
	[Serializable]
	public class MediaProgressEventArgs: EventArgs 
	{
		private double position;

		public MediaProgressEventArgs(double position) 
		{
			this.position = position;
		}
		public double Position 
		{
			get { return position; }
		}
	}

	/// <summary>
	/// Generic media event arguments
	/// </summary>
	[Serializable]
	public class MediaEventArgs: EventArgs 
	{
		private int mediaId;
		private string message;

		public MediaEventArgs(int mediaId) 
		{
			this.mediaId = mediaId;
		}

		public MediaEventArgs(int mediaId, string message) 
		{
			this.mediaId = mediaId;
			this.message = message;
		}

		public int MediaId
		{
			get { return mediaId; }
		}

		public string Message 
		{
			get { return message; }
		}
	}

	[Serializable]
	public class MediaFileChangedEventArgs: EventArgs
	{
		private string fullMediaFileName;

		public MediaFileChangedEventArgs(string fullMediaFileName)
		{
			this.fullMediaFileName = fullMediaFileName;
		}

		public string FullMediaFileName
		{
			get { return fullMediaFileName; }
		}
	}

	/// <summary>
	/// Generic media error event aguments
	/// </summary>
	[Serializable]
	public class MediaErrorEventArgs: EventArgs 
	{
		private int mediaId;
		private int error;
		private string message;
		private Exception ex;

		public MediaErrorEventArgs(int mediaId, int error, string message, Exception ex) 
		{
			this.mediaId = mediaId;
			this.error   = error;
			this.message = message;
			this.ex		 = ex;
		}

		public int MediaId
		{
			get { return mediaId; }
		}

		public int Error 
		{
			get { return error; }
		}

		public string Message 
		{
			get { return message; }
		}

		public Exception Exception 
		{
			get { return ex; }
		}
	}

	/// <summary>
	/// Generic queue action event arguments
	/// </summary>
	[Serializable]
	public class QueueEventArgs: EventArgs 
	{
		private int mediaId		= -1;
		private int newPosition = -1;
		private int position    = -1;
		private Guid guid		= Guid.Empty;

		public QueueEventArgs(int mediaId, Guid guid)
		{
			this.mediaId   = mediaId;
			this.guid	   = guid;
		}

		public QueueEventArgs(int mediaId, Guid guid, int position) 
		{
			this.mediaId   = mediaId;
			this.guid	   = guid;
			this.position  = position;
		}

		public QueueEventArgs(int mediaId, Guid guid, int position, int newPosition) 
		{
			this.mediaId   = mediaId;
			this.guid	   = guid;
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

		public Guid Guid
		{
			get { return guid; }
		}
	}

	/// <summary>
	/// History events details
	/// </summary>
	[Serializable]
	public class HistoryEventArgs: EventArgs
	{
		private int mediaId = -1;
		private Guid guid   = Guid.Empty;
		
		public HistoryEventArgs(int mediaId, Guid guid)
		{
			this.mediaId = mediaId;
			this.guid    = guid;
		}

		public int MediaId
		{
			get { return mediaId; }
		}

		public Guid Guid
		{
			get { return guid; }
		}
	}

	/// <summary>
	/// Volume change event arguments
	/// </summary>
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

	/// <summary>
	/// Rate change event arguments
	/// </summary>
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

	/// <summary>
	/// Balance change event arguments
	/// </summary>
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

	/// <summary>
	/// Type of the media file update
	/// </summary>
	public enum MediaItemUpdateType
	{
		Add,
		Edit,
		Delete
	}

	/// <summary>
	/// Media item add/update/delete event arguments
	/// </summary>
	[Serializable]
	public class MediaItemUpdateEventArgs: EventArgs
	{
		private MediaItemUpdateType type;
		private int mediaId;
		public DataSetMedia.MediaRow Entry = null;

		/// <summary>
		/// Create new media update event args
		/// </summary>
		/// <param name="type">Type of update</param>
		/// <param name="mediaId">Update media ID</param>
		public MediaItemUpdateEventArgs(MediaItemUpdateType type, int mediaId)
		{
			this.type	 = type;
			this.mediaId = mediaId;
		}

		public MediaItemUpdateType Type
		{
			get { return type; }
			set { type = value; }
		}

		public int MediaId
		{
			get { return mediaId; }
			set { mediaId = value; }
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
		private int currentIndex = 0;

		/// <summary>
		/// Add a media collection entry to the list
		/// </summary>
		/// <param name="entry">MediaCollectionEntry pre populated</param>
		public void AddToCollection(MediaCollectionEntry entry) 
		{
			InnerList.Add(entry);
		}

		public void AddToCollection(MediaCollectionEntry entry, int position) 
		{
			InnerList.Insert(position, entry);
		}

		/// <summary>
		/// Get a random media entry
		/// </summary>
		/// <returns>A random entry</returns>
		public MediaCollectionEntry RandomEntry() 
		{
			MediaCollectionEntry retEntry = new MediaCollectionEntry();
			MediaCollectionEntry randomEntry = (MediaCollectionEntry) InnerList[random.Next(InnerList.Count)];
            
			retEntry.MediaFile = randomEntry.MediaFile;
			retEntry.MediaId   = randomEntry.MediaId;

			return retEntry;			
		}

		#region RemoveFromCollection

		/// <summary>
		/// Removes the item with given params from the list
		/// </summary>
		/// <param name="mediaId">ID of the media to remove</param>
		/// <param name="instance">Occurence of the media to remove</param>
		public void RemoveFromCollection(int mediaId, Guid guid) 
		{
			foreach (MediaCollectionEntry entry in new IterIsolate(InnerList))
			{
				if (entry.Guid == guid)
				{
					InnerList.Remove(entry);
				}
			}
		}

		/// <summary>
		/// Removes specific entry from list
		/// </summary>
		/// <param name="entry">Entry to remove</param>
		public void RemoveFromCollection(MediaCollectionEntry entry)
		{
			InnerList.Remove(entry);
		}

		/// <summary>
		/// Removes all instances of a mediaId from the collection
		/// </summary>
		/// <param name="mediaId"></param>
		public void RemoveFromCollection(int mediaId)
		{
			foreach (MediaCollectionEntry entry in new IterIsolate(InnerList))
			{
				if (entry.MediaId == mediaId)
					InnerList.Remove(entry);
			}
		}

		#endregion

		/// <summary>
		/// Dequeues the top item from the collection
		/// </summary>
		public MediaCollectionEntry Dequeue()
		{
			MediaCollectionEntry entry = null;

			if (InnerList.Count > 0)
			{
				entry = (MediaCollectionEntry) InnerList[0];
				InnerList.Remove(entry);
			}
			return entry;
		}

		public int CurrentIndex
		{
			get { return currentIndex; }
			set { currentIndex = value; }
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

	[Serializable]
	public class MediaCollectionEntry 
	{
		private int mediaId;
		private string mediaFile;
		private double duration;
		private Guid guid;

		public MediaCollectionEntry()
		{
			this.guid = Guid.NewGuid();
		}

		public void NewGuid()
		{
			this.guid = Guid.NewGuid();
		}

		public Guid Guid
		{
			get { return guid; }
			set { guid = value; }
		}

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

		public double Duration 
		{
			get { return duration; }
			set { duration = value; }
		}
	}

	#endregion
}
