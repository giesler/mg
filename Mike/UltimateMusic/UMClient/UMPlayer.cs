using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using UMServer;
using UMShared;
using System.Threading;
using System.Text;

namespace UMClient
{
	/// <summary>
	/// Summary description for UMPlayer.
	/// </summary>
	public class UMPlayer : System.Windows.Forms.Form
	{
		#region Declares

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button buttonPlayStop;
		private System.Windows.Forms.Button buttonPause;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button buttonQueueRemove;
		private System.Windows.Forms.Button buttonQueueUp;
		private System.Windows.Forms.Button buttonQueueDown;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ListView listViewQueue;
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.Label labelArtist;
		private System.Windows.Forms.Timer timerPaused;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxSearch;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.ListView listViewSearch;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.PictureBox pictureBoxContainer;
		private System.Windows.Forms.PictureBox pictureBoxProgress;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TreeView treeViewCollection;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ListView listViewCollection;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.Timer timerPing;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBoxVolume;
		private System.Windows.Forms.PictureBox pictureBoxVolumeContainer;
		public Client client;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.OpenFileDialog addFileDialog;
		private MediaBar mediaBar;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.TrackBar trackBarBalance;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TrackBar trackBarRate;
		private System.Windows.Forms.Button buttonAddFile;
		private System.Windows.Forms.Button buttonRemoveFile;
		private System.Windows.Forms.ContextMenu contextMenuAddFile;
		private System.Windows.Forms.MenuItem menuItemAddFile;
		private System.Windows.Forms.MenuItem menuItemAddDirectory;
		private System.Windows.Forms.MenuItem menuItemAddTree;
		private string walkDirectory;
		private bool directoryRecursive;
		private System.Windows.Forms.Label labelRate;
		private System.Windows.Forms.ListView listViewFiles;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private Status directoryAddStatus = null;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader13;
		private System.Windows.Forms.ColumnHeader columnHeader14;
		private System.Windows.Forms.ColumnHeader columnHeader15;
		private System.Windows.Forms.ColumnHeader columnHeader16;
		private DataSetMedia dsMedia;
		private System.Windows.Forms.CheckBox checkBoxDeleteOnAdd;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.ListView listViewNew;
		private System.Windows.Forms.ColumnHeader columnHeader17;
		private System.Windows.Forms.ColumnHeader columnHeader18;
		private System.Windows.Forms.ColumnHeader columnHeader19;
		private System.Windows.Forms.ColumnHeader columnHeader20;
		private System.Windows.Forms.ColumnHeader columnHeader21;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.DateTimePicker dateTimeStart;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.DateTimePicker dateTimeEnd;
		private System.Windows.Forms.Button buttonPrevious;
		private System.Windows.Forms.Label labelOpacity;
		private System.Windows.Forms.TrackBar trackBarOpacity;
		private System.Windows.Forms.ComboBox comboBoxSearch;
		private System.Windows.Forms.TabPage tabPage8;
		private System.Windows.Forms.ListView listViewPlaylists;
		private System.Windows.Forms.ListView listViewPlaylistMedia;
		private System.Windows.Forms.ColumnHeader columnHeader22;
		private System.Windows.Forms.ColumnHeader columnHeader23;
		private System.Windows.Forms.ColumnHeader columnHeader24;
		private System.Windows.Forms.ColumnHeader columnHeader25;
		private System.Windows.Forms.ColumnHeader columnHeader26;
		private System.Windows.Forms.ColumnHeader columnHeader27;
		private System.Windows.Forms.ColumnHeader columnHeader28;
		private System.Windows.Forms.ColumnHeader columnHeader29;
		private System.Windows.Forms.ColumnHeader columnHeader30;
		private System.Windows.Forms.ColumnHeader columnHeader31;
		private System.Windows.Forms.Button buttonPlaylistDelete;
		private System.Windows.Forms.ContextMenu contextMenuMediaList;
		private System.Windows.Forms.Button buttonPlaylistMediaRemove;
		private System.Windows.Forms.TabPage tabPage9;
		private System.Windows.Forms.ColumnHeader columnHeader32;
		private System.Windows.Forms.ColumnHeader columnHeader33;
		private System.Windows.Forms.ColumnHeader columnHeader34;
		private System.Windows.Forms.ColumnHeader columnHeader35;
		private System.Windows.Forms.ColumnHeader columnHeader36;
		private System.Windows.Forms.ListView listViewHistory;
		private System.Windows.Forms.Button buttonClearQueue;
		private System.Windows.Forms.Button buttonCheckMissingSongs;
		private System.Windows.Forms.Button buttonPlaylistAdd;
		private Log log;

		#endregion

		/// <summary>
		/// Creates a new UMPlayer object
		/// </summary>
		public UMPlayer()
		{
			InitializeComponent();
			log = new Log(this);
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

		#region Basic play handling

		/// <summary>
		/// Set up the player with initial values set and such
		/// </summary>
		public void InitialState() 
		{
			// Load the media list
			SqlConnection cn = new SqlConnection(client.ConnectionString);
			SqlDataAdapter da = new SqlDataAdapter("select * from Media", cn);
			dsMedia = new DataSetMedia();
			da.Fill(dsMedia, "Media");

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
			Balance_Changed(client.mediaServer.Balance);
			Rate_Changed(client.mediaServer.Rate);		
			UMPlayer_Resize(this, new EventArgs());
		}

		/// <summary>
		/// Sets up labels and such for the playing song
		/// </summary>
		/// <param name="mediaId">Current song</param>
		public void Playing(int mediaId) 
		{
			DataSetMedia.MediaRow row = client.FindMediaRow(mediaId);

			labelName.Text = row.Name;
			labelName.Visible = true;
			labelArtist.Text = row.Artist;

			buttonPlayStop.Text = "[]";
			buttonPause.Enabled = true;
			buttonPlayStop.Enabled = true;
			timerPaused.Enabled = false;

			mediaBar.labelNowPlaying.Visible = true;
			mediaBar.CurrentSong = row.Name;
			mediaBar.buttonPlayStop.Text = "[]";
			mediaBar.buttonPause.Enabled = true;

			ClearWaitForMessage();
		}

		/// <summary>
		/// Sets state to stopped
		/// </summary>
		public void Stopped() 
		{
			timerPaused.Enabled = false;
			mediaBar.labelNowPlaying.Visible = true;
			labelName.Visible = true;

			buttonPlayStop.Text = ">";
			buttonPlayStop.Enabled = true;
			buttonPause.Enabled = false;

			mediaBar.buttonPlayStop.Text = ">";
			mediaBar.buttonPlayStop.Enabled = true;
			buttonPause.Enabled = false;

			ClearWaitForMessage();
		}

		/// <summary>
		/// Sets state to paused
		/// </summary>
		public void Paused() 
		{
			buttonPlayStop.Enabled = true;
			buttonPause.Enabled = true;
			timerPaused.Enabled = true;            

			mediaBar.buttonPlayStop.Enabled = true;
			mediaBar.buttonPause.Enabled = true;

			ClearWaitForMessage();
		}

		/// <summary>
		/// Sets the progress bar to the progress passed
		/// </summary>
		/// <param name="progress">Percentage of song completion</param>
		public void Progress(double progress) 
		{
			pictureBoxProgress.Width = Convert.ToInt32(((progress) * (double) (pictureBoxContainer.Width-4)));
		}

		/// <summary>
		/// Set the volume
		/// </summary>
		/// <param name="volume">Percentage of volume</param>
		public void Volume_Changed(double volume) 
		{
			pictureBoxVolume.Width = Convert.ToInt32(((volume) * (double) (pictureBoxVolumeContainer.Width-4)));

			ClearWaitForMessage();
		}

		/// <summary>
		/// Sets the rate
		/// </summary>
		/// <param name="rate">Sets the rate trackbar</param>
		public void Rate_Changed(double rate) 
		{
			trackBarRate.Value = (int) (rate * 100);

			ClearWaitForMessage();
		}
		
		/// <summary>
		/// Sets the speaker balance
		/// </summary>
		/// <param name="balance">Balance value</param>
		public void Balance_Changed(int balance) 
		{
			trackBarBalance.Value = balance;

			ClearWaitForMessage();
		}

		public void ShowError(string errorDescription, int mediaId) 
		{
			log.AddToLog("Error", errorDescription, mediaId);
		}

		#endregion

		#region Queue handlers

		/// <summary>
		/// Add the item to the queue
		/// </summary>
		/// <param name="mediaId">Media item to add</param>
		/// <param name="position">Position in the queue</param>
		public void AddToQueue(int mediaId, Guid guid, int position) 
		{
			DataSetMedia.MediaRow row = client.FindMediaRow(mediaId);
			MediaListViewItem item = new MediaListViewItem(this, row, guid);

			// see if we should add at end or insert in queue
			if (position >= listViewQueue.Items.Count || position < 0) 
			{
				listViewQueue.Items.Add(item);
			} 
			else 
			{
				listViewQueue.Items.Insert(position, item);
			}

			ClearWaitForMessage();
		}

		/// <summary>
		/// Remove the item at the given position from the queue
		/// </summary>
		/// <param name="mediaId">Item's ID</param>
		/// <param name="position">Position in queue</param>
		public void RemoveFromQueue(int mediaId, Guid guid) 
		{	
			foreach (MediaListViewItem item in listViewQueue.Items)
			{
				if (item.Guid == guid)
				{
                    listViewQueue.Items.Remove(item);
					break;
				}
			}
			ClearWaitForMessage();
		}

		/// <summary>
		/// Move an item in the queue
		/// </summary>
		/// <param name="mediaId">Item to move</param>
		/// <param name="guid">Unique ID of item</param>
		/// <param name="newPosition"></param>
		public void MovedInQueue(int mediaId, Guid guid, int newPosition) 
		{
			RemoveFromQueue(mediaId, guid);
			AddToQueue(mediaId, guid, newPosition);

			listViewQueue.Items[newPosition].Selected = true;

			ClearWaitForMessage();
		}

		/// <summary>
		/// Reload the queue from the server
		/// </summary>
		public void ReloadQueue(object sender, EventArgs e) 
		{
			MediaCollection queue = client.mediaServer.CurrentQueue();

			// Clear the current queue
			listViewQueue.Items.Clear();

			// Loop through adding listviewitems
			foreach(MediaCollectionEntry entry in queue) 
			{
				listViewQueue.Items.Add(
					new MediaListViewItem(this, client.FindMediaRow(entry.MediaId), entry.Guid));
			}

			ClearWaitForMessage();
		}

		#endregion

		#region History

		public void AddToHistory(int mediaId, Guid guid)
		{
            MediaListViewItem item = new MediaListViewItem(this, client.FindMediaRow(mediaId), guid);
			listViewHistory.Items.Insert(0, item);
		}

		public void RemoveFromHistory(int mediaId, Guid guid)
		{
			foreach (MediaListViewItem item in listViewHistory.Items)
			{
				if (item.Guid == guid)
				{
					listViewHistory.Items.Remove(item);
					break;
				}
			}
		}

		#endregion
		
		/// <summary>
		/// Add info to the textbox log
		/// </summary>
		/// <param name="function">Place event occurred</param>
		/// <param name="message">Event details</param>
		public void AddToLog(string function, string message) 
		{
			log.AddToLog(function, message);
		}

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
			this.pictureBoxProgress = new System.Windows.Forms.PictureBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.buttonPrevious = new System.Windows.Forms.Button();
			this.pictureBoxVolume = new System.Windows.Forms.PictureBox();
			this.pictureBoxVolumeContainer = new System.Windows.Forms.PictureBox();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonPause = new System.Windows.Forms.Button();
			this.buttonPlayStop = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.labelName = new System.Windows.Forms.Label();
			this.labelArtist = new System.Windows.Forms.Label();
			this.pictureBoxContainer = new System.Windows.Forms.PictureBox();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.buttonQueueDown = new System.Windows.Forms.Button();
			this.buttonQueueUp = new System.Windows.Forms.Button();
			this.buttonQueueRemove = new System.Windows.Forms.Button();
			this.listViewQueue = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader16 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.contextMenuMediaList = new System.Windows.Forms.ContextMenu();
			this.tabPage8 = new System.Windows.Forms.TabPage();
			this.buttonPlaylistMediaRemove = new System.Windows.Forms.Button();
			this.listViewPlaylistMedia = new System.Windows.Forms.ListView();
			this.columnHeader22 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader23 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader24 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader25 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader26 = new System.Windows.Forms.ColumnHeader();
			this.buttonPlaylistDelete = new System.Windows.Forms.Button();
			this.buttonPlaylistAdd = new System.Windows.Forms.Button();
			this.listViewPlaylists = new System.Windows.Forms.ListView();
			this.columnHeader27 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader28 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader29 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader30 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader31 = new System.Windows.Forms.ColumnHeader();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.listViewFiles = new System.Windows.Forms.ListView();
			this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
			this.buttonRemoveFile = new System.Windows.Forms.Button();
			this.buttonAddFile = new System.Windows.Forms.Button();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.checkBoxDeleteOnAdd = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.listViewCollection = new System.Windows.Forms.ListView();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.treeViewCollection = new System.Windows.Forms.TreeView();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.comboBoxSearch = new System.Windows.Forms.ComboBox();
			this.listViewSearch = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.buttonSearch = new System.Windows.Forms.Button();
			this.textBoxSearch = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.dateTimeEnd = new System.Windows.Forms.DateTimePicker();
			this.label6 = new System.Windows.Forms.Label();
			this.dateTimeStart = new System.Windows.Forms.DateTimePicker();
			this.label5 = new System.Windows.Forms.Label();
			this.listViewNew = new System.Windows.Forms.ListView();
			this.columnHeader17 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader18 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader19 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader20 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader21 = new System.Windows.Forms.ColumnHeader();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.labelOpacity = new System.Windows.Forms.Label();
			this.trackBarOpacity = new System.Windows.Forms.TrackBar();
			this.labelRate = new System.Windows.Forms.Label();
			this.trackBarRate = new System.Windows.Forms.TrackBar();
			this.label4 = new System.Windows.Forms.Label();
			this.trackBarBalance = new System.Windows.Forms.TrackBar();
			this.tabPage9 = new System.Windows.Forms.TabPage();
			this.listViewHistory = new System.Windows.Forms.ListView();
			this.columnHeader32 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader33 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader34 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader35 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader36 = new System.Windows.Forms.ColumnHeader();
			this.timerPaused = new System.Windows.Forms.Timer(this.components);
			this.timerPing = new System.Windows.Forms.Timer(this.components);
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.addFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.contextMenuAddFile = new System.Windows.Forms.ContextMenu();
			this.menuItemAddFile = new System.Windows.Forms.MenuItem();
			this.menuItemAddDirectory = new System.Windows.Forms.MenuItem();
			this.menuItemAddTree = new System.Windows.Forms.MenuItem();
			this.buttonClearQueue = new System.Windows.Forms.Button();
			this.buttonCheckMissingSongs = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage8.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarRate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBalance)).BeginInit();
			this.tabPage9.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.pictureBoxProgress,
																				 this.panel2,
																				 this.labelName,
																				 this.labelArtist,
																				 this.pictureBoxContainer});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(504, 80);
			this.panel1.TabIndex = 0;
			// 
			// pictureBoxProgress
			// 
			this.pictureBoxProgress.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.pictureBoxProgress.BackColor = System.Drawing.SystemColors.HotTrack;
			this.pictureBoxProgress.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBoxProgress.Location = new System.Drawing.Point(10, 66);
			this.pictureBoxProgress.Name = "pictureBoxProgress";
			this.pictureBoxProgress.Size = new System.Drawing.Size(72, 4);
			this.pictureBoxProgress.TabIndex = 4;
			this.pictureBoxProgress.TabStop = false;
			this.pictureBoxProgress.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxProgress_MouseUp);
			// 
			// panel2
			// 
			this.panel2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.buttonPrevious,
																				 this.pictureBoxVolume,
																				 this.pictureBoxVolumeContainer,
																				 this.buttonNext,
																				 this.buttonPause,
																				 this.buttonPlayStop,
																				 this.label2});
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(344, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(160, 80);
			this.panel2.TabIndex = 2;
			// 
			// buttonPrevious
			// 
			this.buttonPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPrevious.Location = new System.Drawing.Point(8, 48);
			this.buttonPrevious.Name = "buttonPrevious";
			this.buttonPrevious.Size = new System.Drawing.Size(32, 18);
			this.buttonPrevious.TabIndex = 8;
			this.buttonPrevious.Text = "< <";
			this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
			// 
			// pictureBoxVolume
			// 
			this.pictureBoxVolume.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxVolume.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("pictureBoxVolume.BackgroundImage")));
			this.pictureBoxVolume.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBoxVolume.Location = new System.Drawing.Point(18, 26);
			this.pictureBoxVolume.Name = "pictureBoxVolume";
			this.pictureBoxVolume.Size = new System.Drawing.Size(80, 4);
			this.pictureBoxVolume.TabIndex = 6;
			this.pictureBoxVolume.TabStop = false;
			this.pictureBoxVolume.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxVolume_MouseUp);
			// 
			// pictureBoxVolumeContainer
			// 
			this.pictureBoxVolumeContainer.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxVolumeContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBoxVolumeContainer.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBoxVolumeContainer.Location = new System.Drawing.Point(16, 24);
			this.pictureBoxVolumeContainer.Name = "pictureBoxVolumeContainer";
			this.pictureBoxVolumeContainer.Size = new System.Drawing.Size(100, 8);
			this.pictureBoxVolumeContainer.TabIndex = 5;
			this.pictureBoxVolumeContainer.TabStop = false;
			this.pictureBoxVolumeContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxVolumeContainer_MouseUp);
			// 
			// buttonNext
			// 
			this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonNext.Location = new System.Drawing.Point(120, 48);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(32, 18);
			this.buttonNext.TabIndex = 2;
			this.buttonNext.Text = "> >";
			this.buttonNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonNext_MouseUp);
			// 
			// buttonPause
			// 
			this.buttonPause.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPause.Location = new System.Drawing.Point(80, 48);
			this.buttonPause.Name = "buttonPause";
			this.buttonPause.Size = new System.Drawing.Size(32, 18);
			this.buttonPause.TabIndex = 1;
			this.buttonPause.Text = "| |";
			this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
			// 
			// buttonPlayStop
			// 
			this.buttonPlayStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlayStop.Location = new System.Drawing.Point(48, 48);
			this.buttonPlayStop.Name = "buttonPlayStop";
			this.buttonPlayStop.Size = new System.Drawing.Size(32, 18);
			this.buttonPlayStop.TabIndex = 0;
			this.buttonPlayStop.Text = ">";
			this.buttonPlayStop.Click += new System.EventHandler(this.buttonPlayStop_Click);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(16, 8);
			this.label2.Name = "label2";
			this.label2.TabIndex = 7;
			this.label2.Text = "Volume";
			// 
			// labelName
			// 
			this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.labelName.BackColor = System.Drawing.Color.Transparent;
			this.labelName.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelName.Location = new System.Drawing.Point(8, 24);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(328, 32);
			this.labelName.TabIndex = 1;
			this.labelName.Text = "[name]";
			this.labelName.DoubleClick += new System.EventHandler(this.labelName_DoubleClick);
			// 
			// labelArtist
			// 
			this.labelArtist.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.labelArtist.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelArtist.Location = new System.Drawing.Point(8, 8);
			this.labelArtist.Name = "labelArtist";
			this.labelArtist.Size = new System.Drawing.Size(328, 23);
			this.labelArtist.TabIndex = 0;
			this.labelArtist.Text = "[artist]";
			// 
			// pictureBoxContainer
			// 
			this.pictureBoxContainer.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.pictureBoxContainer.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBoxContainer.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBoxContainer.Location = new System.Drawing.Point(8, 64);
			this.pictureBoxContainer.Name = "pictureBoxContainer";
			this.pictureBoxContainer.Size = new System.Drawing.Size(328, 8);
			this.pictureBoxContainer.TabIndex = 3;
			this.pictureBoxContainer.TabStop = false;
			this.pictureBoxContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxContainer_MouseUp);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 368);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.statusBarPanel1});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(504, 22);
			this.statusBar1.TabIndex = 1;
			this.statusBar1.Visible = false;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Width = 488;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1,
																					  this.tabPage8,
																					  this.tabPage2,
																					  this.tabPage4,
																					  this.tabPage3,
																					  this.tabPage5,
																					  this.tabPage6,
																					  this.tabPage9});
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 80);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(504, 288);
			this.tabControl1.TabIndex = 2;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.Transparent;
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.buttonClearQueue,
																				   this.buttonQueueDown,
																				   this.buttonQueueUp,
																				   this.buttonQueueRemove,
																				   this.listViewQueue});
			this.tabPage1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(496, 262);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Queue";
			// 
			// buttonQueueDown
			// 
			this.buttonQueueDown.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonQueueDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonQueueDown.Location = new System.Drawing.Point(456, 232);
			this.buttonQueueDown.Name = "buttonQueueDown";
			this.buttonQueueDown.Size = new System.Drawing.Size(32, 23);
			this.buttonQueueDown.TabIndex = 3;
			this.buttonQueueDown.Text = "\\/";
			this.buttonQueueDown.Click += new System.EventHandler(this.buttonQueueDown_Click);
			// 
			// buttonQueueUp
			// 
			this.buttonQueueUp.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonQueueUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonQueueUp.Location = new System.Drawing.Point(456, 200);
			this.buttonQueueUp.Name = "buttonQueueUp";
			this.buttonQueueUp.Size = new System.Drawing.Size(32, 23);
			this.buttonQueueUp.TabIndex = 2;
			this.buttonQueueUp.Text = "/\\";
			this.buttonQueueUp.Click += new System.EventHandler(this.buttonQueueUp_Click);
			// 
			// buttonQueueRemove
			// 
			this.buttonQueueRemove.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonQueueRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonQueueRemove.Location = new System.Drawing.Point(456, 8);
			this.buttonQueueRemove.Name = "buttonQueueRemove";
			this.buttonQueueRemove.Size = new System.Drawing.Size(32, 23);
			this.buttonQueueRemove.TabIndex = 1;
			this.buttonQueueRemove.Text = "X";
			this.buttonQueueRemove.Click += new System.EventHandler(this.buttonQueueRemove_Click);
			// 
			// listViewQueue
			// 
			this.listViewQueue.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.listViewQueue.BackColor = System.Drawing.SystemColors.Window;
			this.listViewQueue.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader1,
																							this.columnHeader2,
																							this.columnHeader15,
																							this.columnHeader16,
																							this.columnHeader3});
			this.listViewQueue.ContextMenu = this.contextMenuMediaList;
			this.listViewQueue.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewQueue.FullRowSelect = true;
			this.listViewQueue.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewQueue.HideSelection = false;
			this.listViewQueue.Name = "listViewQueue";
			this.listViewQueue.Size = new System.Drawing.Size(448, 262);
			this.listViewQueue.TabIndex = 0;
			this.listViewQueue.View = System.Windows.Forms.View.Details;
			this.listViewQueue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listViewQueue_KeyUp);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 150;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Artist";
			this.columnHeader2.Width = 150;
			// 
			// columnHeader15
			// 
			this.columnHeader15.Text = "Album";
			// 
			// columnHeader16
			// 
			this.columnHeader16.Text = "Track";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Duration";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader3.Width = 100;
			// 
			// contextMenuMediaList
			// 
			this.contextMenuMediaList.Popup += new System.EventHandler(this.contextMenuMediaList_Popup);
			// 
			// tabPage8
			// 
			this.tabPage8.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.buttonPlaylistMediaRemove,
																				   this.listViewPlaylistMedia,
																				   this.buttonPlaylistDelete,
																				   this.buttonPlaylistAdd,
																				   this.listViewPlaylists});
			this.tabPage8.Location = new System.Drawing.Point(4, 22);
			this.tabPage8.Name = "tabPage8";
			this.tabPage8.Size = new System.Drawing.Size(496, 262);
			this.tabPage8.TabIndex = 9;
			this.tabPage8.Text = "Playlists";
			// 
			// buttonPlaylistMediaRemove
			// 
			this.buttonPlaylistMediaRemove.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonPlaylistMediaRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlaylistMediaRemove.Location = new System.Drawing.Point(464, 88);
			this.buttonPlaylistMediaRemove.Name = "buttonPlaylistMediaRemove";
			this.buttonPlaylistMediaRemove.Size = new System.Drawing.Size(24, 23);
			this.buttonPlaylistMediaRemove.TabIndex = 13;
			this.buttonPlaylistMediaRemove.Text = "x";
			this.buttonPlaylistMediaRemove.Click += new System.EventHandler(this.buttonPlaylistMediaRemove_Click);
			// 
			// listViewPlaylistMedia
			// 
			this.listViewPlaylistMedia.BackColor = System.Drawing.SystemColors.Window;
			this.listViewPlaylistMedia.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																									this.columnHeader22,
																									this.columnHeader23,
																									this.columnHeader24,
																									this.columnHeader25,
																									this.columnHeader26});
			this.listViewPlaylistMedia.ContextMenu = this.contextMenuMediaList;
			this.listViewPlaylistMedia.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewPlaylistMedia.FullRowSelect = true;
			this.listViewPlaylistMedia.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewPlaylistMedia.HideSelection = false;
			this.listViewPlaylistMedia.Location = new System.Drawing.Point(8, 88);
			this.listViewPlaylistMedia.Name = "listViewPlaylistMedia";
			this.listViewPlaylistMedia.Size = new System.Drawing.Size(448, 168);
			this.listViewPlaylistMedia.TabIndex = 11;
			this.listViewPlaylistMedia.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader22
			// 
			this.columnHeader22.Text = "Name";
			this.columnHeader22.Width = 150;
			// 
			// columnHeader23
			// 
			this.columnHeader23.Text = "Artist";
			this.columnHeader23.Width = 150;
			// 
			// columnHeader24
			// 
			this.columnHeader24.Text = "Album";
			// 
			// columnHeader25
			// 
			this.columnHeader25.Text = "Track";
			this.columnHeader25.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// columnHeader26
			// 
			this.columnHeader26.Text = "Duration";
			this.columnHeader26.Width = 100;
			// 
			// buttonPlaylistDelete
			// 
			this.buttonPlaylistDelete.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonPlaylistDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlaylistDelete.Location = new System.Drawing.Point(464, 40);
			this.buttonPlaylistDelete.Name = "buttonPlaylistDelete";
			this.buttonPlaylistDelete.Size = new System.Drawing.Size(24, 23);
			this.buttonPlaylistDelete.TabIndex = 10;
			this.buttonPlaylistDelete.Text = "x";
			this.buttonPlaylistDelete.Click += new System.EventHandler(this.buttonPlaylistDelete_Click);
			// 
			// buttonPlaylistAdd
			// 
			this.buttonPlaylistAdd.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonPlaylistAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlaylistAdd.Location = new System.Drawing.Point(464, 8);
			this.buttonPlaylistAdd.Name = "buttonPlaylistAdd";
			this.buttonPlaylistAdd.Size = new System.Drawing.Size(24, 23);
			this.buttonPlaylistAdd.TabIndex = 9;
			this.buttonPlaylistAdd.Text = "+";
			this.buttonPlaylistAdd.Click += new System.EventHandler(this.buttonPlaylistAdd_Click);
			// 
			// listViewPlaylists
			// 
			this.listViewPlaylists.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.listViewPlaylists.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.columnHeader27,
																								this.columnHeader28,
																								this.columnHeader29,
																								this.columnHeader30,
																								this.columnHeader31});
			this.listViewPlaylists.FullRowSelect = true;
			this.listViewPlaylists.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewPlaylists.HideSelection = false;
			this.listViewPlaylists.Location = new System.Drawing.Point(8, 8);
			this.listViewPlaylists.MultiSelect = false;
			this.listViewPlaylists.Name = "listViewPlaylists";
			this.listViewPlaylists.Size = new System.Drawing.Size(448, 72);
			this.listViewPlaylists.TabIndex = 0;
			this.listViewPlaylists.View = System.Windows.Forms.View.Details;
			this.listViewPlaylists.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewPlaylists_MouseUp);
			this.listViewPlaylists.SelectedIndexChanged += new System.EventHandler(this.listViewPlaylists_SelectedIndexChanged);
			// 
			// columnHeader27
			// 
			this.columnHeader27.Text = "Name";
			// 
			// columnHeader28
			// 
			this.columnHeader28.Text = "Created";
			this.columnHeader28.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// columnHeader29
			// 
			this.columnHeader29.Text = "Updated";
			this.columnHeader29.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// columnHeader30
			// 
			this.columnHeader30.Text = "Songs";
			this.columnHeader30.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// columnHeader31
			// 
			this.columnHeader31.Text = "Length";
			this.columnHeader31.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// tabPage5
			// 
			this.tabPage5.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage5.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.listViewFiles,
																				   this.buttonRemoveFile,
																				   this.buttonAddFile,
																				   this.buttonAdd,
																				   this.checkBoxDeleteOnAdd,
																				   this.label3});
			this.tabPage5.Location = new System.Drawing.Point(4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Size = new System.Drawing.Size(496, 262);
			this.tabPage5.TabIndex = 5;
			this.tabPage5.Text = "Add Songs";
			// 
			// listViewFiles
			// 
			this.listViewFiles.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.listViewFiles.BackColor = System.Drawing.SystemColors.Window;
			this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader10});
			this.listViewFiles.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewFiles.FullRowSelect = true;
			this.listViewFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listViewFiles.HideSelection = false;
			this.listViewFiles.Location = new System.Drawing.Point(8, 24);
			this.listViewFiles.Name = "listViewFiles";
			this.listViewFiles.Size = new System.Drawing.Size(448, 200);
			this.listViewFiles.TabIndex = 9;
			this.listViewFiles.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader10
			// 
			this.columnHeader10.Text = "File";
			this.columnHeader10.Width = 250;
			// 
			// buttonRemoveFile
			// 
			this.buttonRemoveFile.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonRemoveFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonRemoveFile.Location = new System.Drawing.Point(464, 64);
			this.buttonRemoveFile.Name = "buttonRemoveFile";
			this.buttonRemoveFile.Size = new System.Drawing.Size(24, 23);
			this.buttonRemoveFile.TabIndex = 8;
			this.buttonRemoveFile.Text = "x";
			this.buttonRemoveFile.Click += new System.EventHandler(this.buttonRemoveFile_Click);
			// 
			// buttonAddFile
			// 
			this.buttonAddFile.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonAddFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonAddFile.Location = new System.Drawing.Point(464, 32);
			this.buttonAddFile.Name = "buttonAddFile";
			this.buttonAddFile.Size = new System.Drawing.Size(24, 23);
			this.buttonAddFile.TabIndex = 7;
			this.buttonAddFile.Text = "+";
			this.buttonAddFile.Click += new System.EventHandler(this.buttonAddFile_Click);
			// 
			// buttonAdd
			// 
			this.buttonAdd.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonAdd.Location = new System.Drawing.Point(408, 232);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.TabIndex = 5;
			this.buttonAdd.Text = "&Add";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// checkBoxDeleteOnAdd
			// 
			this.checkBoxDeleteOnAdd.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.checkBoxDeleteOnAdd.Location = new System.Drawing.Point(8, 232);
			this.checkBoxDeleteOnAdd.Name = "checkBoxDeleteOnAdd";
			this.checkBoxDeleteOnAdd.Size = new System.Drawing.Size(248, 24);
			this.checkBoxDeleteOnAdd.TabIndex = 4;
			this.checkBoxDeleteOnAdd.Text = "&Delete files after they are added";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(488, 23);
			this.label3.TabIndex = 0;
			this.label3.Text = "Select the songs you would like to add below, then click \'Add\'.";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.listViewCollection,
																				   this.splitter1,
																				   this.treeViewCollection});
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(496, 262);
			this.tabPage2.TabIndex = 4;
			this.tabPage2.Text = "Media Collection";
			// 
			// listViewCollection
			// 
			this.listViewCollection.BackColor = System.Drawing.SystemColors.Window;
			this.listViewCollection.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								 this.columnHeader7,
																								 this.columnHeader8,
																								 this.columnHeader11,
																								 this.columnHeader12,
																								 this.columnHeader9});
			this.listViewCollection.ContextMenu = this.contextMenuMediaList;
			this.listViewCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewCollection.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewCollection.FullRowSelect = true;
			this.listViewCollection.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewCollection.HideSelection = false;
			this.listViewCollection.Location = new System.Drawing.Point(195, 0);
			this.listViewCollection.Name = "listViewCollection";
			this.listViewCollection.Size = new System.Drawing.Size(301, 262);
			this.listViewCollection.TabIndex = 2;
			this.listViewCollection.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Name";
			this.columnHeader7.Width = 150;
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Artist";
			this.columnHeader8.Width = 150;
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "Album";
			// 
			// columnHeader12
			// 
			this.columnHeader12.Text = "Track";
			this.columnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "Duration";
			this.columnHeader9.Width = 100;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(192, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 262);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// treeViewCollection
			// 
			this.treeViewCollection.BackColor = System.Drawing.SystemColors.Window;
			this.treeViewCollection.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewCollection.ForeColor = System.Drawing.SystemColors.ControlText;
			this.treeViewCollection.HideSelection = false;
			this.treeViewCollection.ImageIndex = -1;
			this.treeViewCollection.Name = "treeViewCollection";
			this.treeViewCollection.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																						   new System.Windows.Forms.TreeNode("By Artist", new System.Windows.Forms.TreeNode[] {
																																												  new System.Windows.Forms.TreeNode("Loading")}),
																						   new System.Windows.Forms.TreeNode("By Album", new System.Windows.Forms.TreeNode[] {
																																												 new System.Windows.Forms.TreeNode("Loading")}),
																						   new System.Windows.Forms.TreeNode("By Genre", new System.Windows.Forms.TreeNode[] {
																																												 new System.Windows.Forms.TreeNode("Loading")})});
			this.treeViewCollection.SelectedImageIndex = -1;
			this.treeViewCollection.Size = new System.Drawing.Size(192, 262);
			this.treeViewCollection.TabIndex = 0;
			this.treeViewCollection.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCollection_AfterSelect);
			this.treeViewCollection.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewCollection_BeforeExpand);
			// 
			// tabPage4
			// 
			this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage4.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.comboBoxSearch,
																				   this.listViewSearch,
																				   this.buttonSearch,
																				   this.textBoxSearch,
																				   this.label1});
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(496, 262);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Search";
			// 
			// comboBoxSearch
			// 
			this.comboBoxSearch.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.comboBoxSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSearch.Items.AddRange(new object[] {
																"any field",
																"artist",
																"song name",
																"album name",
																"comments"});
			this.comboBoxSearch.Location = new System.Drawing.Point(256, 8);
			this.comboBoxSearch.Name = "comboBoxSearch";
			this.comboBoxSearch.Size = new System.Drawing.Size(176, 21);
			this.comboBoxSearch.TabIndex = 5;
			// 
			// listViewSearch
			// 
			this.listViewSearch.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.listViewSearch.BackColor = System.Drawing.SystemColors.Window;
			this.listViewSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.columnHeader4,
																							 this.columnHeader5,
																							 this.columnHeader13,
																							 this.columnHeader14,
																							 this.columnHeader6});
			this.listViewSearch.ContextMenu = this.contextMenuMediaList;
			this.listViewSearch.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewSearch.FullRowSelect = true;
			this.listViewSearch.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewSearch.HideSelection = false;
			this.listViewSearch.Location = new System.Drawing.Point(0, 32);
			this.listViewSearch.Name = "listViewSearch";
			this.listViewSearch.Size = new System.Drawing.Size(488, 224);
			this.listViewSearch.TabIndex = 4;
			this.listViewSearch.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Name";
			this.columnHeader4.Width = 150;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Artist";
			this.columnHeader5.Width = 150;
			// 
			// columnHeader13
			// 
			this.columnHeader13.Text = "Album";
			// 
			// columnHeader14
			// 
			this.columnHeader14.Text = "Track";
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Duration";
			this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader6.Width = 100;
			// 
			// buttonSearch
			// 
			this.buttonSearch.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonSearch.Location = new System.Drawing.Point(432, 8);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(56, 24);
			this.buttonSearch.TabIndex = 3;
			this.buttonSearch.Text = "&Search";
			this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
			// 
			// textBoxSearch
			// 
			this.textBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxSearch.BackColor = System.Drawing.SystemColors.Window;
			this.textBoxSearch.ForeColor = System.Drawing.SystemColors.ControlText;
			this.textBoxSearch.Location = new System.Drawing.Point(72, 8);
			this.textBoxSearch.Name = "textBoxSearch";
			this.textBoxSearch.Size = new System.Drawing.Size(176, 20);
			this.textBoxSearch.TabIndex = 1;
			this.textBoxSearch.Text = "";
			this.textBoxSearch.Leave += new System.EventHandler(this.textBoxSearch_Leave);
			this.textBoxSearch.Enter += new System.EventHandler(this.textBoxSearch_Enter);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Search for:";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.dateTimeEnd,
																				   this.label6,
																				   this.dateTimeStart,
																				   this.label5,
																				   this.listViewNew});
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(496, 262);
			this.tabPage3.TabIndex = 8;
			this.tabPage3.Text = "New";
			// 
			// dateTimeEnd
			// 
			this.dateTimeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimeEnd.Location = new System.Drawing.Point(288, 8);
			this.dateTimeEnd.Name = "dateTimeEnd";
			this.dateTimeEnd.Size = new System.Drawing.Size(96, 20);
			this.dateTimeEnd.TabIndex = 10;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(256, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 24);
			this.label6.TabIndex = 9;
			this.label6.Text = "and";
			// 
			// dateTimeStart
			// 
			this.dateTimeStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimeStart.Location = new System.Drawing.Point(152, 8);
			this.dateTimeStart.Name = "dateTimeStart";
			this.dateTimeStart.Size = new System.Drawing.Size(96, 20);
			this.dateTimeStart.TabIndex = 8;
			this.dateTimeStart.ValueChanged += new System.EventHandler(this.dateTimeStart_ValueChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(144, 23);
			this.label5.TabIndex = 7;
			this.label5.Text = "Show files added between ";
			// 
			// listViewNew
			// 
			this.listViewNew.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.listViewNew.BackColor = System.Drawing.SystemColors.Window;
			this.listViewNew.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.columnHeader17,
																						  this.columnHeader18,
																						  this.columnHeader19,
																						  this.columnHeader20,
																						  this.columnHeader21});
			this.listViewNew.ContextMenu = this.contextMenuMediaList;
			this.listViewNew.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewNew.FullRowSelect = true;
			this.listViewNew.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewNew.HideSelection = false;
			this.listViewNew.Location = new System.Drawing.Point(0, 40);
			this.listViewNew.Name = "listViewNew";
			this.listViewNew.Size = new System.Drawing.Size(488, 224);
			this.listViewNew.TabIndex = 5;
			this.listViewNew.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader17
			// 
			this.columnHeader17.Text = "Name";
			this.columnHeader17.Width = 150;
			// 
			// columnHeader18
			// 
			this.columnHeader18.Text = "Artist";
			this.columnHeader18.Width = 150;
			// 
			// columnHeader19
			// 
			this.columnHeader19.Text = "Album";
			// 
			// columnHeader20
			// 
			this.columnHeader20.Text = "Track";
			// 
			// columnHeader21
			// 
			this.columnHeader21.Text = "Duration";
			this.columnHeader21.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader21.Width = 100;
			// 
			// tabPage6
			// 
			this.tabPage6.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage6.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.buttonCheckMissingSongs,
																				   this.labelOpacity,
																				   this.trackBarOpacity,
																				   this.labelRate,
																				   this.trackBarRate,
																				   this.label4,
																				   this.trackBarBalance});
			this.tabPage6.Location = new System.Drawing.Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Size = new System.Drawing.Size(496, 262);
			this.tabPage6.TabIndex = 6;
			this.tabPage6.Text = "More";
			// 
			// labelOpacity
			// 
			this.labelOpacity.Location = new System.Drawing.Point(40, 160);
			this.labelOpacity.Name = "labelOpacity";
			this.labelOpacity.TabIndex = 5;
			this.labelOpacity.Text = "Opacity";
			this.labelOpacity.DoubleClick += new System.EventHandler(this.labelOpacity_DoubleClick);
			// 
			// trackBarOpacity
			// 
			this.trackBarOpacity.LargeChange = 10;
			this.trackBarOpacity.Location = new System.Drawing.Point(32, 184);
			this.trackBarOpacity.Maximum = 100;
			this.trackBarOpacity.Name = "trackBarOpacity";
			this.trackBarOpacity.Size = new System.Drawing.Size(456, 45);
			this.trackBarOpacity.TabIndex = 4;
			this.trackBarOpacity.TickFrequency = 10;
			this.trackBarOpacity.Value = 100;
			this.trackBarOpacity.Scroll += new System.EventHandler(this.trackBarOpacity_Scroll);
			// 
			// labelRate
			// 
			this.labelRate.Location = new System.Drawing.Point(32, 88);
			this.labelRate.Name = "labelRate";
			this.labelRate.TabIndex = 3;
			this.labelRate.Text = "Rate";
			this.labelRate.DoubleClick += new System.EventHandler(this.labelRate_DoubleClick);
			// 
			// trackBarRate
			// 
			this.trackBarRate.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.trackBarRate.LargeChange = 10;
			this.trackBarRate.Location = new System.Drawing.Point(24, 112);
			this.trackBarRate.Maximum = 200;
			this.trackBarRate.Minimum = 1;
			this.trackBarRate.Name = "trackBarRate";
			this.trackBarRate.Size = new System.Drawing.Size(448, 45);
			this.trackBarRate.TabIndex = 2;
			this.trackBarRate.TickFrequency = 20;
			this.trackBarRate.Value = 1;
			this.trackBarRate.Scroll += new System.EventHandler(this.trackBarRate_Scroll);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(32, 16);
			this.label4.Name = "label4";
			this.label4.TabIndex = 1;
			this.label4.Text = "Balance";
			this.label4.DoubleClick += new System.EventHandler(this.label4_DoubleClick);
			// 
			// trackBarBalance
			// 
			this.trackBarBalance.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.trackBarBalance.LargeChange = 1000;
			this.trackBarBalance.Location = new System.Drawing.Point(24, 40);
			this.trackBarBalance.Maximum = 5000;
			this.trackBarBalance.Minimum = -5000;
			this.trackBarBalance.Name = "trackBarBalance";
			this.trackBarBalance.Size = new System.Drawing.Size(448, 45);
			this.trackBarBalance.SmallChange = 100;
			this.trackBarBalance.TabIndex = 0;
			this.trackBarBalance.TickFrequency = 1000;
			this.trackBarBalance.Scroll += new System.EventHandler(this.trackBarBalance_Scroll);
			// 
			// tabPage9
			// 
			this.tabPage9.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.listViewHistory});
			this.tabPage9.Location = new System.Drawing.Point(4, 22);
			this.tabPage9.Name = "tabPage9";
			this.tabPage9.Size = new System.Drawing.Size(496, 262);
			this.tabPage9.TabIndex = 10;
			this.tabPage9.Text = "History";
			// 
			// listViewHistory
			// 
			this.listViewHistory.BackColor = System.Drawing.SystemColors.Window;
			this.listViewHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							  this.columnHeader32,
																							  this.columnHeader33,
																							  this.columnHeader34,
																							  this.columnHeader35,
																							  this.columnHeader36});
			this.listViewHistory.ContextMenu = this.contextMenuMediaList;
			this.listViewHistory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewHistory.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewHistory.FullRowSelect = true;
			this.listViewHistory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewHistory.HideSelection = false;
			this.listViewHistory.Name = "listViewHistory";
			this.listViewHistory.Size = new System.Drawing.Size(496, 262);
			this.listViewHistory.TabIndex = 1;
			this.listViewHistory.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader32
			// 
			this.columnHeader32.Text = "Name";
			this.columnHeader32.Width = 150;
			// 
			// columnHeader33
			// 
			this.columnHeader33.Text = "Artist";
			this.columnHeader33.Width = 150;
			// 
			// columnHeader34
			// 
			this.columnHeader34.Text = "Album";
			// 
			// columnHeader35
			// 
			this.columnHeader35.Text = "Track";
			// 
			// columnHeader36
			// 
			this.columnHeader36.Text = "Duration";
			this.columnHeader36.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader36.Width = 100;
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
			// addFileDialog
			// 
			this.addFileDialog.Filter = "Media Files (*.mp3, *.wma)|*.mp3;*.wma|All Files (*.*)|*.*";
			this.addFileDialog.Multiselect = true;
			this.addFileDialog.Title = "Add file(s)";
			// 
			// contextMenuAddFile
			// 
			this.contextMenuAddFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							   this.menuItemAddFile,
																							   this.menuItemAddDirectory,
																							   this.menuItemAddTree});
			// 
			// menuItemAddFile
			// 
			this.menuItemAddFile.Index = 0;
			this.menuItemAddFile.Text = "&Files...";
			this.menuItemAddFile.Click += new System.EventHandler(this.menuItemAddFile_Click);
			// 
			// menuItemAddDirectory
			// 
			this.menuItemAddDirectory.Index = 1;
			this.menuItemAddDirectory.Text = "&Single directory...";
			this.menuItemAddDirectory.Click += new System.EventHandler(this.menuItemAddDirectory_Click);
			// 
			// menuItemAddTree
			// 
			this.menuItemAddTree.Index = 2;
			this.menuItemAddTree.Text = "&Directory and subdirectories...";
			this.menuItemAddTree.Click += new System.EventHandler(this.menuItemAddTree_Click);
			// 
			// buttonClearQueue
			// 
			this.buttonClearQueue.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonClearQueue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonClearQueue.Location = new System.Drawing.Point(456, 40);
			this.buttonClearQueue.Name = "buttonClearQueue";
			this.buttonClearQueue.Size = new System.Drawing.Size(32, 23);
			this.buttonClearQueue.TabIndex = 4;
			this.buttonClearQueue.Text = "XX";
			this.buttonClearQueue.Click += new System.EventHandler(this.buttonClearQueue_Click);
			// 
			// buttonCheckMissingSongs
			// 
			this.buttonCheckMissingSongs.Location = new System.Drawing.Point(400, 240);
			this.buttonCheckMissingSongs.Name = "buttonCheckMissingSongs";
			this.buttonCheckMissingSongs.Size = new System.Drawing.Size(80, 16);
			this.buttonCheckMissingSongs.TabIndex = 6;
			this.buttonCheckMissingSongs.Text = "check...";
			this.buttonCheckMissingSongs.Click += new System.EventHandler(this.buttonCheckMissingSongs_Click);
			// 
			// UMPlayer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(504, 390);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1,
																		  this.statusBar1,
																		  this.panel1});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "UMPlayer";
			this.Text = "Ultimate Music Player";
			this.Resize += new System.EventHandler(this.UMPlayer_Resize);
			this.Load += new System.EventHandler(this.UMPlayer_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UMPlayer_KeyUp);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage8.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarRate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBalance)).EndInit();
			this.tabPage9.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void timerPaused_Tick(object sender, System.EventArgs e)
		{
			labelName.Visible = !labelName.Visible;
			mediaBar.labelNowPlaying.Visible = !mediaBar.labelNowPlaying.Visible;
		}

		public void Pause() 
		{
			buttonPause_Click(this, new EventArgs());
		}

		private void buttonPause_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			if (client.mediaServer.CurrentPlayState == MediaServer.PlayState.Playing) 
			{
				client.mediaServer.Pause();
			} 
			else 
			{
				client.mediaServer.Play();
			}
		}

		public void PlayStop() 
		{
			buttonPlayStop_Click(this, new EventArgs());
		}

		private void buttonPlayStop_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			if (client.mediaServer.CurrentPlayState != MediaServer.PlayState.Stopped) 
			{
				client.mediaServer.Stop();
			} 
			else 
			{
				client.mediaServer.Play();
			}
		}

		public void FillMenuFromQueue(ContextMenu menu)
		{
			foreach (MediaListViewItem item in listViewQueue.Items)
			{
				string text = item.Text;
				if (!item.Entry.IsArtistNull() && item.Entry.Artist.Length > 0)
				{
					text = text + " [" + item.Entry.Artist + "]";
				}
                MediaListItemMenuItem menuItem = new MediaListItemMenuItem(text);
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

		private void buttonQueueRemove_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();

			foreach (MediaListViewItem item in new IterIsolate(listViewQueue.SelectedItems))
			{
				client.mediaServer.RemoveFromQueue(item.Entry.MediaId, item.Guid);
			}
		}

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("select MediaID from Media where Deleted = 0 and (");

			switch (comboBoxSearch.SelectedIndex)
			{
				case 0:		// any field
					sb.Append("Name like '%' + @SearchString + '%' ");
					sb.Append("or Artist like '%' + @SearchString + '%' ");
					sb.Append("or Album like '%' + @SearchString + '%' ");
					break;
				case 1:		// artist
					sb.Append("Artist like '%' + @SearchString + '%'");
					break;
				case 2:		// song name
					sb.Append("Name like '%' + @SearchString + '%'");
					break;
				case 3:		// album name
					sb.Append("Album like '%' + @SearchString + '%'");
					break;
				case 4:		// comments
					sb.Append("Comments like '%' + @SearchString + '%'");
					break;
			}
            sb.Append(") order by Album, Track, Artist, Name");

			SqlConnection cn = new SqlConnection(client.ConnectionString);
			SqlCommand cmd = new SqlCommand(sb.ToString(), cn);
			cmd.Parameters.Add("@SearchString", textBoxSearch.Text);

			listViewSearch.Items.Clear();

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read()) 
			{
				listViewSearch.Items.Add(new MediaListViewItem(this, client.FindMediaRow(Convert.ToInt32(dr[0]))));
			}
			dr.Close();
			cn.Close();

			textBoxSearch.SelectAll();
            textBoxSearch.Focus();
		}

		private void buttonQueueUp_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();

			MediaListViewItem item = (MediaListViewItem) listViewQueue.SelectedItems[0];

			client.mediaServer.MoveInQueue(item.Entry.MediaId, item.Guid, item.Index -1);
		}

		private void buttonQueueDown_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();

			MediaListViewItem item = (MediaListViewItem) listViewQueue.SelectedItems[0];

			client.mediaServer.MoveInQueue(item.Entry.MediaId, item.Guid, item.Index +1);
		}

		private void pictureBoxProgress_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pictureBoxContainer_MouseUp(sender, e);
		}

		private void pictureBoxContainer_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			SetWaitForMessage();
			double newPosition = ((double)(e.X) / (double)(pictureBoxContainer.Width-4));
			client.mediaServer.ChangePosition(newPosition);
		}

		private void timerPing_Tick(object sender, System.EventArgs e)
		{
			// make sure the connection stays alive
			AddToLog( "Ping?", "" );
			client.mediaServer.Ping();			

			double x = client.mediaServer.Duration;
			x++;
		}

		private void UMPlayer_Load(object sender, System.EventArgs e)
		{

			ConnectionDialog connection = new ConnectionDialog();

			if (connection.ShowDialog() == DialogResult.Cancel) 
			{
				Application.Exit();
				this.Dispose();
				return;
			}

			Status status = new Status("Connecting to " + connection.HostName + "...");

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

			status.Message = "Loading...";
			mediaBar = new MediaBar(this);
			mediaBar.Visible = true;
			statusBar1.Panels[0].Text = "Connected to " + connection.HostName;
			pictureBoxProgress.Width = 0;

			status.Message = "Loading queue...";
			ReloadQueue(this, e); 
			
			status.Message = "Loading songs...";
			InitialState();
			timerPing.Enabled = true;

			LoadPlaylists();

#if DEBUG
//            checkBoxShowLog.Checked = true;
#endif
			status.Hide();
			dateTimeStart.Value	= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0, 0));
			dateTimeEnd.Value	= DateTime.Now;
			comboBoxSearch.SelectedIndex = 0;


			int dif = 2; //panel1.Height / 255;

			Graphics gr = this.CreateGraphics();
			SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
			Pen pen = new Pen(brush, dif);
			Point pt1 = new Point(0, 0);
			Point pt2 = new Point(this.Width, 0);
			
			for (int i = 0; i < 255; i++) 
			{
				gr.DrawLine(pen, pt1, pt2);

				//				pt1.X = pt2.X;
				pt1.Y = pt1.Y + dif;
				
				//				pt2.X = pt2.X + dif;
				pt2.Y = pt2.Y + dif;

				brush.Color = Color.FromArgb(0, 0, i);
				pen = new Pen(brush, dif);

			}

		}

		private void pictureBoxVolume_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pictureBoxVolumeContainer_MouseUp(sender, e);
		}

		private void pictureBoxVolumeContainer_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			SetWaitForMessage();
			double newVolume = ((double)(e.X) / (double)(pictureBoxVolumeContainer.Width-4));
			client.mediaServer.Volume = newVolume;
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
				string sql = "update Media set Name = @Name, Artist = @Artist, Album = @Album, Track = @Track, Genre = @Genre, Comments = @Comments where MediaID = @MediaID";
				SqlCommand cmd = new SqlCommand(sql, cn);

				cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 250, "Name");
				cmd.Parameters.Add("@Artist", SqlDbType.NVarChar, 250, "Artist");
				cmd.Parameters.Add("@Album", SqlDbType.NVarChar, 250, "Album");
				cmd.Parameters.Add("@Track", SqlDbType.Int, 4, "Track");
				cmd.Parameters.Add("@Genre", SqlDbType.NVarChar, 250, "Genre");
				cmd.Parameters.Add("@Comments", SqlDbType.NVarChar, 250, "Comments");
				cmd.Parameters.Add("@MediaID", SqlDbType.Int, 4, "MediaID");

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

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{

			Status statusTemp = new Status("Validating files...", listViewFiles.Items.Count);
			ArrayList addFiles = new ArrayList();

			// Get the final list of files we can add
			foreach (ListViewItem item in listViewFiles.Items) 
			{
				
				string file		  = item.Tag.ToString();

				// make sure file isn't already in music collection
				if (!file.StartsWith(client.mediaServer.ShareDirectory)) 
				{
					// Check if the same file already exists in the music share
					DataView dv = new DataView(client.dsMedia.Media);
					dv.RowFilter = "MediaFile = '" + file.Replace("'", "''") + "'";
					if (dv.Count > 0)
					{
						string message = String.Format("The file '{0}' is already in the music collection.  This file will be skipped.  Click 'Cancel' to abort adding files.",
							file);
						if (MessageBox.Show(message, "File already exists", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
							return;
					} 
					else 
					{
						addFiles.Add(file);
					}
				} 
				else 
				{
					addFiles.Add(file);
				}

				statusTemp.Increment(1);
			}
			statusTemp.Hide();

			// Now start actually adding the files
			Status status = new Status("Adding files...", addFiles.Count);
			DataSetMedia dsMedia = new DataSetMedia();

			// Copy files if needed and get basic info on the files
			foreach (string fullpath in addFiles) 
			{
				string destPath = client.mediaServer.DropDirectory;
				if (!Directory.Exists(destPath))
					Directory.CreateDirectory(destPath);

				string destFile = "";
				destFile = fullpath.Substring(fullpath.LastIndexOf(Path.DirectorySeparatorChar)+1);

				// copy/move file only if not already in share path
				if (!fullpath.ToLower().StartsWith(client.mediaServer.ShareDirectory.ToLower()))
				{
                    int i = 0;

					string baseFile = destFile.Substring(0, destFile.LastIndexOf("."));
					string baseExtension = destFile.Substring(destFile.LastIndexOf("."));
                    
					// loop to find a filename we can use
					while (File.Exists(destPath + destFile))
					{
						i++;
                        destFile = baseFile + String.Format(" ({0})", i) + baseExtension;
					}

					// Now set the destFile to the whole thing
					destFile = destPath + Path.DirectorySeparatorChar + destFile;
					
                    // check if we should move (and therefore delete) the source file or simply copy
					if (checkBoxDeleteOnAdd.Checked)
					{
						File.Move(fullpath, destFile);
					}
					else
					{
						if (fullpath.ToLower().StartsWith(client.mediaServer.ShareDirectory))
						{
							File.Move(fullpath, destFile);
						}
						else 
						{
							File.Copy(fullpath, destFile);
						}
					}

					// now tell the server
					client.mediaServer.AddMediaFile(destFile);

				}
				// otherwise we need to tell the server there is a new file
				else
				{
					client.mediaServer.AddMediaFile(fullpath);
				}

				status.Increment(1);
				status.Refresh();
				this.Refresh();
			}

			// Remove entries that were added from the list
			foreach (string file in addFiles) 
			{
				foreach (ListViewItem item in listViewFiles.Items) 
				{
					if (item.Tag.ToString().Equals(file))
					{
						listViewFiles.Items.Remove(item);
						break;
					}
				}
			}

			status.Hide();

			MessageBox.Show(addFiles.Count.ToString() + " files have been added.", "Add Files", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
		}

		private void trackBarBalance_Scroll(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			client.mediaServer.Balance = trackBarBalance.Value;
		}

		private void trackBarRate_Scroll(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			client.mediaServer.Rate = ((double)trackBarRate.Value / 100.0);
		}

		private void buttonRemoveFile_Click(object sender, System.EventArgs e)
		{
			ArrayList removeList = new ArrayList();
			foreach (ListViewItem item in listViewFiles.SelectedItems) 
			{
				removeList.Add(item.Tag);
			}

			foreach (string file in removeList) 
			{
				foreach (ListViewItem item in listViewFiles.Items) 
				{
					if (item.Tag.Equals(file)) 
					{
						listViewFiles.Items.Remove(item);
						continue;
					}
				}
			}
		}

		private void buttonAddFile_Click(object sender, System.EventArgs e)
		{
			contextMenuAddFile.Show(buttonAddFile, new Point(buttonAddFile.Width/2, buttonAddFile.Height/2 ));
		}

		private void menuItemAddFile_Click(object sender, System.EventArgs e)
		{
			if (addFileDialog.ShowDialog() != DialogResult.Cancel) 
			{
				foreach (string file in addFileDialog.FileNames) 
				{
					AddFile(file);
				}
			}
		}

		private void menuItemAddDirectory_Click(object sender, System.EventArgs e)
		{
			AddDirectory(false);
		}

		private void menuItemAddTree_Click(object sender, System.EventArgs e)
		{
			AddDirectory(true);
		}

		private void AddDirectory(bool recursive) 
		{
			Shell32.ShellClass sh = new Shell32.ShellClass();

			Shell32.Folder2 folder = (Shell32.Folder2) sh.BrowseForFolder(0, "Select a folder", 0, null);

			if (folder != null) 
			{
				walkDirectory = folder.Self.Path;
				directoryRecursive = recursive;

				Thread thread = new Thread(new ThreadStart(WalkSubDirsThread));
                
				directoryAddStatus = new Status("Searching for files...", thread);
				thread.Start();
				
			}

		}

		private void AddFile(string file) 
		{
			bool found = false;
			string md5 = MediaUtilities.MD5ToString(MediaUtilities.MD5Hash(file));

			// check if already in list of files
			foreach (ListViewItem item in listViewFiles.Items) 
			{
				if (item.Tag.Equals(file)) 
				{
					found = true;
					break;
				}
			}

			// check if already in music collection
			DataView dv = new DataView(dsMedia.Media);
			dv.RowFilter = String.Format("MD5 = '{0}'", md5);
			if (dv.Count > 0) 
				found = true;

			// if not already in a list, add it
			if (!found) 
			{
				ListViewItem item = new ListViewItem(file);
				item.Tag = file;
				listViewFiles.Items.Add(item);
			}
		}

		private void WalkSubDirsThread() 
		{
			WalkSubDirs(walkDirectory, directoryRecursive);
			directoryAddStatus.Visible = false;
			directoryAddStatus = null;
		}

		private void WalkSubDirs(string path, bool recursive) 
		{
			try 
			{
				foreach (string file in Directory.GetFiles(path, "*.wma"))
				{
					AddFile(file);
				}
				foreach (string file in Directory.GetFiles(path, "*.mp3"))
				{
					AddFile(file);
				}

				if (recursive) 
				{
					foreach(string subDirectory in Directory.GetDirectories(path)) 
					{
						directoryAddStatus.Message = "Looking in " + path + "...";
						WalkSubDirs(subDirectory, true);
					}
				}
			} 
			catch (UnauthorizedAccessException) 
			{
				// ignore
			}

		}

		private void label4_DoubleClick(object sender, System.EventArgs e)
		{
			client.mediaServer.Balance = 0;
		}

		private void labelRate_DoubleClick(object sender, System.EventArgs e)
		{
			client.mediaServer.Rate = 1.0;
		}

		private void UMPlayer_Resize(object sender, System.EventArgs e)
		{
			CustomSizeControls();
		}

		private void CustomSizeControls() 
		{

			// Resize the file listview columns
			listViewFiles.Columns[0].Width = listViewFiles.Width - 22;

			listViewQueue.Columns[0].Width = (int)((listViewQueue.Width-22) * 0.30);
			listViewQueue.Columns[1].Width = (int)((listViewQueue.Width-22) * 0.25);
			listViewQueue.Columns[2].Width = (int)((listViewQueue.Width-22) * 0.25);
			listViewQueue.Columns[3].Width = (int)((listViewQueue.Width-22) * 0.10);
			listViewQueue.Columns[4].Width = (int)((listViewQueue.Width-22) * 0.10);

			listViewSearch.Columns[0].Width = (int)((listViewSearch.Width-22) * 0.30);
			listViewSearch.Columns[1].Width = (int)((listViewSearch.Width-22) * 0.25);
			listViewSearch.Columns[2].Width = (int)((listViewSearch.Width-22) * 0.25);
			listViewSearch.Columns[3].Width = (int)((listViewSearch.Width-22) * 0.10);
			listViewSearch.Columns[4].Width = (int)((listViewSearch.Width-22) * 0.10);

			listViewNew.Columns[0].Width = (int)((listViewNew.Width-22) * 0.30);
			listViewNew.Columns[1].Width = (int)((listViewNew.Width-22) * 0.25);
			listViewNew.Columns[2].Width = (int)((listViewNew.Width-22) * 0.25);
			listViewNew.Columns[3].Width = (int)((listViewNew.Width-22) * 0.10);
			listViewNew.Columns[4].Width = (int)((listViewNew.Width-22) * 0.10);

			listViewCollection.Columns[0].Width = (int)((listViewCollection.Width-22) * 0.30);
			listViewCollection.Columns[1].Width = (int)((listViewCollection.Width-22) * 0.25);
			listViewCollection.Columns[2].Width = (int)((listViewCollection.Width-22) * 0.25);
			listViewCollection.Columns[3].Width = (int)((listViewCollection.Width-22) * 0.10);
			listViewCollection.Columns[4].Width = (int)((listViewCollection.Width-22) * 0.10);

			listViewPlaylistMedia.Columns[0].Width = (int)((listViewPlaylistMedia.Width-22) * 0.30);
			listViewPlaylistMedia.Columns[1].Width = (int)((listViewPlaylistMedia.Width-22) * 0.25);
			listViewPlaylistMedia.Columns[2].Width = (int)((listViewPlaylistMedia.Width-22) * 0.25);
			listViewPlaylistMedia.Columns[3].Width = (int)((listViewPlaylistMedia.Width-22) * 0.10);
			listViewPlaylistMedia.Columns[4].Width = (int)((listViewPlaylistMedia.Width-22) * 0.10);

			listViewPlaylists.Columns[0].Width = (int)((listViewPlaylists.Width-22) * 0.50);
			listViewPlaylists.Columns[1].Width = (int)((listViewPlaylists.Width-22) * 0.15);
			listViewPlaylists.Columns[2].Width = (int)((listViewPlaylists.Width-22) * 0.15);
			listViewPlaylists.Columns[3].Width = (int)((listViewPlaylists.Width-22) * 0.10);
			listViewPlaylists.Columns[4].Width = (int)((listViewPlaylists.Width-22) * 0.10);
			
		}

		private void textBoxSearch_Enter(object sender, System.EventArgs e)
		{
			this.AcceptButton = buttonSearch;
		}

		private void textBoxSearch_Leave(object sender, System.EventArgs e)
		{
			this.AcceptButton = null;		
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// If the search tab, set focus in the search box
			if (tabControl1.SelectedIndex == 2) 
			{
				textBoxSearch.Focus();
			}
			CustomSizeControls();
		}

		private void treeViewCollection_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			SqlConnection cn = new SqlConnection(client.ConnectionString);

			// Check if we have to load children
			if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text.Equals("Loading")) 
			{
				e.Node.Nodes.Clear();
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = cn;

				if (e.Node.Text.Equals("By Artist")) 
				{
					cmd.CommandText = "select Artist from media where Artist <> '' group by Artist";
				} 
				else if (e.Node.Text.Equals("By Album")) 
				{
					cmd.CommandText = "select Album from media where Album <> '' group by Album";
				}
				else if (e.Node.Text.Equals("By Genre"))
				{
                    cmd.CommandText = "select Genre from media where Genre <> '' group by Genre";
				}
					// now check if an artist name clicked
				else if (e.Node is ArtistTreeNode) 
				{
					cmd.CommandText = "select Album from media where Album <> '' and Artist = @Artist group by Album";
					cmd.Parameters.Add("@Artist", e.Node.Text);
				}
                
				cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read()) 
				{
					if (e.Node.Text.Equals("By Artist")) 
					{
						ArtistTreeNode n = new ArtistTreeNode(dr[0].ToString());
						e.Node.Nodes.Add(n); 
						n.Nodes.Add("Loading");
					} 
					else if (e.Node.Text.Equals("By Album")) 
					{
						e.Node.Nodes.Add(new AlbumTreeNode(dr[0].ToString()));
					}
					else if (e.Node.Text.Equals("By Genre"))
					{
						e.Node.Nodes.Add(new GenreTreeNode(dr[0].ToString()));
					}
						// artist name clicked
					else if (e.Node is ArtistTreeNode) 
					{
						e.Node.Nodes.Add(new AlbumTreeNode(dr[0].ToString()));
					}
				}
				dr.Close();
				cn.Close();

			} 
		}

		private void treeViewCollection_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (e.Node is ArtistTreeNode) 
			{
				listViewCollection.Items.Clear();

				ArtistTreeNode artistNode = e.Node as ArtistTreeNode;

				DataView dv = new DataView(dsMedia.Media, "Artist = '" + artistNode.Artist.Replace("'", "''") + "'", "Name", DataViewRowState.CurrentRows);
				foreach (DataRowView rowView in dv) 
				{
					DataSetMedia.MediaRow row = (DataSetMedia.MediaRow) rowView.Row;
					DataSetMedia.MediaRow currentRow = client.FindMediaRow(row.MediaId);
					listViewCollection.Items.Add(new MediaListViewItem(this, currentRow));
				}

			} 
			else if (e.Node is AlbumTreeNode) 
			{
				listViewCollection.Items.Clear();

				AlbumTreeNode albumNode = e.Node as AlbumTreeNode;

				DataView dv = new DataView(dsMedia.Media, "Album = '" + albumNode.Album.Replace("'", "''") + "'", "Album", DataViewRowState.CurrentRows);
				dv.Sort = "Track";

				foreach (DataRowView rowView in dv) 
				{
					DataSetMedia.MediaRow row = (DataSetMedia.MediaRow) rowView.Row;
					DataSetMedia.MediaRow currentRow = client.FindMediaRow(row.MediaId);
					listViewCollection.Items.Add(new MediaListViewItem(this, currentRow));
				}

			}
			else if (e.Node is GenreTreeNode)
			{
				listViewCollection.Items.Clear();

				GenreTreeNode genreNode = e.Node as GenreTreeNode;

				DataView dv = new DataView(dsMedia.Media, "Genre = '" + genreNode.Genre + "'", "Genre", DataViewRowState.CurrentRows);
				dv.Sort = "Track";

				foreach (DataRowView rowView in dv) 
				{
					DataSetMedia.MediaRow row = (DataSetMedia.MediaRow) rowView.Row;
					listViewCollection.Items.Add(new MediaListViewItem(this, row));
				}
			}

		}

		private void SetWaitForMessage() 
		{
			Cursor.Current = Cursors.WaitCursor;
			mediaBar.Cursor = Cursors.WaitCursor;
		}

		private void ClearWaitForMessage() 
		{
			Cursor.Current = Cursors.Default;
			mediaBar.Cursor = Cursors.Default;
		}

		private void listViewQueue_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ( (e.KeyCode == Keys.R && e.Control) || e.KeyCode == Keys.Delete )
			{
				buttonQueueRemove_Click(this, EventArgs.Empty);
			}
		}


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

        public event MediaItemClientUpdateEventHandler MediaItemClientUpdateEvent;		

		public void MediaItemUpdate(object sender, MediaItemClientUpdateEventArgs e)
		{
            if (MediaItemClientUpdateEvent != null)
				MediaItemClientUpdateEvent(sender, e);

			// Check if we are playing this song
			if (e.MediaId == client.mediaServer.CurrentMediaId)
			{
				Playing(e.MediaId);
			}

			// we may want to add this row to the 'new' list
			if (e.Type == MediaItemUpdateType.Add)
				listViewNew.Items.Add(new MediaListViewItem(this, e.Entry));

		}

		private void buttonCheckMissingSongs_Click(object sender, System.EventArgs e)
		{
			Status status = new Status("Checking files...", dsMedia.Media.Rows.Count);

			bool remove = false;
			if (MessageBox.Show("Would you like to delete files from the database not found on disk?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				remove = true;

			SqlConnection cn = new SqlConnection(client.ConnectionString);
			SqlCommand cmd = new SqlCommand("dbo.s_DeleteMediaItem", cn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@MediaID", SqlDbType.Int);

			cn.Open();
			foreach (DataSetMedia.MediaRow row in new IterIsolate(client.dsMedia.Media))
			{
				if (!File.Exists(row.MediaFile))
				{
					AddToLog("Missing", String.Format("MediaID: {0}, File: {1}", row.MediaId, row.MediaFile));
                    
					if (remove)
					{
						cmd.Parameters["@MediaID"].Value = row.MediaId;
						cmd.ExecuteNonQuery();
						client.mediaServer.UpdateMediaItem(MediaItemUpdateType.Delete, row.MediaId);
					}
					
				}
				status.Increment(1);
				status.Refresh();
			}
			cn.Close();

			status.Hide();
		}

		private void dateTimeStart_ValueChanged(object sender, System.EventArgs e)
		{
			UpdateNewList();
		}

		private void UpdateNewList()
		{
			DataView dv = new DataView(client.dsMedia.Media);

			listViewNew.Items.Clear();

			dv.RowFilter = String.Format("DateAdded >= '{0}' And DateAdded <= '{1} 11:59 PM'",
				dateTimeStart.Value.ToShortDateString(), dateTimeEnd.Value.ToShortDateString());

			foreach (DataRowView rowView in dv)
			{
				DataSetMedia.MediaRow row = (DataSetMedia.MediaRow) rowView.Row;
				listViewNew.Items.Add(new MediaListViewItem(this, row));
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
				buttonPause_Click(this, EventArgs.Empty);
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

		private void labelOpacity_DoubleClick(object sender, System.EventArgs e)
		{
            this.Opacity = 1;
			trackBarOpacity.Value = 100;
		}

		private void trackBarOpacity_Scroll(object sender, System.EventArgs e)
		{
            this.Opacity = trackBarOpacity.Value/100.0;
		}

		private void LoadPlaylists()
		{
			SqlConnection cn = new SqlConnection(client.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("s_Playlist_List", cn);
			da.SelectCommand.CommandType = CommandType.StoredProcedure;

			listViewPlaylists.Items.Clear();

			DataSetPlaylist dsPlaylist = new DataSetPlaylist();
			da.Fill(dsPlaylist, "Playlist");
			
			foreach (DataSetPlaylist.PlaylistRow row in dsPlaylist.Playlist)
			{
                PlaylistListViewItem item = new PlaylistListViewItem(this, row);
				listViewPlaylists.Items.Add(item);
			}
            
            listViewPlaylistMedia.Items.Clear();
		}

		private void buttonPlaylistAdd_Click(object sender, System.EventArgs e)
		{
			PlaylistEditor ed = new PlaylistEditor();
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
				cn.Close();

				LoadPlaylists();
			}
		}

		private void buttonPlaylistDelete_Click(object sender, System.EventArgs e)
		{
			if (listViewPlaylists.SelectedItems.Count == 0)
			{
				MessageBox.Show("You must select a playlist to delete!", "Delete Playlist", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			PlaylistListViewItem item = (PlaylistListViewItem) listViewPlaylists.SelectedItems[0];

			string msg = String.Format("Are you sure you want to delete the playlist '{0}'?", item.Entry.PlaylistId);

			if (MessageBox.Show(msg, "Delete Playlist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				SqlConnection cn = new SqlConnection(client.ConnectionString);
				SqlCommand cmd   = new SqlCommand("s_Playlist_Delete", cn);
				cmd.CommandType  = CommandType.StoredProcedure;

				cmd.Parameters.Add("@PlaylistId", SqlDbType.Int);
				cmd.Parameters["@PlaylistId"].Value = item.Entry.PlaylistId;

				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();

				LoadPlaylists();
			}
		}

		private void listViewPlaylists_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (listViewPlaylists.SelectedItems.Count == 0)
				return;

			PlaylistListViewItem item = (PlaylistListViewItem) listViewPlaylists.SelectedItems[0];
			LoadPlaylistMedia(item.Entry.PlaylistId);
		}

		private void LoadPlaylistMedia(int playlistId)
		{
            // clear the curren tlist
			listViewPlaylistMedia.Items.Clear();
            
			SqlConnection cn = new SqlConnection(client.ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_PlaylistMedia_List", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PlaylistId", playlistId);

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				DataSetMedia.MediaRow row = client.FindMediaRow(Convert.ToInt32(dr["MediaId"]));
                PlaylistMediaListViewItem item = new PlaylistMediaListViewItem(this, row, Convert.ToInt32(dr["PlaylistMediaId"]));
                listViewPlaylistMedia.Items.Add(item);
			}
			dr.Close();
			cn.Close();

		}

		#region Media List Context Menus

		private void contextMenuMediaList_Popup(object sender, System.EventArgs e)
		{
			ContextMenu contextMenu = (ContextMenu) sender;
			ListView lv = (ListView) contextMenu.SourceControl;

			// First clear anything already in menu
			contextMenuMediaList.MenuItems.Clear();

			// Area specific menu items first
			if (lv == listViewQueue)
			{
				contextMenuMediaList.MenuItems.Add(new MenuItem("Move Up", new EventHandler(buttonQueueUp_Click)));
				contextMenuMediaList.MenuItems.Add(new MenuItem("Move Down", new EventHandler(buttonQueueDown_Click)));
				contextMenuMediaList.MenuItems.Add(new MenuItem("Remove from queue", new EventHandler(buttonQueueRemove_Click)));
				contextMenuMediaList.MenuItems.Add(new MenuItem("Clear Queue", new EventHandler(buttonClearQueue_Click)));
				contextMenuMediaList.MenuItems.Add(new MenuItem("Refresh", new EventHandler(ReloadQueue)));
				contextMenuMediaList.MenuItems.Add(new MenuItem("-"));
			}
			else if (lv == listViewPlaylistMedia)
			{
				contextMenuMediaList.MenuItems.Add(new MenuItem("Remove from playlist", new EventHandler(buttonPlaylistMediaRemove_Click)));
				contextMenuMediaList.MenuItems.Add(new MenuItem("-"));
			}

			// PLAY NOW
			MediaListItemMenuItem playNow = 
				new MediaListItemMenuItem("&Play Now", lv, new EventHandler(MediaList_PlayNow));
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
			foreach (MediaListViewItem item in listViewQueue.Items)
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
			foreach (PlaylistListViewItem item in listViewPlaylists.Items)
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

				playlistAfter.MenuItems.Add(new MenuItem("Loading..."));
			}

			// New
			MediaListItemMenuItem playlistNew = 
				new MediaListItemMenuItem("New...", lv, new EventHandler(MediaList_AddToPlaylist_New));
			addToPlaylist.MenuItems.Add(playlistNew);

			// BLANK
			MenuItem blankItem = new MenuItem("-");
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

		private void MediaList_QueueTop(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			SetWaitForMessage();

			// Add items in reverse order to top of queue
			foreach (MediaListViewItem item in new IterReverse(lv.SelectedItems)) 
			{
				client.mediaServer.AddToQueue(item.Entry.MediaId, 0);
			}

		}

		private void MediaList_QueueBottom(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			SetWaitForMessage();

			// Add items in order to bottom of queue
			foreach (MediaListViewItem item in lv.SelectedItems) 
			{
				client.mediaServer.AddToQueue(item.Entry.MediaId);
			}
			
		}

		private void MediaList_QueueAfter(object sender, System.EventArgs e)
		{
			// Figure out the listview
			MediaListItemMenuItem menuItem = (MediaListItemMenuItem) sender;
			ListView lv = menuItem.ListView;

			int offset = 1;
			foreach (MediaListViewItem item in lv.SelectedItems) 
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

			if (MessageBox.Show(msg, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) 
			{
				SqlConnection cn = new SqlConnection(client.ConnectionString);
				SqlCommand cmd = new SqlCommand("dbo.s_DeleteMediaItem", cn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@MediaID", SqlDbType.Int);

				cn.Open();
				foreach (MediaListViewItem item in new IterIsolate(lv.SelectedItems)) 
				{
					if (File.Exists(item.Entry.MediaFile))
						File.Delete(item.Entry.MediaFile);
					
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

				LoadPlaylists();

				// now find the added item playlist
				foreach (PlaylistListViewItem item in listViewPlaylists.Items)
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
			if (listViewPlaylists.SelectedItems.Count > 0)
			{
				selectedPlaylist = listViewPlaylists.SelectedIndices[0];
			}
			LoadPlaylists();

			if (selectedPlaylist > 0)
			{
				listViewPlaylists.Items[selectedPlaylist].Selected = true;
			}

			// Check if we need to reload playlist
			if (listViewPlaylists.SelectedItems.Count > 0)
			{
				PlaylistListViewItem item = (PlaylistListViewItem) listViewPlaylists.SelectedItems[0];
				if (item.Entry.PlaylistId == playlistEntry.PlaylistId)
					LoadPlaylistMedia(item.Entry.PlaylistId);
			}

		}

		#endregion

		private void buttonPlaylistMediaRemove_Click(object sender, System.EventArgs e)
		{
			if (listViewPlaylistMedia.SelectedItems.Count == 0)
			{
				MessageBox.Show("You must select items to remove.", "Remove Playlist Items", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Get the current playlist id
			PlaylistListViewItem playlistItem = (PlaylistListViewItem) listViewPlaylists.SelectedItems[0];

			SqlConnection cn = new SqlConnection(client.ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_PlaylistMedia_Remove", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PlaylistMediaId", SqlDbType.Int);

			cn.Open();
			foreach (PlaylistMediaListViewItem item in new IterIsolate(listViewPlaylistMedia.SelectedItems))
			{
                cmd.Parameters["@PlaylistMediaId"].Value = item.PlaylistMediaId;                
				cmd.ExecuteNonQuery();
				listViewPlaylistMedia.Items.Remove(item);
			}
			cn.Close();

		}

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
				FillMenuFromQueue(contextMenuQuickList);

				foreach (MenuItem item in contextMenuQuickList.MenuItems)
				{
					item.Click += new EventHandler(AdvanceToSong);
				}

				contextMenuQuickList.Show(buttonNext, buttonNext.PointToClient(MousePosition));
			}
		}

		private void AdvanceToSong(object sender, System.EventArgs e)
		{
			MediaListItemMenuItem item = (MediaListItemMenuItem) sender;
			client.mediaServer.AdvanceToSong(item.MediaRow.MediaId);
		}

		private void buttonClearQueue_Click(object sender, System.EventArgs e)
		{
			foreach (MediaListViewItem item in new IterIsolate(listViewQueue.Items))
			{
				client.mediaServer.RemoveFromQueue(item.Entry.MediaId, item.Guid);
			}
		}

		private void listViewPlaylists_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ContextMenu contextMenu = new ContextMenu();
		
				contextMenu.MenuItems.Add(new MenuItem("Play", new EventHandler(Playlist_PlayNow)));
				contextMenu.MenuItems.Add(new MenuItem("Delete", new EventHandler(buttonPlaylistDelete_Click)));
				
                contextMenu.Show(listViewPlaylists, listViewPlaylists.PointToClient(MousePosition));
			}
		}

		private void Playlist_PlayNow(object sender, EventArgs e)
		{
			int offset = 0;

			foreach (PlaylistMediaListViewItem item in listViewPlaylistMedia.Items)
			{
                client.mediaServer.AddToQueue(item.Entry.MediaId, offset);
				offset++;
			}
			client.mediaServer.Next();
		}

	}

	#region Media Item client updates

	public delegate void MediaItemClientUpdateEventHandler(object sender, MediaItemClientUpdateEventArgs e);

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

	internal class MediaListItemMenuItem: MenuItem
	{
		private ListView lv = null;
		private DataSetMedia.MediaRow mediaRow = null;
        private DataSetPlaylist.PlaylistRow playlistRow = null;		
		
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
				SubItems.Add(MediaUtilities.DurationToString(entry.TotalDuration));
            
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

	internal class MediaListViewItem: ListViewItem 
	{
		private DataSetMedia.MediaRow entry;
		private UMPlayer player;
		private Guid guid;

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
			this.entry  = entry;
			this.player = player;
			this.guid   = guid;

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
			SubItems.Add(MediaUtilities.DurationToString(entry.Duration));

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
		public void MediaItemUpdateEvent(object sender, MediaItemClientUpdateEventArgs e)
		{
			// check if this item is the current item
			if (e.MediaId == entry.MediaId)
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
