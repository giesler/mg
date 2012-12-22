using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["Login"] != null)
        {
            Response.Cookies["Login"].Expires = DateTime.Now.AddYears(-5);
        }
    }

    protected void OnLogin(object sender, EventArgs e)
    {
        if (this.username.Text.ToLower() == "home" && this.password.Text == "4362")
        {
            HttpCookie cookie = new HttpCookie("Login", "1");
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.AppendCookie(cookie);

            Response.Redirect(Request.ApplicationPath);
        }
        else
        {
            if (Session["att"] != null)
            {
                int val = int.Parse(Session["att"].ToString());
                val = val * 2;
                Session["att"] = val;

                Thread.Sleep(val * 1000);
            }
            else
            {
                Session.Add("att", 1);
            }
        }
    }
}