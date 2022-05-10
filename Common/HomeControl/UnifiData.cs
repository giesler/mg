using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace msn2.net
{
    public class UnifiData
    {
        static UnifiData()
        {
            ServicePointManager.CertificatePolicy = new TrustAllCertsPolicy();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | 
                SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        internal readonly string[] WhoClients = new string[]
        {
            "Sara - Galaxy S10",
            "Mike - Galaxy S10",
            "Neil - LG v60 ThinQ",
            "Shannon - Galaxy S9"
        };

        internal readonly CookieContainer cookieContainer = new CookieContainer();
        internal readonly string ControllerUri = "https://192.168.4.2:8443";
        internal readonly string SiteId = "default";
        internal static readonly string ControllerUserName = "ms2n";
        internal static readonly string ControllerPassword = "2008InspectionReport!";
        internal readonly LoginBody LoginBody = new LoginBody
        {
            UserName = ControllerUserName,
            Password = ControllerPassword
        };

        public List<UnifiClient> GetWhoClients()
        {
            var body = JsonConvert.SerializeObject(LoginBody);
            string content = RestRequest($"{ControllerUri}/api/login", "POST", body);

            if (content.IndexOf("ok") < 0)
            {
                throw new Exception($"Unable to connect to {ControllerUri} as {ControllerUserName}");
            }

            string allClientsRequest = RestRequest($"{ControllerUri}/api/s/{SiteId}/stat/alluser");
            string activeClientsRequest = RestRequest($"{ControllerUri}/api/s/{SiteId}/stat/sta");
            string devicesRequest = RestRequest($"{ControllerUri}/api/s/{SiteId}/stat/device");

            var regClientResponse = JsonConvert.DeserializeObject(allClientsRequest, typeof(RegisteredClientResponse)) as RegisteredClientResponse;
            var activeClientResponse = JsonConvert.DeserializeObject(activeClientsRequest, typeof(ActiveClientResponse)) as ActiveClientResponse;
            var devicesResponse = JsonConvert.DeserializeObject(devicesRequest, typeof(DeviceResponse)) as DeviceResponse;

            var list = new List<UnifiClient>();
            var currentTime = DateTime.Now.AddHours(-1);

            foreach (var client in WhoClients)
            {
                var unifiClient = new UnifiClient { Name = client, Status = "Away" };
                list.Add(unifiClient);

                var regClient = regClientResponse.Clients.FirstOrDefault(c => c.Name != null && c.Name.Equals(client, StringComparison.InvariantCultureIgnoreCase));
                var clientState = activeClientResponse.Clients.FirstOrDefault(c => c.Name != null && c.Name.Equals(client, StringComparison.InvariantCultureIgnoreCase));

                // Check if client is active
                if (regClient != null)
                {
                    unifiClient.LastSeen = ConvertUnixTime(regClient.LastSeenTimestamp);
                    if (clientState != null)
                    {
                        unifiClient.LastSeen = ConvertUnixTime(clientState.LastSeenTimestamp);
                        unifiClient.ConnectionTime = ConvertUnixTime(clientState.AssociationTime);
                    }

                    if (unifiClient.LastSeen > DateTime.Now.AddMinutes(-5))
                    {
                        unifiClient.Status = "Home";
                        unifiClient.IsHome = true;

                        var ap = devicesResponse.Devices.Where(d => d.MacAddress.Equals(clientState.APMacAddress, StringComparison.InvariantCultureIgnoreCase));
                        unifiClient.APName = ap.First().Name;
                    }
                }
            }

            return list;
        }

        private string RestRequest(string uri, string httpMethod = "GET", string body = null)
        {
            var req = (HttpWebRequest)HttpWebRequest.CreateHttp(uri);
            req.Method = httpMethod;
            req.ContentType = "application/json";
            req.CookieContainer = cookieContainer;

            if (body != null)
            {
                var bytes = Encoding.UTF8.GetBytes(body);
                using (var stream = req.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }

            var response = (HttpWebResponse)req.GetResponse();
            string content = null;
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    content = reader.ReadToEnd();
                }
            }

            return content;
        }

        DateTime ConvertUnixTime(long unixTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var temp = start.AddSeconds(unixTime).ToLocalTime();
            var offset = TimeSpan.Zero; // TimeZone.CurrentTimeZone.GetUtcOffset(temp);
            return temp.AddHours(offset.Hours);
        }
    }

    public class TrustAllCertsPolicy: ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint servicePoint, X509Certificate certificate, WebRequest webRequest, int certificateProblem)
        {
            return true;
        }
    }

    internal class LoginBody
    {
        [JsonProperty("username", Required = Required.Always)]
        public string UserName { get; set; }

        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; }
    }

    public class UnifiClient
    {        
        public string Name { get; set; }
        public string APName { get; set; }
        public string Status { get; set; }
        public bool IsHome { get; set; }
        public DateTime LastSeen { get; set; }
        public DateTime ConnectionTime { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(APName))
            {
                return this.Name;
            }

            return $"{Name} ({APName}) [{Status}]";
        }
    }

    public class ActiveClientResponse
    {
        [JsonProperty("meta")]
        public MetaDataResponse Metadata { get; set; }

        [JsonProperty("data")]
        public List<ActiveClientResponseItem> Clients { get; set; }
    }

    public class MetaDataResponse
    {
        [JsonProperty("rc")]
        public string Result { get; set; }
    }

    public class ActiveClientResponseItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("last_seen")]
        public long LastSeenTimestamp { get; set; }

        [JsonProperty("assoc_time")]
        public long AssociationTime { get; set; }

        [JsonProperty("ap_mac")]
        public string APMacAddress { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class RegisteredClientResponse
    {
        [JsonProperty("meta")]
        public MetaDataResponse Metadata { get; set; }

        [JsonProperty("data")]
        public List<RegisteredClientResponseItem> Clients { get; set; }
    }

    public class RegisteredClientResponseItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("last_seen")]
        public long LastSeenTimestamp { get; set; }

        [JsonProperty("assoc_time")]
        public long AssociationTime { get; set; }

        [JsonProperty("ap_mac")]
        public string APMacAddress { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class DeviceResponse
    {
        [JsonProperty("meta")]
        public MetaDataResponse Metadata { get; set; }

        [JsonProperty("data")]
        public List<DeviceResponseItem> Devices { get; set; }
    }

    public class DeviceResponseItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mac")]
        public string MacAddress { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }


}
