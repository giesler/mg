using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using msn2.net.QueuePlayer.Shared;
using msn2.net.Common;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for Search.
	/// </summary>
	public class Search : msn2.net.Controls.ShellForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ComboBox comboBoxSearch;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.TextBox textBoxSearch;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Button buttonNewSearch;
		private msn2.net.QueuePlayer.Client.MediaListView mediaList;

		private UMPlayer player;

		public Search(UMPlayer player)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.player = player;
			this.mediaList.lv.ContextMenu = player.contextMenuMediaList;

			// Add search types
			comboBoxSearch.Items.Add("any field");
			comboBoxSearch.Items.Add("artist");
            comboBoxSearch.Items.Add("song name");
			comboBoxSearch.Items.Add("album name");
			comboBoxSearch.Items.Add("comments");
			comboBoxSearch.Items.Add("duplicate song names");
			comboBoxSearch.Items.Add("SQL");
			comboBoxSearch.Items.Add("Missing files");

			comboBoxSearch.SelectedIndex = 0;
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonNewSearch = new System.Windows.Forms.Button();
			this.comboBoxSearch = new System.Windows.Forms.ComboBox();
			this.buttonSearch = new System.Windows.Forms.Button();
			this.textBoxSearch = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.mediaList = new msn2.net.QueuePlayer.Client.MediaListView();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.buttonNewSearch,
																				 this.comboBoxSearch,
																				 this.buttonSearch,
																				 this.textBoxSearch,
																				 this.label1});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(424, 80);
			this.panel1.TabIndex = 12;
			// 
			// buttonNewSearch
			// 
			this.buttonNewSearch.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonNewSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonNewSearch.Location = new System.Drawing.Point(361, 40);
			this.buttonNewSearch.Name = "buttonNewSearch";
			this.buttonNewSearch.Size = new System.Drawing.Size(56, 24);
			this.buttonNewSearch.TabIndex = 4;
			this.buttonNewSearch.Text = "New...";
			this.buttonNewSearch.Click += new System.EventHandler(this.buttonNewSearch_Click);
			// 
			// comboBoxSearch
			// 
			this.comboBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.comboBoxSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSearch.ItemHeight = 13;
			this.comboBoxSearch.Location = new System.Drawing.Point(72, 8);
			this.comboBoxSearch.Name = "comboBoxSearch";
			this.comboBoxSearch.Size = new System.Drawing.Size(280, 21);
			this.comboBoxSearch.TabIndex = 1;
			this.comboBoxSearch.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearch_SelectedIndexChanged);
			// 
			// buttonSearch
			// 
			this.buttonSearch.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonSearch.Location = new System.Drawing.Point(361, 8);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(56, 24);
			this.buttonSearch.TabIndex = 3;
			this.buttonSearch.Text = "&Search";
			this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
			// 
			// textBoxSearch
			// 
			this.textBoxSearch.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxSearch.BackColor = System.Drawing.SystemColors.Window;
			this.textBoxSearch.ForeColor = System.Drawing.SystemColors.ControlText;
			this.textBoxSearch.Location = new System.Drawing.Point(72, 32);
			this.textBoxSearch.Multiline = true;
			this.textBoxSearch.Name = "textBoxSearch";
			this.textBoxSearch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxSearch.Size = new System.Drawing.Size(280, 40);
			this.textBoxSearch.TabIndex = 2;
			this.textBoxSearch.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Search for:";
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 80);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(424, 3);
			this.splitter1.TabIndex = 13;
			this.splitter1.TabStop = false;
			// 
			// mediaList
			// 
			this.mediaList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mediaList.Location = new System.Drawing.Point(0, 83);
			this.mediaList.Name = "mediaList";
			this.mediaList.Size = new System.Drawing.Size(424, 219);
			this.mediaList.TabIndex = 14;
			this.mediaList.MediaDoubleClick += new System.EventHandler(this.mediaList_MediaDoubleClick);
			// 
			// Search
			// 
			this.AcceptButton = this.buttonSearch;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(424, 302);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.mediaList,
																		  this.splitter1,
																		  this.panel1});
			this.Name = "Search";
			this.Text = "Search";
			this.Load += new System.EventHandler(this.Search_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
            mediaList.Clear();

			if (comboBoxSearch.SelectedIndex < 7)
				SQLSearch();
			else if (comboBoxSearch.SelectedIndex == 7)
				MissingFilesSearch();
		}

		private void SQLSearch()
		{
			if (textBoxSearch.Text.Length == 0)
			{
				MessageBox.Show("You must enter text to search!", "Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			StringBuilder sb = new StringBuilder();

			if (comboBoxSearch.SelectedIndex < 5)
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
				case 5: case 6:		// SQL
					sb.Append(textBoxSearch.Text);
					break;
			}

			if (comboBoxSearch.SelectedIndex < 5)
				sb.Append(") order by Album, Track, Artist, Name");

			SqlConnection cn = new SqlConnection(player.client.ConnectionString);
			SqlCommand cmd = new SqlCommand(sb.ToString(), cn);

			if (comboBoxSearch.SelectedIndex < 5)
				cmd.Parameters.Add("@SearchString", textBoxSearch.Text);

			mediaList.Clear();

			cn.Open();
			SqlDataReader dr = cmd.ExecuteReader();
			while (dr.Read()) 
			{
				DataSetMedia.MediaRow row = player.client.FindMediaRow(Convert.ToInt32(dr[0]));
				if (row != null)
					mediaList.AddItem(new MediaListViewItem(player, row));
			}
			dr.Close();
			cn.Close();

			textBoxSearch.SelectAll();
			textBoxSearch.Focus();
		}

		public void MissingFilesSearch()
		{
			Status status = new Status("Checking files...", player.client.dsMedia.Media.Rows.Count);
			string fileShare = player.client.mediaServer.ShareDirectory + Path.DirectorySeparatorChar;
            
			mediaList.Clear();

			foreach (DataSetMedia.MediaRow row in new IterIsolate(player.client.dsMedia.Media))
			{
				if (row.RowState != DataRowState.Deleted)
				{
					if (!File.Exists(fileShare + row.MediaFile))
					{
						mediaList.AddItem(new MediaListViewItem(player, row));
					}
				}
				status.Increment(1);
				status.Refresh();

				if (status.Cancel)
					break;
			}

			status.Hide();
			status.Dispose();
		}

		private void textBoxSearch_Enter(object sender, System.EventArgs e)
		{
			this.AcceptButton = buttonSearch;
		}

		private void textBoxSearch_Leave(object sender, System.EventArgs e)
		{
			this.AcceptButton = null;		
		}

		private void comboBoxSearch_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (comboBoxSearch.SelectedIndex < 7)
				textBoxSearch.ReadOnly = false;
			else
				textBoxSearch.ReadOnly = true;

			if (comboBoxSearch.SelectedIndex == 5)
				textBoxSearch.Text = "select mediaid from media where deleted=0 and name in (select name from media where deleted=0 group by name having count(*) > 1) order by name";
		}

		private void buttonNewSearch_Click(object sender, System.EventArgs e)
		{
			player.NewSearch();
		}

		private void Search_Load(object sender, System.EventArgs e)
		{
			textBoxSearch.Focus();
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
