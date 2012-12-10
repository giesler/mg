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
using System.Net.Sockets;
using Microsoft.WindowsCE.Forms;
using WinForms = System.Windows.Forms;

namespace msn2.net.ShoppingList
{
    public partial class MainForm : Form
    {
        public static Color[] ListColors = new Color[] { SystemColors.Window, SystemColors.InactiveCaption };// Color.FromArgb(211, 255, 227) };
        public static Color[] ListTextColors = new Color[] { SystemColors.WindowText, SystemColors.WindowText };// Color.FromArgb(211, 255, 227) };

        #region Declares

        LocalSettings settings = null;
        object lockObject = new object();

        bool displayProgress = false;
        float defaultFontSize = 10;
        float currentZoom = 1.0F;
        ShoppingListService listService = null;
        bool loadAllItemsAfterStores = false;
        bool settingsLoaded = false;
        bool reloadListOnDeleteComplete = false;
        List<ShoppingListItem> pendingDeletes = new List<ShoppingListItem>();
        StoreItem selectedStore = null;
        ShoppingListItem undoItem = null;
        string addMenuSelectedStore = null;

        IContainer components = null;
        ListView listView;
        TextBox newItem;
        Button add;
        Label statusLabel;
        Button switchStore;
        Label storeLabel;
        InputPanel inputPanel1;
        ProgressBar progressBar = null;

        MainMenu mainMenu;
        MenuItem menuRefresh;
        MenuItem rightMenu;
        MenuItem menuUndo;
        MenuItem menuUpdateApp;
        MenuItem menuSettings;
        MenuItem menuExit;
        MenuItem menuTextSize;
        MenuItem menuTextBigger;
        MenuItem menuTextNormal;
        MenuItem menuTextSmall;
        MenuItem menuTextBiggest;
        MenuItem menuTextSmallest;
        ContextMenu listViewContextMenu;
        MenuItem menuMoveToList;
        MenuItem menuEdit;

        WinForms.Timer updateTimer;
        WinForms.Timer timerRetryRefresh;
        WinForms.Timer timerResizeCheck;
        WinForms.Timer timerProgress;
        WinForms.Timer timerDelete;

        #endregion

        public MainForm()
        {
            this.SuspendLayout();

            this.mainMenu = new MainMenu();
            this.Menu = this.mainMenu;

            this.menuRefresh = new MenuItem();
            this.menuRefresh.Enabled = false;
            this.menuRefresh.Text = "&Refresh";
            this.menuRefresh.Click += new EventHandler(this.menuRefresh_Click);
            this.mainMenu.MenuItems.Add(this.menuRefresh);

            this.rightMenu = new MenuItem();
            this.rightMenu.Text = "&Menu";
            this.mainMenu.MenuItems.Add(this.rightMenu);

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

            this.menuSettings = new MenuItem();
            this.menuSettings.Text = "&Settings";
            this.menuSettings.Click += new EventHandler(menuSettings_Click);
            this.rightMenu.MenuItems.Add(this.menuSettings);

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

            this.listView = new ListView();
            this.listView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.listView.FullRowSelect = true;
            this.listView.HeaderStyle = ColumnHeaderStyle.None;
            this.listView.Bounds = new Rectangle(3, 50, 235, 206);
            this.listView.TabIndex = 0;
            this.listView.View = View.Details;
            this.listView.ItemCheck += new ItemCheckEventHandler(this.listView1_ItemCheck);
            this.listView.KeyDown += new KeyEventHandler(listView1_KeyDown);
            this.listView.Parent = this;
            this.defaultFontSize = this.listView.Font.Size;

            this.newItem = new TextBox();
            this.newItem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.newItem.Bounds = new Rectangle(3, 26, 167, 21);
            this.newItem.TabIndex = 1;
            this.newItem.GotFocus += new EventHandler(this.newItem_GotFocus);
            this.newItem.KeyPress += new KeyPressEventHandler(this.newItem_KeyPress);
            this.newItem.LostFocus += new EventHandler(this.newItem_LostFocus);
            this.newItem.Parent = this;

            this.add = new Button();
            this.add.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.add.Bounds = new Rectangle(173, 26, 65, 21);
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
            this.storeLabel.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            this.storeLabel.Bounds = new Rectangle(3, 4, 168, 24);
            this.storeLabel.Text = "Loading...";
            this.storeLabel.Parent = this;

            this.inputPanel1 = new InputPanel();
            this.inputPanel1.EnabledChanged += new EventHandler(this.inputPanel1_EnabledChanged);

            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new Size(240, 268);

            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.Text = "Shopping List";

            this.progressBar = new ProgressBar();
            this.progressBar.Bounds = new Rectangle(3, 248, 235, 8);
            this.progressBar.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            this.progressBar.Maximum = 30;
            this.progressBar.Parent = this;
            this.progressBar.BringToFront();

            this.timerProgress = new WinForms.Timer();
            this.timerProgress.Tick += new EventHandler(timerProgress_Tick);
            this.timerProgress.Interval = 1000;
            
            this.ResumeLayout(false);

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadSettings), new object());

            this.LoadAllItems();

            this.UpdateStatus();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
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
            
            if (this.settings.LatestItems.ContainsKey("All") == false)
            {
                this.settings.latestItems.Add("All", new StoreItem("All"));
            }

            this.Invoke(new WaitCallback(this.DisplayLoadedSettings), new object());
        }

        void DisplayLoadedSettings(object foo)
        {
            if (this.settings.Settings.ContainsKey("LastUpdate") == false)
            {
                this.settings.Settings.Add("LastUpdate", DateTime.MinValue.ToString());
            }
            if (this.settings.Settings.ContainsKey("TextSize") == false)
            {
                this.settings.Settings.Add("TextSize", "1.3");
            }

            bool exit = false;
            if (this.settings.Settings.ContainsKey("PIN") == false)
            {
                SettingsDialog dialog = new SettingsDialog();
                if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    this.menuExit_Click(this, EventArgs.Empty);
                    exit = true;
                }
                else
                {
                    this.settings.Settings.Add("PIN", dialog.PIN);
                }
            }

            if (!exit)
            {
                float textSize = float.Parse(this.settings.Settings["TextSize"]);
                this.CheckMenuItem(textSize);

                if (this.settings.LatestItems.Count > 0)
                {
                    this.switchStore.Enabled = true;

                    this.DisplayStores(null);
                    this.LoadShoppingListItems(false);
                    this.listView.CheckBoxes = false;
                    // TODO: add flag to get latest list of stores after getting items
                }
                else
                {
                    this.loadAllItemsAfterStores = true;

                    this.statusLabel.Text = "Connecting...";

                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadStoreList), foo);
                }

                this.settingsLoaded = true;

                this.UpdateStatus();
            }
        }

        object listServiceLoadObject = new object();

        ShoppingListService GetShoppingListService()
        {
            if (this.listService == null)
            {
                while (!this.settingsLoaded)
                {
                    Thread.Sleep(100);
                }

                lock (this.listServiceLoadObject)
                {
                    if (this.listService == null)
                    {
                        this.listService = new mn2.net.ShoppingList.sls.ShoppingListService();
                        this.listService.Credentials = new NetworkCredential("mc", this.settings.Settings["PIN"], "sp");
/*
 * #if DEBUG
                        this.listService.Proxy = new WebProxy("http://192.168.1.1:8080");
#endif
   */                 }
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

                if (this.settings.LatestItems.ContainsKey("All") == false)
                {
                    this.settings.latestItems.Add("All", new StoreItem("All"));                    
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
                durationText = "under one minute ago";
            }
            else if (duration.TotalSeconds < 120)
            {
                durationText = string.Format("1 minute ago");
            }
            else if (duration.TotalMinutes < 60)
            {
                durationText = string.Format("{0:0} minutes ago", duration.TotalMinutes);
            }
            else if (duration.TotalMinutes < 120)
            {
                durationText = string.Format("1 hour ago");
            }
            else if (duration.TotalHours < 24)
            {
                durationText = string.Format("{0:0} hours ago", duration.TotalHours);
            }
            else if (duration.TotalHours < 48)
            {
                durationText = string.Format("Yesterday");
            }
            else if (duration.TotalHours < 72)
            {
                durationText = "The day before yesterday";
            }
            else if (duration.TotalHours < 96)
            {
                durationText = "The day before the day before yesterday";
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
            this.listView.CheckBoxes = false;

            this.displayProgress = true;
            this.progressBar.Visible = true;
            this.progressBar.Value = 0;
            this.SizeControls();
            this.timerProgress.Interval = 250;
            this.timerProgress.Enabled = true;

            this.UpdateStatus();

            this.TriggerResizeCheck();

            bool pendingDeletes = this.ProcessDeleteRequests();
            if (pendingDeletes == true)
            {
                this.reloadListOnDeleteComplete = true;
                this.FlushDeleteRequests();
            }
            else
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.GetAllItems), new object());
            }
        }

        void FlushDeleteRequests()
        {
            foreach (ShoppingListViewItem item in this.listView.Items)
            {
                if (item.DeleteRequested == true)
                {
                    item.DeleteRequestedTime = DateTime.MinValue;
                }
            }
            this.ProcessDeleteRequests();
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
            WebException ex = foo as WebException;
            if (ex != null)
            {
                HttpWebResponse httpResponse = ex.Response as HttpWebResponse;
                if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string msg = "The house PIN is incorrect.  Would you like to try another PIN?";
                    DialogResult result = MessageBox.Show(msg, "Access Denied", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Retry)
                    {
                        this.menuSettings_Click(this, EventArgs.Empty);
                    }
                    else
                    {
                        this.menuExit_Click(this, EventArgs.Empty);
                    }
                }
            }

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
            this.listView.CheckBoxes = enableControls;
            this.menuRefresh.Enabled = enableControls;

            if (enableControls == true)
            {
                this.HideProgress();
            }

            this.UpdateStatus();
        }

        void HideProgress()
        {
            this.displayProgress = false;
            this.progressBar.Visible = false;
            this.timerProgress.Enabled = false;
            this.SizeControls();
        }

        void ReloadItemList()
        {
            if (this.listView.Columns.Count == 0)
            {
                ColumnHeader header = new ColumnHeader();
                header.Text = "item";
                header.Width = this.listView.ClientSize.Width - 9;
                this.listView.Columns.Add(header);

                this.listViewContextMenu = new ContextMenu();
                this.listViewContextMenu.Popup += new EventHandler(this.listViewContextMenu_Popup);
                this.listView.ContextMenu = this.listViewContextMenu;
            }

            int selectedIndex = this.listView.SelectedIndices.Count > 0 ? this.listView.SelectedIndices[0] : 0;

            this.listView.Items.Clear();

            if (this.selectedStore != null)
            {
                List<ShoppingListItem> items = this.selectedStore.Items;
                if (this.selectedStore.Name == "All")
                {
                    items.Clear();
                    foreach (string storeName in this.settings.latestItems.Keys)
                    {
                        if (storeName != "All")
                        {
                            items.AddRange(this.settings.latestItems[storeName].Items);
                        }
                    }
                }
                int index = 0;
                var q = from i in items
                        orderby i.ListItem
                        select i;
                this.listView.BeginUpdate();
                foreach (ShoppingListItem item in q)
                {
                    ShoppingListViewItem listItem = new ShoppingListViewItem(item);
                    listItem.BackColor = index % 2 == 0 ? MainForm.ListColors[0] : MainForm.ListColors[1];
                    listItem.ForeColor = index % 2 == 0 ? MainForm.ListTextColors[0] : MainForm.ListTextColors[1];
                    this.listView.Items.Add(listItem);

                    index++;
                }
                this.listView.EndUpdate();
            }

            if (this.listView.Items.Count > 0)
            {
                selectedIndex = this.listView.Items.Count > selectedIndex ? selectedIndex : this.listView.Items.Count - 1;
                this.listView.Items[selectedIndex].Selected = true;
                this.listView.Items[selectedIndex].Focused = true;
            }

            this.UpdateStatus();
        }

        void UpdateRowColors()
        {
            int index = 0;

            foreach (ShoppingListViewItem item in this.listView.Items)
            {
                item.BackColor = index % 2 == 0 ? MainForm.ListColors[0] : MainForm.ListColors[1];
                item.ForeColor = index % 2 == 0 ? MainForm.ListTextColors[0] : MainForm.ListTextColors[1];
                index++;
            }
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

            if (this.listView.Columns.Count > 0)
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
            Trace.WriteLine("Resize check tick");
        }

        void SetColumnWidth()
        {
            int width = this.listView.ClientSize.Width;
            if (this.listView.Columns.Count > 0)
            {
                if (this.listView.Columns[0].Width != width)
                {
                    this.listView.Columns[0].Width = width;
                }
            }
        }

        void add_Click(object sender, EventArgs e)
        {
            if (this.selectedStore != null && this.newItem.Text.Trim().Length > 0)
            {
                StoreItem addStore = this.selectedStore;
                if (addStore.Name == "All")
                {
                    this.addMenuSelectedStore = null;

                    ContextMenu menu = new ContextMenu();
                    foreach (string storeName in this.settings.LatestItems.Keys)
                    {
                        if (storeName != "All")
                        {
                            MenuItem menuItem = new MenuItem() { Text = storeName };
                            menuItem.Click += new EventHandler(addStoreMenuItemClicked);
                            menu.MenuItems.Add(menuItem);
                        }
                    }
                    menu.Show(this.add, new Point());

                    if (this.addMenuSelectedStore == null)
                    {
                        addStore = null;
                    }
                    else
                    {
                        addStore = this.settings.LatestItems[this.addMenuSelectedStore];
                    }
                }

                if (addStore != null)
                {
                    ShoppingListItem item = new ShoppingListItem();
                    item.Store = addStore.Name;
                    item.ListItem = this.newItem.Text.Trim();
                    addStore.Items.Add(item);

                    ShoppingListViewItem sli = new ShoppingListViewItem(item);

                    int index = GetInsertIndex(item);

                    this.listView.Items.Insert(index, sli);
                    this.UpdateRowColors();

                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.AddItem), sli);

                    this.newItem.Text = string.Empty;
                    this.newItem.Focus();

                    this.TriggerResizeCheck();
                }
            }
        }

        void addStoreMenuItemClicked(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            this.addMenuSelectedStore = item.Text;
        }

        private int GetInsertIndex(ShoppingListItem item)
        {
            int index = 0;
            foreach (ShoppingListViewItem tempItem in this.listView.Items)
            {
                if (item.ListItem.CompareTo(tempItem.Text) < 0)
                {
                    break;
                }
                index++;
            }
            return index;
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
            ShoppingListViewItem item = (ShoppingListViewItem)this.listView.Items[e.Index];

            if (item.Item.Id == 0)
            {
                e.NewValue = CheckState.Unchecked;
            }
            else
            {
                if (e.CurrentValue == CheckState.Checked)
                {
                    item.DeleteRequested = false;
                    item.UpdateItem(item.Item);
                    this.menuUndo.Enabled = false;
                    this.ToggleDataChangeControls(this.timerDelete == null || this.timerDelete.Enabled == false);
                }
                else
                {
                    item.DeleteRequested = true;
                    item.DeleteRequestedTime = DateTime.Now;
                    item.UpdateItem(item.Item);
                    item.Selected = false;
                    this.switchStore.Enabled = false;

                    if (this.timerDelete == null)
                    {
                        this.timerDelete = new WinForms.Timer();
                        this.timerDelete.Interval = 350;
                        this.timerDelete.Tick += new EventHandler(timerDelete_Tick);
                    }

                    this.timerDelete.Enabled = true;
                    this.ToggleDataChangeControls(false);
                }
            }
        }

        void ToggleDataChangeControls(bool enable)
        {
            this.switchStore.Enabled = enable;
            this.menuRefresh.Enabled = enable;
            this.menuUpdateApp.Enabled = enable;
            this.menuSettings.Enabled = enable;
            this.menuExit.Enabled = enable;
        }

        void timerDelete_Tick(object sender, EventArgs e)
        {
            this.ProcessDeleteRequests();
        }

        bool ProcessDeleteRequests()
        {
            bool pending = false;
            List<ShoppingListViewItem> removeList = new List<ShoppingListViewItem>();
            foreach (ShoppingListViewItem item in this.listView.Items)
            {
                if (item.DeleteRequested)
                {
                    TimeSpan elapsedTime = DateTime.Now - item.DeleteRequestedTime;
                    if (elapsedTime.TotalSeconds >= 5)
                    {
                        removeList.Add(item);
                    }
                    else
                    {
                        item.UpdateItem(item.Item);
                        pending = true;
                    }
                }
            }

            foreach (ShoppingListViewItem item in removeList)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DeleteItem), item);
                this.pendingDeletes.Add(item.Item);

                undoItem = new ShoppingListItem();
                undoItem.Store = this.selectedStore.Name;
                undoItem.ListItem = item.Item.ListItem;

                this.menuUndo.Enabled = true;

                this.listView.Items.Remove(item);
                this.UpdateRowColors();

                StoreItem store = GetStore(item.Item.Store);
                if (store != null)
                {
                    store.Items.Remove(item.Item);
                }
            }

            if (removeList.Count > 0)
            {
                this.SizeControls();
            }

            if (pending == false && this.timerDelete != null)
            {
                this.timerDelete.Enabled = false;
                this.ToggleDataChangeControls(true);
                this.TriggerResizeCheck();
            }

            return pending;
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

            this.pendingDeletes.Remove(item.Item);

            if (this.pendingDeletes.Count == 0)
            {
                if (this.reloadListOnDeleteComplete == true)
                {
                    this.reloadListOnDeleteComplete = false;
                    this.LoadAllItems();
                }
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
                int index = this.GetInsertIndex(sli.Item);
                this.listView.Items.Insert(index, sli);
                this.UpdateRowColors();
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
            p.StartInfo = new ProcessStartInfo("http://svc.msn2.net/cab/ShoppingListMobile.cab", "");
            p.Start();

            this.Close();

            Application.Exit();
        }

        void menuSettings_Click(object sender, EventArgs e)
        {
            SettingsDialog dialog = new SettingsDialog();
            dialog.PIN = this.settings.Settings["PIN"];
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.settings.Settings["PIN"] = dialog.PIN;
                this.listService = null;
                this.menuRefresh_Click(this, EventArgs.Empty);
                ThreadPool.QueueUserWorkItem(new WaitCallback(CallBackgroundSave));
            }
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
            CheckMenuItem(1.25F);
        }

        void menuTextSmall_Click(object sender, EventArgs e)
        {
            CheckMenuItem(0.8F);
        }

        void menuTextBiggest_Click(object sender, EventArgs e)
        {
            CheckMenuItem(1.5F);
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
                else if (newZoom == 1.25F)
                {
                    this.menuTextBigger.Checked = true;
                }
                else if (newZoom == 1.5F)
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
            this.listView.Font = new Font(this.listView.Font.Name, newSize, this.listView.Font.Style);
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

            if (this.listView.SelectedIndices.Count > 0 && this.selectedStore != null && this.menuRefresh.Enabled == true)
            {
                this.menuEdit.Enabled = true;
                this.menuMoveToList.Enabled = true;
                this.menuMoveToList.MenuItems.Clear();

                ShoppingListViewItem item = (ShoppingListViewItem) this.listView.Items[this.listView.SelectedIndices[0]];

                foreach (StoreItem store in this.settings.LatestItems.Values)
                {
                    if (store != this.selectedStore && store.Name != "All" && item.Item.Store != store.Name)
                    {
                        MenuItem storeItem = new MenuItem();
                        storeItem.Text = store.Name;
                        storeItem.Click += new EventHandler(storeItem_Click);
                        this.menuMoveToList.MenuItems.Add(storeItem);
                    }
                }
            }
        }

        void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                this.FlushDeleteRequests();

                SwitchStore(e.KeyCode == Keys.Left);

                e.Handled = true;
            }
        }

        private void SwitchStore(bool left)
        {
            if (this.settings.LatestItems.Count > 0 && this.switchStore.Enabled)
            {
                int index = 0;
                for (index = 0; index < this.settings.LatestItems.Count; index++)
                {
                    if (this.selectedStore.Name == this.settings.LatestItems.Skip(index).First().Value.Name)
                    {
                        break;
                    }
                }
                index = left ? index - 1 : index + 1;
                if (index < 0)
                {
                    index = this.settings.LatestItems.Count - 1;
                }
                else if (index > this.settings.LatestItems.Count - 1)
                {
                    index = 0;
                }

                this.selectedStore = this.settings.LatestItems.Skip(index).First().Value;
                this.storeLabel.Text = this.selectedStore.Name;
                this.ReloadItemList();
                this.SetColumnWidth();
            }
        }

        void storeItem_Click(object sender, EventArgs e)
        {
            MenuItem storeMenuItem = (MenuItem)sender;

            if (this.listView.SelectedIndices.Count > 0 && this.selectedStore != null)
            {
                StoreItem newStore = this.settings.LatestItems[storeMenuItem.Text];
                ShoppingListViewItem moveItem = (ShoppingListViewItem)this.listView.Items[this.listView.SelectedIndices[0]];
                StoreItem moveFromStore = this.settings.LatestItems[moveItem.Item.Store];

                if (this.selectedStore.Name != "All")
                {
                    moveFromStore.Items.Remove(moveItem.Item);
                }
                newStore.Items.Add(moveItem.Item);

                moveItem.Item.Store = newStore.Name;

                ThreadPool.QueueUserWorkItem(new WaitCallback(this.UpdateListItem), moveItem.Item);

                if (this.selectedStore.Name != "All")
                {
                    this.listView.Items.Remove(moveItem);
                    this.UpdateRowColors();
                }
            }
        }

        void UpdateListItem(object oItem)
        {
            ShoppingListItem item = (ShoppingListItem)oItem;

            this.GetShoppingListService().UpdateShoppingListItem(item);
        }

        void menuEdit_Click(object sender, EventArgs e)
        {
            if (this.listView.SelectedIndices.Count > 0 && this.selectedStore != null)
            {
                ShoppingListViewItem editItem = (ShoppingListViewItem)this.listView.Items[this.listView.SelectedIndices[0]];

                EditItemForm itemEditor = new EditItemForm(this.inputPanel1);
                itemEditor.Text = editItem.Item.Store;
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

        void switchStore_Click(object sender, EventArgs e)
        {
            this.FlushDeleteRequests();

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

        void timerRetryRefresh_Tick(object sender, EventArgs e)
        {
            this.timerRetryRefresh.Enabled = false;
            this.LoadAllItems();
        }

        void inputPanel1_EnabledChanged(object sender, EventArgs e)
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

            if (this.displayProgress)
            {
                offset += this.progressBar.Height + 5;
                pbarOffset = this.progressBar.Height + 4;
            }

            this.listView.Height = this.ClientSize.Height - this.listView.Top - this.statusLabel.Height - offset;

            this.statusLabel.Top = this.listView.Top + this.listView.Height + pbarOffset;

            Trace.WriteLine("SizeControls: ListHeight: " + this.listView.Height.ToString());

            this.SetColumnWidth();
        }

        void newItem_GotFocus(object sender, EventArgs e)
        {
            if (this.inputPanel1.Enabled == false)
            {
                if (this.Height > this.Width)
                {
                    this.inputPanel1.Enabled = true;
                }
            }
        }

        void newItem_LostFocus(object sender, EventArgs e)
        {
            if (this.inputPanel1.Enabled == true)
            {
                this.inputPanel1.Enabled = false;
            }
        }

        Point mouseDownStart = new Point();

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.HandleMouseDown(e);
        }

        void HandleMouseDown(MouseEventArgs e)
        {
            this.mouseDownStart = new Point(e.X, e.Y);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.HandleMouseUp(e);
        }

        void HandleMouseUp(MouseEventArgs e)
        {
            int xDiff = Math.Abs(e.X - this.mouseDownStart.X);
            int yDiff = Math.Abs(e.Y - this.mouseDownStart.Y);

            if (xDiff > 30 && yDiff < 20)
            {
                this.SwitchStore(e.X > this.mouseDownStart.X);
            }
        }
    }
}