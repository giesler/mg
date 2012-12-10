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
using msn2.net.Configuration;
using msn2.net.Common;

namespace msn2.net.Controls
{
	public class Favorites : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemAdd;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ListView listView1;
		private msn2.net.Controls.CategoryTreeView treeViewCategory;
		
		private System.ComponentModel.IContainer components = null;

		public Favorites()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			Guid treeId = ConfigurationSettings.Current.GetItemAttribute("Favorites", "treeId", Guid.NewGuid().ToString()).Guid;
			treeViewCategory.TreeId = treeId;

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

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemAdd = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.treeViewCategory = new msn2.net.Controls.CategoryTreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.listView1 = new System.Windows.Forms.ListView();
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
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemAdd,
																						 this.menuItemDelete});
			// 
			// menuItemAdd
			// 
			this.menuItemAdd.Index = 0;
			this.menuItemAdd.Text = "&Add";
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Index = 1;
			this.menuItemDelete.Text = "&Delete";
			// 
			// treeViewCategory
			// 
			this.treeViewCategory.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewCategory.Name = "treeViewCategory";
			this.treeViewCategory.Size = new System.Drawing.Size(104, 192);
			this.treeViewCategory.TabIndex = 5;
			this.treeViewCategory.TreeId = new System.Guid("6f9a455a-afdf-4c5a-8973-31d6dc752862");
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(104, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 192);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// listView1
			// 
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.Location = new System.Drawing.Point(107, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(309, 192);
			this.listView1.TabIndex = 7;
			// 
			// Favorites
			// 
			this.AutoLayout = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 192);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listView1,
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

		#region FavoritesPage - Tab page

		public class FavoritePage: Crownwood.Magic.Controls.TabPage
		{
			private System.Windows.Forms.ListView lv;
			private int groupId;
			private System.Windows.Forms.ImageList il = new System.Windows.Forms.ImageList();

			public FavoritePage(string title, int groupId): this(title, groupId, new System.Windows.Forms.ListView())
			{}

			public FavoritePage(string title, int groupId, System.Windows.Forms.ListView lv): base(title, lv)
			{
				il.Images.Add(new System.Drawing.Icon("ENTIRNET.ICO"));

				this.groupId = groupId;

				this.lv = lv;
				lv.FullRowSelect = true;
				lv.HoverSelection = true;
				lv.Click += new EventHandler(lv_Click);
				lv.Visible = true;
				lv.SmallImageList = il;
			
				lv.View = System.Windows.Forms.View.List;
				lv.Items.Add(groupId.ToString());

			}

			public void Page_Selected(object sender, EventArgs e)
			{
				SqlConnection cn = new SqlConnection(ConfigurationSettings.Current.ConnectionString);
				cn.Open();
				SqlCommand cmd = new SqlCommand("s_Favs_Item_List", cn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@FavGroupId", SqlDbType.NVarChar, 50);

				cmd.Parameters["@FavGroupId"].Value = groupId;
				SqlDataReader dr = cmd.ExecuteReader();

				lv.Items.Clear();
				while (dr.Read())
				{
					FavoriteListViewItem item = 
						new FavoriteListViewItem(dr["FavItemName"].ToString(),
						dr["FavItemUrl"].ToString());
					lv.Items.Add(item);
				}

				dr.Close();
				cn.Close();
			}

			private void lv_Click(object sender, EventArgs e)
			{
				if (lv.SelectedItems.Count == 0)
					return;

				FavoriteListViewItem item = (FavoriteListViewItem) lv.SelectedItems[0];

				WebBrowser webBrowser = new WebBrowser(item.Text, item.Url);

				webBrowser.Show();
//				Process p = new Process();
//				p.StartInfo = new ProcessStartInfo(item.Url);
//				p.Start();

			}

			#region FavoritesListItem

			private class FavoriteListViewItem: System.Windows.Forms.ListViewItem
			{
				private string url;

				public FavoriteListViewItem(string text, string url)
				{

					this.Text = text;
					this.url = url;
                
					this.ImageIndex = 0;		
				}
            
				public string Url 
				{
					get { return url; }
					set { url = value; }
				}
			}

			#endregion

		}

		#endregion

//		public class TreeNode: System.Windows.Forms.TreeNode
//		{
//			private Guid categoryId;
//
//			public TreeNode(Guid categoryId, string categoryName)
//			{
//				this.Text		= categoryName;
//				this.categoryId	= categoryId;
//			}
//
//			public CategoryTreeNode(TreeNode node): this(category.CategoryId, category.CategoryName)
//			{
//			}
//
//			public Guid CategoryId
//			{
//				get { return categoryId; }
//			}
//		}
	}
}

