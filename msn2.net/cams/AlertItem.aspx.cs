using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CamLib;
using System.IO;

public partial class AlertItem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie cookie = Request.Cookies["Login"];
        if (cookie == null || cookie.Value != "1")
        {
            Response.Redirect("Login.aspx");
        }
        if (Request.QueryString["a"] == null)
        {
            Response.Redirect("Log.aspx");
        }

        int alertId = int.Parse(Request.QueryString["a"]);
        string height = Request.QueryString["h"];

        CamAlertManager mgr = new CamAlertManager();
        Alert alert = mgr.GetAlert(alertId);

        this.name.Text = alert.Timestamp.ToString("dddd MMMM d h:mm tt").ToUpper();
        if (Request.UserAgent.ToLower().Contains("mobile"))
        {
            this.name.Text = alert.Timestamp.ToString("ddd MMM d h:mm tt").ToUpper();
        }
        this.img.ImageUrl = "GetLogImage.aspx?a=" + alert.Id.ToString();

        int nextId = mgr.GetNextAlertId(alert);
        if (nextId > 0)
        {
            this.nextLink.NavigateUrl = "AlertItem.aspx?a=" + nextId.ToString();
        }
        else
        {
            this.nextLink.Enabled = false;
            this.nextLink.CssClass = "disabledLink";
        }

        int previousId = mgr.GetPreviousAlertId(alert);
        if (previousId > 0)
        {
            this.previousLink.NavigateUrl = "AlertItem.aspx?a=" + previousId.ToString();
        }
        else
        {
            this.previousLink.Enabled = false;
            this.previousLink.CssClass = "disabledLink";
        }
    }
}