using msn2.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HomeDash : System.Web.UI.Page
{

    const string WeatherCacheKey = "weather";
    const string DeviceCacheKey = "device";
    
    protected void Page_Load(object sender, EventArgs e)
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


        Module outside = deviceData.Modules.First(i => i.ModuleName == "Outdoor");
        float outsideF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.Temperature);
        this.outsideCurrent.Text = ((int)outsideF).ToString("0");
        this.outsideCurrentDecimal.Text = "." + (outsideF % 1.0 * 10.0).ToString("0");
        double outsideFrac = outsideF - ((float)((int)outsideF) * 1.0);
        if (outsideFrac < 0.14)
        {
            this.outsideCurrentDecimal.Visible = false;
        }

        outsideImage.ImageUrl = weatherData.WeatherObservation.IconUrl;
        outsideImage.AlternateText = weatherData.WeatherObservation.Weather;

//        this.outsideTrend.Text = GetTrend(outside.DashboardData).Trim();

        float outsideMaxF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.MaxTemp);
        this.outsideHigh.Text = ((int)outsideMaxF).ToString("0");
        this.outsideHighDecimal.Text = (outsideMaxF % 1.0 * 10.0).ToString("0");

        float outsideMinF = NetatmoIntegration.GetFahrenheit(outside.DashboardData.MinTemp);
        this.outsideLow.Text = ((int)outsideMinF).ToString("0");
        this.outsideLowDecimal.Text = (outsideMinF % 1.0 * 10.0).ToString("0");
        //this.outsideHumidity.Text = outside.DashboardData.Humidity.ToString();

        float insideF = NetatmoIntegration.GetFahrenheit(deviceData.DashboardData.Temperature);
        this.mediaRoomCurrent.Text = ((int)insideF).ToString("0");
        this.mediaRoomCurrentDecimal.Text = "." + (insideF % 1.0 * 10.0).ToString("0");
        double insideFrac = insideF - ((float)((int)insideF) * 1.0);
        if (insideFrac < 0.14)
        {
            this.mediaRoomCurrentDecimal.Visible = false;
        }
        //this.insideTrend.Text = GetTrend(data.DashboardData).Trim();

        float insideMaxF = NetatmoIntegration.GetFahrenheit(deviceData.DashboardData.MaxTemp);
        this.mediaRoomHigh.Text = ((int)insideMaxF).ToString("0");
        this.mediaRoomHighDecimal.Text = (insideMaxF % 1.0 * 10.0).ToString("0");

        float insideMinF = NetatmoIntegration.GetFahrenheit(deviceData.DashboardData.MinTemp);
        this.mediaRoomLow.Text = ((int)insideMinF).ToString("0");
        this.mediaRoomLowDecimal.Text = (insideMinF % 1.0 * 10.0).ToString("0");
        //this.insideHumidity.Text = data.DashboardData.Humidity.ToString();
        


        Module bedroom = deviceData.Modules.First(i => i.ModuleName == "Bedroom");
        float bedroomF = NetatmoIntegration.GetFahrenheit(bedroom.DashboardData.Temperature);
        this.bedroomCurrent.Text = ((int)bedroomF).ToString("0");
        this.bedroomCurrentDecimal.Text = "." + (bedroomF % 1.0 * 10.0).ToString("0");
        double bedroomFrac = bedroomF - ((float)((int)bedroomF) * 1.0);
        if (bedroomFrac < 0.14)
        {
            this.bedroomCurrentDecimal.Visible = false;
        }

        //        this.bedroomTrend.Text = GetTrend(bedroom.DashboardData).Trim();

        float bedroomMaxF = NetatmoIntegration.GetFahrenheit(bedroom.DashboardData.MaxTemp);
        this.bedroomHigh.Text = ((int)bedroomMaxF).ToString("0");
        this.bedroomHighDecimal.Text = (bedroomMaxF % 1.0 * 10.0).ToString("0");

        float bedroomMinF = NetatmoIntegration.GetFahrenheit(bedroom.DashboardData.MinTemp);
        this.bedroomLow.Text = ((int)bedroomMinF).ToString("0");
        this.bedroomLowDecimal.Text = (bedroomMinF % 1.0 * 10.0).ToString("0");
        //this.outsideHumidity.Text = outside.DashboardData.Humidity.ToString();



    }
}