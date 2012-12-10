using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using msn2.net.ShoppingList;

namespace SLExpress
{
    public partial class signin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url.ToString().ToLower().IndexOf("devlists") > 0)
            {
                Response.Redirect("http://listgo.mobi/signin.aspx");
            }

            Trace.Write(Request.Url.ToString());

            string liveId = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(liveId))
            {
                ListDataService lds = new ListDataService();

                Person p = lds.GetPerson(liveId, Request.QueryString["name"]);

                ClientAuthenticationData authData = new ClientAuthenticationData();
                authData.PersonUniqueId = p.UniqueId;
                                
                PersonDevice device = lds.AddDevice(authData, Environment.MachineName);
                authData.DeviceUniqueId = device.UniqueId;

                HttpContext.Current.Session["authData"] = authData;

                Response.Cookies["l_authCookie_p"].Value = authData.PersonUniqueId.ToString();
                Response.Cookies["l_authCookie_p"].Expires = DateTime.Now.AddYears(1);
                Response.Cookies["l_authCookie_d"].Value = authData.DeviceUniqueId.ToString();
                Response.Cookies["l_authCookie_d"].Expires = DateTime.Now.AddYears(1);

                Response.Redirect("default.aspx");
            }
        }        
    }
}