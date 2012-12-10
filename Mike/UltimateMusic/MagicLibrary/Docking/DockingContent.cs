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
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.Magic.Docking;

namespace Crownwood.Magic.Docking
{
	public class Content
	{
		protected Control _control;
		protected string _title;
		protected int _imageIndex;
		protected ImageList _imageList;
		protected Size _dockingSize;
		protected Size _dockingMinimumSize;
		protected Size _dockingMaximumSize;
		protected Size _floatingSize;
		protected Point _floatingLocation;
		protected bool _hidden;
		protected Permissions _permissions;
		protected IDockingSite _parent;
	
		public Content(Control control)
		{
			InternalConstruct(control, "", null, -1);
		}

		public Content(Control control, string title)
		{
			InternalConstruct(control, title, null, -1);
		}

		public Content(Control control, string title, ImageList imageList, int imageIndex)
		{
			InternalConstruct(control, title, imageList, imageIndex);
		}

		protected void InternalConstruct(Control control, string title, ImageList imageList, int imageIndex)
		{
			_control = control;
			_title = title;
			_imageList = imageList;
			_imageIndex = imageIndex;

			// Use sensible defaults for rest of properties
			_hidden = false;
			_permissions = Permissions.All;
			_dockingSize = new Size(50,50);
			_dockingMinimumSize = new Size(0,0);
			_dockingMaximumSize = new Size(2196, 2196);
			_floatingSize = new Size(200,150);
			_floatingLocation = new Point(100,100);
		}

		public Control Control
		{
			get { return _control; }
			set { _control = value; }
		}

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public ImageList ImageList
		{
			get { return _imageList; }
			set { _imageList = value; }
		}

		public int ImageIndex
		{
			get { return _imageIndex; }
			set { _imageIndex = value; }
		}

		public Size DockingSize
		{
			get { return _dockingSize; }
			set { _dockingSize = value; }
		}

		public Size DockingMinimumSize
		{
			get { return _dockingMinimumSize; }
			set { _dockingMinimumSize = value; }
		}

		public Size DockingMaximumSize
		{
			get { return _dockingMaximumSize; }
			set { _dockingMaximumSize = value; }
		}

		public Size FloatingSize
		{
			get { return _floatingSize; }
			set { _floatingSize = value; }
		}

		public Point FloatingLocation
		{
			get { return _floatingLocation; }
			set { _floatingLocation = value; }
		}

		public Permissions Permissions
		{
			get { return _permissions; }		
			set { _permissions = value; }
		}

		public bool Hidden
		{
			get { return _hidden; }
			
			set 
			{ 
				if (_hidden != value)
				{
					_hidden = value; 

					if (null != _parent)
					{
						if (_hidden)
							_parent.HideContent(this);
						else	
							_parent.ShowContent(this);
					}
				}
			}
		}

		public void SetState(State state)
		{
			if (null != _parent)
			{
				_parent.StateContent(this, state);
			}
		}

		public IDockingSite Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}
	} 
}
