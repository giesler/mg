using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Microsoft.Live;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using giesler.org.lists.ListData;
using System.IO;
using System.Xml.Linq;

namespace giesler.org.lists
{
    public partial class App : Application
    {
        private IsolatedStorageSettings settings;

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters
                //Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        string GetSetting(string name)
        {
            string val = null;

            if (settings.Contains(name) && settings[name] != null)
            {
                val = settings[name].ToString();
            }

            return val;
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved)
            {
                settings = IsolatedStorageSettings.ApplicationSettings;
            }
            else
            {
                Debug.WriteLine("App instance preserved");
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        public static List<ListEx> Lists { get; set; }
        public static ClientAuthenticationData AuthData { get; set; }
        public static string LiveIdUserId { get; set; }
        public static string LiveIdAccessToken { get; set; }
        public static string LiveIdRefreshToken { get; set; }
        public static List<Contact> LiveContacts { get; set; }
        public static Guid SelectedList
        {
            get
            {
                return selectedList;
            }
            set
            {
                selectedList = value;
            }
        }
        public static bool IsJumpNavigation { get; set; }
        
        static Guid selectedList;

        public static DateTime lastRefreshTime = DateTime.MinValue;
        public static DateTime LastRefreshTime
        {
            get
            {
                return lastRefreshTime;
            }

            set
            {
                lastRefreshTime = value;
            }
        }

        internal static ListDataServiceClient DataProvider
        {
            get
            {
                return new ListDataServiceClient();
            }
        }

        public static new App Current
        {
            get
            {
                return Application.Current as App;
            }
        }

        public static ListData.ClientAuthenticationData AuthDataList
        {
            get
            {
                ListData.ClientAuthenticationData data = null;

                if (AuthData != null)
                {
                    data = new ClientAuthenticationData { DeviceUniqueId = AuthData.DeviceUniqueId, PersonUniqueId = AuthData.PersonUniqueId };
                }

                return data;
            }
        }

        public void SetClientAuth(ClientAuthenticationData listAuth, AppAuthentication liveAuth)
        {
            SetAppSetting("Auth.PersonUniqueId", listAuth.PersonUniqueId.ToString());
            SetAppSetting("Auth.DeviceUniqueId", listAuth.DeviceUniqueId.ToString());
            SetAppSetting("LiveId.UserId", liveAuth.UserId);
            SetAppSetting("LiveId.AccessToken", liveAuth.AccessToken);
            SetAppSetting("LiveId.RefreshToken", liveAuth.RefreshToken);

            AuthData = listAuth;

            LiveIdUserId = liveAuth.UserId;
            LiveIdAccessToken = liveAuth.AccessToken;
            LiveIdRefreshToken = liveAuth.RefreshToken;

            this.settings.Save();
        }

        void SetAppSetting(string name, string value)
        {
            if (this.settings.Contains(name))
            {
                this.settings[name] = value;
            }
            else
            {
                this.settings.Add(name, value);
            }
        }

        const string settingFileName = "listgo.settings";

        object settingsLockObject = new object();
        object dataLockObject = new object();

        public void SaveSettings()
        {
            lock (this.settingsLockObject)
            {
                SetAppSetting("LastRefreshTime", lastRefreshTime.ToString());
                SetAppSetting("SelectedList", App.SelectedList.ToString());

                settings.Save();
            }
        }

        public void SaveAll()
        {
            SetAppSetting("SelectedList", App.SelectedList.ToString());
            this.SaveSettings();

            XDocument doc = new XDocument();

            XElement root = new XElement("data");
            doc.AddFirst(root);

            XElement listsElement = new XElement("lists");
            root.Add(listsElement);

            lock (this.dataLockObject)
            {
                foreach (var list in App.Lists)
                {
                    XElement listElement = new XElement("list");
                    listElement.Add(new XElement("name", list.Name));
                    listElement.Add(new XAttribute("id", list.Id));
                    listElement.Add(new XAttribute("uniqueId", list.UniqueId));
                    listsElement.Add(listElement);

                    XElement itemsElement = new XElement("items");
                    listElement.Add(itemsElement);

                    foreach (ListItemEx item in list.Items)
                    {
                        XElement itemElement = new XElement("item");
                        itemElement.Add(new XElement("name", item.Name));
                        itemElement.Add(new XAttribute("uniqueId", item.UniqueId));
                        itemsElement.Add(itemElement);
                    }
                }

                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(settingFileName))
                    {
                        store.DeleteFile(settingFileName);
                    }

                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(settingFileName, FileMode.Create, store))
                    {
                        doc.Save(stream);
                        stream.Close();
                    }
                }
            }
        }


        private bool haveLoadedAll = false;

        public void LoadAll()
        {
            if (this.haveLoadedAll == false)
            {
                this.haveLoadedAll = true;

                AuthData = new ClientAuthenticationData();
                string temp = GetSetting("Auth.PersonUniqueId");
                if (!string.IsNullOrEmpty(temp))
                {
                    AuthData.PersonUniqueId = new Guid(temp);
                }
                temp = GetSetting("Auth.DeviceUniqueId");
                if (!string.IsNullOrEmpty(temp))
                {
                    AuthData.DeviceUniqueId = new Guid(temp);
                }
                LiveIdUserId = GetSetting("LiveId.UserId");
                LiveIdAccessToken = GetSetting("LiveId.AccessToken");
                LiveIdRefreshToken = GetSetting("LiveId.RefreshToken");
                temp = GetSetting("SelectedList");
                if (!string.IsNullOrEmpty(temp))
                {
                    SelectedList = new Guid(temp);
                }
                //#if DEBUG
                if (AuthData.PersonUniqueId == Guid.Empty)
                {
                    AuthData.PersonUniqueId = new Guid("{feea96d5-3919-42af-8db2-eada650a7dec}"); // giesler@live.com
                }
                if (AuthData.DeviceUniqueId == Guid.Empty)
                {
                    AuthData.DeviceUniqueId = new Guid("{0a0e9d2d-125c-4eef-b4e4-540ddedcf99e}");  // emulator
                }

                //#endif

                string lastTime = GetSetting("LastRefreshTime");
                if (!string.IsNullOrEmpty(lastTime))
                {
                    App.LastRefreshTime = DateTime.Parse(lastTime);
                }

                Debug.WriteLine(LiveIdUserId);
                Debug.WriteLine(LiveIdAccessToken);
                Debug.WriteLine(LiveIdRefreshToken);

                App.Lists = new List<ListEx>();
                lock (this.dataLockObject)
                {
                    using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (store.FileExists(settingFileName))
                        {
                            try
                            {
                                IsolatedStorageFileStream stream = new IsolatedStorageFileStream(settingFileName, FileMode.Open, store);
                                XDocument doc = XDocument.Load(stream);

                                XElement dataElement = doc.Element("data");
                                XElement listsElement = dataElement.Element("lists");
                                foreach (XElement listElement in listsElement.Elements("list"))
                                {
                                    string name = listElement.Element("name").Value;
                                    Guid uniqueId = new Guid(listElement.Attribute("uniqueId").Value);

                                    ListEx list = new ListEx { Name = name, UniqueId = uniqueId };
                                    App.Lists.Add(list);

                                    XElement itemsElement = listElement.Element("items");
                                    foreach (XElement itemElement in itemsElement.Elements("item"))
                                    {
                                        string itemName = itemElement.Element("name").Value;
                                        Guid itemUniqueId = new Guid(itemElement.Attribute("uniqueId").Value);

                                        ListItemEx item = new ListItemEx { Name = itemName, UniqueId = itemUniqueId, ListUniqueId = list.UniqueId };
                                        list.Items.Add(item);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}