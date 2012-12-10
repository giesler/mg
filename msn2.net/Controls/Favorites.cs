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

namespace msn2.net.Controls
{
	public class Favorites : msn2.net.Controls.ShellForm
	{
		private Crownwood.Magic.Controls.TabControl tabControl1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemAdd;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.ComponentModel.IContainer components = null;
		
		public Favorites()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			LoadGroups();

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
			this.tabControl1 = new Crownwood.Magic.Controls.TabControl();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemAdd = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiForm;
			this.tabControl1.ContextMenu = this.contextMenu1;
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.HotTextColor = System.Drawing.SystemColors.ActiveCaption;
			this.tabControl1.HotTrack = false;
			this.tabControl1.ImageList = null;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.PositionTop = true;
			this.tabControl1.SelectedIndex = -1;
			this.tabControl1.ShowArrows = true;
			this.tabControl1.ShowClose = false;
			this.tabControl1.ShrinkPagesToFit = false;
			this.tabControl1.Size = new System.Drawing.Size(416, 192);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.TextColor = System.Drawing.SystemColors.MenuText;
			this.tabControl1.SelectionChanged += new System.EventHandler(this.tabControl1_SelectionChanged);
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
			// Favorites
			// 
			this.AutoLayout = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 192);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tabControl1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Favorites";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Favorites";
			this.ResumeLayout(false);

		}
		#endregion

		private void LoadGroups()
		{
			tabControl1.TabPages.Clear();

			string [] groups = ConfigurationSettings.Current.GetItemAttribute("Favorites", "Groups").StringArray;
            
			foreach (string group in groups)
			{
//                FavoritePage page = new FavoritePage(group);
//				tabControl1.TabPages.Add(page);
			}

		}

		private void tabControl1_SelectionChanged(object sender, System.EventArgs e)
		{
			FavoritePage page = (FavoritePage) tabControl1.TabPages[tabControl1.SelectedIndex];
			page.Page_Selected(this, e);
		}

		private void menuItemAdd_Click(object sender, System.EventArgs e)
		{
//			SqlConnection cn = new SqlConnection(msn2.net.Config.ConnectionString);
//			cn.Open();
//			SqlCommand cmd = new SqlCommand("s_Favs_Group_Add", cn);
//			cmd.CommandType = CommandType.StoredProcedure;
            		
		}

		private void menuItemEdit_Click(object sender, System.EventArgs e)
		{
			
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
		
		}

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

	}
}

