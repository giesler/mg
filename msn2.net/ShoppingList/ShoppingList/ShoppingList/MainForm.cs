using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;
using mn2.net.ShoppingList.sls;

namespace msn2.net.ShoppingList
{
    public partial class MainForm : Form
    {
        private bool storesLoaded = false;
        private LocalSettings settings = null;
        private ShoppingListItem undoItem = null;
        private float defaultFontSize = 10;
        private float currentZoom = 1.0F;
        private ShoppingListService listService = null;
        private List<ShoppingListItem> loadedItems = null;
        private List<StoreItem> loadedStores = new List<StoreItem>();
        private bool loadAllItemsAfterStores = false;

        public MainForm()
        {
            InitializeComponent();

            if (File.Exists("shoppingListSettings.xml"))
            {
                try
                {
                    this.settings = LocalSettings.ReadFromFile("shoppingListSettings.xml");
                }
                catch (Exception)
                {
                    this.settings = new LocalSettings();
                }
            }
            else
            {
                this.settings = new LocalSettings();
            }

            this.defaultFontSize = this.listView1.Font.Size;

            this.listService = new mn2.net.ShoppingList.sls.ShoppingListService();
            this.listService.Credentials = new NetworkCredential("mc", "4362", "sp");

#if DEBUG
            this.listService.Proxy = new WebProxy("http://192.168.1.1:8080");
#endif
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this.settings.Save("shoppingListSettings.xml");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.listView1.Enabled = false;
            this.menuRefresh.Enabled = false;

            if (this.settings.LatestStores.Count > 0)
            {
                foreach (string store in this.settings.LatestStores)
                {
                    this.store.Items.Add(new StoreItem(store));
                }

                this.store.SelectedIndex = 0;

                this.LoadAllItems();
                // TODO: add flag to get latest list of stores after getting items
            }
            else
            {
                this.add.Enabled = false;
                this.newItem.Enabled = false;
                this.store.Enabled = false;
                this.loadAllItemsAfterStores = true;

                this.store.Items.Add("Loading...");
                this.store.SelectedIndex = 0;

                ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadStoreList), new object());
            }
        }

        private void LoadStoreList(object foo)
        {
            try
            {
                string[] stores = this.listService.GetStores();

                foreach (string store in stores)
                {
                    this.loadedStores.Add(new StoreItem(store));
                }

                this.Invoke(new WaitCallback(this.DisplayStores), new object());
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }
        }

        private void DisplayStores(object foo)
        {
            this.store.Items.Clear();

            foreach (StoreItem store in this.loadedStores)
            {
                this.store.Items.Add(store);
            }
            this.store.SelectedIndex = 0;

            this.store.Enabled = true;
            this.add.Enabled = true;
            this.newItem.Enabled = true;
            this.storesLoaded = true;

            if (this.loadAllItemsAfterStores == true)
            {
                this.loadAllItemsAfterStores = false;
                this.LoadAllItems();
            }
        }

        private void AddAttribute(XmlDocument doc, XmlNode node, string name, string val)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = val;
            node.Attributes.Append(att);
        }

        void LoadAllItems()
        {
            this.menuRefresh.Enabled = false;
            this.listView1.Enabled = false;
            this.listView1.CheckBoxes = false;
            this.listView1.Items.Clear();
            this.listView1.Items.Add(new ListViewItem("Loading..."));

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetAllItems), new object());
        }

        private void GetAllItems(object foo)
        {
            try
            {
                ShoppingListItem[] items = this.listService.GetShoppingListItems();
                
                this.loadedItems = new List<ShoppingListItem>(items);

                this.Invoke(new WaitCallback(this.LoadShoppingListItems), foo);
            }
            catch (Exception ex)
            {
                this.DisplayException(ex);
            }
        }

        private void LoadShoppingListItems(object foo)
        {
            foreach (StoreItem store in this.loadedStores)
            {
                store.Items.Clear();
            }

            foreach (ShoppingListItem item in this.loadedItems)
            {
                bool foundStore = false;
                foreach (StoreItem store in this.store.Items)
                {
                    if (string.Equals(store.Name, item.Store, StringComparison.InvariantCultureIgnoreCase))
                    {
                        foundStore = true;
                        store.Items.Add(item);
                    }
                }

                if (foundStore == false)
                {
                    StoreItem newStore = new StoreItem(item.Store);
                    newStore.Items.Add(item);
                    this.store.Items.Add(newStore);
                }
            }

            this.ReloadItemList();

            this.listView1.Enabled = true;
            this.listView1.CheckBoxes = true;
            this.menuRefresh.Enabled = true;
        }

        private void ReloadItemList()
        {
            this.listView1.Items.Clear();

            StoreItem selectedStore = this.store.SelectedItem as StoreItem;
            if (selectedStore != null)
            {
                foreach (ShoppingListItem item in selectedStore.Items)
                {
                    this.listView1.Items.Add(new ShoppingListViewItem(item));
                }
            }

            this.storesLoaded = false;

            this.store.Items.Clear();

            foreach (StoreItem item in this.loadedStores)
            {
                this.store.Items.Add(item);

                if (item == selectedStore)
                {
                    this.store.SelectedItem = item;
                }
            }

            this.storesLoaded = true;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.listView1.Columns[0].Width = this.listView1.ClientSize.Width - 5;
        }

        private void add_Click(object sender, EventArgs e)
        {
            StoreItem selectedStore = this.store.SelectedItem as StoreItem;
            if (selectedStore != null)
            {
                ShoppingListItem item = new ShoppingListItem();
                item.Store = selectedStore.Name;
                item.ListItem = this.newItem.Text;
                selectedStore.Items.Add(item);

                UpdateStoreCount(selectedStore);

                ShoppingListViewItem sli = new ShoppingListViewItem(item);
                this.listView1.Items.Add(sli);

                ThreadPool.QueueUserWorkItem(new WaitCallback(this.AddItem), sli);

                this.newItem.Text = string.Empty;
                this.newItem.Focus();
            }
        }

        private void UpdateStoreCount(StoreItem item)
        {
            bool selected = false;

            this.storesLoaded = false;

            for (int i = 0; i < this.store.Items.Count; i++)
            {
                StoreItem currentItem = this.store.Items[i] as StoreItem;
                if (currentItem == item)
                {
                    if (this.store.SelectedItem == currentItem)
                    {
                        selected = true;
                    }

                    this.store.Items.RemoveAt(i);
                    this.store.Items.Insert(i, currentItem);
                    if (selected == true)
                    {
                        this.store.SelectedIndex = i;
                    }

                    break;
                }
            }

            this.storesLoaded = true;
        }

        private void AddItem(object listViewItem)
        {
            try
            {
                ShoppingListViewItem li = (ShoppingListViewItem)listViewItem;
                ShoppingListItem item = this.listService.AddShoppingListItem(li.Item.Store, li.Item.ListItem);
                this.BeginInvoke(new DisplayAddedDelegate(this.DisplayAddedItem), li, item);
            }
            catch (Exception ex)
            {
                this.DisplayException(ex);
            }
        }

        private delegate void DisplayAddedDelegate(ShoppingListViewItem li, ShoppingListItem newItem);

        private void DisplayAddedItem(ShoppingListViewItem li, ShoppingListItem newItem)
        {
            StoreItem store = this.GetStore(li.Item.Store);
            if (store != null)
            {
                store.Items.Remove(li.Item);
            }

            this.loadedItems.Remove(li.Item);
            if (store == this.store.SelectedItem)
            {
                li.UpdateItem(newItem);
            }
            this.loadedItems.Add(newItem);
            
            if (store != null)
            {
                store.Items.Add(li.Item);
            }
        }

        private void store_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.storesLoaded == true)
            {
                this.ReloadItemList();
            }
        }

        private void menuRefresh_Click(object sender, EventArgs e)
        {
            this.LoadAllItems();
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Checked)
            {
                e.NewValue = CheckState.Checked;
            }
            else
            {
                ShoppingListViewItem item = (ShoppingListViewItem)this.listView1.Items[e.Index];
                item.Deleted = true;
                item.UpdateItem(item.Item);

                if (item.Item.Id == 0)
                {
                    e.NewValue = CheckState.Unchecked;
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.DeleteItem), item);

                    item.ForeColor = Color.Gray;
                    item.Selected = false;

                    StoreItem selectedStore = this.store.SelectedItem as StoreItem;

                    undoItem = new ShoppingListItem();
                    undoItem.Store = selectedStore.Name;
                    undoItem.ListItem = item.Item.ListItem;

                    this.menuUndo.Enabled = true;
                }
            }
        }

        void DeleteItem(object listItem)
        {
            ShoppingListViewItem item = (ShoppingListViewItem)listItem;

            this.listService.DeleteShoppingListItem(item.Item);
            
            this.Invoke(new WaitCallback(this.DeleteCompleted), listItem);
        }

        void DeleteCompleted(object listItem)
        {
            ShoppingListViewItem item = (ShoppingListViewItem)listItem;
            this.listView1.Items.Remove(item);

            StoreItem store = GetStore(item.Item.Store);
            if (store != null)
            {
                store.Items.Remove(item.Item);
                this.UpdateStoreCount(store);
            }
        }

        private StoreItem GetStore(string storeName)
        {
            StoreItem match = null;

            foreach (StoreItem store in this.loadedStores)
            {
                if (string.Equals(store.Name, storeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    match = store;
                    break;
                }
            }

            return match;
        }

        private void menuUndo_Click(object sender, EventArgs e)
        {
            ShoppingListViewItem sli = new ShoppingListViewItem(this.undoItem);
            StoreItem store = this.GetStore(sli.Item.Store);

            if (store == this.store.SelectedItem)
            {
                this.listView1.Items.Add(sli);
            }
            else
            {
                string text = string.Format("'{0}' has been restored to '{1}'.", sli.Item.ListItem, sli.Item.Store);
                MessageBox.Show(text, "Undo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            }            

            if (store != null)
            {
                store.Items.Add(this.undoItem);
                this.UpdateStoreCount(store);
            }

            AddItem(sli);

            this.undoItem = null;
            this.menuUndo.Enabled = false;
        }

        private void menuUpdateApp_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("http://home.msn2.net/cab/ShoppingListCab.cab", "");
            p.Start();

            Application.Exit();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuTextNormal_Click(object sender, EventArgs e)
        {
            CheckMenuItem(menuTextNormal, 1.0F);
        }

        private void menuTextBigger_Click(object sender, EventArgs e)
        {
            CheckMenuItem(menuTextBigger, 1.2F);
        }

        private void menuTextSmall_Click(object sender, EventArgs e)
        {
            CheckMenuItem(menuTextSmall, 0.8F);
        }

        private void menuTextBiggest_Click(object sender, EventArgs e)
        {
            CheckMenuItem(menuTextBiggest, 1.4F);
        }

        private void menuTextSmallest_Click(object sender, EventArgs e)
        {
            CheckMenuItem(menuTextBiggest, 0.6F);
        }

        private void CheckMenuItem(MenuItem menuItem, float newZoom)
        {
            this.menuTextBiggest.Checked = false;
            this.menuTextBigger.Checked = false;
            this.menuTextNormal.Checked = false;
            this.menuTextSmall.Checked = false;
            this.menuTextSmallest.Checked = false;

            menuItem.Checked = true;

            this.currentZoom = newZoom;
            this.SizeControls();
        }

        private void SizeControls()
        {
            float newSize = this.defaultFontSize * this.currentZoom;
            this.listView1.Font = new Font(this.listView1.Font.Name, newSize, this.listView1.Font.Style);
        }



        private delegate void ExceptionEventHandler(Exception ex);
        private void DisplayException(Exception ex)
        {
            if (this.InvokeRequired == true)
            {
                this.Invoke(new ExceptionEventHandler(this.DisplayException), ex);
            }
            else
            {
                ExceptionDialog dialog = new ExceptionDialog(ex);
                dialog.ShowDialog();
            }
        }

    }
}