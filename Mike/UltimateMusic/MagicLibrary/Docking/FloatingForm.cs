// ***************************************************************************** 
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the confidential and proprietary information 
//  of Crownwood Consulting Limited, Haxey, North Lincolnshire, England 
//  and are supplied subject to licence terms. In no event may the Licensee 
//  reverse engineer, decompile, or otherwise attempt to discover the 
//  underlying source code or confidential information herein. 
// 
// ***************************************************************************** 

using System;
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Docking;

namespace Crownwood.Magic.Docking
{
	public class FloatingForm : ExternalForm
	{
		public FloatingForm(DockingBase dockingBase) : base(dockingBase)
		{
			// A tool window has a thin caption bar, no min/max buttons,
			// no small icon or caption menu
			this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
		}

		public override string ToString()
		{
			return "FloatingForm OldState=" + _oldState + " DockingBase=" + _dockingBase.ToString();
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			// OnResize can fire during construction before assignment
			if (null != _dockingBase)
			{
				// After the base class has processed the resize event we now
				// want to update the docking control with its new values
				_dockingBase.FloatingSize = new Size(this.Width, this.Height);
			}
		}

		protected override void OnMove(EventArgs e)
		{
			// Remember the new location
			_dockingBase.FloatingLocation = this.Location;

			base.OnResize(e);
		}

	}
}
