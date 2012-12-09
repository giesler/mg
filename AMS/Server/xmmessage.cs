namespace XMedia
{
    using System;
	using System.Diagnostics;
	using System.Collections;
	using System.Xml;
	using System.Text;
	using System.Data;
	using System.Data.SqlClient;

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
				m.Height = Convert.ToInt32(e.GetAttribute("height"));
				m.Width = Convert.ToInt32(e.GetAttribute("width"));
				m.FileSize = Convert.ToInt32(e.GetAttribute("filesize"));
		
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
		public ArrayList QueryResults;

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
			Sequence = Convert.ToInt32(root.GetAttribute("sequence"));
			Reply = Convert.ToInt32(root.GetAttribute("reply"));

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
			int dr, files;
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
			try
			{
				//get the file count
				files = Convert.ToInt32(GetField("filecount").Value);
			}
			catch
			{
				files = 0;
			}

			//new version?
			bool auLatest = true;
			bool auRequired = true;
			string auVersion = "";
			if (Connection.Version != "0.70")		//NOTE: current version
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

			//if the client has the same # of files as in xmcatalog, then
			//we only need new stuff.. if the counts don't match, then they
			//are proboly changed computers or something, and we need everytyhing
			msg.SetField("listingsize", Connection.FileCount==files?"partial":"full");

			if (Connection.Username=="cache")
			{
				Trace.WriteLine(String.Format("*** asking cache for {0}", msg.GetField("listingsize").Value));
			}

			//set limiters on the queries?
			if(!Connection.Paying)
			{
				msg.SetField("limitindex", XMConnection.LimiterIndex);
				msg.SetField("limitfilter", XMConnection.LimiterFilter);
			}

			//optional auto-upgrade info?
			if (!auLatest)
			{
				msg.SetField("au/version", auVersion);
				msg.SetField("au/required", auRequired);
			}

			//send the msg
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
				if ((Query.QueryMask.CountBits(true)>XMConnection.LimiterIndex) ||
					(Query.RejectionMask.CountBits(false)>XMConnection.LimiterFilter)	)
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
				//ArrayList sc = new ArrayList();
				string[] s = ((string)field.Value).Split(';');
				/*
				int i = 0;
				int j = s.IndexOf(';', i);
				while (j!=-1)
				{
					sc.Add(s.Substring(i, j-i));
					i = j+1;
					j = s.IndexOf(';', i);
				}
				*/
				if (s.Length<1)
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
				foreach(string t in s)
				{
					sb.AppendFormat("(mi.md5={0}) or ", new XMGuid(t).ToStringDB());
				}
				sb.Append(" (1 = 0)) ");
				sb.AppendFormat("and mi.userid = {0}", Connection.UserID.ToStringDB());
				sql = sb.ToString();
			}

			//execute query
			if (!mListingADO.EnsureConnection()) throw new Exception("No database.");
			SqlDataReader rs = mListingADO.SqlExec(sql);

			//convert to listing
			XMMediaItem mi = new XMMediaItem();
			XMMediaListing ml = new XMMediaListing();
			ml.Full = all;
			while (rs.Read())
			{
				mi = new XMMediaItem();
				XMQueryEngine.rs2mi(rs, mi);
				ml.MediaItems.Add(mi);
			}
			rs.Close();

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
			if (Listing.Full)
			{
				//remove from media storage
				sql = "delete from mediastorage where userid = " + Connection.UserID.ToStringDB();
				mListingADO.SqlExecNoResults(sql);

				//remove from media index
				//sql = "delete from mediaindex where userid = " + Connection.UserID.ToStringDB();
				//mListingADO.SqlExec(sql);
			}
			else
			{
				//Debugger.Break();
			}
			
			//walk list of media items
			XMGuid md5;
			StringBuilder sb;
			int c = 0;
			/*TEMP*/// int j = 0;
			/*TEMP*/// SqlDataReader rstemp;
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

						///<TEMP>
						//XMAdo ado = new XMAdo();
						//ado.EnsureConnection();
						mListingADO/*ado*/.SqlExecNoResults(sql);
						///</TEMP>
						c++;

						///<TEMP>
						/*
						j++;
						sql = "select count(*) from mediastorage where userid=0x9A564D7321999DE9C8A8F5BC831141CB";
						rstemp = mListingADO.SqlExec(sql);
						rstemp.Read();
						if ((int)rstemp[0] != j)
						{
							//aha!
							Debug.Assert(false);
							j--;
						}
						rstemp.Close();
						*/
						///</TEMP>
					}
					else if(mi.Action=="remove")
					{
						sql = "exec sp_deletemediastorage @md5=" + md5.ToStringDB()
								+ ", @uid=" + Connection.UserID.ToStringDB();
						mListingADO.SqlExecNoResults(sql);
					}
					
					//insert each index
					for(int i=0;i<mi.Indices.Length;i++)
					{
						sb = new StringBuilder(512);
						sb.Append("exec sp_insertmediaindex ");
						sb.Append("  @uid=");		sb.Append(Connection.UserID.ToStringDB());
						sb.Append(", @md5=");		sb.Append(md5.ToStringDB());
						sb.Append(", @cat1=");		sb.Append(mi.Indices[i].Cat1);
						sb.Append(", @cat2=");		sb.Append(mi.Indices[i].Cat2);
						sb.Append(", @age=");		sb.Append(mi.Indices[i].Age);
						sb.Append(", @breasts=");	sb.Append(mi.Indices[i].Breasts);
						sb.Append(", @build=");		sb.Append(mi.Indices[i].Build);
						sb.Append(", @butt=");		sb.Append(mi.Indices[i].Butt);
						sb.Append(", @chest=");		sb.Append(mi.Indices[i].Chest);
						sb.Append(", @content=");	sb.Append(mi.Indices[i].Content);
						sb.Append(", @eyes=");		sb.Append(mi.Indices[i].Eyes);
						sb.Append(", @facialhair=");sb.Append(mi.Indices[i].FacialHair);
						sb.Append(", @femalegen=");	sb.Append(mi.Indices[i].FemaleGen);
						sb.Append(", @haircolor=");	sb.Append(mi.Indices[i].HairColor);
						sb.Append(", @hairstyle=");	sb.Append(mi.Indices[i].HairStyle);
						sb.Append(", @height=");	sb.Append(mi.Indices[i].Height);
						sb.Append(", @hips=");		sb.Append(mi.Indices[i].Hips);
						sb.Append(", @legs=");		sb.Append(mi.Indices[i].Legs);
						sb.Append(", @malegen=");	sb.Append(mi.Indices[i].MaleGen);
						sb.Append(", @nipples=");	sb.Append(mi.Indices[i].Nipples);
						sb.Append(", @quality=");	sb.Append(mi.Indices[i].Quality);
						sb.Append(", @quantity=");	sb.Append(mi.Indices[i].Quantity);
						sb.Append(", @race=");		sb.Append(mi.Indices[i].Race);
						sb.Append(", @rating=");	sb.Append(mi.Indices[i].Rating);
						sb.Append(", @setting=");	sb.Append(mi.Indices[i].Setting);
						sb.Append(", @skin=");		sb.Append(mi.Indices[i].Skin.ToString());							

						//append score if it is a contest index
						sb.Append(", @score=");
						if (mi.Indices[i].Source==XMIndex.SourceEnum.Contest)
							sb.Append(mi.Indices[i].Score());
						else
							sb.Append(0);
						
						mListingADO.SqlExecNoResults(sb.ToString());
					}
				}
				catch(Exception e)
				{
					//probobly the item already exists, so we can
					//safely ignore this.
					XMLog.WriteLine(e.Message, "DoListing", EventLogEntryType.Warning);
				}
			}
			
			//print out number of items we added
			if (Connection.Username=="cache")
			{
				Trace.WriteLine(String.Format("*** added {0} items for cache", c));
			}
		}
    }
}
