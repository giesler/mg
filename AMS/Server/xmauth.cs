namespace XMedia
{
    using System;
	using ADODB;
	using System.Net;
	using System.Diagnostics;


    /// <summary>
    ///    Summary description for xmauth.
    /// </summary>
    public class XMAuth
    {
		//sql connection
		private static XMAdo mAdo;

		//helper for ado connection
		private static bool EnsureConnection()
		{
			if (mAdo==null)
			{
				mAdo = new XMAdo();
			}

			return mAdo.EnsureConnection();
		}

        public XMAuth()
        {
        }

		/// <summary>
		/// Retrieve auto update information during login
		/// </summary>
		/// <param name="version">Version reported in login message.</param>
		/// <param name="newVersion">New version number.</param>
		/// <param name="required">Can the old version still be used?</param>
		public static bool AutoUpdateCheck(string version, ref string newVersion, ref bool required)
		{
			if (!EnsureConnection())
			{
				return false;
			}

			//fetch new data
			string sql = "select newversion, required from versions where oldversion='" + version + "'";
			ADODB._Recordset rs = mAdo.SqlExec(sql);
			if (rs.EOF)
			{
				return false;
			}

			//success
			newVersion = rs.Fields["newversion"].Value.ToString();
			required = System.Convert.ToBoolean(rs.Fields["required"].Value);
			return true;
		}

		/// <summary>
		/// Set offline bit to 0 for everyone
		/// </summary>
		static public void KickAll()
		{
			if (!EnsureConnection())
			{
				return;
			}

			try 
			{	
			string sql = "update users set online=0, accesstoken=null";
			mAdo.SqlExec(sql);
			}
			catch
			{
				XMLog.WriteLine("Could not kick everyone.", "KickAll", EventLogEntryType.Error);
			}
		}

		/// <summary>
		/// Return the data needed to send an auto update file.
		/// </summary>
		/// <param name="version">Current version.</param>
		/// <param name="path">[out] Relative path to the update file.</param>
		/// <param name="md5">[out] MD5 sum of <path></param>
		public static bool AutoUpdateFile(string version, ref string path, ref XMGuid md5, ref int size)
		{
			if (!EnsureConnection())
			{
				return false;
			}

			//fetch new data
			string sql = "select path, md5, length from versions where oldversion='" + version + "'";
			ADODB._Recordset rs = mAdo.SqlExec(sql);
			if (rs.EOF)
			{
				return false;
			}

			//success
			path = rs.Fields["path"].Value.ToString();
			md5 = new XMGuid((byte[])rs.Fields["md5"].Value);
			size = (int)rs.Fields["length"].Value;
			return true;
		}

		public static XMGuid Login(XMConnection con, int datarate, string username, string password)
		{
			if (!EnsureConnection())
			{
				//failed to open connection
				throw new Exception("No database.");
			}

			//is session already logged in?
			if (con.SessionID!=null)
			{
				throw new Exception("Session already logged in.");
			}

			//load user data
			string sql = String.Format(
				@"select u.userid, u.password, u.paying, u.accesstoken,
				(select count(*) from mediastorage ms where ms.userid = u.userid) as 'filecount'
				from users u
				where u.login='{0}'",
				username);
			ADODB._Recordset rs = mAdo.SqlExec(sql);
			if (rs==null)
			{
				throw new Exception("Query failed.");
			}
			if (rs.EOF)
			{
				throw new Exception("Invalid login.");
			}

			//validate password... we now accept md5 shadows of the actual pwd
			string pwd = rs.Fields["password"].Value.ToString();
			if (pwd != password)
			{
				//plain text does not match.. generate md5s from the password
				//in the db and compare to what was sent by the user
				XMGuid passDB = XMMd5Engine.FromString(pwd);
				XMGuid passLocal = new XMGuid(password);
				if (!passDB.Equals(passLocal))
				{
					throw new Exception("Invalid login.");
				}
			}

			//check the accesstoken.. if it ISNT null, this person is already
			//logged in
			bool alreadyLoggedIn;
			if (rs.Fields["accesstoken"].Value != DBNull.Value)
			{
				//is the old ip same as the new ip?
				alreadyLoggedIn = true;
				if (rs.Fields["hostip"].Value != DBNull.Value)
				{
					if (rs.Fields["hostip"].Value.ToString() == con.HostIP.ToString())
					{
						//its ok
						alreadyLoggedIn = false;
					}
				}

				if (alreadyLoggedIn)
					throw new Exception("You are already logged in to a different computer.  Only one simultaneous login is allowed per username.  If you want to run AMS on more than one computer, you may create new accounts as you need them.");
			}

			//get the userid, create the session id
			XMGuid user = new XMGuid((byte[])rs.Fields[0].Value);
			XMGuid session = new XMGuid(true);	//new session id
			
			//update the database with the new session
			System.Text.StringBuilder sb = new System.Text.StringBuilder(200, 200);
			sb.Append("insert into userslogins(AccessToken, DataRate, DateStamp, HostIP, UserID) ");
			sb.Append("values(");
			sb.Append(session.ToStringDB());
			sb.Append(", ");
			sb.Append(datarate.ToString());
			sb.Append(", GetDate(), '");
			sb.Append("todo");
			sb.Append("', ");
			sb.Append(user.ToStringDB());
			sb.Append(")");
			mAdo.SqlExec(sb.ToString());
			mAdo.SqlExec("update users set datarate=" + datarate.ToString() + ", hostip='" + con.HostIP.ToString() + "', accesstoken = " + session.ToStringDB() + 
					"where userid = " + user.ToStringDB());

			//update the connection
			con.Username = username;
			con.SessionID = session;
			con.UserID = user;
			con.Datarate = datarate;
			con.Paying = Convert.ToBoolean(rs.Fields["paying"].Value);
			con.FileCount = Convert.ToInt32(rs.Fields["filecount"].Value);

			return session;
		}

		public static void Callback(XMConnection con)
		{
			//if we can't make a connection back to the client, then
			//don't let it share any files.. no one will get them
			string sql;
			if (con.ExternalPing())
			{
				sql = "update users set online=1 where userid=" + con.UserID.ToStringDB();
			}
			else
			{
				sql = "update users set online=0 where userid=" + con.UserID.ToStringDB();
			}
			if (mAdo.EnsureConnection())
			{
				mAdo.SqlExec(sql);
			}
		}

		public static void KillSession(XMGuid sid)
		{
			//remove any working data for a session
			string sql = "exec sp_clearsessiondata " + sid.ToStringDB();
			if (EnsureConnection()) 
			{
				mAdo.SqlExec(sql);	
			}
		}

		public static bool Authenticate(XMConnection con, string session)
		{
			if (!EnsureConnection())
			{
				//failed to open connection
				throw (new Exception("No database."));
			}

			//if session if older than 30 minutes, force another login
			/*
			 * if (con.LastActivity.AddMinutes(30).CompareTo(DateTime.Now)<0)
			{
				//timed out
				return false;
			}

			//verify the session id
			string sql = String.Format(
				"select max(datestamp) from userslogins where accesstoken = {0}
			*/
			return true;
		}

		/// <summary>
		/// Makes sure that every user record from the database that is marked
		/// onlne actually has an open connection.
		/// </summary>
		public static void CheckConnections()
		{
			//Trace.WriteLine("Checking connections...");
			
			//kill any dorment connections
			foreach(XMConnection c in XMConnection.Connections)
			{
				//must be null session id, and no recent activity
				if (c.SessionID == null &&
					c.LastActivity < DateTime.Now.AddMinutes(-10))
				{
					//probobly some sort of mis-hap during login, before the
					//session id could get set. if something happens after
					//the session id is set, it will be caught by the next
					//test after 20 minutes.
					XMLog.WriteLine("Closing dorment connection: " + c.Username, "CheckConnections");
					c.Close();
				}
				else
				{
					//if the last activity is older than 20 minutes, we assume
					//that the connection has gone dead.. "ping"s are sent over
					//the open connection if more than 15 minutes elapses without
					//any activity, giving a maximum of 5 minutes for the client
					//to respond before disconnection.
					if (c.LastActivity < DateTime.Now.AddMinutes(-20))
					{
						XMLog.WriteLine("Closing link-dead connection: " + c.Username, "CheckConnections");
						c.Close();
					}
				}
				
			}

			//open db cnnection
			if (!EnsureConnection())
				return;

			//get all users where online=1
			ADODB._Recordset rs;
			try
			{
				rs = mAdo.SqlExec("select * from users where online=1");
			}
			catch(Exception e)
			{
				XMLog.WriteLine(e.Message, "CheckConnections", EventLogEntryType.Error);
				return;
			}

			//walk recordset
			XMGuid uid = null;
			XMGuid at = null;
			//IPAddress ip;
			bool kill;
			object[] cons = XMConnection.Connections;
			while (!rs.EOF)
			{
				kill = true;
				try
				{
					//get accesstoken and ip
					uid = new XMGuid((byte[])rs.Fields["userid"].Value);
					at = new XMGuid((byte[])rs.Fields["accesstoken"].Value);
					//ip = IPAddress.Parse((string)rs.Fields["hostip"].Value);

					//search open connections
					//note: we don't need to sync since this array was copied
					foreach(XMConnection c in cons)
					{
						//test this connection
						if (c.SessionID != null)
						{
							if (c.SessionID.Equals(at)/* &&
								c.HostIP.Equals(ip)*/)
							{
								//connection is alive
								kill = false;
								break;
							}
						}
						else
						{
							Trace.WriteLine("CheckConnections: Skipping null session id.");
						}
					}
				}
				catch(Exception e)
				{
					//we can continue, but make sure to move this user offline
					Trace.WriteLine("Error checking for connection:\n"+e.ToString());
					kill = true;
				}

				//if we couldn't find the connection, or there was an error
				//we will set the online flag to 0
				if (kill && uid!=null)
				{
					XMLog.WriteLine("Kicking user: " + rs.Fields["login"].Value, "CheckConnections", EventLogEntryType.Warning);
					string str = String.Format("update users set online=0, accesstoken=null where userid={0}", uid.ToStringDB());				
					mAdo.SqlExec(str);
				}

				//next record
				rs.MoveNext();
			}
		}
    }
}
