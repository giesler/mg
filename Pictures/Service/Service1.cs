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
        PictureMonitorService monitorService = null;

        public Service1()
        {
            InitializeComponent();

            this.monitorService = new PictureMonitorService();
        }

        protected override void OnStart(string[] args)
        {
            this.monitorService.Start();
        }

        protected override void OnStop()
        {
            this.monitorService.Shutdown();
        }
    }
}
