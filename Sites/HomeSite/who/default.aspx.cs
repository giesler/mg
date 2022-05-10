using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class netwho : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Debugger.IsAttached)
        {
            HttpCookie cookie = Request.Cookies["Login"];
            if (cookie == null || cookie.Value != "1")
            {
                Response.Redirect("https://home.giesler.org/login/?r=https://home.giesler.org/");
            }
        }

        try
        {
            TryLoadData();
        }
        catch (Exception ex)
        {
            errorPanel.Visible = true;
            error.Text = "Error loading: " + ex.Message;
            error.ToolTip = ex.StackTrace;
        }
    }

    void TryLoadData()
    {
        HttpWebRequest deviceRequest = (HttpWebRequest)HttpWebRequest.Create("https://svcs.giesler.org:8443/netwho.aspx");
        HttpWebResponse response = (HttpWebResponse)deviceRequest.GetResponse();

        DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<NetWhoItem>));
        object obj2 = json.ReadObject(response.GetResponseStream());
        var list = obj2 as List<NetWhoItem>;

        if (list == null)
        {
            throw new Exception("No data returned");
        }

        this.items.DataSource = list; 
        this.items.DataBind();
    }

    public string GetStyle(object isHome)
    {
        if (bool.Parse(isHome.ToString()))
        {
            return "color: green";
        }
        else
        {
            return "color: darkred";
        }
    }

    public string GetTimeInfo(object isHome, object lastSeen, object connectionTime)
    {
        if (bool.Parse(isHome.ToString()))
        {
            return "connected " + TsToString(connectionTime.ToString());
        }
        else
        {
            return "last seen " + TsToString(lastSeen.ToString());
        }
    }
    string TsToString(string dt)
    {
        var d = DateTime.Parse(dt.ToString());
        var ts = DateTime.Now - d;
        if (ts.TotalDays < 1)
        {
            if (ts.TotalHours < 1)
            {
                return ts.Minutes.ToString() + " mins ago";
            }
            else
            {
                return d.AddHours(-7).ToShortTimeString();
            }
        }
        else if ((int)ts.TotalDays == 1)
        {
            return "yesterday";
        }
        else
        {
            return ((int)ts.TotalDays).ToString() + " days ago";
        }
    }

    public string GetApInfo(object isHome, object apName)
    {
        if (bool.Parse(isHome.ToString()))
        {
            return " - " + apName.ToString().Replace(" AP", "");
        }

        return string.Empty;
    }
}

public class NetWhoItem
{
    public string Name { get; set; }
    public string APName { get; set; }
    public string Status { get; set; }
    public bool IsHome { get; set; }
    public string LastSeen { get; set; }
    public string ConnectionTime { get; set; }
}