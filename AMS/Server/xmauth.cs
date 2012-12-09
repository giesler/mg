namespace XMedia
{
    using System;
	using System.Data;
	using System.Data.SqlClient;
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
			SqlDataReader rs = mAdo.SqlExec(sql);
			if (!rs.Read())
			{
				return false;
			}

			//success
			newVersion = rs["newversion"].ToString();
			required = Convert.ToBoolean(rs["required"]);
			rs.Close();
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
			mAdo.SqlExecNoResults(sql);
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
			SqlDataReader rs = mAdo.SqlExec(sql);
			if (!rs.Read())
			{
				rs.Close();
				return false;
			}

			//success
			path = rs["path"].ToString();
			md5 = new XMGuid((byte[])rs["md5"]);
			size = Convert.ToInt32(rs["length"]);
			rs.Close();
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
				@"select u.userid, u.password, u.paying, u.accesstoken, u.hostip,
				(select count(*) from mediastorage ms where ms.userid = u.userid) as 'filecount'
				from users u
				where u.login='{0}'",
				username);
			SqlDataReader rs = mAdo.SqlExec(sql);
			if (rs==null)
			{
				throw new Exception("Query failed.");
			}
			if (!rs.Read())
			{
				rs.Close();
				throw new Exception("Invalid login.");
			}

			//validate password... we now accept md5 shadows of the actual pwd
			string pwd = rs["password"].ToString();
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
			if (rs["accesstoken"] != DBNull.Value)
			{
				//is the old ip same as the new ip?
				alreadyLoggedIn = true;
				if (rs["hostip"] != DBNull.Value)
				{
					if (rs["hostip"].ToString() == con.HostIP.ToString())
					{
						//its ok
						alreadyLoggedIn = false;
					}
				}

				if (alreadyLoggedIn)
					throw new Exception("You are already logged in to a different computer.  Only one simultaneous login is allowed per username.  If you want to run AMS on more than one computer, you may create new accounts as you need them.");
			}

			//get the userid, create the session id
			XMGuid user = new XMGuid((byte[])rs[0]);
			XMGuid session = new XMGuid(true);	//new session id
			
			//update the connection
			con.Username = username;
			con.SessionID = session;
			con.UserID = user;
			con.Datarate = datarate;
			con.Paying = Convert.ToBoolean(rs["paying"]);
			con.FileCount = Convert.ToInt32(rs["filecount"]);
			rs.Close();

			//update the database with the new session
			System.Text.StringBuilder sb = new System.Text.StringBuilder(200, 200);
			sb.Append("insert into userslogins(AccessToken, DataRate, DateStamp, HostIP, UserID) ");
			sb.Append("values(");
			sb.Append(session.ToStringDB());
			sb.Append(", ");
			sb.Append(datarate.ToString());
			sb.Append(", GetDate(), '");
			sb.Append(con.HostIP.ToString());
			sb.Append("', ");
			sb.Append(user.ToStringDB());
			sb.Append(")");
			mAdo.SqlExecNoResults(sb.ToString());
			mAdo.SqlExecNoResults("update users set datarate=" + datarate.ToString() + ", hostip='" + con.HostIP.ToString() + "', accesstoken = " + session.ToStringDB() + 
					"where userid = " + user.ToStringDB());

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
				mAdo.SqlExecNoResults(sql);
			}
		}

		public static void KillSession(XMGuid sid)
		{
			//remove any working data for a session
			string sql = "exec sp_clearsessiondata " + sid.ToStringDB();
			if (EnsureConnection()) 
			{
				mAdo.SqlExecNoResults(sql);	
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
					c.LastActivity < (DateTime.Now - XMConfig.NetDormantTimeout))
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
					if (c.LastActivity < (DateTime.Now - XMConfig.NetLinkDeadTimeout))
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
			DataView rs;
			try
			{
				rs = mAdo.SqlExecDataView("select * from users where online=1");
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
			for (int i=0;i<rs.Count;i++)
			{
				kill = true;
				try
				{
					//get accesstoken and ip
					uid = new XMGuid((byte[])rs[i]["userid"]);
					at = new XMGuid((byte[])rs[i]["accesstoken"]);
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
					XMLog.WriteLine("Kicking user: " + rs[i]["login"], "CheckConnections", EventLogEntryType.Warning);
					string str = String.Format("update users set online=0, accesstoken=null where userid={0}", uid.ToStringDB());				
					mAdo.SqlExecNoResults(str);
				}
			}
		}
    }
}
