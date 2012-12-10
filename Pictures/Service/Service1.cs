#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

#endregion

namespace PictureService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            fileSystemWatcher1.EnableRaisingEvents = true;
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
    }
}
