using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace msn2.net
{
    public class WeatherProvider
    {
        public WeatherResponse GetWeatherData()
        {
            string url = "http://api.wunderground.com/api/C2bfc4d17fa56e9e/alerts/conditions/q/pws:KWAKIRKL79.json";
            HttpWebResponse response = Get(url);

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(WeatherResponse));

            object zoneObject = json.ReadObject(response.GetResponseStream());
            return zoneObject as WeatherResponse;
        }

        static HttpWebResponse Get(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "GET";
            req.Proxy = null;
            req.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            return response;
        }

    }

    [DataContract]
    public class WeatherResponse
    {
        [DataMember(Name = "current_observation")]
        public WeatherObservation WeatherObservation { get; set; }
    }

    [DataContract]
    public class WeatherObservation
    {
        [DataMember(Name = "station_id")]
        public string StationId { get; set; }

        [DataMember(Name = "weather")]
        public string Weather { get; set; }

        [DataMember(Name = "feelslike_f")]
        public float FeelLikeF { get; set; }

        [DataMember(Name = "icon_url")]
        public string IconUrl { get; set; }
    }

}