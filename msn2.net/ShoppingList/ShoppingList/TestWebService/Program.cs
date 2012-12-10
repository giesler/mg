using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestWebService
{
    class Program
    {
        static void Main(string[] args)
        {

            XmlDocument doc = new XmlDocument();

            XmlNode query = doc.CreateElement("Query");
            query.InnerXml = string.Empty;

            //query.InnerXml = "<Where><Eq><FieldRef Name='From' /><Value Type='Text'>Grocery Store</Value></Eq></Where>";

            XmlNode viewFields = doc.CreateElement("ViewFields");
            XmlNode field1 = doc.CreateElement("FieldRef");
            AddAttribute(doc, field1, "Name", "Title");
            viewFields.AppendChild(field1);

            XmlNode queryNode = doc.CreateElement("QueryOptions"); // tempNode.ChildNodes[0];
            queryNode.InnerXml = string.Empty;

            homenet.Lists lists = new TestWebService.homenet.Lists();
            lists.Credentials = new System.Net.NetworkCredential("mc", "4362", "sp");
                        
            XmlNode node = lists.GetListItems("Shopping List", "", query, viewFields, "100", queryNode, null);

            Console.WriteLine(node.OuterXml);
            Console.Read();
        }


        static void AddAttribute(XmlDocument doc, XmlNode node, string name, string val)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = val;
            node.Attributes.Append(att);
        }

    }
}
