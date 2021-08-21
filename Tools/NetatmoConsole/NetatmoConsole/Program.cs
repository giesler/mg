using msn2.net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NetatmoConsole
{
    class Program
    {
        static void Main(string[] args)
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
            
            string encoded = System.Web.HttpUtility.UrlEncode(sb.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            webRequest.ContentLength = byteArray.Length;
            Stream stream = webRequest.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();
            
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(OathToken));
            object obj = json.ReadObject(response.GetResponseStream());
            OathToken token = obj as OathToken;

            HttpWebRequest deviceRequest = (HttpWebRequest)HttpWebRequest.Create("https://api.netatmo.net/api/getstationsdata?access_token=" + token.AccessToken);
            response = (HttpWebResponse)deviceRequest.GetResponse();

            json = new DataContractJsonSerializer(typeof(DeviceListResponse));
            object obj2 = json.ReadObject(response.GetResponseStream());

                      Console.Write(obj2.ToString());
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseString = reader.ReadToEnd();

                Console.WriteLine(responseString);
            }


            Console.Read();
        }
    }
}
