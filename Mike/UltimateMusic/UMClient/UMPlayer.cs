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
		private System.Windows.Forms.MenuItem menuItem1;
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
		private System.Windows.Forms.CheckBox checkBox1;
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
		private DataSetMedia dsMedia;

		public UMPlayer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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

		public void InitialState() 
		{
			int mediaId = client.mediaServer.CurrentMediaId;
			MediaServer.PlayState playState = client.mediaServer.CurrentPlayState;

			if (mediaId != 0) 
			{
				MediaCollectionEntry entry = client.mediaCollection[mediaId];

				if (playState == MediaServer.PlayState.Playing) 
				{
					Playing(mediaId);
				}
			}

			// set the volume bar
			Volume_Changed(client.mediaServer.Volume);
			Balance_Changed(client.mediaServer.Balance);
			Rate_Changed(client.mediaServer.Rate);		
			UMPlayer_Resize(this, new EventArgs());

			// Load the media list
			SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
			SqlDataAdapter da = new SqlDataAdapter("select * from Media", cn);
			dsMedia = new DataSetMedia();
			da.Fill(dsMedia, "Media");

		}

		public void Playing(int mediaId) 
		{
            MediaCollectionEntry entry = client.mediaCollection[mediaId];

			labelName.Text = entry.Name;
			labelName.Visible = true;
			labelArtist.Text = entry.Artist;

			buttonPlayStop.Text = "[]";
			buttonPause.Enabled = true;
			buttonPlayStop.Enabled = true;
			timerPaused.Enabled = false;

			mediaBar.labelNowPlaying.Visible = true;
			mediaBar.CurrentSong = entry.Name;
			mediaBar.buttonPlayStop.Text = "[]";
			mediaBar.buttonPause.Enabled = true;
		}

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
		}

		public void Paused() 
		{
			buttonPlayStop.Enabled = true;
			buttonPause.Enabled = true;
			timerPaused.Enabled = true;            

			mediaBar.buttonPlayStop.Enabled = true;
			mediaBar.buttonPause.Enabled = true;
		}

		public void Progress(double progress) 
		{
            pictureBoxProgress.Width = Convert.ToInt32(((progress) * (double) (pictureBoxContainer.Width-4)));
		}

		public void Volume_Changed(double volume) 
		{
			pictureBoxVolume.Width = Convert.ToInt32(((volume) * (double) (pictureBoxVolumeContainer.Width-4)));
		}

		public void Rate_Changed(double rate) 
		{
            trackBarRate.Value = (int) (rate * 100);
		}
		
		public void Balance_Changed(int balance) 
		{
			trackBarBalance.Value = balance;
		}

		#endregion

		#region Queue handlers

		public void AddToQueue(int mediaId, int position) 
		{
            MediaListViewItem item = new MediaListViewItem(client.mediaCollection[mediaId]);

			// see if we should add at end or insert in queue
			if (position >= listViewQueue.Items.Count) 
			{
				listViewQueue.Items.Add(item);
			} 
			else 
			{
				listViewQueue.Items.Insert(position, item);
			}
		}

		public void RemoveFromQueue(int mediaId, int position) 
		{	
			listViewQueue.Items.RemoveAt(position);
		}

		public void MovedInQueue(int mediaId, int instance, int newPosition) 
		{
			RemoveFromQueue(mediaId, instance);
			AddToQueue(mediaId, newPosition);

			listViewQueue.Items[newPosition].Selected = true;
		}

		public void ReloadQueue() 
		{
			
			MediaCollection queueMedia = client.mediaServer.CurrentQueue();
			// Clear the current queue
			listViewQueue.Items.Clear();

			// Loop through adding listviewitems
			foreach(MediaCollectionEntry entry in queueMedia) 
			{
				listViewQueue.Items.Add(new MediaListViewItem(client.mediaCollection[entry.MediaId]));
			}
		}

		#endregion
		
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
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.contextMenuQueueItems = new System.Windows.Forms.ContextMenu();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuQueueEditInfo = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.listViewSearch = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.contextMenuSearchItems = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuSearchAddToQueue = new System.Windows.Forms.MenuItem();
			this.menuSearchTopOfQueue = new System.Windows.Forms.MenuItem();
			this.menuSearchBottomOfQueue = new System.Windows.Forms.MenuItem();
			this.menuSearchQueueAfter = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuSearchEditInfo = new System.Windows.Forms.MenuItem();
			this.buttonSearch = new System.Windows.Forms.Button();
			this.textBoxSearch = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.listViewCollection = new System.Windows.Forms.ListView();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.contextMenuMediaCollection = new System.Windows.Forms.ContextMenu();
			this.menuItemColPlayNow = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItemColQueueTop = new System.Windows.Forms.MenuItem();
			this.menuItemColQueueBottom = new System.Windows.Forms.MenuItem();
			this.menuColQueueAfter = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItemColEditInfo = new System.Windows.Forms.MenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.treeViewCollection = new System.Windows.Forms.TreeView();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.listViewFiles = new System.Windows.Forms.ListView();
			this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
			this.buttonRemoveFile = new System.Windows.Forms.Button();
			this.buttonAddFile = new System.Windows.Forms.Button();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tabPage7 = new System.Windows.Forms.TabPage();
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
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage2.SuspendLayout();
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
			this.buttonNext.Location = new System.Drawing.Point(104, 48);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(40, 23);
			this.buttonNext.TabIndex = 2;
			this.buttonNext.Text = "> >";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonPause
			// 
			this.buttonPause.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPause.Location = new System.Drawing.Point(56, 48);
			this.buttonPause.Name = "buttonPause";
			this.buttonPause.Size = new System.Drawing.Size(40, 23);
			this.buttonPause.TabIndex = 1;
			this.buttonPause.Text = "| |";
			this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
			// 
			// buttonPlayStop
			// 
			this.buttonPlayStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlayStop.Location = new System.Drawing.Point(8, 48);
			this.buttonPlayStop.Name = "buttonPlayStop";
			this.buttonPlayStop.Size = new System.Drawing.Size(40, 23);
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
			this.labelName.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelName.Location = new System.Drawing.Point(8, 24);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(336, 32);
			this.labelName.TabIndex = 1;
			this.labelName.Text = "[name]";
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
																					  this.tabPage4,
																					  this.tabPage2,
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
			this.tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.buttonQueueDown,
																				   this.buttonQueueUp,
																				   this.buttonQueueRemove,
																				   this.listViewQueue});
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
			this.listViewQueue.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader1,
																							this.columnHeader2,
																							this.columnHeader3});
			this.listViewQueue.ContextMenu = this.contextMenuQueueItems;
			this.listViewQueue.FullRowSelect = true;
			this.listViewQueue.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewQueue.HideSelection = false;
			this.listViewQueue.Name = "listViewQueue";
			this.listViewQueue.Size = new System.Drawing.Size(456, 238);
			this.listViewQueue.TabIndex = 0;
			this.listViewQueue.View = System.Windows.Forms.View.Details;
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
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "-";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 4;
			this.menuItem2.Text = "&Refresh";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// tabPage4
			// 
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
			this.listViewSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.columnHeader4,
																							 this.columnHeader5,
																							 this.columnHeader6});
			this.listViewSearch.ContextMenu = this.contextMenuSearchItems;
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
			// columnHeader6
			// 
			this.columnHeader6.Text = "Duration";
			this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.columnHeader6.Width = 100;
			// 
			// contextMenuSearchItems
			// 
			this.contextMenuSearchItems.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								   this.menuItem1,
																								   this.menuSearchAddToQueue,
																								   this.menuSearchEditInfo});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "&Play Now";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
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
			this.listViewCollection.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								 this.columnHeader7,
																								 this.columnHeader8,
																								 this.columnHeader9});
			this.listViewCollection.ContextMenu = this.contextMenuMediaCollection;
			this.listViewCollection.Dock = System.Windows.Forms.DockStyle.Fill;
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
																									   this.menuItemColEditInfo});
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
			this.treeViewCollection.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewCollection.HideSelection = false;
			this.treeViewCollection.ImageIndex = -1;
			this.treeViewCollection.Name = "treeViewCollection";
			this.treeViewCollection.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																						   new System.Windows.Forms.TreeNode("By Artist", new System.Windows.Forms.TreeNode[] {
																																												  new System.Windows.Forms.TreeNode("Loading")})});
			this.treeViewCollection.SelectedImageIndex = -1;
			this.treeViewCollection.Size = new System.Drawing.Size(192, 238);
			this.treeViewCollection.TabIndex = 0;
			this.treeViewCollection.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCollection_AfterSelect);
			this.treeViewCollection.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewCollection_BeforeExpand);
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.listViewFiles,
																				   this.buttonRemoveFile,
																				   this.buttonAddFile,
																				   this.buttonAdd,
																				   this.checkBox1,
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
			this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader10});
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
			// checkBox1
			// 
			this.checkBox1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.checkBox1.Location = new System.Drawing.Point(8, 208);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(248, 24);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "&Delete files after they are added";
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
			this.tabPage7.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this.checkBoxShowLog,
																				   this.textBoxLog});
			this.tabPage7.Location = new System.Drawing.Point(4, 22);
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.Size = new System.Drawing.Size(504, 238);
			this.tabPage7.TabIndex = 7;
			this.tabPage7.Text = "Log";
			// 
			// checkBoxShowLog
			// 
			this.checkBoxShowLog.Location = new System.Drawing.Point(8, 7);
			this.checkBoxShowLog.Name = "checkBoxShowLog";
			this.checkBoxShowLog.Size = new System.Drawing.Size(488, 24);
			this.checkBoxShowLog.TabIndex = 3;
			this.checkBoxShowLog.Text = "&Show Activity";
			// 
			// textBoxLog
			// 
			this.textBoxLog.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
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
			this.tabPage6.Controls.AddRange(new System.Windows.Forms.Control[] {
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
			// UMPlayer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 366);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1,
																		  this.statusBar1,
																		  this.panel1});
			this.Name = "UMPlayer";
			this.Text = "Ultimate Music Player";
			this.Resize += new System.EventHandler(this.UMPlayer_Resize);
			this.Load += new System.EventHandler(this.UMPlayer_Load);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
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
			buttonNext_Click(this, new EventArgs());
		}

		private void buttonNext_Click(object sender, System.EventArgs e)
		{
			client.mediaServer.Next();
		}

		private void buttonQueueRemove_Click(object sender, System.EventArgs e)
		{
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
			SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
			SqlCommand cmd = new SqlCommand("select ID from Media where Name like '%' + @SearchString + '%' or Artist like '%' + @SearchString + '%'", cn);
			cmd.Parameters.Add("@SearchString", textBoxSearch.Text);

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read()) 
			{
                listViewSearch.Items.Add(new MediaListViewItem(client.mediaCollection[Convert.ToInt32(dr["id"])]));
			}
			dr.Close();
			cn.Close();

		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			MediaListViewItem item = (MediaListViewItem) listViewSearch.SelectedItems[0];
			client.mediaServer.AddToQueue(item.MediaEntry.MediaId, 0);
			client.mediaServer.Next();
		}

		private void menuSearchTopOfQueue_Click(object sender, System.EventArgs e)
		{
			foreach (MediaListViewItem item in listViewSearch.SelectedItems) 
			{
				client.mediaServer.AddToQueue(item.MediaEntry.MediaId, 0);
			}
		
		}

		private void menuSearchBottomOfQueue_Click(object sender, System.EventArgs e)
		{
			foreach (MediaListViewItem item in listViewSearch.SelectedItems) 
			{
				client.mediaServer.AddToQueue(item.MediaEntry.MediaId);
			}
		
		}

		private void buttonQueueUp_Click(object sender, System.EventArgs e)
		{
			MediaListViewItem item = (MediaListViewItem) listViewQueue.SelectedItems[0];

			client.mediaServer.MoveInQueue(item.MediaEntry.MediaId, item.Index, item.Index -1);
		}

		private void buttonQueueDown_Click(object sender, System.EventArgs e)
		{
			MediaListViewItem item = (MediaListViewItem) listViewQueue.SelectedItems[0];

			client.mediaServer.MoveInQueue(item.MediaEntry.MediaId, item.Index, item.Index +1);
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			ReloadQueue();
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			MediaListViewItem item = (MediaListViewItem) listViewQueue.SelectedItems[0];

			client.mediaServer.MoveInQueue(item.MediaEntry.MediaId, item.Index, 0);
			client.mediaServer.Next();
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			buttonQueueRemove_Click(sender, e);
		}

		private void menuSearchQueueAfter_Popup(object sender, System.EventArgs e)
		{
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

			mediaBar = new MediaBar(this);
			mediaBar.Visible = true;
			statusBar1.Panels[0].Text = "Connected to " + connection.HostName;
			pictureBoxProgress.Width = 0;

			// Create the client
			client = new Client(this);
			ReloadQueue(); 
			
			InitialState();
			timerPing.Enabled = true;
		}

		private void pictureBoxVolume_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pictureBoxVolumeContainer_MouseUp(sender, e);
		}

		private void pictureBoxVolumeContainer_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            double newVolume = ((double)(e.X) / (double)(pictureBoxVolumeContainer.Width-4));
			client.mediaServer.Volume = newVolume;
		}

		private void notifyIcon1_DoubleClick(object sender, System.EventArgs e)
		{
			this.Visible = true;
		}

		private void menuQueueEditInfo_Click(object sender, System.EventArgs e)
		{
			MediaListViewItem item = (MediaListViewItem) listViewQueue.SelectedItems[0];
			if (item == null)
				return;

			EditMediaInfo(item.MediaEntry.MediaId, item);
		}

		private void EditMediaInfo(int mediaId, MediaListViewItem item) 
		{
			MediaEditor editor = new MediaEditor();
			MediaCollectionEntry entry = client.mediaCollection[mediaId];

			editor.MediaName = entry.Name;
			editor.Artist	= entry.Artist;
			editor.Filename	= entry.MediaFile;

			if (editor.ShowDialog() == DialogResult.OK) 
			{
				// set up db connection
				string sql = "update Media set Name = @Name, Artist = @Artist, Album = @Album, Genre = @Genre, Comments = @Comments where ID = @MediaID";
				SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=music;Data Source=kyle;");
				SqlCommand cmd = new SqlCommand(sql, cn);

				// set query params
                cmd.Parameters.Add("@Name", editor.MediaName);
				cmd.Parameters.Add("@Artist", editor.Artist);
				cmd.Parameters.Add("@MediaID", mediaId);
				cmd.Parameters.Add("@Album", editor.Album);
				cmd.Parameters.Add("@Genre", editor.Genre);
				cmd.Parameters.Add("@Comments", editor.Comments);

				// save to database
				cn.Open();
				cmd.ExecuteNonQuery();
				cn.Close();

				// Save the in memory entry
				entry.Name		= editor.MediaName;
				entry.Artist	= editor.Artist;

				// Set the listview item
				if (item != null)
				{
					item.Text = entry.Name;
					item.SubItems[1].Text = entry.Artist;
				}
			}

		}

		private void menuSearchEditInfo_Click(object sender, System.EventArgs e)
		{
			MediaListViewItem item = (MediaListViewItem) listViewSearch.SelectedItems[0];
			if (item == null)
				return;

			EditMediaInfo(item.MediaEntry.MediaId, item);
		}

		private void buttonAdd_Click(object sender, System.EventArgs e)
		{
			ArrayList addFiles = new ArrayList();

			foreach (ListViewItem item in listViewFiles.Items) 
			{
				string file		  = item.Tag.ToString();
				string sourcePath = file.Substring(0, file.LastIndexOf(@"\")-1);
				string sourceFile = file.Substring(file.LastIndexOf(@"\")+1);

				if (File.Exists(@"\\sp\dfs\Music\" + sourceFile)) 
				{
					string message = String.Format("The file '{0}' already exists in the music collection.  This file will be skipped.  Click 'Cancel' to abort adding files.",
						sourceFile);
					if (MessageBox.Show(message, "File already exists", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
						return;
				} 
				else 
				{
					addFiles.Add(file);
				}
			}

			Status status = new Status("Adding files...", addFiles.Count);
			DataSetMedia dsMedia = new DataSetMedia();

			foreach (string file in addFiles) 
			{
				string sourcePath = file.Substring(0, file.LastIndexOf(@"\")-1);
				string sourceFile = file.Substring(file.LastIndexOf(@"\")+1);

				File.Copy(file, @"\\sp\dfs\Music\" + sourceFile, true);

				string name = sourceFile.Substring(0, sourceFile.LastIndexOf(".")-1);
				string artist = "";
				if (name.IndexOf("-") > 0) 
				{
					// TODO:  Parse name and artist here!
				}

				// TODO: Load the song to get the duration
                // Add to database
                DataSetMedia.MediaRow row = dsMedia.Media.NewMediaRow();
				row.Filename = @"\\sp\dfs\Music\" + sourceFile;
				
			}
		}

		private void trackBarBalance_Scroll(object sender, System.EventArgs e)
		{
			client.mediaServer.Balance = trackBarBalance.Value;
		}

		private void trackBarRate_Scroll(object sender, System.EventArgs e)
		{
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
			foreach (ListViewItem item in listViewFiles.Items) 
			{
				if (item.Tag.Equals(file)) 
				{
					found = true;
					break;
				}
			}
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
			// Resize the file listview columns
			listViewFiles.Columns[0].Width = listViewFiles.Width - 22;

			listViewQueue.Columns[0].Width = (int)((listViewQueue.Width-22) * 0.50);
			listViewQueue.Columns[1].Width = (int)((listViewQueue.Width-22) * 0.30);
			listViewQueue.Columns[2].Width = (int)((listViewQueue.Width-22) * 0.20);

			listViewSearch.Columns[0].Width = (int)((listViewSearch.Width-22) * 0.50);
			listViewSearch.Columns[1].Width = (int)((listViewSearch.Width-22) * 0.30);
			listViewSearch.Columns[2].Width = (int)((listViewSearch.Width-22) * 0.20);

			listViewCollection.Columns[0].Width = (int)((listViewCollection.Width-22) * 0.50);
			listViewCollection.Columns[1].Width = (int)((listViewCollection.Width-22) * 0.30);
			listViewCollection.Columns[2].Width = (int)((listViewCollection.Width-22) * 0.20);
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
			if (tabControl1.SelectedIndex == 1) 
			{
				textBoxSearch.Focus();
			}
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
                
                cn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read()) 
				{
					e.Node.Nodes.Add(new ArtistTreeNode(dr[0].ToString()));
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
					listViewCollection.Items.Add(new MediaListViewItem(client.mediaCollection[row.ID]));
				}

			}
		}

		private void menuItemColPlayNow_Click(object sender, System.EventArgs e)
		{
			MediaListViewItem item = (MediaListViewItem) listViewCollection.SelectedItems[0];

			client.mediaServer.MoveInQueue(item.MediaEntry.MediaId, item.Index, 0);
			client.mediaServer.Next();
		}

		private void menuItemColEditInfo_Click(object sender, System.EventArgs e)
		{
			MediaListViewItem item = (MediaListViewItem) listViewCollection.SelectedItems[0];
			if (item == null)
				return;

			EditMediaInfo(item.MediaEntry.MediaId, item);
		}

		private void menuItemColQueueTop_Click(object sender, System.EventArgs e)
		{
			foreach (MediaListViewItem item in listViewCollection.SelectedItems) 
			{
				client.mediaServer.AddToQueue(item.MediaEntry.MediaId, 0);
			}
		}

		private void menuItemColQueueBottom_Click(object sender, System.EventArgs e)
		{
			foreach (MediaListViewItem item in listViewCollection.SelectedItems) 
			{
				client.mediaServer.AddToQueue(item.MediaEntry.MediaId);
			}
		}

		private void menuColQueueAfter_Popup(object sender, System.EventArgs e)
		{
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
			MenuItem menuItem = (MenuItem) sender;

			int offset = 1;
			foreach (MediaListViewItem item in listViewCollection.SelectedItems) 
			{
				client.mediaServer.AddToQueue(item.MediaEntry.MediaId, menuItem.Index + offset);
				offset++;
			}
		}


	}

	public class MediaListViewItem: ListViewItem 
	{
		private MediaCollectionEntry entry;

		public MediaListViewItem(UMServer.MediaCollectionEntry entry) : base()
		{
			this.entry = entry;

			string durationString = ((int)(entry.Duration / 60)).ToString() + ":" + ((int)(entry.Duration % 60)).ToString("00");

			// Set the sub items
			this.Text = entry.Name;
			SubItems.Add(entry.Artist);
			SubItems.Add(durationString);

		}

		public MediaCollectionEntry MediaEntry 
		{
			get { return entry; }
		}
	}

	public class ArtistTreeNode: TreeNode 
	{
		private string artist;

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
}
