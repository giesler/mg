using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Windows.Forms;

namespace XMAdmin
{
	public class ConfigValues
	{
		public const string ServerUsername = "server.username";
		public const string ServerPassword = "server.password";
		public const string ServerServer = "server.server";
		public const string ServerDatabase = "server.database";
	}

	/// <summary>
	/// Summary description for config.
	/// </summary>
	public class Config
	{
		/// <summary>
		/// Name/value pairs that define the settings for XMAdmin. Use
		/// the values of ConfigValues to access settings.
		/// </summary>
		public static StringDictionary Values = new StringDictionary();

		/// <summary>
		/// Full path of the file settings are read from and written to.
		/// </summary>
		public static string Path
		{
			get
			{
				//append the filename (xmadmin.xml) to the
				//data path given by the framework
				return Application.UserAppDataPath + "\\xmadmin.xml";
			}
		}

		/// <summary>
		/// Clear content, create default settings.
		/// </summary>
		public static void Reset()
		{
			//clear all values in the dictionary
			Values.Clear();

			//insert default values
			Values["server.username"	] = "sa";
			Values["server.password"	] = "";
			Values["server.server"		] = "";
			Values["server.database"	] = "xmcatalog";
		}

		/// <summary>
		/// Load configuration settings from disk.
		/// </summary>
		public static void Load()
		{
			//load the xml file
			XmlDocument xml = new XmlDocument();
			xml.Load(Path);

			//walk children of the document element
			string s, t;
			XmlElement e;
			foreach(XmlNode n in xml.DocumentElement.ChildNodes)
			{
				//ignore evertyhing but elements
				if (n.NodeType != XmlNodeType.Element)
					break;
				e = (XmlElement)n;

				//read name, type
				s = e.GetAttribute("name");
				t = e.GetAttribute("type");

				//if type is anything but 'string', then
				//we need to deserialize the descendent nodes
				if (t != "string")
				{
					//TODO: deserialize custom xml object
				}
				else
				{
					//standard name=value pair
					Values[s] = e.InnerText;
				}
			}
		}

		/// <summary>
		/// Write all configuration settings to disk.
		/// </summary>
		public static void Save()
		{
			//create document
			XmlDocument xml = new XmlDocument();
			
			//insert root element
			XmlElement root = xml.CreateElement("settings");
			xml.AppendChild(root);

			//create an element for each name=value pair
			XmlElement e;
			foreach(DictionaryEntry entry in Values)
			{
				//do not write the password element!
				/*
				if ((string)entry.Key == ConfigValues.ServerPassword)
					break;
				*/

				//write the value to the document
				e = xml.CreateElement("setting");
				e.SetAttribute("name", entry.Key.ToString());
				e.SetAttribute("type", "string");
				e.InnerText = entry.Value.ToString();
				root.AppendChild(e);
			}

			//TODO: serialize any custom objects

			//write to file
			xml.Save(Path);
		}
	}
}
