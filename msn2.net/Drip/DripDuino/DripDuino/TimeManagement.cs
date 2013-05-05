using System;
using Microsoft.SPOT;
using System.Net;
using System.Threading;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace DripDuino
{
    class TimeManagement
    {
        public static bool TimeSet { get; private set; }

        public static void Run()
        {
            while (true)
            {
                // get time
                Debug.Print("Setting time...");
                while (true)
                {
                    DateTime current = DateTime.Now;
                    int offset = -7;        // default to PST summer

                    try
                    {
                        offset = GetTimezoneOffset(offset);

                        DateTime time = NTPTime("ike.sp.msn2.net", offset);
                        Microsoft.SPOT.Hardware.Utility.SetLocalTime(time);
                        Debug.Print("Set time to " + time.ToString() + " from " + current.ToString());
                        TimeSet = true;
                        break;
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
                    byte[] buffer = new byte[500];
                    int count = stream.Read(buffer, 0, buffer.Length);
                    char[] chars = Encoding.UTF8.GetChars(buffer);

                    string content = string.Empty;
                    foreach (char c in chars)
                    {
                        content += c;
                    }

                    string[] properties = content.Split(new char[] { '{', ',', '}' });
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

    }
}
