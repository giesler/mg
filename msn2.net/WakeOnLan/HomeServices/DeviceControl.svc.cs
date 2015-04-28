using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace HomeServices
{
    [AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Allowed)]
    public class DeviceControl : IDeviceControl
    {
        static string GardenDripAddress = "192.168.1.205";

        public DeviceStatus ToggleDevice(string name)
        {
            if (string.Equals(name, "Garden Drip", StringComparison.InvariantCultureIgnoreCase))
            {
                Post("/status", "status");
                return GetDeviceStatus(name);
            }

            throw new Exception("Unknown name");
        }

        public DeviceStatus TurnOff(string name)
        {
            if (string.Equals(name, "Garden Drip", StringComparison.InvariantCultureIgnoreCase))
            {
                Post("/turnoff", "off");
                return GetDeviceStatus(name);
            }

            throw new Exception("Unknown name");
        }

        public DeviceStatus TurnOn(string name, TimeSpan duration)
        {
            if (string.Equals(name, "Garden Drip", StringComparison.InvariantCultureIgnoreCase))
            {
                Post("/toggle", duration.ToString());
                return GetDeviceStatus(name);
            }

            throw new Exception("Unknown name");
        }

        public DeviceStatus GetDeviceStatus(string name)
        {
            if (string.Equals(name, "Garden Drip", StringComparison.InvariantCultureIgnoreCase))
            {
                string result = Get("/statustext");
                DeviceStatus status = new DeviceStatus { IsOn = result.ToLower().StartsWith("on"), StatusText = result.ToLower() };
                return status;
            }

            throw new Exception("Unknown name");
        }

        static void Post(string relativeUrl, string content)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("http://" + GardenDripAddress + relativeUrl);
            webRequest.Method = "POST";
            webRequest.ContentType = "text/plain";

            byte[] bytes = Encoding.UTF8.GetBytes(content);
            webRequest.ContentLength = bytes.Length;

            using (Stream stream = webRequest.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            Debug.WriteLine("Requesting " + webRequest.RequestUri + " - " + content);
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            byte[] responseBytes = new byte[response.ContentLength];
            using (Stream stream = response.GetResponseStream())
            {
                stream.Read(responseBytes, 0, (int)response.ContentLength);
            }

            string responseString = Encoding.UTF8.GetString(responseBytes);
            Debug.WriteLine(response.StatusCode + " - " + responseString);
        }

        static string Get(string relativeUrl)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("http://" + GardenDripAddress + relativeUrl);
            webRequest.Method = "GET";
            webRequest.ContentType = "text/plain";

            Debug.WriteLine("Requesting " + webRequest.RequestUri);
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            byte[] responseBytes = new byte[response.ContentLength];
            using (Stream stream = response.GetResponseStream())
            {
                stream.Read(responseBytes, 0, (int)response.ContentLength);
            }

            string responseString = Encoding.UTF8.GetString(responseBytes);
            Debug.WriteLine(response.StatusCode + " - " + responseString);

            return responseString;
        }

    }
}
