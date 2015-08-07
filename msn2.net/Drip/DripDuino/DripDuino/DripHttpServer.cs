using System;
using Microsoft.SPOT;
using Gsiot.Server;
using System.Collections;

namespace DripDuino
{
    class DripHttpServer
    {
        public static void Run()
        {
            HttpServer server = new HttpServer();

            server.RequestRouting.Add("GET /status",
                context =>
                {
                    StatusPage(context);
                });

            server.RequestRouting.Add("GET /statustext",
                context =>
                {
                    string response = Dripper.IsOn ? "on" : "off";
                    if (Dripper.IsOn)
                    {
                        response += " until " + Dripper.OffTime.ToString("h:mm");
                    }
                    else
                    {
                        response += "; " + GetHistoryString();
                    }
                    context.SetResponse(response, "text/plain");
                });

            server.RequestRouting.Add("POST /turnoff",
                context =>
                {
                    Dripper.Toggle(false);
                    context.SetResponse("ok", "text/plain");
                });

            server.RequestRouting.Add("POST /status",
                context =>
                {
                    Dripper.Toggle(!Dripper.IsOn);
                    Redirect(context, "Toggled drip");
                });

            server.RequestRouting.Add("POST /toggle",
                context =>
                {
                    string[] parts = context.RequestContent.Split(':');
                    TimeSpan duration = new TimeSpan(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                    Dripper.Toggle(true, duration);
                    Redirect(context, "Toggled for " + duration.ToString());
                });

            server.RequestRouting.Add("POST /deletelog",
                context =>
                {
                    string[] parts = context.RequestContent.Split('/');
                    Log.DeleteLog(new DateTime(int.Parse(parts[2]), int.Parse(parts[0]), int.Parse(parts[1])));
                    Redirect(context, "Deleted log " + context.RequestContent);
                });

            server.Run();
        }

        private static void Redirect(RequestHandlerContext context, string message)
        {
            context.SetResponse(HtmlDoc("Redirect", "<!--" + message + "--><script language=\"javascript\">window.location='/status';</script>"), "text/html");
        }

        private static void StatusPage(RequestHandlerContext context)
        {
            string buttonText = Dripper.IsOn ? "Turn Off" : "Turn On";
            string statusText = "";
            if (Dripper.IsOn)
            {
                TimeSpan duration = Dripper.OffTime - Dripper.OnTime;
                statusText = "<br /><br />Turned on at " + Dripper.OnTime.ToString("h:mm tt") + " for " + duration.Minutes + " minutes";
                if (duration.Seconds > 2)
                {
                    statusText += " " + duration.Seconds + " seconds";
                }
            }
            else
            {
                statusText = "<br /><br />" + GetHistoryString();
            }

            statusText += "<br /><font size=\"smallest\">" + DateTime.Now.ToString("M/d/yy h:mm");
            statusText += "<br /><hr noshade />LOG<br />";
            for (int i = 0; i < 7; i++)
            {
                statusText += AddLogEntries(DateTime.Now.Date.AddDays(-1 * i));
            }

            string content = "<form action=\"/status\" method=\"post\"><input type=\"submit\" value=\"" + buttonText + "\" />" + statusText + "</form>";
            context.SetResponse(HtmlDoc("Drip Status", content), "text/html");
        }

        private static string AddLogEntries(DateTime date)
        {
            string statusText = string.Empty;
            string log = Log.GetLog(date);

            if (log.Length > 0)
            {
                string[] entries = log.Split(new char[] { '\r', '\n' });
                for (int i = entries.Length - 1; i >= 0; i--)
                {
                    string entry = entries[i];
                    if (entry.Trim().Length > 0)
                    {
                        statusText += entry + "<br />";
                    }
                }
            }

            return statusText;
        }

        static string GetHistoryString()
        {
            string log = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                log += Log.GetLog(DateTime.Now.AddDays(-1 * i));
            }
            string[] entries = log.Split(new char[] { '\r', '\n' });
            ArrayList list = new ArrayList();
            foreach (string entry in entries)
            {
                if (entry != null && entry.Length > 0)
                {
                    list.Add(entry);
                }
            }
            int startIndex = 0;
            if (entries[0].IndexOf("Turned off") >= 0)
            {
                startIndex = 1;
            }
            int today = 0, yesterday = 0;
            entries = (string[]) list.ToArray(typeof(string));

            for (int i = startIndex; i < entries.Length - 1; i++)
            {
                string timestampString = entries[i].Substring(0, entries[i].IndexOf(": "));
                DateTime timeStamp = DateTimeParse(timestampString);

                string nextTimestampString = entries[i + 1].Substring(0, entries[i + 1].IndexOf(": "));
                DateTime nextTimeStamp = DateTimeParse(nextTimestampString);

                if (nextTimeStamp.Date == DateTime.Now.Date)
                {
                    today += (nextTimeStamp - timeStamp).Minutes;
                }
                else if (nextTimeStamp.Date == DateTime.Now.Date.AddDays(-1))
                {
                    yesterday += (nextTimeStamp - timeStamp).Minutes;
                }
                else
                {
                    break;
                }

                i++;
            }

            string history = "On today " + today.ToString() + " mins, yesterday " + yesterday.ToString() + " mins";
            return history;
        }

        static DateTime DateTimeParse(string dt)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                     
            string month = dt.Substring(0, dt.IndexOf("/"));
            dt = dt.Substring(dt.IndexOf("/") + 1);
            string day = dt.Substring(0, dt.IndexOf("/"));
            dt = dt.Substring(dt.IndexOf("/") + 1);
            string year = "20" + dt.Substring(0, dt.IndexOf(" "));
            dt = dt.Substring(dt.IndexOf(" ") + 1);
            string hour = dt.Substring(0, dt.IndexOf(":"));
            dt = dt.Substring(dt.IndexOf(":") + 1);
            string minute = dt.Substring(0, dt.IndexOf(" "));
            dt = dt.Substring(dt.IndexOf(" ") + 1);
            string ampm = dt.Substring(0, 2);
            if (ampm.ToLower() == "pm")
            {
                hour = (int.Parse(hour) + 12).ToString();
            }
            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), 0);
        }

        static string HtmlDoc(string title, string contents)
        {
            string html = "<html><head><title>" + title + "</title></head><body>" + contents + "</body></html>";
            return html;
        }
    }
}
