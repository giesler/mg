using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using msn2.net.ShoppingList;

namespace mn2.net.ShoppingList
{
    public partial class SelectStore : Form
    {
        private StoreItem selectedStore = null;

        public SelectStore()
        {
            InitializeComponent();
        }

        public void ShowStores(Dictionary<string, StoreItem> items)
        {
            foreach (StoreItem store in items.Values)
            {
                ListViewItem item = new ListViewItem(store.ToString());
                item.Tag = store;
                item.ForeColor = store.Items.Count > 0 ? Color.Black : Color.Gray;
                this.list.Items.Add(item);
            }
        }

        public StoreItem SelectedStore
        {
            get
            {
                StoreItem store = null;
                if (this.list.SelectedIndices.Count > 0)
                {
                    ListViewItem item = this.list.Items[this.list.SelectedIndices[0]];
                    store = (StoreItem)item.Tag;
                }

                return store;                
            }
            set
            {
                this.selectedStore = value;
            }
        }

        private void SelectStore_Resize(object sender, EventArgs e)
        {
            this.columnHeader1.Width = this.list.ClientSize.Width - 20;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SelectStore_Load(object sender, EventArgs e)
        {

        }

        private void SelectStore_Activated(object sender, EventArgs e)
        {

            //if (this.selectedStore != null)
            //{
            //    foreach (ListViewItem item in this.list.Items)
            //    {
            //        if (item.Tag == this.selectedStore)
            //        {
            //            item.Selected = true;
            //            this.selectedStore = null;
            //            break;
            //        }
            //    }
            //}
        }
    }
}