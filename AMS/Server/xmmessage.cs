namespace XMedia
{
    using System;
	using System.Diagnostics;
	using System.Collections;
	using System.Xml;
	using System.Text;

	// helper class for message
	public class XMMessageField
	{
		public string Name;
		public object Value;
	}

	//listing class graph
	public class XMMediaListing
	{
		public bool Full;
		public /*beta2:*/ArrayList MediaItems = new /*beta2:*/ArrayList();

		public XmlElement ToXml(XmlDocument doc)
		{
			//create our base element
			XmlElement listing = doc.CreateElement("listing");
			listing.SetAttribute("type", Full ? "full" : "partial");

			//add media items
			XmlElement mi;
			foreach(XMMediaItem xmmi in MediaItems)
			{
				//basic media item attributes
				mi = doc.CreateElement("media");
				mi.SetAttribute("md5", xmmi.Md5);
				mi.SetAttribute("height", xmmi.Height.ToString());
				mi.SetAttribute("width", xmmi.Width.ToString());
				mi.SetAttribute("filesize", xmmi.FileSize.ToString());

				//index list
				for (int i=0;i<xmmi.Indices.Length;i++)
					mi.AppendChild(xmmi.Indices[i].ToXml(doc));

				//add media item to xml graph
				listing.AppendChild(mi);
			}

			return listing;
		}

		public void FromXml(XmlElement listing)
		{
			//is it full or patial?
			Full = (listing.GetAttribute("type")=="full");

			//parse all media tags
			XMMediaItem m;
			foreach(XmlElement e in listing.GetElementsByTagName("media"))
			{
				//basic attributes
				m = new XMMediaItem();
				m.Action = e.GetAttribute("action");
				m.Md5 = e.GetAttribute("md5");
				m.Height = /*beta2:*/System.Convert.ToInt32(e.GetAttribute("height"));
				m.Width = /*beta2:*/System.Convert.ToInt32(e.GetAttribute("width"));
				m.FileSize = /*beta2:*/System.Convert.ToInt32(e.GetAttribute("filesize"));
			
				//check for index
				XmlNodeList nl = e.GetElementsByTagName("index");
				m.Indices = new XMIndex[nl.Count];
				for(int i=0;i<nl.Count;i++)
					m.Indices[i].FromXml((XmlElement)nl[i]);

				//insert media item into listing
				MediaItems.Add(m);
			}
		}
	}

    /// <summary>
    ///    Summary description for xmmessage.
    /// </summary>
    public class XMMessage
    {
		//the connection we are from
		public XMConnection Connection;

		//generic message data
		public bool IsValid;
		public int Sequence, Reply;
		public string Host, Session, Action, Format;
		public string System;
		public Hashtable Fields = new Hashtable();

		//special data depending on message
		public XMMediaListing Listing;
		public XMQuery Query;
		public /*beta2:*/ArrayList QueryResults;

		//auto update
		public bool auEnable = false;
		public string auPath;
		public XMGuid auMd5;
		public int auSize;
		
        public XMMessage()
        {
			IsValid = false;
        }

		public XMMessage(XmlDocument xml, XMConnection newConnection)
		{
			IsValid = false;
			Connection = newConnection;
			LoadXML(xml);
		}

		public void LoadXML(XmlDocument xml)
		{
			try 
			{
				
			//get sequence number, reply number
			XmlElement root = xml.DocumentElement;
			Sequence = /*beta2:*/Convert.ToInt32(root.GetAttribute("sequence"));
			Reply = /*beta2:*/Convert.ToInt32(root.GetAttribute("reply"));

			//read major elements
			XmlElement el, content=null;
			foreach(XmlNode n in root.ChildNodes)
			{
				if (n.NodeType == XmlNodeType.Element)
				{
					el = (XmlElement)n;
					switch(el.Name)
					{
						case "from":
							Host = el.GetAttribute("host");
							Session = el.GetAttribute("session");
							System = el.GetAttribute("system");
							break;

						case "request":
						case "response":
							Action = el.GetAttribute("for");
							break;

						case "content":
							Format = el.GetAttribute("format");
							content = el;
							break;
					}
				}
			}

			//read simple fields from content
			XMMessageField xmfield;
			XmlElement e, e2;
			if (Format=="text/xml" && content!=null)
			{
				//read all child nodes
				foreach(XmlNode n in content.ChildNodes)
				{
					//is this an element?
					if (n.NodeType == XmlNodeType.Element)
					{
						//is this a simple field?
						e = (XmlElement)n;
						if (e.Name == "field")
						{
							//always create field record
							xmfield = new XMMessageField();
							xmfield.Name = e.GetAttribute("name");
							xmfield.Value = e.InnerText;
							Fields.Add(xmfield.Name, xmfield);				
						
							//complex field?
							switch(xmfield.Name)
							{
								case "listing":
									
									//get listing element
									e2 = null;
									if (e.ChildNodes.Count==1)
										e2 = (XmlElement)e.ChildNodes[0];
									else
										throw new Exception("Missing listing element.");

									//create listing
									Listing = new XMMediaListing();
									Listing.FromXml(e2);
									break;
								
								case "query":
									
									//get query element
									e2 = null;
									if (e.ChildNodes.Count==1)
										e2 = (XmlElement)e.ChildNodes[0];
									else
										throw new Exception("Missing query element.");

									//extract data from xml
									Query = new XMQuery();
									Query.FromXml(e2);
									break;
							}
						}
					}
				}

				//success
				IsValid = true;
			}

			} //try
			catch (Exception e)
			{
				//generic error handler
				XMLog.WriteLine("Error unpacking message: " + e.Message, "XMMessage.LoadXxml");
			}
		}

		public XmlDocument ToXml()
		{
			//create elements
			XmlDocument xml = new XmlDocument();
			XmlElement root, from, request;
			root = xml.CreateElement("message");
			from = xml.CreateElement("from");
			request = xml.CreateElement("request");

			//set attributes
			root.SetAttribute("sequence", Sequence.ToString());
			root.SetAttribute("reply", Reply.ToString());
			from.SetAttribute("host", Connection.LocalIP.ToString());
			from.SetAttribute("session", "0");
			from.SetAttribute("system", "0");
			request.SetAttribute("for", Action);
			
			//setup content
			XmlElement content = xml.CreateElement("content");
			content.SetAttribute("format", "text/html");

			//simple fields
			XmlElement field;
			foreach(XMMessageField f in Fields.Values)
			{
				field = xml.CreateElement("field");
				field.SetAttribute("name", f.Name);
				field.InnerText = f.Value.ToString();
				content.AppendChild(field);
			}

			//complex fields
			if (Listing!=null)
			{
				field = xml.CreateElement("field");
				field.SetAttribute("name", "listing");
				field.AppendChild(Listing.ToXml(xml));
				content.AppendChild(field);
			}
			if (QueryResults!=null)
			{
				field = xml.CreateElement("field");
				field.SetAttribute("name", "results");
				field.AppendChild(XMQuery.ResultsToXml(QueryResults, xml));
				content.AppendChild(field);
			}
			
			//add nodes
			root.AppendChild(from);
			root.AppendChild(request);
			root.AppendChild(content);
	
			//auto update binary message?
			if (auEnable)
			{
				XmlElement binary = xml.CreateElement("binary");
				binary.SetAttribute("md5", auMd5.ToString());
				binary.SetAttribute("size", auSize.ToString());
				root.AppendChild(binary);
			}

			xml.AppendChild(root);
			return xml;
		}

		public XMMessageField GetField(string name)
		{
			//search for the named field
			/*
			foreach(XMMessageField field in Fields)
			{
				if (field.Name==name)
				{
					return field.Value;
				}
			}
			return null;
			*/
			return (XMMessageField)Fields[name];
		}

		public void SetField(string name, object newval)
		{
			//try to find an existing field first
			/*
			foreach(XMMessageField field in Fields)
			{
				if (field.Name==name)
				{
					field.Value = newval;
					return;
				}
			}
			*/
			XMMessageField field = (XMMessageField)Fields[name];
			if (field!=null)
			{
				field.Value = newval;
				return;
			}

			//field does not exist--create
			XMMessageField f2 = new XMMessageField();
			f2.Name = name;
			f2.Value = newval;
			Fields.Add(name, f2);
		}

		public override string ToString()
		{
			//convert message to xml, then string
			XmlDocument xml = ToXml();
			return xml.OuterXml;
		}

		public XMMessage CreateReply()
		{
			//must already be valid
			if (!IsValid)
			{
				return null;
			}

			//create a reply to this message
			XMMessage reply = new XMMessage();
			reply.Connection = this.Connection;
			reply.Reply = this.Sequence;
			reply.Action = this.Action;
			return reply;
		}

		public void Send()
		{
			//send over the current connection
			Connection.SendMessage(this);
		}

		public void Process()
		{
			//message must already be valid
			if (!IsValid)
			{
				return;
			}

			//if we are doing anything but logging in, then
			//we need to authenticate the session
			if (Action!="login")
			{
				//attempt authentication
				if (!XMAuth.Authenticate(Connection, Session))
				{
					//failed to authenticate
					return;
				}
			}

			//perform action
			switch(Action)
			{
				case "login":		//initial login request
					DoLogin();
					break;

				case "listing":		//listing of media from client
					DoListing();
					break;

				case "query":		//media query from client
					DoQuery();
					break;

				case "index":		//index submission from client
					DoIndex();
					break;

				case "motd":		//motd response from client
					DoMotd();
					break;

				case "au":			//auto update file request
					DoAutoUpdate();
					break;

				case "ping":		//ping response was received
					DoPing();
					break;

				default:
					break;
			}
		}

		protected void DoPing()
		{
			//simply reset the ping-in-transit flag, the last activity
			//field will be updated by XMConnection.Alpha
			Connection.ResetKeepAlive();
		}

		protected void DoAutoUpdate()
		{
			//get data
			auMd5 = null;
			auPath = null;
			if (!XMAuth.AutoUpdateFile(Connection.Version, ref auPath, ref auMd5, ref auSize))
			{
				return;
			}

			//create reply message
			XMMessage msg = CreateReply();
			msg.auEnable = true;
			msg.auMd5 = auMd5;
			msg.auPath = auPath;
			msg.auSize = auSize;
			msg.Send();
		}

		protected void DoLogin()
		{
			//read the datarate, username, password
			XMMessage msg;
			int dr;
			string u, p;
			try 
			{
				dr = /*beta2:*/Convert.ToInt32(GetField("datarate").Value);
			}
			catch
			{
				dr = 0;
			}
			try
			{
				//snag version
				Connection.Version = GetField("version").Value.ToString();
			}
			catch
			{
				//version not given (version 0.50)
				Connection.Version = "0.50";
			}
			try 
			{
				u = GetField("username").Value.ToString();
				p = GetField("password").Value.ToString();
			}
			catch
			{
				//must specify username, password
				msg = CreateReply();
				msg.SetField("success", "false");
				msg.SetField("error", "Username and password must be specified.");
				msg.Send();
				return;
			}

			//new version?
			bool auLatest = true;
			bool auRequired = true;
			string auVersion = "";
			if (Connection.Version != "0.60")		//NOTE: current version
			{
				//fetch the record
				auLatest = false;
				if (!XMAuth.AutoUpdateCheck(Connection.Version, ref auVersion, ref auRequired))
				{
					//no db or bad version number
					msg = CreateReply();
					msg.SetField("success", "false");
					msg.SetField("error", "Could not get version info.");
					msg.Send();
					return;
				}

				//required?
				if (auRequired)
				{
					//login failure
					msg = CreateReply();
					msg.SetField("success", "false");
					msg.SetField("error", "Your AMS program is out of date.  Please visit http://www.adultmediaswapper.com to update your program.");
					msg.SetField("au/version", auVersion);
					msg.SetField("au/required", "true");
					msg.Send();	  
					return;
				}
			}

			//all parameters are good, pass to authentication module
			XMGuid session;
			try
			{
				session = XMAuth.Login(Connection, dr, u, p);
			}
			catch(Exception e)
			{
				//failed to log in
				msg = CreateReply();
				msg.SetField("success", "false");
				msg.SetField("error", e.Message);
				msg.Send();
				return;
			}

			//login successfull, return good reply
			msg = CreateReply();
			msg.SetField("success", "true");
			msg.SetField("session", session.ToString());
			msg.SetField("listing", "partial");		//dont need everything
			if(!Connection.Paying)
			{
				msg.SetField("limitindex", XMConnection.LimiterIndex);
				msg.SetField("limitfilter", XMConnection.LimiterFilter);
			}
			if (!auLatest)
			{
				msg.SetField("au/version", auVersion);
				msg.SetField("au/required", auRequired);
			}
			msg.Send();

			//update connection
			Connection.SessionID = session;

			//can we reach client?
			XMAuth.Callback(Connection);

			//send message of the day
			Connection.DoMOTD();
		}

		protected void DoQuery()
		{
			//if we are already doing a query, reject
			//this one
			XMMessage msg;
			if (Connection.InQuery)
			{
				msg = CreateReply();
				msg.Action = "error";
				msg.SetField("error", "You cannot run more than one simultaneous query.");
				msg.Send();
				return;
			}

			//are we allowed to execute this query?
			if (!Connection.Paying)
			{
				if ((Query.QueryMask.CountFields(true)>XMConnection.LimiterIndex) ||
					(Query.RejectionMask.CountFields(false)>XMConnection.LimiterFilter)	)
				{
					msg = CreateReply();
					msg.Action = "error";
					msg.SetField("error", "Query has too many fields.");
					msg.Send();
					return;
				}
			}

			//insert ourselves into the queue
			Connection.InQuery = true;
			XMServer.Engine.EnqueueQuery(this);
		}

		protected void DoIndex()
		{
			//return the requested indexes to the client
			string sql;
			bool all;
			XMMessageField field = (XMMessageField)Fields["all"];
			if (field!=null)
			{
				//return all media for this user
				all = true;
				sql =	"select m.md5 as 'media_md5', mi.*, m.width, m.height, m.filesize " +
						"from mediaindex mi " +
						"inner join media m on m.md5 = mi.md5 " +
						"where mi.userid = " + Connection.UserID.ToStringDB();
			}
			else
			{
				//return only media specified
				all = false;
				field = (XMMessageField)Fields["md5s"];
				if (field==null)
				{
					return;
				}

				//parse the string
				/*beta2:*/ArrayList sc = new ArrayList();
				string s = (string)field.Value;
				int i = 0;
				int j = s.IndexOf(';', i);
				while (j!=-1)
				{
					sc.Add(s.Substring(i, j-i));
					i = j+1;
					j = s.IndexOf(';', i);
				}
				if (sc.Count<1)
				{
					//nothing
					return;
				}

				//create sql
				StringBuilder sb = new StringBuilder(1024);
				sb.Append("select m.md5 as 'media_md5', mi.*, m.width, m.height, m.filesize ");
				sb.Append("from mediaindex mi ");
				sb.Append("inner join media m on m.md5 = mi.md5 ");
				sb.Append("where (");
				foreach(string t in sc)
				{
					sb.AppendFormat("mi.md5={0} or ", new XMGuid(t).ToStringDB());
				}
				sb.Append(" 1 = 0) ");
				sb.AppendFormat("and mi.userid = {0}", Connection.UserID.ToStringDB());
				sql = sb.ToString();
			}

			//execute query
			if (!mListingADO.EnsureConnection()) throw new Exception("No database.");
			ADODB._Recordset rs = mListingADO.SqlExec(sql);

			//convert to listing
			XMMediaItem mi = new XMMediaItem();
			XMMediaListing ml = new XMMediaListing();
			ml.Full = all;
			if (!rs.EOF)
			{
				XMQueryEngine.rs2mi(rs, mi);
				ml.MediaItems.Add(mi);
			}
			while (!rs.EOF)
			{
				mi = new XMMediaItem();
				XMQueryEngine.rs2mi(rs, mi);
				ml.MediaItems.Add(mi);
			}

			//send message
			XMMessage msg = this.CreateReply();
			msg.Listing = ml;
			msg.Action = "listing";
			msg.Send();
		}

		protected void DoMotd()
		{
			
		}

		protected static XMAdo mListingADO;
		protected void DoListing()
		{
			//check our connection
			if (mListingADO==null)
			{
				mListingADO = new XMAdo();
			}
			if (!mListingADO.EnsureConnection())
			{
				throw new Exception("No database.");
			}

			//ensure listing was found
			XMMessage msg;
			if (Listing==null)
			{	
				//no listing found
				msg = CreateReply();
				msg.Action = "error";
				msg.SetField("error", "Missing Listing");
				msg.Send();
				return;
			}

			//is this a full or partial?
			string sql;
			/*
			if (Listing.Full)
			{
				//remove from media storage
				//sql = "delete from mediastorage where userid = " + Connection.UserID.ToStringDB();
				//mListingADO.SqlExec(sql);

				//remove from media index
				//sql = "delete from mediaindex where userid = " + Connection.UserID.ToStringDB();
				//mListingADO.SqlExec(sql);
			}
			else
			{
				//Debugger.Break();
			}
			*/

			//walk list of media items
			XMGuid md5;
			foreach(XMMediaItem mi in Listing.MediaItems)
			{
				try 
				{
					//insert the media storage record (and media, if needed)
					md5 = new XMGuid(mi.Md5);
					if (mi.Action=="insert" || mi.Action=="update")
					{
						sql = "exec sp_insertmediastorage " +
							"  @uid="		+ Connection.UserID		.ToStringDB() + 
							", @md5="		+ md5					.ToStringDB() + 
							", @width="		+ mi.Width				.ToString() +
							", @height="	+ mi.Height				.ToString() + 
							", @filesize="	+ mi.FileSize			.ToString();
						mListingADO.SqlExec(sql);
					}
					else if(mi.Action=="remove")
					{
						sql = "exec sp_deletemediastorage @md5=" + md5.ToStringDB()
								+ ", @uid=" + Connection.UserID.ToStringDB();
						mListingADO.SqlExec(sql);
					}
					
					//insert each index
					for(int i=0;i<mi.Indices.Length;i++)
					{
						sql = "exec sp_insertmediaindex " + 

							"  @uid="			+ Connection.UserID			.ToStringDB() +
							", @md5="			+ md5						.ToStringDB() +
							", @cat1="			+ mi.Indices[i].Cat1		.ToString() + 
							", @cat2="			+ mi.Indices[i].Cat2		.ToString() + 
							", @age="			+ mi.Indices[i].Age			.ToString() + 
							", @breasts="		+ mi.Indices[i].Breasts		.ToString() + 
							", @build="			+ mi.Indices[i].Build		.ToString() + 
							", @butt="			+ mi.Indices[i].Butt		.ToString() + 
							", @chest="			+ mi.Indices[i].Chest		.ToString() + 
							", @content="		+ mi.Indices[i].Content		.ToString() + 
							", @eyes="			+ mi.Indices[i].Eyes		.ToString() + 
							", @facialhair="	+ mi.Indices[i].FacialHair	.ToString() + 
							", @femalegen="		+ mi.Indices[i].FemaleGen	.ToString() + 
							", @gender="		+ mi.Indices[i].Gender		.ToString() + 
							", @haircolor="		+ mi.Indices[i].HairColor	.ToString() + 
							", @hairstyle="		+ mi.Indices[i].HairStyle	.ToString() + 
							", @height="		+ mi.Indices[i].Height		.ToString() + 
							", @hips="			+ mi.Indices[i].Hips		.ToString() + 
							", @legs="			+ mi.Indices[i].Legs		.ToString() + 
							", @malegen="		+ mi.Indices[i].MaleGen		.ToString() + 
							", @nipples="		+ mi.Indices[i].Nipples		.ToString() + 
							", @quality="		+ mi.Indices[i].Quality		.ToString() + 
							", @quantity="		+ mi.Indices[i].Quantity	.ToString() + 
							", @race="			+ mi.Indices[i].Race		.ToString() + 
							", @rating="		+ mi.Indices[i].Rating		.ToString() + 
							", @setting="		+ mi.Indices[i].Setting		.ToString() + 
							", @skin="			+ mi.Indices[i].Skin		.ToString();							
						mListingADO.SqlExec(sql);
					}
					
				}
				catch(Exception e)
				{
					//probobly the item already exists, so we can
					//safely ignore this.
					XMLog.WriteLine(e.Message, "DoListing", EventLogEntryType.Warning);
				}
			}
		}
    }
}
