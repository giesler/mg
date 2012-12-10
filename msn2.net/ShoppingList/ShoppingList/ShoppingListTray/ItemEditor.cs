using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using msn2.net.ShoppingList.sls;

namespace ShoppingListTray
{
    public partial class ItemEditor : Form
    {
        public event EventHandler UpdateItem;
        private ShoppingListItem listItem;

        public ItemEditor(List<string> stores)
        {
            InitializeComponent();

            foreach (string store in stores)
            {
                this.store.Items.Add(store);
            }

            this.store.SelectedIndex = 0;
        }

        public string Store
        {
            set
            {
                for (int i = 0; i < this.store.Items.Count; i++)
                {
                    if (this.store.Items[i].ToString() == value)
                    {
                        this.store.SelectedIndex = i;
                        break;
                    }
                }
            }
            get
            {
                string store = null;

                if (this.store.SelectedIndex >= 0)
                {
                    store = this.store.SelectedItem.ToString();
                }

                return store;
            }
        }

        public void LoadItem(ShoppingListItem item)
        {
            this.listItem = item;

            this.item.Text = item.ListItem;
            this.Store = item.Store;

            this.save.Text = "&Update";
            this.saveNew.Text = "Update && &Add";
        }

        public ShoppingListItem ListItem
        {
            get
            {
                return this.listItem;
            }
            set
            {
                this.listItem = value;
            }
        }

        public string ItemText
        {
            get
            {
                return this.item.Text.Trim();
            }
            set
            {
                this.item.Text = value;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (this.UpdateItem != null)
            {
                this.UpdateItem(this, EventArgs.Empty);
            }

            this.Close();
        }

        private void saveNew_Click(object sender, EventArgs e)
        {
            if (this.UpdateItem != null)
            {
                this.UpdateItem(this, EventArgs.Empty);
            }

            this.save.Text = "&Add";
            this.saveNew.Text = "Add && &New";

            this.listItem = null;
            this.item.Text = string.Empty;
            this.item.Focus();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void item_TextChanged(object sender, EventArgs e)
        {
            this.save.Enabled = this.item.Text.Trim().Length > 0;
            this.saveNew.Enabled = this.save.Enabled;
        }
    }
}
