using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace msn2.net.Pictures.Controls
{

	/// <summary>
	/// Summary description for CategoryPicker.
	/// </summary>
	public class CategoryPicker : System.Windows.Forms.UserControl
	{
		private msn2.net.Pictures.Controls.CategoryTree categoryTree1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnRemoveCategory;
		private System.Windows.Forms.Button btnAddCategory;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ListView lvCategories;
		private System.Windows.Forms.ColumnHeader columnHeader1;

		private System.Collections.ArrayList arCategory = new System.Collections.ArrayList();
		
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
			this.lvCategories = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.btnAddCategory = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnRemoveCategory = new System.Windows.Forms.Button();
			this.categoryTree1 = new msn2.net.Pictures.Controls.CategoryTree();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// lvCategories
			// 
			this.lvCategories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.columnHeader1});
			this.lvCategories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvCategories.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvCategories.HideSelection = false;
			this.lvCategories.Location = new System.Drawing.Point(0, 0);
			this.lvCategories.Name = "lvCategories";
			this.lvCategories.Size = new System.Drawing.Size(112, 176);
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
			this.btnAddCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAddCategory.Location = new System.Drawing.Point(8, 56);
			this.btnAddCategory.Name = "btnAddCategory";
			this.btnAddCategory.Size = new System.Drawing.Size(24, 23);
			this.btnAddCategory.TabIndex = 2;
			this.btnAddCategory.Text = ">";
			this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(304, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 176);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.btnRemoveCategory);
			this.panel1.Controls.Add(this.btnAddCategory);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(307, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(149, 176);
			this.panel1.TabIndex = 3;
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.lvCategories);
			this.panel2.Location = new System.Drawing.Point(40, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(112, 176);
			this.panel2.TabIndex = 3;
			// 
			// btnRemoveCategory
			// 
			this.btnRemoveCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnRemoveCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
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
			this.categoryTree1.Location = new System.Drawing.Point(0, 0);
			this.categoryTree1.Name = "categoryTree1";
			this.categoryTree1.Size = new System.Drawing.Size(304, 176);
			this.categoryTree1.TabIndex = 0;
			this.categoryTree1.DoubleClickCategory += new msn2.net.Pictures.Controls.DoubleClickCategoryEventHandler(this.categoryTree1_DoubleClickCategory);
			// 
			// CategoryPicker
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.categoryTree1);
			this.Name = "CategoryPicker";
			this.Size = new System.Drawing.Size(456, 176);
			this.Load += new System.EventHandler(this.CategoryPicker_Load);
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
			Category category = categoryTree1.SelectedCategory;

            if (category == null)
            {
				MessageBox.Show("You must select a category.");
				return;
			}

			// make sure row isn't already added
			foreach (CategoryListViewItem liTemp in lvCategories.Items) 
			{
				if (liTemp.Category.CategoryId == category.CategoryId) 
				{
                    return;
				}
			}

            CategoryListViewItem li = new CategoryListViewItem(category);
            lvCategories.Items.Add(li);

            if (AddedCategory != null)
            {
                CategoryPickerEventArgs ex = new CategoryPickerEventArgs(category);
                AddedCategory(this, ex);
            }

			// add to arCategory if not there
			if (!arCategory.Contains(category.CategoryId))
				arCategory.Add(category.CategoryId);

		}

		private void btnRemoveCategory_Click(object sender, System.EventArgs e)
		{
			if (lvCategories.SelectedItems.Count == 0) 
			{
				MessageBox.Show("You must select one or more categories to remove!","Remove Category", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			foreach (CategoryListViewItem li in lvCategories.SelectedItems) 
			{
				CategoryPickerEventArgs ex = new CategoryPickerEventArgs(li.Category);
				lvCategories.Items.Remove(li);

				// remove from arCategory if there
				if (arCategory.Contains(li.Category.CategoryId))
                    arCategory.Remove(li.Category.CategoryId);

                // fire event
				if (RemovedCategory != null)
					RemovedCategory (this, ex);
			}

		}

		public void AddSelectedCategory(int categoryId) 
		{

            Category category = PicContext.Current.CategoryManager.GetCategory(categoryId);

            if (category != null)
            {
                CategoryListViewItem item = new CategoryListViewItem(category);

                // add to arCategory if not there
                if (!arCategory.Contains(category.CategoryId))
                    arCategory.Add(category.CategoryId);
            }
        }

		public void ClearSelectedCategories() 
		{
            lvCategories.Items.Clear();
		}

		private void categoryTree1_DoubleClickCategory(object sender, msn2.net.Pictures.Controls.CategoryTreeEventArgs e)
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

		private void CategoryPicker_Load(object sender, System.EventArgs e)
		{
			// Set category tree to half form width
			categoryTree1.Width = this.Width / 2 - panel1.Width / 2;
		}

		public System.Collections.ArrayList selectedCategories
		{
			get 
			{
				return arCategory;
			}
		}

		public List<int> SelectedCategoryIds
		{
			get 
			{
                return GetSelectedCategoryIds();
            }
        }

        private delegate List<int> GetSelectedCategoryIdsDelegate();

        private List<int> GetSelectedCategoryIds()
        {
            if (this.InvokeRequired)
            {
                GetSelectedCategoryIdsDelegate del = new GetSelectedCategoryIdsDelegate(GetSelectedCategoryIds);
                IAsyncResult result = this.BeginInvoke(del);
                return this.Invoke(del) as List<int>;
            }

            List<int> selected = new List<int>(lvCategories.Items.Count);

            foreach (CategoryListViewItem item in lvCategories.Items) 
			{
				selected.Add(item.Category.CategoryId);
			}

            return selected;
		}

	}

	// A delegate type for hooking up change notifications.
	public delegate void AddedCategoryEventHandler(object sender, CategoryPickerEventArgs e);
	public delegate void RemovedCategoryEventHandler(object sender, CategoryPickerEventArgs e);

	// class for passing events up
	public class CategoryPickerEventArgs: EventArgs 
	{
        private Category category;

        public CategoryPickerEventArgs(Category category)
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

    public class CategoryListViewItem : ListViewItem
    {
        private Category category;

        public CategoryListViewItem(Category category)
        {
            this.category = category;
            this.Text = category.Name;
        }

        public Category Category
        {
            get
            {
                return this.category;
            }
        }
    }

}
