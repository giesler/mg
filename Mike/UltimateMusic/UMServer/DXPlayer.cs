using System;
using QuartzTypeLib;
using System.Timers;

namespace UMServer
{
	/// <summary>
	/// Summary description for DXPlayer.
	/// </summary>
	public class DXPlayer
	{
		private FilgraphManagerClass filegraphManager;
		private Timer timer;
		private bool initialized = false;

		// defaults
		private int	   mediaVolume  = 0;
		private int    mediaBalance = 0;
		private double mediaRate	= 1;

		public DXPlayer()
		{
			timer = new Timer(1000);
			timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
		}

		# region Control operations

		public event DXPlayingEventHandler PlayingEvent;

		public void Play() 
		{
			filegraphManager.Run();
			timer.Enabled = true;
			if (PlayingEvent != null) 
			{
				PlayingEvent(this, new EventArgs());
			}
		}

		public event DXPausedEventHandler PausedEvent;

		public void Pause() 
		{
			filegraphManager.Pause();
			timer.Enabled = false;
			if (PausedEvent != null) 
			{
				PausedEvent(this, new EventArgs());
			}
		}

		public event DXStoppedEventHandler StoppedEvent;

		public void Stop() 
		{
			if (initialized) 
			{
				filegraphManager.Stop();
				filegraphManager.CurrentPosition = 0;
			}
			timer.Enabled = false;
			if (StoppedEvent != null) 
			{
				StoppedEvent(this, new EventArgs());
			}
		}

		# endregion

		# region Playing operations

		public event DXEndOfStreamEventHandler EndOfStreamEvent;
		public event DXProgressEventHandler ProgressEvent;

		private void Timer_Elapsed(object sender, ElapsedEventArgs e) 
		{
			// Check if the media is done playing
			if (filegraphManager.CurrentPosition == filegraphManager.Duration) 
			{
				timer.Enabled = false;
				if (EndOfStreamEvent != null) 
				{
					EndOfStreamEvent(this, new EventArgs());
				}				
			}
				// otherwise, raise progress event
			else 
			{
				if (ProgressEvent != null) 
				{
					ProgressEvent(this, new DXProgressEventArgs(filegraphManager.CurrentPosition / filegraphManager.Duration ));
				}
			}
		}

		# endregion
	
		#region Properties

		public string MediaFile 
		{
			set 
			{
				if (filegraphManager != null) 
				{
                    filegraphManager.Stop();					
				}
				filegraphManager = new FilgraphManagerClass();
				filegraphManager.RenderFile(value);
				filegraphManager.Volume  = mediaVolume;
				filegraphManager.Rate	 = mediaRate;
				filegraphManager.Balance = mediaBalance;
				initialized = true;
			}
		}

		public double Duration 
		{
			get 
			{
				if (!initialized)
					return 0;
				else
					return filegraphManager.Duration;
			}
		}

		public event DXVolumeChangedEventHandler VolumeChanged;

		public double Volume 
		{
			get 
			{
				// min value = 0, max value = -10,000
				if (!initialized)
					return 1;
				else
					return ( ((double)filegraphManager.Volume + 2000.0) / 2000.0);
			}
			set 
			{
				mediaVolume = (int) (0 - ((1.0 - value) * 2000.0));
				if (initialized)
					filegraphManager.Volume = mediaVolume;
				if (VolumeChanged != null) 
					VolumeChanged(this, new DXVolumeEventArgs(value));
			}
		}

		public event DXBalanceChangedEventHandler BalanceChanged;
	
		public int Balance 
		{
			get 
			{
				if (!initialized)
					return 0;
				else
					return filegraphManager.Balance;
			}
			set 
			{
				mediaBalance = value;
				if (initialized)
					filegraphManager.Balance = value;
				if (BalanceChanged != null)
					BalanceChanged(this, new DXBalanceEventArgs(value));
			}
		}

		public event DXRateChangedEventHandler RateChanged;
	
		public double Rate 
		{
			get 
			{
				if (!initialized)
					return 1.0;
				else
					return filegraphManager.Rate;
			}
			set 
			{
				if (value > 0 && initialized) 
				{
					mediaRate = value;
					if (initialized)
						filegraphManager.Rate = value;
					if (RateChanged != null)
						RateChanged(this, new DXRateEventArgs(value));
				}
			}
		}

		public double CurrentPosition 
		{
			get 
			{
				return filegraphManager.CurrentPosition;
			}
			set 
			{
				filegraphManager.CurrentPosition = (value * filegraphManager.Duration) ;
			}
		}

		#endregion
	}

	#region Delegate declarations 

	public delegate void DXPlayingEventHandler(object sender, EventArgs e);
	public delegate void DXPausedEventHandler(object sender, EventArgs e);
	public delegate void DXStoppedEventHandler(object sender, EventArgs e);
	public delegate void DXProgressEventHandler(object sender, DXProgressEventArgs e);
	public delegate void DXEndOfStreamEventHandler(object sender, EventArgs e);
	public delegate void DXVolumeChangedEventHandler(object sender, DXVolumeEventArgs e);
	public delegate void DXBalanceChangedEventHandler(object sender, DXBalanceEventArgs e);
	public delegate void DXRateChangedEventHandler(object sender, DXRateEventArgs e);

	#endregion

	#region EventArgs classes

	public class DXProgressEventArgs: EventArgs 
	{
		private double progress;

		public DXProgressEventArgs(double progress) 
		{
			this.progress = progress;
		}

		public double Progress 
		{
			get { return progress; }
		}
	}

	public class DXVolumeEventArgs: EventArgs 
	{
		private double volume;

		public DXVolumeEventArgs(double volume) 
		{
			this.volume = volume;
		}

		public double Volume 
		{
			get { return volume; }
		}
	}

	public class DXBalanceEventArgs: EventArgs 
	{
		private int balance;

		public DXBalanceEventArgs(int balance) 
		{
			this.balance = balance;
		}

		public int Balance 
		{
			get { return balance; }
		}
	}

	public class DXRateEventArgs: EventArgs 
	{
		private double rate;

		public DXRateEventArgs(double rate) 
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
