namespace Cards
{
	using System;
	using System.Net;
	using System.Net.Sockets;
	using System.Threading;
    using System.Collections;

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
		}

		public void Start()
		{
			mThread.Start();
		}

		public void Stop()
		{
			Monitor.Enter(this);
			mStop = false;
			Monitor.Exit(this);
		}

		public void Alpha()
		{
			//start listener
			Socket l;
			l = new Socket(AddressFamily.AfINet,
							SocketType.SockDgram,
							ProtocolType.ProtUDP);
			l.Bind(new IPEndPoint(IPAddress.InaddrAny, Port));

			//listen loop
			bool s;
			IPEndPoint ip = new IPEndPoint(0, 0);
			EndPoint e = (EndPoint)ip;
			byte[] buf = new byte[2048];
			int retval;
			do
			{
				if (l.Poll(1000*10, SelectMode.SelectRead))
				{
					//something to read
					retval = l.ReceiveFrom(buf, buf.Length, 0, ref e);
					
				}
				Monitor.Enter(this);
				s = mStop;
				Monitor.Exit(this);
			}
			while(s);
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
			mStop = false;s
		}

		public void Start()
		{
			//start thread
			mThread.Start();
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
			IPEndPoint ip = new IPEndPoint(0, 0);
			EndPoint e = (EndPoint)ip;
			do
			{
				//check for any packets to read
				if (sock.Poll(1000*10, SelectMode.SelectRead))
				{
					//read datagram
					sock.ReceiveFrom(buf, buf.Length, 0, ref e);

					//read parameters
					//string length (4), string, private (1),
					//current players (4), max players (4)
				}

				Monitor.Enter(this);
				s = mStop;
				Monitor.Exit(this);

			} while (s); 
		}
	}
}