using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using msn2.net;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;
using msn2.net.Common;
using msn2.net.Configuration;
using System.Resources;

namespace msn2.net.Controls
{
	public class Favorites : msn2.net.Controls.ShellForm
	{
		#region Declares

		private System.Windows.Forms.Splitter splitter1;
		private msn2.net.Controls.CategoryTreeView treeViewCategory;
		private System.Windows.Forms.ListView listViewFavorites;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemAdd;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.ImageList imageList1;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private Guid dataId = new Guid("{40DEBAB8-3701-43d5-8447-223F8CC5763A}");

		#endregion

		#region Constructor, Disposal

		public Favorites()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			treeViewCategory.ParentShellForm	= this;
			Data rootNode = ConfigurationSettings.Current.Data.Get("Favorites.Category");

			treeViewCategory.RootNode = rootNode;

		}

		public Favorites(Data data): base(data)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			Data rootNode = data.Get("Favorites.CategoryTree");

			#region Messenger Groups

			// Make sure there are children for each Messenger Group
			MessengerAPI.IMessengerGroups groups	= 
				(MessengerAPI.IMessengerGroups) ConfigurationSettings.Current.Messenger.MyGroups;
			DataCollection dataCollection			= rootNode.GetChildren(typeof(MessengerGroupData));
			foreach (MessengerAPI.IMessengerGroup group in groups)
			{
				if (!dataCollection.Contains(group.Name))
				{
					dataCollection.Add(rootNode.Get(group.Name, typeof(MessengerGroupData)));
				}

				Data currentGroupData = dataCollection[group.Name];
				DataCollection currentContacts = currentGroupData.GetChildren(typeof(MessengerContactData));

				// Make sure all users in group are correctly listed
				// Make sure contact list is correct
				foreach (MessengerAPI.IMessengerContact contact in (MessengerAPI.IMessengerContacts) group.Contacts)
				{
					if (!currentContacts.Contains(contact.SigninName) && contact.SigninName != ConfigurationSettings.Current.MySigninName)
					{
						MessengerContactData contactData = 
							new MessengerContactData(
								ConfigurationSettings.Current.GetSigninId(contact.SigninName));
						currentGroupData.Get(contact.SigninName, contactData, typeof(MessengerContactData));
					}
				}
			}

			#endregion

			treeViewCategory.ParentShellForm	= this;
			treeViewCategory.RootNode			= rootNode;

			this.listViewFavorites.ContextMenu	= contextMenu1;
		}

		public new Point DefaultLocation
		{
			get 
			{
				Point p	= new Point(this.Left, this.Top);
				p.X		= Screen.PrimaryScreen.Bounds.Left + 150;
				p.Y		= Screen.PrimaryScreen.Bounds.Bottom - this.Height - 100;
				return p;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Favorites));
			this.treeViewCategory = new msn2.net.Controls.CategoryTreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.listViewFavorites = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemAdd = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// timerFadeOut
			// 
			this.timerFadeOut.Enabled = false;
			// 
			// timerFadeIn
			// 
			this.timerFadeIn.Enabled = false;
			// 
			// treeViewCategory
			// 
			this.treeViewCategory.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewCategory.Name = "treeViewCategory";
			this.treeViewCategory.ParentShellForm = null;
			this.treeViewCategory.RootNode = null;
			this.treeViewCategory.Size = new System.Drawing.Size(104, 192);
			this.treeViewCategory.TabIndex = 5;
			this.treeViewCategory.CategoryTreeView_AfterSelect += new msn2.net.Controls.CategoryTreeView_AfterSelectDelegate(this.treeViewCategory_CategoryTreeView_AfterSelect);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(104, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 192);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// listViewFavorites
			// 
			this.listViewFavorites.AllowDrop = true;
			this.listViewFavorites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.columnHeader1,
																								this.columnHeader2});
			this.listViewFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewFavorites.HideSelection = false;
			this.listViewFavorites.Location = new System.Drawing.Point(107, 0);
			this.listViewFavorites.Name = "listViewFavorites";
			this.listViewFavorites.Size = new System.Drawing.Size(309, 192);
			this.listViewFavorites.SmallImageList = this.imageList1;
			this.listViewFavorites.TabIndex = 7;
			this.listViewFavorites.View = System.Windows.Forms.View.Details;
			this.listViewFavorites.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewFavorites_MouseUp);
			this.listViewFavorites.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewFavorites_DragDrop);
			this.listViewFavorites.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewFavorites_DragEnter);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 200;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Type";
			this.columnHeader2.Width = 200;
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemAdd,
																						 this.menuItemEdit,
																						 this.menuItemDelete});
			this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
			// 
			// menuItemAdd
			// 
			this.menuItemAdd.Index = 0;
			this.menuItemAdd.Text = "&Add";
			this.menuItemAdd.Click += new System.EventHandler(this.menuItemAdd_Click);
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.Index = 1;
			this.menuItemEdit.Text = "&Edit";
			this.menuItemEdit.Click += new System.EventHandler(this.menuItemEdit_Click);
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Index = 2;
			this.menuItemDelete.Text = "&Delete";
			this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
			// 
			// Favorites
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 192);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listViewFavorites,
																		  this.splitter1,
																		  this.treeViewCategory});
			this.Name = "Favorites";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Favorites";
			this.TitleVisible = true;
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Category Tree View actions

		private void treeViewCategory_CategoryTreeView_AfterSelect(object sender, msn2.net.Controls.CategoryTreeViewEventArgs e)
		{
			Type[] types = new Type[4];
			types[0]	= typeof(FavoriteConfigData);
			types[1]	= typeof(NoteConfigData);
			types[2]	= typeof(msn2.net.Configuration.MessengerContactData);
			types[3]	= typeof(msn2.net.Configuration.MessengerGroupData);

			DataCollection col = e.Data.GetChildren(types);

			listViewFavorites.Items.Clear();

			foreach (Data node in col)
			{
				DataListViewItem item = new DataListViewItem(node);
				listViewFavorites.Items.Add(item);
			}
		}

		#endregion

		#region Context menu / mouse actions

		private void menuItemAdd_Click(object sender, System.EventArgs e)
		{
			FavoriteEdit f = new FavoriteEdit(this);

			if (f.ShowDialog(this) == DialogResult.Cancel)
				return;

			Data data = treeViewCategory.Data.Get(f.Title, f.Url, new FavoriteConfigData(), typeof(FavoriteConfigData));
			DataListViewItem item = new DataListViewItem(data);
			listViewFavorites.Items.Add(item);
		}

		public void menuItemAdd_Note_Click(object sender, System.EventArgs e)
		{
			// Attempt to create new item
			Data data = Notes.Add(this, treeViewCategory.Data);
			if (data != null)
			{
				// Add to listview
				DataListViewItem item = new DataListViewItem(data);
				listViewFavorites.Items.Add(item);			
			}
		}

		private void menuItemEdit_Click(object sender, System.EventArgs e)
		{
			if (listViewFavorites.SelectedItems.Count == 0)
				return;

			DataListViewItem item = (DataListViewItem) listViewFavorites.SelectedItems[0];

			if (item.Data.DataType == typeof(FavoriteConfigData))
			{
				FavoriteEdit fv = new FavoriteEdit(this);
				fv.Title	= item.Data.Text;
				fv.Url		= item.Data.Url;

				if (fv.ShowDialog(this) == DialogResult.Cancel)
					return;

				item.Data.Text	= fv.Title;
				item.Data.Url		= fv.Url;

				item.Data.Save();
			}
			else
			{
				Notes note = new Notes(item.Data);
				note.Show();
			}

			            			
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			if (listViewFavorites.SelectedItems.Count == 0)
				return;

			DataListViewItem item = (DataListViewItem) listViewFavorites.SelectedItems[0];

			if (MessageBox.Show("Are you sure you want to delete '" + item.Text + "'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;

			item.Data.Delete();
			listViewFavorites.Items.Remove(item);
		}

		private void listViewFavorites_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (listViewFavorites.SelectedItems.Count == 0)
				return;

			if (e.Button == MouseButtons.Left)
			{
				DataListViewItem item = (DataListViewItem) listViewFavorites.SelectedItems[0];

				Type type = item.Data.DataType;
				if (type != null)
				{
                    
					if (type == typeof(FavoriteConfigData))
					{
						WebBrowser webBrowser = new WebBrowser(item.Data.Text, item.Data.Url);
						webBrowser.Show();

					}
				}
			}
		}

		private void contextMenu1_Popup(object sender, System.EventArgs e)
		{
			this.menuItemAdd.MenuItems.Clear();

			// Add each item we want to display
			this.menuItemAdd.MenuItems.Add("Favorite", new EventHandler(menuItemAdd_Click));
			this.menuItemAdd.MenuItems.Add("Note", new EventHandler(menuItemAdd_Note_Click));

		}

		#endregion

		#region Drag and Drop URLs

		private void listViewFavorites_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void listViewFavorites_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text))
			{
				FavoriteEdit f = new FavoriteEdit(this);
				f.Url = e.Data.GetData(DataFormats.Text).ToString();

				if (f.ShowDialog(this) == DialogResult.Cancel)
					return;

				Data data = treeViewCategory.Data.Get(f.Title, f.Url, new FavoriteConfigData(), typeof(FavoriteConfigData));
				DataListViewItem item = new DataListViewItem(data);
				listViewFavorites.Items.Add(item);
			}
		}

		#endregion

	}

	#region DataListViewItem

	public class DataListViewItem: ListViewItem
	{
		private Data data = null;

		public DataListViewItem(Data data)
		{
			this.Text		= data.Name;
			this.data		= data;

			this.ImageIndex	= data.ConfigData.IconIndex;

			this.SubItems.Add(data.DataType.ToString());
		}

		public Data Data 
		{ 
			get { return data; }
		}
	}

	#endregion

	#region FavoriteConfigData

	public class FavoriteConfigData: ConfigData
	{
		public override int IconIndex
		{
			get { return 1; }
		}
	}

	#endregion

}

