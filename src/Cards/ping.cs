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
				if (l.Poll(1000*1, SelectMode.SelectRead))
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
		
	}
}