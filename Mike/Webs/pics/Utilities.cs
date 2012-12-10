using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace pics
{
	/// <summary>
	/// Common variables and such for this app
	/// </summary>
	public class Config 
	{
		protected static String _connectionstring;
		protected static String _smptserver;
		protected static String _picturedirectory;

		#region Public static properties
		/// <summary>
		/// Gets the application's connection string
		/// </summary>
		public static String ConnectionString 
		{
			get 
			{ 
				if (_connectionstring == null) 
				{
					_connectionstring = ConfigurationSettings.AppSettings["ConnectionString"];
				}
				return _connectionstring;
			}
		}

		/// <summary>
		/// Gets the applications configured SMTP server
		/// </summary>
		public static String SMTPServer 
		{
			get 
			{
				if (_smptserver == null) 
				{
					_smptserver = ConfigurationSettings.AppSettings["SMTPServer"];
				}
				return _smptserver;
			}
		}

		/// <summary>
		/// Gets the applications configured temporary picture directory
		/// </summary>
		public static String PictureDirectory
		{
			get 
			{
				if (_picturedirectory == null) 
				{
					_picturedirectory = ConfigurationSettings.AppSettings["PictureDirectory"];
				}
				return _picturedirectory;
			}
		}

		#endregion
	}

	public class Msn2Mail
	{
		public static string BuildMessage(string messageHtml)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);
			sb.Append("<html><body topmargin=\"0\" leftmargin=\"0\">");
			sb.Append("<table width=\"100%\" style=\"");
			sb.Append("filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#007300', EndColorStr='#000000'):");
			sb.Append("\"><tr height=\"5\"><td></td></tr></table>");	
			sb.Append("<blockquote>");
			sb.Append(messageHtml);
			sb.Append("</blockquote></body></html>");

			return sb.ToString();
		}
	}

	/// <summary>
	/// Summary description for Utilities.
	/// </summary>
	public class Utilities
	{
		public Utilities()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}

	/// <summary>
	/// Retreives user information
	/// </summary>
	public class PersonInfo 
	{
		#region Declares
		protected int _PersonID = 0;
		protected String _Name;
		protected String _Email;
		protected bool _Valid;
		protected bool _ValidEmail;
		#endregion

		#region Constructors/Loaders
		/// <summary>
		/// Retreives user information for the specified PersonID
		/// </summary>
		/// <param name="_personid">ID of the person</param>
		public PersonInfo(int _personid) 
		{
            // set up a connection and command to retreive info
			SqlConnection cn	= new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]);
            SqlCommand cmd		= new SqlCommand("dbo.sp_PersonInfo", cn);
			cmd.CommandType		= CommandType.StoredProcedure;
			cmd.Parameters.Add("@PersonID", SqlDbType.Int);
			cmd.Parameters["@PersonID"].Value = _personid;

			try 
			{
                cn.Open();
				LoadProps(cmd);
			}
			catch (Exception excep) 
			{
                System.Diagnostics.Trace.Write(excep.ToString());
			}
			finally 
			{
				// make sure connection is closed
				if (cn.State == ConnectionState.Open)
					cn.Close();
			}
			
		}


		public PersonInfo(String _email, String _password) 
		{

			System.Web.HttpContext.Current.Trace.Write("PersonInfo", "Starting");

			// set up a connection and command to retreive info
			SqlConnection cn	= new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]);
			SqlCommand cmd		= new SqlCommand("dbo.sp_Login", cn);
			cmd.CommandType		= CommandType.StoredProcedure;
			cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 150);
			cmd.Parameters.Add("@ValidEmail", SqlDbType.Bit);
			cmd.Parameters["@ValidEmail"].Direction	= ParameterDirection.Output;
			cmd.Parameters["@Email"].Value	  = _email;
			cmd.Parameters["@Password"].Value = _password;

			try 
			{
				cn.Open();
				LoadProps(cmd);

				// Find out if we had a valid email
				_ValidEmail		= Convert.ToBoolean(cmd.Parameters["@ValidEmail"].Value);

				System.Web.HttpContext.Current.Trace.Write("ValidEmail", _ValidEmail.ToString());
			}
			catch (Exception excep) 
			{
				HttpContext.Current.Trace.Warn("PersonInfo Constructor", excep.ToString(), excep);
			}
			finally 
			{
				// make sure connection is closed
				if (cn.State == ConnectionState.Open)
					cn.Close();
			}

		}

		private void LoadProps(SqlCommand cmd) 
		{
			// Set up a reader to read data
			SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);

			// attempt to read
			if (dr.Read()) 
			{

				// set the props on this object
				_PersonID	= Convert.ToInt32(dr["PersonID"]);
				_Name		= dr["FullName"].ToString();
				_Email		= dr["Email"].ToString();

				_Valid		= true;

			} 
			else 
			{
				_Valid = false;
			}

			System.Web.HttpContext.Current.Trace.Write("PersonInfo.LoadProps", "Loading");

			// Check if a 'ValidEmail' param
			if (cmd.Parameters.Contains("@ValidEmail"))
			{
				_ValidEmail		= Convert.ToBoolean(cmd.Parameters["@ValidEmail"].Value);
			}

			dr.Close();

		}

		#endregion

		#region Peron properties
		public int PersonID 
		{
			get { return _PersonID; }
		}

		public String Name 
		{
			get { return _Name; }
		}

		public String Email
		{
			get { return _Email; }
		}

		public bool Valid 
		{
			get { return _Valid; }
		}

		public bool ValidEmail
		{
			get 
			{
				return _ValidEmail; 
			}
		}

		#endregion

	}

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
				// replace the hex value with all but first two chars
				hex = hex.Remove(0, 2);
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
}
