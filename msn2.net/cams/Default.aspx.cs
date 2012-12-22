using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    string cam = "dw1";

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["Login"];
        if (cookie == null || cookie.Value != "1")
        {
            Response.Redirect("Login.aspx");
        }

        if (Request.QueryString["c"] != null)
        {
            cam = Request.QueryString["c"];
        }

        this.main.ImageUrl = string.Format("http://cam1.msn2.net/getimg.aspx?c={0}&id={1}", this.cam, Guid.NewGuid());

        if (cam == "1")
        {
            this.main2.ImageUrl = string.Format("http://cam2.msn2.net/getimg.aspx?c=2&id={0}", Guid.NewGuid());
            this.main2.Visible = true;
            this.rightPanel.Visible = true;

            this.leftPanel.Width = Unit.Percentage(50); 
            this.rightPanel.Width = Unit.Percentage(50);
        }

        if (Request.UserAgent.ToLower().IndexOf("mobile") > 0)
        {
            this.cam1thumb.Height = 32;
            this.cam2thumb.Height = 32;
            this.cam3thumb.Height = 32;
            this.cam4thumb.Height = 32;
            this.cam5thumb.Height = 32;
            this.cam6thumb.Height = 32;

            this.cam1thumb.ImageUrl = this.cam1thumb.ImageUrl.Replace("h=64", "h=32");
            this.cam2thumb.ImageUrl = this.cam2thumb.ImageUrl.Replace("h=64", "h=32");
            this.cam3thumb.ImageUrl = this.cam3thumb.ImageUrl.Replace("h=64", "h=32");
            this.cam4thumb.ImageUrl = this.cam4thumb.ImageUrl.Replace("h=64", "h=32");
            this.cam5thumb.ImageUrl = this.cam5thumb.ImageUrl.Replace("h=64", "h=32");
            this.cam6thumb.ImageUrl = this.cam6thumb.ImageUrl.Replace("h=64", "h=32");
        }
    }

    protected string GetCam()
    {
        return cam;
    }

    protected string GetDebugString()
    {
        return string.Format("Main: {0}, Thumb: {1}", this.GetRefreshInterval("main"), this.GetRefreshInterval("thumb"));
    }

    protected int GetRefreshInterval(string item)
    {
        int interval = 10000;

        if (item == "main")
        {
            interval = 1000;
        }

        if (Request.UserAgent.ToLower().IndexOf("mobile") > 0 && item == "thumb")
        {
            interval = 20000;
        }

        return interval;
    }

    protected int GetThumbHeight()
    {
        return Request.UserAgent.ToLower().IndexOf("mobile") > 0 ? 32 : 64;
    }

    protected string GetBasePrefix()
    {
        string basePrefix = "cam1";

        if (this.cam == "3" || this.cam == "front")
        {
            basePrefix = "cam3";
        }
        else if (this.cam == "dw1")
        {
            basePrefix = "cam5";
        }
        else if (this.cam == "side")
        {
            basePrefix = "cam5";
        }

        return basePrefix;
    }
}