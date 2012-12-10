using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting;
using System.Threading;
using System.IO;
using System.Configuration;
using msn2.net.QueuePlayer.Shared;
using msn2.net.QueuePlayer;
using System.Timers;

namespace msn2.net.QueuePlayer.Server
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class MediaServer: MarshalByRefObject
	{
		#region Declares

		private DirectXPlayer dxPlayer;
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
		private System.Timers.Timer timer;

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
			connectionString = ConfigurationSettings.AppSettings["connectionString"];;
			shareDirectory   = ConfigurationSettings.AppSettings["shareDirectory"];
			watchDirectory   = ConfigurationSettings.AppSettings["watchDirectory"];
			dropDirectory	 = ConfigurationSettings.AppSettings["dropDirectory"];

			// TODO
			//watchDirectory = @"\\sp\dfs\music\new";

			// See if we should enable the FS watcher
			if (watchDirectory != null && watchDirectory.Length > 0)
			{
				fsWatcher = new FileSystemWatcher();
				fsWatcher.Path = WatchDirectory;
				fsWatcher.IncludeSubdirectories = true;
				fsWatcher.Created += new FileSystemEventHandler(FileSystem_Created);
				fsWatcher.Changed += new FileSystemEventHandler(FileSystem_Changed);
				fsWatcher.EnableRaisingEvents = true;
			}

			timer = new System.Timers.Timer(30000);
			timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
			timer.Enabled = true;

			playerThread = new Thread(new ThreadStart(PlayerThreadStart));
			playerThread.Start();

			LoadMediaCollection();
			
			while (playerThread.IsAlive) 
				Thread.Sleep(100);
//#if !DEBUG
			Next();
//#endif
		}

		#endregion

		#region Initialization

		private void PlayerThreadStart() 
		{
			LogDiagnosticEvent("PlayerThreadStart", "Starting player thread.");

			dxPlayer = new DirectXPlayer();

			// subscribe to events of mediaPlayerHost
			dxPlayer.EndOfStreamEvent	+= new Server_EndOfStreamEventHandler(Server_EndOfStreamEvent);
			dxPlayer.PlayingEvent		+= new Server_PlayingEventHandler(PlayingEvent);
			dxPlayer.StoppedEvent		+= new Server_StoppedEventHandler(StoppedEvent);
			dxPlayer.PausedEvent		+= new Server_PausedEventHandler(PausedEvent);
			dxPlayer.ProgressEvent		+= new Server_ProgressEventHandler(Server_ProgressEvent);
			dxPlayer.VolumeChanged		+= new Server_VolumeChangedEventHandler(Server_VolumeEvent);
			dxPlayer.RateChanged		+= new Server_RateChangedEventHandler(Server_RateChanged);
			dxPlayer.BalanceChanged		+= new Server_BalanceChangedEventHandler(Server_BalanceChanged);
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

		public event ShutdownEventHandler ShutdownEvent;

		public void Shutdown()
		{
			if (ShutdownEvent != null)
				ShutdownEvent(this, EventArgs.Empty);
		}

		#endregion

		#region Normal song actions

		protected int currentMediaId;
		private MediaCollectionEntry currentEntry;
		public event PlayingSongEventHandler PlayingSongEvent;
		public event PausedSongEventHandler  PausedSongEvent;
		public event StoppedSongEventHandler StoppedSongEvent;
		public event SetSongEventHandler SetSongEvent;
		public event ProgressEventHandler ProgressEvent;
		public event VolumeEventHandler VolumeEvent;
		public event MediaFileChangedEventHandler MediaFileChanged;

		#region Media Properties

		public PlayState CurrentPlayState 
		{
			get 
			{ 
				return playState;	
			}
		}

		public int CurrentMediaId 
		{
			get 
			{ 
				return currentMediaId; 
			}
		}

		public double Duration 
		{
			get 
			{
				double duration = 0.0;
				try
				{
					duration = dxPlayer.Duration; 
				}
				catch (Exception ex)
				{
					MediaError(0, "Unable to read current duration.", ex);
				}
				return duration;
			}
		}

		public double Volume 
		{
			get 
			{
				double volume = 0.0;

				try
				{
					volume = dxPlayer.Volume; 
				}
				catch (Exception ex)
				{
					MediaError(0, "Unable to read player volume.", ex);
				}

				return volume;
			}
			set 
			{
				try
				{
					dxPlayer.Volume = value; 
				}
				catch (Exception ex)
				{
					MediaError(0, "Unable to set player volume.", ex);
				}
			}
		}

		public double Rate 
		{
			get 
			{ 
				double rate = 0.0;

				try
				{
					rate = dxPlayer.Rate;
				
				}
				catch (Exception ex)
				{
					MediaError(0, "Unable to read rate.", ex);
				}
				
				return rate;
			}
			set 
			{ 
				try
				{
					dxPlayer.Rate = value; 
				}
				catch (Exception ex)
				{
					MediaError(0, "Unable to set rate.", ex);
				}
			}
		}

		public int Balance 
		{
			get 
			{ 
				int balance = 0;

				try
				{
					balance = dxPlayer.Balance;
				}
				catch (Exception ex)
				{
					MediaError(0, "Unable to read balance volume.", ex);
				}

				return balance; 
			}
			set 
			{ 
				try
				{
					dxPlayer.Balance = value; 
				}
				catch (Exception ex)
				{
					MediaError(0, "Unable to set balance.", ex);
				}
			}
		}

		#endregion 


		/// <summary>
		/// Plays the currently selected song
		/// </summary>
		public void Play() 
		{
			LogDiagnosticEvent("Play", "Playing song.");

			try
			{
				dxPlayer.Play();
			}
			catch (Exception ex)
			{
				MediaError(0, "Unable to play media.", ex);
			}

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

			try
			{
				dxPlayer.Pause();
			}
			catch (Exception ex)
			{
				MediaError(0, "Unable to pause player", ex);
			}
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


			try
			{
				dxPlayer.Stop();
			}
			catch (Exception ex)
			{
				MediaError(0, "Unable to stop player.", ex);
			}

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

			// broadcast to players that the media file changed
			if (MediaFileChanged != null)
				MediaFileChanged(this, new MediaFileChangedEventArgs(shareDirectory + Path.DirectorySeparatorChar + entry.MediaFile));

			// Attempt to load the media file
			try
			{
				dxPlayer.MediaFile = shareDirectory + Path.DirectorySeparatorChar + entry.MediaFile;
			}
			catch (Exception ex)
			{
				MediaError(0, "Unable to load media file", ex);
				TryNext(count, true);
			}

			if (SetSongEvent != null)
				SetSongEvent(this, new MediaEventArgs(entry.MediaId));

			// Check if song is valid
			try
			{
				if (!dxPlayer.IsValid) 
				{
					MediaError(1, "The file could not be loaded.", null);
					TryNext(count, true);
					return;
				}
			}
			catch (Exception ex)
			{
				MediaError(0, "Unable to determine media file status.", ex);
				TryNext(count, true);
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

		public double CurrentPosition
		{
			get 
			{ 
				double currentPosition = 0;

				try
				{
					currentPosition = dxPlayer.CurrentPosition;
				}
				catch (Exception ex)
				{
					MediaError(0, "Unable to read current player position.", ex);
				}

				return currentPosition;
			}
			set
			{
				try
				{
					dxPlayer.CurrentPosition = value;
				}
				catch (Exception ex)
				{
					MediaError(0, "Unable to set player position.", ex);
				}
			}
		}

		public void MoveTimeByAmount(double amount)
		{
			LogDiagnosticEvent("MoveTimeByAmount", "amount = " + amount.ToString());

			try
			{
				dxPlayer.MoveTimeByAmount(amount);
			}
			catch (Exception ex)
			{
				MediaError(0, "Unable to change media position.", ex);
			}
		}

		private void Server_EndOfStreamEvent(object sender, EventArgs e) 
		{
			LogDiagnosticEvent("EndOfStreamEvent", "");
			Next();
		}

		private void Server_ProgressEvent(object sender, Server_ProgressEventArgs e) 
		{
			LogDiagnosticEvent("Server_ProgressEvent", "position = " + e.Position.ToString());
			if (ProgressEvent != null)
				ProgressEvent(this, new MediaProgressEventArgs(e.Position));
		}

		private void Server_VolumeEvent(object sender, Server_VolumeEventArgs e) 
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
			entry.NewGuid();
			mediaQueue.AddToCollection(entry, position);

			// Notify everyone a song was added
			if (AddedToQueueEvent != null)
				AddedToQueueEvent(this, new QueueEventArgs(mediaId, entry.Guid, position));
		}

		private void AddToQueue(MediaCollectionEntry entry)
		{
			LogDiagnosticEvent("AddToQueue", "entry.Guid = " + entry.Guid.ToString());
			entry.NewGuid();
			mediaQueue.AddToCollection(entry);

			// Notify everyone a song was added
			if (AddedToQueueEvent != null)
				AddedToQueueEvent(this, new QueueEventArgs(entry.MediaId, entry.Guid, mediaQueue.Count-1));
		}

		private void AddToQueue(MediaCollectionEntry entry, int position)
		{
			LogDiagnosticEvent("AddToQueue", "entry.Guid = " + entry.Guid.ToString() + ", position = " + position.ToString());
			entry.NewGuid();
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
			MediaCollectionEntry entry = mediaCollection[mediaId];
			entry.Guid = guid;
			mediaQueue.AddToCollection(entry, newPosition);

			// Notify everyone a song was moved
			if (MoveInQeuueEvent != null)
				MoveInQeuueEvent(this, new QueueEventArgs(mediaId, entry.Guid, newPosition));
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
			while (mediaQueue.Count < 25) 
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

				// Log the fact this song was played to history table
				SqlConnection cn = new SqlConnection(connectionString);
				SqlCommand cmd   = new SqlCommand("s_AddToHistory", cn);
				cmd.CommandType  = CommandType.StoredProcedure;
            
				cmd.Parameters.Add("@MediaId", SqlDbType.Int);
				cmd.Parameters.Add("@MediaPosition", SqlDbType.Decimal);
				cmd.Parameters.Add("@CurrentUser", SqlDbType.NVarChar, 50);
				cmd.Parameters.Add("@Computer", SqlDbType.NVarChar, 50);

				cmd.Parameters["@MediaId"].Value = entry.MediaId;
				cmd.Parameters["@MediaPosition"].Value = Convert.ToDecimal(dxPlayer.CurrentPosition);
				cmd.Parameters["@CurrentUser"].Value = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
				cmd.Parameters["@Computer"].Value  = System.Environment.MachineName;

				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();
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

		private void Server_RateChanged(object sender, Server_RateEventArgs e) 
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
		private void Server_BalanceChanged(object sender, Server_BalanceEventArgs e) 
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
		private Queue watchingSongs = new Queue();

		private void FileSystem_Created(object sender, FileSystemEventArgs e)
		{
			LogDiagnosticEvent("FileSystem_Created", "New file detected: " + e.FullPath);

			string ext = e.FullPath.Substring(e.FullPath.LastIndexOf(".")+1);
			if (ext != "mp3" && ext != "wma")
				return;

            // Odds are we need to spawn a thread with this item      
			lock (newSongs)
			{
				if (!newSongs.Contains(e.FullPath))
				{
					newSongs.Enqueue(e.FullPath);
					Thread t = new Thread(new ThreadStart(WatchNewFile));
					t.Start();
				}
			}

		}

		private void CheckForNewFiles()
		{
			foreach(string s in Directory.GetFiles(watchDirectory))
			{
                string ext = s.Substring(s.LastIndexOf(".")+1);
				if (ext == "mp3" || ext == "wma")
				{
					// Odds are we need to spawn a thread with this item      
					lock (newSongs)
					{
						lock (watchingSongs)
						{
							if (!newSongs.Contains(s) && !watchingSongs.Contains(s))
							{
								newSongs.Enqueue(s);
								Thread t = new Thread(new ThreadStart(WatchNewFile));
								t.Start();
							}
						}
					}
				}
			}
		}

		private void WatchNewFile()
		{
			bool available = false;
			int attempts = 0;

			string newFile;
			lock (newSongs)
			{
				lock (watchingSongs)
				{
					newFile = newSongs.Dequeue().ToString();
					watchingSongs.Enqueue(newFile);
				}
			}

			// Loop here, trying to rename the file
			FileStream s = null;
			while (!available)
			{
				attempts++;
				if (attempts > 500)
					return;

				try 
				{
					// attempt to open the file exclusively
					 s = File.Open(newFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
					available = true;
				}
				catch (FileNotFoundException)
				{
					return;
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
			cmd.Parameters["@MediaFile"].Value	= destFile.Substring(ShareDirectory.Length+1);
			cmd.Parameters["@MD5"].Value		= MediaUtilities.MD5ToString(MediaUtilities.MD5Hash(destFile));

			// Now load the file to get the duration
			using (DirectXPlayer player = new DirectXPlayer())
			{
				try 
				{
					player.MediaFile = destFile;
					cmd.Parameters["@Duration"].Value = player.Duration;
				}
				catch (Exception ex)
				{
                    MediaError(0, "Exception reading new media file: " + destFile, ex);
					return;
				}
			}

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
			entry.MediaFile	= destFile.Substring(ShareDirectory.Length+1);
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

		#region Timer

		private void Timer_Elapsed(object sender, ElapsedEventArgs e) 
		{
			timer.Enabled = false;

			if (watchDirectory != null && watchDirectory.Length > 0)
			{
				CheckForNewFiles();
			}

			timer.Enabled = true;
		}

		#endregion

	}

}