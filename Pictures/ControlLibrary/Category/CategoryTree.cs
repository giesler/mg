using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace msn2.net.Pictures.Controls
{

	/// <summary>
	/// Summary description for CategoryTree.
	/// </summary>
	public class CategoryTree : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TreeView tvCategory;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuAddChildCat;
		private System.Windows.Forms.MenuItem menuEditCatName;
		private System.Windows.Forms.MenuItem menuDeleteCat;
		private System.Windows.Forms.MenuItem menuAddChild;
		private System.Windows.Forms.MenuItem menuEdit;
		private System.Windows.Forms.MenuItem menuDelete;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuRefresh;
        private System.Windows.Forms.MenuItem menuSaveSlideshow;
        private MenuItem menuItem2;
        private PictureDataSet pictureDataSet1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private bool loading = false;
		
		public CategoryTree()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            this.tvCategory.ImageList = new ImageList();
            this.tvCategory.ImageList.Images.Add(CommonImages.Folder);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            RefreshTree();
        }

        private void RefreshTree()
        {
            if (this.loading == true)
            {
                MessageBox.Show("The tree is already loading.");
                return;
            }

            tvCategory.Nodes.Clear();

            this.loading = true;

            ThreadPool.QueueUserWorkItem(
                new WaitCallback(this.RefreshTreeThread),
                new object[] { });
        }

        private delegate void ExpandTreeNodeDelegate(TreeNode node);

        private void RefreshTreeThread(object o) 
		{
            try
            {
                // clear tree
                if (PicContext.Current != null)
                {
                    // load first node
                    Category rootCategory = PicContext.Current.CategoryManager.GetRootCategory();

                    CategoryTreeNode nRoot = new CategoryTreeNode(rootCategory);
                    tvCategory.HideSelection = false;

                    // load first level
                    FillChildren(nRoot, 2);

                    // Expand root node
                    this.BeginInvoke(new ExpandTreeNodeDelegate(
                        delegate(TreeNode expandNode)
                        {
                            tvCategory.Nodes.Add(expandNode);
                            expandNode.Expand();
                        }), 
                        new object[] { nRoot });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading category tree: " + ex.Message);
            }

            this.loading = false;
		}

        private delegate void AddTreeNodeDelegate(CategoryTreeNode parentNode, TreeNode newNode);

        private void AddCategoryTreeNode(CategoryTreeNode parentNode, TreeNode newNode)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(
                    new AddTreeNodeDelegate(this.AddCategoryTreeNode),
                    new object[] { parentNode, newNode });
            }
            else
            {
                if (parentNode == null)
                {
                    this.tvCategory.Nodes.Add(newNode);
                }
                else
                {
                    parentNode.Nodes.Add(newNode);
                }
            }
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

        public CategoryTreeNode SelectedNode
        {
            get
            {
                return tvCategory.SelectedNode as CategoryTreeNode;
            }
        }

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tvCategory = new System.Windows.Forms.TreeView();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuAddChild = new System.Windows.Forms.MenuItem();
            this.menuEdit = new System.Windows.Forms.MenuItem();
            this.menuDelete = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuRefresh = new System.Windows.Forms.MenuItem();
            this.menuSaveSlideshow = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuDeleteCat = new System.Windows.Forms.MenuItem();
            this.menuEditCatName = new System.Windows.Forms.MenuItem();
            this.menuAddChildCat = new System.Windows.Forms.MenuItem();
            this.pictureDataSet1 = new msn2.net.Pictures.PictureDataSet();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // tvCategory
            // 
            this.tvCategory.ContextMenu = this.contextMenu1;
            this.tvCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCategory.HideSelection = false;
            this.tvCategory.Indent = 10;
            this.tvCategory.Location = new System.Drawing.Point(0, 0);
            this.tvCategory.Name = "tvCategory";
            this.tvCategory.ShowRootLines = false;
            this.tvCategory.Size = new System.Drawing.Size(120, 92);
            this.tvCategory.TabIndex = 0;
            this.tvCategory.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvCategory_AfterLabelEdit);
            this.tvCategory.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvCategory_BeforeExpand);
            this.tvCategory.DoubleClick += new System.EventHandler(this.tvCategory_DoubleClick);
            this.tvCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCategory_AfterSelect);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAddChild,
            this.menuEdit,
            this.menuDelete,
            this.menuItem1,
            this.menuRefresh,
            this.menuSaveSlideshow,
            this.menuItem2});
            // 
            // menuAddChild
            // 
            this.menuAddChild.Index = 0;
            this.menuAddChild.Text = "&Add Child";
            this.menuAddChild.Click += new System.EventHandler(this.menuAddChildCat_Click);
            // 
            // menuEdit
            // 
            this.menuEdit.Index = 1;
            this.menuEdit.Text = "&Edit";
            this.menuEdit.Click += new System.EventHandler(this.menuEditCatName_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Index = 2;
            this.menuDelete.Text = "&Delete";
            this.menuDelete.Click += new System.EventHandler(this.menuDeleteCat_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // menuRefresh
            // 
            this.menuRefresh.Index = 4;
            this.menuRefresh.Text = "&Refresh";
            this.menuRefresh.Click += new System.EventHandler(this.menuRefresh_Click);
            // 
            // menuSaveSlideshow
            // 
            this.menuSaveSlideshow.Index = 5;
            this.menuSaveSlideshow.Text = "&Save slideshow...";
            this.menuSaveSlideshow.Click += new System.EventHandler(this.menuSaveSlideshow_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 6;
            this.menuItem2.Text = "&Publish to homepage";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuDeleteCat
            // 
            this.menuDeleteCat.Index = -1;
            this.menuDeleteCat.Text = "";
            this.menuDeleteCat.Click += new System.EventHandler(this.menuDeleteCat_Click);
            // 
            // menuEditCatName
            // 
            this.menuEditCatName.Index = -1;
            this.menuEditCatName.Text = "";
            this.menuEditCatName.Click += new System.EventHandler(this.menuEditCatName_Click);
            // 
            // menuAddChildCat
            // 
            this.menuAddChildCat.Index = -1;
            this.menuAddChildCat.Text = "";
            this.menuAddChildCat.Click += new System.EventHandler(this.menuAddChildCat_Click);
            // 
            // pictureDataSet1
            // 
            this.pictureDataSet1.DataSetName = "PictureDataSet";
            this.pictureDataSet1.Locale = new System.Globalization.CultureInfo("en-US");
            this.pictureDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // CategoryTree
            // 
            this.Controls.Add(this.tvCategory);
            this.Name = "CategoryTree";
            this.Size = new System.Drawing.Size(120, 92);
            ((System.ComponentModel.ISupportInitialize)(this.pictureDataSet1)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		private void menuAddChildCat_Click(object sender, System.EventArgs e)
		{
			CategoryTreeNode n = this.SelectedNode;
			if (n == null) 
			{
				MessageBox.Show("You must select a category to add a sub category.", "Add Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			fEditCategory ec = new fEditCategory();
            
			ec.NewCategory(n.Category.Id);

			ec.ShowDialog();

			if (!ec.Cancel) 
			{
				// add new tree node
                CategoryTreeNode newCategoryNode = new CategoryTreeNode(ec.SelectedCategory);
                n.Nodes.Add(newCategoryNode);
	
				// expand parent node and select new node
				n.Expand();
				tvCategory.SelectedNode = newCategoryNode;
			}

		}

		private void menuEditCatName_Click(object sender, System.EventArgs e)
		{
			CategoryTreeNode n = this.SelectedNode;
			if (n == null) 
			{
				MessageBox.Show("You must select a category to edit it.", "Edit Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			} 
			else if (n == tvCategory.Nodes[0]) 
			{
				return;
			}

			fEditCategory ec = new fEditCategory();
            ec.CategoryID = n.Category.Id;
            ec.ShowDialog();

			if (!ec.Cancel) 
			{
                n.Update(ec.SelectedCategory);
			}

		}

		private void menuDeleteCat_Click(object sender, System.EventArgs e)
		{
			CategoryTreeNode n = this.SelectedNode;

            DeleteCategoryNode(n);
		}

        public static void DeleteCategoryNode(CategoryTreeNode n)
        {
            if (n.Nodes.Count > 0)
            {
                MessageBox.Show("You cannot delete a category that contains categories.  Please delete the categories below it first.", "Delete Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (n == null)
            {
                MessageBox.Show("You must select a category to delete it.", "Delete Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // make sure we want to delete
            if (MessageBox.Show("Would you like to delete category '" + n.Text + "'?  No pictures will be deleted.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                PicContext.Current.CategoryManager.DeleteCategory(n.Category.Id);
                n.Parent.Nodes.Remove(n);
            }
        }

		private void tvCategory_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
            CategoryTreeNode node = e.Node as CategoryTreeNode;

            throw new NotImplementedException("Category update is not implemetned.");
		}

		private void tvCategory_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
            CategoryTreeNode node = (CategoryTreeNode)e.Node;

            ValidateChildrenLoaded(node);
		}

        private void ValidateChildrenLoaded(CategoryTreeNode node)
        {
            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "<to load>")
                FillChildren(node, 2);
        }

        private delegate void ClearChildrenNodesDelegate(CategoryTreeNode node);

		private void FillChildren(CategoryTreeNode n, int intLevelsToGo) 
		{
			// clear out all child nodes
            ClearChildrenNodesDelegate clearDelegate = delegate(CategoryTreeNode node)
            {
                node.Nodes.Clear();
            };
            this.Invoke(clearDelegate, new object[] { n });
            
			// load child nodes from dvCategory
            List<Category> categories = PicContext.Current.CategoryManager.GetCategories(
                n.Category.Id);

			foreach (Category category in categories) 
			{
                CategoryTreeNode childNode = new CategoryTreeNode(category);
                this.AddCategoryTreeNode(n, childNode);

                if (intLevelsToGo > 0)
                    this.FillChildren(childNode, intLevelsToGo - 1);
                else
                    // add a fake node to be filled in
                    this.AddCategoryTreeNode(childNode, new LoadingTreeNode());
            }

            if (CategoryChildrenLoaded != null)
            {
                CategoryChildrenLoaded(this, new CategoryTreeEventArgs(n.Category));
            }
		}

        public void SetSelectedCategory(Category category)
        {
            string path = category.Path;
            if (tvCategory.Nodes.Count > 0)
            {
                CategoryTreeNode current = tvCategory.Nodes[0] as CategoryTreeNode;
                this.SetSelectedCategory(current, category);
            }
        }

        private void SetSelectedCategory(CategoryTreeNode node, Category selectCategory)
        {
            if (!node.IsExpanded)
            {
                node.Expand();
            }
            
            // Find child node with path
            foreach (CategoryTreeNode childNode in node.Nodes)
            {
                string childNodePath = childNode.FullPath.Substring(childNode.FullPath.IndexOf(@"\"));

                if (childNodePath == selectCategory.Path)
                {
                    this.tvCategory.SelectedNode = childNode;
                    break;
                }
                else if (childNodePath.StartsWith(selectCategory.Path))
                {
                    this.SetSelectedCategory(childNode, selectCategory);
                }
            }
        }


        public Category SelectedCategory
		{
			get 
			{
                CategoryTreeNode node = this.SelectedNode;
                if (node == null)
                {
                    return null;
                }

                return node.Category;
			}

		}

		// events
		public event ClickCategoryEventHandler ClickCategory;
		public event DoubleClickCategoryEventHandler DoubleClickCategory;
        public event ClickCategoryEventHandler CategoryChildrenLoaded;

		private void tvCategory_DoubleClick(object sender, System.EventArgs e)
		{
			CategoryTreeNode n = this.SelectedNode;
			if (n == null)
				return;

			// Fire event for other controls to catch if they want
			CategoryTreeEventArgs ex = new CategoryTreeEventArgs(n.Category);
			
			if (DoubleClickCategory != null)
				DoubleClickCategory(this, ex);

		}

		private void tvCategory_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
            // Make sure children loaded
            ValidateChildrenLoaded(e.Node as CategoryTreeNode);

			if (e.Action != TreeViewAction.Unknown) 
			{
				CategoryTreeNode n = this.SelectedNode;
				if (n == null)
					return;

				// Fire event for other controls to catch if they want
				CategoryTreeEventArgs ex = new CategoryTreeEventArgs(n.Category);
			
				if (ClickCategory != null)
					ClickCategory(this, ex);

			}
		}

		private void menuRefresh_Click(object sender, System.EventArgs e)
		{
			RefreshTree();
		}

		private void menuSaveSlideshow_Click(object sender, System.EventArgs e)
		{

        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            CategoryTreeNode n = this.SelectedNode;
            if (n == null)
                return;

            PicContext.Current.CategoryManager.PublishCategory(n.Category.Id);
        }

	}

	// events
	public delegate void ClickCategoryEventHandler(object sender, CategoryTreeEventArgs e);
	public delegate void DoubleClickCategoryEventHandler(object sender, CategoryTreeEventArgs e);

	// class for passing events up
	public class CategoryTreeEventArgs: EventArgs 
	{
        private Category category;

        public CategoryTreeEventArgs(Category category)
        {
            this.category = category;
        }

        public Category Category
        {
            get
            {
                return this.category;
            }
        }
	}
    
    public class CategoryTreeNode : TreeNode
    {
        private Category category;
        public Category Category
        {
            get
            {
                return category;
            }
        }

        public CategoryTreeNode(Category category)
        {
            this.Update(category);
        }

        public void Update(Category category)
        {
            this.category = category;
            this.Text = category.Name;
        }

        internal bool IsLoadingChildren = false;
    }

    public class LoadingTreeNode : TreeNode
    {
        public LoadingTreeNode()
        {
            base.Text = "<to load>";
        }
    }

}
