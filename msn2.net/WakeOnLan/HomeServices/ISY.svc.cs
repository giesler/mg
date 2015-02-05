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
            List<GroupData> groups = new List<GroupData>();
            List<NodeData> nodes = new List<NodeData>();

            ProcessNodeQuery("/rest/nodes", groups, nodes);
            return nodes;
        }

        private static void ProcessNodeQuery(string nodeQuery, List<GroupData> groups, List<NodeData> nodes)
        {
            XDocument doc = IsyUtilities.GetResponse(nodeQuery);
            foreach (XElement node in doc.Root.Elements("node"))
            {
                NodeData newNode = new NodeData
                {
                    Name = node.Element("name").Value,
                    Address = node.Element("address").Value,
                    Status = "unknown"
                };

                XElement prop = node.Element("property");
                if (prop.Attribute("uom").Value == "%/on/off")
                {
                    string val = prop.Attribute("formatted").Value.Trim();
                    if (val.Length > 0 && val.ToLower() != "off")
                    {
                        newNode.Level = int.Parse(val);
                        newNode.IsOn = newNode.Level > 0;
                        newNode.Status = string.Format("on @ {0}%", newNode.Level);
                    }
                    else
                    {
                        newNode.Status = "off";
                    }
                }
                else
                {
                    string val = prop.Attribute("formatted").Value.Trim();
                    if (val == "On")
                    {
                        newNode.IsOn = true;
                        newNode.Status = "on";
                    }
                    else if (val == "Off")
                    {
                        newNode.IsOn = false;
                        newNode.Status = "off";
                    }

                    if ((newNode.Name.ToLower().Contains("sensor"))
                        && (newNode.Status == "on" || newNode.Status == "off"))
                    {
                        newNode.Status = newNode.Status == "on" ? "closed" : "open";
                    }

                    if ((newNode.Name.ToLower().Contains("coop"))
                        && (newNode.Status == "on" || newNode.Status == "off"))
                    {
                        newNode.Status = newNode.Status == "on" ? "open" : "closed";
                    }
                }

                nodes.Add(newNode);
            }

            foreach (XElement node in doc.Root.Elements("group"))
            {
                GroupData newGroup = new GroupData
                {
                    Name = node.Element("name").Value,
                    Address = node.Element("address").Value
                };

                newGroup.Nodes = new List<NodeData>();
                string status = string.Empty;
                bool statusVaries = false;
                foreach (XElement childNode in node.Element("members").Descendants())
                {
                    NodeData matchingNode = nodes.FirstOrDefault(i => i.Address == childNode.Value);
                    if (matchingNode == null)
                    {
                        throw new Exception("Unable to find node " + childNode.Value);
                    }
                    newGroup.Nodes.Add(matchingNode);

                    if (statusVaries == false)
                    {
                        if (status == string.Empty)
                        {
                            status = matchingNode.Status;
                        }
                        else if (status != matchingNode.Status)
                        {
                            statusVaries = true;
                            status = "mixed states";
                        }
                    }
                }
                newGroup.Status = status;
                groups.Add(newGroup);
            }
        }

        public NodeData GetNode(string address)
        {
            List<GroupData> groups = new List<GroupData>();
            List<NodeData> nodes = new List<NodeData>();

            ProcessNodeQuery("/rest/nodes/" + address, groups, nodes);
            return nodes.FirstOrDefault();
        }

        public IEnumerable<GroupData> GetGroups()
        {
            List<GroupData> groups = new List<GroupData>();
            List<NodeData> nodes = new List<NodeData>();

            ProcessNodeQuery("/rest/nodes", groups, nodes);
            return groups;
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
