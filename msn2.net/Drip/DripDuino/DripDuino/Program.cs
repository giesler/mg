using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.IO;
using System.Text;

namespace DripDuino
{
    public class Program
    {
        private static Thread timeThread;
        private static Thread webServerThread;
        private static Thread dripThread;

        public static void Main()
        {
            timeThread = new Thread(new ThreadStart(TimeManagement.Run));
            timeThread.Start();

            while (!TimeManagement.TimeSet)
            {
                Thread.Sleep(1000);
            }

            webServerThread = new Thread(new ThreadStart(DripHttpServer.Run));
            webServerThread.Start();

            dripThread = new Thread(new ThreadStart(Dripper.Run));
            dripThread.Start();

            while (true)
            {
                Thread.Sleep(100000);
            }
        }
    }
}
