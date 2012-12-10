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

namespace ShoppingList
{
    public partial class MainForm : Form
    {
        private homenet.Lists lists = null;
        private bool storesLoaded = false;
        private LocalSettings settings = null;

        public MainForm()
        {
            InitializeComponent();

            if (File.Exists("shoppingListSettings.xml"))
            {
                this.settings = LocalSettings.ReadFromFile("shoppingListSettings.xml");
            }
            else
            {
                this.settings = new LocalSettings();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            this.settings.Save("shoppingListSettings.xml");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.lists = new ShoppingList.homenet.Lists();
            this.lists.Credentials = new System.Net.NetworkCredential("mc", "4362", "sp");
            //this.lists.Proxy = new WebProxy("http://192.168.1.1/");

            this.listView1.Enabled = false;
            this.menuRefresh.Enabled = false;

            if (this.settings.LatestStores.Count > 0)
            {
                foreach (string store in this.settings.LatestStores)
                {
                    this.store.Items.Add(store);
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

                this.store.Items.Add("Loading...");
                this.store.SelectedIndex = 0;

                this.lists.BeginGetList("Shopping List", new AsyncCallback(StoreListLoaded), new object());
            }
        }

        private void StoreListLoaded(IAsyncResult result)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new AsyncCallback(StoreListLoaded), result);
            }
            else
            {
                XmlNode list = lists.EndGetList(result);

                XmlNode fieldsNode = list.ChildNodes[0];
                XmlNode fieldNode = fieldsNode.ChildNodes[0];
                XmlNode choicesNode = fieldNode.ChildNodes[1];

                this.store.Items.Clear();
                foreach (XmlNode choiceNode in choicesNode.ChildNodes)
                {
                    this.store.Items.Add(choiceNode.InnerText);
                }
                
                this.store.Enabled = true;
                this.add.Enabled = true;
                this.newItem.Enabled = true;
                this.storesLoaded = true;

                this.store.SelectedIndex = 0;
            }
        }

        private void AddAttribute(XmlDocument doc, XmlNode node, string name, string val)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = val;
            node.Attributes.Append(att);
        }

        private void LoadStoreItems()
        {
            XmlDocument doc = new XmlDocument();

            XmlNode query = doc.CreateElement("Query");
            query.InnerXml = "<Where><Eq><FieldRef Name='From' /><Value Type='Text'>" + this.store.Text + "</Value></Eq></Where>";
            
            XmlNode viewFields = doc.CreateElement("ViewFields");
            XmlNode field1 = doc.CreateElement("FieldRef");
            AddAttribute(doc, field1, "Name", "Title");
            viewFields.AppendChild(field1);

            XmlNode queryNode = doc.CreateElement("QueryOptions");
            queryNode.InnerXml = string.Empty;

            this.menuRefresh.Enabled = false;
            this.listView1.Enabled = false;
            this.listView1.CheckBoxes = false;
            this.listView1.Items.Clear();
            this.listView1.Items.Add(new ListViewItem("Loading..."));
            this.lists.BeginGetListItems("Shopping List", "", query, viewFields, "100", queryNode, null, new AsyncCallback(ListItemsLoaded), new object());
        }

        void LoadAllItems()
        {
            XmlDocument doc = new XmlDocument();

            XmlNode query = doc.CreateElement("Query");
            query.InnerXml = string.Empty;

            XmlNode viewFields = doc.CreateElement("ViewFields");
            XmlNode field1 = doc.CreateElement("FieldRef");
            AddAttribute(doc, field1, "Name", "Title");
            viewFields.AppendChild(field1);
            XmlNode field2 = doc.CreateElement("FieldRef");
            AddAttribute(doc, field2, "Name", "From");
            viewFields.AppendChild(field2);

            XmlNode queryNode = doc.CreateElement("QueryOptions");
            queryNode.InnerXml = string.Empty;

            this.menuRefresh.Enabled = false;
            this.listView1.Enabled = false;
            this.listView1.CheckBoxes = false;
            this.listView1.Items.Clear();
            this.listView1.Items.Add(new ListViewItem("Loading..."));
            this.lists.BeginGetListItems("Shopping List", "", query, viewFields, "100", queryNode, null, new AsyncCallback(AllListItemsLoaded), new object());
        }

        private void AllListItemsLoaded(IAsyncResult result)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AsyncCallback(AllListItemsLoaded), result);
            }
            else
            {
                XmlNode results = this.lists.EndGetListItems(result);
                foreach (XmlNode dataNode in results.ChildNodes)
                {
                    if (!(dataNode is XmlWhitespace))
                    {
                        foreach (XmlNode rowNode in dataNode.ChildNodes)
                        {
                            if (!(rowNode is XmlWhitespace))
                            {
                                string store = rowNode.Attributes["ows_From"].Value;

                                if (store == this.store.Text)
                                {
                                    string title = rowNode.Attributes["ows_Title"].Value;
                                    int id = int.Parse(rowNode.Attributes["ows_ID"].Value);
                                    ListViewItem item = new ListViewItem(title);
                                    item.Tag = id;
                                    this.listView1.Items.Add(item);
                                }
                                else
                                {
                                    bool found = false;
                                    foreach (string storeItem in this.store.Items)
                                    {
                                        if (storeItem == store)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }

                                    if (found == false)
                                    {
                                        this.store.Items.Add(store);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ListItemsLoaded(IAsyncResult result)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AsyncCallback(ListItemsLoaded), result);
            }
            else
            {
                try
                {
                    this.listView1.Items.Clear();

                    XmlNode results = this.lists.EndGetListItems(result);
                    foreach (XmlNode dataNode in results.ChildNodes)
                    {
                        if (!(dataNode is XmlWhitespace))
                        {
                            foreach (XmlNode rowNode in dataNode.ChildNodes)
                            {
                                if (!(rowNode is XmlWhitespace))
                                {
                                    string title = rowNode.Attributes["ows_Title"].Value;
                                    int id = int.Parse(rowNode.Attributes["ows_ID"].Value);
                                    ListViewItem item = new ListViewItem(title);
                                    item.Tag = id;
                                    this.listView1.Items.Add(item);
                                }
                            }
                        }
                    }

                    this.listView1.Enabled = true;
                    this.listView1.CheckBoxes = true;
                    this.menuRefresh.Enabled = true;
                }
                catch (Exception ex)
                {
                    string msg = ex.ToString();
                    this.listView1.Items.Add(new ListViewItem(msg));
                }
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.listView1.Columns[0].Width = this.listView1.ClientSize.Width - 5;
        }

        private void add_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode batchNode = doc.CreateElement("Batch");

            XmlNode methodNode = doc.CreateElement("Method");
            AddAttribute(doc, methodNode, "Cmd", "New");
            AddAttribute(doc, methodNode, "ID", "1");
            batchNode.AppendChild(methodNode);

            XmlNode field1Node = doc.CreateElement("Field");
            AddAttribute(doc, field1Node, "Name", "Title");
            field1Node.InnerText = this.newItem.Text;
            methodNode.AppendChild(field1Node);

            XmlNode field2Node = doc.CreateElement("Field");
            AddAttribute(doc, field2Node, "Name", "From");
            field2Node.InnerText = this.store.Text;
            methodNode.AppendChild(field2Node);

            ListViewItem item = new ListViewItem(this.newItem.Text);
            item.Tag = 0;

            this.lists.BeginUpdateListItems("Shopping List", batchNode, new AsyncCallback(AddCompleted), item);

            this.listView1.Items.Add(item);
                        
            this.newItem.Text = string.Empty;
            this.newItem.Focus();
        }

        private void AddCompleted(IAsyncResult result)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AsyncCallback(AddCompleted), result);
            }
            else
            {
                ListViewItem item = (ListViewItem)result.AsyncState;

                XmlNode resultsNode = this.lists.EndUpdateListItems(result);
                foreach (XmlNode childNode in resultsNode)
                {
                    if (childNode.LocalName == "Result")
                    {
                        foreach (XmlNode rowNode in childNode.ChildNodes)
                        {
                            if (rowNode.LocalName == "row")
                            {
                                int id = int.Parse(rowNode.Attributes["ows_ID"].Value);
                                item.Tag = id;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void store_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.storesLoaded == true)
            {
                this.LoadStoreItems();
            }
        }

        private void menuRefresh_Click(object sender, EventArgs e)
        {
            this.LoadStoreItems();
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Checked)
            {
                e.NewValue = CheckState.Checked;
            }
            else
            {
                ListViewItem item = this.listView1.Items[e.Index];

                XmlDocument doc = new XmlDocument();
                XmlNode batchNode = doc.CreateElement("Batch");

                XmlNode methodNode = doc.CreateElement("Method");
                AddAttribute(doc, methodNode, "Cmd", "Delete");
                AddAttribute(doc, methodNode, "ID", "1");
                batchNode.AppendChild(methodNode);

                XmlNode field1Node = doc.CreateElement("Field");
                AddAttribute(doc, field1Node, "Name", "ID");
                field1Node.InnerText = item.Tag.ToString();
                methodNode.AppendChild(field1Node);

                this.lists.BeginUpdateListItems("Shopping List", batchNode, new AsyncCallback(DeleteCompleted), item);

                item.ForeColor = Color.Gray;
                item.Selected = false;
            }
        }

        void DeleteCompleted(IAsyncResult result)
        {
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(new AsyncCallback(DeleteCompleted), result);
            }
            else
            {
                ListViewItem item = (ListViewItem) result.AsyncState;
                this.listView1.Items.Remove(item);
            }
        }
    }
}