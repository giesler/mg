using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace XMedia
{
	/// <summary>
	/// Configuration settings.. all properties are static and read-only.
	/// </summary>
	class XMConfig
	{
		//Variables that store our configuation settings
		//public static readonly string varName;
		public static readonly string	DBServer;
		public static readonly string	DBDatabase;
		public static readonly string	DBUsername;
		public static readonly string	DBPassword;
		public static readonly bool		DBUseNT;
		public static readonly int		NetClientPort;
		public static readonly int		NetServerPort;
		public static readonly string	NetServerIp;
		public static readonly TimeSpan	NetPingTimeout;
		public static readonly TimeSpan	NetKeepAliveInterval;
		public static readonly TimeSpan	NetDormantTimeout;
		public static readonly TimeSpan	NetLinkDeadTimeout;
		public static readonly TimeSpan	NetConnectionCheckInterval;
		public static readonly string	MiscDefaultVersion;
		public static readonly string	MiscCurrentVersion;
		public static readonly int		QueryLimitIndex;
		public static readonly int		QueryLimitFilter;
		public static readonly TimeSpan	QueryStorageTimeout;
		public static readonly int		QueryContestFieldCutoff;
		public static readonly int		QuerySearchCutoff;
		public static readonly int		QueryResultsCutoff;
		public static readonly int		QueryProcessorCount;
		public static readonly TimeSpan	QueryMediaRebuildInterval;

		static XMConfig()
		{
			//load defaults into all settings
			DBServer = "(local)";
			DBDatabase = "xmcatalog";
			DBUsername = "sa";
			DBPassword = "";
			DBUseNT = true;
			NetClientPort = 25347;
			NetServerPort = 25346;
			NetServerIp = "0.0.0.0";
			NetPingTimeout = TimeSpan.FromMilliseconds(750);
			NetKeepAliveInterval = TimeSpan.FromMinutes(15);
			NetDormantTimeout = TimeSpan.FromMinutes(10);
			NetLinkDeadTimeout = TimeSpan.FromMinutes(20);
			NetConnectionCheckInterval = TimeSpan.FromMinutes(5);
			MiscDefaultVersion = "0.50";
			MiscCurrentVersion = "0.70";
			QueryLimitIndex = 3;
			QueryLimitFilter = 3;
			QueryStorageTimeout = TimeSpan.FromMinutes(10);
			QueryContestFieldCutoff = 5;
			QuerySearchCutoff = 5000;
			QueryResultsCutoff = 20;
			QueryProcessorCount = 1;
			QueryMediaRebuildInterval = TimeSpan.FromMinutes(120);
			
			//figure out the path, open the file
			string path = "<unknown>";
			XmlDocument doc = new XmlDocument();
			try
			{
				path = Environment.GetFolderPath(
					Environment.SpecialFolder.CommonApplicationData);
				path += "\\xmserver\\config.xml";

				//file exists?
				if (!File.Exists(path))
				{
					//we can't load.. the defaults we set earlier will
					//have to do
					XMLog.WriteLine(
						"Cannot load configuration settings, file does not exist:\n"+path,
						"XMConfig",
						System.Diagnostics.EventLogEntryType.Warning);
					return;
				}

				//load up the xml
				doc.Load(path);
			}
			catch(Exception e)
			{
				//write a log entry and return.. no error passed
				XMLog.WriteLine(String.Format(
					"Error loading configuration file '{0}':\n{1}",
					path,
					e.Message),
					"XMConfig",
					System.Diagnostics.EventLogEntryType.Error);
				return;
			}

			//load settings from xml
			string val = "<empty>";
			path = "<unknown>";
			try
			{
				//enumerate every node
				foreach(XmlNode n in doc.DocumentElement.ChildNodes)
				{
					//are we an 'entry' element?
					if (n.NodeType == XmlNodeType.Element)
					{
						//convert to element
						XmlElement e = (XmlElement)n;

						//which entry is it?
						path = e.GetAttribute("name");
						val = e.GetAttribute("value");

						switch (path)
						{
							case "Database/Server":
								DBServer = val;
								break;
							case "Database/Database":
								DBDatabase = val;
								break;
							case "Database/Username":
								DBUsername = val;
								break;
							case "Database/Password":
								DBPassword = val;
								break;
							case "Database/UseNT":
								DBUseNT = Convert.ToBoolean(val);
								break;
							case "Net/ClientPort":
								NetClientPort = Convert.ToInt32(val);
								break;
							case "Net/ServerPort":
								NetServerPort = Convert.ToInt32(val);
								break;
							case "Net/ServerIp":
								NetServerIp = val;
								break;
							case "Net/PingTimeout":
								NetPingTimeout = TimeSpan.FromMilliseconds(Convert.ToInt32(val));
								break;
							case "Net/KeepAliveInterval":
								NetKeepAliveInterval = TimeSpan.FromMinutes(Convert.ToInt32(val));
								break;
							case "Net/DormantConnectionTimeout":
								NetDormantTimeout = TimeSpan.FromMinutes(Convert.ToInt32(val));
								break;
							case "Net/LinkDeadConnectionTimeout":
								NetLinkDeadTimeout = TimeSpan.FromMinutes(Convert.ToInt32(val));
								break;
							case "Net/ConnectionCheckInterval":
								NetConnectionCheckInterval = TimeSpan.FromMinutes(Convert.ToInt32(val));
								break;
							case "Misc/DefaultVersion":
								MiscDefaultVersion = val;
								break;
							case "Misc/CurrentVersion":
								MiscCurrentVersion = val;
								break;
							case "Query/LimitIndex":
								QueryLimitIndex = Convert.ToInt32(val);
								break;
							case "Query/LimitFilter":
								QueryLimitFilter = Convert.ToInt32(val);
								break;
							case "Query/StorageTimeout":
								QueryStorageTimeout = TimeSpan.FromMinutes(Convert.ToInt32(val));
								break;
							case "Query/ContestFieldCutoff":
								QueryContestFieldCutoff = Convert.ToInt32(val);
								break;
							case "Query/SearchCutoff":
								QuerySearchCutoff = Convert.ToInt32(val);
								break;
							case "Query/ResultsCutoff":
								QueryResultsCutoff = Convert.ToInt32(val);
								break;
							case "Query/QueryProcessorCount":
								QueryProcessorCount = Convert.ToInt32(val);
								break;
							case "Query/MediaRebuildInterval":
								QueryMediaRebuildInterval = TimeSpan.FromMinutes(Convert.ToInt32(val));
								break;

							default:
								//unknown entry.. write a warning log entry,
								//but otherwise ignore
								XMLog.WriteLine(String.Format(
									"Unknown configuration entry '{0}' with value '{1}'",
									path,
									val),
									"XMConfig",
									System.Diagnostics.EventLogEntryType.Warning);
								break;
						}
					}
				}
			}
			catch(Exception e)
			{
				//write a log entry and return, allow execution to continue
				XMLog.WriteLine(String.Format(
					"Error reading configuration setting '{0}' with value '{1}':\n{2}",
					path,
					val,
					e.Message),
					"XMConfig",
					System.Diagnostics.EventLogEntryType.Error);
				return;
			}

			//configuration succesfully loaded
		}
	}
}