namespace XMedia
{
    using System;
    using System.Collections;
    using System.ServiceProcess;
	using System.Threading;
	using System.Net;
	using System.Net.Sockets;
	using System.Timers;
	using System.Diagnostics;

	//beta2: using Microsoft.Win32.Interop;

	#if NOSERVICE
    public class XMServer //: System.ServiceProcess.ServiceBase
	#else
	public class XMServer : System.ServiceProcess.ServiceBase
	#endif
    {
		protected XMListener mListener;
		protected System.Timers.Timer mTimerConnections;
		protected System.Timers.Timer mTimerMediaRebuild;

		//our public query engine
		protected static XMQueryEngine mEngine = new XMQueryEngine();
		public static XMQueryEngine Engine
		{
			get
			{
				return mEngine;
			}
		}

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
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[] { new XMServer() };
				ServiceBase.Run(ServicesToRun);
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
			//spawn the server starter
			Thread me = new Thread(new ThreadStart(this.Alpha));
			me.Start();
        }
        
		public void Alpha()
		{
			//1. Load parameters
			//2. Load query processors
			//3. Begin listening

			//static inits
			XMConnection.StaticInit();
			XMLog.StaticInit();

			//print init stuff
			XMLog.WriteLine(String.Format(
				"AMS Server Starting:\n\tDatabase: {0} on {1}\n\tQuery Processors: {2}\n\tPort: {3}",
				XMConfig.DBDatabase,
				XMConfig.DBServer,
				XMConfig.QueryProcessorCount,
				XMConfig.NetServerPort),
				"XMServer",
				EventLogEntryType.Information);

			//load data, start a processor
			mEngine.Rebuild();
			mEngine.SetProcessorCount(XMConfig.QueryProcessorCount);

			//start listen server
			if (mListener!=null)
			{
				XMLog.WriteLine("Listener already started.", "XMServer", System.Diagnostics.EventLogEntryType.Error);
				return;
			}
			mListener = new XMListener();
			mListener.Start();

			//start timer for connection checking
			mTimerConnections = new System.Timers.Timer(XMConfig.NetConnectionCheckInterval.TotalMilliseconds);	//2 minutes	
			mTimerConnections.Elapsed += new System.Timers.ElapsedEventHandler(ElapsedConnections);
			mTimerConnections.Start();

			//start timer for media rebuilds
			mTimerMediaRebuild = new System.Timers.Timer(XMConfig.QueryMediaRebuildInterval.TotalMilliseconds); //42 minutes (yes, 42!)
			mTimerMediaRebuild.Elapsed += new System.Timers.ElapsedEventHandler(ElapsedMediaRebuild);
			mTimerMediaRebuild.Start();
		}
 
		public void ElapsedConnections(object o, System.Timers.ElapsedEventArgs args)
		{
			//transfer to xmauth
			XMAuth.CheckConnections();
		}

		public void ElapsedMediaRebuild(object o, System.Timers.ElapsedEventArgs args)
		{
			//start rebuild
			mTimerMediaRebuild.Stop();
			mEngine.Rebuild();
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
					XMLog.WriteLine("Could not stop listener: " + e.Message, "XMServer", System.Diagnostics.EventLogEntryType.Error);
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

			//beta2: socket calls return void, throw exceptions
			Socket mSocket;
			try
			{
				mSocket = new Socket(	/*beta2:*/AddressFamily.InterNetwork, 
										/*beta2:*/SocketType.Stream, 
										/*beta2:*/ProtocolType.Tcp);
				mSocket.Bind(new IPEndPoint(IPAddress.Any, XMConfig.NetServerPort));
				mSocket.Listen(4);
			}
			catch(SocketException se)
			{
				//error occured
				XMLog.WriteLine(se.Message, "Listener", EventLogEntryType.Error);
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