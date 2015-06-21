using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

public partial class Login : System.Web.UI.Page
{
    protected void OnLogin(object sender, EventArgs e)
    {
        if (this.username.Text.ToLower() == "home" && this.password.Text == "4362")
        {
            HttpCookie cookie = new HttpCookie("Login", "1");
            cookie.Expires = DateTime.Now.AddYears(1);
            cookie.Domain = "msn2.net";

            Response.AppendCookie(cookie);

            if (Request.QueryString["r"] != null)
            {
                Response.Redirect(Request.QueryString["r"]);
            }
            else
            {
                Response.Redirect("http://www.msn2.net/");
            }
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

                Thread.Sleep(1000 * 30);
            }
        }
    }
}