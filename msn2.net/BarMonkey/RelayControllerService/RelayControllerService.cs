using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace msn2.net.BarMonkey.RelayController
{
    public partial class RelayControllerService : ServiceBase
    {
        private ServiceHost host = null;

        public RelayControllerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.host = new ServiceHost(typeof(a));
            this.host.Open();
        }

        protected override void OnStop()
        {
            this.host.Close();
        }
    }
}
