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
        public static void Main()
        {
            Thread timeThread = new Thread(new ThreadStart(TimeManagement.Run));
            timeThread.Start();

            while (!TimeManagement.TimeSet)
            {
                Thread.Sleep(1000);
            }

            Thread webServerThread = new Thread(new ThreadStart(DripHttpServer.Run));
            webServerThread.Start();

            Thread dripThread = new Thread(new ThreadStart(Dripper.Run));
            dripThread.Start();

            while (true)
            {
                Thread.Sleep(100000);
            }
        }
    }
}
