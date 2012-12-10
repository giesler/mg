using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SLExpress
{
    public partial class signout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["l_authCookie_p"].Expires = DateTime.Now.AddDays(-100);
            Response.Cookies["l_authCookie_d"].Expires = DateTime.Now.AddDays(-100);
            Session["authData"] = null;
        }
    }
}