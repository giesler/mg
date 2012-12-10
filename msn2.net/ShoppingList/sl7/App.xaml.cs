﻿using System;
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

namespace giesler.org.lists
{
    public partial class App : Application
    {
        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

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
            this.LoadSettings();
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
            // Ensure that application state is restored appropriately
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

        public static List<List> Lists { get; set; }
        public static List<ListItemEx> Items { get; set; }
        public static ListAuth.ClientAuthenticationData AuthData { get; set; }
        public static string LiveIdUserId { get; set; }
        public static string LiveIdAccessToken { get; set; }
        public static string LiveIdRefreshToken { get; set; }
        public static List<Contact> LiveContacts { get; set; }

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

        public static void SetClientAuth(ListAuth.ClientAuthenticationData listAuth, AppAuthentication liveAuth)
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

            settings.Save();
        }

        void LoadSettings()
        {
            AuthData = new ListAuth.ClientAuthenticationData();
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

            Debug.WriteLine(LiveIdUserId);
            Debug.WriteLine(LiveIdAccessToken);
            Debug.WriteLine(LiveIdRefreshToken);
        }

        static void SetAppSetting(string name, string value)
        {
            if (App.settings.Contains(name))
            {
                App.settings[name] = value;
            }
            else
            {
                App.settings.Add(name, value);
            }
        }
    }
}