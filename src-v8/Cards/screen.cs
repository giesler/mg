namespace Cards
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing;
	using System.WinForms;

	public class Screen : Form
	{
		private void InitializeComponent ()
		{
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	
		protected ObjectList mScreens = new ObjectList();
		protected Simulation mSim;
		public Screen(Simulation newSim)
		{
			//store ref to ourself
			mScreens.Add(this);

			//hook up to simulation
			mSim = newSim;

			//hook up our own events
			Closing += new CancelEventHandler(this.FormOnClosing);
			Closed += new EventHandler(this.FormOnClosed);
			Paint += new PaintEventHandler(this.FormOnPaint);
		}

		//close events
		public void FormOnClosing(object Sender, CancelEventArgs args)
		{
			//ask if they want to close
			if (MessageBox.Show("Are you sure you want to close?", "Confirm", 
				MessageBox.YesNo | MessageBox.IconQuestion)==DialogResult.No)
			{
				args.Cancel = true;
			}
			else
			{
				args.Cancel = false;
			}
		}
		public void FormOnClosed(object Sender, EventArgs args)
		{
			//remove from collection, end app if we are last
			mScreens.Remove(this);
			mSim.Stop();
			if (mScreens.Count<1)
			{
				Application.Exit();
			}
		}

		//main paint event
		public void FormOnPaint(object sender, PaintEventArgs args)
		{
			args.Graphics.FillRectangle(Brushes.DarkGreen, args.ClipRectangle);
		}
	}
}