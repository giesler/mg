using msn2.net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeServices
{
    public partial class netwho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnifiData ud = new UnifiData();
            var c = ud.GetWhoClients();
            var s = JsonConvert.SerializeObject(c);

            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(s);
            HttpContext.Current.Response.End();
        }
    }
}