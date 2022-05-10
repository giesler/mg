using System;
using System.Collections.Generic;
using System.IO;
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
            string url = "https://api.weather.com/v2/pws/observations/current?stationId=KWABELLE229&format=json&units=e&apiKey=a138d274d4c8492cb8d274d4c8592cf3";
            HttpWebResponse response = Get(url);

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(WeatherResponse));

            object zoneObject = json.ReadObject(response.GetResponseStream());
            return zoneObject as WeatherResponse;
        }
         
        public FiveDayForecast GetForecastData()
        {
            string url = "https://api.weather.com/v3/wx/forecast/daily/5day?postalKey=98005:US&units=e&language=en-US&format=json&apiKey=a138d274d4c8492cb8d274d4c8592cf3";
            HttpWebResponse response = Get(url);
            
            var json = new DataContractJsonSerializer(typeof(FiveDayForecast));
            var obj = json.ReadObject(response.GetResponseStream());
            return obj as FiveDayForecast;
        }

        public IEnumerable<FullForecast> GetRandleForecastData()
        {
            string url = "https://api.weather.gov/gridpoints/SEW/129,68/forecast";
//            url = "https://forecast.weather.gov/MapClick.php?lat=47.6387&lon=-122.1646&unit=0&lg=english";
            HttpWebResponse response = Get(url);

            //using (var s = new StreamReader(response.GetResponseStream()))
            //{
            //    var r = s.ReadToEnd();
            //    Console.Write(r);
            //}

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Gridpoint));

            object obj = json.ReadObject(response.GetResponseStream());
            var p = obj as Gridpoint;

            var list = new List<FullForecast>();
            foreach (var period in p.Periods.ToList())
            {
                list.Add(new FullForecast
                {
                     Title = period.Name,
                     ForecastText = period.DetailedForecast,
                     High = period.Temperature.ToString(),
                     Low = period.Temperature.ToString(),
                     IconUrl = period.IconUrl
                });
            }

            return list;
        }

        static HttpWebResponse Get(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "GET";
            req.Proxy = null;
//            req.ContentType = "application/json";
            req.UserAgent = "(Giesler Development, giesler@live.com)";
            req.Accept = "application/ld+json";

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
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
    public class Gridpoint
    {
        [DataMember(Name = "periods")]
        public ForecastPeriod[] Periods { get; set; }
    }

    [DataContract]
    public class ForecastPeriod
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "isDaytime")]
        public bool IsDaytime { get; set; }

        [DataMember(Name = "temperature")]
        public int Temperature { get; set; }

        [DataMember(Name = "windSpeed")]
        public string WindSpeed { get; set; }

        [DataMember(Name = "icon")]
        public string IconUrl { get; set; }

        [DataMember(Name = "shortForecast")]
        public string ShortForecast { get; set; }

        [DataMember(Name = "detailedForecast")]
        public string DetailedForecast { get; set; }
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
                PercentagePrecip = simple.PercentagePercip,
                QuantityPercip = simple.QuantityPercip.Quantity,
                Title = txt.Title
            };
        }
            
    }

    [DataContract]
    public class FiveDayForecast
    {
        public FullForecast GetForecast(int offset)
        {
            var forecast = new FullForecast
            {
                ForecastText = Narrative[offset],
                High = MaxTemperature[offset] != null ? MaxTemperature[offset] : "",
                Low = MinTemperature[offset] != null ? MinTemperature[offset] : "",
                PercentagePrecip = PartialForecast[0].PrecipitationChance[offset * 2] != null ? PartialForecast[0].PrecipitationChance[offset * 2] : PartialForecast[0].PrecipitationChance[offset * 2 + 1],
                Title = DayOfWeek[offset],
                QuantityPercip = QuantityOfPreceipitation[offset]
            };

            if (PartialForecast[0].IconCode[offset * 2].HasValue)
            {
                forecast.IconUrl = "https://icons.wxug.com/i/c/v4/" + PartialForecast[0].IconCode[offset * 2].ToString() + ".svg";
            }
            else
            {
                forecast.IconUrl = "https://icons.wxug.com/i/c/v4/" + PartialForecast[0].IconCode[offset * 2 + 1].ToString() + ".svg";
            }

            return forecast;
        }

        [DataMember(Name = "dayOfWeek")]
        public string[] DayOfWeek { get; set; }

        [DataMember(Name = "narrative")]
        public string[] Narrative { get; set; }

        [DataMember(Name = "qpf")]
        public string[] QuantityOfPreceipitation { get; set; }

        [DataMember(Name = "temperatureMax")]
        public string[] MaxTemperature { get; set; }

        [DataMember(Name = "temperatureMin")]
        public string[] MinTemperature { get; set; }

        [DataMember(Name = "daypart")]
        public FiveDayPartialForecast[] PartialForecast {get;set;}
        
    }

    [DataContract]
    public class FiveDayPartialForecast
    {
        [DataMember(Name = "cloudCover")]
        public string[] CloudCoverPercentage { get; set; }

        [DataMember(Name = "dayOrNight")]
        public string[] DayOrNight { get; set; }

        [DataMember(Name = "daypartName")]
        public string[] Name { get; set; }

        [DataMember(Name = "iconCode")]
        public int?[] IconCode { get; set; }

        [DataMember(Name = "narrative")]
        public string[] Narrative { get; set; }

        [DataMember(Name = "precipChance")]
        public string[] PrecipitationChance { get; set; }

        [DataMember(Name="qpf")]
        public string[] PreceipitationQuantity { get; set; }

        [DataMember(Name="temperature")]
        public string[] Temperature { get; set; }

        [DataMember(Name = "windPhrase")]
        public string[] WindPhrase { get; set; }
    }

    public class FullForecast
    {
        public string IconUrl { get; set; }
        public string Title { get; set; }
        public string ForecastText { get; set; }
        public string PercentagePrecip { get; set; }

        public string High { get; set; }

        public string Low { get; set; }

        public string Conditions { get; set; }

        public string QuantityPercip { get; set; }

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
        public string PercentagePercip { get; set; }
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
        public string PercentagePercip { get; set; }

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
        public string Degrees { get; set; }

        public override string ToString()
        {
            return this.Degrees.ToString();
        }
    }

    [DataContract]
    public class PercipQuantity
    {
        [DataMember(Name ="in")]
        public string Quantity { get; set; }

        public override string ToString()
        {
            return this.Quantity.ToString();
        }
    }
}