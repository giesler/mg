using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

public partial class Sonos : System.Web.UI.Page
{
    SonosService.ZonePlayerStatus[] zps = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SonosService.SonosClient client = new SonosService.SonosClient();
        this.zps = client.GetPlayers();

        this.players.DataSource = this.zps;
        this.players.DataBind();
    }

    protected string GetUrl(string url, string suffix)
    {
        return string.Format("http://{0}:1400/{1}", url, suffix);
    }

    protected string FormatBool(bool val)
    {
        return val ? "x" : " ";
    }

    protected string GetCaps(SonosService.ZonePlayerStatus player)
    {
        return (player.Coordinator ? " c" : " ") + (player.WifiEnabled ? " w" : "");
    }

    protected string GetRebootUrl(string ip)
    {
        return GetUrl(ip, "reboot");
    }

    protected string GetTopoUrl(string ip)
    {
        return GetUrl(ip, "status/topology");
    }

    protected string GetSupportUrl(string ip)
    {
        return GetUrl(ip, "support/review");
    }

    protected void players_DataBound(object sender, EventArgs e)
    {
        //for (int i = this.players.Rows.Count - 1; i > 0; i--)
        //{
        //    GridViewRow row = this.players.Rows[i];
        //    GridViewRow previousRow = this.players.Rows[i - 1];
        //    for (int j = 0; j < row.Cells.Count; j++)
        //    {
        //        if (row.Cells[j].Text == previousRow.Cells[j].Text)
        //        {
        //            if (previousRow.Cells[j].RowSpan == 0)
        //            {
        //                if (row.Cells[j].RowSpan == 0)
        //                {
        //                    previousRow.Cells[j].RowSpan += 2;
        //                }
        //                else
        //                {
        //                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
        //                }
        //                row.Cells[j].Visible = false;
        //            }
        //        }
        //    }
        //}
    }

    protected void action_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void rebootAll_Click(object sender, EventArgs e)
    {
        Dictionary<string, Exception> errors = new Dictionary<string, Exception>();

        foreach (var player in this.zps)
        {
            try
            {
                string url = this.GetUrl(player.IpAddress, "reboot");
                string csrf = null;

                HttpWebRequest req = HttpWebRequest.CreateHttp(url);
                req.Method = "GET";
                HttpWebResponse rsp = req.GetResponse() as HttpWebResponse;

                using (var stream = rsp.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string txt = reader.ReadToEnd();
                        Trace.Write(txt);

                        int tokenStart = txt.IndexOf("csrfToken") + 18;
                        int tokenEnd = txt.Substring(tokenStart).IndexOf("\"");
                        csrf = txt.Substring(tokenStart, tokenEnd);
                    }
                }

                req = HttpWebRequest.CreateHttp(url);
                req.Headers.Add("Upgrade-Insecure-Requests", "1");
                req.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
                req.Headers.Add("Origin", url.Substring(0, url.LastIndexOf("/")));
                req.Referer = url;
                req.Headers.Add("DNT", "1");
                req.Method = "POST";

                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                req.Headers.Add("Accept-Encoding", "gzip,deflate");
                req.Headers.Add("Accept-Language", "en-US,en;q=0.8");

                
                req.ContentType = "application/x-www-form-urlencoded";
                Stream s = req.GetRequestStream();
                using (var writer = new StreamWriter(s))
                {
                    string token = Server.UrlEncode(csrf);
                    writer.WriteLine("csrfToken=" + token);
                }

                rsp = req.GetResponse() as HttpWebResponse;

                using (var stream = rsp.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string txt = reader.ReadToEnd();
                        Trace.Write(txt);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(player.Name + "(" + player.UUID + ")", ex);
            }
        }

        if (errors.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html><title>Reboot Errors</title><body>Errors during reboot:");
            sb.AppendLine("<table><tr><td>Name</td><td>Error</td>");

            foreach (string name in errors.Keys)
            {
                sb.AppendLine("<tr><td>" + name + "</td><td>" + errors[name].Message + "</td></tr>");
            }

            sb.AppendLine("</table><br /><a href=sonos.aspx>Return</a>");
            sb.AppendLine("</body></html>");

            Response.Write(sb.ToString());
            Response.End();
        }
        else
        {
            Response.Redirect("sonos.aspx");
        }
    }
}