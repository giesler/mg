namespace Cards
{
	using System;
	using System.Collections;

	public class Simulation
	{
		//creation
		protected static ObjectList mSims = new ObjectList();
		public Simulation()
		{
			//add us to global list of simulations
			mSims.Add(this);
		}
		public void Dispose()
		{
			mPing.Stop();
		}
		
		//parameters
		string mName = "Card Game";
		bool mPrivate;
		string mPassword;
		int mMaxPlayers;
		public bool Private
		{
			get
			{
				return mPrivate;
			}
		}
		public string Password
		{
			get
			{
				return mPassword;
			}
		}
		public int MaxPlayers
		{
			get
			{
				return mMaxPlayers;
			}
		}
		public string Name
		{
			get
			{
				return mName;
			}
		}

		//misc
		protected ObjectList mPlayers = new ObjectList();
		protected PingServer mPing;
		public int CurrentPlayers
		{
			get
			{
				return mPlayers.Count;
			}
		}

		//control
		public void Start(bool newPrivate, string newPassword, int newMax)
		{
			//store params
			mPrivate = newPrivate;
			mPassword = newPassword;
			mMaxPlayers = newMax;

			//start ping server
			mPing = new PingServer(this);
			mPing.Start();
		}
		public void Stop()
		{
			//stop the simulation
			mPing.Stop();

			//remove from list
			mSims.Remove(this);
		}
	}
}