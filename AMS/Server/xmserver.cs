namespace XMedia
{
    using System;
    using System.Collections;
    using System.ServiceProcess;
	using System.Threading;
	using System.Net;
	using System.Net.Sockets;

	using Microsoft.Win32.Interop;

	#if NOSERVICE
	//emulate event log
	public class EventLog
	{
		public void WriteEntry(string message, System.Diagnostics.EventLogEntryType type)
		{
			//write to console
			Console.WriteLine("Event: " + message);
		}
	}
	#endif

	#if NOSERVICE
    public class XMServer //: System.ServiceProcess.ServiceBase
	#else
	public class XMServer : System.ServiceProcess.ServiceBase
	#endif
    {
		protected XMListener mListener;

		//our public query engine
		protected static XMQueryEngine mEngine = new XMQueryEngine();
		public static XMQueryEngine Engine
		{
			get
			{
				return mEngine;
			}
		}

		#if NOSERVICE
		protected EventLog EventLog = new EventLog();
		#endif

        public XMServer()
        {
 			#if SERVICE
				this.ServiceName = "XMedia Server";
			#endif
        }

        // The main entry point for the process
        static void Main()
        {
			//initialize guid support
			XMGuid.Init();

			#if SERVICE
				System.ServiceProcess.ServiceBase[] ServicesToRun;
				ServicesToRun = new System.ServiceProcess.ServiceBase[] { new XMServer() };
				System.ServiceProcess.ServiceBase.Run(ServicesToRun);
			#else
				//create the object and run it
				XMServer server = new XMServer();
				server.OnStart(null);
			#endif
        }

        /// <summary>
        ///    Set things in motion so your service can do its work.
        /// </summary>
        #if NOSERVICE
        protected /*override*/ void OnStart(string[] args)
		#else
        protected override void OnStart(string[] args)
		#endif
        {
			//1. Load parameters
			//2. Load query processors
			//3. Begin listening

			//load data, start a processor
			mEngine.Rebuild();
			mEngine.SetProcessorCount(1);

			//start listen server
			if (mListener!=null)
			{
				EventLog.WriteEntry("Listener already started.", System.Diagnostics.EventLogEntryType.Error);
				return;
			}
			mListener = new XMListener();
			mListener.Start();

        }
 
        /// <summary>
        ///    Stop this service.
        /// </summary>
        #if NOSERVICE
        protected /*override*/ void OnStop()
		#else
		protected override void OnStop()
		#endif
        {
			//1. Set stop flag
			//2. Wait for current queries to end
			//3. Release query processors
			//4. Stop listening

			//stop the server
			if(mListener!=null)
			{
				try
				{
					mListener.Stop();
				}
				catch(Exception e)
				{
					//log exception
					EventLog.WriteEntry("Could not stop listener: " + e.Message, System.Diagnostics.EventLogEntryType.Error);
				}
				
				mListener = null;
			}

        }

    }

	public class XMListener
	{
		protected Thread mThread;
		protected int mContinue;

		public XMListener()
		{
		}

		public void Start()
		{
			//create the thread object
			if (mThread==null)
			{
				mThread = new Thread(new ThreadStart(this.Alpha));
				mThread.Priority = ThreadPriority.BelowNormal;
			}

			//is the thread already running??
			if (mThread.IsAlive)
			{
				throw(new Exception("Listener already started."));
			}

			//start the thread
			mContinue = -1;
			mThread.Start();

		}

		public void Stop()
		{
			//attempt to stop the thread
			if (mThread==null)
			{
				throw(new Exception("Listener not running."));
			}
			if (!mThread.IsAlive)
			{
				throw(new Exception("Listener not running."));
			}

			//set the stop flag
			Interlocked.Exchange(ref mContinue, 0);

			//wait for the thread to exit, 1 second
			if (!mThread.Join(1000))
			{
				//thread did not exit on its own,
				//kill it
				mThread.Abort();
				
				//try waiting another sec
				if (!mThread.Join(1000))
				{
					throw(new Exception("Failed to exit thread."));
				}
			}

			//close any open connections
			XMConnection.CloseConnections();
		}

		public void Alpha()
		{
			//move everyone offline
			XMAuth.KickAll();

			//begin listening
			Socket mSocket = new Socket(AddressFamily.AfINet, 
										SocketType.SockStream, 
										ProtocolType.ProtTCP);
			if (mSocket.Bind(new IPEndPoint(IPAddress.InaddrAny, 25346))!=0)
			{
				//error occureed
				(new EventLog()).WriteEntry("Failed to bind server to port:\n"
					+ Convert.ToString(Windows.GetLastError()), 0);
				return;
			}
			if (mSocket.Listen(4)!=0)
			{
				//error occured
				(new EventLog()).WriteEntry("Failed to set server to listen mode:\n"
					+ Convert.ToString(Windows.GetLastError()), 0);
				return;
			}

			//enter loop
			Socket newSocket;
			XMConnection newClient;
			while(mContinue!=0)
			{
				//are there connections waiting?
				while(mSocket.Poll(0, SelectMode.SelectRead))
				{
					//connect a new socket
					System.Diagnostics.Debug.WriteLine("Accepting new connection.", "XMSERVER");
					newSocket = mSocket.Accept();
					newClient = new XMConnection(newSocket);
				}
					
				//wait a few millisecnods for another connection
				//NOTE: can't wait forever, since we need to check
				//mContinue frequently
				mSocket.Poll(1000*10, SelectMode.SelectRead);	//10 milliseconds
				Thread.Sleep(10);
			}

			//close listener
			mSocket.Close();
		}
	}
}