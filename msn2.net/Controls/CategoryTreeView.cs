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
		#region Declares

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TreeView treeViewCategory;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemAdd;
		private System.Windows.Forms.MenuItem menuItemProperties;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private msn2.net.Configuration.Data menuNode = null;
		private ShellForm parentShellForm = null;
		private Data rootNode		= null;
		
		#endregion

		#region Constructors / Desctructors

		public CategoryTreeView()
		{
			InitializeComponent();
		}

		public CategoryTreeView(ShellForm parentShellForm, Data rootNode)
		{
			InitializeComponent();

			this.parentShellForm	= parentShellForm;
			this.RootNode			= rootNode;

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
			this.treeViewCategory = new System.Windows.Forms.TreeView();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemAdd = new System.Windows.Forms.MenuItem();
			this.menuItemProperties = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// treeViewCategory
			// 
			this.treeViewCategory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewCategory.HideSelection = false;
			this.treeViewCategory.ImageIndex = -1;
			this.treeViewCategory.Name = "treeViewCategory";
			this.treeViewCategory.SelectedImageIndex = -1;
			this.treeViewCategory.Size = new System.Drawing.Size(150, 150);
			this.treeViewCategory.TabIndex = 0;
			this.treeViewCategory.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewCategory_MouseUp);
			this.treeViewCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCategory_AfterSelect);
			this.treeViewCategory.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewCategory_AfterLabelEdit);
			this.treeViewCategory.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewCategory_BeforeExpand);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemAdd,
																						 this.menuItemProperties,
																						 this.menuItemDelete});
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
			// CategoryTreeView
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.treeViewCategory});
			this.Name = "CategoryTreeView";
			this.ResumeLayout(false);

		}
		#endregion
	
		#region Load tree sections

//		private void LoadCategories()
//		{
//			foreach (Data cat in categories.GetRootNodes(typeof(CategoryConfigData)))
//			{
//				treeViewCategory.Nodes.Add(cat);
//				LoadChildCategories(cat);
//
//				// call the function to expnad root cats and populate their children
//				treeViewCategory_BeforeExpand(this, new TreeViewCancelEventArgs(cat, false, TreeViewAction.Expand));
//			}
//		}

		public void LoadChildCategories(TreeNode node)
		{
			if (node != null)
			{
				node.Nodes.Clear();
			}
			else
			{
				node = rootNode;
			}

			Data data = (Data) node;

			DataCollection cats = data.GetChildren(typeof(CategoryConfigData));

			foreach (Data cat in cats)
			{
				Data child = (Data) cat;

				// If we aren't at the root node, add to current node, else add to treeView
				if (node != rootNode)
				{
					node.Nodes.Add(child);
				} 
				else
				{
					treeViewCategory.Nodes.Add(child);
				}
				child.Nodes.Add(new TreeNode("<populateme>"));
				//LoadChildCategories(child);
			}
		}

		#endregion

		#region TreeView actions

		private void treeViewCategory_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			Data data = (Data) e.Node;
			data.Text	= e.Label;
			data.Save();
		}

		private void treeViewCategory_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			// get children children
			foreach (TreeNode child in e.Node.Nodes)
			{
				if (child.Nodes.Count > 0 && child.Nodes[0].Text == "<populateme>")
					LoadChildCategories(child);
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
					menuNode = (Data) node;
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
			if (prompt.ShowDialog(this.parentShellForm) == DialogResult.Cancel)
				return;

			Guid newCategoryId = Guid.Empty;

			Data node = null;

			// Add node to tree at current item - if null, add at root
			if (menuNode == null)
			{
				node = rootNode.Get(prompt.Value, typeof(CategoryConfigData));
				treeViewCategory.Nodes.Add(node);
			}
			else
			{
				node = menuNode.Get(prompt.Value, typeof(CategoryConfigData));
				menuNode.Nodes.Add(node);
				menuNode.Expand();
			}

			treeViewCategory.SelectedNode = node;
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			if (menuNode == null)
				return;

			if (MessageBox.Show(this, "Are you sure you want to delete the selected category?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;

			menuNode.Delete();
			
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
            if (CategoryTreeView_AfterSelect != null)
            	CategoryTreeView_AfterSelect(this, new CategoryTreeViewEventArgs((Data) e.Node));	
		}

		public event CategoryTreeView_AfterSelectDelegate CategoryTreeView_AfterSelect;

		#endregion

		#region Properties

		public Data RootNode
		{
			get { return rootNode; }
			set 
			{
				if (value == null)
					return;

				rootNode = value;
				LoadChildCategories(value);
				foreach (TreeNode node in treeViewCategory.Nodes)
				{
					LoadChildCategories(node);
				}
			}
		}

		public Data Data
		{
			get { return (Data) this.treeViewCategory.SelectedNode; }
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

	/// <summary>
	/// Type specifier for category items in Config
	/// </summary>
	public class CategoryConfigData
	{
	}

	#endregion

}
