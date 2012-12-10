using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;
using msn2.net.QueuePlayer.Shared;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for History.
	/// </summary>
	public class History : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private msn2.net.QueuePlayer.Client.MediaListView mediaList;
		private UMPlayer player;

		public History(UMPlayer player)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.player = player;
			this.mediaList.lv.ContextMenu = player.contextMenuMediaList;

			this.TopLevel = false;

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
			this.mediaList = new msn2.net.QueuePlayer.Client.MediaListView();
			this.SuspendLayout();
			// 
			// mediaList
			// 
			this.mediaList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mediaList.Name = "mediaList";
			this.mediaList.Size = new System.Drawing.Size(328, 222);
			this.mediaList.TabIndex = 0;
			this.mediaList.MediaDoubleClick += new System.EventHandler(this.mediaList_MediaDoubleClick);
			// 
			// History
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(328, 222);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.mediaList});
			this.Name = "History";
			this.Text = "History";
			this.ResumeLayout(false);

		}
		#endregion

		#region Add to History

		public delegate void AddToHistoryDelegate(HistoryEventArgs e);

		public void InvokeAddToHistory(HistoryEventArgs e)
		{
			MediaListViewItem item = new MediaListViewItem(player, player.client.FindMediaRow(e.MediaId), e.Guid);
			mediaList.InsertItem(item, 0);
		}

		#endregion

		#region RemoveFromHistory

		public delegate void RemoveFromHistoryDelegate(HistoryEventArgs e);

		public void InvokeRemoveFromHistory(HistoryEventArgs e)
		{
			mediaList.RemoveItem(e.Guid);
		}

		#endregion

		private void mediaList_MediaDoubleClick(object sender, System.EventArgs e)
		{
			if (mediaList.SelectedItems.Count > 0)
			{
                MediaListViewItem item = (MediaListViewItem) mediaList.SelectedItems[0];
				player.client.mediaServer.PlayMediaId(item.Entry.MediaId);
			}

		}

	}
}
