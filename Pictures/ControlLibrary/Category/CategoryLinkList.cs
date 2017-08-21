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

        public CategoryLinkItem Add(Category category)
        {
            CategoryLinkItem item = new CategoryLinkItem(category);
            this.Controls.Add(item);
            return item;
        }

        public bool Contains(Category category)
        {
            CategoryLinkItem item = this.Find(category);
            return (item != null);
        }

        public CategoryLinkItem Find(Category category)
        {
            foreach (CategoryLinkItem item in this.Controls)
            {
                if (item.Category.Id == category.Id)
                {
                    return item;
                }
            }

            return null;
        }

        public void Remove(Category category)
        {
            foreach (CategoryLinkItem item in this.Controls)
            {
                if (item.Category.Id == category.Id)
                {
                    this.Controls.Remove(item);
                    break;
                }
            }
        }

    }
}
