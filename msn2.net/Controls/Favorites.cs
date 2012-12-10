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

		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemAdd;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.ImageList imageList1;
		private System.ComponentModel.IContainer components = null;
		private msn2.net.Controls.ShellListView shellListView;
		private Guid dataId = new Guid("{40DEBAB8-3701-43d5-8447-223F8CC5763A}");
		private System.Windows.Forms.Splitter splitter1;
		private msn2.net.Controls.CategoryTreeView treeViewCategory;
		//	private Crownwood.Magic.Docking.DockingManager dockingManager = null;

		#endregion

		#region Constructor

		public Favorites()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.treeViewCategory.CategoryTreeView_AfterSelect += new msn2.net.Controls.CategoryTreeView_AfterSelectDelegate(this.treeViewCategory_CategoryTreeView_AfterSelect);

			treeViewCategory.ParentShellForm	= this;
			Data rootNode = ConfigurationSettings.Current.Data.Get("Favorites.Category");

			treeViewCategory.RootData = rootNode;

			shellListView.Data					= rootNode;

			InternalConstructor();
		}

		public Favorites(Data data): base(data)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			treeViewCategory.CategoryTreeView_AfterSelect += new msn2.net.Controls.CategoryTreeView_AfterSelectDelegate(this.treeViewCategory_CategoryTreeView_AfterSelect);

			Data rootNode = data.Get("msn2.net");

			#region Messenger Groups

			if (ConfigurationSettings.Current.Messenger != null)
			{
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
			}

			#endregion

			treeViewCategory.RootData			= rootNode;

			shellListView.Data					= rootNode;

			InternalConstructor();
		}

		private void InternalConstructor()
		{
			
			shellListView.Dock					= DockStyle.Fill;

			treeViewCategory.ParentShellForm	= this;
			shellListView.ParentShellForm		= this;

			
			//			dockingManager = new Crownwood.Magic.Docking.DockingManager(this, Crownwood.Magic.Common.VisualStyle.IDE);

			//			Crownwood.Magic.Docking.Content content;
			
			//			content = new Crownwood.Magic.Docking.Content(dockingManager, treeViewCategory, "Location");
			//			dockingManager.AddContentWithState(content, Crownwood.Magic.Docking.State.DockLeft);
			//			content.DisplaySize = new Size(200, 200);
			//			dockingManager.ShowContent(content);
			
		}

		#endregion

		#region Default location

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

		#endregion

		#region Disposal

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
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemAdd = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.shellListView = new msn2.net.Controls.ShellListView();
			this.treeViewCategory = new msn2.net.Controls.CategoryTreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
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
			// 
			// menuItemAdd
			// 
			this.menuItemAdd.Index = 0;
			this.menuItemAdd.Text = "&Add";
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.Index = 1;
			this.menuItemEdit.Text = "&Edit";
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Index = 2;
			this.menuItemDelete.Text = "&Delete";
			// 
			// shellListView
			// 
			this.shellListView.Data = null;
			this.shellListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.shellListView.Location = new System.Drawing.Point(153, 16);
			this.shellListView.Name = "shellListView";
			this.shellListView.ParentShellForm = null;
			this.shellListView.Size = new System.Drawing.Size(263, 176);
			this.shellListView.TabIndex = 7;
			// 
			// treeViewCategory
			// 
			this.treeViewCategory.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewCategory.Name = "treeViewCategory";
			this.treeViewCategory.ParentShellForm = null;
			this.treeViewCategory.Size = new System.Drawing.Size(150, 192);
			this.treeViewCategory.TabIndex = 8;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(150, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 192);
			this.splitter1.TabIndex = 9;
			this.splitter1.TabStop = false;
			// 
			// Favorites
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 192);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.shellListView,
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
			shellListView.Data	= e.Data;
			//new MenuItem(
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

			if (data.ConfigData != null)
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

	public class FavoriteConfigData: msn2.net.Configuration.ConfigData
	{

		public override int IconIndex
		{
			get { return 1; }
		}

		public FavoriteConfigData()
		{
		}

		#region Add

		public static new Data Add(object sender, ConfigDataAddEventArgs e)
		{
			FavoriteEdit f = new FavoriteEdit();

			if (f.ShowShellDialog(e.Owner) == DialogResult.Cancel)
				return null;

			FavoriteConfigData favConfigData = new FavoriteConfigData();
			return e.Parent.Get(f.Title, f.Url, favConfigData, typeof(FavoriteConfigData));
		}

		#endregion

		#region Edit

		public static void Edit(object sender, ConfigDataAddEventArgs e)
		{
			FavoriteEdit fv = new FavoriteEdit(e.Parent);

			if (fv.ShowShellDialog(e.Owner) == DialogResult.Cancel)
			{
				e.Cancel = true;
				return;
			}
			return; // e.Parent;

		}

		#endregion
	}

	#endregion

	#region FileConfigData
	
	public class FileConfigData: ConfigData
	{
		public FileConfigData(): base()
		{
            base.AddShellAction(new ShellAction("Open", "Open file", "Opens the selected file", new EventHandler(OpenEventHandler)));
		}

		public void OpenEventHandler(object sender, EventArgs e)
		{
		}
	}

	#endregion	

}

