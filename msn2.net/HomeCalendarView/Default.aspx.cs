using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Web.Caching;
using System.Drawing;

namespace HomeCalendarView
{
    public class LocationData
    {
        public string Name { get; set; }
        public decimal Lattitude { get; set; }
        public decimal Longitude { get; set; }
        public string NoaaCurrentConditionsLocation { get; set; }
        public string CurrentConditionsUrl { get; set; }
        public string NoaaCurrentAlertsLocation { get; set; }
    }

    public partial class _Default : System.Web.UI.Page
    {
        private LocationData currentLocation = null;
        private List<LocationData> locations = new List<LocationData>();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LocationData kirkland = new LocationData { Name = "KIRKLAND" };
            kirkland.Lattitude = 47.67881M;
            kirkland.Longitude = -122.20724M;
            kirkland.NoaaCurrentConditionsLocation = "KSEA";
            kirkland.NoaaCurrentAlertsLocation = "WAZ505";
            kirkland.CurrentConditionsUrl = "http://www.nws.noaa.gov/data/current_obs/KSEA.xml";
            this.locations.Add(kirkland);

            LocationData packwood = new LocationData { Name = "PACKWOOD" };
            packwood.Lattitude = 46.4797M;
            packwood.Longitude = -121.822M;
            packwood.NoaaCurrentConditionsLocation = "WAZ519";
            packwood.NoaaCurrentAlertsLocation = "WAZ519";
            packwood.CurrentConditionsUrl = "http://api.wunderground.com/weatherstation/WXCurrentObXML.asp?ID=MOHAW1";
            this.locations.Add(packwood);

            this.currentLocation = kirkland;
            
            this.lastUpdateTime.Text = "LAST UPDATE: " + DateTime.Now.ToShortTimeString();
            this.dataLoadTimer.Enabled = false;
            this.dataLoadTimer.Tick += new EventHandler<EventArgs>(dataLoadTimer_Tick);
            this.closeWarning.Click += new EventHandler(closeWarning_Click);
            this.refreshTimer.Tick += new EventHandler<EventArgs>(refreshTimer_Tick);
            this.refreshTimer.Interval = 60 * 1000 * new Random().Next(18, 23);

            this.todayLabel.Text = DateTime.Now.ToString("dddd MMMM d").ToUpper(); 
            this.todayDateLabel.Text = "LAST UPDATE " + DateTime.Now.ToShortTimeString();
            //this.day1Label.Text = DateTime.Now.AddDays(1).ToString("dddd").ToLower();
            this.day2Label.Text = DateTime.Now.AddDays(2).ToString("dddd").ToUpper();
            this.day3Label.Text = DateTime.Now.AddDays(3).ToString("dddd").ToUpper();
            this.day4Label.Text = DateTime.Now.AddDays(4).ToString("dddd").ToUpper();

            if (this.IsPostBack == false)
            {
                this.onCityClick(this.selectKirkland, EventArgs.Empty);
            }

            Trace.Write("Current weather");
            DisplayCurrent();

            Trace.Write("Forecast");
            DisplayForecast();

            Trace.Write("Forecast details");
            DisplayForecastDetails();

            Trace.Write("Alerts");
            DisplayWeatherAlerts();

            Trace.Write("Done");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.reenableTimer = this.dataLoadTimer.Enabled;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Session["c"] != null)
            {
                this.currentLocation = this.locations[int.Parse(Session["c"].ToString())];
            }
        }

        bool reenableTimer = false;

        void dataLoadTimer_Tick(object sender, EventArgs e)
        {
            Trace.Write("Timer");
            this.dataLoadTimer.Enabled = this.reenableTimer;
        }

        void refreshTimer_Tick(object sender, EventArgs e)
        {
            this.refreshTimer.Enabled = true;
            this.lastUpdateTime.Text = DateTime.Now.ToShortTimeString();
        }

        void closeWarning_Click(object sender, EventArgs e)
        {
            this.warningPanel.Visible = false;
            this.forecastPanel.Visible = true;
        }

        string CacheName(string key)
        {
            return this.currentLocation.Name + "." + key;
        }

        private void LoadForecast(object sender)
        {
            System.Web.Caching.Cache cache = (System.Web.Caching.Cache)sender;

            try
            {
                GovWeatherService.ndfdXML weatherService = new GovWeatherService.ndfdXML();
                string xml = weatherService.NDFDgenByDay(this.currentLocation.Lattitude,
                    this.currentLocation.Longitude, DateTime.Now.Date, "7", "e", "12 hourly");
                cache.Add(this.CacheName("forecastCache"), xml, null, DateTime.Now.AddMinutes(20),
                    TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            catch (Exception ex)
            {
                Trace.Write(ex.ToString());
            }
            finally
            {
                cache.Remove(this.CacheName("LoadForecast"));
            }
        }

        private void DisplayForecast()
        {
            string xml = null;
            object cacheItem = HttpContext.Current.Cache[this.CacheName("forecastCache")];
            if (cacheItem == null)
            {
                this.dataLoadTimer.Enabled = true;

                if (base.Cache[this.CacheName("LoadForecast")] == null)
                {
                    base.Cache.Add(this.CacheName("LoadForecast"), DateTime.Now, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadForecast), HttpContext.Current.Cache);
                }
            }
            else
            {
                xml = cacheItem.ToString();
            }

            if (xml != null)
            {
                this.todayForeastDiv.Visible = true;
                this.todayForecastLoading.Visible = false;

                this.todayHigh.Visible = true;
                this.todayLow.Visible = true;
                this.day1High.Visible = true;
                this.day1Low.Visible = true;
                this.day2High.Visible = true;
                this.day2Low.Visible = true;
                this.day3High.Visible = true;
                this.day3Low.Visible = true;
                this.day4High.Visible = true;
                this.day4Low.Visible = true;

                XmlDocument weatherXml = new XmlDocument();
                weatherXml.LoadXml(xml);

                bool night = DateTime.Now.Hour > 15;

                int offset = night ? -1 : 0;
                this.todayHigh.Visible = !night;
                this.todayHighDiv.Visible = !night;
                this.todayForecastInnerDiv.Style[HtmlTextWriterStyle.Width] = night ? "80px" : "160px";
                this.todayForecastLabel.Text = night ? "TONIGHT" : "FORECAST";

                foreach (XmlNode tempNode in weatherXml.DocumentElement.SelectNodes("data/parameters/temperature"))
                {
                    switch (tempNode.Attributes["type"].Value)
                    {
                        case "maximum":
                            if (offset == 0)
                            {
                                this.todayHigh.Temperature = tempNode.ChildNodes[1].InnerText + "&deg;";
                            }
                            this.day1High.Temperature = tempNode.ChildNodes[2 + offset].InnerText + "&deg;";
                            this.day2High.Temperature = tempNode.ChildNodes[3 + offset].InnerText + "&deg;";
                            this.day3High.Temperature = tempNode.ChildNodes[4 + offset].InnerText + "&deg;";
                            this.day4High.Temperature = tempNode.ChildNodes[5 + offset].InnerText + "&deg;";
                            break;

                        case "minimum":
                            this.todayLow.Temperature = tempNode.ChildNodes[1].InnerText + "&deg;";
                            this.day1Low.Temperature = tempNode.ChildNodes[2].InnerText + "&deg;";
                            this.day2Low.Temperature = tempNode.ChildNodes[3].InnerText + "&deg;";
                            this.day3Low.Temperature = tempNode.ChildNodes[4].InnerText + "&deg;";
                            this.day4Low.Temperature = tempNode.ChildNodes[5].InnerText + "&deg;";
                            break;
                    }
                }

                XmlNode precipNode = weatherXml.DocumentElement.SelectSingleNode("data/parameters/probability-of-precipitation");
                this.todayHigh.Precipitation = precipNode.ChildNodes[1].InnerText + "%";
                this.todayLow.Precipitation = precipNode.ChildNodes[2].InnerText + "%";
                this.day1High.Precipitation = precipNode.ChildNodes[3 + offset].InnerText + "%";
                this.day1Low.Precipitation = precipNode.ChildNodes[4 + offset].InnerText + "%";
                this.day2High.Precipitation = precipNode.ChildNodes[5 + offset].InnerText + "%";
                this.day2Low.Precipitation = precipNode.ChildNodes[6 + offset].InnerText + "%";
                this.day3High.Precipitation = precipNode.ChildNodes[7 + offset].InnerText + "%";
                this.day3Low.Precipitation = precipNode.ChildNodes[8 + offset].InnerText + "%";
                this.day4High.Precipitation = precipNode.ChildNodes[9 + offset].InnerText + "%";
                this.day4Low.Precipitation = precipNode.ChildNodes[10 + offset].InnerText + "%";

                XmlNode descriptionNode = weatherXml.DocumentElement.SelectSingleNode("data/parameters/weather");
                XmlNode conditionsNode = weatherXml.DocumentElement.SelectSingleNode("data/parameters/conditions-icon");

                SetImageProperties(this.todayHigh, conditionsNode.ChildNodes[1]);
                SetImageProperties(this.todayLow, conditionsNode.ChildNodes[2]);
                SetImageProperties(this.day1High, conditionsNode.ChildNodes[3]);
                SetImageProperties(this.day1Low, conditionsNode.ChildNodes[4]);
                SetImageProperties(this.day2High, conditionsNode.ChildNodes[5]);
                SetImageProperties(this.day2Low, conditionsNode.ChildNodes[6]);
                SetImageProperties(this.day3High, conditionsNode.ChildNodes[7]);
                SetImageProperties(this.day3Low, conditionsNode.ChildNodes[8]);
                SetImageProperties(this.day4High, conditionsNode.ChildNodes[9]);
                SetImageProperties(this.day4Low, conditionsNode.ChildNodes[10]);
            }
            else
            {
                this.todayForecastLoading.Visible = true;

                this.todayHigh.Visible = false;
                this.todayLow.Visible = false;
                this.day1High.Visible = false;
                this.day1Low.Visible = false;
                this.day2High.Visible = false;
                this.day2Low.Visible = false;
                this.day3High.Visible = false;
                this.day3Low.Visible = false;
                this.day4High.Visible = false;
                this.day4Low.Visible = false;
            }
        }

        void SetImageProperties(ForecastItem item, XmlNode node)
        {
            if (node != null)
            {
                item.ImageUrl = node.InnerText;

                XmlAttribute att = node.Attributes["weather-sumary"];
                if (att != null)
                {
                    item.ImageAltText = att.Value;
                }
            }
        }

        private void LoadForecastDetails(object sender)
        {
            System.Web.Caching.Cache cache = (System.Web.Caching.Cache)sender;

            try
            {
                string url = string.Format(
                    "http://forecast.weather.gov/MapClick.php?site=sew&textField1={0}&textField2={1}&smap=1&FcstType=dwml",
                    this.currentLocation.Lattitude,
                    this.currentLocation.Longitude);
                WebRequest req = WebRequest.Create(url);
                WebResponse response = req.GetResponse();
                Stream st = response.GetResponseStream();
                StreamReader sr = new StreamReader(st);
                string fileContents = sr.ReadToEnd();
                cache.Add(this.CacheName("fcast2"), fileContents, null, DateTime.Now.AddMinutes(20), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                sr.Close();
                st.Close();
                response.Close();
            }
            finally
            {
                cache.Remove(this.CacheName("LoadForecastDetails"));
            }
        }

        private void DisplayForecastDetails()
        {
            Regex forecastDescriptionRegex = new Regex(@"((<b>(?<period>(\w| )+)\: </b>)(?<fcast>.+?))<br>");

            string fileContents = null;
            object cacheItem = HttpContext.Current.Cache[this.CacheName("fcast2")];
            if (cacheItem == null)
            {
                this.dataLoadTimer.Enabled = true;

                if (base.Cache[this.CacheName("LoadForecastDetails")] == null)
                {
                    base.Cache.Add(this.CacheName("LoadForecastDetails"), DateTime.Now, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadForecastDetails), HttpContext.Current.Cache);
                }
            }
            else
            {
                fileContents = cacheItem.ToString();
            }

            if (fileContents != null)
            {
                List<FCastItem> fcastList = new List<FCastItem>();

                XDocument doc = XDocument.Parse(fileContents);
                var q = doc.Document.Descendants("data").Descendants("parameters").Descendants("wordedForecast");
                if (q != null)
                {
                    int index = 0;
                    foreach (var i in q.Descendants("text"))
                    {
                        fcastList.Add(new FCastItem { Day = index.ToString(), Forecast = i.Value });
                        index++;
                    }
                }
                
                if (fcastList.Count > 0)
                {
                    int offset = 0;
                    //if (fcastList[0].Day != "Today" && fcastList[0].Day != "This Afternoon" && fcastList[0].Day != "Late Afternoon")
/*                    {
                        offset = -1;
                        this.todayLow.ImageAltText = fcastList[0].Forecast;
                    }
  */                  //else
                    //{
                    //    this.todayHigh.ImageAltText = fcastList[0].Forecast;
                    //    this.todayLow.ImageAltText = fcastList[1].Forecast;
                    //}

                    this.todayHigh.ImageAltText = fcastList[0].Forecast;
                    this.todayLow.ImageAltText = fcastList[1].Forecast;
                    this.day1High.ImageAltText = fcastList[2 + offset].Forecast;
                    this.day1Low.ImageAltText = fcastList[3 + offset].Forecast;
                    this.day2High.ImageAltText = fcastList[4 + offset].Forecast;
                    this.day2Low.ImageAltText = fcastList[5 + offset].Forecast;
                    this.day3High.ImageAltText = fcastList[6 + offset].Forecast;
                    this.day3Low.ImageAltText = fcastList[7 + offset].Forecast;
                    this.day4High.ImageAltText = fcastList[8 + offset].Forecast;
                    this.day4Low.ImageAltText = fcastList[9 + offset].Forecast;
                }
            }
        }

        private class FCastItem
        {
            public string Day;
            public string Forecast;
        }

        //private static void GetLatLong(GovWeatherService.ndfdXML weatherService, string zip, out decimal lat, out decimal lng)
        //{
        //    string latLongList = weatherService.LatLonListZipCode(zip);

        //    XmlDocument latLongDoc = new XmlDocument();
        //    latLongDoc.LoadXml(latLongList);
        //    latLongList = latLongDoc.DocumentElement.InnerText; ;

        //    string[] latLongs = latLongList.Split(',');
        //    lat = decimal.Parse(latLongs[0]);
        //    lng = decimal.Parse(latLongs[1]);
        //}

        void LoadCurrent(object sender)
        {
            System.Web.Caching.Cache cache = (System.Web.Caching.Cache)sender;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.currentLocation.CurrentConditionsUrl);
                cache.Add(this.CacheName("current"), doc, null, DateTime.Now.AddMinutes(20), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            catch (Exception)
            { }
            finally
            {
                cache.Remove(this.CacheName("LoadCurrent"));
            }
        }

        private void DisplayCurrent()
        {
            XmlDocument doc = null;
            object cacheItem = HttpContext.Current.Cache[this.CacheName("current")];
            if (cacheItem == null)
            {
                this.dataLoadTimer.Enabled = true;

                if (base.Cache[this.CacheName("LoadCurrent")] == null)
                {
                    base.Cache.Add(this.CacheName("LoadCurrent"), DateTime.Now, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadCurrent), HttpContext.Current.Cache);
                }
            }
            else
            {
                doc = (XmlDocument)cacheItem;
            }

            if (doc == null)
            {
                this.currentLoadingMessage.Text = "loading current conditions...";
                this.currentConditionsTable.Visible = false;
                this.currentConditionsLoading.Visible = true;
            }
            else if (doc != null)
            {
                this.currentConditionsTable.Visible = true;
                this.currentConditionsLoading.Visible = false;

                decimal d = decimal.Parse(doc.DocumentElement.SelectSingleNode("temp_f").InnerText);
                this.currentTemp.Text = d.ToString("0") + "&deg;";

                string timeString = doc.DocumentElement.SelectSingleNode("observation_time_rfc822").InnerText;
                DateTime dt = DateTime.MinValue;
                if (DateTime.TryParse(timeString, out dt))
                {
                    timeString = dt.ToString();
                }
                else
                {
                    timeString = timeString.Substring(0, timeString.IndexOf("-") - 1);
                }
                DateTime updateTime = DateTime.Parse(timeString);
                this.tempUpdateTime.Text = "@" + updateTime.ToString("h:mm");

                string windDirection = doc.DocumentElement.SelectSingleNode("wind_dir").InnerText;
                string windMph = doc.DocumentElement.SelectSingleNode("wind_mph").InnerText;
                if (windMph == "NA")
                {
                    this.windLabel.Text = "calm";
                }
                else
                {
                    if (windDirection == "South")
                    {
                        windDirection = "S";
                    }
                    else if (windDirection == "North")
                    {
                        windDirection = "N";
                    }
                    else if (windDirection == "East")
                    {
                        windDirection = "E";
                    }
                    else if (windDirection == "West")
                    {
                        windDirection = "W";
                    }
                    else if (windDirection == "Southeast")
                    {
                        windDirection = "SE";
                    }
                    else if (windDirection == "Southwest")
                    {
                        windDirection = "SW";
                    }
                    else if (windDirection == "Northeast")
                    {
                        windDirection = "NE";
                    }
                    else if (windDirection == "Northwest")
                    {
                        windDirection = "NW";
                    }
                    decimal windSpeed = decimal.Parse(windMph);
                    this.windLabel.Text = windDirection + " at " + ((int)windSpeed).ToString() + " mph";

                    var q = doc.DocumentElement.SelectSingleNode("wind_gust_mph");
                    if (q != null)
                    {
                        string gusts = q.InnerText;
                        if (gusts != "NA")                        
                        {
                            if (gusts != "0" && gusts != "0.0")
                            {
                                this.windLabel.Text += ",<br />gusts&nbsp;to&nbsp;" + gusts;
                            }
                        }
                    }
                }

                XmlNode windChillNode = doc.DocumentElement.SelectSingleNode("windchill_f");
                if (windChillNode != null && windChillNode.InnerText.Length > 0)
                {
                    int wc = 0;
                    if (int.TryParse(windChillNode.InnerText, out wc) && wc < 32)
                    {
                        this.windChill.Text = "Wind chill: " + windChillNode.InnerText + "&deg;<br />";
                    }
                }
                XmlNode visNode = doc.DocumentElement.SelectSingleNode("visibility_mi");
                if (visNode != null)
                {
                    this.visibility.Text = visNode.InnerText;
                }

                if (this.visibility.Text.EndsWith(".00"))
                {
                    this.visibility.Text = this.visibility.Text.Replace(".00", "");
                }

                XmlNode conditionNode = doc.DocumentElement.SelectSingleNode("icon_url_base");
                if (conditionNode != null)
                {
                    string conditionImage = conditionNode.InnerText;
                    conditionImage += doc.DocumentElement.SelectSingleNode("icon_url_name").InnerText;
                    this.currentCondition.ImageUrl = conditionImage;
                    this.currentConditionText.Text = doc.DocumentElement.SelectSingleNode("weather").InnerText;
                    this.currentCondition.Visible = true;
                }
                else
                {
                    this.currentCondition.Visible = false;
                    this.currentConditionText.Text = "";
                }
            }
        }

        void LoadAlerts(object sender)
        {
            System.Web.Caching.Cache cache = (System.Web.Caching.Cache)sender;

            try
            {
                string url = string.Format(
                    "http://www.weather.gov/alerts-beta/wwaatmget.php?x={0}",
                    this.currentLocation.NoaaCurrentAlertsLocation);
                XElement alertFeed = XElement.Load(url);
                cache.Add(this.CacheName("alerts"), alertFeed, null, DateTime.Now.AddMinutes(20), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            finally
            {
                cache.Remove(this.CacheName("LoadAlerts"));
            }
        }

        XElement alertFeed = null;

        private void DisplayWeatherAlerts()
        {
            object cacheItem = HttpContext.Current.Cache[this.CacheName("alerts")];
            if (cacheItem == null)
            {
                this.dataLoadTimer.Enabled = true;

                if (base.Cache[this.CacheName("LoadAlerts")] == null)
                {
                    base.Cache.Add(this.CacheName("LoadAlerts"), DateTime.Now, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadAlerts), HttpContext.Current.Cache);
                }
            }
            else
            {
                alertFeed = (XElement)cacheItem;
            }

            if (alertFeed != null)
            {
                XNamespace ns = "http://www.w3.org/2005/Atom";
                var items = from item in alertFeed.Elements(ns + "entry")
                            select item;

                bool itemsExist = false;
                if (items.Count() > 0)
                {
                    // Check for 'No warnings' condition
                    var i = items.First();
                    if (i.Element(ns + "summary") != null)
                    {
                        itemsExist = true;
                    }                    
                }

                if (itemsExist)
                {
                    this.warningCell.Visible = true;
                    this.warnings.Controls.Clear();

                    int index = 0;
                    bool falseAlert = false;

                    foreach (var o in items)
                    {
                        string title = CleanupTitle(o.Element(ns + "title").Value);
                        string description = o.Element(ns + "summary").Value;
                        string shortDescription = CleanupDescription(title, ref description);

                        LinkButton link = new LinkButton { Text = title + "<BR>", ToolTip = shortDescription };

                        link.Command += new CommandEventHandler(link_Command);
                        link.CommandArgument = index.ToString();
                        link.ForeColor = Color.Red;
                        this.warnings.Controls.Add(link);

                        index++;

                        if (description.IndexOf("There are no active watches, warnings or advisories") >= 0
                            || shortDescription.IndexOf("There are no active watches, warnings or advisories") >= 0)
                        {
                            falseAlert = true;
                        }
                    }

                    if (falseAlert == true)
                    {
                        this.warningCell.Visible = false;
                    }
                }
            }
        }

        private string CleanupTitle(string title)
        {
            int index = title.LastIndexOf("PST ");
            if (index > 0)
            {
                title = title.Substring(0, index);
            }

            string today = DateTime.Today.ToString("MMMM d");
            title = title.Replace(today, "Today");
            string yesterday = DateTime.Today.AddDays(-1).ToString("MMMM d");
            title = title.Replace(yesterday, "Yesterday");
            string tomorrow = DateTime.Today.AddDays(1).ToString("MMMM d");
            title = title.Replace(tomorrow, "Tomorrow");

            title = title.Replace("January", "Jan");
            title = title.Replace("February", "Feb");
            title = title.Replace("March", "Mar");
            title = title.Replace("April", "Apr");
            title = title.Replace("August", "Aug");
            title = title.Replace("September", "Sep");
            title = title.Replace("October", "Oct");
            title = title.Replace("November", "Nov");
            title = title.Replace("December", "Dec");
            title = title.Replace(" PST", "");

            return title;
        }

        private static string CleanupDescription(string title, ref string description)
        {
            string shortDescription = title;

            int breakIndex = description.IndexOf("<br><br>", 2);
            int endIndex = description.IndexOf("$$");

            if (breakIndex > 0 && breakIndex + 10 < endIndex)
            {
                shortDescription = description.Substring(0, description.IndexOf("<br><br>") - 1).Replace("<br>", " ");

                description = description.Substring(shortDescription.Length);
                int startIndex = description.IndexOf("<br><br>");
                if (startIndex > 0)
                {
                    description = description.Substring(startIndex + 8);
                }
            }

            if (description.IndexOf("$$") > 0)
            {
                description = description.Substring(0, description.IndexOf("$$"));
            }
            if (description.StartsWith("<br><br>"))
            {
                description = description.Substring(8);
            }
            return shortDescription;
        }

        void link_Command(object sender, CommandEventArgs e)
        {
            this.warningPanel.Visible = true;
            this.forecastPanel.Visible = false;

            int index = int.Parse(e.CommandArgument.ToString());

            XNamespace ns = "http://www.w3.org/2005/Atom";
            var alert = (from item in alertFeed.Elements(ns + "entry")
                         select item).ElementAt(index);

            string title = CleanupTitle(alert.Element(ns + "title").Value);
            string description = alert.Element(ns + "summary").Value;
            string shortDescription = CleanupDescription(title, ref description);

            this.warningTitle.Text = shortDescription;
            this.warningText.Text = description.ToLower().Replace("<br><br>", "<br /><br />").Replace("<br>", " ");
        }

        void link_Click(object sender, EventArgs e)
        {
            this.forecastPanel.Visible = false;
            this.warningPanel.Visible = true;
        }

        private List<CalendarItem> GetCalendarItems()
        {
            homenet.Lists listService = new HomeCalendarView.homenet.Lists();
            listService.Credentials = new NetworkCredential("mc", "4362", "sp");
            listService.UnsafeAuthenticatedConnectionSharing = true;

            XmlDocument doc = new XmlDocument();

            XmlNode query = doc.CreateElement("Query");
            query.InnerXml = "<Where><DateRangesOverlap><FieldRef Name=\"EventDate\" /><FieldRef Name=\"EndDate\" /><FieldRef Name=\"RecurrenceID\" /><Value Type=\"DateTime\"><Month /></Value></DateRangesOverlap></Where>";

            XmlNode viewFields = doc.CreateElement("ViewFields");
            AddViewField(doc, viewFields, "Title");
            AddViewField(doc, viewFields, "EventDate");
            AddViewField(doc, viewFields, "EndDate");
            AddViewField(doc, viewFields, "RecurrenceData");
            AddViewField(doc, viewFields, "fRecurrence");
            AddViewField(doc, viewFields, "Id");
            AddViewField(doc, viewFields, "Location");

            XmlNode queryNode = doc.CreateElement("QueryOptions");
            queryNode.InnerXml = "<RecurrencePatternXMLVersion>v3</RecurrencePatternXMLVersion>";
            queryNode.InnerXml = string.Empty;

            XmlNode resultNode = listService.GetListItems("Calendar", "", query, viewFields, "500", queryNode, null);

            List<CalendarItem> items = new List<CalendarItem>();
            foreach (XmlNode dataNode in resultNode.ChildNodes)
            {
                if (!(dataNode is XmlWhitespace))
                {
                    foreach (XmlNode rowNode in dataNode.ChildNodes)
                    {
                        if (!(rowNode is XmlWhitespace))
                        {
                            string title = rowNode.Attributes["ows_Title"].Value;
                            DateTime eventDate = DateTime.Parse(rowNode.Attributes["ows_EventDate"].Value);
                            DateTime endDate = DateTime.Parse(rowNode.Attributes["ows_EndDate"].Value);
                            string recur = rowNode.Attributes["ows_fRecurrence"].Value;
                            string id = rowNode.Attributes["ows_ID"].Value;
                            string location = string.Empty;
                            if (rowNode.Attributes["ows_Location"] != null)
                            {
                                location = rowNode.Attributes["ows_Location"].Value;
                            }

                            if (recur == "0")
                            {
                                items.Add(new CalendarItem { Title = title, EventDate = eventDate, EndDate = endDate, Url = BuildItemUrl(id), Location = location });
                            }
                            else
                            {
                                string recurrenceData = rowNode.Attributes["ows_RecurrenceData"].Value;
                                if (recurrenceData.IndexOf("recurrence") > 0)
                                {
                                    XmlDocument recurDoc = new XmlDocument();
                                    recurDoc.LoadXml(recurrenceData);

                                    XmlNode repeatNode = recurDoc.SelectSingleNode("recurrence/rule/repeat");
                                    if (repeatNode != null)
                                    {
                                        XmlNode weeklyNode = repeatNode.SelectSingleNode("weekly");
                                        if (weeklyNode != null)
                                        {
                                            ProcessWeeklyRecurrence(items, title, id, location, eventDate, endDate, weeklyNode);
                                        }
                                        else
                                        {
                                            // TODO: Other patterns
                                        }
                                    }
                                    else
                                    {
                                        //title = title + " (can't figure out actual date)";
                                        //items.Add(new CalendarItem { Title = title, EventDate = DateTime.Now, EndDate = DateTime.Now });
                                    }
                                }
                                else
                                {
                                    if (recurrenceData.Contains("Every 2 week(s) on: Friday"))
                                    {
                                        ProcessWeeklyRecurrence(items, title, id, location, eventDate, endDate, null);
                                    }
                                }
                                //="<recurrence><rule><firstDayOfWeek>su</firstDayOfWeek><repeat><weekly fr='TRUE' weekFrequency='2' /></repeat><repeatForever>FALSE</repeatForever></rule></recurrence>"
                            }

                        }
                    }
                }
            }
            return items;
        }

        public static string BuildItemUrl(string id)
        {
            return "http://home.msn2.net/Lists/Events/DispForm.aspx?ID=" + id.ToString();
        }

        private static void ProcessWeeklyRecurrence(List<CalendarItem> items, string title, string id,
            string location, DateTime eventDate, DateTime endDate, XmlNode weeklyNode)
        {
            List<int> sundayOffsets = new List<int>();
            if (weeklyNode != null)
            {
                CheckForWeekday(weeklyNode, sundayOffsets, 0, "su");
                CheckForWeekday(weeklyNode, sundayOffsets, 1, "mo");
                CheckForWeekday(weeklyNode, sundayOffsets, 2, "tu");
                CheckForWeekday(weeklyNode, sundayOffsets, 3, "we");
                CheckForWeekday(weeklyNode, sundayOffsets, 4, "th");
                CheckForWeekday(weeklyNode, sundayOffsets, 5, "fr");
                CheckForWeekday(weeklyNode, sundayOffsets, 6, "sa");
            }
            else
            {
                // HACK: only works for Friday
                sundayOffsets.Add(5);
            }

            int frequency = 2;
            if (weeklyNode != null)
            {
                frequency = int.Parse(weeklyNode.Attributes["weekFrequency"].Value);
            }

            DateTime currentTime = eventDate;
            int dayOfWeek = (int)eventDate.DayOfWeek;
            currentTime = currentTime.AddDays(0 - dayOfWeek);

            while (currentTime < endDate && currentTime < DateTime.Now.AddDays(30))
            {
                List<DateTime> possibleDates = new List<DateTime>();
                foreach (int offset in sundayOffsets)
                {
                    DateTime candidateDate = currentTime.AddDays(offset);
                    if (candidateDate >= eventDate)
                    {
                        if (DateInRange(candidateDate) == true)
                        {
                            items.Add(new CalendarItem { Title = title, EventDate = candidateDate, EndDate = endDate, Url = BuildItemUrl(id), Location = location });
                        }
                    }
                }

                currentTime = currentTime.AddDays(7 * frequency);
            }
        }

        private static bool DateInRange(DateTime date)
        {
            return (date < DateTime.Now.AddDays(30) && date > DateTime.Now.AddDays(-15));
        }

        private static void CheckForWeekday(XmlNode weeklyNode, List<int> sundayOffsets, int offset, string dateString)
        {
            XmlAttribute check = weeklyNode.Attributes[dateString];
            if (check != null)
            {
                if (check.Value.ToLower() == "true")
                {
                    sundayOffsets.Add(offset);
                }
            }
        }

        protected string GetDateString(DateTime date)
        {
            if (date < DateTime.Now.AddDays(6))
            {
                return date.DayOfWeek.ToString();
            }

            return date.ToString("dddd MMM d");
        }

        private void AddViewField(XmlDocument doc, XmlNode viewFields, string fieldName)
        {
            XmlNode field1 = doc.CreateElement("FieldRef");
            AddAttribute(doc, field1, "Name", fieldName);
            viewFields.AppendChild(field1);
        }

        private void AddAttribute(XmlDocument doc, XmlNode node, string name, string val)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = val;
            node.Attributes.Append(att);
        }

        protected void onCityClick(object sender, EventArgs e)
        {
            if (this.selectKirkland == sender)
            {
                this.currentLocation = this.locations[0];
                this.selectPackwood.Font.Bold = false;
                this.selectKirkland.Font.Bold = true;
                this.selectKirkland.Enabled = false;
                this.selectPackwood.Enabled = true;
                Session["c"] = 0;
            }
            else
            {
                this.currentLocation = this.locations[1];
                this.selectPackwood.Font.Bold = true;
                this.selectKirkland.Font.Bold = false;
                this.selectKirkland.Enabled = true;
                this.selectPackwood.Enabled = false;
                Session["c"] = 1;
            }

            if (this.currentLocation.Name == "Kirkland")
            {
                this.webcamUrl.NavigateUrl = "http://cc.msn2.net/long";
                this.webcamPicture.ImageUrl = "webcam.aspx";
            }
            else
            {
                this.webcamUrl.NavigateUrl = "http://www.wsdot.wa.gov/aviation/WebCam/Packwood.htm";
                this.webcamPicture.ImageUrl = "http://images.wsdot.wa.gov/airports/packwood7.jpg";
            }
        }
    }

    public class CalendarItem
    {
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Url { get; set; }
        public string Location { get; set; }
    }
}
