using System;
using msn2.net.QueuePlayer.Shared;

namespace msn2.net.QueuePlayer.Shared
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public abstract class IServerPlayer
	{
		#region Control Operations

		public abstract event Server_PlayingEventHandler PlayingEvent;

		public abstract void Play();

		public abstract event Server_PausedEventHandler PausedEvent;

		public abstract void Pause();

		public abstract event Server_StoppedEventHandler StoppedEvent;

		public abstract void Stop();

		#endregion

		# region Playing operations

		public abstract event Server_EndOfStreamEventHandler EndOfStreamEvent;
		public abstract event Server_ProgressEventHandler ProgressEvent;

		# endregion
	
		#region Properties

		public abstract string MediaFile
		{
			set;
		}

		public abstract bool IsValid
		{
			get;
		}

		public abstract double Duration
		{
			get;
		}

		public abstract event Server_VolumeChangedEventHandler VolumeChanged;

		public abstract double Volume
		{
			get;
			set;
		}

		public abstract event Server_BalanceChangedEventHandler BalanceChanged;
	
		public abstract int Balance
		{
			get;
			set;
		}

		public abstract event Server_RateChangedEventHandler RateChanged;
	
		public abstract double Rate
		{
			get;
			set;
		}

		public abstract void MoveTimeByAmount(double changeAmount);

		public abstract double CurrentPosition
		{
			get;
			set;
		}

		#endregion

	}

	#region Delegate Declarations

	public delegate void Server_PlayingEventHandler(object sender, EventArgs e);
	public delegate void Server_PausedEventHandler(object sender, EventArgs e);
	public delegate void Server_StoppedEventHandler(object sender, EventArgs e);
	public delegate void Server_ProgressEventHandler(object sender, Server_ProgressEventArgs e);
	public delegate void Server_EndOfStreamEventHandler(object sender, EventArgs e);
	public delegate void Server_VolumeChangedEventHandler(object sender, Server_VolumeEventArgs e);
	public delegate void Server_BalanceChangedEventHandler(object sender, Server_BalanceEventArgs e);
	public delegate void Server_RateChangedEventHandler(object sender, Server_RateEventArgs e);

	#endregion

	#region EventArgs classes

	public class Server_ProgressEventArgs: EventArgs 
	{
		private double position;

		public Server_ProgressEventArgs(double position) 
		{
			this.position = position;
		}

		public double Position 
		{
			get { return position; }
		}
	}

	public class Server_VolumeEventArgs: EventArgs 
	{
		private double volume;

		public Server_VolumeEventArgs(double volume) 
		{
			this.volume = volume;
		}

		public double Volume 
		{
			get { return volume; }
		}
	}

	public class Server_BalanceEventArgs: EventArgs 
	{
		private int balance;

		public Server_BalanceEventArgs(int balance) 
		{
			this.balance = balance;
		}

		public int Balance 
		{
			get { return balance; }
		}
	}

	public class Server_RateEventArgs: EventArgs 
	{
		private double rate;

		public Server_RateEventArgs(double rate) 
		{
			this.rate = rate;
		}

		public double Rate
		{
			get { return rate; }
		}
	}

	#endregion


}
