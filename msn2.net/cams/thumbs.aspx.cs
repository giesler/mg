using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class thumbs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["Login"];
        if (cookie == null || cookie.Value != "1")
        {
            Response.Redirect("http://login.msn2.net/?r=http://cams.msn2.net");
        }

        List<CamView> thumbViews = CamViews.GetAll();
        this.thumbsView.DataSource = thumbViews;
        this.thumbsView.DataBind();
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
            linkA.Target = "_top";

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