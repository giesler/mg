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
            using (ServiceHost host = new ServiceHost(typeof(ProXRRelayPulseController)))
            {
                host.Open();

                Console.WriteLine("Listening...");
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
