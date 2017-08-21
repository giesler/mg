using System;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;
using System.Linq;

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

        public static Event[] GetEvents(DateTime startTime, DateTime endTime)
        {
            string url = string.Format("https://api.rach.io/1/public/device/{0}/event?startTime={1}&endTime={2}", DeviceId, GetEpoch(startTime), GetEpoch(endTime));
            HttpWebResponse response = Get(url);
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Event[]));
            object jsonObject = json.ReadObject(response.GetResponseStream());
            return jsonObject as Event[];
        }

        public static HttpWebResponse Get(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "GET";
            req.Proxy = null;
            req.ContentType = "application/json";

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

        public static DateTime GetDate(double milliseconds)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime dt = epoch.AddMilliseconds(milliseconds).ToLocalTime();
            return dt;
        }

        public static double GetEpoch(DateTime dt)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = dt - epoch;
            return ts.TotalMilliseconds;
        }

        public static string GetCurrentStatus()
        {
            string status = "Unknown";

            var items = RachioIntegration.GetEvents(DateTime.Now.Date.AddDays(0), DateTime.Now.Date.AddDays(1));

            bool skipTotal = false;
            if (items.Length > 0)
            {
                Event lastEvent = items[0];
                if (lastEvent.Category == "SCHEDULE" && lastEvent.Type == "ZONE_STATUS")
                {
                    EventData statusData = lastEvent.EventData.FirstOrDefault(i => i.Key == "status");
                    if (statusData.Value == "started")
                    {
                        // Zone currently running
                        EventData durationData = lastEvent.EventData.FirstOrDefault(i => i.Key == "duration");
                        int duration = int.Parse(durationData.Value);
                        DateTime endTime = lastEvent.CreateDate.AddMinutes(duration);
                        status = "Running until " + endTime.ToString("h:mm");
                        skipTotal = true;
                    }
                }

                if (lastEvent.Category == "WEATHER" && lastEvent.Type == "WEATHER_INTELLIGENCE")
                {
                    status = "Watering skipped today";
                    skipTotal = true;
                }

                if (!skipTotal)
                {
                    int durationSecs = 0;
                    foreach (Event e in items)
                    {
                        if (e.Category == "SCHEDULE" && e.Type == "ZONE_STATUS")
                        {
                            EventData st = e.EventData.FirstOrDefault(i => i.Key == "status");
                            if (st != null && st.Value == "ZONE_COMPLETED")
                            {
                                EventData d = e.EventData.FirstOrDefault(i => i.Key == "durationInSeconds");
                                durationSecs += int.Parse(d.Value);
                            }
                        }
                    }

                    TimeSpan ts = new TimeSpan(0, 0, durationSecs);
                    status = string.Format("Watered for {0:0} mins", ts.TotalMinutes);
                }
            }

            return status;
        }
    }
}

