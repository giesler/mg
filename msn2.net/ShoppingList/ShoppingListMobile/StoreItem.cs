using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace msn2.net.ShoppingList
{
    public class StoreItem
    {
        public StoreItem(string name)
        {
            this.name = name;
            this.items = new List<mn2.net.ShoppingList.sls.ShoppingListItem>();
        }

        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        private List<mn2.net.ShoppingList.sls.ShoppingListItem> items;
        public List<mn2.net.ShoppingList.sls.ShoppingListItem> Items
        {
            get
            {
                return this.items;
            }
        }

        public override string ToString()
        {
            string suffix = string.Empty;
            if (this.items != null)
            {
                if (this.items.Count > 0)
                {
                    suffix = " (" + this.items.Count.ToString() + ")";
                }
            }

            return this.name + suffix;
        }
    }
}
