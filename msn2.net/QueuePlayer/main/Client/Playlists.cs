using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using msn2.net.QueuePlayer.Shared;
using msn2.net.Common;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for Playlists.
	/// </summary>
	public class Playlists : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonPlaylistMediaRemove;
		private System.Windows.Forms.Button buttonPlaylistDelete;
		private System.Windows.Forms.Button buttonPlaylistAdd;
		public System.Windows.Forms.ListView listViewPlaylists;
		private System.Windows.Forms.ColumnHeader columnHeader27;
		private System.Windows.Forms.ColumnHeader columnHeader28;
		private System.Windows.Forms.ColumnHeader columnHeader29;
		private System.Windows.Forms.ColumnHeader columnHeader30;
		private System.Windows.Forms.ColumnHeader columnHeader31;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public msn2.net.QueuePlayer.Client.MediaListView mediaList;

		private UMPlayer player;

		public Playlists(UMPlayer player)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.player = player;
			this.mediaList.lv.ContextMenu = player.contextMenuMediaList;

			LoadPlaylists();

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
			this.buttonPlaylistMediaRemove = new System.Windows.Forms.Button();
			this.buttonPlaylistDelete = new System.Windows.Forms.Button();
			this.buttonPlaylistAdd = new System.Windows.Forms.Button();
			this.listViewPlaylists = new System.Windows.Forms.ListView();
			this.columnHeader27 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader28 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader29 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader30 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader31 = new System.Windows.Forms.ColumnHeader();
			this.mediaList = new msn2.net.QueuePlayer.Client.MediaListView();
			this.SuspendLayout();
			// 
			// buttonPlaylistMediaRemove
			// 
			this.buttonPlaylistMediaRemove.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonPlaylistMediaRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlaylistMediaRemove.Location = new System.Drawing.Point(376, 88);
			this.buttonPlaylistMediaRemove.Name = "buttonPlaylistMediaRemove";
			this.buttonPlaylistMediaRemove.Size = new System.Drawing.Size(24, 23);
			this.buttonPlaylistMediaRemove.TabIndex = 18;
			this.buttonPlaylistMediaRemove.Text = "x";
			this.buttonPlaylistMediaRemove.Click += new System.EventHandler(this.buttonPlaylistMediaRemove_Click);
			// 
			// buttonPlaylistDelete
			// 
			this.buttonPlaylistDelete.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonPlaylistDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlaylistDelete.Location = new System.Drawing.Point(376, 40);
			this.buttonPlaylistDelete.Name = "buttonPlaylistDelete";
			this.buttonPlaylistDelete.Size = new System.Drawing.Size(24, 23);
			this.buttonPlaylistDelete.TabIndex = 16;
			this.buttonPlaylistDelete.Text = "x";
			this.buttonPlaylistDelete.Click += new System.EventHandler(this.buttonPlaylistDelete_Click);
			// 
			// buttonPlaylistAdd
			// 
			this.buttonPlaylistAdd.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonPlaylistAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonPlaylistAdd.Location = new System.Drawing.Point(376, 8);
			this.buttonPlaylistAdd.Name = "buttonPlaylistAdd";
			this.buttonPlaylistAdd.Size = new System.Drawing.Size(24, 23);
			this.buttonPlaylistAdd.TabIndex = 15;
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
			this.listViewPlaylists.Size = new System.Drawing.Size(360, 72);
			this.listViewPlaylists.TabIndex = 14;
			this.listViewPlaylists.View = System.Windows.Forms.View.Details;
			this.listViewPlaylists.Resize += new System.EventHandler(this.listViewPlaylists_Resize);
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
			// mediaList
			// 
			this.mediaList.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.mediaList.Location = new System.Drawing.Point(8, 88);
			this.mediaList.Name = "mediaList";
			this.mediaList.Size = new System.Drawing.Size(360, 144);
			this.mediaList.TabIndex = 19;
			// 
			// Playlists
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(408, 238);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.mediaList,
																		  this.buttonPlaylistMediaRemove,
																		  this.buttonPlaylistDelete,
																		  this.buttonPlaylistAdd,
																		  this.listViewPlaylists});
			this.Name = "Playlists";
			this.Text = "Playlists";
			this.ResumeLayout(false);

		}
		#endregion

		public void buttonPlaylistMediaRemove_Click(object sender, System.EventArgs e)
		{
			if (mediaList.SelectedItems.Count == 0)
			{
				MessageBox.Show("You must select items to remove.", "Remove Playlist Items", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Get the current playlist id
			PlaylistListViewItem playlistItem = (PlaylistListViewItem) listViewPlaylists.SelectedItems[0];

			SqlConnection cn = new SqlConnection(player.client.ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_PlaylistMedia_Remove", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PlaylistMediaId", SqlDbType.Int);

			cn.Open();
			foreach (PlaylistMediaListViewItem item in new IterIsolate(mediaList.SelectedItems))
			{
				cmd.Parameters["@PlaylistMediaId"].Value = item.PlaylistMediaId;                
				cmd.ExecuteNonQuery();
				mediaList.Items.Remove(item);
			}
			cn.Close();

		}

		public void LoadPlaylists()
		{
			SqlConnection cn = new SqlConnection(player.client.ConnectionString);
			SqlDataAdapter da = new SqlDataAdapter("s_Playlist_List", cn);
			da.SelectCommand.CommandType = CommandType.StoredProcedure;

			listViewPlaylists.Items.Clear();

			DataSetPlaylist dsPlaylist = new DataSetPlaylist();
			da.Fill(dsPlaylist, "Playlist");
			
			foreach (DataSetPlaylist.PlaylistRow row in dsPlaylist.Playlist)
			{
				PlaylistListViewItem item = new PlaylistListViewItem(player, row);
				listViewPlaylists.Items.Add(item);
			}
            
			mediaList.Clear();
		}

		private void buttonPlaylistAdd_Click(object sender, System.EventArgs e)
		{
			PlaylistEditor ed = new PlaylistEditor();
			if (ed.ShowDialog(this) == DialogResult.OK)
			{
				SqlConnection cn = new SqlConnection(player.client.ConnectionString);
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
				SqlConnection cn = new SqlConnection(player.client.ConnectionString);
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

		public void LoadPlaylistMedia(int playlistId)
		{
			// clear the curren tlist
			mediaList.Clear();
            
			SqlConnection cn = new SqlConnection(player.client.ConnectionString);
			SqlCommand cmd   = new SqlCommand("s_PlaylistMedia_List", cn);
			cmd.CommandType  = CommandType.StoredProcedure;
			cmd.Parameters.Add("@PlaylistId", playlistId);

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read())
			{
				DataSetMedia.MediaRow row = player.client.FindMediaRow(Convert.ToInt32(dr["MediaId"]));
				PlaylistMediaListViewItem item = new PlaylistMediaListViewItem(player, row, Convert.ToInt32(dr["PlaylistMediaId"]));
				mediaList.AddItem(item);
			}
			dr.Close();
			cn.Close();

		}

		private void listViewPlaylists_Resize(object sender, System.EventArgs e)
		{
			listViewPlaylists.Columns[0].Width = (int)((listViewPlaylists.Width-22) * 0.50);
			listViewPlaylists.Columns[1].Width = (int)((listViewPlaylists.Width-22) * 0.15);
			listViewPlaylists.Columns[2].Width = (int)((listViewPlaylists.Width-22) * 0.15);
			listViewPlaylists.Columns[3].Width = (int)((listViewPlaylists.Width-22) * 0.10);
			listViewPlaylists.Columns[4].Width = (int)((listViewPlaylists.Width-22) * 0.10);
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

			foreach (PlaylistMediaListViewItem item in mediaList.Items)
			{
				player.client.mediaServer.AddToQueue(item.Entry.MediaId, offset);
				offset++;
			}
			player.client.mediaServer.Next();
		}

		private void listViewPlaylistMedia_DoubleClick(object sender, System.EventArgs e)
		{
			if (mediaList.SelectedItems.Count > 0)
			{
                MediaListViewItem item = (MediaListViewItem) mediaList.SelectedItems[0];
				player.client.mediaServer.PlayMediaId(item.Entry.MediaId);
			}

		}
	}
}
