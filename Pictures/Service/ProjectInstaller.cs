using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;

namespace PictureService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        EventLogInstaller eventLogInstaller;

        public ProjectInstaller()
        {
            InitializeComponent();

            this.eventLogInstaller = new EventLogInstaller();
            this.eventLogInstaller.Log = "Picture Monitor";
            this.eventLogInstaller.Source = this.eventLogInstaller.Log;
            this.eventLogInstaller.BeforeInstall += new InstallEventHandler(eventLogInstaller_BeforeInstall);
            this.Installers.Add(this.eventLogInstaller);
        }

        void eventLogInstaller_BeforeInstall(object sender, InstallEventArgs e)
        {
            if (EventLog.SourceExists("Picture Monitor"))
            {
                EventLog.DeleteEventSource("Picture Monitor");
            }
        }
    }
}