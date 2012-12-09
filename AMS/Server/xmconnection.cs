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

    /// <summary>
    ///    Summary description for xmconnection.
    /// </summary>
    public class XMConnection
    {
		protected Thread mThread;
		protected int mContinue;
		protected int mLastSeq;
		protected Queue mOutbound = new Queue();

		//track data about client
		protected Socket mClient;
		public string Username;
		public DateTime LastActivity;
		public XMGuid SessionID;
		public XMGuid UserID;
		public int Datarate;
		public bool Paying;
		public string Version;

		public const int LimiterIndex = 3;
		public const int LimiterFilter = 3;

		//some types of messages can only be called
		//one at time, track those here
		public bool InQuery;

		protected static ObjectList mConnections;

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

		//open a NEW connection, send the ping, wait for a response
		public bool Ping()
		{
			//setup the outbound socket
			Socket me = new Socket(	AddressFamily.AfINet, 
									SocketType.SockStream, 
									ProtocolType.ProtTCP);
			if (0 !=
				me.Connect(new IPEndPoint(HostIP, 25347)))
			{
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
			IAsyncResult ar = me.BeginReceive(buf, 0, buf.Length, null, null);
			if (!ar.AsyncWaitHandle.WaitOne(750, true))
			{
				//took too long, give up
				return false;
			}

			//success
			return true;
		}

		public static XMConnection[] Connections
		{
			get
			{
				Monitor.Enter(mConnections);
				XMConnection[] temp = (XMConnection[])mConnections.ToArray();
				Monitor.Exit(mConnections);
				return temp;
			}
		}

		public static void CloseConnections()
		{
			//destroy all collections
			Monitor.Enter(mConnections);
			foreach(XMConnection con in mConnections)
			{
				con.Close();
			}
			Monitor.Exit(mConnections);
		}

        public XMConnection(Socket newSocket)
        {  
			//store socket reference
			mClient = newSocket;

			//does the static list exist yet?
			if (mConnections==null)
			{
				mConnections = new ObjectList();
			}

			//add ourselves to static list
			Monitor.Enter(mConnections);
			mConnections.Add(this);
			Monitor.Exit(mConnections);

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
			Monitor.Enter(mConnections);
			mConnections.Remove(this);
			Monitor.Exit(mConnections);
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
				Monitor.Enter(mOutbound);
				while (mOutbound.Count>0)
				{
					SendMessageInner((XMMessage)mOutbound.Dequeue());
				}	
				Monitor.Exit(mOutbound);

				if (mClient.Poll(10*1000, SelectMode.SelectRead))
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
						retval = mClient.Receive(buf, size, avail, 0);

						//success?
						if (retval>0)
						{
							//loop through returned data, looking for
							//null characters

							//get first null
							foundmsg = false;
							nullpos = Array.IndexOf(buf, (byte)0 /*null*/, size, size+retval);
							while(nullpos!=-1)
							{
								//found a null, convert to text and process
								foundmsg = true;
								msg = System.Text.ASCIIEncoding.ASCII.GetString(buf, 0, nullpos);
								//System.Diagnostics.Debug.WriteLine("Received message:\n\t" + msg, "XMSERVER");
								try 
								{
									ProcessMessage(msg);
								}
								catch(Exception e)
								{
									//nothing
									#if NOSERVICE
									Console.WriteLine(e.ToString());
									#endif
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
								nullpos = Array.IndexOf(buf, (byte)0 /*null*/, size, size+retval);
							}

							if (!foundmsg)
							{
								//no nulls found, just move the pointer
								//forward
								size += retval;								
							}							
						}
					}
					else
					{
						//there was zero to read from buffer..
						//this means connection was closed
						Interlocked.Exchange(ref mContinue, 0);
						System.Diagnostics.Debug.WriteLine("Losing connection.", "XMSERVER");
					}
				}

			}
	
			//if we have a session id, remove that sessions entries
			//from the media storage table
			try
			{
				if (!SessionID.Equals(new XMGuid()))
				{
					//release session
					XMAuth.KillSession(SessionID);
				}
			}
			catch
			{
			}

			return;
		}

		public void ProcessMessage(string msg)
		{
			//convert to xml
			XmlDocument xml = new XmlDocument();
			try
			{
				xml.LoadXml(msg);
			}
			catch(XmlException e)
			{
				//error in xml
				System.Diagnostics.Debug.WriteLine("Error converting message to XML.", "XMSERVER");
				throw(e);
			}

			//create a message
			XMMessage xmsg = new XMMessage(xml, this);

			//proccess message
			//System.Diagnostics.Debug.WriteLine("Processing message...", "XMSERVER");
			xmsg.Process();
			//System.Diagnostics.Debug.WriteLine("Done processing message.", "XMSERVER");

			//set last activity
			LastActivity = DateTime.Now;
		}

		public void SendMessage(XMMessage msg)
		{
			//simply enqueue message
			Monitor.Enter(mOutbound);
			mOutbound.Enqueue(msg);
			Monitor.Exit(mOutbound);
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
			mClient.Send(buf, buf.Length, 0);

			//send an auto-update file?
			if (msg.auEnable)
			{
				//read the file
				byte[] auBuf = null;
				File f = new File(msg.auPath);
				auBuf = new Byte[msg.auSize];
				Stream s = f.OpenRead();
				s.Read(auBuf, 0, auBuf.Length);
				
				//send the buffer
				mClient.Send(auBuf, auBuf.Length, 0);
			}
		}

		public IPAddress LocalIP 
		{
			get
			{
				return ((IPEndPoint)mClient.LocalEndpoint).Address;
			}
		}

		public IPAddress HostIP
		{
			get
			{
				return ((IPEndPoint)mClient.RemoteEndpoint).Address;
			}
		}
    }
}
