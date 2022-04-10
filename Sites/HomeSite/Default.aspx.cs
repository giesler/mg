using msn2.net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                Response.Redirect("/login/?r=https://home.giesler.org/");
            }
        }

        bool mobile = Request.UserAgent.ToLower().IndexOf("mobile") > 0;

        if (mobile)
        {
            this.cam1img.Height = 115;
            this.cam2img.Height = 115;
            this.cam3img.Visible = false;
        }
        else
        {
            this.cam1img.Height = 128;
            this.cam2img.Height = 128;
            this.cam3img.Height = 128;
        }
    }
}