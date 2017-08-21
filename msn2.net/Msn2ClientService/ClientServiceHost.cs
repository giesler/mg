using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceProcess;

namespace Msn2ClientService
{
    public partial class ClientServiceHost : ServiceBase
    {
        WebServiceHost host = null;
        ServiceEndpoint ep = null;

        public ClientServiceHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        public void Start()
        {
            RunNetSh(string.Format("advfirewall firewall add rule name=\"Msn2ClientService (TCP 4646)\" dir=in action=allow protocol=TCP localport=4646"));

            host = new WebServiceHost(typeof(ClientService), new Uri("http://localhost:4646/"));
            ep = host.AddServiceEndpoint(typeof(IClientService), new WebHttpBinding(), "");

            ServiceDebugBehavior sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.HttpHelpPageEnabled = false;

            host.Open();
        }

        private static void RunNetSh(string args)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("netsh.exe", args);
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();
        }

        protected override void OnStop()
        {
            host.Close();

            RunNetSh(string.Format("advfirewall firewall delete rule name=\"Msn2ClientService (TCP 4646)\" protocol=TCP localport=4646"));
        }
    }
}
