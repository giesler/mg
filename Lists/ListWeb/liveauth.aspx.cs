using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WindowsLive;

namespace SLExpress
{
    public partial class liveauth : System.Web.UI.Page
    {
        const string LoginPage = "default.aspx";
        const string LogoutPage = LoginPage;
        const string LoginCookie = "webauthtoken";
        static DateTime ExpireCookie = DateTime.Now.AddYears(-10);
        static DateTime PersistCookie = DateTime.Now.AddYears(10);

        // Initialize the WindowsLiveLogin module.
        static WindowsLiveLogin wll = new WindowsLiveLogin(true);

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpRequest req = HttpContext.Current.Request;
            HttpResponse res = HttpContext.Current.Response;

            // Extract the 'action' parameter from the request, if any.
            string action = req["action"];

            /*
              If action is 'logout', clear the login cookie and redirect
              to the logout page.

              If action is 'clearcookie', clear the login cookie and
              return a GIF as response to signify success.

              By default, try to process a login. If login was
              successful, cache the user token in a cookie and redirect
              to the site's main page.  If login failed, clear the cookie
              and redirect to the main page.
            */

            if (action == "logout")
            {
                HttpCookie loginCookie = new HttpCookie(LoginCookie);
                loginCookie.Expires = ExpireCookie;
                res.Cookies.Add(loginCookie);

                HttpCookie pCookie = new HttpCookie("l_authCookie_p");
                pCookie.Expires = ExpireCookie;
                res.Cookies.Add(pCookie);

                HttpCookie dCookie = new HttpCookie("l_authCookie_d");
                dCookie.Expires = ExpireCookie;
                res.Cookies.Add(dCookie);

                HttpCookie idCookie = new HttpCookie("liveid");
                idCookie.Expires = ExpireCookie;
                res.Cookies.Add(idCookie);

                req.RequestContext.HttpContext.Session.Abandon();

                res.Redirect(LogoutPage);
                res.End();
            }
            else if (action == "clearcookie")
            {
                HttpCookie loginCookie = new HttpCookie(LoginCookie);
                loginCookie.Expires = ExpireCookie;
                res.Cookies.Add(loginCookie);

                string type;
                byte[] content;
                wll.GetClearCookieResponse(out type, out content);
                res.ContentType = type;
                res.OutputStream.Write(content, 0, content.Length);

                res.End();
            }
            else
            {
                WindowsLiveLogin.User user = wll.ProcessLogin(req.QueryString);

                HttpCookie loginCookie = new HttpCookie(LoginCookie);
                if (user != null)
                {
                    loginCookie.Value = user.Token;

                    //if (user.UsePersistentCookie)
                    {
                        loginCookie.Expires = PersistCookie;
                    }
                }
                else
                {
                    loginCookie.Expires = ExpireCookie;
                }

                res.Cookies.Add(loginCookie);
                res.Redirect(LoginPage);
                res.End();
            }
        }
    }
}