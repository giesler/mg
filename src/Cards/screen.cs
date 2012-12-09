namespace Cards
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Drawing;
	using System.WinForms;

	public class Screen : Form
	{
		protected ObjectList mScreens = new ObjectList();
		protected Simulation mSim;
		public Screen(Simulation newSim)
		{
			//store ref to ourself
			mScreens.Add(this);

			//hook up to simulation
			mSim = newSim;

			//hook up our own events
			
		}

		//close events
		public void OnClosing(object Sender, CancelEventArgs args)
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
		public void OnClose(object Sender)
		{
			//remove from collection, end app if we are last
			mScreens.Remove(this);
			if (mScreens.Count<1)
			{
				Application.Exit();
			}
		}
	}
}