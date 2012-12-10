using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using mn2.net.ShoppingList.sls;

namespace msn2.net.ShoppingList
{
    class ShoppingListViewItem: ListViewItem
    {
        private ShoppingListItem item = null;

        public ShoppingListViewItem(ShoppingListItem item)
        {
            this.UpdateItem(item);
        }

        public ShoppingListItem Item
        {
            get
            {
                return this.item;
            }
        }

        public void UpdateItem(ShoppingListItem item)
        {
            this.item = item;
            this.Text = item.ListItem;
        }
    }
}
