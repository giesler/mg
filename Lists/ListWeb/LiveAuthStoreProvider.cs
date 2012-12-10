using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Live;
using System.Web.Configuration;
using msn2.net.ShoppingList;

namespace SLExpress
{
    public class AuthStoreProvider : IAuthStoreProvider
    {
        #region IAuthStoreProvider Members

        public void StoreToken(HttpContext context, string userId, IDictionary<string, string> authInfo)
        {
            context.Session["wl_appInfo"] = authInfo;

            string cid = authInfo["UID"];
            string name = "New user";
            msn2.net.ShoppingList.ListAuthentication auth = new msn2.net.ShoppingList.ListAuthentication();
            Person p = auth.GetPerson(cid, name);

            ClientAuthenticationData authData = new ClientAuthenticationData();
            authData.PersonUniqueId = p.UniqueId;

            PersonDevice device = auth.AddDevice(authData, Environment.MachineName);
            authData.DeviceUniqueId = device.UniqueId;

            context.Session["l_authData"] = authData;

            context.Response.Cookies["l_authCookie_p"].Value = authData.PersonUniqueId.ToString();
            context.Response.Cookies["l_authCookie_p"].Expires = DateTime.Now.AddYears(1);
            context.Response.Cookies["l_authCookie_d"].Value = authData.DeviceUniqueId.ToString();
            context.Response.Cookies["l_authCookie_d"].Expires = DateTime.Now.AddYears(1);
        }

        #endregion
    }
}