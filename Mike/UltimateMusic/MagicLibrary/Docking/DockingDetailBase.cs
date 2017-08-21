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
	public abstract class DockingDetailBase : UserControl, IDockingDetail
	{
		// Instance variables
		protected string _category = "None";
		protected IDockingSite _dockingSite = null;
		protected IDockingNotify _dockingNotify = null;
		protected Orientation _orientation = Orientation.Standard;
		protected Content _content;

		public virtual IDockingSite DockingSite
		{
			get { return _dockingSite; }
			set { _dockingSite = value; }
		}

		public virtual IDockingNotify DockingNotify
		{
			get { return _dockingNotify; }
			set { _dockingNotify = value; }
		}

		public string Category
		{
			get { return _category; }
			set { _category = value; }
		}

		public Orientation Orientation 
		{ 
			get { return _orientation; } 
			set { _orientation = value; } 
		}		

		public Content Content
		{
			get { return _content; }
			set { _content = value; }
		}

		public virtual void OnCloseRequest() {}
		public virtual void OnDockingGotFocus() {}
		public virtual void OnDockingLostFocus() {}
		public virtual void OnTextChanged(string title) {}
		public virtual void OnStateChanged(State value) {}

		public override string ToString()
		{
			return "DockingDetailBase Category=" + _category + " Orientation=" + _orientation;
		}
	
		protected override void OnGotFocus(EventArgs e)
		{
			if (null != _dockingNotify)
			{
				// Inform docking site we have the focus
				_dockingNotify.NotifyDetailGotFocus(this);
			}

			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			if (null != _dockingNotify)
			{
				// Inform docking site we have lost focus
				_dockingNotify.NotifyDetailLostFocus(this);
			}

			base.OnLostFocus(e);
		}		
	}
}