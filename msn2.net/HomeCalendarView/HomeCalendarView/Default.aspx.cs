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

namespace HomeCalendarView
{
    public partial class _Default : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.lastUpdateTime.Text = "Last update: " + DateTime.Now.ToShortTimeString();
            this.dataLoadTimer.Enabled = false;
            this.dataLoadTimer.Tick += new EventHandler<EventArgs>(dataLoadTimer_Tick);
            this.closeWarning.Click += new EventHandler(closeWarning_Click);
            this.refreshTimer.Tick += new EventHandler<EventArgs>(refreshTimer_Tick);
            this.refreshTimer.Interval = 60 * 1000 * new Random().Next(18, 23);

            this.todayDateLabel.Text = DateTime.Now.ToString("ddd MMM d").ToLower();
            //this.day1Label.Text = DateTime.Now.AddDays(1).ToString("dddd").ToLower();
            this.day2Label.Text = DateTime.Now.AddDays(2).ToString("dddd").ToLower();
            this.day3Label.Text = DateTime.Now.AddDays(3).ToString("dddd").ToLower();
            this.day4Label.Text = DateTime.Now.AddDays(4).ToString("dddd").ToLower();

            Trace.Write("Current weather");
            DisplayCurrent();

            Trace.Write("Cal");
            DisplayCalendar();

            Trace.Write("Forecast");
            DisplayForecast();

            Trace.Write("Forecast details");
            DisplayForecastDetails();

            Trace.Write("Alerts");
            DisplayWeatherAlerts();

            Trace.Write("Done");

            this.reenableTimer = this.dataLoadTimer.Enabled;
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

        private void LoadForecast(object sender)
        {
            System.Web.Caching.Cache cache = (System.Web.Caching.Cache)sender;

            try
            {
                decimal lat = 47.6727M;
                decimal lng = -122.187M;
                GovWeatherService.ndfdXML weatherService = new GovWeatherService.ndfdXML();
                string xml = weatherService.NDFDgenByDay(lat, lng, DateTime.Now.Date, "7", GovWeatherService.formatType.Item12hourly);
                cache.Add("forecastCache", xml, null, DateTime.Now.AddMinutes(20), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            finally
            {
                cache.Remove("LoadForecast");
            }
        }

        private void DisplayForecast()
        {
            string xml = null;
            object cacheItem = HttpContext.Current.Cache["forecastCache"];
            if (cacheItem == null)
            {
                this.dataLoadTimer.Enabled = true;

                if (base.Cache["LoadForecast"] == null)
                {
                    base.Cache.Add("LoadForecast", DateTime.Now, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
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
                this.todayForecastLabel.Text = night ? "tonight" : "forecast";

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

                this.todayHigh.ImageUrl = conditionsNode.ChildNodes[1].InnerText;
                this.todayHigh.ImageAltText = descriptionNode.ChildNodes[1].Attributes["weather-summary"].Value;
                this.todayLow.ImageUrl = conditionsNode.ChildNodes[2].InnerText;
                this.todayLow.ImageAltText = descriptionNode.ChildNodes[2].Attributes["weather-summary"].Value;

                this.day1High.ImageUrl = conditionsNode.ChildNodes[3 + offset].InnerText;
                this.day1High.ImageAltText = descriptionNode.ChildNodes[3 + offset].Attributes["weather-summary"].Value;
                this.day1Low.ImageUrl = conditionsNode.ChildNodes[4 + offset].InnerText;
                this.day1Low.ImageAltText = descriptionNode.ChildNodes[4 + offset].Attributes["weather-summary"].Value;
                this.day2High.ImageUrl = conditionsNode.ChildNodes[5 + offset].InnerText;
                this.day2High.ImageAltText = descriptionNode.ChildNodes[5 + offset].Attributes["weather-summary"].Value;
                this.day2Low.ImageUrl = conditionsNode.ChildNodes[6 + offset].InnerText;
                this.day2Low.ImageAltText = descriptionNode.ChildNodes[6 + offset].Attributes["weather-summary"].Value;
                this.day3High.ImageUrl = conditionsNode.ChildNodes[7 + offset].InnerText;
                this.day3High.ImageAltText = descriptionNode.ChildNodes[7 + offset].Attributes["weather-summary"].Value;
                this.day3Low.ImageUrl = conditionsNode.ChildNodes[8 + offset].InnerText;
                this.day3Low.ImageAltText = descriptionNode.ChildNodes[8 + offset].Attributes["weather-summary"].Value;
                this.day4High.ImageUrl = conditionsNode.ChildNodes[9 + offset].InnerText;
                this.day4High.ImageAltText = descriptionNode.ChildNodes[9 + offset].Attributes["weather-summary"].Value;
                this.day4Low.ImageUrl = conditionsNode.ChildNodes[10 + offset].InnerText;
                this.day4Low.ImageAltText = descriptionNode.ChildNodes[10 + offset].Attributes["weather-summary"].Value;
            }
        }

        private void LoadForecastDetails(object sender)
        {
            System.Web.Caching.Cache cache = (System.Web.Caching.Cache)sender;

            try
            {
                WebRequest req = WebRequest.Create("http://www.wrh.noaa.gov/forecast/MapClick.php?site=sew&smap=1&textField1=47.67273&textField2=-122.18730&TextType=1");
                WebResponse response = req.GetResponse();
                Stream st = response.GetResponseStream();
                StreamReader sr = new StreamReader(st);
                string fileContents = sr.ReadToEnd();
                cache.Add("fcast2", fileContents, null, DateTime.Now.AddMinutes(20), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                sr.Close();
                st.Close();
                response.Close();
            }
            finally
            {
                cache.Remove("LoadForecastDetails");
            }
        }

        private void DisplayForecastDetails()
        {
            Regex forecastDescriptionRegex = new Regex(@"((<b>(?<period>(\w| )+)\: </b>)(?<fcast>.+?))<br>");

            string fileContents = null;
            object cacheItem = HttpContext.Current.Cache["fcast2"];
            if (cacheItem == null)
            {
                this.dataLoadTimer.Enabled = true;

                if (base.Cache["LoadForecastDetails"] == null)
                {
                    base.Cache.Add("LoadForecastDetails", DateTime.Now, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
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
                Match match = forecastDescriptionRegex.Match(fileContents);

                while (match.Success == true)
                {
                    Group period = match.Groups["period"];
                    Group fcast = match.Groups["fcast"];

                    if (period != null && fcast != null)
                    {
                        fcastList.Add(new FCastItem { Day = period.Value, Forecast = fcast.Value });
                    }
                    match = match.NextMatch();
                }

                if (fcastList.Count > 0)
                {
                    int offset = 0;
                    if (fcastList[0].Day != "Today" && fcastList[0].Day != "This Afternoon" && fcastList[0].Day != "Late Afternoon")
                    {
                        offset = -1;
                        this.todayLow.ImageAltText = fcastList[0].Forecast;
                    }
                    else
                    {
                        this.todayHigh.ImageAltText = fcastList[0].Forecast;
                        this.todayLow.ImageAltText = fcastList[1].Forecast;
                    }

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

        private static void GetLatLong(GovWeatherService.ndfdXML weatherService, out decimal lat, out decimal lng)
        {
            string latLongList = weatherService.LatLonListZipCode("98033");

            XmlDocument latLongDoc = new XmlDocument();
            latLongDoc.LoadXml(latLongList);
            latLongList = latLongDoc.DocumentElement.InnerText; ;

            string[] latLongs = latLongList.Split(',');
            lat = decimal.Parse(latLongs[0]);
            lng = decimal.Parse(latLongs[1]);
        }

        private void DisplayCalendar()
        {
            List<CalendarItem> items = GetCalendarItems();

            todaysEvents.BindToEventList(items.Where(ci => ci.EventDate.Date == DateTime.Now.Date).ToList<CalendarItem>());
            day1Events.BindToEventList(items.Where(ci => ci.EventDate.Date == DateTime.Now.AddDays(1).Date).ToList<CalendarItem>());
            day2Events.BindToEventList(items.Where(ci => ci.EventDate.Date == DateTime.Now.AddDays(2).Date).ToList<CalendarItem>());
            day3Events.BindToEventList(items.Where(ci => ci.EventDate.Date == DateTime.Now.AddDays(3).Date).ToList<CalendarItem>());
            day4Events.BindToEventList(items.Where(ci => ci.EventDate.Date == DateTime.Now.AddDays(4).Date).ToList<CalendarItem>());

            var q = from ci in items
                    where ci.EventDate.Date >= DateTime.Now.AddDays(5).Date && ci.EventDate.Date <= DateTime.Now.AddDays(14)
                    orderby ci.EventDate
                    select ci;
            int count = 0;
            foreach (CalendarItem item in q.Take(3))
            {
                if (count > 0)
                {
                    Label comma = new Label();
                    comma.Text = ", ";
                    this.upcomingEvents.Controls.Add(comma);
                }

                Label label = new Label();
                label.Text = this.GetDateString(item.EventDate) + ": ";
                this.upcomingEvents.Controls.Add(label);

                HyperLink eventLink = new HyperLink();
                eventLink.Text = item.Title;
                eventLink.NavigateUrl = item.Url;
                eventLink.Target = "_top";
                this.upcomingEvents.Controls.Add(eventLink);

                count++;
            }
        }

        void LoadCurrent(object sender)
        {
            System.Web.Caching.Cache cache = (System.Web.Caching.Cache)sender;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("http://www.nws.noaa.gov/data/current_obs/KSEA.xml");
                cache.Add("current", doc, null, DateTime.Now.AddMinutes(20), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            finally
            {
                cache.Remove("LoadCurrent");
            }
        }

        private void DisplayCurrent()
        {
            XmlDocument doc = null;
            object cacheItem = HttpContext.Current.Cache["current"];
            if (cacheItem == null)
            {
                this.dataLoadTimer.Enabled = true;

                if (base.Cache["LoadCurrent"] == null)
                {
                    base.Cache.Add("LoadCurrent", DateTime.Now, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadCurrent), HttpContext.Current.Cache);
                }
            }
            else
            {
                doc = (XmlDocument)cacheItem;
            }

            if (doc != null)
            {
                this.currentConditionsTable.Visible = true;
                this.currentConditionsLoading.Visible = false;

                this.currentTemp.Text = doc.DocumentElement.SelectSingleNode("temp_f").InnerText + "&deg;";

                string timeString = doc.DocumentElement.SelectSingleNode("observation_time_rfc822").InnerText;
                timeString = timeString.Substring(0, timeString.IndexOf("-") - 1);
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

                    string gusts = doc.DocumentElement.SelectSingleNode("wind_gust_mph").InnerText;
                    if (gusts != "NA")
                    {
                        this.windLabel.Text += ",<br />gusts&nbsp;to&nbsp;" + gusts;
                    }
                }

                this.windChill.Text = doc.DocumentElement.SelectSingleNode("windchill_f").InnerText + "&deg;";
                this.visibility.Text = doc.DocumentElement.SelectSingleNode("visibility_mi").InnerText;

                if (this.visibility.Text.EndsWith(".00"))
                {
                    this.visibility.Text = this.visibility.Text.Replace(".00", "");
                }

                string conditionImage = doc.DocumentElement.SelectSingleNode("icon_url_base").InnerText;
                conditionImage += doc.DocumentElement.SelectSingleNode("icon_url_name").InnerText;
                this.currentCondition.ImageUrl = conditionImage;
                this.currentCondition.AlternateText = doc.DocumentElement.SelectSingleNode("weather").InnerText;
                this.currentConditionText.Text = this.currentCondition.AlternateText;
            }
        }

        void LoadAlerts(object sender)
        {
            System.Web.Caching.Cache cache = (System.Web.Caching.Cache)sender;

            try
            {
                XElement alertFeed = XElement.Load("http://www.weather.gov/alerts/wwarssget.php?zone=WAZ505");
                cache.Add("alerts", alertFeed, null, DateTime.Now.AddMinutes(20), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            finally
            {
                cache.Remove("LoadAlerts");
            }
        }

        XElement alertFeed = null;

        private void DisplayWeatherAlerts()
        {
            object cacheItem = HttpContext.Current.Cache["alerts"];
            if (cacheItem == null)
            {
                this.dataLoadTimer.Enabled = true;

                if (base.Cache["LoadAlerts"] == null)
                {
                    base.Cache.Add("LoadAlerts", DateTime.Now, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.LoadAlerts), HttpContext.Current.Cache);
                }
            }
            else
            {
                alertFeed = (XElement)cacheItem;
            }

            if (alertFeed != null)
            {
                var items = from item in alertFeed.Elements("channel").Elements("item")
                            select item;

                if (items.Count() != 0)
                {
                    this.warningCell.Visible = true;
                    this.warnings.Controls.Clear();

                    int index = 0;
                    bool falseAlert = false;

                    foreach (var o in items)
                    {
                        string title = o.Element("title").Value.Replace(" - East Puget Sound Lowlands (Washington)", "").Trim();
                        string description = o.Element("description").Value;
                        string shortDescription = CleanupDescription(title, ref description);

                        LinkButton link = new LinkButton { Text = title + "<BR>", ToolTip = shortDescription };

                        link.Command += new CommandEventHandler(link_Command);
                        link.CommandArgument = index.ToString();
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
                    else
                    {
                        this.severeWeatherAlertLabel.Text = index > 1 ? "Severe&nbsp;Weather&nbsp;Alerts" : "Severe&nbsp;Weather&nbsp;Alert";
                    }
                }
            }
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

            var alert = (from item in alertFeed.Elements("channel").Elements("item")
                         select item).ElementAt(index);

            string title = alert.Element("title").Value;
            string description = alert.Element("description").Value;
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
            CheckForWeekday(weeklyNode, sundayOffsets, 0, "su");
            CheckForWeekday(weeklyNode, sundayOffsets, 1, "mo");
            CheckForWeekday(weeklyNode, sundayOffsets, 2, "tu");
            CheckForWeekday(weeklyNode, sundayOffsets, 3, "we");
            CheckForWeekday(weeklyNode, sundayOffsets, 4, "th");
            CheckForWeekday(weeklyNode, sundayOffsets, 5, "fr");
            CheckForWeekday(weeklyNode, sundayOffsets, 6, "sa");

            int frequency = int.Parse(weeklyNode.Attributes["weekFrequency"].Value);

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
