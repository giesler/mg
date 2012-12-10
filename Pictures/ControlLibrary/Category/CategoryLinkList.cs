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
    public class CategoryLinkList : BaseLinkList
    {
        public CategoryLinkList()
        {
        }

        public CategoryLinkItem Add(CategoryInfo category)
        {
            CategoryLinkItem item = new CategoryLinkItem(category);
            this.Controls.Add(item);
            return item;
        }

        public bool Contains(CategoryInfo category)
        {
            CategoryLinkItem item = this.Find(category);
            return (item != null);
        }

        public CategoryLinkItem Find(CategoryInfo category)
        {
            foreach (CategoryLinkItem item in this.Controls)
            {
                if (item.Category.CategoryId == category.CategoryId)
                {
                    return item;
                }
            }

            return null;
        }
        
        public void Remove(CategoryInfo category)
        {
            foreach (CategoryLinkItem item in this.Controls)
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
