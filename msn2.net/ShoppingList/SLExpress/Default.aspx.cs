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
using SLExpress.lds;
using WindowsLive;

namespace SLExpress
{
    public partial class _Default : System.Web.UI.Page
    {
        private ListDataServiceClient client = null;
        const string LoginCookie = "webauthtoken";
        static WindowsLiveLogin wll = new WindowsLiveLogin(true);
        protected static string AppId = wll.AppId;
        protected string UserId;

        public _Default()
        {
            this.client = new ListDataServiceClient();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //this.client.GetLists(auth);
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
