#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#endregion

namespace msn2.net.Pictures.Controls
{
    partial class CategoryListDialog : Form
    {
        public CategoryListDialog(List<CategoryInfo> categories)
        {
            InitializeComponent();

            foreach (CategoryInfo category in categories)
            {
                CategoryListItem item = new CategoryListItem(category);
                this.listBox1.Items.Add(item);
            }
        }

        public CategoryInfo GetSelectedCategory()
        {
            CategoryInfo category = null;

            if (this.listBox1.SelectedItems.Count > 0)
            {
                CategoryListItem listItem = (CategoryListItem) this.listBox1.SelectedItem;
                category = listItem.Category;
            }

            return category;
        }

        public List<CategoryInfo> GetSelectedCategories()
        {
            List<CategoryInfo> categories = new List<CategoryInfo>();

            foreach (CategoryListItem item in this.listBox1.SelectedItems)
            {
                categories.Add(item.Category);
            }

            return categories;
        }

        public SelectionMode SelectionMode
        {
            get
            {
                return this.listBox1.SelectionMode;
            }
            set
            {
                this.listBox1.SelectionMode = value;
            }
        }
    }

    public class CategoryListItem
    {
        private CategoryInfo category;

        public CategoryListItem(CategoryInfo category)
        {
            this.category = category;
        }

        public CategoryInfo Category
        {
            get
            {
                return this.category;
            }
        }

        public override string ToString()
        {
            return this.category.Path;
        }

    }
}