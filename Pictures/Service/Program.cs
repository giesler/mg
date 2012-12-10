#region Using directives

using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System;
using msn2.net.Pictures;

#endregion

namespace PictureService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            PicContext.Login(PictureConfig.Load(), "mike@giesler.org", "wingwang");

            if (args.Length == 1 && args[0].ToLower() == "/console")
            {
                PictureMonitorService monitorService = new PictureMonitorService();
                monitorService.Start();
                try
                {
                    Console.ReadLine();
                }
                finally
                {
                    monitorService.Shutdown();
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;

                // More than one user Service may run within the same process. To add
                // another service to this process, change the following line to
                // create a second service object. For example,
                //
                //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
                //
                ServicesToRun = new ServiceBase[] { new Service1() };

                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}