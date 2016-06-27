using System;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;

namespace msn2.net
{
    public class RachioIntegration
    {
        const string ApiKey = "63ff7170-d607-48b3-af8b-c289496d16aa";
        const string PersonId = "f444b2c3-479e-4c3d-938e-ac6fe7f86914";
        const string DeviceId = "5b398810-3b4d-43a1-aeb6-71e24f7cb43b";
        const string ZoneId = "f05b9731-0a8c-4afc-84b3-c0a2df1abf20";

        public static Zone GetZone()
        {
            HttpWebResponse response = Get("https://api.rach.io/1/public/zone/" + ZoneId);
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
            settings.UseSimpleDictionaryFormat = true;
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Zone));
            object zoneObject = json.ReadObject(response.GetResponseStream());
            return zoneObject as Zone;
        }

        public static void StartDrip(TimeSpan duration)
        {
            string content = "{ \"id\" : \"" + ZoneId + "\", \"duration\" : " + duration.TotalSeconds.ToString() + " }";
            Put("https://api.rach.io/1/public/zone/start", content);
        }

        public static void StopDrip()
        {
            string content = "{ \"id\" : \"" + DeviceId + "\" }";
            Put("https://api.rach.io/1/public/device/stop_water", content);
        }

        static HttpWebResponse Get(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "GET";
            req.Proxy = null;
            req.ContentType = "application/jason";

            req.Headers.Add("Authorization", "Bearer " + ApiKey);

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            return response;
        }

        static string Put(string url, string content)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "PUT";
            req.Proxy = null;
            req.ContentType = "application/jason";

            req.Headers.Add("Authorization", "Bearer " + ApiKey);

            byte[] byteArray = Encoding.UTF8.GetBytes(content);

            req.ContentLength = byteArray.Length;

            using (var stream = req.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseString = reader.ReadToEnd();

                return responseString;
            }
        }

    }
}

