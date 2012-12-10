using System;
using System.Web;
using System.Configuration;

namespace msn2.net.Pictures
{
	public class PicModule: IHttpModule
	{
		private static string CONTEXTKEY = "asdfasf";

		#region IHttpModule Members

		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(context_BeginRequest);
			context.AuthenticateRequest	+= new EventHandler(context_AuthenticateRequest);
        
		}

        

		public void Dispose()
		{
		}

		#endregion

		private void context_BeginRequest(object sender, EventArgs e)
		{
			PictureConfig config	= Msn2Config.Load();

			PicContext context		= new PicContext(config);
            
			HttpContext.Current.Items.Add(CONTEXTKEY, context);
		}

		private void context_AuthenticateRequest(object sender, EventArgs e)
		{
			HttpContext.Current.Trace.Write("context_AuthRequest: " + HttpContext.Current.Request.IsAuthenticated.ToString());

			if (HttpContext.Current.Request.IsAuthenticated)
			{
				string userName	= HttpContext.Current.User.Identity.Name;
				HttpContext.Current.Trace.Write("User name: " + userName);
				double result	= 0;
				if (Double.TryParse(userName, System.Globalization.NumberStyles.Integer, null, out result))
				{
					int personId = (int) result;
					PicContext.Current.SetCurrentUser(GetPersonById(personId));
				}
				else
				{
					HttpContext.Current.Trace.Write("Trying NT login...");
					PicContext.Current.SetCurrentUser(GetPersonByWindowsLogin(userName));
				}
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

	}

	/// <summary>
	/// Summary description for PicContext.
	/// </summary>
	public class PicContext
	{
		#region Declares

		private PictureManager	pictureManager;
		private CategoryManager	categoryManager;
        private PictureCache    pictureCache;
        private UserManager		userManager;
        private GroupManager groupManager;
		private PictureConfig	config;
		private PersonInfo		currentUser;
		private static string CONTEXTKEY = "asdfasf";
		private static PicContext context;

		#endregion

		#region Static Methods

		public static PicContext Load(PictureConfig config, int personId)
		{
            HttpContext httpContext = HttpContext.Current;
            PicContext picContext = null;
            string personContextKey = "PicContext.PersonId." + personId.ToString();

            if (httpContext != null)
            {
                object cachedPicContextObject = httpContext.Cache[personContextKey];
                if (cachedPicContextObject != null)
                {
                    picContext = cachedPicContextObject as PicContext;
                }
            }

            if (context == null)
            {
                context = new PicContext(config);
                context.SetCurrentUser(context.userManager.GetPerson(personId));
            }

            if (httpContext != null)
            {
                httpContext.Cache.Add(
                    personContextKey, 
                    context, null, DateTime.MinValue, 
                    TimeSpan.FromMinutes(1), 
                    System.Web.Caching.CacheItemPriority.Normal, 
                    null);
            }

            return context;
		}

		#endregion

		#region Internal constructor

		internal PicContext(PictureConfig config)
		{
			this.config		= config;

			pictureManager	= new PictureManager(config.ConnectionString);
			categoryManager	= new CategoryManager(config.ConnectionString);
            pictureCache = new PictureCache();
            userManager		= new UserManager(config.ConnectionString);
            groupManager = new GroupManager(config.ConnectionString);
			currentUser		= null;
		}

		#endregion
		
		#region Static Properties

		public static PicContext Current
		{
			get
			{
				if (HttpContext.Current	!= null)
				{
					return (PicContext) HttpContext.Current.Items[CONTEXTKEY];
				}
				else
				{
					return context;
				}
			}
		}
		
		#endregion

		#region Properties

		public PictureManager PictureManager
		{
			get
			{
				return pictureManager;
			}
		}

		public CategoryManager CategoryManager
		{
			get
			{
				return categoryManager;
			}
		}

        public PictureCache PictureCache
        {
            get
            {
                return this.pictureCache;
            }
        }

        public UserManager UserManager
		{
			get
			{
				return userManager;
			}
		}

        public GroupManager GroupManager
        {
            get
            {
                return this.groupManager;
            }
        }

		public PictureConfig Config
		{
			get
			{
				return config;
			}
		}

		public PersonInfo CurrentUser
		{
			get
			{
				return currentUser;
			}
		}

		#endregion

		#region Internal Methods

		internal void SetCurrentUser(PersonInfo info)
		{
			currentUser = info;
		}

		#endregion
	}
}
