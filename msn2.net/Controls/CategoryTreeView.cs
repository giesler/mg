using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using msn2.net.Common;
using msn2.net.Configuration;
using System.Data.SqlClient;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for CategoryTreeView.
	/// </summary>
	public class CategoryTreeView : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.IContainer components;
		#region Declares

		private System.Windows.Forms.TreeView treeViewCategory;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemAdd;
		private System.Windows.Forms.MenuItem menuItemProperties;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private CategoryTreeNode menuNode		= null;
		private ShellForm parentShellForm		= null;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItemRefresh;
		private CategoryTreeNode rootNode		= null;
		
		#endregion

		#region Constructors / Desctructors

		public CategoryTreeView()
		{
			InitializeComponent();
		}

		public CategoryTreeView(ShellForm parentShellForm, Data rootData)
		{
			InitializeComponent();

			this.parentShellForm	= parentShellForm;
			this.RootData			= rootData;

			menuItemProperties.Visible = false;

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

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CategoryTreeView));
			this.treeViewCategory = new System.Windows.Forms.TreeView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemAdd = new System.Windows.Forms.MenuItem();
			this.menuItemProperties = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemRefresh = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// treeViewCategory
			// 
			this.treeViewCategory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewCategory.HideSelection = false;
			this.treeViewCategory.ImageList = this.imageList1;
			this.treeViewCategory.Name = "treeViewCategory";
			this.treeViewCategory.Size = new System.Drawing.Size(150, 130);
			this.treeViewCategory.TabIndex = 0;
			this.treeViewCategory.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCategory_AfterExpand);
			this.treeViewCategory.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCategory_AfterCollapse);
			this.treeViewCategory.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewCategory_MouseUp);
			this.treeViewCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCategory_AfterSelect);
			this.treeViewCategory.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewCategory_AfterLabelEdit);
			this.treeViewCategory.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewCategory_BeforeExpand);
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
																						 this.menuItemProperties,
																						 this.menuItemDelete,
																						 this.menuItem1,
																						 this.menuItemRefresh});
			// 
			// menuItemAdd
			// 
			this.menuItemAdd.Index = 0;
			this.menuItemAdd.Text = "&Add";
			this.menuItemAdd.Click += new System.EventHandler(this.menuItemAdd_Click);
			// 
			// menuItemProperties
			// 
			this.menuItemProperties.Index = 1;
			this.menuItemProperties.Text = "&Properties";
			this.menuItemProperties.Click += new System.EventHandler(this.menuItemProperties_Click);
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Index = 2;
			this.menuItemDelete.Text = "&Delete";
			this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 3;
			this.menuItem1.Text = "-";
			// 
			// menuItemRefresh
			// 
			this.menuItemRefresh.Index = 4;
			this.menuItemRefresh.Text = "&Refresh";
			this.menuItemRefresh.Click += new System.EventHandler(this.menuItemRefresh_Click);
			// 
			// CategoryTreeView
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.treeViewCategory});
			this.Name = "CategoryTreeView";
			this.Size = new System.Drawing.Size(150, 130);
			this.Load += new System.EventHandler(this.CategoryTreeView_Load);
			this.ResumeLayout(false);

		}
		#endregion
	
		#region Load tree sections

		public void LoadChildCategories(CategoryTreeNode node)
		{

			Data data = (Data) node.Data;

			Type[] types	= new Type[1];
			types[0]		= typeof(CategoryConfigData);
//            types[1]		= typeof(msn2.net.Configuration.MessengerGroupData);

			DataCollection cats = data.GetChildren(types);

			// Clear children
			node.Nodes.Clear();

			foreach (Data child in cats)
			{
				CategoryTreeNode childNode = new CategoryTreeNode(child);

				// Add nodes we read to the parent node
				node.Nodes.Add(childNode);

				childNode.Nodes.Add(new TreeNode("<populateme>"));
				LoadChildCategories(childNode);
			}
		}

		#endregion

		#region TreeView actions

		private void treeViewCategory_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			CategoryTreeNode treeNode	= (CategoryTreeNode) e.Node;
			treeNode.Data.Text			= e.Label;
			treeNode.Data.Save();
		}

		private void treeViewCategory_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			// get children children
			foreach (TreeNode child in e.Node.Nodes)
			{
				if (child.Nodes.Count > 0 && child.Nodes[0].Text == "<populateme>")
					LoadChildCategories((CategoryTreeNode) child);
			}
		}

		#endregion

		#region Context menu

		private void treeViewCategory_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				Point pt		= new Point(e.X, e.Y);
				TreeNode node	= treeViewCategory.GetNodeAt(pt);
				
				if (node != null)
				{
					menuNode = (CategoryTreeNode) node;
				}
				else
				{
					menuNode = null;
				}
				contextMenu1.Show(treeViewCategory, pt);
			}
		}

		private void menuItemAdd_Click(object sender, System.EventArgs e)
		{
			// First get a new name
			InputPrompt prompt = new InputPrompt("Enter the new category name:");
			prompt.Dialog = true;

			if (prompt.ShowShellDialog(this.parentShellForm) == DialogResult.Cancel)
				return;

			Guid newCategoryId = Guid.Empty;

			Data data = null;
			CategoryTreeNode treeNode;

			// Add node to tree at current item - if null, add at root
			if (menuNode == null)
			{
				data = rootNode.Data.Get(prompt.Value, typeof(CategoryConfigData));
				treeNode = new CategoryTreeNode(data);
				treeViewCategory.Nodes.Add(treeNode);
			}
			else
			{
				data = menuNode.Data.Get(prompt.Value, typeof(CategoryConfigData));
				treeNode = new CategoryTreeNode(data);
				menuNode.Nodes.Add(treeNode);
				menuNode.Expand();
			}

			treeViewCategory.SelectedNode = treeNode;
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			if (menuNode == null)
				return;

			if (MessageBox.Show(this, "Are you sure you want to delete the selected category?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;

			menuNode.Data.Delete();
			
			treeViewCategory.Nodes.Remove(menuNode);
		}

		private void menuItemProperties_Click(object sender, System.EventArgs e)
		{
			// TODO: InputPrompt or whatever form is set for this node type
		}

		#endregion

		#region Events

		private void treeViewCategory_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (e.Node == null || e.Node.Text == "")
				return;

            if (CategoryTreeView_AfterSelect != null)
            	CategoryTreeView_AfterSelect(this, new CategoryTreeViewEventArgs(((CategoryTreeNode) e.Node).Data));	
		}

		private void treeViewCategory_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			e.Node.ImageIndex = 1;
		}

		private void treeViewCategory_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			e.Node.ImageIndex = 0;
		}

		private void CategoryTreeView_Load(object sender, System.EventArgs e)
		{
		
		}

		private void menuItemRefresh_Click(object sender, System.EventArgs e)
		{
			this.RootData = this.rootNode.Data;
		}

		public event CategoryTreeView_AfterSelectDelegate CategoryTreeView_AfterSelect;

		#endregion

		#region Properties

		public Data RootData
		{
			get { return rootNode.Data; }
			set 
			{
				if (value == null)
					return;

				treeViewCategory.Nodes.Clear();

				rootNode = new CategoryTreeNode(value);
				treeViewCategory.Nodes.Add(rootNode);
				LoadChildCategories(rootNode);
				rootNode.Expand();

				foreach (CategoryTreeNode node in treeViewCategory.Nodes)
				{
					LoadChildCategories(node);
					if (node.Nodes.Count > 0 && node.Nodes.Count < 4)
						node.Expand();
				}
			}
		}

		public Data Data
		{
			get 
			{ 
				CategoryTreeNode treeNode = (CategoryTreeNode) this.treeViewCategory.SelectedNode;
				return treeNode.Data; 
			}
		}

		public ShellForm ParentShellForm
		{
			get { return parentShellForm; }
			set { this.parentShellForm = value; }
		}

		#endregion
	}

	public delegate void CategoryTreeView_AfterSelectDelegate(object sender, CategoryTreeViewEventArgs e);

	#region Related EventArgs Classes

	[Serializable]
	public class CategoryTreeViewEventArgs: EventArgs
	{
		private Data node = null;

		public CategoryTreeViewEventArgs(Data node)
		{
			this.node	= node;
		}

		public Data Data
		{
			get { return node; }
		}
	}

	#endregion

	#region CategoryConfigData

	public enum CategoryType
	{
		PersonalCategory,
		SharedCategory
	}

	/// <summary>
	/// Type specifier for category items in Config
	/// </summary>
	public class CategoryConfigData: msn2.net.Configuration.ConfigData
	{
		private CategoryType categoryType = CategoryType.PersonalCategory;

		public CategoryConfigData()
		{}

		public CategoryType CategoryType
		{
			get { return categoryType; }
			set { categoryType = value; }
		}
	}

	#endregion

	#region CategoryTreeNode

	public class CategoryTreeNode: TreeNode
	{
		private Data data;

		public CategoryTreeNode(Data data)
		{
			this.data = data;
			this.Text = data.Text;
		}

		public Data Data
		{
			get { return data; }
			set { data = value; }
		}
	}

	#endregion

	// TODO: Make extensible - each category has it's own expand functions to populate date
}
