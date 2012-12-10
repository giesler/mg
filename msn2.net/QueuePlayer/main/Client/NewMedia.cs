using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using msn2.net.QueuePlayer.Shared;
using System.Runtime.Remoting.Messaging;
using System.Data;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for NewMedia.
	/// </summary>
	public class NewMedia : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DateTimePicker dateTimeEnd;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.DateTimePicker dateTimeStart;
		private System.Windows.Forms.Label label5;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private msn2.net.QueuePlayer.Client.MediaListView mediaList;
		private UMPlayer player;

		public NewMedia(UMPlayer player)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.player = player;
			this.mediaList.lv.ContextMenu = player.contextMenuMediaList;

			dateTimeStart.Value	= DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0, 0));
			dateTimeEnd.Value	= DateTime.Now;

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
			this.dateTimeEnd = new System.Windows.Forms.DateTimePicker();
			this.label6 = new System.Windows.Forms.Label();
			this.dateTimeStart = new System.Windows.Forms.DateTimePicker();
			this.label5 = new System.Windows.Forms.Label();
			this.mediaList = new msn2.net.QueuePlayer.Client.MediaListView();
			this.SuspendLayout();
			// 
			// dateTimeEnd
			// 
			this.dateTimeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimeEnd.Location = new System.Drawing.Point(224, 8);
			this.dateTimeEnd.Name = "dateTimeEnd";
			this.dateTimeEnd.Size = new System.Drawing.Size(96, 20);
			this.dateTimeEnd.TabIndex = 15;
			this.dateTimeEnd.ValueChanged += new System.EventHandler(this.dateTimeEnd_ValueChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(192, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 24);
			this.label6.TabIndex = 14;
			this.label6.Text = "and";
			// 
			// dateTimeStart
			// 
			this.dateTimeStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimeStart.Location = new System.Drawing.Point(104, 8);
			this.dateTimeStart.Name = "dateTimeStart";
			this.dateTimeStart.Size = new System.Drawing.Size(88, 20);
			this.dateTimeStart.TabIndex = 13;
			this.dateTimeStart.ValueChanged += new System.EventHandler(this.dateTimeStart_ValueChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 24);
			this.label5.TabIndex = 12;
			this.label5.Text = "Show files added between ";
			// 
			// mediaList
			// 
			this.mediaList.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.mediaList.Location = new System.Drawing.Point(0, 32);
			this.mediaList.Name = "mediaList";
			this.mediaList.Size = new System.Drawing.Size(424, 288);
			this.mediaList.TabIndex = 16;
			this.mediaList.MediaDoubleClick += new System.EventHandler(this.mediaList_MediaDoubleClick);
			// 
			// NewMedia
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(424, 318);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.mediaList,
																		  this.dateTimeEnd,
																		  this.label6,
																		  this.dateTimeStart,
																		  this.label5});
			this.Name = "NewMedia";
			this.Text = "NewMedia";
			this.ResumeLayout(false);

		}
		#endregion

		#region Add media events

		public delegate void MediaItemAddedDelegate(MediaItemUpdateEventArgs e);

		public void MediaItemAdded(MediaItemUpdateEventArgs e)
		{
			if (e.Type == MediaItemUpdateType.Add)
			{
				DataSetMedia.MediaRow row = player.client.dsMedia.Media.FindByMediaId(e.MediaId);
				mediaList.AddItem(new MediaListViewItem(player, row));
			}
		}

		#endregion

		private void dateTimeStart_ValueChanged(object sender, System.EventArgs e)
		{
            UpdateNewList();		
		}

		private void dateTimeEnd_ValueChanged(object sender, System.EventArgs e)
		{
			UpdateNewList();
		}
	
		private void UpdateNewList()
		{
			DataView dv = new DataView(player.client.dsMedia.Media);

			mediaList.Clear();

			dv.RowFilter = String.Format("DateAdded >= '{0}' And DateAdded <= '{1} 11:59 PM'",
				dateTimeStart.Value.ToShortDateString(), dateTimeEnd.Value.ToShortDateString());

			foreach (DataRowView rowView in dv)
			{
				DataSetMedia.MediaRow row = (DataSetMedia.MediaRow) rowView.Row;
				mediaList.AddItem(new MediaListViewItem(player, row));
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

	}
}
