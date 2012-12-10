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
        public CategoryListDialog(List<Category> categories)
        {
            InitializeComponent();

            foreach (Category category in categories)
            {
                CategoryListItem item = new CategoryListItem(category);
                this.listBox1.Items.Add(item);
            }
        }

        public Category GetSelectedCategory()
        {
            Category category = null;

            if (this.listBox1.SelectedItems.Count > 0)
            {
                category = (Category) this.listBox1.SelectedItem;
            }

            return category;
        }

        public List<Category> GetSelectedCategories()
        {
            List<Category> categories = new List<Category>();

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
        private Category category;

        public CategoryListItem(Category category)
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

        public override string ToString()
        {
            return this.category.Path;
        }

    }
}