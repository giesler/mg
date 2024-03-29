﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    string cam = "1";
    List<CamView> thumbViews = new List<CamView>();

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!Debugger.IsAttached)
        {
            HttpCookie cookie = Request.Cookies["Login"];
            if (cookie == null || cookie.Value != "1")
            {
                Response.Redirect("/login/?r=/cams/");
            }
        }

        bool mobile = Request.UserAgent.ToLower().IndexOf("mobile") > 0;

        this.thumbViews = CamViews.GetAll();
        
        this.logLink1.Visible = true;
        this.logLink2.Visible = true;
        this.homeLink1.Visible = true;
        this.homeLink2.Visible = true;
//        this.controlLink1.Visible = true;
//        this.controlLink2.Visible = true;
        this.logLinkSeperator0.Visible = !mobile;
//        this.logLinkSeperator4.Visible = true;
        this.logLinkSeperator5.Visible = true;
        this.cam = "gdw";

        this.topLinks.Visible = !mobile;
        this.bottomLinks.Visible = mobile;

        this.thumbs.DataSource = this.thumbViews;
        this.thumbs.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["c"] != null)
        {
            cam = Request.QueryString["c"];
        }

//        this.main.ImageUrl = string.Format("http://cam1.ms2n.net:8808/getimg.aspx?c={0}&id={1}", this.cam, Guid.NewGuid());
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
            interval = 3000;
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
        Cam cam = null;

        foreach (CamView view in this.thumbViews)
        {
            cam = view.Cameras.FirstOrDefault(i => i.Id.ToLower() == this.cam);
            if (cam != null)
            {
                return cam.HostPrefix;
            }
        }

        return "cams";
    }

    protected void thumbs_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        CamView view = e.Item.DataItem as CamView;
        if (view != null)
        {
            bool isMobile = Request.UserAgent.ToLower().IndexOf("mobile") > 0;
            int thumbHeight = isMobile ? 32 : 64;

            HyperLink link = (HyperLink)e.Item.FindControl("thumbLink");
            Image image = (Image)e.Item.FindControl("thumbImage");
            
            link.NavigateUrl = string.Format("./?c={0}", view.Cameras[0].Id);
            image.ImageUrl = string.Format("../getimg.aspx?c={1}&h={2}&id=th", view.Cameras[0].HostPrefix, view.Cameras[0].Id, thumbHeight);
        }
    }
}