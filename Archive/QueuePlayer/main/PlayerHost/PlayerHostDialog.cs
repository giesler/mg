using System;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using msn2.net.QueuePlayer;
using msn2.net.QueuePlayer.Shared;
using msn2.net.QueuePlayer.Server;

namespace msn2.net.QueuePlayer.SpeakerHost
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class PlayerHostDialog : System.Windows.Forms.Form
	{

		#region Declares

		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.ComponentModel.IContainer components;
		private string serverName = null;
		private PlayerClient playerClient = null;

		#endregion

		#region Constructor / Disposal

		public PlayerHostDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Connect to server set in XML
			serverName = ConfigurationSettings.AppSettings["server"];

			// Register server channel so when we create object it is created at server
			ChannelServices.RegisterChannel(new TcpChannel(0));
			RemotingConfiguration.RegisterWellKnownClientType(
				typeof(MediaServer),
				"tcp://" + serverName + ":777/RemotingMedia/MyMedia");

			playerClient = new PlayerClient();


		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Text = "notifyIcon1";
			this.notifyIcon1.Visible = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "E&xit Player Host";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// PlayerHostDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(248, 78);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1});
			this.Name = "PlayerHostDialog";
			this.Text = "Player Host";
			this.Load += new System.EventHandler(this.PlayerHostDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new PlayerHostDialog());
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			notifyIcon1.Visible = false;
			Application.Exit();
		}

		private void PlayerHostDialog_Load(object sender, System.EventArgs e)
		{
		}
	}

	public class PlayerClient: MarshalByRefObject, IDisposable
	{
		#region Declares

		private MediaServer server = null;
		private DirectXPlayer player = null;
		private string shareDirectory = "";

		#endregion

		#region Prevent idle disposal

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

		#endregion

		#region Constructor / Disposal

		public PlayerClient()
		{
			// Connect to server
			try
			{
				server						= new MediaServer();
				shareDirectory				= server.ShareDirectory;
			}
			catch (System.Net.Sockets.SocketException)
			{
				MessageBox.Show("Unable to connect to media server.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
				return;
			}

			// Create local DXPlayer
			player						= new DirectXPlayer();
			player.Volume				= server.Volume;
			player.Rate					= server.Rate;
			player.Balance				= server.Balance;

			// subscribe to events we care about
			server.MediaFileChanged		+= new MediaFileChangedEventHandler(MediaFileChangedEvent);
			server.PlayingSongEvent		+= new PlayingSongEventHandler(PlayingEvent);
			server.PausedSongEvent		+= new PausedSongEventHandler(PausedEvent);
			server.StoppedSongEvent		+= new StoppedSongEventHandler(StoppedEvent);
			server.VolumeEvent			+= new VolumeEventHandler(VolumeEvent);
			server.ShutdownEvent		+= new ShutdownEventHandler(ShutdownEvent);
			server.PreloadMediaEvent	+= new PreloadMediaEventHandler(PreloadMediaEvent);

			// Preload current queue
			foreach (MediaCollectionEntry entry in server.CurrentQueue())
			{
				player.PreloadMedia(shareDirectory + Path.DirectorySeparatorChar + entry.MediaFile);
			}

			// Start playing song if anything is loaded
			if (server.CurrentPlayState == MediaServer.PlayState.Playing)
			{
				player.MediaFile			= server.CurrentMediaFile;
				player.CurrentPosition		= server.CurrentPosition;
				player.Play();
			}
		}

		public void Dispose() 
		{
			server.MediaFileChanged		-= new MediaFileChangedEventHandler(MediaFileChangedEvent);
			server.PlayingSongEvent		-= new PlayingSongEventHandler(PlayingEvent);
			server.PausedSongEvent		-= new PausedSongEventHandler(PausedEvent);
			server.StoppedSongEvent		-= new StoppedSongEventHandler(StoppedEvent);
			server.VolumeEvent			-= new VolumeEventHandler(VolumeEvent);
			server.ShutdownEvent		-= new ShutdownEventHandler(ShutdownEvent);
			server.PreloadMediaEvent	-= new PreloadMediaEventHandler(PreloadMediaEvent);
		}

		#endregion

		#region Events

		[OneWay]
		public void MediaFileChangedEvent(object sender, MediaFileChangedEventArgs e)
		{
			player.MediaFile = e.FullMediaFileName;
		}

		[OneWay]
		public void PlayingEvent(object sender, MediaEventArgs e) 
		{
			player.Play();
		}

		[OneWay]
		public void PausedEvent(object sender, MediaEventArgs e) 
		{
			player.Pause();
		}

		[OneWay]
		public void StoppedEvent(object sender, MediaEventArgs e) 
		{
			player.Stop();
		}

		[OneWay]
		public void VolumeEvent(object sender, MediaVolumeEventArgs e)
		{
			player.Volume = e.Volume;
		}

		[OneWay]
		public void ShutdownEvent(object sender, EventArgs e)
		{
			Application.Exit();
		}

		[OneWay]
		public void PreloadMediaEvent(object sender, PreloadMediaEventArgs e)
		{
			player.PreloadMedia(e.Filename);
		}

		#endregion

	}
}
