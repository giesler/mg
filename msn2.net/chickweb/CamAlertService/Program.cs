using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace CamAlertService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "/console")
            {
                CamAlertMonitor monitor = new CamAlertMonitor();
                monitor.Start();

                while (true)
                {
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch (Exception)
                    {
                    }
                }

                Console.WriteLine("Shutting down...");

                monitor.Stop();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			        { 
				        new CamAlertService() 
			        };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
