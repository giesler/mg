using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace PicAdminCS
{
	// A delegate type for hooking up change notifications.
	public delegate void AddedCategoryEventHandler(object sender, CategoryPickerEventArgs e);
	public delegate void RemovedCategoryEventHandler(object sender, CategoryPickerEventArgs e);

	// class for passing events up
	public class CategoryPickerEventArgs: EventArgs 
	{
        public int CategoryID;
	}


	/// <summary>
	/// Summary description for CategoryPicker.
	/// </summary>
	public class CategoryPicker : System.Windows.Forms.UserControl
	{
		private PicAdminCS.CategoryTree categoryTree1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnRemoveCategory;
		private System.Windows.Forms.Button btnAddCategory;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ListView lvCategories;
		private System.Windows.Forms.ColumnHeader columnHeader1;

		private ArrayList arCategory = new ArrayList();
		private PicAdminCS.DataSetCategory dsCategory = new PicAdminCS.DataSetCategory();

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CategoryPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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
			this.dsCategory = new PicAdminCS.DataSetCategory();
			this.lvCategories = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.btnAddCategory = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnRemoveCategory = new System.Windows.Forms.Button();
			this.categoryTree1 = new PicAdminCS.CategoryTree();
			((System.ComponentModel.ISupportInitialize)(this.dsCategory)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// dsCategory
			// 
			this.dsCategory.DataSetName = "DataSetCategory";
			this.dsCategory.Locale = new System.Globalization.CultureInfo("en-US");
			this.dsCategory.Namespace = "http://www.tempuri.org/DataSetCategory.xsd";
			// 
			// lvCategories
			// 
			this.lvCategories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.columnHeader1});
			this.lvCategories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvCategories.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvCategories.HideSelection = false;
			this.lvCategories.Name = "lvCategories";
			this.lvCategories.Size = new System.Drawing.Size(264, 176);
			this.lvCategories.TabIndex = 1;
			this.lvCategories.View = System.Windows.Forms.View.Details;
			this.lvCategories.Resize += new System.EventHandler(this.lvCategories_Resize);
			this.lvCategories.DoubleClick += new System.EventHandler(this.lvCategories_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Category";
			this.columnHeader1.Width = 500;
			// 
			// btnAddCategory
			// 
			this.btnAddCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnAddCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAddCategory.Location = new System.Drawing.Point(8, 56);
			this.btnAddCategory.Name = "btnAddCategory";
			this.btnAddCategory.Size = new System.Drawing.Size(24, 23);
			this.btnAddCategory.TabIndex = 2;
			this.btnAddCategory.Text = ">";
			this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(152, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 176);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.panel2,
																				 this.btnRemoveCategory,
																				 this.btnAddCategory});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(155, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(301, 176);
			this.panel1.TabIndex = 3;
			// 
			// panel2
			// 
			this.panel2.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.panel2.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.lvCategories});
			this.panel2.Location = new System.Drawing.Point(40, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(264, 176);
			this.panel2.TabIndex = 3;
			// 
			// btnRemoveCategory
			// 
			this.btnRemoveCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnRemoveCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRemoveCategory.Location = new System.Drawing.Point(8, 96);
			this.btnRemoveCategory.Name = "btnRemoveCategory";
			this.btnRemoveCategory.Size = new System.Drawing.Size(24, 23);
			this.btnRemoveCategory.TabIndex = 2;
			this.btnRemoveCategory.Text = "<";
			this.btnRemoveCategory.Click += new System.EventHandler(this.btnRemoveCategory_Click);
			// 
			// categoryTree1
			// 
			this.categoryTree1.Dock = System.Windows.Forms.DockStyle.Left;
			this.categoryTree1.Name = "categoryTree1";
			this.categoryTree1.Size = new System.Drawing.Size(152, 176);
			this.categoryTree1.TabIndex = 0;
			this.categoryTree1.DoubleClickCategory += new PicAdminCS.DoubleClickCategoryEventHandler(this.categoryTree1_DoubleClickCategory);
			// 
			// CategoryPicker
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1,
																		  this.splitter1,
																		  this.categoryTree1});
			this.Name = "CategoryPicker";
			this.Size = new System.Drawing.Size(456, 176);
			((System.ComponentModel.ISupportInitialize)(this.dsCategory)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		
		// An event that clients can use to be notified whenever the
		// elements of the list change.
		public event AddedCategoryEventHandler AddedCategory;
        public event RemovedCategoryEventHandler RemovedCategory;

		private void btnAddCategory_Click(object sender, System.EventArgs e)
		{
			DataSetCategory.CategoryRow cr = categoryTree1.SelectedCategory;

			if (cr == null) 
			{
				MessageBox.Show("You must select a category.");
				return;
			}

			// make sure row isn't already added
			foreach (ListViewItem liTemp in lvCategories.Items) 
			{
				if (((DataSetCategory.CategoryRow) liTemp.Tag).CategoryID == cr.CategoryID) 
				{
					MessageBox.Show("The category '" + cr.CategoryName + "' has already been added.","Add Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}
			}

			ListViewItem li = lvCategories.Items.Add(cr.CategoryPath.ToString());
			li.Tag = cr;

			CategoryPickerEventArgs ex = new CategoryPickerEventArgs();
			ex.CategoryID = cr.CategoryID;

			if (AddedCategory != null)
				AddedCategory(this, ex);

			// add to dsCategory
//			DataSetCategory.CategoryRow cRow = dsCategory.Category.AddCategoryRow(
//				cr.CategoryParentID, cr.CategoryName, cr.CategoryPath);
//			cRow.CategoryID = ex.CategoryID;

			// add to arCategory if not there
			if (!arCategory.Contains(cr.CategoryID))
				arCategory.Add(cr.CategoryID);

		}

		private void btnRemoveCategory_Click(object sender, System.EventArgs e)
		{
			if (lvCategories.SelectedItems.Count == 0) 
			{
				MessageBox.Show("You must select one or more categories to remove!","Remove Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			foreach (ListViewItem li in lvCategories.SelectedItems) 
			{
				CategoryPickerEventArgs ex = new CategoryPickerEventArgs();
				DataSetCategory.CategoryRow cr = (DataSetCategory.CategoryRow) li.Tag;
				ex.CategoryID = cr.CategoryID;
				lvCategories.Items.Remove(li);

				// remove from dsCategory
				DataSetCategory.CategoryRow crLocal = dsCategory.Category.FindByCategoryID(cr.CategoryID);
				if (crLocal != null)
                    dsCategory.Category.RemoveCategoryRow(crLocal);

				// remove from arCategory if there
				if (arCategory.Contains(cr.CategoryID))
					arCategory.Remove(cr.CategoryID);

				// fire event
				if (RemovedCategory != null)
					RemovedCategory (this, ex);
			}

		}

		public void AddSelectedCategory(int CategoryID) 
		{
            DataSetCategory.CategoryRow cr = categoryTree1.FindCategoryInfo(CategoryID);

			if (cr == null) 
			{
				MessageBox.Show("Category information for id " + CategoryID.ToString() + " was not found.");
				return;
			}

			ListViewItem li = lvCategories.Items.Add(cr.CategoryPath.ToString());
			li.Tag = cr;

			// add to dsCategory
			DataSetCategory.CategoryRow newcr = dsCategory.Category.NewCategoryRow();
			newcr.CategoryParentID  = cr.CategoryParentID;
			newcr.CategoryName		= cr.CategoryName;
			newcr.CategoryPath      = cr.CategoryPath;
			dsCategory.Category.AddCategoryRow(newcr);

			// add to arCategory if not there
			if (!arCategory.Contains(cr.CategoryID))
				arCategory.Add(cr.CategoryID);

		}

		public void ClearSelectedCategories() 
		{
            lvCategories.Items.Clear();
		}

		private void categoryTree1_DoubleClickCategory(object sender, PicAdminCS.CategoryTreeEventArgs e)
		{
            btnAddCategory_Click(sender, e);
		}

		private void lvCategories_DoubleClick(object sender, System.EventArgs e)
		{
			btnRemoveCategory_Click(sender, e);
		}

		private void lvCategories_Resize(object sender, System.EventArgs e)
		{
			if (lvCategories.Width > 100)
				lvCategories.Columns[0].Width = lvCategories.Width-20;
		}

		public DataSetCategory datasetCategory 
		{
			get 
			{
				return dsCategory;
			}
		}

		public ArrayList selectedCategories
		{
			get 
			{
				return arCategory;
			}
		}
	}
}
