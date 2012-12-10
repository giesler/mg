using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using msn2.net.Pictures;

public partial class rss : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "text/xml";

        XmlWriter writer = XmlWriter.Create(Response.OutputStream);
        writer.WriteStartDocument();

        writer.WriteStartElement("rss");
        writer.WriteAttributeString("version", "2.0");
        writer.WriteStartElement("channel");

        writer.WriteElementString("title", "pics.msn2.net pictures");
        writer.WriteElementString("link", "http://pics.msn2.net/");
        writer.WriteElementString("description", "Pictures from pics.msn2.net");
        writer.WriteElementString("language", "en-us");
        writer.WriteElementString("ttl", "1440");

        for (int i = 0; i < 10; i++)
        {
            Picture randomPic = PicContext.Current.PictureManager.GetRandomPicture();

            string drillUrl = string.Format("picview.aspx?p={0}&type=random", randomPic.Id);
            string picUrl = ""; // BUGBUG: pic cache needed string.Format("GetImage.axd?p={0}&mw={1}&mh={2}", randomPic.Id, randomPic.Height, randomPic.Width);

            writer.WriteStartElement("item");
            writer.WriteElementString("title", randomPic.Title);
            writer.WriteElementString("description", randomPic.Description);
            writer.WriteElementString("link", drillUrl);

            writer.WriteStartElement("enclosure");
            writer.WriteAttributeString("url", picUrl);
            writer.WriteAttributeString("type", "image/jpg");
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndDocument();
        Response.OutputStream.Flush();
        Response.End();
    }
}