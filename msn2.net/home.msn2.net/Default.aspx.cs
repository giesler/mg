using System;
using System.Diagnostics;
using System.Web;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Debugger.IsAttached)
        {
            HttpCookie cookie = Request.Cookies["Login"];
            if (cookie == null || cookie.Value != "1")
            {
                Response.Redirect("http://login.msn2.net/?r=http://home.msn2.net/");
            }
        }
    }
}