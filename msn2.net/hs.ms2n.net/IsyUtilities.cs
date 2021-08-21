using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Linq;

namespace HomeServices
{
    public class IsyUtilities
    {
        public static XDocument GetResponse(string urlSuffix)
        {            
            string url = string.Format("{0}{1}", "http://192.168.1.55", urlSuffix);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = WebRequestMethods.Http.Get;
            req.Credentials = new NetworkCredential("admin", "admin");
            XDocument doc = null;

            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                if (response.ContentLength > 0)
                {
                    using (Stream s = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(s);
                        string reply = reader.ReadToEnd();
                        doc = XDocument.Parse(reply);
                    }
                }
            }

            return doc;
        }

    }

    [DataContract]
    public class NodeData
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public bool? IsOn { get; set; }
        [DataMember]
        public int? Level { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    [DataContract]
    public class GroupData
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public List<NodeData> Nodes { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}

