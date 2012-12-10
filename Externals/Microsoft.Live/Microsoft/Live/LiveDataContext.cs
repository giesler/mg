namespace Microsoft.Live
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Browser;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.Phone.Controls;
    using System.Windows.Controls;

    public class LiveDataContext : DataServiceContext
    {
        private DataServiceQuery<Album> _Albums;
        private DataServiceQuery<CalendarEvent> _CalendarEvents;
        private DataServiceQuery<Calendar> _Calendars;
        private DataServiceQuery<ContactCategory> _Categories;
        private DataServiceQuery<Activity> _ContactActivities;
        private DataServiceQuery<Contact> _Contacts;
        private DataServiceQuery<DocumentFolder> _DocumentFolders;
        private DataServiceQuery<Activity> _MyActivities;
        private DataServiceQuery<Profile> _Profiles;

        public SignedInUser SignedInUser { get; set; }

        //[CompilerGenerated]
        //private Microsoft.Live.SignedInUser <SignedInUser>k__BackingField;
        private AppInformation appInfo;
        private const string ApplicationIdHeader = "X-HTTP-Live-ApplicationId";
        private object asyncState;
        private const string AuthorizationHeader = "Authorization";
        private Dictionary<string, Uri> baseUris;
        private int connecting;
        private bool isFullTrustAuth;
        private bool isSignedIn;
        private const string LiveFXAuthorizationHeader = "X-HTTP-Live-Authorization";
        private const string LiveLibraryHeader = "X-HTTP-Live-Library";
        private Dictionary<WebRequestData, object> outstandingWebRequests;
        private static AsyncCallback readServiceDocumentCompleted = new AsyncCallback(LiveDataContext.ReadServiceDocumentCompleted);
        private Uri serviceRoot;
        private const string SignedInUserElement = "SignedInUser";
        private SynchronizationContext uiSyncContext;

        public event SignInCompletedEventHandler SignInCompleted;

        public LiveDataContext() : base(new Uri("http://apis.live.net"))
        {
            this.baseUris = new Dictionary<string, Uri>();
            this.outstandingWebRequests = new Dictionary<WebRequestData, object>();
            this.OnContextCreated();
        }

        public LiveDataContext(Uri serviceRoot) : base(serviceRoot)
        {
            this.baseUris = new Dictionary<string, Uri>();
            this.outstandingWebRequests = new Dictionary<WebRequestData, object>();
            this.OnContextCreated();
        }

        private void CheckSignInStatus()
        {
            if (Interlocked.CompareExchange(ref this.connecting, 1, 0) == 1)
            {
                throw new InvalidOperationException(StringResource.SignInInProgress);
            }
            if (this.isSignedIn)
            {
                throw new InvalidOperationException(StringResource.UserAlreadySignedIn);
            }
        }

        private static AuthorizationException CreateAuthorizationException(string error, Exception e)
        {
            if ((e != null) && (e is AuthorizationException))
            {
                return (e as AuthorizationException);
            }
            return new AuthorizationException(error, e);
        }

        public LiveDataServiceCollection<Album> GetAlbumsCollection()
        {
            return new LiveDataServiceCollection<Album>(this, this.AlbumsQuery);
        }

        private void GetBaseUrisFromServiceDocumentAsync(Uri serviceDocUri, bool isRootDoc, Action<Exception> callback)
        {
            if (string.Compare(serviceDocUri.AbsolutePath, "/") == 0)
            {
                serviceDocUri = new Uri(serviceDocUri.ToString() + "v4.0");
            }
            Uri requestUri = new Uri(serviceDocUri.ToString() + "?$expand=Contacts,Profiles,Activities,Documents,Photos,Calendar,Sync");
            WebRequest webRequest = WebRequest.Create(requestUri);
            webRequest.Method = "GET";
            WebRequestData key = new WebRequestData(webRequest, this, isRootDoc, callback);
            WebHeaderCollection headers = new WebHeaderCollection();
            headers["X-HTTP-Live-Authorization"] = this.DelAuthToken;
            headers["X-HTTP-Live-ApplicationId"] = this.appInfo.ClientId;
            webRequest.Headers = headers;
            SetHeaderForAnalytics(webRequest.Headers);
            lock (this.outstandingWebRequests)
            {
                this.outstandingWebRequests.Add(key, null);
            }
            webRequest.BeginGetResponse(readServiceDocumentCompleted, key);
        }

        public LiveDataServiceCollection<CalendarEvent> GetCalendarEventsCollection()
        {
            return new LiveDataServiceCollection<CalendarEvent>(this, this.CalendarEventsQuery);
        }

        public LiveDataServiceCollection<Calendar> GetCalendarsCollection()
        {
            return new LiveDataServiceCollection<Calendar>(this, this.CalendarsQuery);
        }

        public LiveDataServiceCollection<Activity> GetContactActivitiesCollection()
        {
            return new LiveDataServiceCollection<Activity>(this, this.ContactsActivitiesQuery);
        }

        public LiveDataServiceCollection<ContactCategory> GetContactCategoriesCollection()
        {
            return new LiveDataServiceCollection<ContactCategory>(this, this.ContactCategoriesQuery);
        }

        public LiveDataServiceCollection<Contact> GetContactsCollection()
        {
            return new LiveDataServiceCollection<Contact>(this, this.ContactsQuery);
        }

        public LiveDataServiceCollection<DocumentFolder> GetDocumentFoldersCollection()
        {
            return new LiveDataServiceCollection<DocumentFolder>(this, this.DocumentFoldersQuery);
        }

        public LiveDataServiceCollection<Activity> GetMyActivitiesCollection()
        {
            return new LiveDataServiceCollection<Activity>(this, this.MyActivitiesQuery);
        }

        public LiveDataServiceCollection<Profile> GetProfilesCollection()
        {
            return new LiveDataServiceCollection<Profile>(this, this.ProfilesQuery);
        }

        private void HandleCompletion(bool success)
        {
            SendOrPostCallback d = null;
            WaitCallback callBack = null;
            if (this.SignInCompleted != null)
            {
                if (this.uiSyncContext != null)
                {
                    if (d == null)
                    {
                        d = delegate {
                            this.SignInCompleted(this, new SignInCompletedEventArgs(success, this.appInfo.AuthInfo, this.asyncState));
                        };
                    }
                    this.uiSyncContext.Post(d, null);
                }
                else
                {
                    if (callBack == null)
                    {
                        callBack = delegate {
                            this.SignInCompleted(this, new SignInCompletedEventArgs(success, this.appInfo.AuthInfo, this.asyncState));
                        };
                    }
                    ThreadPool.QueueUserWorkItem(callBack, null);
                }
            }
        }

        private bool InitializeSignInContext(AppInformation appInfo)
        {
            this.appInfo = appInfo;
            //this.serviceRoot = appInfo.ServiceRoot;
            this.BaseUri = appInfo.ServiceRoot;
            this.uiSyncContext = SynchronizationContext.Current;
            bool flag = (appInfo.AuthInfo != null) && (!string.IsNullOrEmpty(appInfo.AuthInfo.AccessToken) || !string.IsNullOrEmpty(appInfo.AuthInfo.RefreshToken));
            WebRequest.RegisterPrefix(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", new object[] { this.serviceRoot.Scheme, this.serviceRoot.Host }), WebRequestCreator.ClientHttp);
            return flag;
        }

        private static bool IsUnauthorizedException(Exception e)
        {
            bool flag = false;
            WebException exception = e as WebException;
            if ((exception != null) && (exception.Response != null))
            {
                HttpWebResponse response = exception.Response as HttpWebResponse;
                if ((response != null) && (response.StatusCode == HttpStatusCode.Unauthorized))
                {
                    flag = true;
                }
            }
            return flag;
        }

        private void LaunchLoginDialogAsync(WebBrowser host)
        {
            if (string.IsNullOrEmpty(this.appInfo.ClientId))
            {
                throw new ArgumentException(StringResource.NoClientId, "applicationInfo.ClientId");
            }
            if (string.IsNullOrEmpty(this.appInfo.ClientSecret))
            {
                throw new ArgumentException(StringResource.NoClientSecret, "applicationInfo.ClientSecret");
            }
            if (this.appInfo.RequestedOffers.Count == 0)
            {
                throw new ArgumentException(StringResource.NoOffer, "applicationInfo.RequestedOffers");
            }
            SignInDialog dialog = new SignInDialog(host, this.appInfo);
            dialog.SignInComplete += SignInDialogClosed;
            host.Visibility = Visibility.Visible;
            //Grid.SetColumn(dialog, 0);
            //Grid.SetColumnSpan(dialog, 1);
            //Grid.SetRow(dialog, 0);
            //Grid.SetRowSpan(dialog, 1);
            //dialog.Height = 600;
            //dialog.Width = 400;
            //host.Children.Add(dialog);
            //dialog.set_HasCloseButton(true);
            //dialog.add_Closed(new EventHandler(this.SignInDialogClosed));
            //dialog.Show();
        }

        private void OnBeforeSendingRequest(object sender, SendingRequestEventArgs e)
        {
            if (!this.isSignedIn)
            {
                throw new InvalidOperationException(StringResource.UserNotSignedIn);
            }
            e.RequestHeaders["X-HTTP-Live-Authorization"] = this.DelAuthToken;
            e.RequestHeaders["X-HTTP-Live-ApplicationId"] = this.appInfo.ClientId;
            SetHeaderForAnalytics(e.RequestHeaders);
        }

        private void OnContextCreated()
        {
            this.serviceRoot = base.BaseUri;
            base.ResolveName = new Func<Type, string>(this.ResolveNameFromType);
            base.ResolveType = new Func<string, Type>(this.ResolveTypeFromName);
            base.ResolveSet = new Func<string, Uri>(this.ResolveEntitySet);
            base.SendingRequest += new EventHandler<SendingRequestEventArgs>(this.OnBeforeSendingRequest);
            base.ReadingEntity += new EventHandler<ReadingWritingEntityEventArgs>(this.OnReadingEntity);
            base.SaveChangesDefaultOptions = SaveChangesOptions.ReplaceOnUpdate;
            base.MergeOption = MergeOption.OverwriteChanges;
            base.IgnoreMissingProperties = true;
        }

        private void OnReadingEntity(object sender, ReadingWritingEntityEventArgs e)
        {
            if ((e != null) && (e.Entity is LiveResource))
            {
                ((LiveResource) e.Entity).SetDataContext(this);
            }
        }

        private void OnSignInCompleted(bool completeRequest)
        {
            Interlocked.Exchange(ref this.connecting, 0);
            if (completeRequest)
            {
                this.HandleCompletion(this.isSignedIn);
            }
        }

        private static void ParseReturnedServiceDocumentXml(LiveDataContext dataContext, WebResponse webResponse, bool isAsync, Action<Exception> asyncCallbak)
        {
            XmlReader reader = XmlReader.Create(webResponse.GetResponseStream());
            reader.Read();
            XElement element = XElement.Load(reader);
            Uri uri = null;
            XAttribute attribute = element.Attribute(XName.Get("base", "http://www.w3.org/XML/1998/namespace"));
            if (attribute != null)
            {
                uri = new Uri(attribute.Value, UriKind.Absolute);
            }
            IEnumerable<XElement> source = element.Elements(XName.Get("workspace", "http://www.w3.org/2007/app"));
            XElement element2 = null;
            if (source.Count<XElement>() > 1)
            {
                foreach (XElement element3 in source)
                {
                    if (string.Compare(element3.Elements(XName.Get("title", "http://www.w3.org/2005/Atom")).First<XElement>().Value, "v4.0", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        element2 = element3;
                        break;
                    }
                }
            }
            else
            {
                element2 = source.FirstOrDefault<XElement>();
            }
            if ((dataContext.SignedInUser == null) && (element2 != null))
            {
                XElement userElement = element2.Elements(XName.Get("SignedInUser", "http://schemas.microsoft.com/ado/2007/08/dataservices")).Select<XElement, XElement>(delegate (XElement user) {
                    return user;
                }).SingleOrDefault<XElement>();
                if (userElement != null)
                {
                    dataContext.SignedInUser = new Microsoft.Live.SignedInUser();
                    PopulateSignedInUser(userElement, dataContext.SignedInUser);
                }
            }
            if (element2 != null)
            {
                if (!string.IsNullOrEmpty(element2.Attribute(XName.Get("base", "http://www.w3.org/XML/1998/namespace")).Value))
                {
                    uri = new Uri(element2.Attribute(XName.Get("base", "http://www.w3.org/XML/1998/namespace")).Value, UriKind.Absolute);
                }
                string text1 = element2.Elements(XName.Get("title", "http://www.w3.org/2005/Atom")).First<XElement>().Value;
                foreach (XElement element5 in element2.Elements(XName.Get("collection", "http://www.w3.org/2007/app")))
                {
                    IEnumerable<XElement> enumerable2 = element5.Descendants(XName.Get("workspace", "http://www.w3.org/2007/app"));
                    if (enumerable2.Count<XElement>() > 0)
                    {
                        XElement element6 = enumerable2.FirstOrDefault<XElement>();
                        Uri baseUri = uri;
                        if (!string.IsNullOrEmpty(element6.Attribute(XName.Get("base", "http://www.w3.org/XML/1998/namespace")).Value))
                        {
                            baseUri = new Uri(element6.Attribute(XName.Get("base", "http://www.w3.org/XML/1998/namespace")).Value, UriKind.Absolute);
                        }
                        foreach (XElement element7 in element6.Elements(XName.Get("collection", "http://www.w3.org/2007/app")))
                        {
                            string text2 = element7.Elements(XName.Get("title", "http://www.w3.org/2005/Atom")).First<XElement>().Value;
                            Uri uri3 = new Uri(baseUri, element7.Attribute(XName.Get("href")).Value);
                            string key = element7.Element(XName.Get("title", "http://www.w3.org/2005/Atom")).Value;
                            lock (dataContext.baseUris)
                            {
                                if (!dataContext.baseUris.ContainsKey(key))
                                {
                                    dataContext.baseUris.Add(key, uri3);
                                }
                                continue;
                            }
                        }
                        continue;
                    }
                }
            }
        }

        private static void PopulateSignedInUser(XElement userElement, Microsoft.Live.SignedInUser user)
        {
            foreach (XElement element in userElement.Elements())
            {
                string localName = element.Name.LocalName;
                if (localName != null)
                {
                    if (!(localName == "Cid"))
                    {
                        if (localName == "Uri")
                        {
                            goto Label_004E;
                        }
                    }
                    else
                    {
                        user.Cid = element.Value;
                    }
                }
                continue;
            Label_004E:
                user.Uri = element.Value;
            }
        }

        private void ProcessServiceDocumentsAsync(bool completeRequest, Action<Exception> callback)
        {
            if (this.baseUris.Count > 0)
            {
                this.isSignedIn = true;
                if (completeRequest)
                {
                    this.OnSignInCompleted(true);
                }
                if (callback != null)
                {
                    callback(null);
                }
            }
            else
            {
                this.GetBaseUrisFromServiceDocumentAsync(this.serviceRoot, true, delegate (Exception error) {
                    if (error == null)
                    {
                        this.isSignedIn = true;
                    }
                    else
                    {
                        this.appInfo.AuthInfo.Error = CreateAuthorizationException(StringResource.FailToReadServiceDoc, error);
                    }
                    if (completeRequest)
                    {
                        this.OnSignInCompleted(true);
                    }
                    if (callback != null)
                    {
                        callback(error);
                    }
                });
            }
        }

        private static void ReadServiceDocumentCompleted(IAsyncResult asyncResult)
        {
            if (asyncResult.IsCompleted)
            {
                WebRequestData asyncState = asyncResult.AsyncState as WebRequestData;
                LiveDataContext dataContext = asyncState.DataContext;
                Exception exception = null;
                WebRequest webRequest = asyncState.WebRequest;
                WebResponse webResponse = null;
                bool flag = false;
                try
                {
                    webResponse = webRequest.EndGetResponse(asyncResult);
                    if (webResponse == null)
                    {
                        throw new Exception(string.Format(CultureInfo.CurrentCulture, StringResource.NullResponseReceived, new object[] { webRequest.RequestUri.ToString() }));
                    }
                    ParseReturnedServiceDocumentXml(dataContext, webResponse, true, asyncState.Callback);
                }
                catch (Exception exception2)
                {
                    if (asyncState.IsRootDocument)
                    {
                        exception = exception2;
                        flag = true;
                    }
                }
                lock (dataContext.outstandingWebRequests)
                {
                    dataContext.outstandingWebRequests.Remove(asyncState);
                    if (dataContext.outstandingWebRequests.Count == 0)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    dataContext.isSignedIn = exception == null;
                    if (asyncState.Callback != null)
                    {
                        asyncState.Callback(exception);
                    }
                }
            }
        }

        private void RefreshAccessTokenAndSignInAsync(WebBrowser host)
        {
            WrapProtocolHelper.RefreshAccessTokenAsync(this.appInfo, delegate (bool success) {
                SendOrPostCallback d = null;
                if (success)
                {
                    this.ProcessServiceDocumentsAsync(true, null);
                }
                else
                {
                    if (this.appInfo.DisableUXSignIn)
                    {
                        this.appInfo.AuthInfo.Error = CreateAuthorizationException(StringResource.FailToRefresh, this.appInfo.AuthInfo.Error);
                        this.OnSignInCompleted(true);
                    }
                    try
                    {
                        if (d == null)
                        {
                            d = delegate {
                                this.LaunchLoginDialogAsync(host);
                            };
                        }
                        this.uiSyncContext.Post(d, null);
                    }
                    catch (Exception exception)
                    {
                        if (this.appInfo.AuthInfo.Error == null)
                        {
                            this.appInfo.AuthInfo.Error = exception;
                        }
                        this.OnSignInCompleted(true);
                    }
                }
            });
        }

        public void RefreshAccessTokenAsync(AppInformation applicationInfo)
        {
            if (!this.IsWindowsApp)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, StringResource.MustBeOOBFullTrust, new object[] { "RefreshAccessTokenAsync" }));
            }
            if (this.connecting == 1)
            {
                throw new InvalidOperationException(StringResource.SignInInProgress);
            }
            if (applicationInfo == null)
            {
                throw new ArgumentNullException("applicationInfo");
            }
            if ((applicationInfo.AuthInfo == null) || string.IsNullOrEmpty(applicationInfo.AuthInfo.RefreshToken))
            {
                throw new ArgumentException(StringResource.RefreshTokenInvalid, "applicationInfo.AuthInfo.RefreshToken");
            }
            if (string.IsNullOrEmpty(applicationInfo.ClientId))
            {
                throw new ArgumentException(StringResource.NoClientId, "applicationInfo.ClientId");
            }
            if (string.IsNullOrEmpty(applicationInfo.ClientSecret))
            {
                throw new ArgumentException(StringResource.NoClientSecret, "applicationInfo.ClientSecret");
            }
            this.appInfo = applicationInfo;
            this.isSignedIn = false;
            WrapProtocolHelper.RefreshAccessTokenAsync(this.appInfo, delegate (bool success) {
                this.isSignedIn = success;
                if (success)
                {
                    this.OnSignInCompleted(true);
                }
                else
                {
                    this.appInfo.AuthInfo.Error = CreateAuthorizationException(StringResource.FailToRefresh, this.appInfo.AuthInfo.Error);
                    this.OnSignInCompleted(true);
                }
            });
        }

        internal Uri ResolveEntitySet(string entitySetName)
        {
            if (!this.isSignedIn)
            {
                throw new InvalidOperationException(StringResource.UserNotSignedIn);
            }
            Uri uri = null;
            if (Uri.IsWellFormedUriString(entitySetName, UriKind.Absolute))
            {
                uri = new Uri(entitySetName);
            }
            if (this.baseUris.ContainsKey(entitySetName))
            {
                uri = this.baseUris[entitySetName];
            }
            if (uri == null)
            {
                throw new DataServiceClientException(string.Format(CultureInfo.CurrentCulture, StringResource.CantFindBaseUri, new object[] { entitySetName }));
            }
            if (string.Compare(uri.Host, this.serviceRoot.Host, StringComparison.OrdinalIgnoreCase) != 0)
            {
                WebRequest.RegisterPrefix(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", new object[] { uri.Scheme, uri.Host }), WebRequestCreator.ClientHttp);
            }
            return uri;
        }

        protected string ResolveNameFromType(Type clientType)
        {
            if (clientType.Namespace.Equals("Microsoft.Live", StringComparison.Ordinal))
            {
                return clientType.Name;
            }
            return clientType.FullName;
        }

        protected Type ResolveTypeFromName(string typeName)
        {
            if (typeName.LastIndexOf('.') == -1)
            {
                return base.GetType().Assembly.GetType("Microsoft.Live." + typeName, true);
            }
            return null;
        }

        private static void SetHeaderForAnalytics(WebHeaderCollection headers)
        {
            string[] strArray = typeof(LiveDataContext).Assembly.FullName.Split(new char[] { ',' });
            if (strArray.Length >= 2)
            {
                string str = strArray[0].Trim();
                string str2 = string.Empty;
                for (int i = 1; i < strArray.Length; i++)
                {
                    string str3 = strArray[i].Trim();
                    int index = str3.IndexOf("Version=");
                    if (index != -1)
                    {
                        str2 = str3.Substring(index + "Version=".Length);
                        break;
                    }
                }
                headers["X-HTTP-Live-Library"] = string.Format(CultureInfo.InvariantCulture, "{0}.{1}/{2}", new object[] { str, "Silverlight", str2 });
            }
        }

        public void SignInAsync(WebBrowser host,AppInformation applicationInfo, object state)
        {
            if (applicationInfo == null)
            {
                throw new ArgumentNullException("applicationInfo");
            }
            this.asyncState = state;
            bool flag = this.InitializeSignInContext(applicationInfo);
            this.CheckSignInStatus();
            if (flag)
            {
                this.SignInWithTokenAsync(host);
            }
            else
            {
                this.LaunchLoginDialogAsync(host);
            }
        }

        private void SignInDialogClosed(object sender, EventArgs e)
        {
            Action<bool> callback = null;
            SignInDialog dialog = sender as SignInDialog;
            if (dialog != null)
            {
                dialog.Browser.Visibility = Visibility.Collapsed;
                dialog.SignInComplete -= SignInDialogClosed;
                //dialog.remove_Closed(new EventHandler(this.SignInDialogClosed));
                if ((dialog.DialogResult??false) ==  false)
                {
                    if (this.appInfo.AuthInfo.Error == null)
                    {
                        this.appInfo.AuthInfo.Error = CreateAuthorizationException(StringResource.FailToAuthenticate, null);
                    }
                    this.OnSignInCompleted(true);
                }
                else
                {
                    if (callback == null)
                    {
                        callback = delegate(bool success)
                        {
                            if (!success)
                            {
                                this.OnSignInCompleted(true);
                            }
                            else
                            {
                                this.ProcessServiceDocumentsAsync(true, null);
                            }
                        };
                    }
                    WrapProtocolHelper.RequestAccessTokenAsync(this.appInfo, callback);
                }
            }
        }

        private void SignInWithTokenAsync(WebBrowser host)
        {
            Action<Exception> callback = null;
            if (!string.IsNullOrEmpty(this.appInfo.AuthInfo.AccessToken))
            {
                if (callback == null)
                {
                    callback = delegate (Exception error) {
                        if (error == null)
                        {
                            this.isSignedIn = true;
                            this.OnSignInCompleted(true);
                        }
                        else if (IsUnauthorizedException(error) && !string.IsNullOrEmpty(this.appInfo.AuthInfo.RefreshToken))
                        {
                            this.RefreshAccessTokenAndSignInAsync(host);
                        }
                        else
                        {
                            this.appInfo.AuthInfo.Error = CreateAuthorizationException(StringResource.FailToReadServiceDoc, error);
                            this.OnSignInCompleted(true);
                        }
                    };
                }
                this.ProcessServiceDocumentsAsync(false, callback);
            }
            else
            {
                this.RefreshAccessTokenAndSignInAsync(host);
            }
        }

        public DataServiceQuery<Album> AlbumsQuery
        {
            get
            {
                if (this._Albums == null)
                {
                    this._Albums = base.CreateQuery<Album>("Albums");
                }
                return this._Albums;
            }
        }

        public Uri BaseUri
        {
            get
            {
                return this.serviceRoot;
            }
            set
            {
                base.BaseUri = value;
            }
        }

        public DataServiceQuery<CalendarEvent> CalendarEventsQuery
        {
            get
            {
                if (this._CalendarEvents == null)
                {
                    this._CalendarEvents = base.CreateQuery<CalendarEvent>("Events");
                }
                return this._CalendarEvents;
            }
        }

        public DataServiceQuery<Calendar> CalendarsQuery
        {
            get
            {
                if (this._Calendars == null)
                {
                    this._Calendars = base.CreateQuery<Calendar>("Calendars");
                }
                return this._Calendars;
            }
        }

        public IDictionary<string, Uri> CollectionBaseUris
        {
            get
            {
                return this.baseUris;
            }
            set
            {
                if ((value == null) || (value.Count == 0))
                {
                    throw new ArgumentException("CollectionBaseUris");
                }
                if ((this.connecting == 1) || this.isSignedIn)
                {
                    throw new InvalidOperationException(StringResource.CantSetCollectionBaseUris);
                }
                this.baseUris = new Dictionary<string, Uri>(value);
            }
        }

        public DataServiceQuery<ContactCategory> ContactCategoriesQuery
        {
            get
            {
                if (this._Categories == null)
                {
                    this._Categories = base.CreateQuery<ContactCategory>("Categories");
                }
                return this._Categories;
            }
        }

        public DataServiceQuery<Activity> ContactsActivitiesQuery
        {
            get
            {
                if (this._ContactActivities == null)
                {
                    this._ContactActivities = base.CreateQuery<Activity>("ContactsActivities");
                }
                return this._ContactActivities;
            }
        }

        public DataServiceQuery<Contact> ContactsQuery
        {
            get
            {
                //this.BaseUri = new Uri(this.baseUris["AllContacts"].AbsoluteUri.Replace("AllContacts",""));

                if (this._Contacts == null)
                {
                    this._Contacts = base.CreateQuery<Contact>("AllContacts");

                }
                return this._Contacts;
            }
        }

        private string DelAuthToken
        {
            get
            {
                if (!this.isFullTrustAuth)
                {
                    return ("WRAP access_token=" + this.appInfo.AuthInfo.AccessToken);
                }
                return this.appInfo.AuthInfo.AccessToken;
            }
        }

        public DataServiceQuery<DocumentFolder> DocumentFoldersQuery
        {
            get
            {
                if (this._DocumentFolders == null)
                {
                    this._DocumentFolders = base.CreateQuery<DocumentFolder>("Folders");
                }
                return this._DocumentFolders;
            }
        }

        public bool IsUserSignedIn
        {
            get
            {
                return this.isSignedIn;
            }
        }

        private bool IsWindowsApp
        {
            get
            {
                return (((Application.Current != null)));// && Application.Current.get_IsRunningOutOfBrowser()) && Application.Current.get_HasElevatedPermissions());
            }
        }

        public DataServiceQuery<Activity> MyActivitiesQuery
        {
            get
            {
                if (this._MyActivities == null)
                {
                    this._MyActivities = base.CreateQuery<Activity>("MyActivities");
                }
                return this._MyActivities;
            }
        }

        public DataServiceQuery<Profile> ProfilesQuery
        {
            get
            {
                if (this._Profiles == null)
                {
                    this._Profiles = base.CreateQuery<Profile>("Profiles");
                }
                return this._Profiles;
            }
        }

        //public Microsoft.Live.SignedInUser SignedInUser
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<SignedInUser>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<SignedInUser>k__BackingField = value;
        //    }
        //}
    }

  

}

