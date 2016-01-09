using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    string cam = "1";
    bool loggedIn = false;
    List<CamView> thumbViews = new List<CamView>();

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        CamView coopView = CamViews.GetCoopView();
        //CamView coopYardView = CamViews.GetCoopSideView();
        CamView drivewayView = CamViews.GetDrivewayView();
        CamView frontView = CamViews.GetFrontView();
        CamView sideView = CamViews.GetSideView();

        HttpCookie cookie = Request.Cookies["Login"];
        if (cookie != null && cookie.Value == "1")
        {
            loggedIn = true;
        }

        bool mobile = Request.UserAgent.ToLower().IndexOf("mobile") > 0;

        if (!loggedIn)
        {
            this.thumbViews.Add(coopView);
            //this.thumbViews.Add(coopYardView);
            this.cam = "1";
        }
        else
        {
            this.thumbViews.Add(drivewayView);
            this.thumbViews.Add(frontView);
            this.thumbViews.Add(sideView);
            //this.thumbViews.Add(coopYardView);
            this.thumbViews.Add(coopView);

            this.signInOutLink1.Text = "SIGN OUT";
            this.signInOutLink2.Text = this.signInOutLink1.Text;
            this.logLink1.Visible = true;
            this.logLink2.Visible = true;
            this.controlLink1.Visible = true;
            this.controlLink2.Visible = true;
            this.logLinkSeperator1.Visible = true;
            this.logLinkSeperator2.Visible = true;
            this.logLinkSeperator3.Visible = true;
            this.logLinkSeperator4.Visible = true;
            this.cam = "dw1";

            this.topLinks.Visible = !mobile;
            this.bottomLinks.Visible = mobile;
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

        this.main.ImageUrl = string.Format("http://cam1.msn2.net:8808/getimg.aspx?c={0}&id={1}", this.cam, Guid.NewGuid());

        if (cam == "1")
        {
            this.main2.ImageUrl = string.Format("http://cam2.msn2.net:8808/getimg.aspx?c=2&id={0}", Guid.NewGuid());
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
            imageA.ImageUrl = string.Format("http://{0}.msn2.net:8808/getimg.aspx?c={1}&h={2}&id=th", view.Cameras[0].HostPrefix, view.Cameras[0].Id, thumbHeight);

            if (view.Cameras.Count > 1)
            {
                imageB.ImageUrl = string.Format("http://{0}.msn2.net:8808/getimg.aspx?c={1}&h={2}&id=th", view.Cameras[1].HostPrefix, view.Cameras[1].Id, thumbHeight);
            }
            else
            {
                imageB.Visible = false;
            }
        }
    }
}