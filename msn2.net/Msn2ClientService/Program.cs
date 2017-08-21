using System;
using System.ServiceProcess;

namespace Msn2ClientService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "/console")
            {
                ClientServiceHost host = new ClientServiceHost();
                host.Start();
                Console.ReadLine();
                host.Stop();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new ClientServiceHost() 
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
