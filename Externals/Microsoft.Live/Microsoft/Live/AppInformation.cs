namespace Microsoft.Live
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.CompilerServices;
    using System.Net;
    //using System.Windows.Browser;

    public class AppInformation
    {
        public AppAuthentication AuthInfo { get; set; }
        public string CallbackUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientState { get; set; }
        public Uri ConsentRoot { get; set; }
        public bool DisableUXSignIn { get; set; }
        public string PolicyUrl { get; set; }
        public Collection<Offer> RequestedOffers { get; set; }
        public Uri ServiceRoot { get; set; }
        public string SessionId { get; set; }
        public string TouUrl { get; set; }

        //[CompilerGenerated]
        //private AppAuthentication <AuthInfo>k__BackingField;
        //[CompilerGenerated]
        //private string <CallbackUrl>k__BackingField;
        //[CompilerGenerated]
        //private string <ClientId>k__BackingField;
        //[CompilerGenerated]
        //private string <ClientSecret>k__BackingField;
        //[CompilerGenerated]
        //private string <ClientState>k__BackingField;
        //[CompilerGenerated]
        //private Uri <ConsentRoot>k__BackingField;
        //[CompilerGenerated]
        //private bool <DisableUXSignIn>k__BackingField;
        //[CompilerGenerated]
        //private string <PolicyUrl>k__BackingField;
        //[CompilerGenerated]
        //private Collection<Offer> <RequestedOffers>k__BackingField;
        //[CompilerGenerated]
        //private Uri <ServiceRoot>k__BackingField;
        //[CompilerGenerated]
        //private string <SessionId>k__BackingField;
        //[CompilerGenerated]
        //private string <TouUrl>k__BackingField;

        internal AppInformation()
        {
            this.InitializeData(null, null, null, null, null);
        }

        public AppInformation(IDictionary<string, string> initParams)
        {
            if (initParams == null)
            {
                throw new ArgumentNullException("initParams");
            }
            AppAuthentication authInfo = new AppAuthentication();
            string clientId = null;
            foreach (string str2 in initParams.Keys)
            {
                string grantedOffers = initParams[str2];
                if ((string.Compare(str2, "ClientID", StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(str2, "c_clientId", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    clientId = grantedOffers;
                    continue;
                }
                if ((string.Compare(str2, "ClientState", StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(str2, "c_clientState", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    this.ClientState = grantedOffers;
                    continue;
                }
                if ((string.Compare(str2, "AccessToken", StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(str2, "c_accessToken", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    authInfo.AccessToken = HttpUtility.UrlDecode(grantedOffers);
                    continue;
                }
                if (string.Compare(str2, "RefreshToken", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    authInfo.RefreshToken = grantedOffers;
                    continue;
                }
                if ((string.Compare(str2, "Expiry", StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(str2, "c_expiry", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    authInfo.Expiry = grantedOffers;
                    continue;
                }
                if ((string.Compare(str2, "UID", StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(str2, "c_uid", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    authInfo.UserId = grantedOffers;
                    continue;
                }
                if ((string.Compare(str2, "Scope", StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(str2, "c_scope", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    authInfo.GrantedOffers = WrapProtocolHelper.ParseOffers(grantedOffers);
                    authInfo.Scope = grantedOffers;
                    continue;
                }
                if (((string.Compare(str2, "Error", StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(str2, "c_error", StringComparison.OrdinalIgnoreCase) == 0)) && !string.IsNullOrEmpty(grantedOffers))
                {
                    authInfo.Error = new AuthorizationException(grantedOffers);
                }
            }
            this.InitializeData(clientId, null, null, null, authInfo);
        }

        public AppInformation(string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("clientId");
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentException("clientSecret");
            }
            Collection<Offer> requestedOffers = new Collection<Offer>();
            requestedOffers.Add(new Offer("WL_Profiles.View"));
            this.InitializeData(clientId, clientSecret, string.Empty, requestedOffers, null);
        }

        public AppInformation(string clientId, string clientSecret, Collection<Offer> requestedOffers)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("clientId");
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentException("clientSecret");
            }
            if ((requestedOffers == null) || (requestedOffers.Count == 0))
            {
                throw new ArgumentException("requestedOffers");
            }
            this.InitializeData(clientId, clientSecret, string.Empty, requestedOffers, null);
        }

        public AppInformation(string clientId, string clientSecret, AppAuthentication authInfo) : this(clientId, clientSecret, authInfo, false)
        {
        }

        public AppInformation(string clientId, string clientSecret, AppAuthentication authInfo, bool disableUXSignIn)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("clientId");
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentException("clientSecret");
            }
            if (authInfo == null)
            {
                throw new ArgumentNullException("authInfo");
            }
            this.DisableUXSignIn = disableUXSignIn;
            if (disableUXSignIn)
            {
                this.InitializeData(clientId, clientSecret, null, null, authInfo);
            }
            else
            {
                Collection<Offer> requestedOffers = new Collection<Offer>();
                requestedOffers.Add(new Offer("WL_Profiles.View"));
                this.InitializeData(clientId, clientSecret, string.Empty, requestedOffers, authInfo);
            }
        }

        private void InitializeData(string clientId, string clientSecret, string callback, Collection<Offer> requestedOffers, AppAuthentication authInfo)
        {
            this.ServiceRoot = new Uri("http://apis.live.net");
            this.ConsentRoot = new Uri("https://consent.live.com/");
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.CallbackUrl = callback;
            this.RequestedOffers = requestedOffers;
            this.AuthInfo = (authInfo != null) ? authInfo : new AppAuthentication();
            this.PolicyUrl = string.Empty;
            this.TouUrl = string.Empty;
        }

        //public AppAuthentication AuthInfo
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<AuthInfo>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    internal set
        //    {
        //        this.<AuthInfo>k__BackingField = value;
        //    }
        //}

        //public string CallbackUrl
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<CallbackUrl>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<CallbackUrl>k__BackingField = value;
        //    }
        //}

        //public string ClientId
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ClientId>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    internal set
        //    {
        //        this.<ClientId>k__BackingField = value;
        //    }
        //}

        //public string ClientSecret
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ClientSecret>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    internal set
        //    {
        //        this.<ClientSecret>k__BackingField = value;
        //    }
        //}

        //public string ClientState
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ClientState>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<ClientState>k__BackingField = value;
        //    }
        //}

        //public Uri ConsentRoot
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ConsentRoot>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<ConsentRoot>k__BackingField = value;
        //    }
        //}

        internal Uri ConsentUrl
        {
            get
            {
                if (this.ConsentRoot != null)
                {
                    return new Uri(this.ConsentRoot, "connect.aspx");
                }
                return null;
            }
        }

        //internal bool DisableUXSignIn
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<DisableUXSignIn>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<DisableUXSignIn>k__BackingField = value;
        //    }
        //}

        //public string PolicyUrl
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<PolicyUrl>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<PolicyUrl>k__BackingField = value;
        //    }
        //}

        //public Collection<Offer> RequestedOffers
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<RequestedOffers>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<RequestedOffers>k__BackingField = value;
        //    }
        //}

        //public Uri ServiceRoot
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<ServiceRoot>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<ServiceRoot>k__BackingField = value;
        //    }
        //}

        //internal string SessionId
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<SessionId>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<SessionId>k__BackingField = value;
        //    }
        //}

        //public string TouUrl
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<TouUrl>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    set
        //    {
        //        this.<TouUrl>k__BackingField = value;
        //    }
        //}
    }
}

