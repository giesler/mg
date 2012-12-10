using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using UMServer;
using System.Threading;
using Crownwood.Magic;
using Crownwood.Magic.Docking;

namespace UMClient
{
	/// <summary>
	/// Summary description for UMPlayer.
	/// </summary>
	public class UMPlayer : System.Windows.Forms.Form
	{
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
		private System.Windows.Forms.ContextMenu contextMenuSearchItems;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.MenuItem menuSearchAddToQueue;
		private System.Windows.Forms.MenuItem menuSearchTopOfQueue;
		private System.Windows.Forms.MenuItem menuSearchBottomOfQueue;
		private System.Windows.Forms.ContextMenu contextMenuQueueItems;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.PictureBox pictureBoxContainer;
		private System.Windows.Forms.PictureBox pictureBoxProgress;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TreeView treeViewCollection;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ListView listViewCollection;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.MenuItem menuSearchQueueAfter;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.Timer timerPing;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBoxVolume;
		private System.Windows.Forms.PictureBox pictureBoxVolumeContainer;
		private Client client;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.MenuItem menuQueueEditInfo;
		private System.Windows.Forms.MenuItem menuSearchEditInfo;
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
		private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.CheckBox checkBoxShowLog;
		private System.Windows.Forms.TextBox textBoxLog;
		private Status directoryAddStatus = null;
		private System.Windows.Forms.ContextMenu contextMenuMediaCollection;
		private System.Windows.Forms.MenuItem menuItemColPlayNow;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItemColEditInfo;
		private System.Windows.Forms.MenuItem menuItemColQueueTop;
		private System.Windows.Forms.MenuItem menuItemColQueueBottom;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuColQueueAfter;
		private System.Windows.Forms.MenuItem menuItemColDelete;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader13;
		private System.Windows.Forms.ColumnHeader columnHeader14;
		private System.Windows.Forms.ColumnHeader columnHeader15;
		private System.Windows.Forms.ColumnHeader columnHeader16;
		private DataSetMedia dsMedia;

		private IDockingManager dockManager;
		private Content editorContent;
		private System.Windows.Forms.Button buttonCheckMissingSongs;
		private System.Windows.Forms.CheckBox checkBoxDeleteOnAdd;
		private System.Windows.Forms.MenuItem menuSearchDelete;
		private System.Windows.Forms.MenuItem menuQueueDelete;
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
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuSearchPlayNow;
		private System.Windows.Forms.ContextMenu contextMenuNew;
		private System.Windows.Forms.MenuItem menuNewPlayNow;
		private System.Windows.Forms.MenuItem menuNewQueueAfter;
		private System.Windows.Forms.MenuItem menuNewQueueTop;
		private System.Windows.Forms.MenuItem menuNewQueueBottom;
		private System.Windows.Forms.MenuItem menuNewEditInfo;
		private System.Windows.Forms.MenuItem menuNewDelete;
		private System.Windows.Forms.Button buttonPrevious;
		private System.Windows.Forms.Button button1;
		private MediaEditor editor;

		/// <summary>
		/// Creates a new UMPlayer object
		/// </summary>
		public UMPlayer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			dockManager = new Crownwood.Magic.Docking.DockingManagerIDE(this);

			// Create the edit form and bar
			//			editor = new MediaEditor();
			//			editorContent = new Content(editor, "Media Info");
			//			editorContent.DockingMinimumSize	= new Size(200, 0);
			//			editorContent.DockingSize			= new Size(300, 0);
			//			dockManager.AddSingleContent(editorContent, State.DockRight);
			
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
			SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
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
			DataSetMedia.MediaRow row = client.FindMediaRow(mediaId);

			
			string mediaFileInfo = row.Artist + " - " + row.Name + "  [ID: " + row.MediaId.ToString() + "]";
			//			MediaError error = new MediaError(errorDescription, mediaFileInfo, row.MediaFile);

			textBoxLog.Text = textBoxLog.Text + DateTime.Now.ToShortTimeString() + ": " + errorDescription + Convert.ToChar(13) + Convert.ToChar(10);
			textBoxLog.Text = textBoxLog.Text + mediaFileInfo + Convert.ToChar(13) + Convert.ToChar(10);

			//			this.AddOwnedForm(error);
			
			//			error.Visible = true;
			//			error.Refresh();
		}

		#endregion

		#region Queue handlers

		/// <summary>
		/// Add the item to the queue
		/// </summary>
		/// <param name="mediaId">Media item to add</param>
		/// <param name="position">Position in the queue</param>
		public void AddToQueue(int mediaId, int position) 
		{
			DataSetMedia.MediaRow row = client.FindMediaRow(mediaId);
			MediaListViewItem item = new MediaListViewItem(this, row);

			// see if we should add at end or insert in queue
			if (position >= listViewQueue.Items.Count) 
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
		public void RemoveFromQueue(int mediaId, int position) 
		{	
			MediaListViewItem item = (MediaListViewItem) listViewQueue.Items[position];

			if (item.MediaEntry.MediaId == mediaId) 
			{
				listViewQueue.Items.Remove(item);
			} 
			else 
			{
				ShowError("The item in the queue at position " + position.ToString() + " did not match the item that should be deleted.  Reloading queue.", mediaId);
				ReloadQueue();
			}

			ClearWaitForMessage();
		}

		/// <summary>
		/// Move an item in the queue
		/// </summary>
		/// <param name="mediaId">Item to move</param>
		/// <param name="position">Position of old item</param>
		/// <param name="newPosition"></param>
		public void MovedInQueue(int mediaId, int position, int newPosition) 
		{
			RemoveFromQueue(mediaId, position);
			AddToQueue(mediaId, newPosition);

			listViewQueue.Items[newPosition].Selected = true;

			ClearWaitForMessage();
		}

		/// <summary>
		/// Reload the queue from the server
		/// </summary>
		public void ReloadQueue() 
		{
			ArrayList queue = client.mediaServer.CurrentQueue();

			// Clear the current queue
			listViewQueue.Items.Clear();

			// Loop through adding listviewitems
			foreach(int mediaId in queue) 
			{
				listViewQueue.Items.Add(new MediaListViewItem(this, client.FindMediaRow(mediaId)));
			}

			ClearWaitForMessage();
		}

		#endregion
		
		/// <summary>
		/// Add info to the textbox log
		/// </summary>
		/// <param name="function">Place event occurred</param>
		/// <param name="message">Event details</param>
		public void AddToLog(string function, string message) 
		{
			if (checkBoxShowLog.Checked) 
			{
				textBoxLog.Text = textBoxLog.Text + System.DateTime.Now.ToLongTimeString() + ": "
					+ function + ":  " + message + Convert.ToChar(13) + Convert.ToChar(10);
				textBoxLog.SelectionStart = textBoxLog.Text.Length;
			}
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
			this.contextMenuQueueItems = new System.Windows.Forms.ContextMenu();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuQueueEditInfo = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuQueueDelete = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.listViewSearch = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.contextMenuSearchItems = new System.Windows.Forms.ContextMenu();
			this.menuSearchPlayNow = new System.Windows.Forms.MenuItem();
			this.menuSearchAddToQueue = new System.Windows.Forms.MenuItem();
			this.menuSearchTopOfQueue = new System.Windows.Forms.MenuItem();
			this.menuSearchBottomOfQueue = new System.Windows.Forms.MenuItem();
			this.menuSearchQueueAfter = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuSearchEditInfo = new System.Windows.Forms.MenuItem();
			this.menuSearchDelete = new System.Windows.Forms.MenuItem();
			this.buttonSearch = new System.Windows.Forms.Button();
			this.textBoxSearch = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.listViewCollection = new System.Windows.Forms.ListView();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.contextMenuMediaCollection = new System.Windows.Forms.ContextMenu();
			this.menuItemColPlayNow = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItemColQueueTop = new System.Windows.Forms.MenuItem();
			this.menuItemColQueueBottom = new System.Windows.Forms.MenuItem();
			this.menuColQueueAfter = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItemColEditInfo = new System.Windows.Forms.MenuItem();
			this.menuItemColDelete = new System.Windows.Forms.MenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.treeViewCollection = new System.Windows.Forms.TreeView();
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
			this.contextMenuNew = new System.Windows.Forms.ContextMenu();
			this.menuNewPlayNow = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuNewQueueTop = new System.Windows.Forms.MenuItem();
			this.menuNewQueueBottom = new System.Windows.Forms.MenuItem();
			this.menuNewQueueAfter = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.menuNewEditInfo = new System.Windows.Forms.MenuItem();
			this.menuNewDelete = new System.Windows.Forms.MenuItem();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.listViewFiles = new System.Windows.Forms.ListView();
			this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
			this.buttonRemoveFile = new System.Windows.Forms.Button();
			this.buttonAddFile = new System.Windows.Forms.Button();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.checkBoxDeleteOnAdd = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tabPage7 = new System.Windows.Forms.TabPage();
			this.buttonCheckMissingSongs = new System.Windows.Forms.Button();
			this.checkBoxShowLog = new System.Windows.Forms.CheckBox();
			this.textBoxLog = new System.Windows.Forms.TextBox();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.labelRate = new System.Windows.Forms.Label();
			this.trackBarRate = new System.Windows.Forms.TrackBar();
			this.label4 = new System.Windows.Forms.Label();
			this.trackBarBalance = new System.Windows.Forms.TrackBar();
			this.timerPaused = new System.Windows.Forms.Timer(this.components);
			this.timerPing = new System.Windows.Forms.Timer(this.components);
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.addFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.contextMenuAddFile = new System.Windows.Forms.ContextMenu();
			this.menuItemAddFile = new System.Windows.Forms.MenuItem();
			this.menuItemAddDirectory = new System.Windows.Forms.MenuItem();
			this.menuItemAddTree = new System.Windows.Forms.MenuItem();
			this.button1 = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.tabPage7.SuspendLayout();
			this.tabPage6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarRate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBalance)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.pictureBoxProgress,
																				 this.panel2,
																				 this.labelName,
																				 this.labelArtist,
																				 this.pictureBoxContainer});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(512, 80);
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
			this.pictureBoxProgress.Size = new System.Drawing.Size(80, 4);
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
			this.panel2.Location = new System.Drawing.Point(352, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(160, 80);
			this.panel2.TabIndex = 2;
			// 
			// buttonPrevious
			// 
			this.buttonPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPrevious.Location = new System.Drawing.Point(8, 48);
			this.buttonPrevious.Name = "buttonPrevious";
			this.buttonPrevious.Size = new System.Drawing.Size(32, 16);
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
			this.buttonNext.Size = new System.Drawing.Size(32, 16);
			this.buttonNext.TabIndex = 2;
			this.buttonNext.Text = "> >";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonPause
			// 
			this.buttonPause.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPause.Location = new System.Drawing.Point(80, 48);
			this.buttonPause.Name = "buttonPause";
			this.buttonPause.Size = new System.Drawing.Size(32, 16);
			this.buttonPause.TabIndex = 1;
			this.buttonPause.Text = "| |";
			this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
			// 
			// buttonPlayStop
			// 
			this.buttonPlayStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlayStop.Location = new System.Drawing.Point(48, 48);
			this.buttonPlayStop.Name = "buttonPlayStop";
			this.buttonPlayStop.Size = new System.Drawing.Size(32, 16);
			this.buttonPlayStop.TabIndex = 0;
			this.buttonPlayStop.Text = ">";
			this.buttonPlayStop.Click += new System.EventHandler(this.buttonPlayStop_Click);
			// 
			// label2
			// 
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
			this.labelName.Size = new System.Drawing.Size(336, 32);
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
			this.labelArtist.Size = new System.Drawing.Size(336, 23);
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
			this.pictureBoxContainer.Size = new System.Drawing.Size(336, 8);
			this.pictureBoxContainer.TabIndex = 3;
			this.pictureBoxContainer.TabStop = false;
			this.pictureBoxContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxContainer_MouseUp);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 344);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.statusBarPanel1});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(512, 22);
			this.statusBar1.TabIndex = 1;
			this.statusBar1.Visible = false;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Width = 496;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.tabPage1,
																					  this.tabPage3,
																					  this.tabPage2,
																					  this.tabPage4,
																					  this.tabPage5,
																					  this.tabPage7,
																					  this.tabPage6});
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 80);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(512, 264);
			this.tabControl1.TabIndex = 2;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.buttonQueueDown,
																				   this.buttonQueueUp,
																				   this.buttonQueueRemove,
																				   this.listViewQueue});
			this.tabPage1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(504, 238);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Queue";
			// 
			// buttonQueueDown
			// 
			this.buttonQueueDown.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonQueueDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonQueueDown.Location = new System.Drawing.Point(464, 208);
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
			this.buttonQueueUp.Location = new System.Drawing.Point(464, 176);
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
			this.buttonQueueRemove.Location = new System.Drawing.Point(464, 8);
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
			this.listViewQueue.ContextMenu = this.contextMenuQueueItems;
			this.listViewQueue.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewQueue.FullRowSelect = true;
			this.listViewQueue.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewQueue.HideSelection = false;
			this.listViewQueue.Name = "listViewQueue";
			this.listViewQueue.Size = new System.Drawing.Size(456, 238);
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
			// contextMenuQueueItems
			// 
			this.contextMenuQueueItems.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								  this.menuItem3,
																								  this.menuQueueEditInfo,
																								  this.menuItem5,
																								  this.menuQueueDelete,
																								  this.menuItem4,
																								  this.menuItem2});
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 0;
			this.menuItem3.Text = "&Play Now";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuQueueEditInfo
			// 
			this.menuQueueEditInfo.Index = 1;
			this.menuQueueEditInfo.Text = "&Edit Info";
			this.menuQueueEditInfo.Click += new System.EventHandler(this.menuQueueEditInfo_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 2;
			this.menuItem5.Text = "&Remove";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuQueueDelete
			// 
			this.menuQueueDelete.Index = 3;
			this.menuQueueDelete.Text = "&Delete";
			this.menuQueueDelete.Click += new System.EventHandler(this.menuQueueDelete_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 4;
			this.menuItem4.Text = "-";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 5;
			this.menuItem2.Text = "&Refresh";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// tabPage4
			// 
			this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage4.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.listViewSearch,
																				   this.buttonSearch,
																				   this.textBoxSearch,
																				   this.label1});
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(504, 238);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Search";
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
			this.listViewSearch.ContextMenu = this.contextMenuSearchItems;
			this.listViewSearch.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewSearch.FullRowSelect = true;
			this.listViewSearch.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewSearch.HideSelection = false;
			this.listViewSearch.Location = new System.Drawing.Point(0, 32);
			this.listViewSearch.Name = "listViewSearch";
			this.listViewSearch.Size = new System.Drawing.Size(496, 200);
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
			// contextMenuSearchItems
			// 
			this.contextMenuSearchItems.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								   this.menuSearchPlayNow,
																								   this.menuSearchAddToQueue,
																								   this.menuSearchEditInfo,
																								   this.menuSearchDelete});
			// 
			// menuSearchPlayNow
			// 
			this.menuSearchPlayNow.Index = 0;
			this.menuSearchPlayNow.Text = "&Play Now";
			this.menuSearchPlayNow.Click += new System.EventHandler(this.menuSearchPlayNow_Click);
			// 
			// menuSearchAddToQueue
			// 
			this.menuSearchAddToQueue.Index = 1;
			this.menuSearchAddToQueue.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								 this.menuSearchTopOfQueue,
																								 this.menuSearchBottomOfQueue,
																								 this.menuSearchQueueAfter});
			this.menuSearchAddToQueue.Text = "&Add to Queue";
			// 
			// menuSearchTopOfQueue
			// 
			this.menuSearchTopOfQueue.Index = 0;
			this.menuSearchTopOfQueue.Text = "&Top of Queue";
			this.menuSearchTopOfQueue.Click += new System.EventHandler(this.menuSearchTopOfQueue_Click);
			// 
			// menuSearchBottomOfQueue
			// 
			this.menuSearchBottomOfQueue.Index = 1;
			this.menuSearchBottomOfQueue.Text = "&Bottom of Queue";
			this.menuSearchBottomOfQueue.Click += new System.EventHandler(this.menuSearchBottomOfQueue_Click);
			// 
			// menuSearchQueueAfter
			// 
			this.menuSearchQueueAfter.Index = 2;
			this.menuSearchQueueAfter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								 this.menuItem6});
			this.menuSearchQueueAfter.Text = "After...";
			this.menuSearchQueueAfter.Popup += new System.EventHandler(this.menuSearchQueueAfter_Popup);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 0;
			this.menuItem6.Text = "Song List";
			// 
			// menuSearchEditInfo
			// 
			this.menuSearchEditInfo.Index = 2;
			this.menuSearchEditInfo.Text = "&Edit Info";
			this.menuSearchEditInfo.Click += new System.EventHandler(this.menuSearchEditInfo_Click);
			// 
			// menuSearchDelete
			// 
			this.menuSearchDelete.Index = 3;
			this.menuSearchDelete.Text = "&Delete";
			this.menuSearchDelete.Click += new System.EventHandler(this.menuSearchDelete_Click);
			// 
			// buttonSearch
			// 
			this.buttonSearch.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonSearch.Location = new System.Drawing.Point(440, 8);
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
			this.textBoxSearch.Size = new System.Drawing.Size(360, 20);
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
			// tabPage2
			// 
			this.tabPage2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.listViewCollection,
																				   this.splitter1,
																				   this.treeViewCollection});
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(504, 238);
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
			this.listViewCollection.ContextMenu = this.contextMenuMediaCollection;
			this.listViewCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewCollection.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewCollection.FullRowSelect = true;
			this.listViewCollection.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewCollection.HideSelection = false;
			this.listViewCollection.Location = new System.Drawing.Point(195, 0);
			this.listViewCollection.Name = "listViewCollection";
			this.listViewCollection.Size = new System.Drawing.Size(309, 238);
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
			// contextMenuMediaCollection
			// 
			this.contextMenuMediaCollection.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																									   this.menuItemColPlayNow,
																									   this.menuItem8,
																									   this.menuItemColEditInfo,
																									   this.menuItemColDelete});
			// 
			// menuItemColPlayNow
			// 
			this.menuItemColPlayNow.Index = 0;
			this.menuItemColPlayNow.Text = "&Play Now";
			this.menuItemColPlayNow.Click += new System.EventHandler(this.menuItemColPlayNow_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemColQueueTop,
																					  this.menuItemColQueueBottom,
																					  this.menuColQueueAfter});
			this.menuItem8.Text = "&Add To Queue";
			// 
			// menuItemColQueueTop
			// 
			this.menuItemColQueueTop.Index = 0;
			this.menuItemColQueueTop.Text = "&Top of Queue";
			this.menuItemColQueueTop.Click += new System.EventHandler(this.menuItemColQueueTop_Click);
			// 
			// menuItemColQueueBottom
			// 
			this.menuItemColQueueBottom.Index = 1;
			this.menuItemColQueueBottom.Text = "&Bottom of Queue";
			this.menuItemColQueueBottom.Click += new System.EventHandler(this.menuItemColQueueBottom_Click);
			// 
			// menuColQueueAfter
			// 
			this.menuColQueueAfter.Index = 2;
			this.menuColQueueAfter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							  this.menuItem9});
			this.menuColQueueAfter.Text = "&After";
			this.menuColQueueAfter.Popup += new System.EventHandler(this.menuColQueueAfter_Popup);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 0;
			this.menuItem9.Text = "Song list";
			// 
			// menuItemColEditInfo
			// 
			this.menuItemColEditInfo.Index = 2;
			this.menuItemColEditInfo.Text = "&Edit Info";
			this.menuItemColEditInfo.Click += new System.EventHandler(this.menuItemColEditInfo_Click);
			// 
			// menuItemColDelete
			// 
			this.menuItemColDelete.Index = 3;
			this.menuItemColDelete.Text = "&Delete";
			this.menuItemColDelete.Click += new System.EventHandler(this.menuItemColDelete_Click);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(192, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 238);
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
			this.treeViewCollection.Size = new System.Drawing.Size(192, 238);
			this.treeViewCollection.TabIndex = 0;
			this.treeViewCollection.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCollection_AfterSelect);
			this.treeViewCollection.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewCollection_BeforeExpand);
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
			this.tabPage3.Size = new System.Drawing.Size(504, 238);
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
			this.listViewNew.ContextMenu = this.contextMenuNew;
			this.listViewNew.ForeColor = System.Drawing.SystemColors.ControlText;
			this.listViewNew.FullRowSelect = true;
			this.listViewNew.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewNew.HideSelection = false;
			this.listViewNew.Location = new System.Drawing.Point(0, 40);
			this.listViewNew.Name = "listViewNew";
			this.listViewNew.Size = new System.Drawing.Size(496, 200);
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
			// contextMenuNew
			// 
			this.contextMenuNew.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.menuNewPlayNow,
																						   this.menuItem10,
																						   this.menuNewEditInfo,
																						   this.menuNewDelete});
			// 
			// menuNewPlayNow
			// 
			this.menuNewPlayNow.Index = 0;
			this.menuNewPlayNow.Text = "&Play Now";
			this.menuNewPlayNow.Click += new System.EventHandler(this.menuNewPlayNow_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 1;
			this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuNewQueueTop,
																					   this.menuNewQueueBottom,
																					   this.menuNewQueueAfter});
			this.menuItem10.Text = "&Add To Queue";
			// 
			// menuNewQueueTop
			// 
			this.menuNewQueueTop.Index = 0;
			this.menuNewQueueTop.Text = "&Top of Qeueue";
			this.menuNewQueueTop.Click += new System.EventHandler(this.menuNewQueueTop_Click);
			// 
			// menuNewQueueBottom
			// 
			this.menuNewQueueBottom.Index = 1;
			this.menuNewQueueBottom.Text = "&Bottom of Queue";
			this.menuNewQueueBottom.Click += new System.EventHandler(this.menuNewQueueBottom_Click);
			// 
			// menuNewQueueAfter
			// 
			this.menuNewQueueAfter.Index = 2;
			this.menuNewQueueAfter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							  this.menuItem16});
			this.menuNewQueueAfter.Text = "&After";
			this.menuNewQueueAfter.Popup += new System.EventHandler(this.menuNewQueueAfter_Popup);
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 0;
			this.menuItem16.Text = "Song List";
			// 
			// menuNewEditInfo
			// 
			this.menuNewEditInfo.Index = 2;
			this.menuNewEditInfo.Text = "&Edit Info";
			this.menuNewEditInfo.Click += new System.EventHandler(this.menuNewEditInfo_Click);
			// 
			// menuNewDelete
			// 
			this.menuNewDelete.Index = 3;
			this.menuNewDelete.Text = "&Delete";
			this.menuNewDelete.Click += new System.EventHandler(this.menuNewDelete_Click);
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
			this.tabPage5.Size = new System.Drawing.Size(504, 238);
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
			this.listViewFiles.Size = new System.Drawing.Size(456, 176);
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
			this.buttonRemoveFile.Location = new System.Drawing.Point(472, 64);
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
			this.buttonAddFile.Location = new System.Drawing.Point(472, 32);
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
			this.buttonAdd.Location = new System.Drawing.Point(416, 208);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.TabIndex = 5;
			this.buttonAdd.Text = "&Add";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// checkBoxDeleteOnAdd
			// 
			this.checkBoxDeleteOnAdd.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.checkBoxDeleteOnAdd.Location = new System.Drawing.Point(8, 208);
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
			// tabPage7
			// 
			this.tabPage7.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage7.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.buttonCheckMissingSongs,
																				   this.checkBoxShowLog,
																				   this.textBoxLog});
			this.tabPage7.Location = new System.Drawing.Point(4, 22);
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.Size = new System.Drawing.Size(504, 238);
			this.tabPage7.TabIndex = 7;
			this.tabPage7.Text = "Log";
			// 
			// buttonCheckMissingSongs
			// 
			this.buttonCheckMissingSongs.Location = new System.Drawing.Point(408, 8);
			this.buttonCheckMissingSongs.Name = "buttonCheckMissingSongs";
			this.buttonCheckMissingSongs.Size = new System.Drawing.Size(80, 16);
			this.buttonCheckMissingSongs.TabIndex = 4;
			this.buttonCheckMissingSongs.Text = "check...";
			this.buttonCheckMissingSongs.Click += new System.EventHandler(this.buttonCheckMissingSongs_Click);
			// 
			// checkBoxShowLog
			// 
			this.checkBoxShowLog.Location = new System.Drawing.Point(8, 7);
			this.checkBoxShowLog.Name = "checkBoxShowLog";
			this.checkBoxShowLog.Size = new System.Drawing.Size(488, 24);
			this.checkBoxShowLog.TabIndex = 3;
			this.checkBoxShowLog.Text = "&Show Activity";
			this.checkBoxShowLog.CheckedChanged += new System.EventHandler(this.checkBoxShowLog_CheckedChanged);
			// 
			// textBoxLog
			// 
			this.textBoxLog.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxLog.BackColor = System.Drawing.SystemColors.Window;
			this.textBoxLog.ForeColor = System.Drawing.SystemColors.ControlText;
			this.textBoxLog.Location = new System.Drawing.Point(8, 31);
			this.textBoxLog.Multiline = true;
			this.textBoxLog.Name = "textBoxLog";
			this.textBoxLog.ReadOnly = true;
			this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxLog.Size = new System.Drawing.Size(488, 200);
			this.textBoxLog.TabIndex = 2;
			this.textBoxLog.Text = "";
			// 
			// tabPage6
			// 
			this.tabPage6.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage6.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.button1,
																				   this.labelRate,
																				   this.trackBarRate,
																				   this.label4,
																				   this.trackBarBalance});
			this.tabPage6.Location = new System.Drawing.Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Size = new System.Drawing.Size(504, 238);
			this.tabPage6.TabIndex = 6;
			this.tabPage6.Text = "More";
			// 
			// labelRate
			// 
			this.labelRate.Location = new System.Drawing.Point(32, 104);
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
			this.trackBarRate.Location = new System.Drawing.Point(24, 128);
			this.trackBarRate.Maximum = 200;
			this.trackBarRate.Minimum = 1;
			this.trackBarRate.Name = "trackBarRate";
			this.trackBarRate.Size = new System.Drawing.Size(456, 45);
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
			this.trackBarBalance.Size = new System.Drawing.Size(456, 45);
			this.trackBarBalance.SmallChange = 100;
			this.trackBarBalance.TabIndex = 0;
			this.trackBarBalance.TickFrequency = 1000;
			this.trackBarBalance.Scroll += new System.EventHandler(this.trackBarBalance_Scroll);
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
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(384, 200);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(64, 24);
			this.button1.TabIndex = 4;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// UMPlayer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 366);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1,
																		  this.statusBar1,
																		  this.panel1});
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
			this.tabPage4.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.tabPage7.ResumeLayout(false);
			this.tabPage6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBarRate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBalance)).EndInit();
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

		public void Next() 
		{
			SetWaitForMessage();
			buttonNext_Click(this, EventArgs.Empty);
		}

		public void Previous()
		{
            SetWaitForMessage();
			buttonPrevious_Click(this, EventArgs.Empty);
		}

		private void buttonNext_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			client.mediaServer.Next();
		}

		private void buttonQueueRemove_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();

			ArrayList removeList = new ArrayList();

			foreach (MediaListViewItem item in listViewQueue.SelectedItems) 
			{
				removeList.Add(item.MediaEntry.MediaId);
			}

			for (int i = removeList.Count-1; i >= 0; i--) 
			{
				int mediaId = Convert.ToInt32(removeList[i]);
				int index   = -1;
				// find the item again - we need the index
				foreach (MediaListViewItem item in listViewQueue.SelectedItems) 
				{
					if (item.MediaEntry.MediaId == mediaId) 
					{
						index = item.Index;
					}
				}
				client.mediaServer.RemoveFromQueue(mediaId, index);
			}
		}

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
			string sql = "select MediaID from Media where Deleted = 0 and "
				+ "(Name like '%' + @SearchString + '%' or "
				+ "Artist like '%' + @SearchString + '%' or "
				+ "Album like '%' + @SearchString + '%') order by Album, Track, Artist, Name";
			SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
			SqlCommand cmd = new SqlCommand(sql, cn);
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

		private void menuSearchTopOfQueue_Click(object sender, System.EventArgs e)
		{
			AddToQueue(listViewSearch, true);
		}

		private void menuSearchBottomOfQueue_Click(object sender, System.EventArgs e)
		{
			AddToQueue(listViewSearch, false);
		}

		private void buttonQueueUp_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();

			MediaListViewItem item = (MediaListViewItem) listViewQueue.SelectedItems[0];

			client.mediaServer.MoveInQueue(item.MediaEntry.MediaId, item.Index, item.Index -1);
		}

		private void buttonQueueDown_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();

			MediaListViewItem item = (MediaListViewItem) listViewQueue.SelectedItems[0];

			client.mediaServer.MoveInQueue(item.MediaEntry.MediaId, item.Index, item.Index +1);
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			ReloadQueue();
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();

			MediaListViewItem item = (MediaListViewItem) listViewQueue.SelectedItems[0];
			int mediaId = item.MediaEntry.MediaId;
			client.mediaServer.RemoveFromQueue(mediaId, item.Index);
			client.mediaServer.PlayMediaId(mediaId);
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			buttonQueueRemove_Click(sender, e);
		}

		private void menuSearchQueueAfter_Popup(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			menuSearchQueueAfter.MenuItems.Clear();

			foreach (MediaListViewItem item in listViewQueue.Items)
			{
				MenuItem menuItem = new MenuItem(item.Text);
				menuItem.Visible = true;
				menuItem.Click += new EventHandler(menuSearchQueueAfterItem_Click);
				menuSearchQueueAfter.MenuItems.Add(menuItem);
			}
		}

		private void menuSearchQueueAfterItem_Click(object sender, System.EventArgs e) 
		{
			SetWaitForMessage();
			MenuItem menuItem = (MenuItem) sender;

			int offset = 1;
			foreach (MediaListViewItem item in listViewSearch.SelectedItems) 
			{
				client.mediaServer.AddToQueue(item.MediaEntry.MediaId, menuItem.Index + offset);
				offset++;
			}
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
			ReloadQueue(); 
			
			status.Message = "Loading songs...";
			InitialState();
			timerPing.Enabled = true;

#if DEBUG
//            checkBoxShowLog.Checked = true;
#endif
			status.Hide();
			dateTimeStart.Value	= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0, 0));
			dateTimeEnd.Value	= DateTime.Now;
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

		private void menuQueueEditInfo_Click(object sender, System.EventArgs e)
		{
			EditMediaInfo(listViewQueue);
		}

		private void EditMediaInfo(ListView lv)
		{
			if (lv.SelectedItems.Count >= 1)
			{
				MediaListViewItem item = (MediaListViewItem) lv.SelectedItems[0];
				EditMediaInfo(item.MediaEntry.MediaId);
			}
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
				SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
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

		private void menuSearchEditInfo_Click(object sender, System.EventArgs e)
		{
			EditMediaInfo(listViewSearch);
		}

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{

			ArrayList addFiles = new ArrayList();

			// Get the final list of files we can add
			foreach (ListViewItem item in listViewFiles.Items) 
			{
				
				string file		  = item.Tag.ToString();

				// make sure file isn't already in music collection
				if (!file.StartsWith(client.FileSharePath)) 
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
			}

			// Now start actually adding the files
			Status status = new Status("Adding files...", addFiles.Count);
			DataSetMedia dsMedia = new DataSetMedia();

			// Copy files if needed and get basic info on the files
			foreach (string fullpath in addFiles) 
			{
				string destPath = @"\\sp\dfs\music\temp\";
				if (!Directory.Exists(destPath))
					Directory.CreateDirectory(destPath);

				string destFile = "";
				destFile = fullpath.Substring(fullpath.LastIndexOf(@"\")+1);

				if (!fullpath.ToLower().StartsWith(client.FileSharePath.ToLower()))
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
					destFile = destPath + destFile;
					
                    // check if we should move (and therefore delete) the source file or simply copy
					if (checkBoxDeleteOnAdd.Checked)
					{
						File.Move(fullpath, destFile);
					}
					else
					{
						if (fullpath.ToLower().StartsWith(@"\\sp\dfs\music"))
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
			dv.RowFilter = "MediaFile = '" + file.Replace("'", "''") + "'";
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
			SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");

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

		private void menuItemColPlayNow_Click(object sender, System.EventArgs e)
		{
			PlayMediaItem(listViewCollection);
		}

		private void menuItemColEditInfo_Click(object sender, System.EventArgs e)
		{
			EditMediaInfo(listViewCollection);
		}

		private void menuItemColQueueTop_Click(object sender, System.EventArgs e)
		{
			AddToQueue(listViewCollection, true);
		}

		private void menuItemColQueueBottom_Click(object sender, System.EventArgs e)
		{
			AddToQueue(listViewCollection, false);
		}

		private void menuColQueueAfter_Popup(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			menuColQueueAfter.MenuItems.Clear();

			foreach (MediaListViewItem item in listViewQueue.Items)
			{
				MenuItem menuItem = new MenuItem(item.Text);
				menuItem.Visible = true;
				menuItem.Click += new EventHandler(menuColQueueAfterItem_Click);
				menuColQueueAfter.MenuItems.Add(menuItem);
			}
		}

		private void menuColQueueAfterItem_Click(object sender, System.EventArgs e) 
		{
			QueueAfterItem(sender, listViewCollection);
		}

		private void menuItemColDelete_Click(object sender, System.EventArgs e)
		{
			DeleteMediaItems(listViewCollection);
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
			SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
			string sql = "update Media set MediaFile = @MediaFile where MediaID = @MediaID";
			SqlCommand cmd = new SqlCommand(sql, cn);
			cmd.Parameters.Add("@MediaFile", SqlDbType.NVarChar, 1024);
			cmd.Parameters.Add("@MediaID", SqlDbType.Int);

			string root = @"\\sp\dfs\music";
			
			cn.Open();
	
			// make sure file exists
			if (File.Exists(entry.MediaFile) && entry.MediaFile.IndexOf(root + @"\cd\") == -1 )
			{				
				// check if artist directory exists, if not create
				if (!Directory.Exists(root + @"\" + entry.Artist.Trim()))
				{
					Directory.CreateDirectory(root + @"\" + entry.Artist.Trim());
				}

				// Figure out the destination file name
				string dest = MediaUtilities.NameMediaFile(entry);
				string sourcePath = entry.MediaFile.Substring(0, entry.MediaFile.LastIndexOf(@"\"));

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

			SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
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

		private void checkBoxShowLog_CheckedChanged(object sender, System.EventArgs e)
		{
			client.Logging = checkBoxShowLog.Checked;
		}

		private void menuSearchDelete_Click(object sender, System.EventArgs e)
		{
			DeleteMediaItems(listViewSearch);
		}

		private void menuQueueDelete_Click(object sender, System.EventArgs e)
		{
			DeleteMediaItems(listViewQueue);
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

		private void menuSearchPlayNow_Click(object sender, System.EventArgs e)
		{
			PlayMediaItem(listViewSearch);
		}

		private void menuNewPlayNow_Click(object sender, System.EventArgs e)
		{
			PlayMediaItem(listViewNew);
		}

		private void menuNewQueueAfter_Popup(object sender, System.EventArgs e)
		{
			SetWaitForMessage();
			menuNewQueueAfter.MenuItems.Clear();

			foreach (MediaListViewItem item in listViewQueue.Items)
			{
				MenuItem menuItem = new MenuItem(item.Text);
				menuItem.Visible = true;
				menuItem.Click += new EventHandler(menuNewQueueAfterItem_Click);
				menuNewQueueAfter.MenuItems.Add(menuItem);
			}
		}

		private void menuNewQueueAfterItem_Click(object sender, System.EventArgs e) 
		{
			QueueAfterItem(sender, listViewNew);
		}

		private void menuNewDelete_Click(object sender, System.EventArgs e)
		{
			DeleteMediaItems(listViewNew);
		}

		private void menuNewEditInfo_Click(object sender, System.EventArgs e)
		{
			EditMediaInfo(listViewNew);
		}

		private void menuNewQueueTop_Click(object sender, System.EventArgs e)
		{
			AddToQueue(listViewNew, true);
		}

		private void menuNewQueueBottom_Click(object sender, System.EventArgs e)
		{
			AddToQueue(listViewNew, false);
		}

		#region Context menu actions

		private void PlayMediaItem(ListView lv)
		{
			if (lv.SelectedItems.Count >= 1)
			{
				SetWaitForMessage();
				MediaListViewItem item = (MediaListViewItem) lv.SelectedItems[0];
				client.mediaServer.PlayMediaId(item.MediaEntry.MediaId);
			}
		}

		private void QueueAfterItem(object sender, ListView lv)
		{
			SetWaitForMessage();
			MenuItem menuItem = (MenuItem) sender;

			int offset = 1;
			foreach (MediaListViewItem item in lv.SelectedItems) 
			{
				client.mediaServer.AddToQueue(item.MediaEntry.MediaId, menuItem.Index + offset);
				offset++;
			}

		}

		private void AddToQueue(ListView lv, bool addAtTop)
		{
			SetWaitForMessage();

			if (addAtTop)
			{
				// Add items in reverse order to top of queue
				foreach (MediaListViewItem item in new IterReverse(lv.SelectedItems)) 
				{
					client.mediaServer.AddToQueue(item.MediaEntry.MediaId, 0);
				}
			}
			else
			{
				// Add items in order to bottom of queue
				foreach (MediaListViewItem item in lv.SelectedItems) 
				{
					client.mediaServer.AddToQueue(item.MediaEntry.MediaId);
				}
			}

		}

		private void DeleteMediaItems(ListView lv)
		{
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
				SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
				SqlCommand cmd = new SqlCommand("dbo.s_DeleteMediaItem", cn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@MediaID", SqlDbType.Int);

				cn.Open();
				foreach (MediaListViewItem item in new IterIsolate(lv.SelectedItems)) 
				{
					if (File.Exists(item.MediaEntry.MediaFile))
                        File.Delete(item.MediaEntry.MediaFile);
					
					cmd.Parameters["@MediaID"].Value = item.MediaEntry.MediaId;
					cmd.ExecuteNonQuery();

					client.mediaServer.UpdateMediaItem(MediaItemUpdateType.Delete, item.MediaEntry.MediaId);
				}
				cn.Close();

			}
		}

		#endregion

		private void UMPlayer_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// Next
			if (e.KeyData == Keys.NumPad3)
			{
				buttonNext_Click(this, EventArgs.Empty);
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

		private void button1_Click(object sender, System.EventArgs e)
		{
		
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
    
	internal class MediaListViewItem: ListViewItem 
	{
		private DataSetMedia.MediaRow entry;
		private UMPlayer player;
		private int mediaId;

		/// <summary>
		/// List view item constructor for Media item
		/// </summary>
		/// <param name="player">Player to subscribe to for updates</param>
		/// <param name="entry">New entry</param>
		public MediaListViewItem(UMPlayer player, DataSetMedia.MediaRow entry) : base()
		{
			this.entry  = entry;
			this.player = player;
			this.mediaId = entry.MediaId;

			string durationString =  ((int)(entry.Duration / 60)).ToString() + ":" + ((int)(entry.Duration % 60)).ToString("00");

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
			SubItems.Add(durationString);

			// subscribe to update events
			player.MediaItemClientUpdateEvent += new MediaItemClientUpdateEventHandler(MediaItemUpdateEvent);

		}

		public void Dispose() 
		{
			player.MediaItemClientUpdateEvent -= new MediaItemClientUpdateEventHandler(MediaItemUpdateEvent);
		}
        
		public DataSetMedia.MediaRow MediaEntry 
		{
			get { return entry; }
		}
        
		/// <summary>
		/// Called by client when an item has been updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void MediaItemUpdateEvent(object sender, MediaItemClientUpdateEventArgs e)
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
}
