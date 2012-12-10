using System;
using System.Web;
using System.Configuration;

namespace msn2.net.Pictures
{

	/// <summary>
	/// Summary description for PicContext.
	/// </summary>
	public class PicContext
	{
		#region Declares

		private PictureManager	pictureManager;
		private CategoryManager	categoryManager;
        private PictureCacheInfo    pictureCache;
        private UserManager		userManager;
        private GroupManager groupManager;
		private PictureConfig	config;
		private PersonInfo		currentUser;
		private static string CONTEXTKEY = "asdfasf";
		private static PicContext context;
        private PictureDataContext picData;

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

        public static bool LoginWindowsUser(PictureConfig config)
        {
            PicContext context = new PicContext(config);
            UserManager userManager = new UserManager(context);
            PersonInfo loginInfo = null;

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            loginInfo = userManager.GetPerson(userName);

            if (loginInfo != null)
            {
                Load(config, loginInfo.Id);
            }

            return (PicContext.Current != null);
        }

        public static bool Login(PictureConfig config, string userName, string password)
        {
            string dbPassword = UserManager.GetEncryptedPassword(password);

            PicContext context = new PicContext(config);
            UserManager userManager = new UserManager(context);

            bool isValidEmail = false;
            PersonInfo loginInfo = userManager.Login(userName, dbPassword, ref isValidEmail);
            if (loginInfo != null)
            {
                Load(config, loginInfo.Id);
            }

            return (PicContext.Current != null);
        }
        
		#endregion

		#region Internal constructor

		internal PicContext(PictureConfig config)
		{
			this.config		= config;

			pictureManager	= new PictureManager(this);
			categoryManager	= new CategoryManager(this);
            pictureCache = new PictureCacheInfo(this);
            userManager		= new UserManager(this);
            groupManager = new GroupManager(this);
			currentUser		= null;
            this.picData = new PictureDataContext(config.ConnectionString);
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

        public PictureCacheInfo PictureCache
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

        internal PictureDataContext DataContext
        {
            get
            {
                return this.picData;
            }
        }

		#endregion

		#region Internal Methods

		internal void SetCurrentUser(PersonInfo info)
		{
			this.currentUser = info;
		}

		#endregion

        public void SubmitChanges()
        {
            this.DataContext.SubmitChanges();
        }

        public PicContext Clone()
        {
            PicContext pc = new PicContext(this.Config);
            pc.currentUser = this.currentUser;
            return pc;
        }
	}

}
