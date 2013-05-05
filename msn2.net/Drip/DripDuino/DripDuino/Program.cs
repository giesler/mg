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
        private static bool timeSet = false;
        private static Thread timeThread;
        private static Thread webServerThread;

        public static void Main()
        {
            timeThread = new Thread(new ThreadStart(TimeThread));
            timeThread.Start();

            while (!timeSet)
            {
                Thread.Sleep(1000);
            }

            webServerThread = new Thread(new ThreadStart(WebServerThread));
            webServerThread.Start();

            while (true)
            {
                Thread.Sleep(10000);
            }
        }

        private static void TimeThread()
        {
            while (true)
            {
                // get time
                bool set = false;
                Debug.Print("Setting time...");
                while (!set)
                {
                    DateTime current = DateTime.Now;
                    int offset = -7;        // default to PST summer

                    try
                    {
                        offset = GetTimezoneOffset(offset);

                        DateTime time = NTPTime("ike.sp.msn2.net", offset);
                        Microsoft.SPOT.Hardware.Utility.SetLocalTime(time);
                        Debug.Print("Set time to " + time.ToString() + " from " + current.ToString());
                        set = true;

                    }
                    catch (Exception ex)
                    {
                        Debug.Print("Error setting time, waiting 10 seconds: " + ex.Message);
                    }
                }

                // Sleep for one day
                DateTime start = DateTime.Now;
                while (start.AddDays(1) > DateTime.Now)
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private static int GetTimezoneOffset(int offset)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("http://api.geonames.org/timezoneJSON?lat=47.680265&lng=-122.172113&username=demo");
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                using (Stream stream = webResponse.GetResponseStream())
                {
                    int length = (int)webResponse.ContentLength;
                    byte[] buffer = new byte[length];
                    int toRead = buffer.Length;
                    while (toRead > 0)
                    {
                        int read = stream.Read(buffer, buffer.Length - toRead, toRead);
                        toRead = toRead - read;
                    }

                    char[] chars = Encoding.UTF8.GetChars(buffer);
                    string[] properties = chars.ToString().Split(new char[] { '{', ',', '}' });
                    foreach (string item in properties)
                    {
                        if (item.IndexOf("dstOffset") > 0)
                        {
                            string[] parts = item.Split(new char[] { ':' });
                            offset = int.Parse(parts[1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Error getting timezone offset: " + ex.Message);
            }

            return offset;
        }

        static DateTime NTPTime(String TimeServer, int UTC_offset)
        {
            // Find endpoint for timeserver
            IPEndPoint ep = new IPEndPoint(Dns.GetHostEntry(TimeServer).AddressList[0], 123);

            // Connect to timeserver
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.Connect(ep);

            // Make send/receive buffer
            byte[] ntpData = new byte[48];
            Array.Clear(ntpData, 0, 48);

            // Set protocol version
            ntpData[0] = 0x1B;

            // Send Request
            s.Send(ntpData);

            // Receive Time
            s.Receive(ntpData);

            byte offsetTransmitTime = 40;

            ulong intpart = 0;
            ulong fractpart = 0;

            for (int i = 0; i <= 3; i++)
                intpart = (intpart << 8) | ntpData[offsetTransmitTime + i];

            for (int i = 4; i <= 7; i++)
                fractpart = (fractpart << 8) | ntpData[offsetTransmitTime + i];

            ulong milliseconds = (intpart * 1000 + (fractpart * 1000) / 0x100000000L);

            s.Close();

            TimeSpan timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);
            DateTime dateTime = new DateTime(1900, 1, 1);
            dateTime += timeSpan;

            TimeSpan offsetAmount = new TimeSpan(0, UTC_offset, 0, 0, 0);
            DateTime networkDateTime = (dateTime + offsetAmount);

            return networkDateTime;
        }

        private static void WebServerThread()
        {

        }
    }
}
