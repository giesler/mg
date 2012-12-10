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
using mn2.net.ShoppingList;

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
                }
            }

            if (this.settings == null)
            {
                this.settings = new LocalSettings();
                this.settings.Settings.Add("LastUpdate", DateTime.MinValue.ToString());
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
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.menuRefresh.Enabled = false;

            if (this.settings.LatestItems.Count > 0)
            {
                this.DisplayStores(null);
                this.LoadShoppingListItems(null);
                this.listView1.CheckBoxes = false;
                this.LoadAllItems();
                // TODO: add flag to get latest list of stores after getting items
            }
            else
            {
                this.add.Enabled = false;
                this.newItem.Enabled = false;
                this.store.Enabled = false;
                this.loadAllItemsAfterStores = true;

                this.statusLabel.Text = "Connecting...";

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
                    if (this.settings.LatestItems.ContainsKey(store) == false)
                    {
                        this.settings.LatestItems.Add(store, new StoreItem(store));
                    }
                }

                this.SaveSettings();

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

            foreach (StoreItem store in this.settings.LatestItems.Values)
            {
                this.store.Items.Add(store);
            }
            this.store.SelectedIndex = 0;

            this.store.Enabled = true;
            this.add.Enabled = true;
            this.newItem.Enabled = true;
            this.storesLoaded = true;
            this.UpdateStatus();

            if (this.loadAllItemsAfterStores == true)
            {
                this.loadAllItemsAfterStores = false;
                this.LoadAllItems();
            }
        }

        private void UpdateStatus()
        {
            DateTime lastUpdate = DateTime.MinValue;
            if (this.settings.Settings["LastUpdate"] != null)
            {
                lastUpdate = DateTime.Parse(this.settings.Settings["LastUpdate"]);
            }

            if (lastUpdate == DateTime.MinValue)
            {
                this.statusLabel.Text = "Last update: never";
            }
            else
            {
                TimeSpan duration = DateTime.Now - lastUpdate;

                string durationText = GetDurationString(duration);

                this.statusLabel.Text = "Last update: " + durationText;
            }

            if (this.menuRefresh.Enabled == false)
            {
                this.statusLabel.Text += ".  Refreshing...";
            }
        }
        
        static string GetDurationString(TimeSpan duration)
        {
            string durationText = string.Empty;
            if (duration.TotalMinutes < 1)
            {
                durationText = "< 1 minute ago";
            }
            else if (duration.TotalMinutes == 1)
            {
                durationText = string.Format("1 minute ago");
            }
            else if (duration.TotalMinutes < 60)
            {
                durationText = string.Format("{0:0} minutes ago", duration.TotalMinutes);
            }
            else if (duration.TotalHours == 1)
            {
                durationText = string.Format("1 hour ago");
            }
            else if (duration.TotalHours < 24)
            {
                durationText = string.Format("{0:0} hours ago", duration.TotalHours);
            }
            else if (duration.TotalDays == 1)
            {
                durationText = string.Format("1 day ago");
            }
            else
            {
                durationText = string.Format("{0:0} days ago", duration.TotalDays);
            }
            return durationText;
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
            this.add.Enabled = false;
            this.listView1.CheckBoxes = false;

            this.UpdateStatus();

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetAllItems), new object());
        }

        private void GetAllItems(object foo)
        {
            try
            {
                ShoppingListItem[] items = this.listService.GetShoppingListItems();

                foreach (StoreItem store in this.settings.LatestItems.Values)
                {
                    store.Items.Clear();
                }

                foreach (ShoppingListItem item in items)
                {
                    if (this.settings.LatestItems.ContainsKey(item.Store) == false)
                    {
                        this.settings.LatestItems.Add(item.Store, new StoreItem(item.Store));
                    }
                    this.settings.LatestItems[item.Store].Items.Add(item);
                }

                this.settings.Settings["LastUpdate"] = DateTime.Now.ToString();
                this.SaveSettings();

                this.Invoke(new WaitCallback(this.LoadShoppingListItems), foo);
            }
            catch (Exception ex)
            {
                this.DisplayException(ex);
            }
        }

        private void LoadShoppingListItems(object foo)
        {
            this.ReloadItemList();

            this.add.Enabled = true;
            this.listView1.CheckBoxes = true;
            this.menuRefresh.Enabled = true;

            this.UpdateStatus();
        }

        private void ReloadItemList()
        {
            this.listView1.Items.Clear();

            StoreItem selectedStore = this.store.SelectedItem as StoreItem;
            if (selectedStore != null)
            {
                var q = from i in selectedStore.Items
                        orderby i.ListItem
                        select i;
                foreach (ShoppingListItem item in q)
                {
                    this.listView1.Items.Add(new ShoppingListViewItem(item));
                }
            }

            this.storesLoaded = false;

            this.store.Items.Clear();

            foreach (StoreItem item in this.settings.LatestItems.Values)
            {
                this.store.Items.Add(item);

                if (item == selectedStore)
                {
                    this.store.SelectedItem = item;
                }
            }

            this.UpdateStatus();

            this.storesLoaded = true;
        }

        private void SaveSettings()
        {
            this.settings.Save("shoppingListSettings.xml");
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.listView1.Columns[0].Width = this.listView1.ClientSize.Width - 9;
        }

        private void add_Click(object sender, EventArgs e)
        {
            StoreItem selectedStore = this.store.SelectedItem as StoreItem;
            if (selectedStore != null && this.newItem.Text.Trim().Length > 0)
            {
                ShoppingListItem item = new ShoppingListItem();
                item.Store = selectedStore.Name;
                item.ListItem = this.newItem.Text.Trim();
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

            this.settings.LatestItems[store.Name].Items.Remove(li.Item);
            if (store == this.store.SelectedItem)
            {
                li.UpdateItem(newItem);
            }
            this.settings.LatestItems[store.Name].Items.Add(newItem);

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

            foreach (StoreItem store in this.settings.LatestItems.Values)
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

            this.Close();

            Application.Exit();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();

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
                if (this.IsDisposed == false)
                {
                    this.Invoke(new ExceptionEventHandler(this.DisplayException), ex);
                }
            }
            else
            {
                ExceptionDialog dialog = new ExceptionDialog(ex);
                dialog.ShowDialog();
            }
        }

        private void newItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                this.add_Click(this, EventArgs.Empty);
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                this.updateTimer.Enabled = false;
            }

            this.UpdateStatus();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            this.UpdateStatus();
            this.updateTimer.Enabled = true;
        }

        private void listViewContextMenu_Popup(object sender, EventArgs e)
        {
            this.menuMoveToList.Enabled = false;
            StoreItem selectedStore = this.store.SelectedItem as StoreItem;

            if (this.listView1.SelectedIndices.Count > 0 && selectedStore != null)
            {
                this.menuMoveToList.Enabled = true;
                this.menuMoveToList.MenuItems.Clear();

                foreach (StoreItem store in this.settings.LatestItems.Values)
                {
                    if (store != selectedStore)
                    {
                        MenuItem storeItem = new MenuItem();
                        storeItem.Text = store.Name;
                        storeItem.Click += new EventHandler(storeItem_Click);
                        this.menuMoveToList.MenuItems.Add(storeItem);
                    }
                }
            }
        }

        void storeItem_Click(object sender, EventArgs e)
        {
            MenuItem storeMenuItem = (MenuItem)sender;
            StoreItem currentStore = this.store.SelectedItem as StoreItem;

            if (this.listView1.SelectedIndices.Count > 0 && currentStore != null)
            {
                StoreItem newStore = this.settings.LatestItems[storeMenuItem.Text];
                ShoppingListViewItem moveItem = (ShoppingListViewItem)this.listView1.Items[this.listView1.SelectedIndices[0]];
                
                currentStore.Items.Remove(moveItem.Item);
                newStore.Items.Add(moveItem.Item);

                moveItem.Item.Store = newStore.Name;

                ThreadPool.QueueUserWorkItem(new WaitCallback(this.UpdateListItem), moveItem.Item);
                                
                this.UpdateStoreCount(currentStore);
                this.UpdateStoreCount(newStore);

                this.listView1.Items.Remove(moveItem);
            }
        }

        void UpdateListItem(object oItem)
        {
            ShoppingListItem item = (ShoppingListItem)oItem;

            this.listService.UpdateShoppingListItem(item);
        }

        private void menuEdit_Click(object sender, EventArgs e)
        {            
            StoreItem store = this.store.SelectedItem as StoreItem;

            if (this.listView1.SelectedIndices.Count > 0 && store != null)
            {
                ShoppingListViewItem editItem = (ShoppingListViewItem)this.listView1.Items[this.listView1.SelectedIndices[0]];

                EditItemForm itemEditor = new EditItemForm();
                itemEditor.Text = store.Name;
                itemEditor.ItemText = editItem.Item.ListItem;
                if (itemEditor.ShowDialog() == DialogResult.OK)
                {
                    if (editItem.Item.ListItem != itemEditor.ItemText)
                    {
                        editItem.Item.ListItem = itemEditor.ItemText;
                        editItem.UpdateItem(editItem.Item);

                        ThreadPool.QueueUserWorkItem(UpdateListItem, editItem.Item);
                    }
                }
            }
        }
    }
}