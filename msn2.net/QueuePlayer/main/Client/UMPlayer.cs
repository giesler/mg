
#region using...
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Text;
using System.Diagnostics;
using Crownwood.Magic;
using Crownwood.Magic.Docking;
using msn2.net.QueuePlayer.Shared;
using msn2.net.QueuePlayer.Server;
using msn2.net.Common;
#endregion

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for UMPlayer.
	/// </summary>
	public class UMPlayer : msn2.net.Controls.ShellForm
	{

		#region Declares

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button buttonNext;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timerPaused;
		private System.Windows.Forms.Timer timerPing;
		private System.Windows.Forms.PictureBox pictureBoxVolume;
		private System.Windows.Forms.PictureBox pictureBoxVolumeContainer;
		public Client client;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.Button buttonPrevious;
		public System.Windows.Forms.ContextMenu contextMenuMediaList;
		public Log log;
		public MediaTooltip tooltip = new MediaTooltip();
		public PlayerQueue playerQueue = null;
		public History history = null;
		public Advanced advanced = null;
		public Playlists playlists = null;
		public NewMedia newMedia = null;
		private System.Windows.Forms.Panel panelTabs;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.Button buttonPlayPause;
		protected Crownwood.Magic.Docking.IDockingManager dockManager = null;
		private msn2.net.Controls.ProgressBar progressBarCurrent;
		private DataSetMedia.MediaRow currentMediaEntry = null;
		private msn2.net.Controls.ProgressBar titleProgressBar = null;
		private TitleBarButtons titleBarButtons = null;

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(UMPlayer));
			this.panel1 = new System.Windows.Forms.Panel();
			this.progressBarCurrent = new msn2.net.Controls.ProgressBar();
			this.panel2 = new System.Windows.Forms.Panel();
			this.buttonPrevious = new System.Windows.Forms.Button();
			this.pictureBoxVolume = new System.Windows.Forms.PictureBox();
			this.pictureBoxVolumeContainer = new System.Windows.Forms.PictureBox();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.buttonPlayPause = new System.Windows.Forms.Button();
			this.contextMenuMediaList = new System.Windows.Forms.ContextMenu();
			this.timerPaused = new System.Windows.Forms.Timer(this.components);
			this.timerPing = new System.Windows.Forms.Timer(this.components);
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.panelTabs = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// timerFadeOut
			// 
			this.timerFadeOut.Enabled = false;
			// 
			// timerFadeIn
			// 
			this.timerFadeIn.Enabled = false;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.progressBarCurrent,
																				 this.panel2});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(312, 48);
			this.panel1.TabIndex = 0;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// progressBarCurrent
			// 
			this.progressBarCurrent.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.progressBarCurrent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.progressBarCurrent.DisplayValues = true;
			this.progressBarCurrent.Location = new System.Drawing.Point(4, 36);
			this.progressBarCurrent.Maximum = 100;
			this.progressBarCurrent.Minimum = 0;
			this.progressBarCurrent.Name = "progressBarCurrent";
			this.progressBarCurrent.ProgressBackColor = System.Drawing.Color.Maroon;
			this.progressBarCurrent.RemainingBackColor = System.Drawing.Color.Gainsboro;
			this.progressBarCurrent.ShowProgressValue = true;
			this.progressBarCurrent.ShowRemainingValue = true;
			this.progressBarCurrent.Size = new System.Drawing.Size(160, 12);
			this.progressBarCurrent.TabIndex = 7;
			this.progressBarCurrent.Value = 0;
			this.progressBarCurrent.ChangeValueEvent += new msn2.net.Controls.ChangeValueDelegate(this.progressBarCurrent_ChangeValueEvent);
			// 
			// panel2
			// 
			this.panel2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.buttonPrevious,
																				 this.pictureBoxVolume,
																				 this.pictureBoxVolumeContainer,
																				 this.buttonNext,
																				 this.buttonStop,
																				 this.buttonPlayPause});
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.panel2.Location = new System.Drawing.Point(168, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(144, 48);
			this.panel2.TabIndex = 2;
			this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
			// 
			// buttonPrevious
			// 
			this.buttonPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPrevious.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonPrevious.Location = new System.Drawing.Point(8, 24);
			this.buttonPrevious.Name = "buttonPrevious";
			this.buttonPrevious.Size = new System.Drawing.Size(32, 20);
			this.buttonPrevious.TabIndex = 8;
			this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
			this.buttonPrevious.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonPrevious_Paint);
			// 
			// pictureBoxVolume
			// 
			this.pictureBoxVolume.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxVolume.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("pictureBoxVolume.BackgroundImage")));
			this.pictureBoxVolume.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBoxVolume.Location = new System.Drawing.Point(50, 10);
			this.pictureBoxVolume.Name = "pictureBoxVolume";
			this.pictureBoxVolume.Size = new System.Drawing.Size(56, 4);
			this.pictureBoxVolume.TabIndex = 6;
			this.pictureBoxVolume.TabStop = false;
			this.pictureBoxVolume.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxVolume_MouseDown);
			// 
			// pictureBoxVolumeContainer
			// 
			this.pictureBoxVolumeContainer.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxVolumeContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBoxVolumeContainer.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBoxVolumeContainer.Location = new System.Drawing.Point(48, 8);
			this.pictureBoxVolumeContainer.Name = "pictureBoxVolumeContainer";
			this.pictureBoxVolumeContainer.Size = new System.Drawing.Size(88, 8);
			this.pictureBoxVolumeContainer.TabIndex = 5;
			this.pictureBoxVolumeContainer.TabStop = false;
			this.pictureBoxVolumeContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxVolumeContainer_MouseDown);
			// 
			// buttonNext
			// 
			this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonNext.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonNext.Location = new System.Drawing.Point(104, 24);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(32, 20);
			this.buttonNext.TabIndex = 2;
			this.buttonNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonNext_MouseUp);
			this.buttonNext.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonNext_Paint);
			// 
			// buttonStop
			// 
			this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonStop.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonStop.Location = new System.Drawing.Point(72, 24);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(32, 20);
			this.buttonStop.TabIndex = 1;
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			this.buttonStop.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonStop_Paint);
			// 
			// buttonPlayPause
			// 
			this.buttonPlayPause.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlayPause.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonPlayPause.Location = new System.Drawing.Point(40, 24);
			this.buttonPlayPause.Name = "buttonPlayPause";
			this.buttonPlayPause.Size = new System.Drawing.Size(32, 20);
			this.buttonPlayPause.TabIndex = 0;
			this.buttonPlayPause.Click += new System.EventHandler(this.buttonPlayPause_Click);
			this.buttonPlayPause.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonPlayPause_Paint);
			// 
			// contextMenuMediaList
			// 
			this.contextMenuMediaList.Popup += new System.EventHandler(this.contextMenuMediaList_Popup);
			// 
			// timerPaused
			// 
			this.timerPaused.Interval = 500;
			this.timerPaused.Tick += new System.EventHandler(this.timerPaused_Tick);
			// 
			// timerPing
			// 
			this.timerPing.Interval = 60000;
			this.timerPing.Tick += new System.EventHandler(this.timerPing_Tick);
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Text = "notifyIcon1";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
			// 
			// panelTabs
			// 
			this.panelTabs.BackColor = System.Drawing.Color.Transparent;
			this.panelTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTabs.Location = new System.Drawing.Point(0, 48);
			this.panelTabs.Name = "panelTabs";
			this.panelTabs.Size = new System.Drawing.Size(312, 2);
			this.panelTabs.TabIndex = 2;
			// 
			// UMPlayer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(312, 50);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelTabs,
																		  this.panel1});
			this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "UMPlayer";
			this.Text = "Queue Player";
			this.TitleVisible = true;
			this.Load += new System.EventHandler(this.UMPlayer_Load);
			this.TitleMouseLeave += new System.EventHandler(this.UMPlayer_TitleMouseLeave);
			this.Closed += new System.EventHandler(this.UMPlayer_Closed);
			this.Rollup_Collapse += new System.EventHandler(this.UMPlayer_Rollup_Collapse);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.UMPlayer_Paint);
			this.TitleHover += new System.EventHandler(this.UMPlayer_TitleHover);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UMPlayer_KeyUp);
			this.Rollup_Expand += new System.EventHandler(this.UMPlayer_Rollup_Expand);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Constructor / Disposal

		/// <summary>
		/// Creates a new UMPlayer object
		/// </summary>
		public UMPlayer()
		{
			System.Threading.Thread.CurrentThread.Name = "UMPlayer Thread";

			InitializeComponent();
			log = new Log(this);

			dockManager = new Crownwood.Magic.Docking.DockingManagerIDE(this);
			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				if (client != null)
					client.Dispose();
			}
			base.Dispose( disposing );
		}

		#endregion

		#region InitialState

		/// <summary>
		/// Set up the player with initial values set and such
		/// </summary>
		public void InitialState() 
		{

			// Get the currently playing song (if there is one)
			int mediaId = client.mediaServer.CurrentMediaId;
			MediaServer.PlayState playState = client.mediaServer.CurrentPlayState;

			// If a song is playing, call the Playing function to init state
			if (mediaId != 0) 
			{
				if (playState == MediaServer.PlayState.Playing) 
				{
					Playing(mediaId);
				}
			}

			// Set the basic sliders and such
			Volume_Changed(client.mediaServer.Volume);

		}

		#endregion

		#region Load

		private void UMPlayer_Load(object sender, System.EventArgs e)
		{

			ConnectionDialog connection = new ConnectionDialog();

			//			if (connection.ShowDialog() == DialogResult.Cancel) 
			//			{
			//				Application.Exit();
			//				this.Dispose();
			//				return;
			//			}

			Status status = new Status("Connecting to server...");

			// Create the client
			client = new Client(this);

			// make sure we are connected
			while (!client.Connected)
			{
				string msg = String.Format("The server '{0}' is not responding.", connection.HostName);
				if (MessageBox.Show(msg, "Connection Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
				{
					this.Visible = false;
					Application.Exit();
					return;
				}
				client = new Client(this);
			}

			// Progress bar for title region
			titleProgressBar						= new msn2.net.Controls.ProgressBar();
			titleProgressBar.ProgressBackColor		= Color.LawnGreen;
			titleProgressBar.RemainingBackColor		= Color.LightGray;
			titleProgressBar.ShowProgressValue		= false;
			titleProgressBar.ShowRemainingValue		= false;
			titleProgressBar.ChangeValueEvent		+= new msn2.net.Controls.ChangeValueDelegate(this.progressBarCurrent_ChangeValueEvent);
			titleProgressBar.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
			this.AddTitleBarControl(titleProgressBar, 0, 14, 2, false);

			// Title bar buttons
			titleBarButtons						= new TitleBarButtons(this);
			this.AddButtons(titleBarButtons, titleBarButtons.Width, false);
			

			status.Message = "Loading songs...";
			InitialState();
			timerPing.Enabled = true;

			// see if we want to play locally
			if (connection.checkBoxPlayLocally.Checked)
			{
				client.StartLocalPlayer();
			}
			connection.Dispose();

			status.Hide();

			this.Visible = true;

			this.Location		= new Point(Screen.PrimaryScreen.WorkingArea.Width - 50 - this.Width, 50);
			this.TopMost		= true;
			TabForm tabForm		= new TabForm(this);
			tabForm.Location	= new Point(this.Left, this.Top + this.Height);
			tabForm.Size		= new Size(this.Width, this.Height * 3);
			tabForm.TopMost		= true;
            
			Crownwood.Magic.Controls.TabControl tab1 = new Crownwood.Magic.Controls.TabControl();
			tab1.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
			tab1.ShowClose  = false;
			tab1.Dock		= DockStyle.Fill;
			tab1.PositionTop = true;
			tab1.Font		 = new Font("Arial", 8);
			tabForm.panelTabs.Controls.Add(tab1);
			tab1.BringToFront();
			tabForm.Show();

			playerQueue = new PlayerQueue(this);
			playerQueue.Opacity = 0.7;
			Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage("Queue", playerQueue);
			tab1.TabPages.Add(page);

			history = new History(this);
			tab1.TabPages.Add(new Crownwood.Magic.Controls.TabPage("History", history));

			playlists = new Playlists(this);
			tab1.TabPages.Add(new Crownwood.Magic.Controls.TabPage("Playlists", playlists));

			Search search = new Search(this);
			tab1.TabPages.Add(new Crownwood.Magic.Controls.TabPage("Search", search));

			BrowseCollection browser = new BrowseCollection(this);
			tab1.TabPages.Add(new Crownwood.Magic.Controls.TabPage("Browse Collection", browser));

			newMedia = new NewMedia(this);
			tab1.TabPages.Add(new Crownwood.Magic.Controls.TabPage("New Media", newMedia));

			AddSongs addSongs = new AddSongs(this);
			tab1.TabPages.Add(new Crownwood.Magic.Controls.TabPage("Add Songs", addSongs));

			advanced = new Advanced(this);
			tab1.TabPages.Add(new Crownwood.Magic.Controls.TabPage("Advanced", advanced));

			tab1.TabPages.Add(new Crownwood.Magic.Controls.TabPage("Log", log));

			history.Visible = true;

		}

		#endregion

		#region Playing

		public delegate void PlayingDelegate(int mediaId);

		/// <summary>
		/// Sets up labels and such for the playing song
		/// </summary>
		/// <param name="mediaId">Current song</param>
		public void Playing(int mediaId) 
		{
			currentMediaEntry = client.FindMediaRow(mediaId);

			if (currentMediaEntry != null)
			{
				this.TitleVisible = true;
				panel1.Refresh();

				progressBarCurrent.Maximum  = Convert.ToInt32(currentMediaEntry.Duration);
				titleProgressBar.Maximum	= Convert.ToInt32(currentMediaEntry.Duration);

				if (RolledUp)
				{
					this.Text = currentMediaEntry.Name;
				}

				buttonPlayPause.Refresh();
				timerPaused.Enabled = false;

				titleBarButtons.Refresh();

				tooltip.labelName.Text = currentMediaEntry.Name;
				if (currentMediaEntry.IsArtistNull())
					tooltip.labelArtist.Text = "";
				else
					tooltip.labelArtist.Text = currentMediaEntry.Artist;
			

				if (currentMediaEntry.IsAlbumNull() || currentMediaEntry.Album.Length == 0)
				{
					tooltip.labelAlbum.Text = "";
					tooltip.Height = tooltip.labelAlbum.Top;
					tooltip.ResumeLayout(true);
				}
				else
				{
					tooltip.labelAlbum.Text = currentMediaEntry.Album;
					if (currentMediaEntry.IsTrackNull() || currentMediaEntry.Track == 0)
						tooltip.labelTrack.Text = "";
					else
						tooltip.labelTrack.Text = "Track " + currentMediaEntry.Track.ToString();
					tooltip.Height = tooltip.labelAlbum.Top + (tooltip.labelTrack.Height + tooltip.labelName.Top);
					tooltip.ResumeLayout(true);
				}
			}
			
			tooltip.PaintCalculated = false;
			

			ClearWaitForMessage();
		}

		#endregion

		#region Stopped

		public delegate void StoppedDelegate();

		/// <summary>
		/// Sets state to stopped
		/// </summary>
		public void Stopped() 
		{
			timerPaused.Enabled = false;
			this.TitleVisible = true;

			buttonPlayPause.Refresh();
			titleBarButtons.Refresh();

			ClearWaitForMessage();
		}

		#endregion

		#region Paused

		public delegate void PausedDelegate();

		/// <summary>
		/// Sets state to paused
		/// </summary>
		public void Paused() 
		{
			buttonPlayPause.Refresh();
			titleBarButtons.Refresh();
			
			timerPaused.Enabled = true;            

			ClearWaitForMessage();
		}

		#endregion

		#region Progress

		public delegate void ProgressDelegate(double progress);

		/// <summary>
		/// Sets the progress bar to the progress passed
		/// </summary>
		/// <param name="progress">Percentage of song completion</param>
		public void Progress(double position) 
		{
			if (this.WindowState != FormWindowState.Minimized)
			{
				progressBarCurrent.Value	= Convert.ToInt32(position);
				titleProgressBar.Value		= Convert.ToInt32(position);
			}
		}

		#endregion

		#region Volume

		public delegate void VolumeDelegate(double volume);

		/// <summary>
		/// Set the volume
		/// </summary>
		/// <param name="volume">Percentage of volume</param>
		public void Volume_Changed(double volume) 
		{
			pictureBoxVolume.Width = Convert.ToInt32(((volume) * (double) (pictureBoxVolumeContainer.Width-4)));
			ClearWaitForMessage();
		}

		#endregion

		#region ShowError

        public delegate void ShowErrorDelegate(string errorDescription, int mediaId);

		public void ShowError(string errorDescription, int mediaId) 
		{
			DataSetMedia.MediaRow row = client.FindMediaRow(mediaId);

			MediaError error;

			if (row != null)
				error = new MediaError(errorDescription, row.Name + " - " + row.Artist, row.MediaFile);
			else
				error = new MediaError(errorDescription, "unavailable", "unavailable");

			error.Show();

			log.AddToLog("Error", errorDescription, mediaId);
		}

		#endregion

		#region NewSearch

		public void NewSearch()
		{
			Search search = new Search(this);
			Crownwood.Magic.Docking.Content content = dockManager.CreateContent(search, "Search");
			search.Visible = true;
			dockManager.AddSingleContent(content, State.DockRight);
			content.SetState(State.Floating);
			search.Height = 225;
			search.Width  = 350;
		}

		public void NewSearch(string searchString)
		{
			Search search = new Search(this);
			search.Visible = true;
			search.Height = 225;
			search.Width  = 350;
		}

		#endregion

		private void timerPaused_Tick(object sender, System.EventArgs e)
		{
			if (this.RolledUp)
				this.TitleVisible = !this.TitleVisible;
		}

		#region Play

		public delegate void PlayDelegate();

		public void Play()
		{
			buttonPlayPause_Click(this, new EventArgs());
		}

		#endregion

		#region Pause

		public delegate void PauseDelegate();
		
		public void Pause() 
		{
			buttonPlayPause_Click(this, new EventArgs());
		}

		#endregion

		#region Stop

		public delegate void StopDelegate();

		public void Stop() 
		{
			buttonStop_Click(this, new EventArgs());
		}

		#endregion

		public void FillMenuFromQueue(ContextMenu menu, bool mediaBar)
		{
			foreach (MediaListViewItem item in playerQueue.mediaList.Items)
			{
				string text = item.Text;

				if (!item.Entry.IsArtistNull() && item.Entry.Artist.Length > 0)
				{
						text = text + " [" + item.Entry.Artist + "]";
				}
                MediaListItemMenuItem menuItem = null;
				menuItem = new MediaListItemMenuItem(text);
                menuItem.MediaRow = item.Entry;
				menu.MenuItems.Add(menuItem);
			}
		}

		public void Next() 
		{
			SetWaitForMessage();
			client.mediaServer.Next();
		}

		public void Previous()
		{
            SetWaitForMessage();
			buttonPrevious_Click(this, EventArgs.Empty);
		}

		private void timerPing_Tick(object sender, System.EventArgs e)
		{
			// make sure the connection stays alive
			log.AddToLog( "Ping?", "" );
			client.mediaServer.Ping();			

			double x = client.mediaServer.Duration;
			x++;
		}

		private void notifyIcon1_DoubleClick(object sender, System.EventArgs e)
		{
			this.Visible = true;
		}

		private void EditMediaInfo(int mediaId) 
		{

			DataSetMedia.MediaRow row = client.FindMediaRow(mediaId);

			MediaEditor editor = new MediaEditor(row);

			//editor.LoadMedia(row);
			//editorContent.SetState(State.DockRight);
			
			if (editor.ShowDialog() == DialogResult.OK) 
			{
				// set up db connection
				SqlConnection cn = new SqlConnection(client.ConnectionString);
				string sql = "update Media set Name = @Name, Artist = @Artist, Album = @Album, Track = @Track, Genre = @Genre, Comments = @Comments, MediaFile = @MediaFile where MediaID = @MediaID";
				SqlCommand cmd = new SqlCommand(sql, cn);

				cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 250, "Name");
				cmd.Parameters.Add("@Artist", SqlDbType.NVarChar, 250, "Artist");
				cmd.Parameters.Add("@Album", SqlDbType.NVarChar, 250, "Album");
				cmd.Parameters.Add("@Track", SqlDbType.Int, 4, "Track");
				cmd.Parameters.Add("@Genre", SqlDbType.NVarChar, 250, "Genre");
				cmd.Parameters.Add("@Comments", SqlDbType.NVarChar, 250, "Comments");
				cmd.Parameters.Add("@MediaID", SqlDbType.Int, 4, "MediaID");
				cmd.Parameters.Add("@MediaFile", SqlDbType.NVarChar, 500, "MediaFile");

				cmd.Parameters["@Name"].Value = row.Name;
				cmd.Parameters["@Artist"].Value = row.Artist;
				cmd.Parameters["@Album"].Value = row.Album;
				if (!row.IsTrackNull())
					cmd.Parameters["@Track"].Value = row.Track;
				else 
					cmd.Parameters["@Track"].Value = System.DBNull.Value;
				cmd.Parameters["@Genre"].Value = row.Genre;
				cmd.Parameters["@Comments"].Value = row.Comments;
				cmd.Parameters["@MediaID"].Value = row.MediaId;
				cmd.Parameters["@MediaFile"].Value = row.MediaFile;
                
				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();

				// now rename the file if needed
				RenameMediaFile(row);

				row.AcceptChanges();
				client.dsMedia.AcceptChanges();

				// now raise the event
				client.mediaServer.UpdateMediaItem(MediaItemUpdateType.Edit, row.MediaId);

			}
			

		}

		public void SetWaitForMessage() 
		{
			Cursor.Current = Cursors.WaitCursor;
		}

		public void ClearWaitForMessage() 
		{
			Cursor.Current = Cursors.Default;
		}

		#region RenameMediaFile

		private void RenameMediaFile(DataSetMedia.MediaRow entry)
		{
			SqlConnection cn = new SqlConnection(client.ConnectionString);
			string sql = "update Media set MediaFile = @MediaFile where MediaID = @MediaID";
			SqlCommand cmd = new SqlCommand(sql, cn);
			cmd.Parameters.Add("@MediaFile", SqlDbType.NVarChar, 1024);
			cmd.Parameters.Add("@MediaID", SqlDbType.Int);

			string root = client.mediaServer.ShareDirectory;
			
			cn.Open();
	
			// make sure file exists
			if (File.Exists(entry.MediaFile) && entry.MediaFile.IndexOf(root + @"\cd\") == -1 )
			{				
				// check if artist directory exists, if not create
				if (!Directory.Exists(root + Path.DirectorySeparatorChar + entry.Artist.Trim()))
				{
					Directory.CreateDirectory(root + Path.DirectorySeparatorChar + entry.Artist.Trim());
				}

				// Figure out the destination file name
				string dest = client.mediaServer.ShareDirectory + Path.DirectorySeparatorChar + MediaUtilities.NameMediaFile(entry);
				string sourcePath = entry.MediaFile.Substring(0, entry.MediaFile.LastIndexOf(Path.DirectorySeparatorChar));

				// Make sure dest file doesn't already exist
				if (!File.Exists(dest))
				{
					// If the names aren't the same, we want to move the file
					if (dest != entry.MediaFile)
					{
						// start a new sql transaction
						SqlTransaction trans = cn.BeginTransaction();
						cmd.Transaction = trans;
						bool success = false;

						try
						{

							cmd.Parameters["@MediaFile"].Value = dest;
							cmd.Parameters["@MediaID"].Value   = entry.MediaId;

							cmd.ExecuteNonQuery();

							File.Move(entry.MediaFile, dest);
							entry.MediaFile = dest;
							success = true;

						}
						catch (Exception ex)
						{
							ShowError(ex.ToString(), entry.MediaId);
						}
						finally 
						{
							// only save if we copied file
							if (success)
							{
								trans.Commit();
								entry.MediaFile = dest;
							}
							else
							{
								trans.Rollback();
							}
						}

						// If directory empty, delete it
						if (Directory.GetFiles(sourcePath).GetLength(0) == 0 
							&& Directory.GetDirectories(sourcePath).GetLength(0) == 0)
						{
                            Directory.Delete(sourcePath);
						}
					}
				}

			} // done checking file exists
    
			// cleanup
			cn.Close();

		}

		#endregion

        public event MediaItemClientUpdateEventHandler MediaItemClientUpdateEvent;		

		public delegate void MediaItemUpdateDelegate(MediaItemUpdateEventArgs e);

		public void MediaItemUpdate(MediaItemUpdateEventArgs e)
		{
            if (MediaItemClientUpdateEvent != null)
				MediaItemClientUpdateEvent(this, e);

			// Check if we are playing this song
			if (e.MediaId == client.mediaServer.CurrentMediaId)
			{
				Playing(e.MediaId);
			}
		}

		#region Context menu actions

		private void QueueAfterItem(object sender, ListView lv)
		{
			SetWaitForMessage();
			MenuItem menuItem = (MenuItem) sender;

			int offset = 1;
			foreach (MediaListViewItem item in lv.SelectedItems) 
			{
				client.mediaServer.AddToQueue(item.Entry.MediaId, menuItem.Index + offset);
				offset++;
			}

		}

		#endregion

		public void UMPlayer_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// Next
			if (e.KeyData == Keys.NumPad3)
			{
				client.mediaServer.Next();
				e.Handled = true;
			}
				// Pause
			else if (e.KeyData == Keys.NumPad5)
			{
				buttonPlayPause_Click(this, EventArgs.Empty);
				e.Handled = true;
			}
				// Volume Up
			else if (e.KeyData == Keys.NumPad8)
			{
				SetWaitForMessage();
				double newVolume = ((double)(pictureBoxVolume.Width + (pictureBoxVolume.Width/10)) / (double)(pictureBoxVolumeContainer.Width-4));
				client.mediaServer.Volume = newVolume;
				e.Handled = true;
			}
				// Volume down
			else if (e.KeyData == Keys.NumPad2) 
			{
				SetWaitForMessage();
				double newVolume1 = ((double)(pictureBoxVolume.Width - (pictureBoxVolume.Width/10)) / (double)(pictureBoxVolumeContainer.Width-4));
				client.mediaServer.Volume = newVolume1;
				e.Handled = true;
			}
				// Previous
			else if (e.KeyData == Keys.NumPad1)
			{
				SetWaitForMessage();
				client.mediaServer.Previous();
			}
				// Back 10 secs of song
			else if (e.KeyData == Keys.NumPad4)
			{
				SetWaitForMessage();
				client.mediaServer.MoveTimeByAmount(-10.0);				
			}
				// Forward 10 secs of song
			else if (e.KeyData == Keys.NumPad6)
			{
				SetWaitForMessage();
				client.mediaServer.MoveTimeByAmount(10.0);	
			}
		}

		private void buttonPrevious_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			client.mediaServer.Previous();
		}

		private void labelName_DoubleClick(object sender, System.EventArgs e)
		{
            EditMediaInfo(client.mediaServer.CurrentMediaId);		
		}

		#region Media List Context Menus

		private void contextMenuMediaList_Popup(object sender, System.EventArgs e)
		{
			ContextMenu contextMenu = (ContextMenu) sender;
			ListView lv = (ListView) contextMenu.SourceControl;

			// First clear anything already in menu
			contextMenuMediaList.MenuItems.Clear();

			// Area specific menu items first
			if (lv == playerQueue.mediaList.lv)
			{
				#region Queue only context menu items

				contextMenuMediaList.MenuItems.Add(new MediaListItemMenuItem("Move Up", lv, new EventHandler(playerQueue.buttonQueueUp_Click)));
				contextMenuMediaList.MenuItems.Add(new MediaListItemMenuItem("Move Down", lv, new EventHandler(playerQueue.buttonQueueDown_Click)));

				// Move To...
				MediaListItemMenuItem moveTo = new MediaListItemMenuItem("Move to...");
				contextMenuMediaList.MenuItems.Add(moveTo);

				// Top of queue
				moveTo.MenuItems.Add(
					new MediaListItemMenuItem("Top of queue", lv, new EventHandler(MediaQueue_MoveTop)));

				// Bottom of queue
				moveTo.MenuItems.Add(
					new MediaListItemMenuItem("Bottom of queue", lv, new EventHandler(MediaQueue_MoveBottom)));

				// After....
				MediaListItemMenuItem moveAfter = new MediaListItemMenuItem("After...");
				moveTo.MenuItems.Add(moveAfter);

				// Add queue entries to after list
				foreach (MediaListViewItem item in playerQueue.mediaList.Items)
				{
					string name = item.Entry.Name;
					if (!item.Entry.IsArtistNull() && item.Entry.Artist.Length > 0)
						name = name + " [" + item.Entry.Artist + "]";
					MediaListItemMenuItem menuItem = 
						new MediaListItemMenuItem(name, lv, new EventHandler(MediaQueue_MoveAfter), item.Entry);
					moveAfter.MenuItems.Add(menuItem);
				}

				contextMenuMediaList.MenuItems.Add(new MediaListItemMenuItem("Remove from queue", lv, new EventHandler(playerQueue.buttonQueueRemove_Click)));
				contextMenuMediaList.MenuItems.Add(new MediaListItemMenuItem("-"));
				contextMenuMediaList.MenuItems.Add(new MediaListItemMenuItem("Refresh", lv, new EventHandler(playerQueue.ReloadQueue)));
				contextMenuMediaList.MenuItems.Add(new MediaListItemMenuItem("Clear Queue", lv, new EventHandler(playerQueue.buttonClearQueue_Click)));
				contextMenuMediaList.MenuItems.Add(new MediaListItemMenuItem("-"));

				#endregion
			}
			else if (playlists != null && lv == playlists.mediaList.lv)
			{
				contextMenuMediaList.MenuItems.Add(new MenuItem("Remove from playlist", new EventHandler(playlists.buttonPlaylistMediaRemove_Click)));
				contextMenuMediaList.MenuItems.Add(new MenuItem("-"));
			}

			// PLAY NOW
			MediaListItemMenuItem playNow = 
				new MediaListItemMenuItem("&Play Now", lv, new EventHandler(MediaList_PlayNow));
			playNow.DefaultItem = true;
			contextMenuMediaList.MenuItems.Add(playNow);

			// ADD TO QUEUE
			MediaListItemMenuItem addToQueue = new MediaListItemMenuItem("Add to &queue");
            contextMenuMediaList.MenuItems.Add(addToQueue);

			// Top of queue
            MediaListItemMenuItem topOfQueue = 
				new MediaListItemMenuItem("Top of queue", lv, new EventHandler(MediaList_QueueTop));
			addToQueue.MenuItems.Add(topOfQueue);

			// Bottom of queue
            MediaListItemMenuItem bottomOfQueue = 
				new MediaListItemMenuItem("Bottom of queue", lv, new EventHandler(MediaList_QueueBottom));
			addToQueue.MenuItems.Add(bottomOfQueue);

			// After....
			MediaListItemMenuItem queueAfter = new MediaListItemMenuItem("After...");
			addToQueue.MenuItems.Add(queueAfter);

			// Add queue entries to after list
			foreach (MediaListViewItem item in playerQueue.mediaList.Items)
			{
				string name = item.Entry.Name;
				if (!item.Entry.IsArtistNull() && item.Entry.Artist.Length > 0)
                    name = name + " [" + item.Entry.Artist + "]";
				MediaListItemMenuItem menuItem = 
					new MediaListItemMenuItem(name, lv, new EventHandler(MediaList_QueueAfter), item.Entry);
				queueAfter.MenuItems.Add(menuItem);
			}

			// ADD TO PLAYLIST
            MediaListItemMenuItem addToPlaylist = new MediaListItemMenuItem("Add to &playlist");
			contextMenuMediaList.MenuItems.Add(addToPlaylist);

			// Add playlist entries to menu
			foreach (PlaylistListViewItem item in playlists.listViewPlaylists.Items)
			{
				MediaListItemMenuItem playlistMenuItem =
					new MediaListItemMenuItem(item.Entry.PlaylistName);
                addToPlaylist.MenuItems.Add(playlistMenuItem);
                	
				// Add top of playlist item
				MediaListItemMenuItem topOfPlaylist = 
					new MediaListItemMenuItem("Top of playlist", lv, new EventHandler(MediaList_AddToPlaylist_Top), item.Entry);
				playlistMenuItem.MenuItems.Add(topOfPlaylist);

				// Add bottom of playlist item
				MediaListItemMenuItem bottomOfPlaylist = 
					new MediaListItemMenuItem("Bottom of playlist", lv, new EventHandler(MediaList_AddToPlaylist_Bottom), item.Entry);
				playlistMenuItem.MenuItems.Add(bottomOfPlaylist);

				// Add an 'after' item that is loaded on demand
                MediaListItemMenuItem playlistAfter =
					new MediaListItemMenuItem("After", lv, null, item.Entry);
				playlistAfter.Popup += new EventHandler(MediaList_AddToPlaylist_After_Popup);
				playlistMenuItem.MenuItems.Add(playlistAfter);

				playlistAfter.MenuItems.Add(new MediaListItemMenuItem("Loading..."));
			}

			// New
			MediaListItemMenuItem playlistNew = 
				new MediaListItemMenuItem("New...", lv, new EventHandler(MediaList_AddToPlaylist_New));
			addToPlaylist.MenuItems.Add(playlistNew);

			// BLANK
			MenuItem blankItem = new MediaListItemMenuItem("-");
			contextMenuMediaList.MenuItems.Add(blankItem);

			// EDIT
			MediaListItemMenuItem editMedia = 
				new MediaListItemMenuItem("&Edit", lv, new EventHandler(MediaList_Edit));
			contextMenuMediaList.MenuItems.Add(editMedia);

			// DELETE
			MediaListItemMenuItem deleteMedia = 
				new MediaListItemMenuItem("&Delete", lv, new EventHandler(MediaList_Delete));
			contextMenuMediaList.MenuItems.Add(deleteMedia);

		}

		private void MediaList_PlayNow(object sender, System.EventArgs e)
		{
			// Figure out the listview
            MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			// Play the first selected item
			if (lv.SelectedItems.Count >= 1)
			{
				SetWaitForMessage();
				MediaListViewItem item = (MediaListViewItem) lv.SelectedItems[0];
				client.mediaServer.PlayMediaId(item.Entry.MediaId);
			}
		}

		private void MediaQueue_MoveTop(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			SetWaitForMessage();

			// Loop through selected items
			foreach (MediaListViewItem item in new IterReverse(lv.SelectedItems)) 
			{
				// Move to top of queue
				client.mediaServer.MoveInQueue(item.Entry.MediaId, item.Guid, 0);
			}
		}

		private void MediaList_QueueTop(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			SetWaitForMessage();

			// Loop through selected items
			foreach (MediaListViewItem item in new IterReverse(lv.SelectedItems)) 
			{
				// Add to top of queue
				client.mediaServer.AddToQueue(item.Entry.MediaId, 0);
			}
		}

		private void MediaQueue_MoveBottom(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			SetWaitForMessage();

			// Loop through selected items
			foreach (MediaListViewItem item in new IterIsolate(lv.SelectedItems)) 
			{
				// Move to bottom of queue
				client.mediaServer.MoveInQueue(item.Entry.MediaId, item.Guid, lv.Items.Count-1);
			}
		}

		private void MediaList_QueueBottom(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			SetWaitForMessage();

			// Loop through selected items
			foreach (MediaListViewItem item in lv.SelectedItems) 
			{
				// Add items in order to bottom of queue
				client.mediaServer.AddToQueue(item.Entry.MediaId);
			}
		}

		private void MediaQueue_MoveAfter(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			int offset = 1;
			foreach (MediaListViewItem item in new IterIsolate(lv.SelectedItems)) 
			{
				// Move to position in queue
				client.mediaServer.MoveInQueue(item.Entry.MediaId, item.Guid, menuItem.Index + offset);
				offset++;
			}
		}

		private void MediaList_QueueAfter(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			int offset = 1;
			foreach (MediaListViewItem item in new IterIsolate(lv.SelectedItems)) 
			{
				client.mediaServer.AddToQueue(item.Entry.MediaId, menuItem.Index + offset);
				offset++;
			}
		}

		private void MediaList_Edit(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			// Edit the first selected item
			if (lv.SelectedItems.Count >= 1)
			{
				MediaListViewItem item = (MediaListViewItem) lv.SelectedItems[0];
				EditMediaInfo(item.Entry.MediaId);
			}
		}

		private void MediaList_Delete(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			string msg = "";

			if (lv.SelectedItems.Count == 1) 
			{
				ListViewItem item = lv.SelectedItems[0];
				msg = "Are you sure you want to delete the entry '" + item.Text + "'?  You will not be able to recover it.";
			} 
			else 
			{
				msg = "Are you sure you want to delete the " + lv.SelectedItems.Count + " selected items?  You will not be able to recover them.";
			}

			string fileShare = client.mediaServer.ShareDirectory + Path.DirectorySeparatorChar;

			if (MessageBox.Show(msg, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) 
			{
				SqlConnection cn = new SqlConnection(client.ConnectionString);
				SqlCommand cmd = new SqlCommand("dbo.s_DeleteMediaItem", cn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@MediaID", SqlDbType.Int);

				cn.Open();
				foreach (MediaListViewItem item in new IterIsolate(lv.SelectedItems)) 
				{
					if (File.Exists(fileShare + item.Entry.MediaFile))
					{
						while (true)
						{
							try 
							{
								File.Delete(fileShare + item.Entry.MediaFile);
								break;
							}
							catch (Exception)
							{
                                msg = String.Format("The file {0} is currently in use and cannot be deleted.", fileShare + item.Entry.MediaFile);
								DialogResult response = MessageBox.Show(msg, "Delete", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
								if (response == DialogResult.Cancel)
								{
									cn.Close();
									return;
								}
							}
						}
					}
					
					cmd.Parameters["@MediaID"].Value = item.Entry.MediaId;
					cmd.ExecuteNonQuery();

					client.mediaServer.UpdateMediaItem(MediaItemUpdateType.Delete, item.Entry.MediaId);
				}
				cn.Close();

			}
		}

		private void MediaList_AddToPlaylist_Top(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			MediaList_AddToPlaylist(menuItem.PlaylistRow, lv, 0);
		}

		private void MediaList_AddToPlaylist_Bottom(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			MediaList_AddToPlaylist(menuItem.PlaylistRow, lv, 10000);
		}

		private void MediaList_AddToPlaylist_After_Popup(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			menuItem.MenuItems.Clear();

            SqlConnection cn = new SqlConnection(client.ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_PlaylistMedia_List", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PlaylistId", menuItem.PlaylistRow.PlaylistId);

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				DataSetMedia.MediaRow row = client.FindMediaRow(Convert.ToInt32(dr["MediaId"]));
				MediaListItemMenuItem item =
					new MediaListItemMenuItem(row.Name, lv, new EventHandler(MediaList_AddToPlaylist_After), menuItem.PlaylistRow);
				menuItem.MenuItems.Add(item);
			}
			dr.Close();
			cn.Close();

		}

		private void MediaList_AddToPlaylist_After(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			MediaList_AddToPlaylist(menuItem.PlaylistRow, lv, menuItem.Index+1);
		}

		private void MediaList_AddToPlaylist_New(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			PlaylistEditor ed = new PlaylistEditor();

			// If OK, then add new playlist
			if (ed.ShowDialog(this) == DialogResult.OK)
			{
				SqlConnection cn = new SqlConnection(client.ConnectionString);
				SqlCommand cmd	 = new SqlCommand("s_Playlist_Add", cn);
				cmd.CommandType  = CommandType.StoredProcedure;
				cmd.Parameters.Add("@PlaylistName", SqlDbType.NVarChar, 100);
				cmd.Parameters.Add("@PlaylistId", SqlDbType.Int);
				cmd.Parameters["@PlaylistId"].Direction = ParameterDirection.Output;

				cmd.Parameters["@PlaylistName"].Value = ed.PlaylistName;

				cn.Open();
				cmd.ExecuteNonQuery();
				int playlistId = Convert.ToInt32(cmd.Parameters["@PlaylistId"].Value);
				cn.Close();

				playlists.LoadPlaylists();

				// now find the added item playlist
				foreach (PlaylistListViewItem item in playlists.listViewPlaylists.Items)
				{
					if (item.Entry.PlaylistId == playlistId)
					{
						MediaList_AddToPlaylist(item.Entry, lv, 0);
					}
				}
			}

		}

		private void MediaList_AddToPlaylist(DataSetPlaylist.PlaylistRow playlistEntry, ListView lv, int insertPosition)
		{
			SqlConnection cn = new SqlConnection(client.ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_PlaylistMedia_Add", cn);
			cmd.CommandType	 = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PlaylistId", SqlDbType.Int);
			cmd.Parameters.Add("@MediaId", SqlDbType.Int);
			cmd.Parameters.Add("@Position", SqlDbType.Int);

			cmd.Parameters["@PlaylistId"].Value = playlistEntry.PlaylistId;

			cn.Open();

			int position = insertPosition;
			foreach (MediaListViewItem item in lv.SelectedItems) 
			{
				cmd.Parameters["@MediaId"].Value  = item.Entry.MediaId;
				cmd.Parameters["@Position"].Value = position;

				cmd.ExecuteNonQuery();

				position++;
			}

			cn.Close();

			// Save any playlist selection
			int selectedPlaylist = -1;
			if (playlists.listViewPlaylists.SelectedItems.Count > 0)
			{
				selectedPlaylist = playlists.listViewPlaylists.SelectedIndices[0];
			}
			playlists.LoadPlaylists();

			if (selectedPlaylist > 0)
			{
				playlists.listViewPlaylists.Items[selectedPlaylist].Selected = true;
			}

			// Check if we need to reload playlist
			if (playlists.listViewPlaylists.SelectedItems.Count > 0)
			{
				PlaylistListViewItem item = (PlaylistListViewItem) playlists.listViewPlaylists.SelectedItems[0];
				if (item.Entry.PlaylistId == playlistEntry.PlaylistId)
					playlists.LoadPlaylistMedia(item.Entry.PlaylistId);
			}

		}

		#endregion

		private void buttonNext_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				SetWaitForMessage();
				client.mediaServer.Next();
			}
			else if (e.Button == MouseButtons.Right)
			{
				SetWaitForMessage();
				ContextMenu contextMenuQuickList = new ContextMenu();

				// get the queue from the server
				FillMenuFromQueue(contextMenuQuickList, false);

				foreach (MenuItem item in contextMenuQuickList.MenuItems)
				{
					item.Click += new EventHandler(AdvanceToSong);
				}

				contextMenuQuickList.Show(buttonNext, buttonNext.PointToClient(MousePosition));
			}
		}

		public void AdvanceToSong(object sender, System.EventArgs e)
		{
			MediaListItemMenuItem item = (MediaListItemMenuItem) sender;
			client.mediaServer.AdvanceToSong(item.MediaRow.MediaId);
		}

		private void pictureBoxVolume_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pictureBoxVolumeContainer_MouseDown(sender, e);
		}

		private void pictureBoxVolumeContainer_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			SetWaitForMessage();
			double newVolume = ((double)(e.X) / (double)(pictureBoxVolumeContainer.Width-4));
			client.mediaServer.Volume = newVolume;
		}

		private void labelName_MouseHover(object sender, System.EventArgs e)
		{
			this.SuspendOpactiyChanges = true;
			tooltip.Left = MousePosition.X + 10;
			tooltip.Top  = MousePosition.Y + 10;
			tooltip.ResumeLayout(true);
			tooltip.Refresh();
			tooltip.Show();
		}

		private void labelName_MouseLeave(object sender, System.EventArgs e)
		{
            tooltip.Hide();		
			this.SuspendOpactiyChanges = false;
		}

		public void ResizeMediaListView(ListView lv)
		{
			lv.Columns[0].Width = (int)((lv.Width-22) * 0.30);
			lv.Columns[1].Width = (int)((lv.Width-22) * 0.25);
			lv.Columns[2].Width = (int)((lv.Width-22) * 0.25);
			lv.Columns[3].Width = (int)((lv.Width-22) * 0.10);
			lv.Columns[4].Width = (int)((lv.Width-22) * 0.10);
		}

		private void UMPlayer_Closed(object sender, System.EventArgs e)
		{
			if (client != null)
			{
				client.Disconnect();
				client = null;
			}
		}

		private void buttonStop_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			client.mediaServer.Stop();
		}

		private void buttonPlayPause_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			if (client.mediaServer.CurrentPlayState != MediaServer.PlayState.Playing) 
			{
				client.mediaServer.Play();
			} 
			else 
			{
				client.mediaServer.Pause();
			}

		}

		#region Main form button painting functions

		private void buttonPlayPause_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			ShadeRegion(e, Color.LightGray);

			int width	= 8;
			int height  = 8;
			int x		= buttonPlayPause.Width / 2 - (width / 2);
			int y		= buttonPlayPause.Height / 2 - (height / 2);
			int offset	= 2;

			if (client.mediaServer.CurrentPlayState == MediaServer.PlayState.Playing)
			{
				// draw pause
				Point pt1 = new Point(x + width / 2 - offset, y);
				Point pt2 = new Point(x + width / 2 - offset, y + height);

				Point pt3 = new Point(x + width / 2 + offset, y);
				Point pt4 = new Point(x + width / 2 + offset, y + height);

				using (Pen pen = new Pen(new SolidBrush(Color.DimGray), 2))
				{
					e.Graphics.DrawLine(pen, pt1, pt2);
					e.Graphics.DrawLine(pen, pt3, pt4);
				}
			}
			else
			{
				// draw play
				Point[] points = new Point[4];

				points[0] = new Point(x, y);						// upper left
				points[1] = new Point(x + width, y + height /2);	// center right
				points[2] = new Point(x, y + height);				// lower left
				points[3] = new Point(x, y);						// upper left again

				e.Graphics.FillPolygon(new SolidBrush(Color.DimGray), points, System.Drawing.Drawing2D.FillMode.Alternate);
			}
		}

		private void buttonStop_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			ShadeRegion(e, Color.LightGray);

			int width	= 8;
			int x		= buttonStop.Width / 2 - (width / 2);
			int y		= buttonStop.Height / 2 - (width / 2);

			e.Graphics.FillRectangle(new SolidBrush(Color.DimGray), x, y, width, width);
		}

		private void buttonNext_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			ShadeRegion(e, Color.LightGray);

			int width	= 8;
			int height  = 8;
			int x		= buttonPlayPause.Width / 2 - (width / 2);
			int y		= buttonPlayPause.Height / 2 - (height / 2);
			int offset	= 7;

			// draw play
			Point[] points = new Point[4];

			points[0] = new Point(x - offset/2, y);						// upper left
			points[1] = new Point(x + width - offset/2, y + height /2);	// center right
			points[2] = new Point(x - offset/2, y + height);				// lower left
			points[3] = new Point(x - offset/2, y);						// upper left again

			e.Graphics.FillPolygon(new SolidBrush(Color.DimGray), points, System.Drawing.Drawing2D.FillMode.Alternate);

			points[0] = new Point(x + offset/2, y);						// upper left
			points[1] = new Point(x + width + offset/2, y + height /2);	// center right
			points[2] = new Point(x + offset/2, y + height);				// lower left
			points[3] = new Point(x + offset/2, y);						// upper left again

			e.Graphics.FillPolygon(new SolidBrush(Color.DimGray), points, System.Drawing.Drawing2D.FillMode.Alternate);

		}



		private void buttonPrevious_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            ShadeRegion(e, Color.LightGray);
			
			int width	= 8;
			int height  = 8;
			int x		= buttonPlayPause.Width / 2 - (width / 2);
			int y		= buttonPlayPause.Height / 2 - (height / 2);
			int offset	= 7;

			// draw play
			Point[] points = new Point[4];

			points[0] = new Point(x + width + offset/2, y);						// upper left
			points[1] = new Point(x + width + offset/2, y + height);	// center right
			points[2] = new Point(x + offset/2, y + height / 2);				// lower left
			points[3] = new Point(x + width + offset/2, y);						// upper left again

			e.Graphics.FillPolygon(new SolidBrush(Color.DimGray), points, System.Drawing.Drawing2D.FillMode.Alternate);

			points[0] = new Point(x + width - offset/2, y);						// upper left
			points[1] = new Point(x + width - offset/2, y + height);	// center right
			points[2] = new Point(x - offset/2, y + height / 2);				// lower left
			points[3] = new Point(x + width - offset/2, y);						// upper left again

			e.Graphics.FillPolygon(new SolidBrush(Color.DimGray), points, System.Drawing.Drawing2D.FillMode.Alternate);

		}

		#endregion

		private void progressBarCurrent_ChangeValueEvent(object sender, msn2.net.Controls.ChangeValueEventArgs e)
		{
			SetWaitForMessage();
            client.mediaServer.CurrentPosition = Convert.ToDouble(e.Value);		
		}

		private void UMPlayer_Rollup_Collapse(object sender, System.EventArgs e)
		{
			titleBarButtons.Visible		= true;
			titleProgressBar.Visible	= true;
			this.MoreButtonsWidth		= titleBarButtons.Width;

			if (currentMediaEntry == null)
				this.Text = "Queue Player";
			else
				this.Text = currentMediaEntry.Name;
		}

		private void UMPlayer_Rollup_Expand(object sender, System.EventArgs e)
		{
			titleBarButtons.Visible		= false;
			titleProgressBar.Visible	= false;
			this.MoreButtonsWidth		= 0;

			this.Text = "Queue Player";

			this.Refresh();
		}

		private void UMPlayer_TitleHover(object sender, System.EventArgs e)
		{
			// bail if currently moving form
			if (this.Moving)
				return;

			if (this.RolledUp)
			{
				this.SuspendOpactiyChanges = true;
				tooltip.Left = MousePosition.X + 10;
				tooltip.Top  = MousePosition.Y + 10;
				tooltip.ResumeLayout(true);
				tooltip.Refresh();
				tooltip.Show();
			}
		}

		private void UMPlayer_TitleMouseLeave(object sender, System.EventArgs e)
		{
			tooltip.Hide();
			this.SuspendOpactiyChanges = false;
		}

		private void UMPlayer_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			ShadeRegion(e, Color.LightGray, Color.Gray);
		}

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			ShadeRegion(e, Color.LightGray, Color.Gray);

			if (currentMediaEntry == null)
				return;

			// ARTIST
			e.Graphics.DrawString(currentMediaEntry.Artist, new Font("Arial", 8), new SolidBrush(Color.Black), 2, 2);

			// NAME
			SizeF sizeF		= new SizeF();
			Font nameFont	= new Font("Arial", 10, FontStyle.Bold);
			string temp		= currentMediaEntry.Name;
			sizeF			= e.Graphics.MeasureString(temp, nameFont);
			if (sizeF.Width > e.ClipRectangle.Width)
				temp = temp + "...";

			while (sizeF.Width > e.ClipRectangle.Width)
			{
				if (temp.Length > 5)
				{
					temp = temp.Substring(0, temp.Length-5) + "...";
				}
				else
				{
					break;
				}
				sizeF = e.Graphics.MeasureString(temp, nameFont);
			}
			e.Graphics.DrawString(temp, nameFont, new SolidBrush(Color.Black), 2, 14);

		}

		private void panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			ShadeRegion(e, Color.LightGray, Color.Gray);

			// Volume label
			e.Graphics.DrawString("Volume:", new Font("Arial", 8), new SolidBrush(Color.Black), 2, 4);
		}


	}

	#region Media Item client updates

	public delegate void MediaItemClientUpdateEventHandler(object sender, MediaItemUpdateEventArgs e);

	public class MediaItemClientUpdateEventArgs: EventArgs
	{
		private int mediaId;
		private MediaItemUpdateType type;
		private DataSetMedia.MediaRow entry;

		public MediaItemClientUpdateEventArgs(MediaItemUpdateType type, int mediaId)
		{
            this.type	 = type;
			this.mediaId = mediaId;
		}

		/// <summary>
		/// Create new media update event args
		/// </summary>
		/// <param name="type">Type of update</param>
		/// <param name="mediaId">Update media ID</param>
		public MediaItemClientUpdateEventArgs(MediaItemUpdateType type, DataSetMedia.MediaRow entry)
		{
			this.type    = type;
			this.entry   = entry;
			this.mediaId = entry.MediaId;
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

		public DataSetMedia.MediaRow Entry
		{
			get { return entry; }
			set { entry = value; }
		}

	}

	#endregion
    
	#region Additional classes

	internal class MediaListItemMenuItem: MenuItem //Crownwood.Magic.Menus.MenuItemIDE
	{
		private ListView lv = null;
		private DataSetMedia.MediaRow mediaRow = null;
        private DataSetPlaylist.PlaylistRow playlistRow = null;		
		private Font font = null;
		
		public MediaListItemMenuItem(string text) : this(text, null, null)
		{}

		public MediaListItemMenuItem(string text, ListView lv) : this(text, lv, null)
		{}

		public MediaListItemMenuItem(string text, ListView lv, EventHandler onClick) : base()
		{
			this.Text = text;
			this.lv = lv;
			this.Visible = true;
			if (onClick != null)
                this.Click += onClick;

			this.font = new Font("Arial", 8);
//			this.OwnerDraw = true;
		}

		public MediaListItemMenuItem(string text, ListView lv, EventHandler onClick, DataSetMedia.MediaRow mediaRow) : this(text, lv, onClick)
		{
			this.mediaRow = mediaRow;
		}
        
		public MediaListItemMenuItem(string text, ListView lv, EventHandler onClick, DataSetPlaylist.PlaylistRow playlistRow) : this(text, lv, onClick)
		{
			this.playlistRow = playlistRow;
		}

		public ListView ListView
		{
			get { return lv; }
		}

		public DataSetMedia.MediaRow MediaRow
		{
			get { return mediaRow; }
			set { mediaRow = value; }
		}

		public DataSetPlaylist.PlaylistRow PlaylistRow
		{
			get { return playlistRow; }
		}

	}


	internal class PlaylistListViewItem: ListViewItem
	{
		private DataSetPlaylist.PlaylistRow entry;
		private UMPlayer player;

		public PlaylistListViewItem(UMPlayer player, DataSetPlaylist.PlaylistRow entry) : base()
		{
            this.entry	= entry;                        			
			this.player	= player;

            // Set the sub items
			this.Text = entry.PlaylistName;
			SubItems.Add(entry.PlaylistCreated.ToShortDateString());
			SubItems.Add(entry.PlaylistUpdated.ToShortDateString());
			SubItems.Add(entry.TotalSongs.ToString());
			if (entry.IsTotalDurationNull())
				SubItems.Add("0");
			else
				SubItems.Add(Utilities.DurationToString(entry.TotalDuration));
            
		}

		public DataSetPlaylist.PlaylistRow Entry
		{
			get { return entry; }
		}

	}

	internal class PlaylistMediaListViewItem: MediaListViewItem
	{
		private int playlistMediaId = 0;

		public PlaylistMediaListViewItem(UMPlayer player, DataSetMedia.MediaRow entry, int playlistMediaId) : base(player, entry)
		{
            this.playlistMediaId = playlistMediaId;
		}

		public int PlaylistMediaId
		{
			get { return playlistMediaId; }
		}
	}

	public class MediaListViewItem: ListViewItem 
	{
		private DataSetMedia.MediaRow entry;
		private UMPlayer player;
		private Guid guid;
		private int mediaId;

		#region Constructors

		/// <summary>
		/// List view item constructor for Media item
		/// </summary>
		/// <param name="player">Player to subscribe to for updates</param>
		/// <param name="entry">New entry</param>
		public MediaListViewItem(UMPlayer player, DataSetMedia.MediaRow entry) : this(player, entry, Guid.Empty)
		{}

		/// <summary>
		/// List view item constructor for Media item
		/// </summary>
		/// <param name="player">Player to subscribe to for updates</param>
		/// <param name="entry">New entry</param>
		/// <param name="guid">Unique ID</param>
		public MediaListViewItem(UMPlayer player, DataSetMedia.MediaRow entry, Guid guid) : base()
		{
			this.entry   = entry;
			this.player  = player;
			this.guid    = guid;
			this.mediaId = entry.MediaId;

			// Set the sub items
			this.Text = entry.Name;
			SubItems.Add(entry.Artist);
			SubItems.Add(entry.Album);
			if (entry.IsTrackNull())
				SubItems.Add(String.Empty);
			else if (entry.Track != 0)
				SubItems.Add(entry.Track.ToString());
			else
				SubItems.Add(String.Empty);
			SubItems.Add(Utilities.DurationToString(entry.Duration));

			System.Windows.Forms.ListViewItem.ListViewSubItem delItem =
				new System.Windows.Forms.ListViewItem.ListViewSubItem();
			delItem.Text	  = "x";
			delItem.ForeColor = Color.Navy;
			delItem.Font	  = new Font("Arial", 8, FontStyle.Underline);
			SubItems.Add(delItem);

			// subscribe to update events
			player.MediaItemClientUpdateEvent += new MediaItemClientUpdateEventHandler(MediaItemUpdateEvent);

		}

		#endregion

		public void Dispose() 
		{
			player.MediaItemClientUpdateEvent -= new MediaItemClientUpdateEventHandler(MediaItemUpdateEvent);
		}

		#region Properties
        
		public DataSetMedia.MediaRow Entry 
		{
			get { return entry; }
		}

		public Guid Guid
		{
			get { return guid; }
		}

		#endregion
        
		/// <summary>
		/// Called by client when an item has been updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void MediaItemUpdateEvent(object sender, MediaItemUpdateEventArgs e)
		{
			// check if this item is the current item
			if (e.MediaId == mediaId)
			{
				if (e.Type == MediaItemUpdateType.Edit)
				{
					SubItems[0].Text = e.Entry.Name;
					SubItems[1].Text = e.Entry.Artist;
					if (!e.Entry.IsAlbumNull())
						SubItems[2].Text = e.Entry.Album;
					if (!e.Entry.IsTrackNull() && entry.Track != 0)
						SubItems[3].Text = e.Entry.Track.ToString();
					if (!e.Entry.IsDurationNull() && entry.Duration > 0)
						SubItems[4].Text = ((int)(e.Entry.Duration / 60)).ToString() + ":" + ((int)(e.Entry.Duration % 60)).ToString("00");					
				}
				else if (e.Type == MediaItemUpdateType.Delete)
				{
					if (this.ListView != null)
						this.ListView.Items.Remove(this);
				}

			}
		}

	}

	public class ArtistTreeNode: TreeNode 
	{
		private string artist;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="artist">Name of artist</param>
		public ArtistTreeNode(string artist) : base() 
		{
			this.artist = artist;
			this.Text = artist;
		}

		public string Artist 
		{
			get { return artist; }
			set { artist = value; }
		}
	}

	public class AlbumTreeNode: TreeNode 
	{
		private string album;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="album">Album name</param>
		public AlbumTreeNode(string album) : base() 
		{
			this.album = album;
			this.Text = album;
		}

		public string Album 
		{
			get { return album; }
			set { album = value; }
		}
	}

	public class GenreTreeNode: TreeNode 
	{
		private string genre;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="genre">Genre</param>
		public GenreTreeNode(string genre) : base() 
		{
			this.genre = genre;
			this.Text = genre;
		}

		public string Genre 
		{
			get { return genre; }
			set { genre = value; }
		}
	}

	#endregion

}
