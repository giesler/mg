using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Live;

namespace SLExpress
{
    public class SessionIdProvider: ISessionIdProvider
    {
        const string WlSessionId = "wl_session_id";
        public string GetSessionId(HttpContext context)
        {
            string sessionGuid = string.Empty;
            if (context.Session[WlSessionId] != null)
            {
                sessionGuid = context.Session[WlSessionId].ToString();
            }
            else
            {
                sessionGuid = Guid.NewGuid().ToString();
                context.Session.Add(WlSessionId, sessionGuid);
            }
            return (string)sessionGuid;
        }

    }
}