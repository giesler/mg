using System;
using Microsoft.SPOT;
using Gsiot.Server;

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

            server.RequestRouting.Add("POST /status",
                        context =>
                            {
                                Dripper.Toggle(!Dripper.IsOn);
                                context.SetResponse(HtmlDoc("Redirect", "<script language=\"javascript\">window.location='/status';</script>"), "text/html");
                            });

            server.Run();
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
            statusText += "<br /><font size=\"smallest\">" + DateTime.Now.ToString("M/d/yy h:mm");
            string log = Log.GetLog(DateTime.Now.Date);
            if (log.Length > 0)
            {
                statusText += "<br /><hr noshade />LOG<br />";
                string[] entries = log.Split(new char[] { '\r', '\n' });
                for (int i = entries.Length - 1; i >= 0; i--)
                {
                    string entry = entries[i];
                    if (entry.Trim().Length > 0)
                    {
                        statusText += entry.Substring(entry.IndexOf(" ")).Trim() + "<br />";
                    }
                }
            }

            string content = "<form action=\"/status\" method=\"post\"><input type=\"submit\" value=\"" + buttonText + "\" />" + statusText + "</form>";
            context.SetResponse(HtmlDoc("Drip Status", content), "text/html");
        }

        static string HtmlDoc(string title, string contents)
        {
            string html = "<html><head><title>" + title + "</title><META HTTP-EQUIV=\"CACHE-CONTROL\" CONTENT=\"NO-CACHE\" /></head><body>" + contents + "</body></html>";
            return html;
        }
    }
}
