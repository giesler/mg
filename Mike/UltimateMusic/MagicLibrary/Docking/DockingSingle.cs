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
using System.Collections;
using System.ComponentModel;

namespace Crownwood.Magic.Docking
{
	public class DockingSingle : DockingBase
	{
		// Instance variables
		protected EventHandler _focusGot = null; 
		protected EventHandler _focusLost = null;

		public override Permissions Permissions 
		{ 
			set 
			{ 
				// Make the new permissions effective
				base.Permissions = value; 

				// Reflect new permissions into content object
				if (this.Count == 1)
					this[0].Permissions = base.Permissions; 
			}
		}

		public override Size DockingSize 
		{ 
			set
			{
				// Make the new size effective
				base.DockingSize = value; 

				// Reflect new size into content object
				if (this.Count == 1)
					this[0].DockingSize = base.DockingSize; 
			}
		}

		public override Size DockingMinimumSize
		{ 
			set
			{
				// Make the new minimum size effective
				base.DockingMinimumSize = value; 

				// Reflect new minimum size into content object
				if (this.Count == 1)
					this[0].DockingMinimumSize = base.DockingMinimumSize; 
			}
		}

		public override Size DockingMaximumSize 
		{ 
			set
			{
				// Make the new maximum size effective
				base.DockingMaximumSize = value; 

				// Reflect new maximum size into content object
				if (this.Count == 1)
					this[0].DockingMaximumSize = base.DockingMaximumSize; 
			}
		}

		public override Size FloatingSize 
		{ 
			set
			{
				// Make the new size effective visually
				base.FloatingSize = value;

				// Reflect new size into content object
				if (this.Count == 1)
					this[0].FloatingSize = base.FloatingSize; 
			}
		}

		public override Point FloatingLocation 
		{ 
			set
			{
				// Make the new location effective visually
				base.FloatingLocation = value;

				// Reflect new location into content object
				if (this.Count == 1)
					this[0].FloatingLocation = base.FloatingLocation; 
			}
		}

		public override bool Hidden
		{ 
			set
			{
				// Make the change effective visually
				base.Hidden = value;

				// Reflect new visibility into content object
				if (this.Count == 1)
					this[0].Hidden = base.Hidden; 
			}
		}

		public override void AddContent(Content content)
		{
			// Must provide a valid reference
			if (null == content)
				throw new ArgumentNullException("Cannot add null content");

			if (null == content.Control)
				throw new ArgumentNullException("Cannot add null content.Control");

			// We only allow a single content to exist at a time and
			// so if we already have any content then get rid of it
			if (this.Count == 1)
				RemoveContent(this[0]);

			// User supplied control occupies area left after all
			// the detail objects have been positioned and sized
			content.Control.Dock = DockStyle.Fill;

			// Add to visual appearance
			Controls.Add(content.Control);

			// Reposition at start of list to ensure it is sized/positioned last
			Controls.SetChildIndex(content.Control, 0);

			// Set out title to that required by content
			this.Text = content.Title;

			// Copy across content details for use locally
			this.Permissions = content.Permissions;
			this.DockingSize = content.DockingSize;
			this.DockingMinimumSize = content.DockingMinimumSize;
			this.DockingMaximumSize = content.DockingMaximumSize;
			this.FloatingSize = content.FloatingSize;
			this.FloatingLocation = content.FloatingLocation;
			this.Hidden = content.Hidden;
			
			// Let the base class store this in collection for us
			base.AddContent(content);

			// Monitor the focus events on content
			WatchFocusOnContent();

			// Request that controls be relayed out 
			content.Control.PerformLayout();	
		}

		public override void RemoveContent(Content content)
		{
			// Must provide a valid reference
			if (null == content)
				throw new ArgumentNullException("Cannot remove null content");

			if (null == content.Control)
				throw new ArgumentNullException("Cannot add null content.Control");

			if (this.Count == 1)
			{
				if (content != this[0])
					throw new ArgumentException("No matching content to remove");

				// No longer interested in focus events on contents
				UnwatchFocusOnContent();

				// Remove from visual appearance
				Controls.Remove(this[0].Control);

				// Remove title 
				this.Text = "";
	
				// Let base class remove it from its collection
				base.RemoveContent(content);
			}
		}

		public override void ClearContents()
		{
			// We only allow a single content to exist at a time 
			// so if we have any content then get rid of it now
			if (this.Count == 1)
				RemoveContent(this[0]);
		}

		public override void HideContent(Content content) 
		{
			this.Hidden = true;
		}
		
		public override void ShowContent(Content content) 
		{
			this.Hidden = false;
		}

		public override void StateContent(Content content, State state)
		{
			this.State = state;
		}

		public override void NotifyDetailGotFocus(IDockingDetail sender)
		{
			// Set focus to the contents of the object
			if (this.Count == 1)
				this[0].Control.Focus();
			else
				NotifyContentGotFocus(null);
		}

		public override void NotifyDetailLostFocus(IDockingDetail sender)
		{
			// Assume that with no contents then focus to leaving docking control
			if (this.Count == 1)
				NotifyContentLostFocus(null);
		}

		public override string ToString()
		{
			return "DockingSingle State=" + _state + " Hidden=" + _hidden + " Details =" + _details.Count;
		}
		protected void WatchFocusOnContent()
		{
			_focusGot = new EventHandler(OnContentGotFocus);
			_focusLost =  new EventHandler(OnContentLostFocus);

			// Track the focus inside the contents
			this[0].Control.GotFocus += _focusGot;
			this[0].Control.LostFocus += _focusLost;
		}

		protected void UnwatchFocusOnContent()
		{
			// Reverse previous event hooks
			this[0].Control.GotFocus -= _focusGot;
			this[0].Control.LostFocus -= _focusLost;
		}

		protected void OnContentGotFocus(object sender, EventArgs e)
		{
			NotifyContentGotFocus(null);
		}

		protected void OnContentLostFocus(object sender, EventArgs e)
		{
			NotifyContentLostFocus(null);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			// Do we have any content defined?
			if (this.Count == 1)
			{
				// Set focus to the current content, we have focus events monitoring 
				// the content and so we will inform detail objects that focus has 
				// moved to this docking control through the OnContentGotFocus
				this[0].Control.Focus();
			}
			else
			{
				// No content, at least notify detail object that focus has
				// moved to this docking control so they can update appearance
				NotifyContentGotFocus(null);
			}

			// Let delegates fire
			base.OnGotFocus(e);
		}

	}
}