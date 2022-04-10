using msn2.net.HomeSeer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeServices
{
    public partial class HomeSeerStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HomeSeerIntegration hs = new HomeSeerIntegration();
            var s = hs.GetHomeSeerDeviceStatusJson();

            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(s);
            HttpContext.Current.Response.End();
        }
    }
}