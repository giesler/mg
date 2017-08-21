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
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Crownwood.Magic.Controls
{
	public class InertButton : Button
	{
		protected bool _mouseCapture = false;
		protected MouseButtons _mouseButton = MouseButtons.None;

		public InertButton()
		{
			// Prevent drawing flicker by blitting from memory
			SetStyle(ControlStyles.DoubleBuffer, true);

			// Should not be allowed to select this control
			SetStyle(ControlStyles.Selectable, false);
		}

		public override string ToString()
		{
			return "InertButton";
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!_mouseCapture)
			{
				// Mouse is over the button and being pressed, so change
				// state and remember the original button pressed as we only
				// care about the same button when it goes up
				_mouseCapture = true;
				_mouseButton = e.Button;
			}

			// Let delegates fire thru base
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			// Are we waiting for this button to go up?
			if (e.Button == _mouseButton)
			{
				// Generate a click event?
				if (_mouseCapture)
					OnClick(new EventArgs());

				// Set state back to become normal
				_mouseCapture = false;
			}

			// Let delegates fire thru base
			base.OnMouseUp(e);
		}

	}
}