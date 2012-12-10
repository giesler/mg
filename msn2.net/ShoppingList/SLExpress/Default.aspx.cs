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
                    ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["l_authData"];
                    ListDataService lds = new ListDataService();
                    System.Collections.Generic.List<GetListsResult> lists = lds.GetLists(authData);

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
                        this.list.SelectedIndex = 0;
                        this.OnListSelectedIndexChanged(this, null);
                    }
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
        }

        void OnListSelectedIndexChanged(object sender, EventArgs e)
        {
            this.items.DataSource = null;

            if (this.list.SelectedIndex >= 0)
            {
                Guid listId = new Guid(this.list.SelectedValue);
                ClientAuthenticationData authData = (ClientAuthenticationData)HttpContext.Current.Session["l_authData"]; 
                ListDataService lds = new ListDataService();
                var q = lds.GetListItems(authData, listId);
                this.items.DataSource = q.OrderBy(i => i.Name);
                
                this.items.DataBind();
            }
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
