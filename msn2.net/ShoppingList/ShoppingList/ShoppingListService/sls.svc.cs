using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Net;

namespace msn2.net.ShoppingList
{
    public class ShoppingListService : IShoppingListService
    {
        private homenet.Lists listsService;

        public ShoppingListService()
        {
            this.listsService = new msn2.net.ShoppingList.homenet.Lists();
            this.listsService.Credentials = new NetworkCredential("mc", "4362", "sp");
            //this.listsService.Proxy = new WebProxy("http://192.168.1.1/");
        }

        public List<string> GetStores()
        {
            XmlNode list = this.listsService.GetList("Shopping List");

            XmlNode fieldsNode = list.ChildNodes[0];
            XmlNode fieldNode = fieldsNode.ChildNodes[0];
            XmlNode choicesNode = fieldNode.ChildNodes[1];

            List<string> stores = new List<string>();
            foreach (XmlNode choiceNode in choicesNode.ChildNodes)
            {
                stores.Add(choiceNode.InnerText);
            }

            return stores;
        }

        public List<ShoppingListItem> GetShoppingListItems()
        {
            return this.GetShoppingListItemsForStore(null);
        }

        public List<ShoppingListItem> GetShoppingListItemsForStore(string store)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode query = doc.CreateElement("Query");
            if (store != null)
            {
                query.InnerXml = "<Where><Eq><FieldRef Name='From' /><Value Type='Text'>" + store + "</Value></Eq></Where>";
            }
            else
            {
                query.InnerXml = string.Empty;
            }

            XmlNode viewFields = doc.CreateElement("ViewFields");
            XmlNode field1 = doc.CreateElement("FieldRef");
            AddAttribute(doc, field1, "Name", "Title");
            viewFields.AppendChild(field1);
            if (store == null)
            {
                XmlNode field2 = doc.CreateElement("FieldRef");
                AddAttribute(doc, field2, "Name", "From");
                viewFields.AppendChild(field2);
            }

            XmlNode queryNode = doc.CreateElement("QueryOptions");
            queryNode.InnerXml = string.Empty;

            XmlNode resultNode = this.listsService.GetListItems("Shopping List", "", query, viewFields, "100", queryNode, null);

            List<ShoppingListItem> results = new List<ShoppingListItem>();

            foreach (XmlNode dataNode in resultNode.ChildNodes)
            {
                if (!(dataNode is XmlWhitespace))
                {
                    foreach (XmlNode rowNode in dataNode.ChildNodes)
                    {
                        if (!(rowNode is XmlWhitespace))
                        {
                            string title = rowNode.Attributes["ows_Title"].Value;
                            int id = int.Parse(rowNode.Attributes["ows_ID"].Value);
                            ShoppingListItem item = new ShoppingListItem();
                            if (store == null)
                            {
                                string from = rowNode.Attributes["ows_From"].Value;
                                item.Store = from;
                            }
                            else
                            {
                                item.Store = store;
                            }
                            item.ListItem = title;
                            item.Id = id;
                            results.Add(item);
                        }
                    }
                }
            }

            return results;
        }

        public ShoppingListItem AddShoppingListItem(string store, string listItem)
        {
            ShoppingListItem item = new ShoppingListItem();
            item.Store = store;
            item.ListItem = listItem;

            XmlDocument doc = new XmlDocument();
            XmlNode batchNode = doc.CreateElement("Batch");

            XmlNode methodNode = doc.CreateElement("Method");
            AddAttribute(doc, methodNode, "Cmd", "New");
            AddAttribute(doc, methodNode, "ID", "1");
            batchNode.AppendChild(methodNode);

            XmlNode field1Node = doc.CreateElement("Field");
            AddAttribute(doc, field1Node, "Name", "Title");
            field1Node.InnerText = item.ListItem;
            methodNode.AppendChild(field1Node);

            XmlNode field2Node = doc.CreateElement("Field");
            AddAttribute(doc, field2Node, "Name", "From");
            field2Node.InnerText = item.Store;
            methodNode.AppendChild(field2Node);

            XmlNode resultNode = this.listsService.UpdateListItems("Shopping List", batchNode);

            foreach (XmlNode childNode in resultNode.ChildNodes)
            {
                if (childNode.LocalName == "Result")
                {
                    foreach (XmlNode rowNode in childNode.ChildNodes)
                    {
                        if (rowNode.LocalName == "row")
                        {
                            item.Id = int.Parse(rowNode.Attributes["ows_ID"].Value);
                            break;
                        }
                    }

                }
            }

            return item;
        }

        public void DeleteShoppingListItem(ShoppingListItem listItem)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode batchNode = doc.CreateElement("Batch");

            XmlNode methodNode = doc.CreateElement("Method");
            AddAttribute(doc, methodNode, "Cmd", "Delete");
            AddAttribute(doc, methodNode, "ID", "1");
            batchNode.AppendChild(methodNode);

            XmlNode field1Node = doc.CreateElement("Field");
            AddAttribute(doc, field1Node, "Name", "ID");
            field1Node.InnerText = listItem.Id.ToString();
            methodNode.AppendChild(field1Node);

            this.listsService.UpdateListItems("Shopping List", batchNode);
        }

        private void AddAttribute(XmlDocument doc, XmlNode node, string name, string val)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = val;
            node.Attributes.Append(att);
        }
    }
}
