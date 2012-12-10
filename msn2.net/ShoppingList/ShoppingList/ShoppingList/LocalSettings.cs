using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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

        private List<string> latestStores = new List<string>();

        public List<string> LatestStores
        {
            get
            {
                return this.latestStores;
            }
        }

        public void Save(string fileName)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.Unicode);
            writer.WriteStartDocument();
            writer.WriteStartElement("shoppingList");
            writer.WriteAttributeString("version", "1.0");

            writer.WriteStartElement("s");
            foreach (string key in this.settings.Keys)
            {
                writer.WriteStartElement("i");
                writer.WriteAttributeString("name", key);
                writer.WriteString(this.settings[key]);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("st");
            foreach (string key in this.latestStores)
            {
                writer.WriteStartElement("sti");
                writer.WriteString(key);
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

            using (XmlTextReader tr = new XmlTextReader(fileName))
            {
                while (tr.Read())
                {
                    if (tr.Name == "i")
                    {
                        if (tr.MoveToAttribute("name"))
                        {
                            string name = tr.Value;
                            string value = tr.ReadElementContentAsString();

                            settings.settings.Add(name, value);
                        }
                    }
                    else if (tr.Name == "sti")
                    {
                        string value = tr.ReadElementContentAsString();
                        settings.LatestStores.Add(value);
                    }
                }
            }

            return settings;
        }
    }

}
