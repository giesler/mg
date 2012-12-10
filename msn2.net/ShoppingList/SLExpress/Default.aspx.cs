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
using SLExpress.ShoppingListService;
using WindowsLive;
using msn2.net.ShoppingList;
using System.Text;

namespace SLExpress
{
    public partial class _Default : System.Web.UI.Page
    {
        const string LoginCookie = "webauthtoken";
        static WindowsLiveLogin wll = new WindowsLiveLogin(true);
        protected static string AppId = wll.AppId;
        protected string UserId;

        public _Default()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.list.SelectedIndexChanged += new EventHandler(OnListSelectedIndexChanged);
            this.addButton.Click += new EventHandler(addButton_Click);
            this.cancelButton.Click += new EventHandler(cancelButton_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["l_authData"] == null)
            {
                if (HttpContext.Current.Request.Cookies["l_authCookie_p"] != null && HttpContext.Current.Request.Cookies["l_authCookie_d"] != null)
                {
                    Guid uid = new Guid(HttpContext.Current.Request.Cookies["l_authCookie_p"].Value);
                    Guid did = new Guid(HttpContext.Current.Request.Cookies["l_authCookie_d"].Value);
                    ClientAuthenticationData authData = new ClientAuthenticationData { PersonUniqueId = uid, DeviceUniqueId = did };
                    HttpContext.Current.Session["l_authData"] = authData;
                }
            }

            if (HttpContext.Current.Session["l_authData"] == null)
            {
                Response.Redirect("signin.aspx");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    this.LoadLists();
                }

                this.OnListSelectedIndexChanged(this, null);
            }

            /* If the user token has been cached in a site cookie, attempt
               to process it and extract the user ID. */

            HttpRequest req = HttpContext.Current.Request;
            HttpCookie loginCookie = req.Cookies[LoginCookie];

            if (loginCookie != null)
            {
                string token = loginCookie.Value;

                if (!string.IsNullOrEmpty(token))
                {
                    WindowsLiveLogin.User user = wll.ProcessToken(token);

                    if (user != null)
                    {
                        UserId = user.Id;
                    }
                }
            }
        }

        private void LoadLists()
        {
            int selectedIndex = this.list.SelectedIndex;

            ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["l_authData"];
            ListDataService lds = new ListDataService();
            System.Collections.Generic.List<GetListsResult> lists = lds.GetLists(authData);
            
            this.list.Items.Clear();
            foreach (GetListsResult list in lists.OrderBy(l => l.Name))
            {
                string name = list.Name;
                if (list.ItemCount > 0)
                {
                    name += " (" + list.ItemCount.ToString() + ")";
                }
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(name, list.UniqueId.ToString());
                this.list.Items.Add(li);
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

        void OnListSelectedIndexChanged(object sender, EventArgs e)
        {
            this.itemPanel.Controls.Clear();

            if (this.list.SelectedIndex >= 0)
            {
                Guid listId = new Guid(this.list.SelectedValue);
                ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["l_authData"];
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
                }

                LinkButton add = new LinkButton { Text = "Add..." };
                add.Click += new EventHandler(OnAddItem);
                add.ID = "addLink";
                this.itemPanel.Controls.Add(add);
            }
        }

        void OnAddItem(object sender, EventArgs e)
        {
            this.main.Visible = false;
            this.addPanel.Visible = true;

            Page.ClientScript.RegisterStartupScript(typeof(_Default), "AddFocus", "if (document.all.add) document.all.add.focus();", true);
        }

        void cancelButton_Click(object sender, EventArgs e)
        {
            this.main.Visible = true;
            this.addPanel.Visible = false;
        }

        void addButton_Click(object sender, EventArgs e)
        {
            if (this.add.Text.Trim().Length > 0)
            {
                ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["l_authData"];
                Guid selectedList = new Guid(this.list.SelectedValue);

                ListDataService lds = new ListDataService();
                lds.AddListItem(authData, selectedList, this.add.Text.Trim());

                this.add.Text = string.Empty;
                this.main.Visible = true;
                this.addPanel.Visible = false;

                this.LoadLists();
            }
        }


        protected void OnCheckboxChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            ListDataService lds = new ListDataService();
            ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["l_authData"];
            Guid id = new Guid(cb.ID);
            lds.DeleteListItem(authData, id);

            this.LoadLists();
        }
        
        protected string SessionId
        {
            get
            {
                SessionIdProvider oauth = new SessionIdProvider();
                return oauth.GetSessionId(HttpContext.Current);
            }
        }
    }
}
