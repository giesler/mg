using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace msn2.net
{
    public class NetatmoIntegration
    {
        private OathToken token = null;

        public static DateTime GetDate(double milliseconds)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime dt = epoch.AddSeconds(milliseconds).ToLocalTime();
            return dt;
        }

        public static float GetFahrenheit(float celsius)
        {
            float result = (celsius * 9 / 5) + 32;
            return result;
        }

        public void Init()
        {
            this.GetToken();
        }

        public Device GetWeatherData()
        {
            if (this.token == null)
            {
                GetToken();
            }

            HttpWebRequest deviceRequest = (HttpWebRequest)HttpWebRequest.Create("https://api.netatmo.net/api/getstationsdata?access_token=" + token.AccessToken);
            HttpWebResponse response = (HttpWebResponse)deviceRequest.GetResponse();

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(DeviceListResponse));
            object obj2 = json.ReadObject(response.GetResponseStream());
            DeviceListResponse data = obj2 as DeviceListResponse;
            
            if (data.Devices.Devices.Length > 0)
            {
                return data.Devices.Devices[0];
            }

            return null;
        }

        private void GetToken()
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://api.netatmo.net/oauth2/token");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            webRequest.Method = "POST";

            StringBuilder sb = new StringBuilder();
            sb.Append("grant_type=password");
            sb.Append("&client_id=5696fe6a49c75f93aee3507a");
            sb.Append("&client_secret=qptOodmtxf2rHwGza2X1A9NPeTJxSFHLp27SiT6EkRSSB");
            sb.Append("&username=giesler@live.com");
            sb.Append("&password=sweet2Go");

            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            webRequest.ContentLength = byteArray.Length;
            Stream stream = webRequest.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            HttpWebResponse r = (HttpWebResponse)webRequest.GetResponse();

            DataContractJsonSerializer j = new DataContractJsonSerializer(typeof(OathToken));
            object obj = j.ReadObject(r.GetResponseStream());
            this.token = obj as OathToken;
        }
    }
}
