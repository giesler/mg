using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Messaging;
using System.IO;
using msn2.net.QueuePlayer.Shared;
using msn2.net.QueuePlayer.Server;
using msn2.net.QueuePlayer;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for Client.
	/// </summary>
	public class Client: MarshalByRefObject, IDisposable
	{
		private UMPlayer umPlayer;
		public MediaServer mediaServer;
		public DataSetMedia	dsMedia = new DataSetMedia();
		private bool logging;
		private bool connected = false;
		public DirectXPlayer player = null;

		private string connectionString;

		// This override ensures that if the object is idle for an extended 
		// period, waiting for messages, it won't lose its lease. Without this 
		// override (or an alternative, such as implementation of a lease 
		// sponsor), an idle object that inherits from MarshalByRefObject 
		// may be collected even though references to it still exist.
		/// <summary>
		/// Prevent garbage collection of object
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService() 
		{
			return null;
		}

		/// <summary>
		/// Default consturcotr
		/// </summary>
		/// <param name="umPlayer">Media player form</param>
		public Client(UMPlayer umPlayer)
		{
			this.umPlayer = umPlayer;

			// Create a server
			mediaServer = new MediaServer();

			try
			{
				connectionString = mediaServer.ConnectionString;

				// Load the list of media
				LoadMediaCollection();

				// Subscribe to events
				mediaServer.PlayingSongEvent += new PlayingSongEventHandler(PlayingEvent);
				mediaServer.PausedSongEvent += new PausedSongEventHandler(PausedEvent);
				mediaServer.StoppedSongEvent += new StoppedSongEventHandler(StoppedEvent);
				mediaServer.ProgressEvent += new ProgressEventHandler(ProgressEvent);
				mediaServer.PongEvent += new PongEventHandler(PongEvent);
				mediaServer.VolumeEvent += new VolumeEventHandler(VolumeEvent);

				// Queue Events
				mediaServer.AddedToQueueEvent += new AddedToQueueEventHandler(AddedToQueueEvent);
				mediaServer.RemovedFromQueueEvent += new RemovedFromQueueEventHandler(RemovedFromQueueEvent);
				mediaServer.MoveInQeuueEvent += new MoveInQueueEventHandler(MovedInQueueEvent);

				// History Events
				mediaServer.AddToHistoryEvent += new AddToHistoryEventHandler(AddedToHistoryEvent);
				mediaServer.RemoveFromHistoryEvent += new RemoveFromHistoryEventHandler(RemovedFromHistoryEvent);

				// Advanced
				mediaServer.RateChanged += new RateEventHandler(RateEvent);
				mediaServer.BalanceChanged += new BalanceEventHandler(BalanceEvent);

				mediaServer.MediaCollectionReloadedEvent += new MediaCollectionReloadedEventHandler(MediaCollectionReloadedEvent);
				mediaServer.MediaErrorEvent += new MediaErrorEventHandler(MediaErrorEvent);
				mediaServer.MediaItemUpdateEvent += new MediaItemUpdateEventHandler(MediaItemUpdate);

				mediaServer.MediaFileChanged += new MediaFileChangedEventHandler(MediaFileChanged);
				mediaServer.ShutdownEvent += new ShutdownEventHandler(ShutdownEvent);

				connected = true;
			
			}
			catch (Exception ex)
			{
                System.Diagnostics.Trace.WriteLine(ex.ToString());                
			}
		}

		public void StartLocalPlayer()
		{
            player = new DirectXPlayer();
			
		}

		public void StopLocalPlayer()
		{
			if (player != null)
			{
				player.Stop();
				player = null;
			}
		}

		public bool Connected
		{
			get { return connected; }
		}

		public void Dispose() 
		{
		}

		public void Disconnect()
		{

			if (logging)
				mediaServer.LogEvent -= new LogEventHandler(LogServerEvent);

			mediaServer.PlayingSongEvent -= new PlayingSongEventHandler(PlayingEvent);
			mediaServer.PausedSongEvent -= new PausedSongEventHandler(PausedEvent);
			mediaServer.StoppedSongEvent -= new StoppedSongEventHandler(StoppedEvent);
			mediaServer.ProgressEvent -= new ProgressEventHandler(ProgressEvent);
			mediaServer.PongEvent -= new PongEventHandler(PongEvent);
			mediaServer.VolumeEvent -= new VolumeEventHandler(VolumeEvent);

			// Queue Events
			mediaServer.AddedToQueueEvent -= new AddedToQueueEventHandler(AddedToQueueEvent);
			mediaServer.RemovedFromQueueEvent -= new RemovedFromQueueEventHandler(RemovedFromQueueEvent);
			mediaServer.MoveInQeuueEvent -= new MoveInQueueEventHandler(MovedInQueueEvent);

			// History Events
			mediaServer.AddToHistoryEvent -= new AddToHistoryEventHandler(AddedToHistoryEvent);
			mediaServer.RemoveFromHistoryEvent -= new RemoveFromHistoryEventHandler(RemovedFromHistoryEvent);

			// Advanced
			mediaServer.RateChanged -= new RateEventHandler(RateEvent);
			mediaServer.BalanceChanged -= new BalanceEventHandler(BalanceEvent);

			mediaServer.MediaCollectionReloadedEvent -= new MediaCollectionReloadedEventHandler(MediaCollectionReloadedEvent);
			mediaServer.MediaErrorEvent -= new MediaErrorEventHandler(MediaErrorEvent);
			mediaServer.MediaItemUpdateEvent -= new MediaItemUpdateEventHandler(MediaItemUpdate);

			mediaServer.MediaFileChanged -= new MediaFileChangedEventHandler(MediaFileChanged);
			mediaServer.ShutdownEvent -= new ShutdownEventHandler(ShutdownEvent);

			mediaServer = null;

			StopLocalPlayer();
		}

		[OneWay]
		public void LogServerEvent(object sender, LogEventArgs e) 
		{
			object[] eventArgs = new object[2];
			eventArgs[0] = (object) e.Function;
			eventArgs[1] = (object) e.Message;
			umPlayer.log.BeginInvoke(
				new Log.AddToLogDelegate(umPlayer.log.AddToLog), eventArgs);
		}

		[OneWay]
		public void PlayingEvent(object sender, MediaEventArgs e) 
		{
			if (player != null)
			{
				player.Play();
			}

			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e.MediaId;
			umPlayer.Invoke(
				new UMPlayer.PlayingDelegate(umPlayer.Playing), eventArgs);
		}

		[OneWay]
		public void PausedEvent(object sender, MediaEventArgs e) 
		{
			if (player != null)
			{
				player.Pause();
				player.CurrentPosition = mediaServer.CurrentPosition;
			}
			umPlayer.BeginInvoke(
				new UMPlayer.PausedDelegate(umPlayer.Paused));
		}

		[OneWay]
		public void StoppedEvent(object sender, MediaEventArgs e) 
		{
			if (player != null)
			{
				player.Stop();
			}
			umPlayer.BeginInvoke(
				new UMPlayer.StoppedDelegate(umPlayer.Stopped));
		}

		[OneWay]
		public void MediaFileChanged(object sender, MediaFileChangedEventArgs e)
		{
			if (player != null)
			{
				player.MediaFile = e.FullMediaFileName;
			}
		}

		#region Queue Events
		
		[OneWay]
		public void AddedToQueueEvent(object sender, QueueEventArgs e) 
		{
			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e;
			umPlayer.playerQueue.BeginInvoke(
				new PlayerQueue.AddToQueueDelegate(umPlayer.playerQueue.InvokeAddToQueue), eventArgs);
		}

		[OneWay]
		public void RemovedFromQueueEvent(object sender, QueueEventArgs e) 
		{
			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e;
			umPlayer.playerQueue.BeginInvoke(
				new PlayerQueue.RemoveFromQueueDelegate(umPlayer.playerQueue.InvokeRemoveFromQueue), eventArgs);
		}

		[OneWay]
		public void MovedInQueueEvent(object sender, QueueEventArgs e) 
		{
			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e;
			umPlayer.playerQueue.BeginInvoke(
				new PlayerQueue.MovedInQueueDelegate(umPlayer.playerQueue.InvokeMovedInQueue), eventArgs);
		}

		#endregion

		#region History Events

		[OneWay]
		public void AddedToHistoryEvent(object sender, HistoryEventArgs e)
		{
			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e;
			umPlayer.playerQueue.BeginInvoke(
				new History.AddToHistoryDelegate(umPlayer.history.InvokeAddToHistory), eventArgs);
		}

		[OneWay]
		public void RemovedFromHistoryEvent(object sender, HistoryEventArgs e)
		{
			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e;
			umPlayer.playerQueue.BeginInvoke(
				new History.RemoveFromHistoryDelegate(umPlayer.history.InvokeRemoveFromHistory), eventArgs);
		}


		#endregion

		#region Advanceed

		[OneWay]
		public void RateEvent(object sender, MediaRateEventArgs e) 
		{
			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e;
			umPlayer.advanced.BeginInvoke(
				new Advanced.RateChangedDelegate(umPlayer.advanced.InvokeRateChanged), eventArgs);
		}

		[OneWay]
		public void BalanceEvent(object sender, MediaBalanceEventArgs e) 
		{
			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e;
			umPlayer.advanced.BeginInvoke(
				new Advanced.BalanceChangedDelegate(umPlayer.advanced.InvokeBalanceChanged), eventArgs);
		}

		#endregion

		[OneWay]
		public void ProgressEvent(object sender, MediaProgressEventArgs e) 
		{
			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e.Position;
			umPlayer.BeginInvoke(
				new UMPlayer.ProgressDelegate(umPlayer.Progress), eventArgs);
		}

		[OneWay]
		public void PongEvent(object sender, EventArgs e) 
		{
			LogServerEvent(sender, new LogEventArgs("PONG!", ""));
		}

		[OneWay]
		public void VolumeEvent(object sender, MediaVolumeEventArgs e) 
		{
			if (player != null)
			{
				player.Volume = e.Volume;
			}

			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e.Volume;
			umPlayer.BeginInvoke(
				new UMPlayer.VolumeDelegate(umPlayer.Volume_Changed), eventArgs);
		}

		[OneWay]
		public void MediaCollectionReloadedEvent(object sender, EventArgs e) 
		{
			LoadMediaCollection();
		}

		[OneWay]
		public void MediaErrorEvent(object sender, MediaErrorEventArgs e) 
		{
			object[] eventArgs = new object[2];
			eventArgs[0] = (object) e.Message;
			eventArgs[1] = (object) e.MediaId;
			umPlayer.BeginInvoke(
				new UMPlayer.ShowErrorDelegate(umPlayer.ShowError), eventArgs);
		}

		[OneWay]
		public void MediaItemUpdate(object sender, MediaItemUpdateEventArgs e)
		{
			SqlConnection cn = new SqlConnection(ConnectionString);
			int mediaId = 0;

			// First get the row we will be using
			DataSetMedia.MediaRow row = null;
			cn.Open();
			switch (e.Type)
			{
				case MediaItemUpdateType.Add:
					LogServerEvent(sender, new LogEventArgs("MediaItemUpdate", "New media item added - " + e.MediaId.ToString()));
					DataSetMedia dsNewMedia = new DataSetMedia();
					SqlDataAdapter da = new SqlDataAdapter("select * from media where mediaid = " + e.MediaId.ToString(), cn);
					da.Fill(dsNewMedia, "Media");
					dsMedia.Merge(dsNewMedia);
					row = FindMediaRow(e.MediaId);
					break;
				case MediaItemUpdateType.Edit:
					DataSetMedia dsUpdatedMedia = new DataSetMedia();
					SqlDataAdapter daedit = new SqlDataAdapter("select * from media where mediaid = " + e.MediaId.ToString(), cn);
					daedit.Fill(dsUpdatedMedia, "Media");
					dsMedia.Merge(dsUpdatedMedia);
					row = FindMediaRow(e.MediaId);
					break;
				case MediaItemUpdateType.Delete:
					row = FindMediaRow(e.MediaId);
					mediaId = row.MediaId;
					row.Delete();
					break;
			}
			cn.Close();
			
			e.Entry = row;

			object[] eventArgs = new object[1];
			eventArgs[0] = (object) e;

			// Notify main form
			umPlayer.BeginInvoke(
				new UMPlayer.MediaItemUpdateDelegate(umPlayer.MediaItemUpdate), eventArgs);

			// Notify new songs form
			if (e.Type == MediaItemUpdateType.Add)
			{
				// notify new songs tab
				umPlayer.newMedia.BeginInvoke(
					new NewMedia.MediaItemAddedDelegate(umPlayer.newMedia.MediaItemAdded), eventArgs);
			}
			
		}

		public bool Logging 
		{
			get 
			{
				return logging; 
			}
			set 
			{
				if (value) 
				{
					if (!logging)
						mediaServer.LogEvent += new LogEventHandler(LogServerEvent);
					logging = value;
				}
				else 
				{
					if (logging)
						mediaServer.LogEvent -= new LogEventHandler(LogServerEvent);
					logging = value;
				}
			}
		}

		public DataSetMedia.MediaRow FindMediaRow(int mediaId) 
		{
			return dsMedia.Media.FindByMediaId(mediaId);
		}

		public void LoadMediaCollection() 
		{
			dsMedia.Clear();

			SqlConnection cn = new SqlConnection(ConnectionString);
			SqlDataAdapter da = new SqlDataAdapter("select * from Media where Deleted=0", cn);
			da.Fill(dsMedia, "Media");
		}

		public string ConnectionString
		{
			get { return connectionString; }
		}

		[OneWay]
		public void ShutdownEvent(object sender, EventArgs e)
		{
		}

	}

	public class ClientUtilities
	{
		public static void FadeIn(System.Windows.Forms.Form form)
		{
			form.Opacity = 0;
			form.Visible = true;

			for (int i = 0; i <= 5; i++)
			{
				form.Opacity = (i*20)/100.0;
				form.Refresh();
			}

		}

		public static void FadeOut(System.Windows.Forms.Form form)
		{
			// Bail if form already hidden
			if (!form.Visible)
				return;

			for (int i = 5; i >= 0; i--)
			{
				form.Opacity = (i*20)/100.0;
				form.Refresh();
			}

			form.Visible = false;

		}

	}
}
