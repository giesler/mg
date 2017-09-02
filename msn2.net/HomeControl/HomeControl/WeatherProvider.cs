using System;
using System.Linq;
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

        public ForecastResponse GetForecastData()
        {
            string url = "http://api.wunderground.com/api/c2bfc4d17fa56e9e/forecast/q/WA/Kirkland.json";
            HttpWebResponse response = Get(url);

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ForecastResponse));

            object obj = json.ReadObject(response.GetResponseStream());
            return obj as ForecastResponse;
        }

        public ForecastResponse GetRandleForecastData()
        {
            string url = "http://api.wunderground.com/api/c2bfc4d17fa56e9e/forecast10day/q/WA/Randle.json";
            HttpWebResponse response = Get(url);

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ForecastResponse));

            object obj = json.ReadObject(response.GetResponseStream());
            return obj as ForecastResponse;
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

    [DataContract]
    public class ForecastResponse
    {
        [DataMember(Name = "forecast")]
        public ForecastText Forecast { get; set; }

        public FullForecast GetForecast(DayOfWeek day)
        {
            var txt = Forecast.ForecastDay.Forecasts.ToList().First(i => i.Title == day.ToString());
            var simple = Forecast.SimpleForecastDay.SimpleForecastDays.ToList().First(i => i.Date.DayOfWeek == day);

            return new FullForecast
            {
                ForecastText = txt.ForecastText,
                Conditions = simple.Conditions,
                IconUrl = simple.IconUrl,
                High = simple.High.Degrees,
                Low = simple.Low.Degrees,
                PercentagePrecip = simple.PercentagePercip,
                QuantityPercip = simple.QuantityPercip.Quantity,
                Title = txt.Title
            };
        }
            
    }

    public class FullForecast
    {
        public string IconUrl { get; set; }
        public string Title { get; set; }
        public string ForecastText { get; set; }
        public int PercentagePrecip { get; set; }

        public int High { get; set; }

        public int Low { get; set; }

        public string Conditions { get; set; }

        public decimal QuantityPercip { get; set; }

    }

    [DataContract]
    public class ForecastText
    {
        [DataMember(Name = "txt_forecast")]
        public ForecastDay ForecastDay { get; set; }

        [DataMember(Name = "simpleforecast")]
        public SimpleForecastDay SimpleForecastDay { get; set; }
    }

    [DataContract]
    public class ForecastDay
    {
        [DataMember(Name = "date")]
        public string DateString { get; set; }

        [DataMember(Name = "forecastday")]
        public ForecastDayItem[] Forecasts { get; set; }
    }

    [DataContract]
    public class ForecastDayItem
    {
        [DataMember(Name = "period")]
        public int Period { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "icon_url")]
        public string IconUrl { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "fcttext")]
        public string ForecastText { get; set; }

        [DataMember(Name = "pop")]
        public int PercentagePercip { get; set; }
    }

    [DataContract]
    public class SimpleForecastDay
    {
        [DataMember(Name = "forecastday")]
        public SimpleForecastDayItem[] SimpleForecastDays { get; set; }
    }

    [DataContract]
    public class SimpleForecastDayItem
    {
        [DataMember(Name = "period")]
        public int Period { get; set; }

        [DataMember(Name ="date")]
        public DateItem Date {get;set;}

        [DataMember(Name ="high")]
        public TemperatureItem High { get; set; }

        [DataMember(Name ="low")]
        public TemperatureItem Low { get; set; }

        [DataMember(Name ="conditions")]
        public string Conditions { get; set; }

        [DataMember(Name ="icon_url")]
        public string IconUrl { get; set; }

        [DataMember(Name ="pop")]
        public int PercentagePercip { get; set; }

        [DataMember(Name ="qpf_allday")]
        public PercipQuantity QuantityPercip { get; set; }

    }

    [DataContract]
    public class DateItem
    {
        [DataMember(Name = "weekday")]
        public string DayOfWeekString { get; set; }

        public DayOfWeek DayOfWeek
        {
            get
            {
                return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), DayOfWeekString);
            }
        }
    }

    [DataContract]
    public class TemperatureItem
    {
        [DataMember(Name ="fahrenheit")]
        public int Degrees { get; set; }

        public override string ToString()
        {
            return this.Degrees.ToString();
        }
    }

    [DataContract]
    public class PercipQuantity
    {
        [DataMember(Name ="in")]
        public decimal Quantity { get; set; }

        public override string ToString()
        {
            return this.Quantity.ToString();
        }
    }
}