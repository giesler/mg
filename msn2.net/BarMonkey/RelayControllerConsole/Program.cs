using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using msn2.net.BarMonkey.RelayController;

namespace msn2.net.BarMonkey.RelayController
{
    class Program
    {
        static void Main(string[] args)
        {
            Type serviceType = typeof(ProXRRelayPulseController);
            if (args.Length > 0 && args[0] == "/console")
            {
                serviceType = typeof(ConsoleRelayController);
            }

            Console.WriteLine("Starting controller " + serviceType.Name);

            using (ServiceHost host = new ServiceHost(serviceType))
            {
                host.Open();

                Console.WriteLine("Listening at " + host.BaseAddresses[0].ToString());
                try
                {
                    Console.ReadLine();
                }
                catch (Exception)
                {
                }

                host.Close();

            }
        }
    }
}
