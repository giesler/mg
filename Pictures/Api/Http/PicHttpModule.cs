using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;

namespace msn2.net.Pictures
{
    public class PicHttpModule : IHttpModule
    {
        private static string CONTEXTKEY = "asdfasf";

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
        }

        public void Dispose()
        {
        }

        #endregion

        private void context_BeginRequest(object sender, EventArgs e)
        {
            PictureConfig config = PictureConfig.Load();

            PicContext context = new PicContext(config);

            HttpContext.Current.Items.Add(CONTEXTKEY, context);
        }

        private void context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Trace.Write("context_AuthRequest: " + HttpContext.Current.Request.IsAuthenticated.ToString());

            if (HttpContext.Current.Request.IsAuthenticated)
            {
                string userName = HttpContext.Current.User.Identity.Name;
                HttpContext.Current.Trace.Write("User name: " + userName);
                double result = 0;
                if (Double.TryParse(userName, System.Globalization.NumberStyles.Integer, null, out result))
                {
                    int personId = (int)result;
                    PicContext.Current.SetCurrentUser(GetPersonById(personId));
                }
                else
                {
                    HttpContext.Current.Trace.Write("Trying NT login...");
                    PicContext.Current.SetCurrentUser(GetPersonByWindowsLogin(userName));
                }
            }
            
            if (ConfigurationManager.AppSettings["PictureAutoLogin"] != null && PicContext.Current.CurrentUser == null)
            {
                PicContext.Current.SetCurrentUser(GetPersonById(1));
            }
        }

        private PersonInfo GetPersonById(int personId)
        {
            HttpContext httpContext = HttpContext.Current;
            PersonInfo personInfo = null;
            string cacheKey = "PersonInfo.Person." + personId.ToString();

            if (httpContext != null)
            {
                object cacheObject = httpContext.Cache[cacheKey];
                if (cacheObject != null)
                {
                    personInfo = cacheObject as PersonInfo;
                    return personInfo;
                }
            }

            if (personInfo == null)
            {
                personInfo = PicContext.Current.UserManager.GetPerson(personId);
            }

            if (httpContext != null)
            {
                httpContext.Cache.Add(
                    cacheKey,
                    personInfo,
                    null,
                    DateTime.MaxValue,
                    TimeSpan.FromMinutes(1), System.Web.Caching.CacheItemPriority.Normal, null);
            }

            return personInfo;
        }

        private PersonInfo GetPersonByWindowsLogin(string personId)
        {
            HttpContext httpContext = HttpContext.Current;
            PersonInfo personInfo = null;
            string cacheKey = "PersonInfo.Person." + personId.ToString();

            if (httpContext != null)
            {
                object cacheObject = httpContext.Cache[cacheKey];
                if (cacheObject != null)
                {
                    personInfo = cacheObject as PersonInfo;
                }
            }

            if (personInfo == null)
            {
                personInfo = PicContext.Current.UserManager.GetPerson(personId);
            }

            if (httpContext != null && personInfo != null)
            {
                httpContext.Cache.Add(
                    cacheKey,
                    personInfo,
                    null,
                    DateTime.MaxValue,
                    TimeSpan.FromMinutes(1), System.Web.Caching.CacheItemPriority.Normal, null);
            }

            return personInfo;
        }

    }
}
