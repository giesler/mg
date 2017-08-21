using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Threading;

namespace CamAlertService
{
    public partial class CamAlertService : ServiceBase
    {
        CamAlertMonitor monitor;

        public CamAlertService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            monitor = new CamAlertMonitor();
            monitor.Start();
        }

        protected override void OnStop()
        {
            monitor.Stop();
        }
    }
}
