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

        private System.Collections.ArrayList arCategory = new System.Collections.ArrayList();
        private SplitContainer splitContainer1;
        private CategoryTree categoryTree1;
        private Panel panel1;
        private Button btnRemoveCategory;
        private Button btnAddCategory;
        private ListView lvCategories;
        private ColumnHeader columnHeader1;
		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CategoryPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            this.categoryTree1.HideIcons();
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.categoryTree1 = new msn2.net.Pictures.Controls.CategoryTree();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRemoveCategory = new System.Windows.Forms.Button();
            this.btnAddCategory = new System.Windows.Forms.Button();
            this.lvCategories = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.categoryTree1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvCategories);
            this.splitContainer1.Size = new System.Drawing.Size(349, 189);
            this.splitContainer1.SplitterDistance = 205;
            this.splitContainer1.TabIndex = 5;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // categoryTree1
            // 
            this.categoryTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryTree1.Location = new System.Drawing.Point(0, 0);
            this.categoryTree1.Name = "categoryTree1";
            this.categoryTree1.Size = new System.Drawing.Size(169, 189);
            this.categoryTree1.TabIndex = 5;
            this.categoryTree1.ClickCategory += new msn2.net.Pictures.Controls.ClickCategoryEventHandler(this.categoryTree1_ClickCategory);
            this.categoryTree1.DoubleClickCategory += new msn2.net.Pictures.Controls.DoubleClickCategoryEventHandler(this.categoryTree1_DoubleClickCategory);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRemoveCategory);
            this.panel1.Controls.Add(this.btnAddCategory);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(169, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(36, 189);
            this.panel1.TabIndex = 4;
            // 
            // btnRemoveCategory
            // 
            this.btnRemoveCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRemoveCategory.Enabled = false;
            this.btnRemoveCategory.Location = new System.Drawing.Point(6, 90);
            this.btnRemoveCategory.Name = "btnRemoveCategory";
            this.btnRemoveCategory.Size = new System.Drawing.Size(23, 23);
            this.btnRemoveCategory.TabIndex = 2;
            this.btnRemoveCategory.Text = "<";
            this.btnRemoveCategory.Click += new System.EventHandler(this.btnRemoveCategory_Click);
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAddCategory.Enabled = false;
            this.btnAddCategory.Location = new System.Drawing.Point(6, 61);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(23, 23);
            this.btnAddCategory.TabIndex = 2;
            this.btnAddCategory.Text = ">";
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
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
            this.lvCategories.Size = new System.Drawing.Size(140, 189);
            this.lvCategories.TabIndex = 5;
            this.lvCategories.UseCompatibleStateImageBehavior = false;
            this.lvCategories.View = System.Windows.Forms.View.Details;
            this.lvCategories.Resize += new System.EventHandler(this.lvCategories_Resize);
            this.lvCategories.SelectedIndexChanged += new System.EventHandler(this.lvCategories_SelectedIndexChanged);
            this.lvCategories.DoubleClick += new System.EventHandler(this.lvCategories_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Selected Categories";
            this.columnHeader1.Width = 500;
            // 
            // CategoryPicker
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "CategoryPicker";
            this.Size = new System.Drawing.Size(349, 189);
            this.Load += new System.EventHandler(this.CategoryPicker_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		
		// An event that clients can use to be notified whenever the
		// elements of the list change.
		public event AddedCategoryEventHandler AddedCategory;
        public event RemovedCategoryEventHandler RemovedCategory;

		private void btnAddCategory_Click(object sender, System.EventArgs e)
		{
            if (btnAddCategory.Enabled == true)
            {
                Category category = categoryTree1.SelectedCategory;

                // make sure row isn't already added
                foreach (CategoryListViewItem liTemp in lvCategories.Items)
                {
                    if (liTemp.Category.Id == category.Id)
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
                if (!arCategory.Contains(category.Id))
                    arCategory.Add(category.Id);
            }
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
				if (arCategory.Contains(li.Category.Id))
                    arCategory.Remove(li.Category.Id);

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
                if (!arCategory.Contains(category.Id))
                {
                    arCategory.Add(category.Id);
                    lvCategories.Items.Add(item);
                }
            }
        }

		public void ClearSelectedCategories() 
		{
            arCategory.Clear();
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
                lvCategories.Columns[0].Width = lvCategories.Width - 20;
            else
            {
                lvCategories.Columns[0].Width = 95;
            }
		}

		private void CategoryPicker_Load(object sender, System.EventArgs e)
		{
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
				selected.Add(item.Category.Id);
			}

            return selected;
		}

        private void lvCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            bool leftItemSelected = (categoryTree1.SelectedCategory != null
                && categoryTree1.SelectedNode.Nodes.Count == 0);
            bool rightItemSelected = (lvCategories.SelectedItems.Count > 0);

            btnAddCategory.Enabled = leftItemSelected;
            btnRemoveCategory.Enabled = rightItemSelected;
        }

        private void categoryTree1_ClickCategory(object sender, CategoryTreeEventArgs e)
        {
            UpdateControls();
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
