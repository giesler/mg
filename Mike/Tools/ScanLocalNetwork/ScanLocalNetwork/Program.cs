using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ScanLocalNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists("log.txt"))
            {
                File.Delete("log.txt");
            }
                        
            for (int i = 1; i < 255; i++)
            {
                try
                {
                    /*
                    WebRequest request = WebRequest.CreateDefault(new Uri("http://192.168.1." + i.ToString()));
                    request.Timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                    WebResponse response = request.GetResponse();
                    Log("192.168.1.{0},OK,{1}", i, response.ContentLength);                                        
                     */
                    Ping ping = new Ping();
                    PingReply reply = ping.Send(IPAddress.Parse("192.168.4." + i.ToString()), (int)TimeSpan.FromSeconds(4).TotalMilliseconds);
                    Log("192.168.4.{0},OK,{1}", i, reply.Status);                                        
                }
                catch (Exception ex)
                {
                    Log("192.168.4.{0},Exception,{1}", i, ex.Message);
                }
            }            
        }

        static void Log(string message, params object[] args)
        {
            File.AppendAllText("log.txt", string.Format(message, args));
            Console.WriteLine(string.Format(message, args));
        }
    }
}
