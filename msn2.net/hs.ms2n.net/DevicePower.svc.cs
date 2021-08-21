using msn2.net.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace HomeServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DevicePower : IDevicePower
    {
        public void Resume(string accessKey, string macAddress)
        {
            if (!string.Equals(accessKey, ConfigurationManager.AppSettings["accessKey"], StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Access denied.");
            }

            PowerManager.WakeUp(macAddress);
        }

        public void Suspend(string accessKey, string hostNameOrIPAddress)
        {
            if (!string.Equals(accessKey, ConfigurationManager.AppSettings["accessKey"], StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Access denied.");
            }

            HttpWebRequest wr = WebRequest.CreateHttp(string.Format("http://{0}:4646/Suspend", hostNameOrIPAddress));
            wr.Timeout = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;

            using (WebResponse response = wr.GetResponse())
            {
                Trace.Write(response.ToString());
            }
        }
    }
}
