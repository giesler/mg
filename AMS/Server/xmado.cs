namespace XMedia
{
    using System;
	using System.Diagnostics;
	using System.Data;
	using System.Data.SqlClient;
	using System.Text;

	public class XMAdo
	{
		private SqlConnection mConnection;

		public bool EnsureConnection()
		{
			//make sure that our database is connected
			if (mConnection==null)
			{
				mConnection = new SqlConnection();
			}
			if (mConnection.State == ConnectionState.Open)
			{
				return true;
			}

			//build connection string
			string con = String.Format(
							"Data Source={0};Initial Catalog={1}",
							XMConfig.DBServer,
							XMConfig.DBDatabase);
			if (XMConfig.DBUseNT)
			{
				con += ";Integrated Security=SSPI";
			}
			else
			{
				con += String.Format(
					";User Id={0};Password={1}",
					XMConfig.DBUsername,
					XMConfig.DBPassword);
			}

			//open database
			try 
			{
				mConnection.ConnectionString = con;
				mConnection.Open();
			}
			catch(Exception e)
			{
				//failed
				XMLog.WriteLine("Database connection failed: " + e.Message, "Database", EventLogEntryType.Error);
				return false;
			}

			//database is now conencted
			return true;
		}

		public SqlDataReader SqlExec(string s)
		{
			//helper function
			SqlCommand cmd = mConnection.CreateCommand();
			cmd.CommandText = s;
			cmd.CommandType = CommandType.Text;
			return cmd.ExecuteReader();
		}

		public void SqlExecNoResults(string s)
		{
			//helper function
			SqlCommand cmd = mConnection.CreateCommand();
			cmd.CommandText = s;
			cmd.CommandType = CommandType.Text;
			cmd.ExecuteNonQuery();
		}

		public DataView SqlExecDataView(string s)
		{
			//helper function
			DataSet ds = new DataSet();
			SqlDataAdapter cmd = new SqlDataAdapter(s, mConnection);
			cmd.Fill(ds);
			return ds.Tables[0].DefaultView;
		}
	}

	public class XMMd5Engine
	{
		public XMMd5Engine()
		{
			init();
		}

		//static helpers
		public static XMGuid FromString(string input)
		{
			XMMd5Engine e = new XMMd5Engine();
			e.Update(input);
			e.Finish();
			return e.Md5;
		}

		// next, the private data:
		private UInt32[] state = new UInt32[4];
		private UInt32[] count = new UInt32[2];     // number of *bits*, mod 2^64
		private Byte[] buffer = new Byte[64];		// input buffer
		private Byte[] digest = new Byte[16];
		private bool finalized;
		private Char[] mtempchar = new Char[33];

		private static Byte[] PADDING = new Byte[64]{
														0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
														0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
														0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
													};

		/*
		License to copy and use this software is granted provided that it
		is identified as the "RSA Data Security, Inc. MD5 Message-Digest
		Algorithm" in all material mentioning or referencing this software
		or this function.

		License is also granted to make and use derivative works provided
		that such works are identified as "derived from the RSA Data
		Security, Inc. MD5 Message-Digest Algorithm" in all material
		mentioning or referencing the derived work.

		*/

		public void Update(string input)
		{
			//convert the string to a byte array
			byte[] buf = ASCIIEncoding.ASCII.GetBytes(input);
			Update(buf, (UInt32)buf.Length);
		}

		public void Update(Byte[] input, UInt32 input_length)
		{
			UInt32 input_index, buffer_index;
			UInt32 buffer_space;                // how much space is left in buffer

			if (finalized)
			{
				throw new Exception("Cannot update after finalize has been called.");	
			}

			// Compute number of bytes mod 64
			buffer_index = (UInt32)((count[0] >> 3) & 0x3F);

			// Update number of bits
			if (  (count[0] += ((UInt32) input_length << 3))<((UInt32) input_length << 3) )
				count[1]++;

			count[1] += ((UInt32)input_length >> 29);


			buffer_space = 64 - buffer_index;  // how much space is left in buffer

			// Transform as many times as possible.
			if (input_length >= buffer_space)
			{
				// ie. we have enough to fill the buffer
				// fill the rest of the buffer and transform
				//memcpy (buffer + buffer_index, input, buffer_space);
				//memcpy(dest, src, size)
				Array.Copy(input, 0, buffer, (int)buffer_index, (int)buffer_space);
				transform(buffer);

				// now, transform each 64-byte piece of the input, bypassing the buffer
				for (input_index = buffer_space; input_index + 63 < input_length; input_index += 64)
				{
					Byte[] temp = new Byte[64];
					Array.Copy(input, (int)input_index, temp, 0, 64);
					transform(temp /*input+input_index*/);
				}

				buffer_index = 0;  // so we can buffer remaining
			}
			else
			{
				input_index=0;     // so we can buffer the whole input
			}

			// and here we do the buffering:
			//memcpy(buffer+buffer_index, input+input_index, input_length-input_index);
			Array.Copy(input, (int)input_index, buffer, (int)buffer_index, (int)(input_length-input_index));
		}

		// MD5 finalization. Ends an MD5 message-digest operation, writing the
		// the message digest and zeroizing the context.
		public void Finish()
		{
			Byte[] bits = new Byte[8];
			UInt32 index;
			UInt32 padLen;
			
			if (finalized)
			{
				return;
			}

			// Save number of bits
			encode(ref bits, count, 8);

			// Pad out to 56 mod 64.
			index = (UInt32) ((count[0] >> 3) & 0x3f);
			padLen = (index < 56) ? (56 - index) : (120 - index);
			Update (PADDING, padLen);

			// Append length (before padding)
			Update(bits, 8);

			// Store state in digest
			encode(ref digest, state, 16);

			// Zeroize sensitive information
			for(int i=0;i<buffer.Length;i++)
				buffer[i]=0;

			finalized = true;
		}

		public Byte[] Raw
		{
			get
			{
				if (!finalized)
				{
					throw new Exception("Call finalize() before retrieving the MD5.");
				}
				return digest;
			}
		}

		public XMGuid Md5
		{
			get
			{	
				if (!finalized)
				{
					throw new Exception("Call finalize() before retrieving the MD5.");
				}
				return new XMGuid(digest);
			}
		}

		// PRIVATE METHODS:
		private void init()
		{
			finalized = false;  // we just started!

			// Nothing counted, so count=0
			count[0] = 0;
			count[1] = 0;

			// Load magic initialization constants.
			state[0] = 0x67452301;
			state[1] = 0xefcdab89;
			state[2] = 0x98badcfe;
			state[3] = 0x10325476;
		}

		// Constants for MD5Transform routine.
		// Although we could use C++ style constants, defines are actually better,
		// since they let us easily evade scope clashes.

		private const UInt32 S11 = 7;
		private const UInt32 S12 = 12;
		private const UInt32 S13 = 17;
		private const UInt32 S14 = 22;
		private const UInt32 S21 = 5;
		private const UInt32 S22 = 9;
		private const UInt32 S23 = 14;
		private const UInt32 S24 = 20;
		private const UInt32 S31 = 4;
		private const UInt32 S32 = 11;
		private const UInt32 S33 = 16;
		private const UInt32 S34 = 23;
		private const UInt32 S41 = 6;
		private const UInt32 S42 = 10;
		private const UInt32 S43 = 15;
		private const UInt32 S44 = 21;

		// MD5 basic transformation. Transforms state based on block.
		private void transform (Byte[] block)
		{
			UInt32
				a = state[0],
				b = state[1],
				c = state[2],
				d = state[3];
			UInt32[]
				x = new UInt32[16];

			decode(ref x, block, 64);

			/* Round 1 */
			FF (ref a, b, c, d, x[ 0], S11, 0xd76aa478); /* 1 */
			FF (ref d, a, b, c, x[ 1], S12, 0xe8c7b756); /* 2 */
			FF (ref c, d, a, b, x[ 2], S13, 0x242070db); /* 3 */
			FF (ref b, c, d, a, x[ 3], S14, 0xc1bdceee); /* 4 */
			FF (ref a, b, c, d, x[ 4], S11, 0xf57c0faf); /* 5 */
			FF (ref d, a, b, c, x[ 5], S12, 0x4787c62a); /* 6 */
			FF (ref c, d, a, b, x[ 6], S13, 0xa8304613); /* 7 */
			FF (ref b, c, d, a, x[ 7], S14, 0xfd469501); /* 8 */
			FF (ref a, b, c, d, x[ 8], S11, 0x698098d8); /* 9 */
			FF (ref d, a, b, c, x[ 9], S12, 0x8b44f7af); /* 10 */
			FF (ref c, d, a, b, x[10], S13, 0xffff5bb1); /* 11 */
			FF (ref b, c, d, a, x[11], S14, 0x895cd7be); /* 12 */
			FF (ref a, b, c, d, x[12], S11, 0x6b901122); /* 13 */
			FF (ref d, a, b, c, x[13], S12, 0xfd987193); /* 14 */
			FF (ref c, d, a, b, x[14], S13, 0xa679438e); /* 15 */
			FF (ref b, c, d, a, x[15], S14, 0x49b40821); /* 16 */

			/* Round 2 */
			GG (ref a, b, c, d, x[ 1], S21, 0xf61e2562); /* 17 */
			GG (ref d, a, b, c, x[ 6], S22, 0xc040b340); /* 18 */
			GG (ref c, d, a, b, x[11], S23, 0x265e5a51); /* 19 */
			GG (ref b, c, d, a, x[ 0], S24, 0xe9b6c7aa); /* 20 */
			GG (ref a, b, c, d, x[ 5], S21, 0xd62f105d); /* 21 */
			GG (ref d, a, b, c, x[10], S22,  0x2441453); /* 22 */
			GG (ref c, d, a, b, x[15], S23, 0xd8a1e681); /* 23 */
			GG (ref b, c, d, a, x[ 4], S24, 0xe7d3fbc8); /* 24 */
			GG (ref a, b, c, d, x[ 9], S21, 0x21e1cde6); /* 25 */
			GG (ref d, a, b, c, x[14], S22, 0xc33707d6); /* 26 */
			GG (ref c, d, a, b, x[ 3], S23, 0xf4d50d87); /* 27 */
			GG (ref b, c, d, a, x[ 8], S24, 0x455a14ed); /* 28 */
			GG (ref a, b, c, d, x[13], S21, 0xa9e3e905); /* 29 */
			GG (ref d, a, b, c, x[ 2], S22, 0xfcefa3f8); /* 30 */
			GG (ref c, d, a, b, x[ 7], S23, 0x676f02d9); /* 31 */
			GG (ref b, c, d, a, x[12], S24, 0x8d2a4c8a); /* 32 */

			/* Round 3 */
			HH (ref a, b, c, d, x[ 5], S31, 0xfffa3942); /* 33 */
			HH (ref d, a, b, c, x[ 8], S32, 0x8771f681); /* 34 */
			HH (ref c, d, a, b, x[11], S33, 0x6d9d6122); /* 35 */
			HH (ref b, c, d, a, x[14], S34, 0xfde5380c); /* 36 */
			HH (ref a, b, c, d, x[ 1], S31, 0xa4beea44); /* 37 */
			HH (ref d, a, b, c, x[ 4], S32, 0x4bdecfa9); /* 38 */
			HH (ref c, d, a, b, x[ 7], S33, 0xf6bb4b60); /* 39 */
			HH (ref b, c, d, a, x[10], S34, 0xbebfbc70); /* 40 */
			HH (ref a, b, c, d, x[13], S31, 0x289b7ec6); /* 41 */
			HH (ref d, a, b, c, x[ 0], S32, 0xeaa127fa); /* 42 */
			HH (ref c, d, a, b, x[ 3], S33, 0xd4ef3085); /* 43 */
			HH (ref b, c, d, a, x[ 6], S34,  0x4881d05); /* 44 */
			HH (ref a, b, c, d, x[ 9], S31, 0xd9d4d039); /* 45 */
			HH (ref d, a, b, c, x[12], S32, 0xe6db99e5); /* 46 */
			HH (ref c, d, a, b, x[15], S33, 0x1fa27cf8); /* 47 */
			HH (ref b, c, d, a, x[ 2], S34, 0xc4ac5665); /* 48 */

			/* Round 4 */
			II (ref a, b, c, d, x[ 0], S41, 0xf4292244); /* 49 */
			II (ref d, a, b, c, x[ 7], S42, 0x432aff97); /* 50 */
			II (ref c, d, a, b, x[14], S43, 0xab9423a7); /* 51 */
			II (ref b, c, d, a, x[ 5], S44, 0xfc93a039); /* 52 */
			II (ref a, b, c, d, x[12], S41, 0x655b59c3); /* 53 */
			II (ref d, a, b, c, x[ 3], S42, 0x8f0ccc92); /* 54 */
			II (ref c, d, a, b, x[10], S43, 0xffeff47d); /* 55 */
			II (ref b, c, d, a, x[ 1], S44, 0x85845dd1); /* 56 */
			II (ref a, b, c, d, x[ 8], S41, 0x6fa87e4f); /* 57 */
			II (ref d, a, b, c, x[15], S42, 0xfe2ce6e0); /* 58 */
			II (ref c, d, a, b, x[ 6], S43, 0xa3014314); /* 59 */
			II (ref b, c, d, a, x[13], S44, 0x4e0811a1); /* 60 */
			II (ref a, b, c, d, x[ 4], S41, 0xf7537e82); /* 61 */
			II (ref d, a, b, c, x[11], S42, 0xbd3af235); /* 62 */
			II (ref c, d, a, b, x[ 2], S43, 0x2ad7d2bb); /* 63 */
			II (ref b, c, d, a, x[ 9], S44, 0xeb86d391); /* 64 */

			state[0] += a;
			state[1] += b;
			state[2] += c;
			state[3] += d;

			// Zeroize sensitive information.
			//memset ( (Byte *) x, 0, sizeof(x));

		}

		// Encodes input (UInt32) into output (unsigned char). Assumes len is
		// a multiple of 4.
		private void encode (ref Byte[] output, UInt32[] input, UInt32 len)
		{
			UInt32 i, j;

			for (i = 0, j = 0; j < len; i++, j += 4) 
			{
				output[j]   = (Byte)  (input[i] & 0xff);
				output[j+1] = (Byte) ((input[i] >> 8) & 0xff);
				output[j+2] = (Byte) ((input[i] >> 16) & 0xff);
				output[j+3] = (Byte) ((input[i] >> 24) & 0xff);
			}
		}

		// Decodes input (unsigned char) into output (UInt32). Assumes len is
		// a multiple of 4.
		private void decode (ref UInt32[] output, Byte[] input, UInt32 len)
		{
			UInt32 i, j;

			for (i = 0, j = 0; j < len; i++, j += 4)
				output[i] = ((UInt32)input[j]) | (((UInt32)input[j+1]) << 8) |
					(((UInt32)input[j+2]) << 16) | (((UInt32)input[j+3]) << 24);
		}

		// ROTATE_LEFT rotates x left n bits.
		private UInt32 rotate_left (UInt32 x, UInt32 n)
		{
			return (x << (int)n) | (x >> (int)(32-n));
		}

		// F, G, H and I are basic MD5 functions.
		private UInt32 F(UInt32 x, UInt32 y, UInt32 z)
		{
			return (x & y) | (~x & z);
		}
		private UInt32 G(UInt32 x, UInt32 y, UInt32 z)
		{
			return (x & z) | (y & ~z);
		}
		private UInt32 H(UInt32 x, UInt32 y, UInt32 z)
		{
			return x ^ y ^ z;
		}
		private UInt32 I(UInt32 x, UInt32 y, UInt32 z)
		{
			return y ^ (x | ~z);
		}

		// FF, GG, HH, and II transformations for rounds 1, 2, 3, and 4.
		// Rotation is separate from addition to prevent recomputation.

		void FF(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 x, UInt32  s, UInt32 ac)
		{
			a += F(b, c, d) + x + ac;
			a = rotate_left (a, s) +b;
		}

		void GG(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 x, UInt32 s, UInt32 ac)
		{
			a += G(b, c, d) + x + ac;
			a = rotate_left (a, s) +b;
		}

		void HH(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 x, UInt32 s, UInt32 ac)
		{
			a += H(b, c, d) + x + ac;
			a = rotate_left (a, s) +b;
		}

		void II(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 x, UInt32 s, UInt32 ac)
		{
			a += I(b, c, d) + x + ac;
			a = rotate_left (a, s) +b;
		}
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

}