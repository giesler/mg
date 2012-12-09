namespace Cards
{
	using System;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;
    using System.Collections;
	using System.IO;

	public class PingResponse
	{
		public PingResponse(BinaryReader reader)
		{
			//must be read in order
			Name = reader.ReadString();
			Private = reader.ReadBoolean();
			CurrentPlayers = reader.ReadInt32();
			MaxPlayers = reader.ReadInt32();
		}
		public PingResponse(Simulation sim)
		{
			Name = sim.Name;
			Private = sim.Private;
			CurrentPlayers = sim.CurrentPlayers;
			MaxPlayers = sim.MaxPlayers;
		}
		public void Write(BinaryWriter writer)
		{
			writer.WriteString(Name);
			writer.Write(Private);
			writer.Write(CurrentPlayers);
			writer.Write(MaxPlayers);
		}

		public string Name;
		public bool Private;
		public int CurrentPlayers;
		public int MaxPlayers;
	}

	public class PingServer
	{
		protected Thread mThread;
		protected bool mStop;
		protected Simulation mSim;

		public const int Port = 4000;

		public PingServer(Simulation newSim)
		{
			//create thread
			mThread = new Thread(new ThreadStart(this.Alpha));
			mSim = newSim;
			mStop = false;
		}

		public void Start()
		{
			mThread.Start();
		}

		public void Stop()
		{
			Monitor.Enter(this);
			mStop = true;
			Monitor.Exit(this);
		}

		public void Alpha()
		{
			//start listener
			Socket l;
			l = new Socket(AddressFamily.AfINet,
							SocketType.SockDgram,
							ProtocolType.ProtUDP);
			if (l.Bind(new IPEndPoint(IPAddress.InaddrAny, Port))!=0)
			{
				throw(new Exception("Bind failed."));
			}

			//listen loop
			bool s;
			IPEndPoint ip = new IPEndPoint(0, 0);
			EndPoint e = (EndPoint)ip;
			byte[] buf = new byte[2048];
			int retval;
			MemoryStream stream;
			BinaryWriter writer;
			PingResponse response;
			do
			{
				if (l.Poll(1000*10, SelectMode.SelectRead))
				{
					//something to read
					retval = l.ReceiveFrom(buf, buf.Length, 0, ref e);
					
					//build stream
					//string, private, current, max players
					response = new PingResponse(mSim);
					stream = new MemoryStream();
					writer = new BinaryWriter(stream);
					response.Write(writer);

					//send response
                    l.SendTo(stream.ToArray(), stream.Length.ToInt32(), 0, e);					
				}
				Monitor.Enter(this);
				s = mStop;
				Monitor.Exit(this);
			}
			while(!s);
		}
	}

	public class PingClient
	{
		public const int Port = 4001;
		protected bool mStop;
		protected Thread mThread;
		protected Queue mServers;

		public PingClient()
		{
			//create objects
			mThread = new Thread(new ThreadStart(this.Alpha));
			mServers = new Queue();
			mStop = false;
		}

		public void Start()
		{
			//start thread
			mThread.Start();

			//create basic ping packet
			byte[] buf = new Byte[32];

			//send the ping
			Socket s = new Socket(AddressFamily.AfINet,
									SocketType.SockDgram,
									ProtocolType.ProtUDP);
			s.Bind(new IPEndPoint(IPAddress.InaddrAny, Port));
			s.SendTo(buf, buf.Length, 0, new IPEndPoint(IPAddress.InaddrBroadcast, PingServer.Port));
		}

		public void Stop()
		{
			Monitor.Enter(this);
			mStop = true;
			Monitor.Exit(this);
		}

		public int Count
		{
			get
			{
				Monitor.Enter(this);
				int temp = mServers.Count;
				Monitor.Exit(this);
				return temp;
			}
		}

		public PingResponse Pop()
		{
			Monitor.Enter(this);
			PingResponse temp = (PingResponse)mServers.Dequeue();
			Monitor.Exit(this);
			return temp;
		}

		public void Alpha()
		{
			//create listening socket
			ObjectList connections = new ObjectList();
			Socket sock = new Socket(AddressFamily.AfINet, 
									SocketType.SockDgram,
									ProtocolType.ProtUDP);
			sock.Bind(new IPEndPoint(IPAddress.InaddrAny, Port));

			//wait for responses
			bool s;
			byte[] buf = new byte[2048];
			int retval;
			IPEndPoint ip = new IPEndPoint(0, 0);
			EndPoint e = (EndPoint)ip;
			MemoryStream stream;
			BinaryReader reader;
			PingResponse response;
			do
			{
				//check for any packets to read
				if (sock.Poll(1000*10, SelectMode.SelectRead))
				{
					//read datagram
					retval = sock.ReceiveFrom(buf, buf.Length, 0, ref e);

					//read parameters 
					stream = new MemoryStream(buf, 0, retval, false);
					reader = new BinaryReader(stream);
					response = new PingResponse(reader);

					//store response
					Monitor.Enter(this);
					mServers.Enqueue(response);
					Monitor.Exit(this);
				}

				Monitor.Enter(this);
				s = mStop;
				Monitor.Exit(this);

			} while (!s); 
		}
	}
}