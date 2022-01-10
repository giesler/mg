using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Net;

public partial class Login : System.Web.UI.Page
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        IPAddress[] homeIps = null;

        if (Cache["HomeIps"] != null)
        {
            homeIps = (IPAddress[])Cache["HomeIps"];
        }
        else
        {
            try
            {
                homeIps = Dns.GetHostAddresses("ddns.ms2n.net");
            }
            catch (Exception ex)
            {
                Trace.Write(ex.ToString());
            }
        }

        if (homeIps != null)
        {
            foreach (IPAddress address in homeIps)
            {
                if (string.Equals(address.ToString(), Request.UserHostAddress, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (Request.QueryString["r"] != null)
                    {
                        LoginWithExpiration(DateTime.Now.AddDays(1));
                    }
                    else
                    {
                        this.username.Text = "home";
                    }
                }
            }
        }
    }

    protected void OnLogin(object sender, EventArgs e)
    {
        if (this.username.Text.ToLower() == "home" && this.password.Text == "4362")
        {
            LoginWithExpiration(DateTime.Now.AddYears(1));
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

    void LoginWithExpiration(DateTime expire)
    {
        HttpCookie cookie = new HttpCookie("Login", "1");
        cookie.Expires = expire;
   //     cookie.Domain = "ms2n.net";

        Response.AppendCookie(cookie);

        if (Request.QueryString["r"] != null)
        {
            Response.Redirect(Request.QueryString["r"]);
        }
        else
        {
            Response.Redirect("/");
        }
    }
}