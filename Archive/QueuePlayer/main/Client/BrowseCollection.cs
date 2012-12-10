using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using msn2.net.QueuePlayer.Shared;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for BrowseCollection.
	/// </summary>
	public class BrowseCollection : msn2.net.Controls.ShellForm
	{
		#region Declares

		private System.Windows.Forms.TreeView treeViewCollection;
		private System.ComponentModel.Container components = null;
		private msn2.net.QueuePlayer.Client.MediaListView mediaList;

		#endregion

		#region Constructor

		public BrowseCollection()
		{
			InitializeComponent();

			mediaList.lv.ContextMenu = QueuePlayerClient.Player.contextMenuMediaList;
		}

		#endregion

		#region Disposal

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

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.treeViewCollection = new System.Windows.Forms.TreeView();
			this.mediaList = new msn2.net.QueuePlayer.Client.MediaListView();
			this.SuspendLayout();
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
			this.treeViewCollection.Size = new System.Drawing.Size(192, 222);
			this.treeViewCollection.TabIndex = 3;
			this.treeViewCollection.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCollection_AfterSelect);
			this.treeViewCollection.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewCollection_BeforeExpand);
			// 
			// mediaList
			// 
			this.mediaList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mediaList.Location = new System.Drawing.Point(192, 0);
			this.mediaList.Name = "mediaList";
			this.mediaList.Size = new System.Drawing.Size(320, 222);
			this.mediaList.TabIndex = 4;
			this.mediaList.MediaDoubleClick += new System.EventHandler(this.mediaList_MediaDoubleClick);
			// 
			// BrowseCollection
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 222);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.mediaList,
																		  this.treeViewCollection});
			this.Name = "BrowseCollection";
			this.Text = "BrowseCollection";
			this.ResumeLayout(false);

		}
		#endregion

		#region Other Events

		private void treeViewCollection_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			SqlConnection cn = new SqlConnection(QueuePlayerClient.Player.client.ConnectionString);

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
			mediaList.Clear();

			if (e.Node is ArtistTreeNode) 
			{
				ArtistTreeNode artistNode = e.Node as ArtistTreeNode;

				DataView dv = new DataView(QueuePlayerClient.Player.client.dsMedia.Media, "Artist = '" + artistNode.Artist.Replace("'", "''") + "'", "Name", DataViewRowState.CurrentRows);
				foreach (DataRowView rowView in dv) 
				{
					DataSetMedia.MediaRow row = (DataSetMedia.MediaRow) rowView.Row;
					DataSetMedia.MediaRow currentRow = QueuePlayerClient.Player.client.FindMediaRow(row.MediaId);
					mediaList.AddItem(new MediaListViewItem(QueuePlayerClient.Player, currentRow));
				}

			} 
			else if (e.Node is AlbumTreeNode) 
			{
				AlbumTreeNode albumNode = e.Node as AlbumTreeNode;

				DataView dv = new DataView(QueuePlayerClient.Player.client.dsMedia.Media, "Album = '" + albumNode.Album.Replace("'", "''") + "'", "Album", DataViewRowState.CurrentRows);
				dv.Sort = "Track";

				foreach (DataRowView rowView in dv) 
				{
					DataSetMedia.MediaRow row = (DataSetMedia.MediaRow) rowView.Row;
					DataSetMedia.MediaRow currentRow = QueuePlayerClient.Player.client.FindMediaRow(row.MediaId);
					mediaList.AddItem(new MediaListViewItem(QueuePlayerClient.Player, currentRow));
				}

			}
			else if (e.Node is GenreTreeNode)
			{
				GenreTreeNode genreNode = e.Node as GenreTreeNode;

				DataView dv = new DataView(QueuePlayerClient.Player.client.dsMedia.Media, "Genre = '" + genreNode.Genre + "'", "Genre", DataViewRowState.CurrentRows);
				dv.Sort = "Track";

				foreach (DataRowView rowView in dv) 
				{
					DataSetMedia.MediaRow row = (DataSetMedia.MediaRow) rowView.Row;
					mediaList.AddItem(new MediaListViewItem(QueuePlayerClient.Player, row));
				}
			}

		}

		
		private void mediaList_MediaDoubleClick(object sender, System.EventArgs e)
		{
			if (mediaList.SelectedItems.Count > 0)
			{
				MediaListViewItem item = (MediaListViewItem) mediaList.SelectedItems[0];
				QueuePlayerClient.Player.client.mediaServer.PlayMediaId(item.Entry.MediaId);
			}

		}

		#endregion

	}
}
