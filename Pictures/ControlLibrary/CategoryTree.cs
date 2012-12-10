using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

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
		private System.Data.DataView dvCategory;
		private System.Data.SqlClient.SqlConnection cn;
		private System.Data.SqlClient.SqlDataAdapter daCategory;
		private System.Windows.Forms.MenuItem menuAddChild;
		private System.Windows.Forms.MenuItem menuEdit;
		private System.Windows.Forms.MenuItem menuDelete;
		private msn2.net.Pictures.Controls.DataSetCategory dsCategory;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuRefresh;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private System.Windows.Forms.MenuItem menuSaveSlideshow;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public CategoryTree()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Set the connection string
			cn.ConnectionString = Config.ConnectionString;

			RefreshTree();

		}

		public void RefreshTree () 
		{
			daCategory.Fill(dsCategory,"Category");

			// clear tree
			tvCategory.Nodes.Clear();

			// load first node
            Category rootCategory = PicContext.Current.CategoryManager.GetRootCategory();

			CategoryTreeNode nRoot = new CategoryTreeNode(rootCategory);
			tvCategory.Nodes.Add(nRoot);

            tvCategory.HideSelection = false;

            // load first level
			FillChildren(nRoot, 2);
			nRoot.Expand();	

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
			this.daCategory = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.tvCategory = new System.Windows.Forms.TreeView();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuAddChild = new System.Windows.Forms.MenuItem();
			this.menuEdit = new System.Windows.Forms.MenuItem();
			this.menuDelete = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuRefresh = new System.Windows.Forms.MenuItem();
			this.menuSaveSlideshow = new System.Windows.Forms.MenuItem();
			this.menuDeleteCat = new System.Windows.Forms.MenuItem();
			this.dvCategory = new System.Data.DataView();
			this.dsCategory = new msn2.net.Pictures.Controls.DataSetCategory();
			this.menuEditCatName = new System.Windows.Forms.MenuItem();
			this.menuAddChildCat = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.dvCategory)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dsCategory)).BeginInit();
			this.SuspendLayout();
			// 
			// daCategory
			// 
			this.daCategory.DeleteCommand = this.sqlDeleteCommand1;
			this.daCategory.InsertCommand = this.sqlInsertCommand1;
			this.daCategory.SelectCommand = this.sqlSelectCommand1;
			this.daCategory.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																								 new System.Data.Common.DataTableMapping("Table", "Category", new System.Data.Common.DataColumnMapping[] {
																																																			 new System.Data.Common.DataColumnMapping("CategoryID", "CategoryID"),
																																																			 new System.Data.Common.DataColumnMapping("CategoryParentID", "CategoryParentID"),
																																																			 new System.Data.Common.DataColumnMapping("CategoryName", "CategoryName"),
																																																			 new System.Data.Common.DataColumnMapping("CategoryPath", "CategoryPath"),
																																																			 new System.Data.Common.DataColumnMapping("CategoryDescription", "CategoryDescription"),
																																																			 new System.Data.Common.DataColumnMapping("CategoryDate", "CategoryDate"),
																																																			 new System.Data.Common.DataColumnMapping("Publish", "Publish")})});
			this.daCategory.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = @"DELETE FROM Category WHERE (CategoryID = @Original_CategoryID) AND (CategoryDate = @Original_CategoryDate OR @Original_CategoryDate IS NULL AND CategoryDate IS NULL) AND (CategoryDescription = @Original_CategoryDescription OR @Original_CategoryDescription IS NULL AND CategoryDescription IS NULL) AND (CategoryName = @Original_CategoryName) AND (CategoryParentID = @Original_CategoryParentID) AND (CategoryPath = @Original_CategoryPath OR @Original_CategoryPath IS NULL AND CategoryPath IS NULL) AND (Publish = @Original_Publish)";
			this.sqlDeleteCommand1.Connection = this.cn;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryDate", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryDescription", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryDescription", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryPath", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=picdbserver;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = @"INSERT INTO Category(CategoryParentID, CategoryName, CategoryPath, CategoryDescription, CategoryDate, Publish) VALUES (@CategoryParentID, @CategoryName, @CategoryPath, @CategoryDescription, @CategoryDate, @Publish); SELECT CategoryID, CategoryParentID, CategoryName, CategoryPath, CategoryDescription, CategoryDate, Publish FROM Category WHERE (CategoryID = @@IDENTITY)";
			this.sqlInsertCommand1.Connection = this.cn;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryParentID", System.Data.SqlDbType.Int, 4, "CategoryParentID"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.NVarChar, 500, "CategoryName"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryPath", System.Data.SqlDbType.NVarChar, 2500, "CategoryPath"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryDescription", System.Data.SqlDbType.NVarChar, 2500, "CategoryDescription"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryDate", System.Data.SqlDbType.DateTime, 4, "CategoryDate"));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, "Publish"));
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT CategoryID, CategoryParentID, CategoryName, CategoryPath, CategoryDescript" +
				"ion, CategoryDate, Publish FROM Category";
			this.sqlSelectCommand1.Connection = this.cn;
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = @"UPDATE Category SET CategoryParentID = @CategoryParentID, CategoryName = @CategoryName, CategoryPath = @CategoryPath, CategoryDescription = @CategoryDescription, CategoryDate = @CategoryDate, Publish = @Publish WHERE (CategoryID = @Original_CategoryID) AND (CategoryDate = @Original_CategoryDate OR @Original_CategoryDate IS NULL AND CategoryDate IS NULL) AND (CategoryDescription = @Original_CategoryDescription OR @Original_CategoryDescription IS NULL AND CategoryDescription IS NULL) AND (CategoryName = @Original_CategoryName) AND (CategoryParentID = @Original_CategoryParentID) AND (CategoryPath = @Original_CategoryPath OR @Original_CategoryPath IS NULL AND CategoryPath IS NULL) AND (Publish = @Original_Publish); SELECT CategoryID, CategoryParentID, CategoryName, CategoryPath, CategoryDescription, CategoryDate, Publish FROM Category WHERE (CategoryID = @CategoryID)";
			this.sqlUpdateCommand1.Connection = this.cn;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryParentID", System.Data.SqlDbType.Int, 4, "CategoryParentID"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.NVarChar, 500, "CategoryName"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryPath", System.Data.SqlDbType.NVarChar, 2500, "CategoryPath"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryDescription", System.Data.SqlDbType.NVarChar, 2500, "CategoryDescription"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryDate", System.Data.SqlDbType.DateTime, 4, "CategoryDate"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, "Publish"));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryDate", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryDescription", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryDescription", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryPath", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"));
			// 
			// tvCategory
			// 
			this.tvCategory.ContextMenu = this.contextMenu1;
			this.tvCategory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvCategory.HideSelection = false;
			this.tvCategory.ImageIndex = -1;
			this.tvCategory.Location = new System.Drawing.Point(0, 0);
			this.tvCategory.Name = "tvCategory";
			this.tvCategory.SelectedImageIndex = -1;
			this.tvCategory.Size = new System.Drawing.Size(120, 112);
			this.tvCategory.TabIndex = 0;
			this.tvCategory.DoubleClick += new System.EventHandler(this.tvCategory_DoubleClick);
			this.tvCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCategory_AfterSelect);
			this.tvCategory.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvCategory_AfterLabelEdit);
			this.tvCategory.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvCategory_BeforeExpand);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuAddChild,
																						 this.menuEdit,
																						 this.menuDelete,
																						 this.menuItem1,
																						 this.menuRefresh,
																						 this.menuSaveSlideshow});
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
			// menuDeleteCat
			// 
			this.menuDeleteCat.Index = -1;
			this.menuDeleteCat.Text = "";
			this.menuDeleteCat.Click += new System.EventHandler(this.menuDeleteCat_Click);
			// 
			// dvCategory
			// 
			this.dvCategory.Table = this.dsCategory.Category;
			// 
			// dsCategory
			// 
			this.dsCategory.DataSetName = "DataSetCategory";
			this.dsCategory.Locale = new System.Globalization.CultureInfo("en-US");
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
			// CategoryTree
			// 
			this.Controls.Add(this.tvCategory);
			this.Name = "CategoryTree";
			this.Size = new System.Drawing.Size(120, 112);
			((System.ComponentModel.ISupportInitialize)(this.dvCategory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dsCategory)).EndInit();
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
            
			ec.NewCategory(n.Category.CategoryId);

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
            ec.CategoryID = n.Category.CategoryId;
            ec.ShowDialog();

			if (!ec.Cancel) 
			{
                n.Update(ec.SelectedCategory);
			}

		}

		private void menuDeleteCat_Click(object sender, System.EventArgs e)
		{
			CategoryTreeNode n = this.SelectedNode;

			if (n.Nodes.Count > 0) 
			{
				MessageBox.Show("You cannot currently delete a category that contains categories.  Please delete the categories below first.", "Delete Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			if (n == null) 
			{
				MessageBox.Show("You must select a category to delete it.", "Delete Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			// make sure we want to delete
			if (MessageBox.Show("Would you like to delete category '" + n.Text + "'?  All categories below it will also be deleted.  No pictures will be deleted, but picture-category associations may be.", 
				"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
			{
				DataSetCategory.CategoryRow cr = dsCategory.Category.FindByCategoryID( 
                    n.Category.CategoryId);
				cr.Delete();
				tvCategory.Nodes.Remove(n);
			}
		}

		private void tvCategory_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
            CategoryTreeNode node = e.Node as CategoryTreeNode;

            throw new NotImplementedException("Category update is not implemetned.");
		}

		private void tvCategory_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "<to load>")
                FillChildren(e.Node as CategoryTreeNode , 2);
		}

		private void FillChildren(CategoryTreeNode n, int intLevelsToGo) 
		{
			// clear out all child nodes
			n.Nodes.Clear();
            
			// load child nodes from dvCategory

            Collection<Category> categories = PicContext.Current.CategoryManager.GetCategories(
                n.Category.CategoryId);

			foreach (Category category in categories) 
			{
                CategoryTreeNode childNode = new CategoryTreeNode(category);
                n.Nodes.Add(childNode);

                if (intLevelsToGo > 0)
					this.FillChildren(childNode, intLevelsToGo-1);
                else
                    // add a fake node to be filled in
                    childNode.Nodes.Add("<to load>");
            }

		}

        public void SetSelectedCategory(Category category)
        {
            string path = category.Path;
            CategoryTreeNode current = tvCategory.Nodes[0] as CategoryTreeNode;
            this.SetSelectedCategory(current, category);
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
    }
}
