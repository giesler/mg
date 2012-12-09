namespace XMedia
{
    using System;
    using System.Collections;
    using System.ServiceProcess;
	using System.Threading;
	using System.Net;
	using System.Net.Sockets;
	using System.Xml;
	using System.IO;
	using System.Diagnostics;
	using System.Text;
	using System.Data.SqlClient;

    /// <summary>
    ///    Summary description for xmconnection.
    /// </summary>
    public class XMConnection
    {
		protected Thread mThread;
		protected int mContinue;
		protected int mLastSeq;
		protected Queue mOutbound = new Queue();

		/// <summary>
		/// When false, the Alpha loop will send keep-alive (ping) requests
		/// after 15 minutes of inactivity, and set the flag.  When a 
		/// response is received, ProccessMessage will reset this flag.
		/// </summary>
		protected bool KeepAliveInTransit = false;
		public void ResetKeepAlive()
		{
			KeepAliveInTransit = false;
		}

		//track data about client
		protected Socket mClient;
		public string Username = "<unknown>";
		public DateTime LastActivity;
		public XMGuid SessionID;
		public XMGuid UserID;
		public int Datarate;
		public bool Paying;
		public string Version;
		public int FileCount = 0;

		//some types of messages can only be called
		//one at time, track those here
		public bool InQuery;

		protected static /*beta2:*/ArrayList mConnections;

		//send the MOTD message over this connection
		public void DoMOTD()
		{
			//TODO: load motd from file or something
			/*
			XMMessage msg = new XMMessage();
			msg.Connection = this;
			msg.Action = "motd";
			msg.SetField("type", "none");
			msg.SetField("message", "Welcome to Adult Media Swapper!");
			msg.SetField("question", "");
			msg.SetField("choices", "");
			msg.Send();
			*/
		}

		/// <summary>
		/// Return a formated string describing the user's list
		/// of owned collections.
		/// </summary>
		public string Collections
		{
			get
			{
				XMAdo ado = XMAdo.FromPool();
				SqlDataReader rs;
				StringBuilder sb;
				try
				{
					//query database
					rs = ado.SqlExec("select * from collections where userid=" + UserID.ToStringDB());
					if (!rs.Read())
					{
						//nothing
						rs.Close();
						ado.ReturnToPool();
						return "";
					}

					//build string
					sb = new StringBuilder(512);
					do
					{
						sb.AppendFormat("{0},{1};", rs["collectionid"], rs["name"]);
					} while (rs.Read());
					ado.ReturnToPool();
					return sb.ToString();
				}
				catch(Exception e)
				{
					ado.ReturnToPool();
					throw e;
				}
			}
		}

		/// <summary>
		/// Send a XMMSG_PING over the current connection.
		/// </summary>
		public void InternalPing()
		{
			XMMessage ping = new XMMessage();
			ping.Connection = this;
			ping.Action = "ping";
			ping.SetField("reason", "keep-alive");
			ping.Send();
		}

		/// <summary>
		/// Opens a *new* connection, sends the ping, waits for a response.
		/// NOTE: This function times out after 750*2 milliseconds, returning false.
		/// </summary>
		/// <returns>Returns false if the connection could not be made, or if
		/// no data was received on the connection after sending the ping.</returns>
		public bool ExternalPing()
		{
			//setup the outbound socket
			Socket me = new Socket(	AddressFamily.InterNetwork, 
									SocketType.Stream, 
									ProtocolType.Tcp);
			IAsyncResult ar = me.BeginConnect(
				new IPEndPoint(HostIP, XMConfig.NetClientPort),
				null, null);
			if (!ar.AsyncWaitHandle.WaitOne(XMConfig.NetPingTimeout, false))
			{
				//took too long
				return false;
			}
			
			//create the message
			XMMessage msg = new XMMessage();
			msg.Connection = this;
			msg.Action = "ping";
			msg.SetField("reason", "login");
			string str = msg.ToString();
			
			//send it
			byte[] bufstr = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
			byte[] buf = new byte[bufstr.Length+1];
			bufstr.CopyTo(buf, 0);
			buf[buf.Length-1] = 0;
			me.Send(buf, buf.Length, 0);
			
			//receive something.. anything will do
			ar = me.BeginReceive(buf, 0, buf.Length, SocketFlags.None, null, null);
			if (!ar.AsyncWaitHandle.WaitOne(XMConfig.NetPingTimeout, true))
			{
				//took too long, give up
				me.Close();
				return false;
			}

			//success
			me.Close();
			return true;
		}

		/// <summary>
		/// Global initialization of static members.
		/// </summary>
		public static void StaticInit()
		{
			//create the connections array
			if (mConnections==null)
			{
				mConnections = new ArrayList();
			}
		}

		public static object[] Connections
		{
			get
			{
				object[] temp;
				lock (mConnections)
				{
					temp = mConnections.ToArray();
				}
				return temp;
			}
		}

		public static void CloseConnections()
		{
			//destroy all collections
			lock(mConnections)
			{
				foreach(XMConnection con in mConnections)
				{
					con.Close();
				}
			}
		}

        public XMConnection(Socket newSocket)
        {  
			//set our last activity date
			LastActivity = DateTime.Now;

			//store socket reference
			mClient = newSocket;

			//add ourselves to static list
			lock(mConnections)
			{
				mConnections.Add(this);
			}

			//start thread
			mContinue = -1;
			mThread = new Thread(new ThreadStart(this.Alpha));
			mThread.Priority = ThreadPriority.BelowNormal;
			mThread.Start();
        }

		public void Close()
		{
			//stop thread first
			Interlocked.Exchange(ref mContinue, 0);
			if (mThread.IsAlive)
			{
				if (!mThread.Join(1000))
				{
					//manually destroy the thread
					mThread.Abort();
					if (!mThread.Join(1000))
					{
						//thread never closed
						throw(new Exception("Failed to exit thread."));
					}
				}
			}

			//ensure that the connection was closed
			if (mClient!=null)
			{
				if (mClient.Connected)
				{
					mClient.Close();
				}
			}

			//remove ourself from collection
			lock(mConnections)
			{
				mConnections.Remove(this);
			}
		}

		public void Alpha()
		{
			//pump data out of the socket
			byte[] buf = new byte[5196];		// -stores accumulated data
			byte[] buf2 = null;					// -temp buffer used when
												//    expanding buf
			int size = 0;						// -pointer into buf
			int avail = 0;						// -amount of data to
												//    be read into buf
			int retval;							// -value returned from receive
			int nullpos;						// -pointer into buf for the
												//    null character
			string msg;							// -converted text of any 
												//    complete message received
			bool foundmsg;						// -tracks whether a msg was
												//    processed during loop
			while (mClient.Connected && mContinue!=0)
			{
				//send any outbound messages
				lock(mOutbound)
				{
					while (mOutbound.Count>0)
					{
						SendMessageInner((XMMessage)mOutbound.Dequeue());
					}	
				}

				//do we need to send any keep-alive requests?
				if (!KeepAliveInTransit &&
					LastActivity < (DateTime.Now - XMConfig.NetKeepAliveInterval))
				{
					KeepAliveInTransit = true;
					InternalPing();
				}

				//read data from the buffer
				if (mClient.Poll(500*1000, SelectMode.SelectRead))
				{
					//data available, read it until there is less
					//than the full buffer left
					avail = mClient.Available;
					if (avail>0)
					{
						//do we need to expand buffer?
						if (buf.Length < (size+avail))
						{
							//expand buffer to (needed)+5k
							buf2 = new byte[size+avail+5196];
							Array.Copy(buf, 0, buf2, 0, size);
							buf = buf2;
							buf2 = null;	//buf is now new array, old
							//array has been moved out of roots
						}
						
						//read data into buffer
						try
						{
							retval = mClient.Receive(buf, size, avail, 0);
						}
						catch (Exception e)
						{
							XMLog.WriteLine("Socket error while receiving: " + e.Message, "Connection");
							retval = 0;
						}

						//success?
						if (retval>0)
						{
							//loop through returned data, looking for
							//null characters

							//get first null
							foundmsg = false;
							nullpos = Array.IndexOf(buf, (byte)0 /*null*/, size, retval);
							while(nullpos!=-1)
							{
								//found a null, convert to text and process
								foundmsg = true;
								msg = System.Text.ASCIIEncoding.ASCII.GetString(buf, 0, nullpos);
								try 
								{
									ProcessMessage(msg);
								}
								catch(Exception e)
								{
									//nothing
									XMLog.WriteLine(e.ToString(), "ProcessMessage", EventLogEntryType.Error);
									//mClient.Close();
								}

								//create new buffer with just the excess
								//from the original, reset size to end of
								//valid data
								retval = ((size+retval)-(nullpos+1));
								buf2 = new byte[retval+5196];
								Array.Copy(buf, nullpos+1, buf2, 0, retval);
								buf = buf2;
								buf2 = null;
								size = 0;
																						
								//try to get next null
								nullpos = Array.IndexOf(buf, (byte)0 /*null*/, size, retval);

							}

							if (!foundmsg)
							{
								//no nulls found, just move the pointer
								//forward
								size += retval;	
								//Trace.WriteLine("No NULL found in " + retval + " bytes.");			
							}							
						}
					}
					else
					{
						//there was zero to read from buffer..
						//this means connection was closed
						Interlocked.Exchange(ref mContinue, 0);
					}
				}
			}
	
			//if we have a session id, remove that sessions entries
			//from the media bstorage table
			try
			{
				XMLog.WriteLine("Losing connection to " + Username, "Connection", EventLogEntryType.Information);
				if (!SessionID.Equals(new XMGuid()))
				{
					//release session
					XMAuth.KillSession(SessionID);
				}
			}
			catch
			{
				Trace.WriteLine("Error calling KillSession.");
			}

			//remove ourselves from the collection
			lock(mConnections)
			{
				mConnections.Remove(this);
			}

			return;
		}

		public void ProcessMessage(string msg)
		{
			//convert to xml
			XmlDocument xml = new XmlDocument();
			XMMessage xmsg;
			try
			{
				//proccess message
				xml.LoadXml(msg);
				xmsg = new XMMessage(xml, this);
				xmsg.Process();
			}
			catch(Exception e)
			{
				//error processing message.. record IP address
				string str = String.Format(
					"Error processing message from {0}({1}): {2}\n\tSource: {3}", 
					Username,
					HostIP.ToString(),
					e.Message,
					msg);
				XMLog.WriteLine(str, "ProcessMessage", EventLogEntryType.Warning);
			}

			//set last activity
			LastActivity = DateTime.Now;
		}

		public void SendMessage(XMMessage msg)
		{
			//do we need to send the message NOW?
			if (msg.Immediate)
			{
				SendMessageInner(msg);
			}
			else
			{
				//simply enqueue message
				lock(mOutbound)
				{
					mOutbound.Enqueue(msg);
				}
			}
		}

		protected void SendMessageInner(XMMessage msg)
		{
			//create buffer with null-char
			//float ts = System.Diagnostics.Counter.GetElapsed();
			string xml = msg.ToString();
			byte[] buf = new Byte[xml.Length+1];
			byte[] temp = System.Text.ASCIIEncoding.ASCII.GetBytes(xml);
			Array.Copy(temp, buf, temp.Length);
			buf[buf.Length-1] = 0;

			//push message over the wire
			try
			{
				mClient.Send(buf, buf.Length, 0);
			}
			catch (Exception e)
			{
				XMLog.WriteLine("SendMessageInner: " + e.Message, "Connection");
				return;
			}

			//send an auto-update file?
			if (msg.auEnable)
			{
				//read the file
				byte[] auBuf = new Byte[msg.auSize];
				/*beta2:
				File f = new File(msg.auPath);
				Stream s = f.OpenRead();
				s.Read(auBuf, 0, auBuf.Length);
				*/
				FileStream fs = File.OpenRead(msg.auPath);
				fs.Read(auBuf, 0, auBuf.Length);
				fs.Close();
				
				//send the buffer
				mClient.Send(auBuf, auBuf.Length, 0);
			}
		}

		public IPAddress LocalIP 
		{
			get
			{
				return ((IPEndPoint)mClient.LocalEndPoint).Address;
			}
		}

		public IPAddress HostIP
		{
			get
			{
				return ((IPEndPoint)mClient.RemoteEndPoint).Address;
			}
		}
    }
}
