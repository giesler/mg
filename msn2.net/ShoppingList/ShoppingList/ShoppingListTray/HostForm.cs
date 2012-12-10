using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using msn2.net.ShoppingList.sls;
using msn2.net.ShoppingList.Properties;

namespace ShoppingListTray
{
    public partial class HostForm : Form
    {
        private List<ShoppingListItem> listItems = null;
        private List<string> stores = null;
        ShoppingListServiceClient client = new ShoppingListServiceClient();
        DateTime lastUpdate = DateTime.MinValue;

        public HostForm()
        {
            InitializeComponent();

            this.refreshTimer.Interval = 2000;
            this.refreshTimer.Enabled = true;

            this.notifyIcon1.ContextMenuStrip = this.trayMenu;
        }

        private void LoadData(object foo)
        {
            ShoppingListServiceClient listClient = new ShoppingListServiceClient();

            this.stores = listClient.GetStores();
            this.listItems = listClient.GetShoppingListItems();
            this.lastUpdate = DateTime.Now;

            this.BeginInvoke(new MethodInvoker(this.DataLoadCompleted));
        }

        private void DataLoadCompleted()
        {
            int minutes = new Random().Next(15, 45);
            this.refreshTimer.Interval = minutes * 60 * 1000;
            this.refreshTimer.Enabled = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Visible = false;
            this.Left = -10000;
            this.Top = -10000;
        }

        private void trayMenu_Opening(object sender, CancelEventArgs e)
        {
            this.trayMenu.Items.Clear();

            if (this.stores != null && this.listItems != null)
            {
                foreach (string store in this.stores)
                {
                    var q = from i in this.listItems
                            where i.Store == store
                            orderby i.ListItem
                            select i;
                    int itemCount = q.Count<ShoppingListItem>();
                    string storeLabel = itemCount > 0 ?
                        store + " (" + itemCount + ")" :
                        store;
                                        
                    ToolStripMenuItem storeMenuItem = new ToolStripMenuItem(storeLabel);
                    this.trayMenu.Items.Add(storeMenuItem);

                    foreach (ShoppingListItem item in q)
                    {
                        ToolStripMenuItem itemMenu = new ToolStripMenuItem(item.ListItem);
                        itemMenu.Tag = item;
                        itemMenu.Font = new Font(itemMenu.Font, FontStyle.Regular);
                        storeMenuItem.DropDownItems.Add(itemMenu);

                        ToolStripMenuItem editItem = new ToolStripMenuItem("Edit");
                        editItem.Click += new EventHandler(editItem_Click);
                        editItem.Image = Resources.edit;
                        editItem.Tag = item;
                        itemMenu.DropDownItems.Add(editItem);

                        ToolStripMenuItem moveItem = new ToolStripMenuItem("Move to");
                        moveItem.Image = Resources.MoveToFolderHS;
                        itemMenu.DropDownItems.Add(moveItem);

                        foreach (string storeItem in this.stores)
                        {
                            if (item.Store != storeItem)
                            {
                                ToolStripMenuItem moveToStoreItem = new ToolStripMenuItem(storeItem);
                                moveToStoreItem.Tag = item;
                                moveToStoreItem.Click += new EventHandler(moveToStoreItem_Click);
                                moveItem.DropDownItems.Add(moveToStoreItem);
                            }
                        }
                        
                        ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete");
                        deleteItem.Click += new EventHandler(deleteItem_Click);
                        deleteItem.Image = Resources.delete;
                        deleteItem.Tag = item;
                        itemMenu.DropDownItems.Add(deleteItem);
                    }

                    if (itemCount > 0)
                    {
                        storeMenuItem.DropDownItems.Add(new ToolStripSeparator());
                    }

                    ToolStripMenuItem addToStoreItem = new ToolStripMenuItem("Add..");
                    addToStoreItem.Click += new EventHandler(addToStoreItem_Click);
                    addToStoreItem.Font = new Font(addToStoreItem.Font, FontStyle.Regular);
                    addToStoreItem.Tag = store;
                    addToStoreItem.Image = Resources.NewDocumentHS;
                    storeMenuItem.DropDownItems.Add(addToStoreItem);

                    if (itemCount > 0)
                    {
                        storeMenuItem.Font = new Font(storeMenuItem.Font, FontStyle.Bold);
                    }
                }

                this.trayMenu.Items.Add(new ToolStripSeparator());

                ToolStripMenuItem refreshItem = new ToolStripMenuItem();
                TimeSpan duration = DateTime.Now - this.lastUpdate;
                refreshItem.Text = string.Format("Refresh (Last update: {0})", GetDurationString(duration));
                refreshItem.Click += new EventHandler(refreshItem_Click);
                refreshItem.Image = Resources.refresh;
                this.trayMenu.Items.Add(refreshItem);
            }
            else
            {
                ToolStripMenuItem loading = new ToolStripMenuItem("Loading...");
                loading.Enabled = false;
                this.trayMenu.Items.Add(loading);
            }

            this.trayMenu.Items.Add(new ToolStripSeparator());
            
            ToolStripMenuItem exit = new ToolStripMenuItem("Exit");
            exit.Click += new EventHandler(exit_Click);
            this.trayMenu.Items.Add(exit);
        }

        void refreshItem_Click(object sender, EventArgs e)
        {
            this.refreshTimer_Tick(this, EventArgs.Empty);
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

        void addToStoreItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;

            ItemEditor editor = new ItemEditor(this.stores);
            editor.UpdateItem += new EventHandler(editor_UpdateItem);
            SetEditorPosition(editor);
            editor.Store = item.Tag.ToString();
            editor.Show();
        }

        void moveToStoreItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem storeItem = (ToolStripMenuItem)sender;
            ShoppingListItem listItem = (ShoppingListItem)storeItem.Tag;

            listItem.Store = storeItem.Text;

            ShoppingListServiceClient client = new ShoppingListServiceClient();
            client.BeginUpdateShoppingListItem(listItem, new AsyncCallback(DefaultCallback), null);
        }

        void DefaultCallback(IAsyncResult result)
        {
        }

        void deleteItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem storeItem = (ToolStripMenuItem)sender;
            ShoppingListItem listItem = (ShoppingListItem)storeItem.Tag;

            this.listItems.Remove(listItem);

            ShoppingListServiceClient client = new ShoppingListServiceClient();
            client.BeginDeleteShoppingListItem(listItem, new AsyncCallback(DefaultCallback), null);
        }

        void editItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            ShoppingListItem listItem = (ShoppingListItem)item.Tag;

            ItemEditor editor = new ItemEditor(this.stores);
            editor.LoadItem(listItem);
            editor.UpdateItem += new EventHandler(editor_UpdateItem);
            SetEditorPosition(editor);
            editor.Show();
        }

        void exit_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            this.Close();
        }

        void SetEditorPosition(ItemEditor editor)
        {
            Point mousePoint = new Point(MousePosition.X, MousePosition.Y);
            if (mousePoint.X + editor.Width + 20 > Screen.PrimaryScreen.WorkingArea.Width)
            {
                mousePoint.X = Screen.PrimaryScreen.WorkingArea.Width - 20 - editor.Width;
            }
            if (mousePoint.Y + editor.Height + 20 > Screen.PrimaryScreen.WorkingArea.Height)
            {
                mousePoint.Y = Screen.PrimaryScreen.WorkingArea.Height - 20 - editor.Height;
            }
                        
            editor.Location = mousePoint;
        }

        void editor_UpdateItem(object sender, EventArgs e)
        {
            ItemEditor editor = (ItemEditor)sender;

            if (editor.ListItem == null)
            {
                client.BeginAddShoppingListItem(editor.Store, editor.ItemText, AddItemCompleted, null);
            }
            else
            {
                editor.ListItem.Store = editor.Store;
                editor.ListItem.ListItem = editor.ItemText;
                client.BeginUpdateShoppingListItem(editor.ListItem, DefaultCallback, null);
            }
        }

        void AddItemCompleted(IAsyncResult result)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new AsyncCallback(AddItemCompleted), result);
            }
            else
            {
                ShoppingListItem item = client.EndAddShoppingListItem(result);
                this.listItems.Add(item);                
            }
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            this.refreshTimer.Enabled = false;

            ThreadPool.QueueUserWorkItem(LoadData, new object());
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.stores != null)
                {
                    ItemEditor editor = new ItemEditor(this.stores);
                    editor.UpdateItem += new EventHandler(editor_UpdateItem);
                    SetEditorPosition(editor);
                    editor.Show();
                }
            }
        }

    }
}
