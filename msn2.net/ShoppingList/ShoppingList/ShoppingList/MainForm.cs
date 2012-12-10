﻿using System;
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
using System.Net.Sockets;
using Microsoft.WindowsCE.Forms;
using WinForms = System.Windows.Forms;

namespace msn2.net.ShoppingList
{
    public partial class MainForm : Form
    {
        LocalSettings settings = null;
        ShoppingListItem undoItem = null;
        float defaultFontSize = 10;
        float currentZoom = 1.0F;
        ShoppingListService listService = null;
        bool loadAllItemsAfterStores = false;
        StoreItem selectedStore = null;
        object lockObject = new object();
        public static Color[] ListColors = new Color[] { Color.LightCyan, Color.White };

        IContainer components = null;
        MainMenu mainMenu1;
        ListView listView1;
        ColumnHeader Item;
        TextBox newItem;
        Button add;
        MenuItem menuRefresh;
        MenuItem rightMenu;
        MenuItem menuUndo;
        MenuItem menuUpdateApp;
        MenuItem menuExit;
        MenuItem menuTextSize;
        MenuItem menuTextBigger;
        MenuItem menuTextNormal;
        MenuItem menuTextSmall;
        MenuItem menuTextBiggest;
        MenuItem menuTextSmallest;
        Label statusLabel;
        WinForms.Timer updateTimer;
        ContextMenu listViewContextMenu;
        MenuItem menuMoveToList;
        MenuItem menuEdit;
        Button switchStore;
        Label storeLabel;
        WinForms.Timer timerRetryRefresh;
        WinForms.Timer timerResizeCheck;
        WinForms.Timer timerProgress;
        InputPanel inputPanel1;
        PerfTimer perfTimer = new PerfTimer();
        private bool settingsLoaded = false;
        ProgressBar progressBar = null;

        public MainForm()
        {
            perfTimer.Start();

            OutputTicks("Start");

            this.SuspendLayout();

            this.mainMenu1 = new MainMenu();
            this.Menu = this.mainMenu1;

            this.menuRefresh = new MenuItem();
            this.menuRefresh.Enabled = false;
            this.menuRefresh.Text = "&Refresh";
            this.menuRefresh.Click += new EventHandler(this.menuRefresh_Click);
            this.mainMenu1.MenuItems.Add(this.menuRefresh);

            this.rightMenu = new MenuItem();
            this.rightMenu.Text = "&Menu";
            this.mainMenu1.MenuItems.Add(this.rightMenu);

            #region Right Menu

            this.menuUndo = new MenuItem();
            this.menuUndo.Enabled = false;
            this.menuUndo.Text = "&Undo";
            this.menuUndo.Click += new EventHandler(this.menuUndo_Click);
            this.rightMenu.MenuItems.Add(this.menuUndo);

            this.rightMenu.MenuItems.Add(new MenuItem() { Text = "-" });

            this.menuUpdateApp = new MenuItem();
            this.menuUpdateApp.Text = "U&pdate app";
            this.menuUpdateApp.Click += new EventHandler(this.menuUpdateApp_Click);
            this.rightMenu.MenuItems.Add(this.menuUpdateApp);

            this.rightMenu.MenuItems.Add(new MenuItem() { Text = "-" });

            this.menuTextSize = new MenuItem();
            this.menuTextSize.Text = "Text Size";
            this.rightMenu.MenuItems.Add(this.menuTextSize);

            this.menuTextBiggest = new MenuItem();
            this.menuTextBiggest.Text = "&Biggest";
            this.menuTextBiggest.Click += new EventHandler(this.menuTextBiggest_Click);
            this.menuTextSize.MenuItems.Add(this.menuTextBiggest);

            this.menuTextBigger = new MenuItem();
            this.menuTextBigger.Text = "&Bigger";
            this.menuTextBigger.Click += new EventHandler(this.menuTextBigger_Click);
            this.menuTextSize.MenuItems.Add(this.menuTextBigger);

            this.menuTextNormal = new MenuItem();
            this.menuTextNormal.Checked = true;
            this.menuTextNormal.Text = "&Normal";
            this.menuTextNormal.Click += new EventHandler(this.menuTextNormal_Click);
            this.menuTextSize.MenuItems.Add(this.menuTextNormal);

            this.menuTextSmall = new MenuItem();
            this.menuTextSmall.Text = "&Small";
            this.menuTextSmall.Click += new EventHandler(this.menuTextSmall_Click);
            this.menuTextSize.MenuItems.Add(this.menuTextSmall);

            this.menuTextSmallest = new MenuItem();
            this.menuTextSmallest.Text = "Smallest";
            this.menuTextSmallest.Click += new EventHandler(this.menuTextSmallest_Click);
            this.menuTextSize.MenuItems.Add(this.menuTextSmallest);

            this.rightMenu.MenuItems.Add(new MenuItem() { Text = "-" });

            this.menuExit = new MenuItem();
            this.menuExit.Text = "E&xit";
            this.menuExit.Click += new EventHandler(this.menuExit_Click);
            this.rightMenu.MenuItems.Add(this.menuExit);

            #endregion

            this.listView1 = new ListView();
            this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = ColumnHeaderStyle.None;
            this.listView1.Bounds = new Rectangle(3, 50, 234, 206);
            this.listView1.TabIndex = 0;
            this.listView1.View = View.Details;
            this.listView1.ItemCheck += new ItemCheckEventHandler(this.listView1_ItemCheck);
            this.listView1.Parent = this;
            this.defaultFontSize = this.listView1.Font.Size;

            this.newItem = new TextBox();
            this.newItem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.newItem.Bounds = new Rectangle(3, 26, 164, 21);
            this.newItem.TabIndex = 1;
            this.newItem.GotFocus += new EventHandler(this.newItem_GotFocus);
            this.newItem.KeyPress += new KeyPressEventHandler(this.newItem_KeyPress);
            this.newItem.LostFocus += new EventHandler(this.newItem_LostFocus);
            this.newItem.Parent = this;

            this.add = new Button();
            this.add.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.add.Bounds = new Rectangle(173, 26, 64, 21);
            this.add.TabIndex = 2;
            this.add.Text = "&Add";
            this.add.Click += new EventHandler(this.add_Click);
            this.add.Parent = this;
            this.add.Enabled = false;

            this.statusLabel = new Label();
            this.statusLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.statusLabel.Font = new Font("Tahoma", 6F, FontStyle.Regular);
            this.statusLabel.Bounds = new Rectangle(3, 257, 234, 10);
            this.statusLabel.Text = "Last update: never";
            this.statusLabel.Parent = this;

            this.updateTimer = new WinForms.Timer();
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 20000;
            this.updateTimer.Tick += new EventHandler(this.updateTimer_Tick);

            this.switchStore = new Button();
            this.switchStore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.switchStore.Bounds = new Rectangle(173, 2, 65, 21);
            this.switchStore.TabIndex = 4;
            this.switchStore.Text = "&Switch";
            this.switchStore.Click += new EventHandler(this.switchStore_Click);
            this.switchStore.Parent = this;
            this.switchStore.Enabled = false;

            this.storeLabel = new Label();
            this.storeLabel.Font = new Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.storeLabel.Bounds = new Rectangle(5, 2, 162, 24);
            this.storeLabel.Text = "Loading...";
            this.storeLabel.Parent = this;

            this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
            this.inputPanel1.EnabledChanged += new EventHandler(this.inputPanel1_EnabledChanged);

            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);

            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Text = "Shopping List";

            this.progressBar = new ProgressBar();
            this.progressBar.Bounds = new Rectangle(3, 249, 234, 8);
            this.progressBar.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            this.progressBar.Maximum = 30;
            this.progressBar.Parent = this;

            this.timerProgress = new WinForms.Timer();
            this.timerProgress.Tick += new EventHandler(timerProgress_Tick);
            this.timerProgress.Interval = 1000;
            
            this.ResumeLayout(false);

            OutputTicks("init done");

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadSettings), new object());

            this.LoadAllItems();

            this.UpdateStatus();

            OutputTicks("ctor done");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        void OutputTicks(string name)
        {
            Int64 elapsed = this.perfTimer.ElapsedTime();
            Trace.WriteLine(string.Format("{0} - {1}", name, elapsed));
        }

        void LoadSettings(object foo)
        {
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
            }

            this.Invoke(new WaitCallback(this.DisplayLoadedSettings), new object());
        }

        void DisplayLoadedSettings(object foo)
        {
            OutputTicks("CallbackFromSettingsLoaded");

            if (this.settings.Settings.ContainsKey("LastUpdate") == false)
            {
                this.settings.Settings.Add("LastUpdate", DateTime.MinValue.ToString());
            }
            if (this.settings.Settings.ContainsKey("TextSize") == false)
            {
                this.settings.Settings.Add("TextSize", "1.2");
            }

            float textSize = float.Parse(this.settings.Settings["TextSize"]);
            this.CheckMenuItem(textSize);

            if (this.settings.LatestItems.Count > 0)
            {
                this.switchStore.Enabled = true;

                this.DisplayStores(null);
                OutputTicks("DisplayStores");
                this.LoadShoppingListItems(false);
                OutputTicks("LoadShoppingListItems");
                this.listView1.CheckBoxes = false;
                // TODO: add flag to get latest list of stores after getting items
            }
            else
            {
                this.loadAllItemsAfterStores = true;

                this.statusLabel.Text = "Connecting...";

                ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadStoreList), foo);
            }

            OutputTicks("DisplayLoadedSettingsEnd");

            this.settingsLoaded = true;

            this.UpdateStatus();
        }

        object listServiceLoadObject = new object();

        ShoppingListService GetShoppingListService()
        {
            if (this.listService == null)
            {
                lock (this.listServiceLoadObject)
                {
                    if (this.listService == null)
                    {
                        this.listService = new mn2.net.ShoppingList.sls.ShoppingListService();
                        this.listService.Credentials = new NetworkCredential("mc", "4362", "sp");
#if DEBUG
                        this.listService.Proxy = new WebProxy("http://192.168.1.1:8080");
#endif
                    }
                }
            }

            return this.listService;
        }

        void LoadStoreList(object foo)
        {
            try
            {
                string[] stores = this.GetShoppingListService().GetStores();

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

        void DisplayStores(object foo)
        {
            if (this.selectedStore == null)
            {
                this.selectedStore = this.settings.latestItems.Values.First<StoreItem>();
            }

            this.storeLabel.Text = this.selectedStore.Name;

            this.switchStore.Enabled = true;
            
            this.UpdateStatus();

            if (this.loadAllItemsAfterStores == true)
            {
                this.loadAllItemsAfterStores = false;
                this.LoadAllItems();
            }
        }

        void UpdateStatus()
        {
            DateTime lastUpdate = DateTime.MinValue;
            if (this.settings != null && this.settings.Settings.ContainsKey("LastUpdate") == true)
            {
                lastUpdate = DateTime.Parse(this.settings.Settings["LastUpdate"]);
            }

            TimeSpan duration = DateTime.Now - lastUpdate;

            if (lastUpdate == DateTime.MinValue || duration.TotalDays > 90)
            {
                this.statusLabel.Text = "Last update: never";
            }
            else
            {
                string durationText = GetDurationString(duration);

                this.statusLabel.Text = "Last update: " + durationText;
            }

            if (this.menuRefresh.Enabled == false)
            {
                if (this.settingsLoaded == false)
                {
                    this.statusLabel.Text = "Reading settings...";
                }
                else if (this.timerRetryRefresh != null && this.timerRetryRefresh.Enabled == true)
                {
                    this.statusLabel.Text += ".  Connection failed.  Will retry.";
                }
                else
                {
                    this.statusLabel.Text += ".  Refreshing...";
                }
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

        void AddAttribute(XmlDocument doc, XmlNode node, string name, string val)
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

            this.progressBar.Visible = true;
            this.progressBar.Value = 0;
            this.SizeControls();
            this.timerProgress.Interval = 250;
            this.timerProgress.Enabled = true;

            this.UpdateStatus();

            this.TriggerResizeCheck();

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetAllItems), new object());
        }

        void GetAllItems(object foo)
        {
            try
            {
                ShoppingListItem[] items = this.GetShoppingListService().GetShoppingListItems();

                this.WaitForSettingsToLoad();

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

                this.Invoke(new WaitCallback(this.LoadShoppingListItems), true);
            }
            catch (SocketException ex)
            {
                this.WaitForSettingsToLoad();
                this.Invoke(new WaitCallback(this.ConnectFailure), ex);
            }
            catch (WebException ex)
            {
                this.WaitForSettingsToLoad();
                this.Invoke(new WaitCallback(this.ConnectFailure), ex);
            }
            catch (Exception ex)
            {
                this.DisplayException(ex);
            }
        }

        void WaitForSettingsToLoad()
        {
            while (this.settingsLoaded == false)
            {
                Thread.Sleep(100);
            }
        }

        void ConnectFailure(object foo)
        {
            if (this.timerRetryRefresh == null)
            {
                this.timerRetryRefresh = new WinForms.Timer();
                this.timerRetryRefresh.Interval = 1000;
                this.timerRetryRefresh.Tick += new EventHandler(this.timerRetryRefresh_Tick);
            }

            this.timerRetryRefresh.Interval = this.timerRetryRefresh.Interval * 2;
            this.timerRetryRefresh.Enabled = true;

            if (this.timerRetryRefresh.Interval > 60 * 1000)
            {
                this.timerRetryRefresh.Interval = 60 * 1000;
            }

            this.HideProgress();

            this.UpdateStatus();
        }

        void LoadShoppingListItems(object oEnableControls)
        {
            this.ReloadItemList();

            bool enableControls = (bool)oEnableControls;
            this.add.Enabled = enableControls;
            this.listView1.CheckBoxes = enableControls;
            this.menuRefresh.Enabled = enableControls;

            if (enableControls == true)
            {
                this.HideProgress();
            }

            this.UpdateStatus();
        }

        void HideProgress()
        {
            this.progressBar.Visible = false;
            this.timerProgress.Enabled = false;
            this.SizeControls();
        }

        void ReloadItemList()
        {
            if (this.listView1.Columns.Count == 0)
            {
                this.Item = new ColumnHeader();
                this.Item.Text = "item";
                this.Item.Width = this.listView1.ClientSize.Width - 9;
                this.listView1.Columns.Add(this.Item);

                this.listViewContextMenu = new ContextMenu();
                this.listViewContextMenu.Popup += new EventHandler(this.listViewContextMenu_Popup);
                this.listView1.ContextMenu = this.listViewContextMenu;
            }

            this.listView1.Items.Clear();

            if (this.selectedStore != null)
            {
                int index = 0;
                var q = from i in this.selectedStore.Items
                        orderby i.ListItem
                        select i;
                this.listView1.BeginUpdate();
                foreach (ShoppingListItem item in q)
                {
                    ShoppingListViewItem listItem = new ShoppingListViewItem(item);
                    listItem.BackColor = MainForm.ListColors[index];
                    index++;
                    if (index >= MainForm.ListColors.Length)
                    {
                        index = 0;
                    }
                    this.listView1.Items.Add(listItem);
                }
                this.listView1.EndUpdate();
            }

            this.UpdateStatus();
        }

        void SaveSettings()
        {
            lock (this.lockObject)
            {
                this.settings.Save("shoppingListSettings.xml");
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.listView1.Columns.Count > 0)
            {
                this.SetColumnWidth();

                TriggerResizeCheck();
            }
        }

        private void TriggerResizeCheck()
        {
            if (this.timerResizeCheck == null)
            {
                this.timerResizeCheck = new WinForms.Timer();
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
            int width = this.listView1.ClientSize.Width - 9;
            if (this.listView1.Columns[0].Width != width)
            {
                this.listView1.Columns[0].Width = width;
            }
        }

        void add_Click(object sender, EventArgs e)
        {
            if (this.selectedStore != null && this.newItem.Text.Trim().Length > 0)
            {
                ShoppingListItem item = new ShoppingListItem();
                item.Store = selectedStore.Name;
                item.ListItem = this.newItem.Text.Trim();
                selectedStore.Items.Add(item);

                ShoppingListViewItem sli = new ShoppingListViewItem(item);
                this.listView1.Items.Add(sli);
                sli.BackColor = sli.Index % 2 == 0 ? MainForm.ListColors[0] : MainForm.ListColors[1];
                
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.AddItem), sli);

                this.newItem.Text = string.Empty;
                this.newItem.Focus();
            }
        }

        void AddItem(object listViewItem)
        {
            try
            {
                ShoppingListViewItem li = (ShoppingListViewItem)listViewItem;
                ShoppingListItem item = this.GetShoppingListService().AddShoppingListItem(li.Item.Store, li.Item.ListItem);
                this.BeginInvoke(new DisplayAddedDelegate(this.DisplayAddedItem), li, item);
            }
            catch (Exception ex)
            {
                this.DisplayException(ex);
            }
        }

        delegate void DisplayAddedDelegate(ShoppingListViewItem li, ShoppingListItem newItem);

        void DisplayAddedItem(ShoppingListViewItem li, ShoppingListItem newItem)
        {
            StoreItem store = this.GetStore(li.Item.Store);
            store.Items.Remove(li.Item);

            if (store == this.selectedStore)
            {
                li.UpdateItem(newItem);
            }

            store.Items.Add(li.Item);
        }

        void menuRefresh_Click(object sender, EventArgs e)
        {
            this.LoadAllItems();
        }

        void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
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

                    undoItem = new ShoppingListItem();
                    undoItem.Store = this.selectedStore.Name;
                    undoItem.ListItem = item.Item.ListItem;

                    this.menuUndo.Enabled = true;
                }
            }
        }

        void DeleteItem(object listItem)
        {
            ShoppingListViewItem item = (ShoppingListViewItem)listItem;

            this.GetShoppingListService().DeleteShoppingListItem(item.Item);

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
            }
        }

        StoreItem GetStore(string storeName)
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

        void menuUndo_Click(object sender, EventArgs e)
        {
            ShoppingListViewItem sli = new ShoppingListViewItem(this.undoItem);
            StoreItem store = this.GetStore(sli.Item.Store);

            if (store == this.selectedStore)
            {
                this.listView1.Items.Add(sli);
                sli.BackColor = sli.Index % 2 == 0 ? MainForm.ListColors[0] : MainForm.ListColors[1];
            }
            else
            {
                string text = string.Format("'{0}' has been restored to '{1}'.", sli.Item.ListItem, sli.Item.Store);
                MessageBox.Show(text, "Undo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            }

            if (store != null)
            {
                store.Items.Add(this.undoItem);
            }

            AddItem(sli);

            this.undoItem = null;
            this.menuUndo.Enabled = false;
        }

        void menuUpdateApp_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("http://home.msn2.net/cab/ShoppingListCab.cab", "");
            p.Start();

            this.Close();

            Application.Exit();
        }

        void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();

            Application.Exit();
        }

        void menuTextNormal_Click(object sender, EventArgs e)
        {
            CheckMenuItem(1.0F);
        }

        void menuTextBigger_Click(object sender, EventArgs e)
        {
            CheckMenuItem(1.2F);
        }

        void menuTextSmall_Click(object sender, EventArgs e)
        {
            CheckMenuItem(0.8F);
        }

        void menuTextBiggest_Click(object sender, EventArgs e)
        {
            CheckMenuItem(1.4F);
        }

        void menuTextSmallest_Click(object sender, EventArgs e)
        {
            CheckMenuItem(0.6F);
        }

        void CheckMenuItem(float newZoom)
        {
            if (this.menuTextBigger != null)
            {
                this.menuTextBiggest.Checked = false;
                this.menuTextBigger.Checked = false;
                this.menuTextNormal.Checked = false;
                this.menuTextSmall.Checked = false;
                this.menuTextSmallest.Checked = false;

                if (newZoom == 1.0F)
                {
                    this.menuTextNormal.Checked = true;
                }
                else if (newZoom == 1.2F)
                {
                    this.menuTextBigger.Checked = true;
                }
                else if (newZoom == 1.4F)
                {
                    this.menuTextBiggest.Checked = true;
                }
                else if (newZoom == 0.8F)
                {
                    this.menuTextSmall.Checked = true;
                }
                else if (newZoom == 0.6F)
                {
                    this.menuTextSmallest.Checked = true;
                }
            }

            if (this.currentZoom != newZoom)
            {
                this.currentZoom = newZoom;
                this.ZoomControls();

                this.settings.Settings["TextSize"] = newZoom.ToString();
                ThreadPool.QueueUserWorkItem(new WaitCallback(CallBackgroundSave));
            }
        }

        void CallBackgroundSave(object foo)
        {
            this.SaveSettings();
        }

        void ZoomControls()
        {
            float newSize = this.defaultFontSize * this.currentZoom;
            this.listView1.Font = new Font(this.listView1.Font.Name, newSize, this.listView1.Font.Style);
        }

        delegate void ExceptionEventHandler(Exception ex);
        void DisplayException(Exception ex)
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

        void newItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                this.add_Click(this, EventArgs.Empty);
            }
        }

        void updateTimer_Tick(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                this.updateTimer.Enabled = false;
            }

            this.UpdateStatus();
        }
        
        void timerProgress_Tick(object sender, EventArgs e)
        {
            int curValue = this.progressBar.Value;

            if (curValue > 5)
            {
                this.timerProgress.Interval = 500;
            }
            else if (curValue > 12)
            {
                this.timerProgress.Interval = 750;
            }
            else if (curValue > 20)
            {
                this.timerProgress.Interval = 1000;
            }
            else if (curValue > 25)
            {
                this.timerProgress.Interval = 2000;
            }

            curValue++;

            if (curValue < this.progressBar.Maximum)
            {
                this.progressBar.Value = curValue;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            this.UpdateStatus();
            this.updateTimer.Enabled = true;
        }

        private void listViewContextMenu_Popup(object sender, EventArgs e)
        {
            if (this.listViewContextMenu.MenuItems.Count == 0)
            {
                this.menuEdit = new MenuItem();
                this.menuEdit.Text = "&Edit";
                this.menuEdit.Click += new EventHandler(this.menuEdit_Click);
                this.listViewContextMenu.MenuItems.Add(this.menuEdit);

                this.menuMoveToList = new MenuItem();
                this.menuMoveToList.Text = "&Move to list";
                this.listViewContextMenu.MenuItems.Add(this.menuMoveToList);
            }

            this.menuEdit.Enabled = false;
            this.menuMoveToList.Enabled = false;

            if (this.listView1.SelectedIndices.Count > 0 && this.selectedStore != null && this.menuRefresh.Enabled == true)
            {
                this.menuEdit.Enabled = true;
                this.menuMoveToList.Enabled = true;
                this.menuMoveToList.MenuItems.Clear();

                foreach (StoreItem store in this.settings.LatestItems.Values)
                {
                    if (store != this.selectedStore)
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

            if (this.listView1.SelectedIndices.Count > 0 && this.selectedStore != null)
            {
                StoreItem newStore = this.settings.LatestItems[storeMenuItem.Text];
                ShoppingListViewItem moveItem = (ShoppingListViewItem)this.listView1.Items[this.listView1.SelectedIndices[0]];

                this.selectedStore.Items.Remove(moveItem.Item);
                newStore.Items.Add(moveItem.Item);

                moveItem.Item.Store = newStore.Name;

                ThreadPool.QueueUserWorkItem(new WaitCallback(this.UpdateListItem), moveItem.Item);

                this.listView1.Items.Remove(moveItem);
            }
        }

        void UpdateListItem(object oItem)
        {
            ShoppingListItem item = (ShoppingListItem)oItem;

            this.GetShoppingListService().UpdateShoppingListItem(item);
        }

        private void menuEdit_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedIndices.Count > 0 && this.selectedStore != null)
            {
                ShoppingListViewItem editItem = (ShoppingListViewItem)this.listView1.Items[this.listView1.SelectedIndices[0]];

                EditItemForm itemEditor = new EditItemForm(this.inputPanel1);
                itemEditor.Text = this.selectedStore.Name;
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

        private void switchStore_Click(object sender, EventArgs e)
        {
            SelectStore ss = new SelectStore();
            ss.ShowStores(this.settings.LatestItems);
            ss.SelectedStore = this.selectedStore;

            if (ss.ShowDialog() == DialogResult.OK)
            {
                this.selectedStore = ss.SelectedStore;
                this.storeLabel.Text = this.selectedStore.Name;
                this.ReloadItemList();
            }
        }

        private void timerRetryRefresh_Tick(object sender, EventArgs e)
        {
            this.timerRetryRefresh.Enabled = false;
            this.LoadAllItems();
        }

        private void inputPanel1_EnabledChanged(object sender, EventArgs e)
        {
            this.SizeControls();
        }

        void SizeControls()
        {
            int offset = 0;
            int pbarOffset = 0;

            if (this.inputPanel1.Enabled == true)
            {
                offset = this.inputPanel1.Bounds.Height;
            }

            if (this.menuRefresh.Enabled == false)
            {
                offset += this.progressBar.Height;
                pbarOffset = this.progressBar.Height;
            }

            this.listView1.Height = this.ClientSize.Height - this.listView1.Top - this.statusLabel.Height - offset;

            this.statusLabel.Top = this.listView1.Top + this.listView1.Height + pbarOffset;
        }

        private void newItem_GotFocus(object sender, EventArgs e)
        {
            if (this.inputPanel1.Enabled == false)
            {
                if (this.Height > this.Width)
                {
                    this.inputPanel1.Enabled = true;
                }
            }
        }

        private void newItem_LostFocus(object sender, EventArgs e)
        {
            if (this.inputPanel1.Enabled == true)
            {
                this.inputPanel1.Enabled = false;
            }
        }
    }
}