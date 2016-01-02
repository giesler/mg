using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

public partial class feed : System.Web.UI.Page
{
    const string UpstairHallwayAddress = "24060";
    const string LivingRoomSideLightsAddress = "18965";
    const string MediaRoomSideLightsAddress = "58666";
    const string MasterSinkLightAddress = "2B BD 8C 1";
    const string MasterBathFanAddress = "34 75 65 1";
    const string Garage1SwitchAddress = "28 CA 15 2";
    const string Garage1SensorAddress = "28 CA 15 1";
    const string Garage2SwitchAddress = "34 88 F9 2";
    const string Garage2SensorAddress = "34 88 F9 1";
    const string GardenDripName = "Garden drip";

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();

        Response.ContentType = "text/xml";

        
        XDocument doc = new XDocument(new XElement("rss"));
        XElement root = doc.Element("rss");
        root.Add(new XAttribute("version", "2.0"));

        XElement link = new XElement("link");
        link.Add(new XAttribute("rel", "alternate"));
        link.Add(new XAttribute("type", "application/rss+xml"));
        link.Add(new XAttribute("title", "MSN2 Home Control"));
        link.Add(new XAttribute("href", ""));
        root.Add(link);

        XElement channel = new XElement("channel");
        channel.Add(new XElement("title", "MSN2 Home Control"));
        channel.Add(new XElement("link", "http://control.msn2.net"));
        channel.Add(new XElement("description", "Home control"));
        root.Add(channel);

        
        IsyData.ISYClient isy = new IsyData.ISYClient();
        List<IsyData.GroupData> groups = isy.GetGroups().ToList();

        // first group contains all ISY nodes
        var garage1 = groups.First().Nodes.FirstOrDefault(g => g.Address == Garage1SensorAddress);

        XElement item = new XElement("item");
        item.Add(new XElement("title", "garage 1"));
        item.Add(new XElement("description", garage1.Status));

        var garage2 = groups.First().Nodes.FirstOrDefault(g => g.Address == Garage2SensorAddress);
        XElement item2 = new XElement("item");
        item2.Add(new XElement("title", "garage 2"));
        item2.Add(new XElement("description", garage2.Status));
        channel.Add(item2);

        channel.Add(item, item2);

        Response.Write(doc.ToString());

        Response.End();
    }
}