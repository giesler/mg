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
		private msn2.net.Common.Tree categories = null;
		private System.Windows.Forms.TreeView treeViewCategory;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemAdd;
		private System.Windows.Forms.MenuItem menuItemProperties;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private msn2.net.Common.CategoryTreeNode menuNode = null;
		private Guid treeId	= Guid.NewGuid();
		
		#endregion

		#region Constructors / Desctructors

		public CategoryTreeView()
		{
			InitializeComponent();
		}

		public CategoryTreeView(Guid treeId)
		{
			InitializeComponent();

			this.TreeId = treeId;
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

		private void LoadCategories()
		{
			foreach (TreeNode cat in categories.GetRootCategories())
			{
				CategoryTreeNode node = (CategoryTreeNode) cat;
				treeViewCategory.Nodes.Add(node);
				LoadChildCategories(node);

				// call the function to expnad root cats and populate their children
				treeViewCategory_BeforeExpand(this, new TreeViewCancelEventArgs(node, false, TreeViewAction.Expand));
			}
		}

		public void LoadChildCategories(TreeNode node)
		{
			node.Nodes.Clear();
			CategoryTreeNode catNode = (CategoryTreeNode) node;

			CategoryTreeNodeCollection cats = categories.GetChildCategories(catNode.CategoryId);

			foreach (CategoryTreeNode cat in cats)
			{
				CategoryTreeNode child = (CategoryTreeNode) cat;
				node.Nodes.Add(child);
				child.Nodes.Add(new TreeNode("<populateme>"));
				//LoadChildCategories(child);
			}
		}

		#endregion

		#region TreeView actions

		private void treeViewCategory_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			CategoryTreeNode node = (CategoryTreeNode) e.Node;
			categories.UpdateCategory(node.CategoryId, e.Label);
		}

		private void treeViewCategory_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			// get children children
			foreach (CategoryTreeNode child in e.Node.Nodes)
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
			if (prompt.ShowDialog(this) == DialogResult.Cancel)
				return;

			Guid newCategoryId = Guid.Empty;

			// Add category
			if (menuNode == null)
			{
				newCategoryId = categories.AddRootCategory(prompt.Value);
				treeViewCategory.Nodes.Add(new CategoryTreeNode(newCategoryId, prompt.Value, Guid.Empty, ConfigurationSettings.Current.UserId));
			}
			else
			{
				newCategoryId = categories.AddChildCategory(prompt.Value, menuNode.CategoryId);
				menuNode.Nodes.Add(new CategoryTreeNode(newCategoryId, prompt.Value, menuNode.CategoryId, menuNode.UserId));
			}
		}

		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			if (menuNode == null)
				return;

			if (MessageBox.Show("Are you sure you want to delete the selected category?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				return;

			categories.DeleteCategory(menuNode.CategoryId);
			
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
            	CategoryTreeView_AfterSelect(this, new CategoryTreeViewEventArgs((CategoryTreeNode) e.Node));	
		}

		public event CategoryTreeView_AfterSelectDelegate CategoryTreeView_AfterSelect;

		#endregion

		#region Properties

		public Guid TreeId
		{
			get { return treeId; }
			set 
			{
				categories = new msn2.net.Common.Tree(value, ConfigurationSettings.Current.UserId);
				LoadCategories();
			}
		}

		#endregion
	}

	public delegate void CategoryTreeView_AfterSelectDelegate(object sender, CategoryTreeViewEventArgs e);

	[Serializable]
	public class CategoryTreeViewEventArgs: EventArgs
	{
		private CategoryTreeNode node = null;

		public CategoryTreeViewEventArgs(CategoryTreeNode node)
		{
			this.node	= node;
		}

		public CategoryTreeNode CategoryTreeNode
		{
			get { return node; }
		}
	}
}
