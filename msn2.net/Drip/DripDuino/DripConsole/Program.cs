using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace DripConsole
{
    class Program
    {
        static string DripAddress = "dripduino.msn2.net";

        static void Main(string[] args)
        {
            try
            {
                if (Debugger.IsAttached)
                {
                    Thread.Sleep(1000 * 10);
                }

                if (args.Length == 2 && args[0] == "drip")
                {
                    Post("/toggle", args[1]);
                }
                else if (args.Length == 2 && args[0] == "deletelog")
                {
                    Post("/deletelog", args[1]);
                }
                else
                {
                    Console.WriteLine("usage: DripConsole drip hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            if (Debugger.IsAttached)
            {
                Console.Read();
            }
        }

        static void Post(string relativeUrl, string content)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("http://" + DripAddress + relativeUrl);
            webRequest.Method = "POST";
            webRequest.ContentType = "text/plain";

            byte[] bytes = Encoding.UTF8.GetBytes(content);
            webRequest.ContentLength = bytes.Length;

            using (Stream stream = webRequest.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            Log("Requesting " + webRequest.RequestUri + " - " + content);
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            byte[] responseBytes = new byte[response.ContentLength]; 
            using (Stream stream = response.GetResponseStream())
            {
                stream.Read(responseBytes, 0, (int)response.ContentLength);
            }

            string responseString = Encoding.UTF8.GetString(responseBytes);
            Log(response.StatusCode + " - " + responseString);
        }

        static void Log(string message)
        {
            Console.WriteLine(DateTime.Now.ToString("D/m/y h:mm:ss") + ": " + message);
        }
    }
}
