namespace XMedia
{
    using System;
	using ADODB;

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
			string sql = String.Format("select userid, paying from users where login='{0}' and password='{1}'",
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

			//get the userid, create the session id
			XMGuid user = new XMGuid((byte[])rs.Fields[0].Value);
			XMGuid session = new XMGuid(true);	//new session id
			bool paying = rs.Fields[1].Value.ToString().ToBoolean();

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
    }
}