using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.IO;
using UMShared;

namespace UMServer
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class MediaServer: MarshalByRefObject
	{
		#region Declares

		private DXPlayer dxPlayer;
		private Thread playerThread;
		private MediaCollection mediaCollection = new MediaCollection();
		private MediaCollection mediaQueue = new MediaCollection();
		private MediaCollection mediaHistory = new MediaCollection();
		private PlayState playState;
		private string connectionString;
		private string dropDirectory;
		private string watchDirectory;
		private string shareDirectory;
		private FileSystemWatcher fsWatcher;

		#endregion

		# region Enums

		public enum PlayState 
		{
			Playing,
			Stopped,
			Paused
		}

		#endregion

		#region Constructors

		public MediaServer()
		{
            connectionString = "User ID=sa;Password=too;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;";
			watchDirectory   = @"\\sp\dfs\Music\new";
			dropDirectory	 = @"\\sp\dfs\Music\drop";
			shareDirectory   = @"\\sp\dfs\Music";

            fsWatcher = new FileSystemWatcher();
			fsWatcher.Path = WatchDirectory;
			fsWatcher.IncludeSubdirectories = true;
			fsWatcher.Created += new FileSystemEventHandler(FileSystem_Created);
			fsWatcher.Changed += new FileSystemEventHandler(FileSystem_Changed);
			fsWatcher.EnableRaisingEvents = true;

			playerThread = new Thread(new ThreadStart(PlayerThreadStart));
			playerThread.Start();

			LoadMediaCollection();
			
			while (playerThread.IsAlive) 
				Thread.Sleep(100);

#if !DEBUG
			Next();
#endif
		}

		#endregion

		#region Initialization

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

			SqlConnection cn = new SqlConnection(ConnectionString);
			SqlCommand cmd = new SqlCommand("select MediaID, MediaFile from Media where Deleted=0", cn);
			cn.Open();

			SqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read()) 
			{
				MediaCollectionEntry entry = new MediaCollectionEntry();
				entry.MediaId	= Convert.ToInt32(dr["MediaID"]);
				entry.MediaFile	= dr["MediaFile"].ToString();
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

		#endregion

		#region Diagnostics

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

		public event MediaErrorEventHandler MediaErrorEvent;
        
		/// <summary>
		/// Event when a media error event happens
		/// </summary>
		/// <param name="error">Error number</param>
		/// <param name="message">Error message</param>
		/// <param name="ex">Exception</param>
		public void MediaError(int error, string message, Exception ex) 
		{
			if (MediaErrorEvent != null) 
			{
				MediaErrorEvent(this, new MediaErrorEventArgs(currentMediaId, error, message, ex));
			}
		}

		#endregion

		#region Normal song actions

		protected int currentMediaId;
		private MediaCollectionEntry currentEntry;
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
			TryNext(10, true);
		}


		/// <summary>
		/// Attempt to start the next song in the queue, if possible
		/// </summary>
		/// <param name="count">Recursive count</param>
		private void TryNext(int count, bool addToHistory) {

			// make sure we don't loop through more than 10 times trying 'next'
			if (count == 0) 
			{
				return;
			}
			count--;

			LogDiagnosticEvent("Next", "(" + (0 - count).ToString() + ")");

			// Add current song to history
			if (addToHistory)
			{
				AddToHistory(currentEntry);
			}

			// Get the top song from the queue
			MediaCollectionEntry entry = RemoveFromQueue();
			currentMediaId = entry.MediaId;
			currentEntry   = entry;

			// Make sure media ID exists
			if (!System.IO.File.Exists(entry.MediaFile)
				&& !System.IO.File.Exists(shareDirectory + Path.DirectorySeparatorChar + entry.MediaFile)) 
			{
				MediaError(2, "File does not exist", null);
				TryNext(count, true);
				return;
			}

			dxPlayer.MediaFile = shareDirectory + Path.DirectorySeparatorChar + entry.MediaFile;

			// Check if song is valid
			if (!dxPlayer.IsValid) 
			{
				MediaError(1, "The file could not be loaded.", null);
				TryNext(count, true);
				return;
			}

			Play();
		}

		/// <summary>
		/// Moves to the previous song
		/// </summary>
		public void Previous()
		{
			LogDiagnosticEvent("Previous", "");

            // Get the currently playing song, and add to top of queue
			if (currentMediaId != 0)
			{
				AddToQueue(currentMediaId, 0);
			}

			// Remove the top item from history and play it
			MediaCollectionEntry entry = RemoveFromHistory();

			if (entry != null)
			{
				AddToQueue(entry, 0);
				TryNext(10, false);
			}
			else
			{
				TryNext(10, true);
				MediaError(4, "You cannot go back because there is no more history.", new Exception());
			}
			
		}



		public void AdvanceToSong(int mediaId)
		{
            // Advance to the 'mediaId' entry
			do
			{
				AddToHistory(currentEntry);
				currentEntry = RemoveFromQueue();

			} while (currentEntry.MediaId != mediaId);

			// Re-add the item we want to play to the queue
			AddToQueue(currentEntry, 0);
			TryNext(10, false);
		}

		/// <summary>
		/// Play the media now
		/// </summary>
		/// <param name="mediaId">ID of media to play</param>
		public void PlayMediaId(int mediaId) 
		{
			LogDiagnosticEvent("PlayMediaId", "MediaId = " + mediaId.ToString());
            AddToQueue(mediaId, 0);
			Next();
		}

		public void ChangePosition(double newPosition) 
		{
			LogDiagnosticEvent("ChangePosition", "newPosition = " + newPosition.ToString());
			dxPlayer.CurrentPosition = newPosition;
		}

		public void MoveTimeByAmount(double amount)
		{
			LogDiagnosticEvent("MoveTimeByAmount", "amount = " + amount.ToString());
			dxPlayer.MoveTimeByAmount(amount);
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
		
		#region Queue Events

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

		#endregion

		# region Adding to a queue

		/// <summary>
		/// Add an entry to the queue
		/// </summary>
		/// <param name="mediaId">ID of the media entry to add</param>
		public void AddToQueue(int mediaId) 
		{
			// Add at end of queue
			AddToQueue(mediaId, mediaQueue.Count-1);
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
				AddedToQueueEvent(this, new QueueEventArgs(mediaId, entry.Guid, position));
		}

		private void AddToQueue(MediaCollectionEntry entry)
		{
			LogDiagnosticEvent("AddToQueue", "entry.Guid = " + entry.Guid.ToString());
			mediaQueue.AddToCollection(entry);

			// Notify everyone a song was added
			if (AddedToQueueEvent != null)
				AddedToQueueEvent(this, new QueueEventArgs(entry.MediaId, entry.Guid, mediaQueue.Count-1));
		}

		private void AddToQueue(MediaCollectionEntry entry, int position)
		{
			LogDiagnosticEvent("AddToQueue", "entry.Guid = " + entry.Guid.ToString() + ", position = " + position.ToString());
			mediaQueue.AddToCollection(entry, position);

			// Notify everyone a song was added
			if (AddedToQueueEvent != null)
				AddedToQueueEvent(this, new QueueEventArgs(entry.MediaId, entry.Guid, position));
		}

		#endregion

		/// <summary>
		/// Removes an entry from the queue
		/// </summary>
		/// <param name="mediaId">ID of the media entry to move</param>
		/// <param name="position">Position of the media entry to move</param>
		public void RemoveFromQueue(int mediaId, Guid guid) 
		{
			LogDiagnosticEvent("RemoveToQueue", "mediaId = " + mediaId.ToString() + ", guid = " + guid.ToString());

			foreach (MediaCollectionEntry entry in mediaQueue)
			{
				if (entry.Guid == guid) 
				{
					mediaQueue.RemoveFromCollection(entry);

					// Notify everyone a song was added
					if (RemovedFromQueueEvent != null)
						RemovedFromQueueEvent(this, new QueueEventArgs(entry.MediaId, guid));

					// make sure queue is refilled
					FillQueue();

					break;
				}
			}
		}

		/// <summary>
		/// Removes the top item from the queue and returns it
		/// </summary>
		/// <returns>Top queue item</returns>
		private MediaCollectionEntry RemoveFromQueue()
		{
			LogDiagnosticEvent("RemoveFromQueue", "");

            MediaCollectionEntry entry = mediaQueue.Dequeue();

			if (entry != null)
			{
				// Notify everyone a song was added
				if (RemovedFromQueueEvent != null)
					RemovedFromQueueEvent(this, new QueueEventArgs(entry.MediaId, entry.Guid));
			}
			
			// make sure queue is refilled
			FillQueue();

			return entry;
		}

		/// <summary>
		/// Move a media entry within the queue
		/// </summary>
		/// <param name="mediaId">ID of the media entry to move</param>
		/// <param name="oldPosition">Old queue position</param>
		/// <param name="newPosition">New queue position</param>
		public void MoveInQueue(int mediaId, Guid guid, int newPosition) 
		{
			LogDiagnosticEvent("MoveInQueue", String.Format("mediaId = {0}, guid = {1}, newPosition = {2}",
				mediaId, guid, newPosition));
			
			mediaQueue.RemoveFromCollection(mediaId, guid);
			mediaQueue.AddToCollection(mediaCollection[mediaId], newPosition);

			// Notify everyone a song was moved
			if (MoveInQeuueEvent != null)
				MoveInQeuueEvent(this, new QueueEventArgs(mediaId, guid, -1, newPosition));
		}

		/// <summary>
		/// All songs in the current queue
		/// </summary>
		/// <returns>An ordered list of songs</returns>
		public MediaCollection CurrentQueue() 
		{
			return mediaQueue;
		}

		/// <summary>
		/// Refills a queue to the fixed amount
		/// </summary>
		private void FillQueue() 
		{
			// fill the queue
			while (mediaQueue.Count < 15) 
			{
				AddToQueue(mediaCollection.RandomEntry());
			}
		}

		#endregion

		#region History

		public event AddToHistoryEventHandler AddToHistoryEvent;
		public event RemoveFromHistoryEventHandler RemoveFromHistoryEvent;

		private void AddToHistory(MediaCollectionEntry entry)
		{
			// add the current item to history
			if (entry != null)
			{
				mediaHistory.AddToCollection(entry, 0);
				if (AddToHistoryEvent != null)
					AddToHistoryEvent(this, new HistoryEventArgs(entry.MediaId, entry.Guid));
			}
		}

		private MediaCollectionEntry RemoveFromHistory()
		{
			MediaCollectionEntry entry = null;

			entry = mediaHistory.Dequeue();

			if (entry != null)
			{
				if (RemoveFromHistoryEvent != null)
					RemoveFromHistoryEvent(this, new HistoryEventArgs(entry.MediaId, entry.Guid));
			}
			return entry;
		}

		#endregion

		#region Media related properties 
		
		public event RateEventHandler RateChanged;

		private void DXRateChanged(object sender, DXRateEventArgs e) 
		{
			if (RateChanged != null)
				RateChanged(this, new MediaRateEventArgs(e.Rate));
		}

		/// <summary>
		/// Event occurs whenever a user changes the balance
		/// </summary>
		public event BalanceEventHandler BalanceChanged;

		/// <summary>
		/// Call when the balance has been changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DXBalanceChanged(object sender, DXBalanceEventArgs e) 
		{
			if (BalanceChanged != null)
				BalanceChanged(this, new MediaBalanceEventArgs(e.Balance));
		}

		/// <summary>
		/// Subscribe to this event to known when to reload the collection
		/// </summary>
		public event MediaCollectionReloadedEventHandler MediaCollectionReloadedEvent;

		/// <summary>
		/// Reload the entire media collection
		/// </summary>
		public void ReloadMediaCollection() 
		{
			LoadMediaCollection();

			if (MediaCollectionReloadedEvent != null) 
				MediaCollectionReloadedEvent(this, EventArgs.Empty);
		}

		/// <summary>
		/// Occurs whenever a media item is updated on a client or server
		/// </summary>
		public event MediaItemUpdateEventHandler MediaItemUpdateEvent;

		/// <summary>
		/// Instructs clients and server to load/reload the media item
		/// </summary>
		/// <param name="type">Type of update</param>
		/// <param name="mediaId">Media ID</param>
		public void UpdateMediaItem(MediaItemUpdateType type, int mediaId)
		{
			// Refresh our queues
			if (type == MediaItemUpdateType.Delete)
			{
				mediaCollection.RemoveFromCollection(mediaId);
				mediaQueue.RemoveFromCollection(mediaId);
				mediaHistory.RemoveFromCollection(mediaId);
			}
			else if (type == MediaItemUpdateType.Edit)
			{
				// reload filename in case it changed
				SqlConnection cn = new SqlConnection(ConnectionString);
				cn.Open();
				SqlCommand cmd = new SqlCommand("select MediaFile from Media where MediaID = " + mediaId.ToString(), cn);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
                    mediaCollection[mediaId].MediaFile = dr[0].ToString();					
				}
				dr.Close();
				cn.Close();
			}

			if (MediaItemUpdateEvent != null)
				MediaItemUpdateEvent(this, new MediaItemUpdateEventArgs(type, mediaId));
		}

		#endregion

		#region File system watching events

		private Queue newSongs = new Queue();

		private void FileSystem_Created(object sender, FileSystemEventArgs e)
		{
			LogDiagnosticEvent("FileSystem_Created", "New file detected: " + e.FullPath);

            // Odds are we need to spawn a thread with this item      
			lock (newSongs)
			{
				newSongs.Enqueue(e.FullPath);
			}

			Thread t = new Thread(new ThreadStart(WatchNewFile));
			t.Start();
		}

		private void WatchNewFile()
		{
			bool available = false;
			string newFile;
			lock (newSongs)
			{
				newFile = newSongs.Dequeue().ToString();
			}

			// Loop here, trying to rename the file
			FileStream s = null;
			while (!available)
			{
				try 
				{
					// attempt to open the file exclusively
					s = File.Open(newFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
					available = true;
				}
				catch (IOException)
				{}
				finally 
				{
					if (s != null)
					{
						s.Close();
					}
				}                

				// Wait for file to close
				Thread.Sleep(500);
			}

			// Add to media server and collection
			AddMediaFile(newFile);

            LogDiagnosticEvent("WatchNewFile", String.Format("File {0} is available.", newFile));

		}

		/// <summary>
		/// Adds a media file to the database and local collection
		/// </summary>
		/// <param name="fullpath"></param>
		public void AddMediaFile(string fullpath)
		{
			
			// Connect to db to save update
			SqlConnection cn = new SqlConnection(ConnectionString);
			SqlCommand cmd = new SqlCommand("dbo.s_AddMediaItem", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			// Set up params for sp
			cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@Artist", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@Album", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@Track", SqlDbType.Int);
			cmd.Parameters.Add("@Genre", SqlDbType.NVarChar, 250);
			cmd.Parameters.Add("@Bitrate", SqlDbType.Int);
			cmd.Parameters.Add("@MediaFile", SqlDbType.NVarChar, 500);
			cmd.Parameters.Add("@Duration", SqlDbType.Decimal, 9);
			cmd.Parameters.Add("@MediaId", SqlDbType.Int);
			cmd.Parameters.Add("@MD5", SqlDbType.NVarChar, 50);
			cmd.Parameters["@MediaId"].Direction = ParameterDirection.Output;

			// parse file for short name
			string file = fullpath.Substring(fullpath.LastIndexOf(@"\")+1);
			file = file.Substring(0, file.LastIndexOf(".")).Trim();

			// Set initial field values
			cmd.Parameters["@Name"].Value	= file;
			cmd.Parameters["@Artist"].Value = "unknown";
				
			// Check if a - in name, if so split name up
			if (file.IndexOf("-") > 0) 
			{

				Array arSplit = file.Split('-');
				int splits = arSplit.GetLength(0);

				if (splits <= 6)
				{
					cmd.Parameters["@Artist"].Value = MediaUtilities.ProperCasing(arSplit.GetValue(0).ToString().Trim());

					if (splits >= 2)
						cmd.Parameters["@Name"].Value = MediaUtilities.ProperCasing(arSplit.GetValue(1).ToString().Trim());

					if (splits >= 3)
						cmd.Parameters["@Album"].Value = MediaUtilities.ProperCasing(arSplit.GetValue(2).ToString().Trim());

					if (splits >= 4)
					{
						int track = 0;
						try 
						{
							track = Convert.ToInt32(arSplit.GetValue(3));
						}
						catch (Exception) {}							
						cmd.Parameters["@Track"].Value = track;
					}

					if (splits >= 5)
						cmd.Parameters["@Genre"].Value = MediaUtilities.ProperCasing(arSplit.GetValue(4).ToString().Trim());

					if (splits >= 6)
					{
						string temp = arSplit.GetValue(5).ToString().Trim();
						if (temp.IndexOf(" ") > 0)
							temp = temp.Substring(0, temp.IndexOf(" "));
						int bitrate = 0;
						try 
						{
							bitrate = Convert.ToInt32(temp);
						}
						catch (Exception) {}
						cmd.Parameters["@Bitrate"].Value = bitrate;
					}
				} 
				else
				{
					// we have more then 6 splits
					string temp = file;

					// start with artist and name
					cmd.Parameters["@Name"].Value   = MediaUtilities.ProperCasing(arSplit.GetValue(1).ToString().Trim());
					cmd.Parameters["@Artist"].Value = MediaUtilities.ProperCasing(arSplit.GetValue(0).ToString().Trim());

					// Now get last things, starting with bitrate
					int bitrate = 0;
					try 
					{
						bitrate = Convert.ToInt32(temp.Substring(temp.LastIndexOf("-")+1));
					} 
					catch (Exception) { }
					cmd.Parameters["@Bitrate"].Value = bitrate;
					temp = temp.Substring(0, temp.LastIndexOf("-"));

					// genre
					cmd.Parameters["@Genre"].Value = MediaUtilities.ProperCasing(temp.Substring(temp.LastIndexOf("-")+1));
					temp = temp.Substring(0, temp.LastIndexOf("-"));

					// track
					int track = 0;
					try
					{
						track = Convert.ToInt32(temp.Substring(temp.LastIndexOf("-")+1));
					}
					catch (Exception) { }
					cmd.Parameters["@Track"].Value = track;
					temp = temp.Substring(0, temp.LastIndexOf("-"));

					// Album = what is left after arSplit - 1, and before 'track' position
					string name = arSplit.GetValue(1).ToString();
					temp = temp.Substring(temp.IndexOf(name) + name.Length + 1);
					if (temp.Trim().Substring(0, 1) == "-")
					{
						temp = (temp.Trim().Substring(1).Trim());
					}
					cmd.Parameters["@Album"].Value = MediaUtilities.ProperCasing(temp);

				}

			}
			// Figure out destination filename - and if not there, then move it
			string destFile = MediaUtilities.NameMediaFile(cmd, fullpath);
			destFile = ShareDirectory + @"\" + destFile;
			if (destFile.Length > 225)
			{
				destFile = destFile.Substring(0, 225) + destFile.Substring(destFile.LastIndexOf("."));
			}
			if (fullpath != destFile)
			{
				string targetDir = destFile.Substring(0, destFile.LastIndexOf(@"\"));
				if (!Directory.Exists(targetDir))
				{
					// wrap it in case another thread creates the dir
					try 
					{
						Directory.CreateDirectory(targetDir);
					} 
					catch (Exception) { }
				}

				if (!File.Exists(destFile))
				{

					File.Move(fullpath, destFile);
				}
			}
			cmd.Parameters["@MediaFile"].Value	= destFile;
			cmd.Parameters["@MD5"].Value		= MediaUtilities.MD5ToString(MediaUtilities.MD5Hash(destFile));

			// Now load the file to get the duration
			DXPlayer player = new DXPlayer();
			player.MediaFile = destFile;
			cmd.Parameters["@Duration"].Value = player.Duration;

			// Add to database
			cn.Open();
			cmd.ExecuteNonQuery();

			// figure out the mediaId added                    
			int mediaId = Convert.ToInt32(cmd.Parameters["@MediaID"].Value);
			cn.Close();

			// tell the clients an entry was added
			UpdateMediaItem(MediaItemUpdateType.Add, mediaId);

			// add to server
			MediaCollectionEntry entry = new MediaCollectionEntry();
			entry.MediaId	= mediaId;
			entry.MediaFile	= destFile;
			mediaCollection.AddToCollection(entry);


		}

		private void FileSystem_Changed(object sender, FileSystemEventArgs e)
		{            
			LogDiagnosticEvent("Changed", String.Format("File: {0} [not handled]", e.FullPath));
		}

		#endregion

		#region Server Properties

		/// <summary>
		/// Returns the database connection string the server is using
		/// </summary>
		public string ConnectionString
		{
			get { return connectionString; }
		}

		public string DropDirectory
		{
			get { return dropDirectory; }
		}

		public string WatchDirectory
		{
			get { return watchDirectory; }
		}

		public string ShareDirectory
		{
			get { return shareDirectory; }
		}

        #endregion

	}

	#region Events raised by MediaServer

	/* Basic diagnostic events */
	public delegate void LogEventHandler(object sender, LogEventArgs e);
	public delegate void PongEventHandler(object sender, EventArgs e);
	public delegate void MediaErrorEventHandler(object sender, MediaErrorEventArgs e);
	
	/* Normal song events */
	public delegate void PlayingSongEventHandler(object sender, MediaEventArgs e);
	public delegate void PausedSongEventHandler(object sender, MediaEventArgs e);
	public delegate void StoppedSongEventHandler(object sender, MediaEventArgs e);

	/* Other properties */
	public delegate void ProgressEventHandler(object sender, MediaProgressEventArgs e);
	public delegate void VolumeEventHandler(object sender, MediaVolumeEventArgs e);
	public delegate void RateEventHandler(object sender, MediaRateEventArgs e);
	public delegate void BalanceEventHandler(object sender, MediaBalanceEventArgs e);
	public delegate void MediaCollectionReloadedEventHandler(object sender, EventArgs e);
	public delegate void MediaItemUpdateEventHandler(object sender, MediaItemUpdateEventArgs e);

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
		internal void AddToCollection(MediaCollectionEntry entry) 
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

		#region RemoveFromCollection

		/// <summary>
		/// Removes the item with given params from the list
		/// </summary>
		/// <param name="mediaId">ID of the media to remove</param>
		/// <param name="instance">Occurence of the media to remove</param>
		internal void RemoveFromCollection(int mediaId, Guid guid) 
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
		internal void RemoveFromCollection(MediaCollectionEntry entry)
		{
			InnerList.Remove(entry);
		}

		/// <summary>
		/// Removes all instances of a mediaId from the collection
		/// </summary>
		/// <param name="mediaId"></param>
		internal void RemoveFromCollection(int mediaId)
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
		internal MediaCollectionEntry Dequeue()
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

		internal MediaCollectionEntry this[int mediaId] 
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

		public Guid Guid
		{
			get { return guid; }
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