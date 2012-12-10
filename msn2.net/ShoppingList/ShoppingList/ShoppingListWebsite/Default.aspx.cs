using System;
using System.Collections.Generic;
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
using System.Net;
using ShoppingListService;
using System.Threading;

public partial class _Default : System.Web.UI.Page 
{
    private ShoppingListService.ShoppingListServiceClient client = null;

    public _Default()
    {
        this.client = new ShoppingListService.ShoppingListServiceClient();
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.storeList.SelectedIndexChanged += new EventHandler(storeList_SelectedIndexChanged);
        this.GridView1.RowEditing += new GridViewEditEventHandler(GridView1_RowEditing);
        this.GridView1.RowUpdating += new GridViewUpdateEventHandler(GridView1_RowUpdating);
        this.GridView1.RowCancelingEdit += new GridViewCancelEditEventHandler(GridView1_RowCancelingEdit);
        this.GridView1.RowDeleting += new GridViewDeleteEventHandler(GridView1_RowDeleting);

        this.addButton.Click += new EventHandler(addButton_Click);

        if (this.IsPostBack == false)
        {
            string script = "function initDataLoad() {" + ClientScript.GetPostBackEventReference(this.updatePanel, "onInitLoad") + "; }";
            ScriptManager.RegisterClientScriptBlock(this.updatePanel, this.updatePanel.GetType(), "initLoadScript", script, true);

            string timerScript = "setTimeout('initDataLoad();', 250);";
            ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "initLoad", timerScript, true);
        }
        else
        {
            string eventArg = base.Request["__EVENTARGUMENT"];
            if (eventArg != null)
            {
                if (eventArg.Contains("onInitLoad") == true)
                {
                    this.ReloadAll();
                }
            }
        }
    }

    void addButton_Click(object sender, EventArgs e)
    {
        if (this.addItem.Text.Trim().Length > 0)
        {
            ListItem item = this.storeList.Items[this.storeList.SelectedIndex];
            string store = item.Value;

            this.client.AddShoppingListItem(store, this.addItem.Text.Trim());

            this.addItem.Text = string.Empty;
            this.addItem.Focus();

            this.ReloadAll();
        }
    }

    void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = this.GridView1.Rows[e.RowIndex];
        HiddenField idField = (HiddenField)row.FindControl("itemId");
        ShoppingListItem item = new ShoppingListItem();
        item.Id = int.Parse(idField.Value);
        this.client.DeleteShoppingListItem(item);
        this.ReloadAll();
    }

    void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.GridView1.EditIndex = -1;
        this.ReloadAll();
    }

    void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = this.GridView1.Rows[e.RowIndex];

        ListItem storeItem = this.storeList.Items[this.storeList.SelectedIndex];
        string store = storeItem.Value;

        TextBox updatedText = (TextBox)row.FindControl("itemText");
        HiddenField idField = (HiddenField)row.FindControl("itemId");

        ShoppingListItem item = new ShoppingListItem();
        item.ListItem = updatedText.Text;
        item.Store = store;
        item.Id = int.Parse(idField.Value);
        this.client.UpdateShoppingListItem(item);

        this.GridView1.EditIndex = -1;

        this.ReloadAll();
    }

    void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        this.GridView1.EditIndex = e.NewEditIndex;
        this.ReloadAll();
    }

    void storeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GridView1.DataSource = null;

        if (this.storeList.SelectedIndex >= 0)
        {
            ListItem item = this.storeList.Items[this.storeList.SelectedIndex];

            List<ShoppingListService.ShoppingListItem> items = this.client.GetShoppingListItemsForStore(
                item.Value);

            var q = from i in items
                    orderby i.ListItem
                    select i;

            this.GridView1.DataSource = q;
            this.GridView1.DataBind();
        }
    }

    protected void ReloadAll()
    {
        int selectedStoreIndex = this.storeList.SelectedIndex;

        this.storeList.Items.Clear();

        List<string> stores = this.client.GetStores();
        List<ShoppingListService.ShoppingListItem> items = this.client.GetShoppingListItems();
        var allItems = from i in items
                         group i by i.Store into g
                         orderby g.Key
                         select new { Name = g.Key, Items = g };

        foreach (string storeName in stores)
        {
            var q = from s in allItems
                             where s.Name == storeName
                             select s;

            string title = storeName;
            var storeItems = q.Count() > 0 ? q.First() : null;
            if (storeItems != null && storeItems.Items.Count() > 0)
            {
                title = title + " (" + storeItems.Items.Count() + ")";
            }
            ListItem item = new ListItem(title);
            item.Value = storeName;
            this.storeList.Items.Add(item);
        }

        if (selectedStoreIndex >= 0)
        {
            this.storeList.SelectedIndex = selectedStoreIndex;
        }
        else
        {
            this.storeList.SelectedIndex = 0;
        }

        this.storeList_SelectedIndexChanged(this, EventArgs.Empty);
    }

}
