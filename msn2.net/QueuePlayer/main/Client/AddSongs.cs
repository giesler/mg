using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using msn2.net.QueuePlayer.Shared;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for AddSongs.
	/// </summary>
	public class AddSongs : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView listViewFiles;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.Button buttonRemoveFile;
		private System.Windows.Forms.Button buttonAddFile;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.CheckBox checkBoxDeleteOnAdd;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.OpenFileDialog addFileDialog;
		private System.Windows.Forms.ContextMenu contextMenuAddFile;
		private System.Windows.Forms.MenuItem menuItemAddFile;
		private System.Windows.Forms.MenuItem menuItemAddDirectory;
		private System.Windows.Forms.MenuItem menuItemAddTree;
		private UMPlayer player;

		// declares for searching
		private string walkDirectory;
		private bool directoryRecursive;
		private Status directoryAddStatus = null;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddSongs(UMPlayer player)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.player = player;
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
			this.listViewFiles = new System.Windows.Forms.ListView();
			this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
			this.buttonRemoveFile = new System.Windows.Forms.Button();
			this.buttonAddFile = new System.Windows.Forms.Button();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.checkBoxDeleteOnAdd = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.addFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.contextMenuAddFile = new System.Windows.Forms.ContextMenu();
			this.menuItemAddFile = new System.Windows.Forms.MenuItem();
			this.menuItemAddDirectory = new System.Windows.Forms.MenuItem();
			this.menuItemAddTree = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
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
			this.listViewFiles.Size = new System.Drawing.Size(376, 168);
			this.listViewFiles.TabIndex = 15;
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
			this.buttonRemoveFile.Location = new System.Drawing.Point(392, 64);
			this.buttonRemoveFile.Name = "buttonRemoveFile";
			this.buttonRemoveFile.Size = new System.Drawing.Size(24, 23);
			this.buttonRemoveFile.TabIndex = 14;
			this.buttonRemoveFile.Text = "x";
			this.buttonRemoveFile.Click += new System.EventHandler(this.buttonRemoveFile_Click);
			// 
			// buttonAddFile
			// 
			this.buttonAddFile.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonAddFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonAddFile.Location = new System.Drawing.Point(392, 32);
			this.buttonAddFile.Name = "buttonAddFile";
			this.buttonAddFile.Size = new System.Drawing.Size(24, 23);
			this.buttonAddFile.TabIndex = 13;
			this.buttonAddFile.Text = "+";
			this.buttonAddFile.Click += new System.EventHandler(this.buttonAddFile_Click);
			// 
			// buttonAdd
			// 
			this.buttonAdd.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonAdd.Location = new System.Drawing.Point(344, 200);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.TabIndex = 12;
			this.buttonAdd.Text = "&Add";
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// checkBoxDeleteOnAdd
			// 
			this.checkBoxDeleteOnAdd.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.checkBoxDeleteOnAdd.Location = new System.Drawing.Point(8, 200);
			this.checkBoxDeleteOnAdd.Name = "checkBoxDeleteOnAdd";
			this.checkBoxDeleteOnAdd.Size = new System.Drawing.Size(248, 24);
			this.checkBoxDeleteOnAdd.TabIndex = 11;
			this.checkBoxDeleteOnAdd.Text = "&Delete files after they are added";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(488, 23);
			this.label3.TabIndex = 10;
			this.label3.Text = "Select the songs you would like to add below, then click \'Add\'.";
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
			// AddSongs
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(424, 230);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listViewFiles,
																		  this.buttonRemoveFile,
																		  this.buttonAddFile,
																		  this.buttonAdd,
																		  this.checkBoxDeleteOnAdd,
																		  this.label3});
			this.Name = "AddSongs";
			this.Text = "AddSongs";
			this.Resize += new System.EventHandler(this.AddSongs_Resize);
			this.ResumeLayout(false);

		}
		#endregion

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
			DataView dv = new DataView(player.client.dsMedia.Media);
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

		
		private void buttonAdd_Click(object sender, System.EventArgs e)
		{

			Status statusTemp = new Status("Validating files...", listViewFiles.Items.Count);
			ArrayList addFiles = new ArrayList();

			// Get the final list of files we can add
			foreach (ListViewItem item in listViewFiles.Items) 
			{
				
				string file		  = item.Tag.ToString();

				// make sure file isn't already in music collection
				if (!file.StartsWith(player.client.mediaServer.ShareDirectory)) 
				{
					// Check if the same file already exists in the music share
					DataView dv = new DataView(player.client.dsMedia.Media);
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
				string destPath = player.client.mediaServer.DropDirectory;
				if (!Directory.Exists(destPath))
					Directory.CreateDirectory(destPath);

				string destFile = "";
				destFile = fullpath.Substring(fullpath.LastIndexOf(Path.DirectorySeparatorChar)+1);

				// copy/move file only if not already in share path
				if (!fullpath.ToLower().StartsWith(player.client.mediaServer.ShareDirectory.ToLower()))
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
						if (fullpath.ToLower().StartsWith(player.client.mediaServer.ShareDirectory))
						{
							File.Move(fullpath, destFile);
						}
						else 
						{
							File.Copy(fullpath, destFile);
						}
					}

					// now tell the server
					player.client.mediaServer.AddMediaFile(destFile);

				}
					// otherwise we need to tell the server there is a new file
				else
				{
					player.client.mediaServer.AddMediaFile(fullpath);
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

		private void AddSongs_Resize(object sender, System.EventArgs e)
		{
			// Resize the file listview columns
			listViewFiles.Columns[0].Width = listViewFiles.Width - 22;
		}
	}
}
