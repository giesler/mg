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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Crownwood.Magic.Docking;

namespace Crownwood.Magic.Docking
{
	public class BorderForControl : UserControl
	{
		private int _borderWidth = 4;
		private Container components = null;
		private IDockingNotify _dockingNotify = null;

		// This constructor should never be used.
		private BorderForControl() {}

		public BorderForControl(IDockingNotify notify, Control userControl, int borderWidth)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// Check for mandatory parameters
			if (null == notify)
				throw new ArgumentNullException("Must provide IDockingNotify parameter");

			// Remember parameters
			_dockingNotify = notify;
			_borderWidth = borderWidth;
			
			if (null != userControl)
			{
				// Remove any docking style on passed in control
				userControl.Dock = DockStyle.None;

				// Track focus changes to user supplied control
				userControl.GotFocus += new EventHandler(OnControlGotFocus);
				userControl.LostFocus += new EventHandler(OnControlLostFocus);

				// Add as a child control
				Controls.Add(userControl);
			}
		}	

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		public int BorderWidth
		{
			get { return _borderWidth; }
			set { _borderWidth = value; }
		}

		public override string ToString()
		{
			return "BorderForControl Width=" + _borderWidth;
		}

		protected void OnControlGotFocus(Object sender, EventArgs e)
		{
			_dockingNotify.NotifyContentGotFocus(null);  
		}

		protected void OnControlLostFocus(Object sender, EventArgs e)
		{
			_dockingNotify.NotifyContentLostFocus(null);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			// We never want focus, set it to the child control instead
			if (Controls.Count >= 1)
				Controls[0].Focus();

			// If failed to set focus away then we must remember to fire delegates 
			// as any docking control that contains us will be watching for this event
			if (this.Focused)
				base.OnGotFocus(e);
		}
		
		protected override void OnResize(EventArgs e)
		{
			ResizeOnlyTheChild();

			base.OnResize(e);
		}
		
		protected override void OnLayout(LayoutEventArgs e)	
		{
			ResizeOnlyTheChild();

			base.OnLayout(e);
		}

		protected void ResizeOnlyTheChild()
		{
			// Do we have a child control to resize? 
			if (Controls.Count >= 1)
			{
				Size ourSize = this.Size;

				// Get the first child (there should not be any others)
				Control child = this.Controls[0];

				// Define new position
				child.Location = new Point(_borderWidth, _borderWidth);

				// Define new size
				child.Size = new Size(ourSize.Width - _borderWidth * 2, 
									  ourSize.Height - _borderWidth * 2);
			}
		}
	
	}
}
