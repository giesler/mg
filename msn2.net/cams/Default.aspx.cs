﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    string cam = "1";
    List<CamView> thumbViews = new List<CamView>();
    bool loggedIn = false;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        Cam coopBottom = new Cam("1") { HostPrefix = "cam1", Orientation = Orientation.Vertical };
        Cam coopTop = new Cam("2") { HostPrefix = "cam2", Orientation = Orientation.Vertical };
        Cam coopSide = new Cam("3") { HostPrefix = "cam3" };
        Cam driveway = new Cam("dw1") { HostPrefix = "cam4" };
        Cam front = new Cam("front") { HostPrefix = "cam5" };
        Cam side = new Cam("side") { HostPrefix = "cam6" };

        CamView coopView = new CamView
        {
            MaxHeight = 64,
            Name = "Coop",
            Orientation = Orientation.Vertical,
            RefreshInterval = TimeSpan.FromSeconds(20)
        };
        coopView.Cameras.Add(coopBottom);
        coopView.Cameras.Add(coopTop);

        CamView coopYardView = new CamView
        {
            MaxHeight = 64,
            Name = "Outside Coop",
            Orientation = Orientation.Horizontal,
            RefreshInterval = TimeSpan.FromSeconds(20),
        };
        coopYardView.Cameras.Add(coopSide);

        CamView drivewayView = new CamView
        {
            MaxHeight = 64,
            Name = "Driveway",
            Orientation = Orientation.Horizontal,
            RefreshInterval = TimeSpan.FromSeconds(20)
        };
        drivewayView.Cameras.Add(driveway);

        CamView frontView = new CamView
        {
            MaxHeight = 64,
            Name = "Front",
            Orientation = Orientation.Horizontal,
            RefreshInterval = TimeSpan.FromSeconds(20)
        };
        frontView.Cameras.Add(front);

        CamView sideView = new CamView
        {
            MaxHeight = 64,
            Name = "Side",
            Orientation = Orientation.Horizontal,
            RefreshInterval = TimeSpan.FromSeconds(20)
        };
        sideView.Cameras.Add(side);


        HttpCookie cookie = Request.Cookies["Login"];
        if (cookie != null && cookie.Value == "1")
        {
            loggedIn = true;
        }

        if (!loggedIn)
        {
            this.thumbViews.Add(coopView);
            this.thumbViews.Add(coopYardView);
            this.cam = "1";
        }
        else
        {
            this.thumbViews.Add(drivewayView);
            this.thumbViews.Add(frontView);
            this.thumbViews.Add(sideView);
            this.thumbViews.Add(coopYardView);
            this.thumbViews.Add(coopView);

            this.signInOutLink.Text = "SIGN OUT";
            this.logLink.Visible = true;
            this.logLinkSeperator.Visible = true;
            this.cam = "dw1";
        }

        this.thumbs.DataSource = this.thumbViews;
        this.thumbs.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
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

            HyperLink linkA = (HyperLink)e.Item.FindControl("thumbALink");
            Image imageA = (Image)e.Item.FindControl("thumbAImage");
            Image imageB = (Image)e.Item.FindControl("thumbBImage");

            linkA.NavigateUrl = string.Format("./?c={0}", view.Cameras[0].Id);
            imageA.ImageUrl = string.Format("http://{0}.msn2.net/getimg.aspx?c={1}&h={2}&id=th", view.Cameras[0].HostPrefix, view.Cameras[0].Id, thumbHeight);

            if (view.Cameras.Count > 1)
            {
                imageB.ImageUrl = string.Format("http://{0}.msn2.net/getimg.aspx?c={1}&h={2}&id=th", view.Cameras[1].HostPrefix, view.Cameras[1].Id, thumbHeight);
            }
            else
            {
                imageB.Visible = false;
            }
        }
    }
}