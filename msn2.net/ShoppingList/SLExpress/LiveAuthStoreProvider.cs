using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Live;
using System.Web.Configuration;

namespace SLExpress
{
    public class AuthStoreProvider : IAuthStoreProvider
    {
        #region IAuthStoreProvider Members

        public void StoreToken(HttpContext context, string userId, IDictionary<string, string> authInfo)
        {
            context.Session["wl_appInfo"] = authInfo;
        }

        #endregion
    }
}