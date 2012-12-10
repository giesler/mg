using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using msn2.net.QueuePlayer.Shared;
using System.Diagnostics;
using msn2.net.Common;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for PlayerQueue.
	/// </summary>
	public class PlayerQueue : System.Windows.Forms.Form
	{
		public System.Windows.Forms.Button buttonClearQueue;
		public System.Windows.Forms.Button buttonQueueDown;
		public System.Windows.Forms.Button buttonQueueUp;
		public System.Windows.Forms.Button buttonQueueRemove;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public msn2.net.QueuePlayer.Client.MediaListView mediaList;
		private UMPlayer player;

		public PlayerQueue(UMPlayer player)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


			this.player = player;
			mediaList.lv.ContextMenu = player.contextMenuMediaList;

			ReloadQueue(this, EventArgs.Empty);
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
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonClearQueue = new System.Windows.Forms.Button();
			this.buttonQueueDown = new System.Windows.Forms.Button();
			this.buttonQueueUp = new System.Windows.Forms.Button();
			this.buttonQueueRemove = new System.Windows.Forms.Button();
			this.mediaList = new msn2.net.QueuePlayer.Client.MediaListView();
			this.SuspendLayout();
			// 
			// buttonClearQueue
			// 
			this.buttonClearQueue.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonClearQueue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonClearQueue.Location = new System.Drawing.Point(424, 40);
			this.buttonClearQueue.Name = "buttonClearQueue";
			this.buttonClearQueue.Size = new System.Drawing.Size(32, 23);
			this.buttonClearQueue.TabIndex = 9;
			this.buttonClearQueue.Text = "XX";
			this.buttonClearQueue.Visible = false;
			this.buttonClearQueue.Click += new System.EventHandler(this.buttonClearQueue_Click);
			// 
			// buttonQueueDown
			// 
			this.buttonQueueDown.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonQueueDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonQueueDown.Location = new System.Drawing.Point(424, 240);
			this.buttonQueueDown.Name = "buttonQueueDown";
			this.buttonQueueDown.Size = new System.Drawing.Size(32, 23);
			this.buttonQueueDown.TabIndex = 8;
			this.buttonQueueDown.Text = "\\/";
			this.buttonQueueDown.Visible = false;
			this.buttonQueueDown.Click += new System.EventHandler(this.buttonQueueDown_Click);
			// 
			// buttonQueueUp
			// 
			this.buttonQueueUp.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonQueueUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonQueueUp.Location = new System.Drawing.Point(424, 208);
			this.buttonQueueUp.Name = "buttonQueueUp";
			this.buttonQueueUp.Size = new System.Drawing.Size(32, 23);
			this.buttonQueueUp.TabIndex = 7;
			this.buttonQueueUp.Text = "/\\";
			this.buttonQueueUp.Visible = false;
			this.buttonQueueUp.Click += new System.EventHandler(this.buttonQueueUp_Click);
			// 
			// buttonQueueRemove
			// 
			this.buttonQueueRemove.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonQueueRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonQueueRemove.Location = new System.Drawing.Point(424, 8);
			this.buttonQueueRemove.Name = "buttonQueueRemove";
			this.buttonQueueRemove.Size = new System.Drawing.Size(32, 23);
			this.buttonQueueRemove.TabIndex = 6;
			this.buttonQueueRemove.Text = "X";
			this.buttonQueueRemove.Visible = false;
			this.buttonQueueRemove.Click += new System.EventHandler(this.buttonQueueRemove_Click);
			// 
			// mediaList
			// 
			this.mediaList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mediaList.Name = "mediaList";
			this.mediaList.ShowDeleteColumn = true;
			this.mediaList.Size = new System.Drawing.Size(464, 270);
			this.mediaList.TabIndex = 10;
			this.mediaList.MediaItemDroppedEvent += new msn2.net.QueuePlayer.Client.MediaItemDropped(this.mediaList_MediaItemDroppedEvent);
			this.mediaList.MediaDoubleClick += new System.EventHandler(this.mediaList_MediaDoubleClick);
			this.mediaList.MediaKeyUp += new System.Windows.Forms.KeyEventHandler(this.mediaList_MediaKeyUp);
			this.mediaList.MediaItemDeleteEvent += new msn2.net.QueuePlayer.Client.MediaItemDelete(this.mediaList_MediaItemDeleteEvent);
			// 
			// PlayerQueue
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 270);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.mediaList,
																		  this.buttonClearQueue,
																		  this.buttonQueueDown,
																		  this.buttonQueueUp,
																		  this.buttonQueueRemove});
			this.Name = "PlayerQueue";
			this.Text = "Queue";
			this.ResumeLayout(false);

		}
		#endregion

		public void buttonQueueRemove_Click(object sender, System.EventArgs e)
		{
			player.SetWaitForMessage();

			foreach (MediaListViewItem item in new IterIsolate(mediaList.SelectedItems))
			{
				player.client.mediaServer.RemoveFromQueue(item.Entry.MediaId, item.Guid);
			}
		}

		public void buttonQueueUp_Click(object sender, System.EventArgs e)
		{
			player.SetWaitForMessage();
			MediaListViewItem item = (MediaListViewItem) mediaList.SelectedItems[0];
			player.client.mediaServer.MoveInQueue(item.Entry.MediaId, item.Guid, item.Index -1);
		}

		public void buttonQueueDown_Click(object sender, System.EventArgs e)
		{
			player.SetWaitForMessage();
			MediaListViewItem item = (MediaListViewItem) mediaList.SelectedItems[0];
			player.client.mediaServer.MoveInQueue(item.Entry.MediaId, item.Guid, item.Index +1);
		}

		private void mediaList_MediaKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ( (e.KeyCode == Keys.R && e.Control) || e.KeyCode == Keys.Delete )
			{
				buttonQueueRemove_Click(this, EventArgs.Empty);
			}
		}

		#region AddToQueue

		public delegate void AddToQueueDelegate(QueueEventArgs e);

		public void InvokeAddToQueue(QueueEventArgs e) 
		{
			DataSetMedia.MediaRow row = player.client.FindMediaRow(e.MediaId);
			MediaListViewItem item = new MediaListViewItem(player, row, e.Guid);

			// see if we should add at end or insert in queue
			if (e.Position >= mediaList.Items.Count || e.Position < 0) 
			{
				mediaList.AddItem(item);
			} 
			else 
			{
				mediaList.InsertItem(item, e.Position);
			}

			player.ClearWaitForMessage();
		}

		#endregion

		#region RemoveFromQueue

		public delegate void RemoveFromQueueDelegate(QueueEventArgs e);

		public void InvokeRemoveFromQueue(QueueEventArgs e)
		{
			mediaList.RemoveItem(e.Guid);
			player.ClearWaitForMessage();
		}

		#endregion

		#region MoveInQueue
		
		public delegate void MovedInQueueDelegate(QueueEventArgs e);

		public void InvokeMovedInQueue(QueueEventArgs e) 
		{
			InvokeRemoveFromQueue(e);
			InvokeAddToQueue(e);

			if (mediaList.SelectedItems.Count > 0)
				mediaList.SelectedItems[0].Selected = false;

			mediaList.Items[e.Position].Selected = true;

			player.ClearWaitForMessage();
		}

		#endregion

		/// <summary>
		/// Reload the queue from the server
		/// </summary>
		public void ReloadQueue(object sender, EventArgs e) 
		{
			MediaCollection queue = player.client.mediaServer.CurrentQueue();

			// Clear the current queue
			mediaList.Clear();

			// Loop through adding listviewitems
			foreach(MediaCollectionEntry entry in queue) 
			{
				mediaList.AddItem(
					new MediaListViewItem(
					player, player.client.FindMediaRow(entry.MediaId), entry.Guid));
			}

			player.ClearWaitForMessage();
		}

		public void buttonClearQueue_Click(object sender, System.EventArgs e)
		{
			player.SetWaitForMessage();

			foreach (MediaListViewItem item in new IterIsolate(mediaList.Items))
			{
				player.client.mediaServer.RemoveFromQueue(item.Entry.MediaId, item.Guid);
			}
		}

		private void mediaList_MediaDoubleClick(object sender, System.EventArgs e)
		{
			if (mediaList.SelectedItems.Count > 0)
			{
                MediaListViewItem item = (MediaListViewItem) mediaList.SelectedItems[0];
				player.client.mediaServer.PlayMediaId(item.Entry.MediaId);
			}
		}

		private void mediaList_MediaItemDroppedEvent(object sender, msn2.net.QueuePlayer.Client.MediaItemDropEventArgs e)
		{
			player.SetWaitForMessage();
			int position = e.TargetItem.Index;

			foreach (MediaListViewItem item in e.SourceItems)
			{
				// check if we want to add to queue
				if (item.Guid == Guid.Empty)
				{
					player.client.mediaServer.AddToQueue(item.Entry.MediaId, position);
				}
					// otherwise we are in the queue and want to move
				else
				{
					player.client.mediaServer.MoveInQueue(item.Entry.MediaId, item.Guid, position);
				}
				position++;
			}
		}

		private void mediaList_MediaItemDeleteEvent(object sender, msn2.net.QueuePlayer.Client.MediaItemDeleteEventArgs e)
		{
			player.SetWaitForMessage();
			player.client.mediaServer.RemoveFromQueue(e.Item.Entry.MediaId, e.Item.Guid);		
		}
	}
}
