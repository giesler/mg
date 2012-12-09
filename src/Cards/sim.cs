namespace Cards
{
	using System;
	using System.Collections;

	public class Simulation
	{
		//creation
		protected static Simulation mTheSim;
		public Simulation()
		{
			//should only have one copy of this
			if (mTheSim!=null)
			{
				throw new Exception("Only one simulation object may exist.");
			}
			mTheSim = this;
		}
		public static Simulation GetSim()
		{
			return mTheSim;
		}

		//parameters
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

		//misc
		PingServer mPing;

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
	}
}