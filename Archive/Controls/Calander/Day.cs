using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using msn2.net.Configuration;

namespace msn2.net.Controls.Calander
{
	/// <summary>
	/// Summary description for Day.
	/// </summary>
	public class Day : msn2.net.Controls.Shell.UserControl
	{
		#region Declares

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panelSlots;
		private System.Windows.Forms.Panel panelTime;
		private System.Windows.Forms.Splitter splitter1;
		private TimeSpan timeSpan;
		
		#endregion

		#region Constructor

		public Day()
		{
			InitializeComponent();
		}

		public Day(Data data, TimeSpan timeSpan): base(data)
		{
			InitializeComponent();

			LoadTimeSpan(timeSpan);
		}

		#endregion

		#region Disposal

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelSlots = new System.Windows.Forms.Panel();
			this.panelTime = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.SuspendLayout();
			// 
			// panelSlots
			// 
			this.panelSlots.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelSlots.Location = new System.Drawing.Point(59, 0);
			this.panelSlots.Name = "panelSlots";
			this.panelSlots.Size = new System.Drawing.Size(293, 104);
			this.panelSlots.TabIndex = 0;
			// 
			// panelTime
			// 
			this.panelTime.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelTime.Name = "panelTime";
			this.panelTime.Size = new System.Drawing.Size(56, 104);
			this.panelTime.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(56, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 104);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// Day
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelSlots,
																		  this.splitter1,
																		  this.panelTime});
			this.Name = "Day";
			this.Size = new System.Drawing.Size(352, 104);
			this.ResumeLayout(false);

		}
		#endregion

		#region Methods
	
		public event TimeSpanChangedDelegate TimeSpanChangedEvent;

		public void LoadTimeSpan(TimeSpan timeSpan)
		{
			TimeSpan oldTimeSpan	= this.timeSpan;
			this.timeSpan			= timeSpan;
            
			panelSlots.Controls.Clear();


			// Notify subscribers we changed the time
			if (TimeSpanChangedEvent != null)
			{
				TimeSpanChangedEvent(this, new TimeSpanChangedEventArgs(oldTimeSpan, this.timeSpan));
			}

		}

		#endregion

		#region Properties
	
		public TimeSpan TimeSpan
		{
			get
			{
				return timeSpan;
			}
			set
			{
				timeSpan = value;
			}
		}

		#endregion
	}

	public delegate void TimeSpanChangedDelegate(object sender, TimeSpanChangedEventArgs e);

	public class TimeSpanChangedEventArgs: EventArgs
	{
		public TimeSpan oldTimeSpan;
		public TimeSpan newTimeSpan;

		public TimeSpanChangedEventArgs(TimeSpan oldTimeSpan, TimeSpan newTimeSpan)
		{
			this.oldTimeSpan	= oldTimeSpan;
			this.newTimeSpan	= newTimeSpan;
		}

		public TimeSpan OldTimeSpan
		{
			get
			{
				return oldTimeSpan;
			}
			set
			{
				oldTimeSpan = value;
			}
		}

		public TimeSpan NewTimeSpan
		{
			get
			{
				return newTimeSpan;
			}
			set
			{
				newTimeSpan = value;
			}
		}
	}
}
