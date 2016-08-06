using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace msn2.net
{
    public class SonosIntegration
    {
        static string defaultZoneName = "Kitchen";
        static string defaultZoneIp = "192.168.1.67";

        public static SonosPlayingData GetPlayingData(string roomName)
        {
            return GetPlayingData(roomName, defaultZoneIp);
        }

        public static SonosPlayingData GetPlayingData(string roomName, string defaultIp)
        {
            string soapRequestTemplate = "<s:Envelope s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body>{0}</s:Body></s:Envelope>";
            XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace uns = "urn:schemas-upnp-org:service:AVTransport:1";

            string zoneName = defaultZoneName;
            string zoneIp = defaultZoneIp;
            string coordinatorIp = GetCoordinator(zoneName, zoneIp);

            HttpWebRequest statRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://{0}:1400/MediaRenderer/AVTransport/Control", coordinatorIp));
            statRequest.ContentType = "text/xml";
            statRequest.Method = "POST";
            statRequest.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:AVTransport:1#GetTransportInfo\"");

            string soapStatus = string.Format(soapRequestTemplate, "<u:GetTransportInfo xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID><Channel>Master</Channel></u:GetTransportInfo>");
            byte[] byteStatus = Encoding.UTF8.GetBytes(soapStatus);
            statRequest.ContentLength = byteStatus.Length;
            using (Stream s = statRequest.GetRequestStream())
            {
                s.Write(byteStatus, 0, byteStatus.Length);
                s.Close();
            }

            HttpWebResponse statResponse = (HttpWebResponse)statRequest.GetResponse();
            using (Stream s = statResponse.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    string statusXml = sr.ReadToEnd();
                    XDocument status = XDocument.Parse(statusXml);

                    XElement body = status.Descendants(ns + "Body").First();
                    XElement transportInfo = body.Descendants(uns + "GetTransportInfoResponse").First().Descendants("CurrentTransportState").First();
                    if (transportInfo.Value.ToLower() != "playing" && transportInfo.Value.ToLower() != "paused_playback")
                    {
                        return new SonosPlayingData();
                    }
                }
            }

            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://{0}:1400/MediaRenderer/AVTransport/Control", coordinatorIp));
            webRequest.ContentType = "text/xml";
            webRequest.Method = "POST";
            webRequest.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:AVTransport:1#GetPositionInfo\"");

            string soap = string.Format(soapRequestTemplate, "<u:GetPositionInfo xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID><Channel>Master</Channel></u:GetPositionInfo>");

            byte[] byteArray = Encoding.UTF8.GetBytes(soap);
            webRequest.ContentLength = byteArray.Length;
            Stream stream = webRequest.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseString = reader.ReadToEnd();
                XDocument doc = XDocument.Parse(responseString);

                XElement body = doc.Descendants(ns + "Body").First();
                XElement trackMetaData = body.Descendants(uns + "GetPositionInfoResponse").First().Descendants("TrackMetaData").First();

                string xml = HttpUtility.UrlDecode(trackMetaData.Value);
                if (trackMetaData.Value == "NOT_IMPLEMENTED")
                {
                    return new SonosPlayingData();
                }
                else
                {
                    XDocument playing = XDocument.Parse(xml.Replace(" & ", " &amp; "));
                    XNamespace nsupnp = "urn:schemas-upnp-org:metadata-1-0/upnp/";
                    XNamespace nsr = "urn:schemas-rinconnetworks-com:metadata-1-0/";
                    XNamespace nsdc = "http://purl.org/dc/elements/1.1/";

                    XElement item = playing.Elements().First();

                    XElement albumArtUri = item.Descendants(nsupnp + "albumArtURI").FirstOrDefault();
                    XElement title = item.Descendants(nsdc + "title").FirstOrDefault();
                    XElement album = item.Descendants(nsupnp + "album").FirstOrDefault();
                    XElement artist = item.Descendants(nsr + "albumArtist").FirstOrDefault();

                    SonosPlayingData data = new SonosPlayingData();
                    if (albumArtUri != null)
                    {
                        data.AlbumArtUri = string.Format("http://{0}:1400{1}", coordinatorIp, albumArtUri.Value);
                    }
                    if (title != null)
                    {
                        data.Title = title.Value;
                    }
                    if (album != null)
                    {
                        data.Album = album.Value;
                    }
                    if (artist != null)
                    {
                        data.Artist = artist.Value;
                    }
                    return data;
                }
            }
        }

        public static IEnumerable<ZonePlayerStatus> GetPlayers()
        {
            List<ZonePlayerStatus> players = new List<ZonePlayerStatus>();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://" + defaultZoneIp + ":1400/status/topology");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseString = reader.ReadToEnd();
                XDocument doc = XDocument.Parse(responseString);

                XElement xPlayers = doc.Root.Element("ZonePlayers");
                foreach (XElement xPlayer in xPlayers.Elements())
                {
                    ZonePlayerStatus player = new ZonePlayerStatus { Name = xPlayer.Value };
                    player.BehindWifiExt =  BoolParse(xPlayer.Attribute("behindwifiext").Value);
                    player.ChannelFrequency = int.Parse( xPlayer.Attribute("channelfreq").Value);
                    player.Coordinator = BoolParse(xPlayer.Attribute("coordinator").Value);
                    player.Group = xPlayer.Attribute("group").Value;
                    player.HasConfiguredSSID = BoolParse(xPlayer.Attribute("hasconfiguredssid").Value);
                    player.Location = xPlayer.Attribute("location").Value;
                    player.UUID = xPlayer.Attribute("uuid").Value;
                    player.Version = xPlayer.Attribute("version").Value;
                    player.WifiEnabled = BoolParse(xPlayer.Attribute("wifienabled").Value);
                    player.WirelessLeafOnly = BoolParse(xPlayer.Attribute("wirelessleafonly").Value);
                    player.WirelessMode = BoolParse(xPlayer.Attribute("wirelessmode").Value);
                    player.IpAddress = GetIpFromLocationUri(player.Location);
                    players.Add(player);
                }

                Dictionary<string, int> groups = new Dictionary<string, int>();
                foreach (ZonePlayerStatus player in players)
                {
                    if (groups.ContainsKey(player.Group))
                    {
                        player.GroupNumber = groups[player.Group];    
                    }
                    else
                    {
                        player.GroupNumber = groups.Count + 1;
                        groups.Add(player.Group, player.GroupNumber);
                    }
                }

                return players.OrderBy(i => i.Name).OrderByDescending(i => i.Coordinator).OrderBy(i => i.GroupNumber);
            }
        }

        private static bool BoolParse(string s)
        {
            if (s == "0")
            {
                return false;
            }
            else if (s == "1")
            {
                return true;
            }

            return bool.Parse(s);
        }

        private static string GetCoordinator(string zoneName, string zoneIp)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://" + zoneIp + ":1400/status/topology");
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            using (Stream s = res.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    string xml = sr.ReadToEnd();
                    XDocument topo = XDocument.Parse(xml);

                    XElement players = topo.Root.Element("ZonePlayers");
                    var target = players.Elements("ZonePlayer").Where(p => p.Value.ToLower() == zoneName.ToLower());
                    if (target == null)
                    {
                        throw new Exception(string.Format("Unable to find Sonos player '{0}' listed in Sonos topology.", zoneName, zoneIp));
                    }
                    if (target.Attributes("coordinator").Where(a => a.Value == "true").Count() == 0)
                    {
                        string groupName = target.Attributes("group").First().Value;

                        var newTarget = players.Elements("ZonePlayer").Where(p => p.Attribute("group").Value == groupName && p.Attribute("coordinator").Value == "true").First();

                        return GetIpFromLocationUri(newTarget.Attribute("location").Value);
                    }
                    else
                    {
                        // zoneIp is a coordinator
                        return zoneIp;
                    }
                }
            }
        }

        private static string GetIpFromLocationUri(string location)
        {
            // Location format: http://192.168.1.69:1400/xml/device_description.xml
            int startIndex = location.IndexOf("://");
            int endIndex = location.IndexOf(":1400");
            string zoneIp = location.Substring(startIndex + 3, endIndex - startIndex - 3);
            return zoneIp;
        }
    }
}
