using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PicAdminCS
{
	// events
	public delegate void ClickCategoryEventHandler(object sender, CategoryTreeEventArgs e);
	public delegate void DoubleClickCategoryEventHandler(object sender, CategoryTreeEventArgs e);

	// class for passing events up
	public class CategoryTreeEventArgs: EventArgs 
	{
		public DataSetCategory.CategoryRow categoryRow;
	}


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
		private System.Data.SqlClient.SqlConnection cn;
		private PicAdminCS.DataSetCategory dsCategory;
		private System.Data.DataView dvCategory;
		private System.Data.SqlClient.SqlDataAdapter daCategory;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public CategoryTree()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			daCategory.Fill(dsCategory,"Category");

			// load first node
			dvCategory.RowFilter = "CategoryID = 1";
			TreeNode nRoot = new TreeNode("Categories");
			nRoot.Tag = (DataSetCategory.CategoryRow) dvCategory[0].Row;
			tvCategory.Nodes.Add(nRoot);
			nRoot.Expand();	

            // load first level
			FillChildren(nRoot, 2);

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
			this.menuAddChildCat = new System.Windows.Forms.MenuItem();
			this.menuEditCatName = new System.Windows.Forms.MenuItem();
			this.menuDeleteCat = new System.Windows.Forms.MenuItem();
			this.dvCategory = new System.Data.DataView();
			this.dsCategory = new PicAdminCS.DataSetCategory();
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
																																																			 new System.Data.Common.DataColumnMapping("CategoryPath", "CategoryPath")})});
			this.daCategory.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = "DELETE FROM Category WHERE (CategoryID = @CategoryID) AND (CategoryName = @Catego" +
				"ryName) AND (CategoryParentID = @CategoryParentID) AND (CategoryPath = @Category" +
				"Path OR @CategoryPath1 IS NULL AND CategoryPath IS NULL)";
			this.sqlDeleteCommand1.Connection = this.cn;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryPath", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryPath1", System.Data.SqlDbType.NVarChar, 1000, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Original, null));
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = "INSERT INTO Category (CategoryParentID, CategoryName, CategoryPath) VALUES (@Cate" +
				"goryParentID, @CategoryName, \'\'); SELECT CategoryID, CategoryParentID, CategoryN" +
				"ame, CategoryPath FROM Category WHERE (CategoryID = @@IDENTITY)";
			this.sqlInsertCommand1.Connection = this.cn;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryName", System.Data.DataRowVersion.Current, null));
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT CategoryID, CategoryParentID, CategoryName, CategoryPath FROM Category";
			this.sqlSelectCommand1.Connection = this.cn;
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = @"UPDATE Category SET CategoryParentID = @CategoryParentID, CategoryName = @CategoryName, CategoryPath = CategoryPath WHERE (CategoryID = @Original_CategoryID) AND (CategoryName = @Original_CategoryName) AND (CategoryParentID = @Original_CategoryParentID) AND (CategoryPath = @Original_CategoryPath OR @Original_CategoryPath1 IS NULL AND CategoryPath IS NULL); SELECT CategoryID, CategoryParentID, CategoryName, CategoryPath FROM Category WHERE (CategoryID = @Select_CategoryID)";
			this.sqlUpdateCommand1.Connection = this.cn;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryParentID", System.Data.SqlDbType.Variant, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryName", System.Data.SqlDbType.Variant, 500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryName", System.Data.SqlDbType.NVarChar, 500, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryParentID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryParentID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryPath", System.Data.SqlDbType.NVarChar, 2500, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryPath", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryPath1", System.Data.SqlDbType.NChar, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Current, null));
			// 
			// tvCategory
			// 
			this.tvCategory.ContextMenu = this.contextMenu1;
			this.tvCategory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvCategory.FullRowSelect = true;
			this.tvCategory.HideSelection = false;
			this.tvCategory.ImageIndex = -1;
			this.tvCategory.Name = "tvCategory";
			this.tvCategory.SelectedImageIndex = -1;
			this.tvCategory.Size = new System.Drawing.Size(150, 110);
			this.tvCategory.TabIndex = 0;
			this.tvCategory.DoubleClick += new System.EventHandler(this.tvCategory_DoubleClick);
			this.tvCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCategory_AfterSelect);
			this.tvCategory.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvCategory_AfterLabelEdit);
			this.tvCategory.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvCategory_BeforeExpand);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuAddChildCat,
																						 this.menuEditCatName,
																						 this.menuDeleteCat});
			// 
			// menuAddChildCat
			// 
			this.menuAddChildCat.Index = 0;
			this.menuAddChildCat.Text = "&Add Child Category";
			this.menuAddChildCat.Click += new System.EventHandler(this.menuAddChildCat_Click);
			// 
			// menuEditCatName
			// 
			this.menuEditCatName.Enabled = false;
			this.menuEditCatName.Index = 1;
			this.menuEditCatName.Text = "&Edit Category Name";
			this.menuEditCatName.Click += new System.EventHandler(this.menuEditCatName_Click);
			// 
			// menuDeleteCat
			// 
			this.menuDeleteCat.Enabled = false;
			this.menuDeleteCat.Index = 2;
			this.menuDeleteCat.Text = "&Delete Category";
			this.menuDeleteCat.Click += new System.EventHandler(this.menuDeleteCat_Click);
			// 
			// dvCategory
			// 
			this.dvCategory.Table = this.dsCategory.Category;
			// 
			// dsCategory
			// 
			this.dsCategory.DataSetName = "dsCategory";
			this.dsCategory.Locale = new System.Globalization.CultureInfo("en-US");
			this.dsCategory.Namespace = "http://www.tempuri.org/DataSetCategory.xsd";
			// 
			// CategoryTree
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tvCategory});
			this.Name = "CategoryTree";
			this.Size = new System.Drawing.Size(150, 110);
			((System.ComponentModel.ISupportInitialize)(this.dvCategory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dsCategory)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void menuAddChildCat_Click(object sender, System.EventArgs e)
		{

			TreeNode n;
			n = tvCategory.SelectedNode;
			if (n == null) 
			{
				MessageBox.Show("You must select a category to add a sub category.", "Add Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			fPromptText p = new fPromptText();
			p.FormCaption = "New Category";
			p.Message = "Enter the new category name:";
			p.ShowDialog();

			if (!p.Cancel) 
			{
				// add new tree node
                TreeNode nChild = new TreeNode(p.Value);
                n.Nodes.Add(nChild);

				// add database rec
				DataSetCategory.CategoryRow cr = dsCategory.Category.AddCategoryRow(
						((DataSetCategory.CategoryRow)nChild.Parent.Tag).CategoryID, 
						p.Value.ToString(), "");
                daCategory.Update(dsCategory, "Category");
				dsCategory.AcceptChanges();

				// get key and update node tag
                nChild.Tag = cr;
	
				// expand parent node and select new node
				n.Expand();
				tvCategory.SelectedNode = nChild;
			}

		}

		private void menuEditCatName_Click(object sender, System.EventArgs e)
		{
			TreeNode n;
			n = tvCategory.SelectedNode;
			if (n == null) 
			{
				MessageBox.Show("You must select a category to edit it.", "Edit Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			} 
			else if (n == tvCategory.Nodes[0]) 
			{
				return;
			}
			n.BeginEdit();
		}

		private void menuDeleteCat_Click(object sender, System.EventArgs e)
		{
			TreeNode n;
			n = tvCategory.SelectedNode;
			if (n == null) 
			{
				MessageBox.Show("You must select a category to delete it.", "Delete Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			// make sure we want to delete
			if (MessageBox.Show("Would you like to delete category '" + n.Text + "'?  All categories below it will also be deleted.", 
				"Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
			{
				DataSetCategory.CategoryRow cr = dsCategory.Category.FindByCategoryID( ((DataSetCategory.CategoryRow)n.Tag).CategoryID );
				cr.Delete();
				daCategory.Update(dsCategory, "Category");
                tvCategory.Nodes.Remove(n);
			}
		}

		private void tvCategory_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
		{
			// see if a new node
			if (e.Node.Tag.ToString() == "") 
			{
				MessageBox.Show( "Invalid node...");
			}
			// otherwise an existing node
			else 
			{
                DataSetCategory.CategoryRow cr = dsCategory.Category.FindByCategoryID(
					((DataSetCategory.CategoryRow)e.Node.Tag).CategoryID);
				cr.CategoryName = e.Label;
				cr.EndEdit();
			}

			// either way, update the database
			daCategory.Update(dsCategory, "Category");
		}

		private void tvCategory_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "<to load>")
                FillChildren(e.Node, 2);
		}

		private void FillChildren(TreeNode n, int intLevelsToGo) 
		{
			// clear out all child nodes
			n.Nodes.Clear();
            
			// load child nodes from dvCategory
			DataSetCategory.CategoryRow crParent = (DataSetCategory.CategoryRow) n.Tag;
			dvCategory.RowFilter = "CategoryParentID = " + crParent.CategoryID.ToString() + " And CategoryID <> 1";

			TreeNode nChild;

			foreach (DataRowView dr in dvCategory) 
			{
				// add this row as a node
				DataSetCategory.CategoryRow cr = (DataSetCategory.CategoryRow) dr.Row;
				nChild = n.Nodes.Add(cr.CategoryName.ToString());
				nChild.Tag = cr;

				if (intLevelsToGo > 0)
					FillChildren(nChild, intLevelsToGo-1);
				else
					// add a fake node to be filled in
					nChild.Nodes.Add("<to load>");
			}


		}

		public DataSetCategory.CategoryRow FindCategoryInfo(int CategoryID) 
		{
            return (dsCategory.Category.FindByCategoryID(CategoryID));
		}

		public DataSetCategory.CategoryRow SelectedCategory
		{
			get 
			{
                if (tvCategory.SelectedNode == null)
					return null;

				return (DataSetCategory.CategoryRow) tvCategory.SelectedNode.Tag;
			}

		}

		// events
		public event ClickCategoryEventHandler ClickCategory;
		public event DoubleClickCategoryEventHandler DoubleClickCategory;

		private void tvCategory_DoubleClick(object sender, System.EventArgs e)
		{
			TreeNode n = tvCategory.SelectedNode;
			if (n == null)
				return;

			// Fire event for other controls to catch if they want
			CategoryTreeEventArgs ex = new CategoryTreeEventArgs();
			ex.categoryRow = (DataSetCategory.CategoryRow) n.Tag;
			
			if (DoubleClickCategory != null)
				DoubleClickCategory(this, ex);

		}

		private void tvCategory_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{

			TreeNode n = tvCategory.SelectedNode;
			if (n == null)
				return;

			// Fire event for other controls to catch if they want
			CategoryTreeEventArgs ex = new CategoryTreeEventArgs();
			ex.categoryRow = (DataSetCategory.CategoryRow) n.Tag;
			
			if (ClickCategory != null)
				ClickCategory(this, ex);

		}
	}
}
