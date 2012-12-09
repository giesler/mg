using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Messaging;
using UMServer;

namespace UMClient
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
		public string FileSharePath = @"\\sp\dfs\Music";

		// This override ensures that if the object is idle for an extended 
		// period, waiting for messages, it won't lose its lease. Without this 
		// override (or an alternative, such as implementation of a lease 
		// sponsor), an idle object that inherits from MarshalByRefObject 
		// may be collected even though references to it still exist.
		public override object InitializeLifetimeService() 
		{
			return null;
		}

		public Client(UMPlayer umPlayer)
		{
			this.umPlayer = umPlayer;

			// Load the list of media
			LoadMediaCollection();

			// Create a server
			mediaServer = new MediaServer();

			// Subscribe to events
			mediaServer.PlayingSongEvent += new PlayingSongEventHandler(PlayingEvent);
			mediaServer.PausedSongEvent += new PausedSongEventHandler(PausedEvent);
			mediaServer.StoppedSongEvent += new StoppedSongEventHandler(StoppedEvent);
			mediaServer.ProgressEvent += new ProgressEventHandler(ProgressEvent);
			mediaServer.PongEvent += new PongEventHandler(PongEvent);
			mediaServer.VolumeEvent += new VolumeEventHandler(VolumeEvent);
			mediaServer.RateChanged += new RateEventHandler(RateEvent);
			mediaServer.BalanceChanged += new BalanceEventHandler(BalanceEvent);

			mediaServer.AddedToQueueEvent += new AddedToQueueEventHandler(AddedToQueueEvent);
			mediaServer.RemovedFromQueueEvent += new RemovedFromQueueEventHandler(RemovedFromQueueEvent);
			mediaServer.MoveInQeuueEvent += new MoveInQueueEventHandler(MovedInQueueEvent);

			mediaServer.MediaCollectionReloadedEvent += new MediaCollectionReloadedEventHandler(MediaCollectionReloadedEvent);
			mediaServer.MediaErrorEvent += new MediaErrorEventHandler(MediaErrorEvent);

		}

		public void Dispose() 
		{
			if (logging)
				mediaServer.LogEvent -= new LogEventHandler(LogServerEvent);

			mediaServer.PlayingSongEvent -= new PlayingSongEventHandler(PlayingEvent);
			mediaServer.PausedSongEvent -= new PausedSongEventHandler(PausedEvent);
			mediaServer.StoppedSongEvent -= new StoppedSongEventHandler(StoppedEvent);
			mediaServer.ProgressEvent -= new ProgressEventHandler(ProgressEvent);
			mediaServer.PongEvent -= new PongEventHandler(PongEvent);
			mediaServer.VolumeEvent -= new VolumeEventHandler(VolumeEvent);

			mediaServer.AddedToQueueEvent -= new AddedToQueueEventHandler(AddedToQueueEvent);
			mediaServer.RemovedFromQueueEvent -= new RemovedFromQueueEventHandler(RemovedFromQueueEvent);
			mediaServer.MoveInQeuueEvent -= new MoveInQueueEventHandler(MovedInQueueEvent);

			mediaServer.MediaCollectionReloadedEvent -= new MediaCollectionReloadedEventHandler(MediaCollectionReloadedEvent);
			mediaServer.MediaErrorEvent -= new MediaErrorEventHandler(MediaErrorEvent);

		}

		[OneWay]
		public void LogServerEvent(object sender, LogEventArgs e) 
		{
			umPlayer.AddToLog( e.Function, e.Message );
		}

		[OneWay]
		public void PlayingEvent(object sender, MediaEventArgs e) 
		{
			umPlayer.Playing(e.MediaId);
		}

		[OneWay]
		public void PausedEvent(object sender, MediaEventArgs e) 
		{
			umPlayer.Paused();
		}

		[OneWay]
		public void StoppedEvent(object sender, MediaEventArgs e) 
		{
			umPlayer.Stopped();
		}

		[OneWay]
		public void ProgressEvent(object sender, MediaProgressEventArgs e) 
		{
			umPlayer.Progress(e.Progress);
		}

		[OneWay]
		public void PongEvent(object sender, EventArgs e) 
		{
			umPlayer.AddToLog( "PONG!", "" );
		}

		[OneWay]
		public void VolumeEvent(object sender, MediaVolumeEventArgs e) 
		{
			umPlayer.Volume_Changed(e.Volume);
		}

		[OneWay]
		public void RateEvent(object sender, MediaRateEventArgs e) 
		{
			umPlayer.Rate_Changed(e.Rate);
		}

		[OneWay]
		public void BalanceEvent(object sender, MediaBalanceEventArgs e) 
		{
			umPlayer.Balance_Changed(e.Balance);
		}

		[OneWay]
		public void AddedToQueueEvent(object sender, QueueEventArgs e) 
		{
			umPlayer.AddToQueue(e.MediaId, e.Position);
		}

		[OneWay]
		public void RemovedFromQueueEvent(object sender, QueueEventArgs e) 
		{
			umPlayer.RemoveFromQueue(e.MediaId, e.Position);
		}

		[OneWay]
		public void MovedInQueueEvent(object sender, QueueEventArgs e) 
		{
            umPlayer.MovedInQueue(e.MediaId, e.Position, e.NewPosition);
		}

		[OneWay]
		public void MediaCollectionReloadedEvent(object sender, EventArgs e) 
		{
			LoadMediaCollection();
		}

		[OneWay]
		public void MediaErrorEvent(object sender, MediaErrorEventArgs e) 
		{
            umPlayer.ShowError(e.Message, e.MediaId);
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

			SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
			SqlDataAdapter da = new SqlDataAdapter("select * from Media", cn);
			da.Fill(dsMedia, "Media");
		}

	}
}