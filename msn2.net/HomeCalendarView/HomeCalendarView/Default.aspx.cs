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

namespace HomeCalendarView
{
    public partial class _Default : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            this.todayDateLabel.Text = DateTime.Now.ToString("ddd MMM dd").ToLower();
            this.day1Label.Text = DateTime.Now.AddDays(1).ToString("dddd").ToLower();
            this.day2Label.Text = DateTime.Now.AddDays(2).ToString("dddd").ToLower();
            this.day3Label.Text = DateTime.Now.AddDays(3).ToString("dddd").ToLower();

            List<CalendarItem> items = GetCalendarItems();

            todaysEvents.BindToEventList(items.Where(ci => ci.EventDate.Date == DateTime.Now.Date).ToList<CalendarItem>());
            day1Events.BindToEventList(items.Where(ci => ci.EventDate.Date == DateTime.Now.AddDays(1).Date).ToList<CalendarItem>());
            day2Events.BindToEventList(items.Where(ci => ci.EventDate.Date == DateTime.Now.AddDays(2).Date).ToList<CalendarItem>());
            day3Events.BindToEventList(items.Where(ci => ci.EventDate.Date == DateTime.Now.AddDays(3).Date).ToList<CalendarItem>());

            var q = from ci in items
                    where ci.EventDate.Date >= DateTime.Now.AddDays(4).Date && ci.EventDate.Date <= DateTime.Now.AddDays(14)
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

            GovWeatherService.ndfdXML weatherService = new GovWeatherService.ndfdXML();

            string latLongList = weatherService.LatLonListZipCode("98033");
            XmlDocument latLongDoc = new XmlDocument();
            latLongDoc.LoadXml(latLongList);
            latLongList = latLongDoc.DocumentElement.InnerText; ;

            string[] latLongs = latLongList.Split(',');
            decimal lat = decimal.Parse(latLongs[0]);
            decimal lng = decimal.Parse(latLongs[1]);

            string xml = weatherService.NDFDgenByDay(lat, lng, DateTime.Now.Date, "7", HomeCalendarView.GovWeatherService.formatType.Item12hourly);
            XmlDocument weatherXml = new XmlDocument();
            weatherXml.LoadXml(xml);

            int offset = 0;
            if (DateTime.Now.Hour > 14)
            {
                offset = -1;
                this.todayHighTemp.Visible = false;
                this.todayWeatherImage2.Visible = false;
                this.precipToday2.Visible = false;
                this.todayTempDivider.Text = "Low ";
            }

            foreach (XmlNode tempNode in weatherXml.DocumentElement.SelectNodes("data/parameters/temperature"))
            {
                switch (tempNode.Attributes["type"].Value)
                {
                    case "maximum":
                        if (offset == 0)
                        {
                            todayHighTemp.Text = tempNode.ChildNodes[1].InnerText;
                        }
                        day1High.Text = tempNode.ChildNodes[2 + offset].InnerText;
                        day2High.Text = tempNode.ChildNodes[3 + offset].InnerText;
                        day3High.Text = tempNode.ChildNodes[4 + offset].InnerText;
                        break;

                    case "minimum":
                        todayLowTemp.Text = tempNode.ChildNodes[1].InnerText;
                        day1Low.Text = tempNode.ChildNodes[2].InnerText;
                        day2Low.Text = tempNode.ChildNodes[3].InnerText;
                        day3Low.Text = tempNode.ChildNodes[4].InnerText;
                        break;
                }
            }

            XmlNode precipNode = weatherXml.DocumentElement.SelectSingleNode("data/parameters/probability-of-precipitation");
            precipToday1.Text = precipNode.ChildNodes[1].InnerText + "%";
            if (offset == 0)
            {
                precipToday2.Text = precipNode.ChildNodes[2].InnerText + "%";
            }
            precipDay1am.Text = precipNode.ChildNodes[3 + offset].InnerText + "%";
            precipDay1pm.Text = precipNode.ChildNodes[4 + offset].InnerText + "%";
            precipDay2am.Text = precipNode.ChildNodes[5 + offset].InnerText + "%";
            precipDay2pm.Text = precipNode.ChildNodes[6 + offset].InnerText + "%";
            precipDay3am.Text = precipNode.ChildNodes[7 + offset].InnerText + "%";
            precipDay3pm.Text = precipNode.ChildNodes[8 + offset].InnerText + "%";

            XmlNode descriptionNode = weatherXml.DocumentElement.SelectSingleNode("data/parameters/weather");
            XmlNode conditionsNode = weatherXml.DocumentElement.SelectSingleNode("data/parameters/conditions-icon");
            todayWeatherImage1.ImageUrl = conditionsNode.ChildNodes[1].InnerText;
            todayWeatherImage1.AlternateText = descriptionNode.ChildNodes[1].Attributes["weather-summary"].Value;
            if (offset == 0)
            {
                todayWeatherImage2.ImageUrl = conditionsNode.ChildNodes[2].InnerText;
                todayWeatherImage2.AlternateText = descriptionNode.ChildNodes[2].Attributes["weather-summary"].Value;
            }
            day1Image1.ImageUrl = conditionsNode.ChildNodes[3 + offset].InnerText;
            day1Image1.AlternateText = descriptionNode.ChildNodes[3 + offset].Attributes["weather-summary"].Value;
            day1Image2.ImageUrl = conditionsNode.ChildNodes[4 + offset].InnerText;
            day1Image2.AlternateText = descriptionNode.ChildNodes[4 + offset].Attributes["weather-summary"].Value;
            day2Image1.ImageUrl = conditionsNode.ChildNodes[5 + offset].InnerText;
            day2Image1.AlternateText = descriptionNode.ChildNodes[5 + offset].Attributes["weather-summary"].Value;
            day2Image2.ImageUrl = conditionsNode.ChildNodes[6 + offset].InnerText;
            day2Image2.AlternateText = descriptionNode.ChildNodes[6 + offset].Attributes["weather-summary"].Value;
            day3Image1.ImageUrl = conditionsNode.ChildNodes[7 + offset].InnerText;
            day3Image1.AlternateText = descriptionNode.ChildNodes[7 + offset].Attributes["weather-summary"].Value;
            day3Image2.ImageUrl = conditionsNode.ChildNodes[8 + offset].InnerText;
            day3Image2.AlternateText = descriptionNode.ChildNodes[8 + offset].Attributes["weather-summary"].Value;
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
                                items.Add(new CalendarItem { Title = title, EventDate = eventDate, EndDate = endDate, Url = BuildItemUrl(id), Location=location });
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
                            items.Add(new CalendarItem { Title = title, EventDate = candidateDate, EndDate = endDate, Url = BuildItemUrl(id), Location=location });
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
