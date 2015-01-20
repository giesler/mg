using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Xml.Linq;

namespace HomeServices
{
    [AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Allowed)]
    public class ISY : IISY
    {
        public IEnumerable<NodeData> GetNodes()
        {
            return ProcessNodeQuery("/rest/nodes");
        }

        private static IEnumerable<NodeData> ProcessNodeQuery(string nodeQuery)
        {
            List<NodeData> items = new List<NodeData>();
            XDocument doc = IsyUtilities.GetResponse(nodeQuery);
            foreach (XElement node in doc.Root.Elements("node"))
            {
                NodeData newNode = new NodeData
                {
                    Name = node.Element("name").Value,
                    Address = node.Element("address").Value
                };

                XElement prop = node.Element("property");
                if (prop.Attribute("uom").Value == "%/on/off")
                {
                    string val = prop.Attribute("formatted").Value.Trim();
                    if (val.Length > 0)
                    {
                        newNode.Level = int.Parse(val);
                        newNode.IsOn = newNode.Level > 0;
                    }
                }
                else
                {
                    string val = prop.Attribute("formatted").Value.Trim();
                    if (val == "On")
                    {
                        newNode.IsOn = true;
                    }
                    else if (val == "Off")
                    {
                        newNode.IsOn = false;
                    }
                }

                items.Add(newNode);
            }

            return items;
        }

        public IEnumerable<GroupData> GetGroups()
        {
            List<GroupData> items = new List<GroupData>();
            XDocument doc = IsyUtilities.GetResponse("/rest/nodes");
            foreach (XElement node in doc.Root.Elements("group"))
            {
                items.Add(new GroupData
                {
                    Name = node.Element("name").Value,
                    Address = node.Element("address").Value
                });
            }

            return items;        
        }

        public NodeData GetNode(string address)
        {
            return ProcessNodeQuery("/rest/nodes/" + address).FirstOrDefault();
        }

        public void TurnOff(string address)
        {
            IsyUtilities.GetResponse("/rest/nodes/" + address + "/set/DOF/");
        }

        public void TurnOn(string address)
        {
            IsyUtilities.GetResponse("/rest/nodes/" + address + "/set/DON/");
        }

        public bool GetStatus(string address)
        {
            return false;
        }
    }
}
