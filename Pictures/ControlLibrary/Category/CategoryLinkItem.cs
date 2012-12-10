#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using msn2.net.Pictures;

#endregion

namespace msn2.net.Pictures.Controls
{
    public class CategoryLinkItem : BaseLinkItem
    {
        private CategoryInfo category;

        public CategoryLinkItem(CategoryInfo category)
        {
            this.category = category;
            base.Text = category.Name;
        }

        public CategoryInfo Category
        {
            get
            {
                return this.category;
            }
        }
    }
}
