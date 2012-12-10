using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using msn2.net.Configuration;
using msn2.net.Common;
using System.Reflection;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for ShellListView.
	/// </summary>
	public class ShellListView : System.Windows.Forms.UserControl
	{
		#region Declares

		private System.Windows.Forms.ListView listViewFavorites;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemAdd;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.ImageList imageList1;
		private System.ComponentModel.IContainer components;
		private ShellForm parentForm = null;
		private Data data = null;
		private ContextMenu contextMenu = null;
		private Type[] types = null;

		#endregion

		#region Constructors

		public ShellListView()
		{
			InternalConstructor();
		}

		public ShellListView(ShellForm parentForm, Data data)
		{
			InternalConstructor();
			
			this.parentForm			= parentForm;
			this.data				= data;
		}

		private void InternalConstructor()
		{
			InitializeComponent();

			Type[] def  = new Type[3];
			def[0]		= typeof(FavoriteConfigData);
			def[1]		= typeof(NoteConfigData);
			def[2]		= typeof(RecipeConfigData);
			this.types	= def;

			// Set up context menu
			contextMenu = new ContextMenu();
			contextMenu.Popup += new EventHandler(ContextMenu_Popup);
			this.listViewFavorites.ContextMenu	= contextMenu;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ShellListView));
			this.listViewFavorites = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemAdd = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// listViewFavorites
			// 
			this.listViewFavorites.AllowDrop = true;
			this.listViewFavorites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.columnHeader1,
																								this.columnHeader2});
			this.listViewFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewFavorites.HideSelection = false;
			this.listViewFavorites.Name = "listViewFavorites";
			this.listViewFavorites.Size = new System.Drawing.Size(150, 130);
			this.listViewFavorites.SmallImageList = this.imageList1;
			this.listViewFavorites.TabIndex = 8;
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
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemAdd,
																						 this.menuItemEdit,
																						 this.menuItemDelete});
			this.contextMenu1.Popup += new System.EventHandler(this.contentMenu1_Popup);
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
			// ShellListView
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listViewFavorites});
			this.Name = "ShellListView";
			this.Size = new System.Drawing.Size(150, 130);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		public Type[] Types
		{
			get 
			{
				return types;
			}
			set
			{
				types = value;
			}
		}

		public Data Data 
		{
			get 
			{ 
				return data;
			}
			set
			{
				data = value;

				if (data != null)
				{
					DataCollection col = data.GetChildren(types);

					listViewFavorites.Items.Clear();

					foreach (Data node in col)
					{
						DataListViewItem item = new DataListViewItem(node);
						listViewFavorites.Items.Add(item);
					}
				}
			}
		}

		public ShellForm ParentShellForm
		{
			get 
			{
				return parentForm;
			}
			set
			{
				parentForm = value;
			}
		}

		#endregion

		#region Context menu / mouse actions

		private void ContextMenu_Popup(object sender, System.EventArgs e)
		{
			
			// Populate the context menu with this type's menu
			if (listViewFavorites.SelectedItems.Count == 0)
			{
			}

			contextMenu.MenuItems.Clear();

			DataListViewItem item = null;
			if (listViewFavorites.SelectedItems.Count != 0)
			{
				item = (DataListViewItem) listViewFavorites.SelectedItems[0];
			}

			// Build add list
			MenuItem addMenu = contextMenu.MenuItems.Add("Add");
			foreach (Type t in types)
			{
				MenuItem menuItem = new MenuItem(t.ToString(), new EventHandler(menuItemAdd_Click));
				addMenu.MenuItems.Add(menuItem);
			}

			if (item != null)
			{
				// Build edit list
				MenuItem editMenu = contextMenu.MenuItems.Add("Edit", new EventHandler(menuItemEdit_Click));

				// Delete
				MenuItem deleteMenu = contextMenu.MenuItems.Add("Delete", new EventHandler(menuItemDelete_Click));
			}

		}

		private void menuItemAdd_Click(object sender, System.EventArgs e)
		{
            MenuItem item = (MenuItem) sender;
			
			// Get the type of item clicked
			foreach (Type t in types)
			{
				if (item.Text == t.ToString())
				{
					MethodInfo mi = t.GetMethod("Add");
					object[] methodParams = new object[2];
					methodParams [0] = this;
					methodParams [1] = new ConfigDataAddEventArgs(this.parentForm, this.Data);
					object retVal = mi.Invoke(new object(), methodParams );

					// Update form if user didn't cancel
					if (retVal != null)
					{
						Data newData = (Data) retVal;
						DataListViewItem listViewItem = new DataListViewItem(newData);
						listViewFavorites.Items.Add(listViewItem);
					}
				}
			}
		
		}

		private void menuItemEdit_Click(object sender, System.EventArgs e)
		{
			if (listViewFavorites.SelectedItems.Count == 0)
				return;

			DataListViewItem item = (DataListViewItem) listViewFavorites.SelectedItems[0];

			// Find out if type contains an 'Edit' method
			Type t = item.Data.ConfigData.GetType();
			MethodInfo methodInfo = t.GetMethod("Edit");
			if (methodInfo != null)
			{
				object[] methodParams = new object[2];
				methodParams [0] = this;
				methodParams [1] = new ConfigDataAddEventArgs(this.parentForm, item.Data);
				Object retVal = methodInfo.Invoke(new object(), methodParams );

				// BUGBUG: Check retVal or event args for cancel
			}

			if (item.Data.DataType == typeof(FavoriteConfigData))
			{
			}
			else if (item.Data.DataType == typeof(NoteConfigData))
			{
//				Notes note = new Notes(item.Data);
//				note.Show();
			}
			else if (item.Data.DataType == typeof(RecipeConfigData))
			{
//				Recipe recipe = new Recipe(item.Data);
//				recipe.Show();
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
				FavoriteEdit f = new FavoriteEdit();
				f.Dialog = true;
				f.Url = e.Data.GetData(DataFormats.Text).ToString();

				if (f.ShowShellDialog(this.parentForm) == DialogResult.Cancel)
					return;

				Data newData = data.Get(f.Title, f.Url, new FavoriteConfigData(), typeof(FavoriteConfigData));
				DataListViewItem item = new DataListViewItem(newData);
				listViewFavorites.Items.Add(item);
			}
		}

		private void contentMenu1_Popup(object sender, System.EventArgs e)
		{
			this.menuItemAdd.MenuItems.Clear();

			// Add each item we want to display
			this.menuItemAdd.MenuItems.Add("Favorite", new EventHandler(menuItemAdd_Click));
			//this.menuItemAdd.MenuItems.Add("Note", new EventHandler(menuItemAdd_Note_Click));
			//this.menuItemAdd.MenuItems.Add("Recipe", new EventHandler(menuItemAdd_Recipe_Click));
		}

		#endregion

	}
}
