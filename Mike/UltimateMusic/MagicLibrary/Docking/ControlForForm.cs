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
	class ControlForForm : UserControl
	{
		protected Form _contents = null;
		protected IDockingNotify _dockingNotify = null;
			
		// Must use parameterised constructor
		private ControlForForm() {}

		public ControlForForm(IDockingNotify notify, Form form)
		{
			// Check for mandatory parameters
			if (null == notify)
				throw new ArgumentNullException("Must provide IDockingNotify parameter");

			if (null == form)
				throw new ArgumentNullException("Must provide a Form parameter");

			// Remember passed in parameters
			_dockingNotify = notify;
			_contents = form;
				
			// Have to ensure the Form is not a top level form
			_contents.TopLevel = false;

			// We are the new parent of this form
			_contents.Parent = this;

			// To prevent user resizing the form manually and prevent
			// the caption bar appearing, we use the 'None' border style.
			_contents.FormBorderStyle = FormBorderStyle.None;

			// It should fill our entire client area
			_contents.Dock = DockStyle.Fill;

			// We want to know when the form area has the mouse pressed
			_contents.MouseDown += new MouseEventHandler(OnFormMouseDown);

			// Need to be notified if any child (recursively) of the
			// form gets or loses focus, so we can tell the docking site
			// so it can take appropriate action
			MonitorAllControls(_contents);

			// Show it !!
			_contents.Show();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			// Pass focus straight into the embedded form
			if (null != _contents)
				_contents.Focus();

			base.OnGotFocus(e);
		}

		protected void OnFormMouseDown(Object sender, MouseEventArgs e)
		{
			// Set the focus to the form and so the first 
			// if the form has any controls one will be focused
			if (null != _contents)
				_contents.Focus();

			// Inform docking site we have focus
			_dockingNotify.NotifyContentGotFocus(null); 
		}

		protected void MonitorAllControls(Control control)
		{
			ControlCollection col = control.Controls;			

			foreach(Control child in col)
			{
				child.GotFocus += new EventHandler(OnChildGotFocus);
				child.LostFocus += new EventHandler(OnChildLostFocus);

				// Recurse into all children of this child
				MonitorAllControls(child);
			}
		}

		protected void OnChildGotFocus(Object sender, EventArgs e)
		{
			_dockingNotify.NotifyContentGotFocus(null); 
		}

		protected void OnChildLostFocus(Object sender, EventArgs e)
		{
			// We dont know it the focus is moving staight to another
			// control on the same form, but it doesnt really matter as
			// we will catch that event next and process it
			_dockingNotify.NotifyContentLostFocus(null); 
		}

		public override string ToString()
		{
			return "ControlForForm Form(" + _contents + ")";
		}
	}
}
