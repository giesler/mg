using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using mn2.net.ShoppingList.sls;
using System.Drawing;

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

        public bool Deleted { get; set; }
        public bool DeleteRequested { get; set; }
        public DateTime DeleteRequestedTime { get; set; }

        public void UpdateItem(ShoppingListItem item)
        {
            this.item = item;
            this.Text = item.ListItem;

            if (this.DeleteRequested == true)
            {
                this.ForeColor = Color.Gray;

                TimeSpan elapsedTime = DateTime.Now - this.DeleteRequestedTime;
                if (elapsedTime.TotalSeconds < 60)
                {
                    string newText = null;
                    int seconds = 5 - (int)elapsedTime.TotalSeconds;
                    if (seconds > 0)
                    {
                        newText = string.Format("{0} ({1}...)", this.item.ListItem, seconds);
                    }
                    if (this.Text != newText)
                    {
                        this.Text = newText;
                    }
                }
            }
            else
            {
                this.ForeColor = SystemColors.WindowText;
            }
        }
    }
}
