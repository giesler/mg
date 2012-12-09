namespace Cards
{
	using System;
	using System.Collections;
	using System.Drawing;
	using System.WinForms;

	public class Screen : Form
	{
		protected Simulation mSim;
		public Screen(Simulation newSim)
		{
			//hook up events to simulation
			mSim = newSim;
		}
	}
}