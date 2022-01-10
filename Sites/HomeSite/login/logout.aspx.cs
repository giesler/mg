using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["Login"].Value = "0";
        Response.Cookies["Login"].Expires = DateTime.Now.AddYears(-5);
        Response.Cookies["Login"].Domain = "msn2.net";

        string qs = string.Empty;
        if (Request.QueryString["r"] != null)
        {
            qs = "?r=" + Request.QueryString["r"];
        }

        Response.Redirect(Request.ApplicationPath + qs);
    }
}