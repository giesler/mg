using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using mn2.net.ShoppingList.sls;

namespace msn2.net.ShoppingList
{
    class LocalSettings
    {
        private Dictionary<string, string> settings = new Dictionary<string, string>();

        public Dictionary<string, string> Settings
        {
            get
            {
                return this.settings;
            }
        }

        public Dictionary<string, StoreItem> latestItems = new Dictionary<string, StoreItem>();

        public Dictionary<string, StoreItem> LatestItems
        {
            get
            {
                return this.latestItems;
            }
        }

        public void Save(string fileName)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;
            
            writer.WriteStartDocument();
            writer.WriteStartElement("shoppingList");
            writer.WriteAttributeString("version", "1.0");

            writer.WriteStartElement("s");
            foreach (string key in this.settings.Keys)
            {
                writer.WriteStartElement("si");
                writer.WriteAttributeString("name", key);
                writer.WriteString(this.settings[key]);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("stl");
            foreach (StoreItem store in this.LatestItems.Values.OrderBy(i => i.Name))
            {
                writer.WriteStartElement("st");
                writer.WriteAttributeString("name", store.Name);

                if (store.Items.Count > 0)
                {
                    writer.WriteStartElement("stl");
                    foreach (ShoppingListItem item in store.Items)
                    {
                        writer.WriteStartElement("sti");
                        writer.WriteAttributeString("id", item.Id.ToString());
                        writer.WriteString(item.ListItem);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();

            writer.Close();
        }

        public static LocalSettings ReadFromFile(string fileName)
        {
            LocalSettings settings = new LocalSettings();
            StoreItem lastStore = null;

            using (XmlTextReader tr = new XmlTextReader(fileName))
            {
                while (tr.Read())
                {
                    if (tr.Name == "si" && tr.NodeType == XmlNodeType.Element)
                    {
                        if (tr.MoveToAttribute("name"))
                        {
                            string name = tr.Value;
                            tr.MoveToElement();
                            string value = tr.ReadElementContentAsString();

                            settings.settings.Add(name, value);
                        }
                    }
                    else if (tr.Name == "st" && tr.NodeType == XmlNodeType.Element)
                    {
                        if (tr.MoveToAttribute("name"))
                        {
                            StoreItem store = new StoreItem(tr.Value);
                            settings.LatestItems.Add(store.Name, store);
                            lastStore = store;
                        }
                    }
                    else if (tr.Name == "sti" && tr.NodeType == XmlNodeType.Element)
                    {
                        if (tr.MoveToAttribute("id") == true)
                        {
                            int id = int.Parse(tr.Value);
                            tr.MoveToElement();

                            string itemText = tr.ReadElementContentAsString();
                            ShoppingListItem item = new ShoppingListItem { Id = id, ListItem = itemText, Store = lastStore.Name };

                            settings.LatestItems[lastStore.Name].Items.Add(item);
                        }
                    }
                }
            }

            return settings;
        }
    }

}
