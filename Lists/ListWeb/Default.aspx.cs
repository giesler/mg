using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using msn2.net.ShoppingList;
using System.Text;

namespace SLExpress
{
    public partial class _Default : System.Web.UI.Page
    {
        protected string UserId;
        int editItemColumns = 30;

        public _Default()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.list.SelectedIndexChanged += new EventHandler(OnListSelectedIndexChanged);
            this.addButton.Click += new EventHandler(addButton_Click);
            this.cancelButton.Click += new EventHandler(cancelButton_Click);

            if (Request.Browser.IsMobileDevice)
            {
                this.editItemColumns = 25;

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["authData"] == null)
            {
                if (HttpContext.Current.Request.Cookies["l_authCookie_p"] != null && HttpContext.Current.Request.Cookies["l_authCookie_d"] != null)
                {
                    Guid uid = new Guid(HttpContext.Current.Request.Cookies["l_authCookie_p"].Value);
                    Guid did = new Guid(HttpContext.Current.Request.Cookies["l_authCookie_d"].Value);
                    ClientAuthenticationData authData = new ClientAuthenticationData { PersonUniqueId = uid, DeviceUniqueId = did };
                    HttpContext.Current.Session["authData"] = authData;
                }
            }

            if (HttpContext.Current.Session["authData"] != null)
            {
                if (!Page.IsPostBack)
                {
                    this.LoadLists();
                }

                this.OnListSelectedIndexChanged(this, null);
            }
            else
            {
                Response.Redirect("signin.aspx");
            }
        }

        private void LoadLists()
        {
            int selectedIndex = this.list.SelectedIndex;

            ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["authData"];
            ListDataService lds = new ListDataService();
            System.Collections.Generic.List<GetListsResult> lists = lds.GetLists(authData);
            
            this.list.Items.Clear();
            this.moveItemList.Controls.Clear();

            foreach (GetListsResult list in lists.OrderBy(l => l.Name))
            {
                string name = list.Name;
                if (list.ItemCount > 0)
                {
                    name += " (" + list.ItemCount.ToString() + ")";
                }
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(name, list.UniqueId.ToString());
                this.list.Items.Add(li);

                LinkButton button = new LinkButton { Text = list.Name, ID = list.UniqueId.ToString() };
                button.Click += new EventHandler(OnMoveItemStart);
                this.moveItemList.Controls.Add(button);

                this.moveItemList.Controls.Add(new Literal { Text = "<br />" });
            }

            if (this.list.Items.Count > 0)
            {
                this.list.SelectedIndex = selectedIndex;

                if (this.list.SelectedIndex == -1)
                {
                    this.list.SelectedIndex = 0;
                }

                this.OnListSelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        void OnMoveItemSelectList(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;

            ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["authData"];
            ListDataService lds = new ListDataService();

            // TODO: get list items, update changed item...   lds.GetListItems()

            this.LoadLists();

            this.editItems.Visible = true;
            this.moveItem.Visible = false;
            this.addMode.Enabled = true;
            this.viewMode.Enabled = true;            
        }

        void OnListSelectedIndexChanged(object sender, EventArgs e)
        {
            this.itemPanel.Controls.Clear();
            this.editItems.Controls.Clear();

            if (this.list.SelectedIndex >= 0)
            {
                Guid listId = new Guid(this.list.SelectedValue);
                ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["authData"];
                ListDataService lds = new ListDataService();
                var q = lds.GetListItems(authData, listId);
                foreach (var i in q.OrderBy(i => i.Name))
                {
                    CheckBox cb = new CheckBox { Text = i.Name };
                    cb.AutoPostBack = true;
                    cb.CheckedChanged += new EventHandler(OnCheckboxChanged);
                    cb.ID = i.UniqueId.ToString();
                    this.itemPanel.Controls.Add(cb);
                    this.itemPanel.Controls.Add(new LiteralControl("<br />"));

                    TextBox tb = new TextBox { Text = i.Name };
                    tb.ID = "TB:" + i.UniqueId.ToString();
                    tb.Columns = this.editItemColumns;
                    this.editItems.Controls.Add(tb);
                    
                    Button btn = new Button { Text = "save" };
                    btn.Click += new EventHandler(OnSaveItem);
                    this.editItems.Controls.Add(btn);

                    Button delete = new Button { Text = "delete" };
                    delete.Click += new EventHandler(OnDeleteItem);
                    this.editItems.Controls.Add(delete);

                    Button move = new Button { Text = "move" };
                    move.Click += new EventHandler(OnMoveItemStart);
                    //this.editItems.Controls.Add(move);

                    this.editItems.Controls.Add(new LiteralControl("<br />"));
                }
            }
        }

        void OnDeleteItem(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int index = btn.Parent.Controls.IndexOf(btn);
            TextBox tb = (TextBox)btn.Parent.Controls[index - 2];
            Guid itemId = new Guid(tb.ID.ToString().Substring(3));

            ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["authData"]; 
            ListDataService lds = new ListDataService();
            lds.DeleteListItem(authData, itemId);

            this.LoadLists();
        }

        void OnMoveItemStart(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int index = btn.Parent.Controls.IndexOf(btn);
            TextBox tb = (TextBox)btn.Parent.Controls[index - 3];
            Guid itemId = new Guid(tb.ID.ToString().Substring(3));

            this.editItems.Visible = false;
            this.moveItemId.Value = itemId.ToString();
            this.moveItem.Visible = true;

            this.addMode.Enabled = false;
            this.editMode.Enabled = false;
            this.viewMode.Enabled = false;
        }

        void OnSaveItem(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int index = btn.Parent.Controls.IndexOf(btn);
            TextBox tb = (TextBox)btn.Parent.Controls[index - 1];

            ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["authData"];
            Guid selectedList = new Guid(this.list.SelectedValue); 
            
            Guid itemId = new Guid(tb.ID.ToString().Substring(3));
            ListDataService lds = new ListDataService();
            var listItems = lds.GetListItems(authData, selectedList);

            var item = listItems.First(i => i.UniqueId == itemId);

            string updatedName = Request.Form[tb.ID].ToString();
            msn2.net.ShoppingList.ListItem updatedItem = new msn2.net.ShoppingList.ListItem { UniqueId = item.UniqueId, Name = updatedName.Trim() };
            lds.UpdateListItem(authData, updatedItem);

            this.LoadLists();
        }

        void cancelButton_Click(object sender, EventArgs e)
        {
            this.OnView(null, null);
        }

        void addButton_Click(object sender, EventArgs e)
        {
            if (this.add.Text.Trim().Length > 0)
            {
                ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["authData"];
                Guid selectedList = new Guid(this.list.SelectedValue);

                foreach (string item in this.add.Text.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    ListDataService lds = new ListDataService();
                    lds.AddListItem(authData, selectedList, item.Trim());
                }

                this.add.Text = string.Empty;

                this.LoadLists();
                
                this.OnView(null, null);
            }
        }


        protected void OnCheckboxChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            ListDataService lds = new ListDataService();
            ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["authData"];
            Guid id = new Guid(cb.ID);
            lds.DeleteListItem(authData, id);

            this.LoadLists();
        }

        protected void OnAdd(object sender, EventArgs e)
        {
            this.addMode.Font.Bold = true;
            this.editMode.Font.Bold = false;
            this.viewMode.Font.Bold = false;

            this.addPanel.Visible = true;
            this.editPanel.Visible = false;
            this.main.Visible = false;

            Page.ClientScript.RegisterStartupScript(typeof(_Default), "AddFocus", "if (document.all.add) document.all.add.focus();", true);
        }

        protected void OnEdit(object sender, EventArgs e)
        {
            this.addMode.Font.Bold = false;
            this.editMode.Font.Bold = true;
            this.viewMode.Font.Bold = false;
            
            this.addPanel.Visible = false;
            this.editPanel.Visible = true;
            this.main.Visible = false;
        }

        protected void OnView(object sender, EventArgs e)
        {
            this.addMode.Font.Bold = false;
            this.editMode.Font.Bold = false;
            this.viewMode.Font.Bold = true;

            this.addPanel.Visible = false;
            this.editPanel.Visible = false;
            this.main.Visible = true;
        }

        protected void OnCancelMove(object sender, EventArgs e)
        {
            this.addMode.Enabled = true;
            this.editMode.Enabled = true;
            this.viewMode.Enabled = true;

            this.editItems.Visible = true;
            this.moveItem.Visible = false;
        }
    }
}
