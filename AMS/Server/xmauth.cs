namespace XMedia
{
    using System;
	using ADODB;
	using System.Net;
	using System.Diagnostics;

    public class XMGuid
	{
		//some static stuff for parsing guid strings
		private static byte[] sChars;
		private static Random sRandom;
		public static void Init()
		{
			//fill the schars array
			sChars = new byte[255];
			for (int i=0;i<sChars.Length;i++)
			{
				sChars[i] = 255;
			}
			sChars['0'] = 0;
			sChars['1'] = 1;
			sChars['2'] = 2;
			sChars['3'] = 3;
			sChars['4'] = 4;
			sChars['5'] = 5;
			sChars['6'] = 6;
			sChars['7'] = 7;
			sChars['8'] = 8;
			sChars['9'] = 9;
			sChars['a'] = 10;
			sChars['b'] = 11;
			sChars['c'] = 12;
			sChars['d'] = 13;
			sChars['e'] = 14;
			sChars['f'] = 15;

			//create randomizer
			sRandom = new Random();
		}

		protected byte[] mBuf;
		protected Random mRnd;
		public byte[] Buffer
		{
			get
			{
				return mBuf;
			}
			set
			{
				mBuf = value;
			}
		}
		public XMGuid()
		{
			//empty buffer
			mBuf = new byte[16];
		}
		public XMGuid(bool random)
		{
			//create buffer
			mBuf = new byte[16];

			//randomize?
			if (random)
			{
				sRandom.NextBytes(mBuf);
			}
		}
		public XMGuid(byte[] buf)
		{
			//attach to a buffer
			if (buf.Length!=16)
			{
				throw new Exception("Attempted to use invalid sized buffer for GUID.");
			}
			mBuf = buf;
		}
		public XMGuid(string guid)
		{
			mBuf = new byte[16];
			FromString(guid);
		}
		public override bool Equals(object e) 
		{
			//are the two buffers the same?
			byte[] buf = ((XMGuid)e).Buffer;
			for (int i=0;i<16;i++)
			{
				if (mBuf[i]!=buf[i])
				{
					//failed
					return false;
				}
			}
			return true;
		}
		public override int GetHashCode()
		{
			//xor the md5 down to 32 bits
			return (int)(
				((mBuf[0] << 0) | (mBuf[1] << 8) | (mBuf[2] << 16) | (mBuf[3] << 24)) ^
				((mBuf[4] << 0) | (mBuf[5] << 8) | (mBuf[6] << 16) | (mBuf[7] << 24)) ^
				((mBuf[8] << 0) | (mBuf[9] << 8) | (mBuf[10] << 16) | (mBuf[11] << 24)) ^
				((mBuf[12] << 0) | (mBuf[13] << 8) | (mBuf[14] << 16) | (mBuf[15] << 24)));
		}
		public override string ToString()
		{
			//convert to string
			System.Text.StringBuilder sb = new System.Text.StringBuilder(32,32);
			sb.AppendFormat("{0:x2}", mBuf[0]);
			sb.AppendFormat("{0:x2}", mBuf[1]);
			sb.AppendFormat("{0:x2}", mBuf[2]);
			sb.AppendFormat("{0:x2}", mBuf[3]);
			sb.AppendFormat("{0:x2}", mBuf[4]);
			sb.AppendFormat("{0:x2}", mBuf[5]);
			sb.AppendFormat("{0:x2}", mBuf[6]);
			sb.AppendFormat("{0:x2}", mBuf[7]);
			sb.AppendFormat("{0:x2}", mBuf[8]);
			sb.AppendFormat("{0:x2}", mBuf[9]);
			sb.AppendFormat("{0:x2}", mBuf[10]);
			sb.AppendFormat("{0:x2}", mBuf[11]);
			sb.AppendFormat("{0:x2}", mBuf[12]);
			sb.AppendFormat("{0:x2}", mBuf[13]);
			sb.AppendFormat("{0:x2}", mBuf[14]);
			sb.AppendFormat("{0:x2}", mBuf[15]);
			return sb.ToString();
		}
		public string ToStringDB()
		{
			//normal hex string, but with 0x prepended
			return "0x" + ToString();
		}
		public void FromString(string hex)
		{
			//remove 0x if found
			if (hex.StartsWith("0x"))
			{
				hex.Remove(0, 2);
			}

			//convert to char array
			char[] c = hex.ToCharArray();
			if (c.Length!=32)
			{
				throw new Exception("Hex string must contain exactly 16 bytes.");
			}

			//read all 16 bytes
			for (int i=0;i<16;i++)
			{
				mBuf[i] = (byte)(sChars[c[i*2]]*16);
				mBuf[i] += sChars[c[(i*2)+1]];
			}
		}
	}

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

			//validate username and password
			string sql = String.Format("select userid, paying, accesstoken from users where login='{0}' and password='{1}'",
						username, password);
			ADODB._Recordset rs = mAdo.SqlExec(sql);
			if (rs==null)
			{
				throw new Exception("Query failed.");
			}
			if (rs.EOF)
			{
				throw new Exception("Invalid login.");
			}

			//check the accesstoken.. if it ISNT null, this person is already
			//logged in
			if (rs.Fields["accesstoken"].Value == DBNull.Value)
			{
				throw new Exception("You are already logged in to a different computer.  Only one simultaneous login is allowed per username.  If you want to run AMS on more than one computer, you may create new accounts as you need them.");
			}

			//get the userid, create the session id
			XMGuid user = new XMGuid((byte[])rs.Fields[0].Value);
			XMGuid session = new XMGuid(true);	//new session id
			bool paying = /*beta2:*/System.Convert.ToBoolean(rs.Fields[1].Value);

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
			con.Paying = paying;

			return session;
		}

		public static void Callback(XMConnection con)
		{
			//if we can't make a connection back to the client, then
			//don't let it share any files.. no one will get them
			string sql;
			if (con.Ping())
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
					XMLog.WriteLine("Closing dorment connection.", "CheckConnections");
					c.Close();
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
