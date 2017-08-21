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
using Crownwood.Magic.Docking;

namespace Crownwood.Magic.Docking
{
	public class DockingBase : UserControl, IDockingSite, IDockingNotify
	{
		protected bool _hidden;
		protected State _state;
		protected Permissions _permissions;
		protected Size _dockingSize;
		protected Size _dockingMinimumSize;
		protected Size _dockingMaximumSize;
		protected Size _floatingSize;
		protected Point _floatingLocation;
		protected ArrayList _contents;
		protected ArrayList _details;
		protected Control _oldParent;
		protected ExternalForm _externalForm;

		public DockingBase()
		{
			// Set the default instance values
			_hidden = false;
			_state = State.DockLeft;
			_permissions = Permissions.All;
			_dockingSize = new Size(50,50);
			_dockingMinimumSize = new Size(0,0);
			_dockingMaximumSize = new Size(2196, 2196);
			_floatingSize = new Size(200,150);
			_floatingLocation = new Point(100,100);
			_contents = new ArrayList();
			_details = new ArrayList();
			_oldParent = null;
			_externalForm = null;

			// Make dock style match default _state
			this.Dock = DockStyle.Left;
			this.Size = _dockingSize;
		}

		public virtual State State 
		{ 
			get { return _state; }
			
			set
			{
				// Nothing to do if value is unchanged
				if (value != _state)
				{
					if (CheckPermissionForState(value))
					{
						// Get each detail to update itself based on our new state
						foreach(IDockingDetail detail in _details)
							detail.OnStateChanged(value);

						switch(value)
						{
							case State.DockTop:
								if (_state == State.Floating)
									RequestRestore();								

								this.Dock = DockStyle.Top;
								RepositionAtStart();
								break;
							case State.DockLeft:
								if (_state == State.Floating)
									RequestRestore();								

								this.Dock = DockStyle.Left;
								RepositionAtStart();
								break;
							case State.DockBottom:
								if (_state == State.Floating)	
									RequestRestore();								

								this.Dock = DockStyle.Bottom;
								RepositionAtStart();
								break;
							case State.DockRight:
								if (_state == State.Floating)
									RequestRestore();								

								this.Dock = DockStyle.Right;
								RepositionAtStart();
								break;
							case State.Floating:
								RequestFloat();
								break;
						}

						_state = value;
					}
				}
			}
		}

		public virtual Permissions Permissions 
		{ 
			get { return _permissions; }
			set { _permissions = value; }
		}

		public virtual bool Hidden 
		{ 
			get { return _hidden; }
			
			set
			{
				// Change in value?
				if (_hidden != value)
				{
					_hidden = value;

					// Apply change to current docking state
					switch(_state)
					{
						case State.DockTop:
						case State.DockLeft:
						case State.DockRight:
						case State.DockBottom:
							if (_hidden)
								this.Hide();
							else
								this.Show();
							break;
						case State.Floating:
							if (_hidden)
							{
								_externalForm.Hide();
								this.Hide();
							}
							else
							{
								this.Show();
								_externalForm.Show();
							}
							break;
					}
				}
			}
		}

		public virtual DockingBase Base 
		{ 
			get { return this; }
		}

		public virtual Control DockingParent 
		{
			get
			{
				if (null != _oldParent)
					return _oldParent;
				else
					return this.Parent;
			} 
		}

		public virtual Size DockingMinimumSize
		{ 
			get { return _dockingMinimumSize; }
			
			set
			{
				// Sanity check the minimum size is less than maximum size
				if ((value.Width > _dockingMaximumSize.Width) ||
					(value.Height > _dockingMaximumSize.Height))
					throw new ArgumentOutOfRangeException("Value must be less than current Maximum");

				_dockingMinimumSize = value;

				// Apply change to current docking state
				switch(_state)
				{
					case State.DockTop:
					case State.DockLeft:
					case State.DockBottom:
					case State.DockRight:
						Size size = this.Size;

						// Calculate the new control size
						if (size.Width < _dockingMinimumSize.Width)
							size.Width = _dockingMinimumSize.Width;
					
						if (size.Height < _dockingMinimumSize.Height)
							size.Height = _dockingMinimumSize.Height;

						// Required to resize this control?
						if (this.Size != size)
						{
							this.DockingSize = size;
							this.Size = size;
							this.Refresh();
						}
						break;
				}
			}
		}

		public virtual Size DockingMaximumSize 
		{ 
			get { return _dockingMaximumSize; }
			
			set
			{
				// Sanity check the maximum size is more than the current minimum size
				if ((_dockingMinimumSize.Width > value.Width) ||
					(_dockingMinimumSize.Height > value.Height))
					throw new ArgumentOutOfRangeException("Value must be greater than current Minimum");

				_dockingMaximumSize = value;

				// Apply change to current docking state
				switch(_state)
				{
					case State.DockTop:
					case State.DockLeft:
					case State.DockBottom:
					case State.DockRight:
						Size size = this.Size;

						// Calculate the new control size
						if (size.Width > _dockingMaximumSize.Width)
							size.Width = _dockingMaximumSize.Width;
					
						if (size.Height > _dockingMaximumSize.Height)
							size.Height = _dockingMaximumSize.Height;

						// Required to resize this control?
						if (this.Size != size)
						{
							this.DockingSize = size;
							this.Size = size;
							this.Refresh();
						}
						break;
				}
			}
		}

		public virtual Size DockingSize 
		{ 
			get { return _dockingSize; }

			set
			{
				// Enforce the minimum docking size
				if (value.Width < _dockingMinimumSize.Width)
					value.Width = _dockingMinimumSize.Width;

				if (value.Height < _dockingMinimumSize.Height)
					value.Height = _dockingMinimumSize.Height;
				
				// Enforce the maximum docking size
				if (value.Width > _dockingMaximumSize.Width)
					value.Width = _dockingMaximumSize.Width;

				if (value.Height > _dockingMaximumSize.Height)
					value.Height = _dockingMaximumSize.Height;

				_dockingSize = value;

				// Apply change to current docking state
				switch(_state)
				{
					case State.DockTop:
					case State.DockBottom:
						this.Height = _dockingSize.Height;
						break;
					case State.DockLeft:
					case State.DockRight:
						this.Width = _dockingSize.Width;
						break;
				}
			}
		}

		public virtual Size FloatingSize 
		{ 
			get { return _floatingSize; }
			
			set
			{
				_floatingSize = value;

				// Apply change to current docking state
				switch(_state)
				{
					case State.Floating:
						_externalForm.Size = _floatingSize;
						break;
				}
			}
		}

		public virtual Point FloatingLocation 
		{ 
			get { return _floatingLocation; } 
			
			set
			{
				_floatingLocation = value;

				// Apply change to current docking state
				switch(_state)
				{
					case State.Floating:
						_externalForm.Location = _floatingLocation;
						break;
				}
			}
		}

		public virtual int Count 
		{ 
			get { return _contents.Count; }
		}

		public virtual Content this[int index]
		{
			get { return (_contents[index] as Content); }
		}

		public virtual void AddContent(Content content)
		{
			// Define the docking site acting as parent
			content.Parent = this;

			_contents.Add(content);
		}

		public virtual void RemoveContent(Content content)
		{
			_contents.Remove(content);

			// Remove the docking site acting as parent
			content.Parent = null;
		}

		public virtual void ClearContents()
		{
			foreach(Object obj in _contents)
			{
				Content cont = obj as Content;

				// Remove the docking site acting as parent
				cont.Parent = null;
			}

			_contents.Clear();
		}

		public virtual void HideContent(Content content) {}
		public virtual void ShowContent(Content content) {}
		public virtual void StateContent(Content content, State state) {}

		public virtual int DetailCount 
		{ 
			get { return _details.Count; }
		}

		public virtual IDockingDetail GetDetail(int index)
		{
			return (_details[index] as IDockingDetail);
		}

		public virtual void AddDetail(IDockingDetail detail)
		{
			// Provide detail with a back reference
			detail.DockingSite = this;
			detail.DockingNotify = this;

			// Store in internal list
			_details.Add(detail);

			Control controlDetail = detail as Control;

			// Is the detail also a user interface control?
			if (null != controlDetail)
			{
				// Make this control part of the appearance
				this.Controls.Add(controlDetail);
			}

			// Get the detail to adjust itself to current status
			detail.OnStateChanged(this.State);
		}
		
		public virtual void RemoveDetail(IDockingDetail detail)
		{
			_details.Remove(detail);

			Control controlDetail = detail as Control;

			// Is the detail also a user interface control?
			if (null != controlDetail)
			{
				// Make this control part of the appearance
				this.Controls.Remove(controlDetail);
			}
		}

		public virtual void ClearDetails()
		{
			foreach(IDockingDetail detail in _details)
			{
				Control controlDetail = detail as Control;

				// Is the detail also a user interface control?
				if (null != controlDetail)
				{
					// Make this control part of the appearance
					this.Controls.Remove(controlDetail);
				}
				
				// Remove back references to ourself
				detail.DockingSite = null;
				detail.DockingNotify = null;
			}

			_details.Clear();
		}

		public virtual bool IsContainer 
		{ 
			get { return false; }
		}

		public virtual Rectangle AcceptRectangle()
		{
			return new Rectangle(0, 0, 0, 0);
		}

		public virtual bool CanAccept(IDockingSite dockingSite)
		{
			return false;
		}

		public virtual bool CanAccept(IDockingSite dockingSite, Content content)
		{
			return false;
		}

		public virtual bool Accept(IDockingSite dockingSite)
		{
			int count = dockingSite.Count;

			// Process all the content from the source
			for(int index=count-1; index>=0; index--)
			{
				Content content = dockingSite[index];

				if (!Accept(dockingSite, content))
					return false;
			}

			return true;
		}

		public virtual bool Accept(IDockingSite dockingSite, Content content)
		{
			if (null != content)
			{
				// Remove content from old site
				dockingSite.RemoveContent(content);

				// Use existing method to perform addition of new content
				AddContent(content);

				return true;
			}

			return false;
		}

		public virtual IDockingSite DockingSiteFromContent(Content content)
		{
			return null;
		}

		public virtual void Kill()
		{
			// Remove all our contained items
			this.ClearContents();
			this.ClearDetails();

			// Apply change to current docking state
			switch(_state)
			{
				case State.DockTop:
				case State.DockLeft:
				case State.DockBottom:
				case State.DockRight:
					// Are we contained inside a parent container?
					if (null != this.Parent)
					{
						// Remove ourself from the parent
						Parent.Controls.Remove(this);
					}
					break;
				case State.Floating:
					if (null != _externalForm)
					{
						// Kill the external form without restoring
						_externalForm.Close();
					}
					break;	
			}	

			
			// Get rid of our resources
			Dispose();
		}

		public void RepositionAtStart()
		{	
			if (this.Parent != null)
			{
				int index = 0;

				foreach(Control child in this.Parent.Controls)
				{
					if (child is DockingBase)
						break;
					else
						index++;
				}

				this.Parent.Controls.SetChildIndex(this, index);
			}
		}

		public bool MatchingExternalForm(ExternalForm external)
		{
			return (external == _externalForm);
		}

		public virtual bool DetailHasFocus(IDockingDetail sender)
		{
			return this.ContainsFocus;
		}

		public virtual void NotifyResize(IDockingDetail sender, int xDelta, int yDelta)
		{
			switch(_state)
			{
				case State.DockTop:
				case State.DockLeft:
				case State.DockRight:
				case State.DockBottom:
					Size dockSize = this.DockingSize;

					// Find new requested size as current size with deltas
					dockSize.Width += xDelta;
					dockSize.Height += yDelta;

					// Make new size request that will enforce min/max
					this.DockingSize = dockSize;

					// Force a repaint of the control to ensure the new size is 
					// displayed, otherwise it will wait until the mouse is 
					// released before repainting.  We are assuming that this is
					// called when dragging a detail that is resizing the control.
					this.Refresh();
					break;
				case State.Floating:
					break;
			}
		}

		public virtual void NotifyCloseRequest(IDockingDetail sender)
		{
			// Inform each detail of request to fire close events
			foreach(IDockingDetail detail in _details)
				detail.OnCloseRequest();
		}

		public virtual void NotifyDetailGotFocus(IDockingDetail sender) {}
		public virtual void NotifyDetailLostFocus(IDockingDetail sender) {}

		public virtual void NotifyContentGotFocus(IDockingDetail sender)
		{
			// Inform each detail of change in focus
			foreach(IDockingDetail detail in _details)
				detail.OnDockingGotFocus();
		}

		public virtual void NotifyContentLostFocus(IDockingDetail sender)
		{
			// Inform each detail of change in focus
			foreach(IDockingDetail detail in _details)
				detail.OnDockingLostFocus();
		}

		public override DockStyle Dock
		{
			get { return base.Dock; }

			set
			{
				// Nothing to do if value is unchanged
				if (value != base.Dock)
				{
					DockStyle oldStyle = base.Dock;

					// Must have updated details before ourself
					base.Dock = value;

					switch(value)
					{
						case DockStyle.Top:
						case DockStyle.Bottom:
							this.ClientSize = new Size(this.ClientSize.Width, this.DockingSize.Height);
							break;
						case DockStyle.Left:
						case DockStyle.Right:
							this.ClientSize = new Size(this.DockingSize.Width, this.ClientSize.Height);
							break;
						case DockStyle.None:
							this.ClientSize = this.DockingSize;;
							break;
					}
	
					// Force a redraw of the entire control
					this.Refresh();
				}
			}
		}

		public override string Text
		{
			get { return base.Text; }
			
			set
			{
				base.Text = value;

				// Inform each detail of change in text property
				foreach(IDockingDetail detail in _details)
					detail.OnTextChanged(value);
			}
		}

		public bool CheckPermissionForState(State newState)
		{
			bool permission = false;

			// Check permission to restore to the old state
			switch(newState)
			{
				case State.DockTop:
					permission = ((_permissions & Permissions.DockTop) != 0);
					break;
				case State.DockLeft:
					permission = ((_permissions & Permissions.DockLeft) != 0);
					break;
				case State.DockBottom:
					permission = ((_permissions & Permissions.DockBottom) != 0);
					break;
				case State.DockRight:
					permission = ((_permissions & Permissions.DockRight) != 0);
					break;
				case State.Floating:
					permission = ((_permissions & Permissions.Floating) != 0);
					break;
			}

			return permission;
		}

		protected void RequestRestore()
		{
			if (null != _externalForm)
			{
				// Restore back to a docked state in old parent
				_externalForm.Restore(false);
				
				// No longer need the external form
				_externalForm = null;

				// Clear down old parent as not in external state
				_oldParent = null;
			}
		}

		protected void RequestFloat()
		{
			if (null == _externalForm)
			{
				// Remember the old parent
				_oldParent = this.Parent;

				// Create a top level form to host us
				_externalForm = new FloatingForm(this);

				// Move to its saved position/location
				_externalForm.Size = _floatingSize;
				_externalForm.Location = _floatingLocation;

				Form parent = _oldParent as Form;

				if (null != parent)
					_externalForm.Owner = parent;

				// Make it visible if it is supposed to be
				if (!_hidden)
					_externalForm.Show();
			}			
		}

		public override string ToString()
		{
			return "DockingBase State=" + _state + " Hidden=" + _hidden + " Details=" + _details.Count + ")";
		}
	}
}
