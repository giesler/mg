#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

#endregion

namespace msn2.net.Pictures.Controls
{
    public partial class CategoryFlowLayoutPanel : FlowLayoutPanel
    {
        public CategoryFlowLayoutPanel()
        {
        }

        public CategoryItem AddCategory(Category category)
        {
            CategoryItem item = new CategoryItem(category);
            this.Controls.Add(item);
            return item;
        }

        public bool ContainsCategory(Category category)
        {
            CategoryItem item = this.FindCategory(category);
            return (item != null);
        }

        public CategoryItem FindCategory(Category category)
        {
            foreach (CategoryItem item in this.Controls)
            {
                if (item.Category.CategoryId == category.CategoryId)
                {
                    return item;
                }
            }

            return null;
        }
        
        public void Clear()
        {
            this.Controls.Clear();
        }

        public void RemoveCategory(Category category)
        {
            foreach (CategoryItem item in this.Controls)
            {
                if (item.Category.CategoryId == category.CategoryId)
                {
                    this.Controls.Remove(item);
                    break;
                }
            }
        }

    }
}
