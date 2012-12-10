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
        private Timer timerResizeCheck = null;

        public SelectStore()
        {
            InitializeComponent();
        }

        public void ShowStores(Dictionary<string, StoreItem> items)
        {
            int index = 0;

            foreach (StoreItem store in items.Values)
            {
                ListViewItem item = new ListViewItem(store.Name);
                item.Tag = store;
                if (store.Items.Count > 0)
                {
                    item.SubItems.Add(store.Items.Count.ToString());
                }
                item.BackColor = MainForm.ListColors[index];
                index++;
                if (index >= MainForm.ListColors.Length)
                {
                    index = 0;
                }
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
            this.SetColumnWidth();
            
            if (this.timerResizeCheck == null)
            {
                this.timerResizeCheck = new Timer();
                this.timerResizeCheck.Tick += new EventHandler(timerResizeCheck_Tick);
                this.timerResizeCheck.Interval = 500;
            }

            if (this.timerResizeCheck.Enabled == false)
            {
                this.timerResizeCheck.Enabled = true;
            }
        }

        void timerResizeCheck_Tick(object sender, EventArgs e)
        {
            this.timerResizeCheck.Enabled = false;
            this.SetColumnWidth();
        }

        void SetColumnWidth()
        {
            int width = this.list.ClientSize.Width - this.countColumn.Width;
            if (width != this.nameColumn.Width)
            {
                this.nameColumn.Width = width;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.list.SelectedIndices.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}