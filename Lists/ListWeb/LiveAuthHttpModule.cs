using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace SLExpress
{
    public class LiveAuthHttpModule: IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(OnEndRequest);
        }

        void OnEndRequest(object sender, EventArgs e)
        {
            HttpCookie wlid = HttpContext.Current.Response.Cookies["wl_cid"];
            if (wlid != null && !string.IsNullOrEmpty(wlid.Value))
            {
                Trace.WriteLine(wlid.Value);
                HttpCookie localWlId = new HttpCookie("liveid", wlid.Value);
                localWlId.Expires = DateTime.Now.AddYears(10);
                HttpContext.Current.Response.Cookies.Add(localWlId);                
            }
        }

        #endregion



    }
}