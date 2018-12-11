using msn2.net;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web;

public partial class _Default : System.Web.UI.Page
{
    const string WeatherCacheKey = "weather";
    const string DeviceCacheKey = "device";
    const string ForecastCacheKey = "forecast";
    const string RandleCacheKey = "randle";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Debugger.IsAttached)
        {
            HttpCookie cookie = Request.Cookies["Login"];
            if (cookie == null || cookie.Value != "1")
            {
                Response.Redirect("http://login.ms2n.net/?r=http://home.ms2n.net/");
            }
        }

        this.LoadData();
    }

    void LoadData()
    {
        try
        {
            TryLoadData();
        }
        catch (Exception e)
        {
            errorPanel.Visible = true;
            weatherPanel.Visible = false;
            error.Text = "Error loading weather and device data: " + e.Message;
            error.ToolTip = e.StackTrace;
        }
    }

    void TryLoadData()
        { 
        NetatmoIntegration netatmo = new NetatmoIntegration();
        netatmo.Init();

        WeatherProvider weather = new WeatherProvider();


        var weatherData = HttpContext.Current.Cache.Get(WeatherCacheKey) as WeatherResponse;
        if (weatherData == null)
        {
            weatherData = weather.GetWeatherData();
            HttpContext.Current.Cache.Add(WeatherCacheKey, weatherData, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }

        var deviceData = HttpContext.Current.Cache.Get(DeviceCacheKey) as Device;
        if (deviceData == null)
        {
            deviceData = netatmo.GetWeatherData();
            HttpContext.Current.Cache.Add(DeviceCacheKey, deviceData, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }

        var forecastData = HttpContext.Current.Cache.Get(ForecastCacheKey) as ForecastResponse;
        if (forecastData == null)
        {
            forecastData = weather.GetForecastData();
            HttpContext.Current.Cache.Add(ForecastCacheKey, forecastData, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }

        var randleData = HttpContext.Current.Cache.Get(RandleCacheKey) as ForecastResponse;
        if (randleData == null)
        {
            randleData = weather.GetRandleForecastData();
            HttpContext.Current.Cache.Add(RandleCacheKey, randleData, null, DateTime.Now.AddMinutes(90), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }


        Module outside = deviceData.Modules.First(i => i.ModuleName == "Outdoor");
        float outsideF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.Temperature);
        this.outsideCurrent.Text = ((int)outsideF).ToString("0");

        outsideImage.ImageUrl = weatherData.WeatherObservation.IconUrl;
        outsideImage.AlternateText = weatherData.WeatherObservation.Weather;

        //        this.outsideTrend.Text = GetTrend(outside.DashboardData).Trim();

        float outsideMaxF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.MaxTemp);
        this.outsideHigh.Text = ((int)outsideMaxF).ToString("0");

        float outsideMinF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.MinTemp);
        this.outsideLow.Text = ((int)outsideMinF).ToString("0");
        //this.outsideHumidity.Text = outside.DashboardData.Humidity.ToString();

        float insideF = NetatmoIntegration.GetFahrenheit(deviceData.DashboardData.Temperature);
        this.mediaRoomCurrent.Text = ((int)insideF).ToString("0");
        //this.insideTrend.Text = GetTrend(data.DashboardData).Trim();

        //this.insideHumidity.Text = data.DashboardData.Humidity.ToString();



        Module bedroom = deviceData.Modules.First(i => i.ModuleName == "Bedroom");
        float bedroomF = NetatmoIntegration.GetFahrenheit(bedroom.DashboardData.Temperature);
        this.bedroomCurrent.Text = ((int)bedroomF).ToString("0");

        //        this.bedroomTrend.Text = GetTrend(bedroom.DashboardData).Trim();

        //this.outsideHumidity.Text = outside.DashboardData.Humidity.ToString();

        var day = DateTime.Now.DayOfWeek;

        var day0 = forecastData.GetForecast(day);
        this.day0icon.ImageUrl = day0.IconUrl;
        this.day0icon.AlternateText = day0.ForecastText;
        this.day0hi.Text = day0.High.ToString();
        this.day0low.Text = day0.Low.ToString();
        this.day0pop.Text = day0.PercentagePrecip.ToString() + "%";
        this.day0precip.Text = Precip(day0.QuantityPercip) + "\"";

        day = NextDay(day);

        var day1 = forecastData.GetForecast(day);
        this.day1icon.ImageUrl = day1.IconUrl;
        this.day1icon.AlternateText = day1.ForecastText;
        this.day1hi.Text = day1.High.ToString();
        this.day1low.Text = day1.Low.ToString();
        this.day1pop.Text = day1.PercentagePrecip.ToString() + "%";
        this.day1precip.Text = Precip(day1.QuantityPercip) + "\"";

        day = NextDay(day);

        var day2 = forecastData.GetForecast(day);
        this.day2icon.ImageUrl = day2.IconUrl;
        this.day2icon.AlternateText = day2.ForecastText;
        this.day2hi.Text = day2.High.ToString();
        this.day2low.Text = day2.Low.ToString();
        this.day2pop.Text = day2.PercentagePrecip.ToString() + "%";
        this.day2precip.Text = Precip(day2.QuantityPercip) + "\"";
        this.day2Label.Text = day2.Title.ToLower();

        var randleDay0 = DayOfWeek.Saturday;
        var randleDay1 = DayOfWeek.Sunday;

        bool isWeekend = false;
        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
        {
            // use next two forecast days
            this.randleHeader.Text = "this weekend in randle";
            isWeekend = true;

            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                randleDay0 = DayOfWeek.Sunday;
                randleDay1 = DayOfWeek.Monday;
            }
        }
        else
        {
            this.randleHeader.Text = "next weekend in randle";
        }

        var rDay0 = randleData.GetForecast(randleDay0);
        this.randleDay0Icon.ImageUrl = rDay0.IconUrl;
        this.randleDay0Icon.AlternateText = rDay0.ForecastText;
        this.randleDay0Hi.Text = rDay0.High.ToString();
        this.randleDay0Low.Text = rDay0.Low.ToString();
        this.randleDay0pop.Text = rDay0.PercentagePrecip.ToString() + "%";
        this.randleDay0precip.Text = Precip(rDay0.QuantityPercip) + "\"";
        this.randleDay0Name.Text = isWeekend ? "today" : rDay0.Title.ToLower();

        var rDay1 = randleData.GetForecast(randleDay1);
        this.randleDay1Icon.ImageUrl = rDay1.IconUrl;
        this.randleDay1Icon.AlternateText = rDay1.ForecastText;
        this.randleDay1Hi.Text = rDay1.High.ToString();
        this.randleDay1Low.Text = rDay1.Low.ToString();
        this.randleDay1pop.Text = rDay1.PercentagePrecip.ToString() + "%";
        this.randleDay1precip.Text = Precip(rDay1.QuantityPercip) + "\"";
        this.randleDay1Name.Text = rDay1.Title.ToLower();

    }

    string Precip(decimal val)
    {
        if ((double)val < 0.01d)
        {
            return "0";
        }
        else
        {
            return val.ToString();
        }
    }

    DayOfWeek NextDay(DayOfWeek current)
    {
        if (current == DayOfWeek.Sunday)
        {
            return DayOfWeek.Monday;
        }
        else if (current == DayOfWeek.Monday)
        {
            return DayOfWeek.Tuesday;
        }
        else if (current == DayOfWeek.Tuesday)
        {
            return DayOfWeek.Wednesday;
        }
        else if (current == DayOfWeek.Wednesday)
        {
            return DayOfWeek.Thursday;
        }
        else if (current == DayOfWeek.Thursday)
        {
            return DayOfWeek.Friday;
        }
        else if (current == DayOfWeek.Saturday)
        {
            return DayOfWeek.Sunday;
        }

        return current;
    }
}