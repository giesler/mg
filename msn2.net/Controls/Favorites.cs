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
		private Guid dataId = new Guid("{40DEBAB8-3701-43d5-8447-223F8CC5763A}");

		#endregion

		#region Constructor, Disposal

		public Favorites()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			treeViewCategory.ParentShellForm	= this;
			treeViewCategory.RootNode			= ConfigurationSettings.Current.Data.Get("Favorites.Category");
			
			this.Left = Screen.PrimaryScreen.Bounds.Left + 150;
			this.Top  = Screen.PrimaryScreen.Bounds.Bottom - this.Height - 100;
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
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemAdd = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
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
			this.listViewFavorites.ContextMenu = this.contextMenu1;
			this.listViewFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewFavorites.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listViewFavorites.HideSelection = false;
			this.listViewFavorites.Location = new System.Drawing.Point(107, 0);
			this.listViewFavorites.Name = "listViewFavorites";
			this.listViewFavorites.Size = new System.Drawing.Size(309, 192);
			this.listViewFavorites.SmallImageList = this.imageList1;
			this.listViewFavorites.TabIndex = 7;
			this.listViewFavorites.View = System.Windows.Forms.View.List;
			this.listViewFavorites.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewFavorites_MouseUp);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemAdd,
																						 this.menuItemEdit,
																						 this.menuItemDelete});
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
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// Favorites
			// 
			this.AutoLayout = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 192);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listViewFavorites,
																		  this.splitter1,
																		  this.treeViewCategory});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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
			DataCollection col = e.Data.GetChildren(typeof(FavoriteConfigData));

			listViewFavorites.Items.Clear();

			foreach (Data node in col)
			{
				FavoriteListViewItem item = new FavoriteListViewItem(node);
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

			Data data = treeViewCategory.Data.Get(f.Title, f.Url, typeof(FavoriteConfigData));
			FavoriteListViewItem item = new FavoriteListViewItem(data);
			listViewFavorites.Items.Add(item);

		}

		private void menuItemEdit_Click(object sender, System.EventArgs e)
		{
			if (listViewFavorites.SelectedItems.Count == 0)
				return;

			FavoriteListViewItem item = (FavoriteListViewItem) listViewFavorites.SelectedItems[0];

			FavoriteEdit fv = new FavoriteEdit(this);
			fv.Title	= item.Data.Text;
			fv.Url		= item.Data.Url;

			if (fv.ShowDialog(this) == DialogResult.Cancel)
				return;

			item.Data.Text	= fv.Title;
			item.Data.Url		= fv.Url;

            item.Data.Save();
			            			
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			if (listViewFavorites.SelectedItems.Count == 0)
				return;

			FavoriteListViewItem item = (FavoriteListViewItem) listViewFavorites.SelectedItems[0];

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
				FavoriteListViewItem item = (FavoriteListViewItem) listViewFavorites.SelectedItems[0];

				WebBrowser webBrowser = new WebBrowser(item.Data.Text, item.Data.Url);

				webBrowser.Show();
			}
		}

		#endregion

		#region FavoritesListViewItem class

		private class FavoriteListViewItem: ListViewItem
		{
			private Data node;

			public FavoriteListViewItem(Data node)
			{
				this.node	= node;
				this.Text	= node.Text;
				this.ImageIndex = 0;
			}

			public Data Data
			{
				get { return node; }
			}
		}

		#endregion

	}

	#region FavoriteConfigData

	public class FavoriteConfigData
	{
	}

	#endregion

}

