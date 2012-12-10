#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using msn2.net.Pictures;

#endregion

namespace PictureService
{
    public partial class Service1 : ServiceBase
    {
        private PictureStorageManager pictureStorageManager = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            fileSystemWatcher1.EnableRaisingEvents = true;

            ConfigureStorageManager();
        }

        protected override void OnStop()
        {
            fileSystemWatcher1.EnableRaisingEvents = false;
        }

        private void fileSystemWatcher1_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                eventLog1.WriteEntry("Processing file " + e.FullPath);

            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("Error processing file " + e.FullPath + ": " + ex.Message + ", " + ex.StackTrace);
            }
            finally
            {
                eventLog1.WriteEntry("Done processing file " + e.FullPath);
            }
        }

        private void ConfigureStorageManager()
        {
            // Initialize channel and remoting
            TcpChannel channel = new TcpChannel(PictureStorageManager.GetDefaultPort());
            ChannelServices.RegisterChannel(channel);
            
            RemotingConfiguration.ApplicationName = PictureStorageManager.GetDefaultApplicationName();
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(PictureStorageManager),
                PictureStorageManager.GetDefaultObjectUri(), 
                WellKnownObjectMode.Singleton);

            // Activate
            pictureStorageManager  = (PictureStorageManager)Activator.GetObject(
                typeof(PictureStorageManager), 
                PictureStorageManager.GetUri(Environment.MachineName));
            pictureStorageManager.Ping();
        }

    }
}
