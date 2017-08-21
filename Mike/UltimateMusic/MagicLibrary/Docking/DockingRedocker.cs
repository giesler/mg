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
using System.Collections;
using System.Windows.Forms;
using Crownwood.Magic.Controls;

namespace Crownwood.Magic.Docking
{
	public class Redocker 
	{
		private class HotZone
		{
			// Instance variables
			protected Control _item;
			protected Rectangle _zone;
			protected Rectangle _draw;
			protected State _state;
			protected bool _container;

			public HotZone(Rectangle zone, Rectangle draw, State state, Control item)
			{
				_zone = zone;
				_draw = draw;
				_state = state;
				_item = item;
				_container = false;
			}

			public HotZone(Rectangle zone, Rectangle draw, Control item)
			{
				_zone = zone;
				_draw = draw;
				_state = State.Floating;
				_item = item;
				_container = true;
			}

			public Rectangle Zone		{ get { return _zone; } }
			public Rectangle DrawArea	{ get { return _draw; } }
			public State State			{ get { return _state; } }
			public Control Item			{ get { return _item; } }
			public bool Container		{ get { return _container; } }

			public bool Contains(Point point)
			{
				return _zone.Contains(point);
			}
		}

		// Class constants
		protected const int _hotVector = 20;

		// Instance variables
		private HotZone _zone = null;		// Must be private because of embedded HotZone private class
		protected Size _floatSize;
		protected Point _startCursor;
		protected Point _drawPoint;
		protected Rectangle _controlArea;
		protected Control _control = null;
		protected IDockingSite _dockingSite = null;
		protected IDockingNotify _dockingNotify = null;
		protected bool _mouseCapture = false;
		protected bool _mouseMovement = false;
		protected Rectangle _aggregateRect = new Rectangle();
		protected Rectangle _dockingRect = new Rectangle();
		protected Rectangle _mouseRect = new Rectangle();
		protected ArrayList _hotZones = new ArrayList();
		protected DateTime _lastClick = DateTime.MinValue;
		protected MouseButtons _mouseButton = MouseButtons.None;
		protected Content _content = null;
		protected bool _doubleClick = true;
		protected Control _dockingParent = null;

		// Must use parameterised constructor
		private Redocker() {}

		// DockingSite is the actual docking control to be processed
		// Control is the control to be used when performing client to screen coordinates
		// DockingParent  is the form that can accept new docking positioning of dockingsite
		public Redocker(IDockingSite dockSite, Control control, Control dockingParent)
		{
			// Mandatory parameters
			if (null == dockSite)
				throw new ArgumentNullException("Must provide IDockingSite");

			if (null == control)
				throw new ArgumentNullException("Must provide control");

			if (null == dockingParent)
				throw new ArgumentNullException("Must provide dockingParent");

			_dockingSite = dockSite;
			_dockingNotify = dockSite as IDockingNotify;
			_control = control;
			_dockingParent = dockingParent;
		}

		public Content Content
		{
			get { return _content; }
			set { _content = value; }
		}

		public bool DoubleClick
		{
			get { return _doubleClick; }
			set { _doubleClick = value; }
		}

		public void OnMouseDown(MouseEventArgs e)
		{
			Point mousePos = _control.PointToScreen(new Point(e.X, e.Y));
 
			Size clickSize = SystemInformation.DoubleClickSize;

 			// Create a region that mouse must move outside to begin redocking activity
			Rectangle mouseRect = new Rectangle(mousePos.X - clickSize.Width / 2, 
												mousePos.Y - clickSize.Height / 2, 
												clickSize.Width, clickSize.Height);

			// Call onto the method that does the real work
			OnMouseDown(e, mouseRect);
		}

		public void OnMouseDown(MouseEventArgs e, Rectangle mouseRect)
		{
			// We dont want to start the redocking activity unless the mouse
			// is pressed and the mouse moves a little distance away from its
			// pressed position.  Otherwise just clicking on a window to select
			// it would start a redocking action which might changes its dock style.

			// Have we captured the mouse yet?
			if (!_mouseCapture)
			{
				// Note that mouse capture has occured
				_mouseCapture = true;

				// Remember which button started the capture
				_mouseButton = e.Button;
				
				// Has mouse moved enough to warrant redocking?
				_mouseMovement = false;

				// Save the rectangle the mouse must move outside before redocking occurs
				_mouseRect = mouseRect;
			}
		}

		public void OnMouseMove(MouseEventArgs e)
		{
			// Only interested if mouse capture has happened
			if (_mouseCapture)
			{
				// Are we processing mouse movements yet?
				if (_mouseMovement)
					ProcessMouseMove(e);
				else
				{
					Point mousePos = _control.PointToScreen(new Point(e.X, e.Y));
	
					// Check if mouse has moved outside movement area
					if (!_mouseRect.Contains(mousePos))
					{
						// Begin redocker activity
						ProcessMouseDown(e);
						ProcessMouseMove(e);

						_mouseMovement = true;
					}				
				}
			}
		}

		public void OnMouseUp(MouseEventArgs e)
		{
			// Only interested in the same button that started activity
			if (_mouseCapture && (_mouseButton == e.Button))
			{
				// Undo any drawing to prevent screen problems
				RemoveExistingIndicater();

				if (_mouseMovement)
					ProcessMouseUp(e);

				// Reset the internal state
				_mouseCapture = false;

				if (_doubleClick)
				{
					DateTime now = DateTime.Now;
					DateTime next = _lastClick.AddMilliseconds(SystemInformation.DoubleClickTime);
				
					// Has this up event occured within double click period?
					if (next >= now)
					{
						// Is the source allowed to be floated?
						if (PermissionAllowed(Permissions.Floating))
						{
							if (null != _content)
							{
								// Remember the parent as the docking site might not exist after DockingSiteFromContent
								Control parent = _dockingSite.DockingParent;
				
								// Get hold of a new docking site to represent the new contents
								IDockingSite newSite = _dockingSite.DockingSiteFromContent(_content);
					
								// Add the site to the same parent form
								parent.Controls.Add(newSite.Base);

								// Ensure the new site is placed in the floating state
								newSite.State = State.Floating;
							}
							else
							{
								// Tell the control to become floating
								_dockingSite.State = State.Floating;
							}
						}
					}

					// Remember for next time around
					_lastClick = now;
				}
			}
		}

		public virtual void ProcessMouseDown(MouseEventArgs e)
		{
			// Remove any existing information
			_hotZones.Clear();

			// Which docking control are we being used for
			DockingBase dockingBase = _dockingSite.Base;

			// Find the parents client area in screen coordinates
			Rectangle area = _dockingParent.RectangleToScreen(_dockingParent.ClientRectangle);

			ArrayList toplist = new ArrayList();
			ArrayList leftlist = new ArrayList();
			ArrayList rightlist = new ArrayList();
			ArrayList bottomlist = new ArrayList();

			// We want lists of docked controls grouped by style
			foreach(Control item in _dockingParent.Controls)
			{
				if (item.Visible)
				{
					if (item.Dock == DockStyle.Top)
						toplist.Insert(0, item);
					if (item.Dock == DockStyle.Left)
						leftlist.Insert(0, item);
					if (item.Dock == DockStyle.Bottom)
						bottomlist.Insert(0, item);
					if (item.Dock == DockStyle.Right)
						rightlist.Insert(0, item);
				}
			}

			int index = -1;

			// Size of the docking site being moved
			Size sourceSize = _dockingSite.DockingSize;

			// If moving just a content and not whole docking site
			if (null != _content)
			{
				// Then recover the size required from the content only
				sourceSize = _content.DockingSize;
			}

			GenerateTopZones(toplist, area, sourceSize, ref index);
			GenerateLeftZones(leftlist, area, sourceSize, ref index);
			GenerateBottomZones(bottomlist, area, sourceSize, ref index);
			GenerateRightZones(rightlist, area, sourceSize, ref index);
			GenerateExternalZones();

			// Was a hot zone added for the control being dragged?
			if (index != -1)
			{
				// Make this hotzone first in list, so whenever the mouse is over 
				// that control it always leaves its positon uneffected

				object zone = _hotZones[index];
				
				_hotZones.RemoveAt(index);
				_hotZones.Insert(0, zone);
			}

			// No floating indicator currently drawn
			_drawPoint = new Point(-1, -1);

			// Remember cursor position when pressed
			_startCursor = _control.PointToScreen(new Point(e.X, e.Y));

			// Remember docking control screen area
			_controlArea = dockingBase.RectangleToScreen(dockingBase.ClientRectangle);

			// Find size of floated control
			_floatSize = new Size(_dockingSite.FloatingSize.Width, _dockingSite.FloatingSize.Height);
		}

		public virtual void ProcessMouseMove(MouseEventArgs e)
		{
			// Show a useful cursor to indicate movement
			_control.Cursor = Cursors.Hand;

			// Find current mouse position
			Point position = _control.PointToScreen(new Point(e.X, e.Y));

			bool bFound = false;

			// Search for any hotzone we are in
			foreach(HotZone hot in _hotZones)
			{
				if (hot.Contains(position))
				{
					// Remove any existing floating indicator
					if (_drawPoint.X >= 0)
						DrawFloatingIndicator(new Point(-1, -1));

					if (hot.Container)
					{
						// Remove any existing aggregate indicator
						if (_dockingRect.Width != 0)
							DrawDockedIndicator(new Rectangle());

						// Draw the indicator of new docking position
						DrawAggregateIndicator(hot.DrawArea);
					}
					else
					{
						// Remove any existing aggregate indicator
						if (_aggregateRect.Width != 0)
							DrawAggregateIndicator(new Rectangle());

						// Draw the indicator of new docking position
						DrawDockedIndicator(hot.DrawArea);
					}
										
					// Remember our zone
					_zone = hot;
		
					bFound = true;
					break;
				}
			}

			// Did we find a zone?
			if (!bFound)
			{
				// Remove any existing docking indicator
				if (_dockingRect.Width != 0)
					DrawDockedIndicator(new Rectangle());

				// Remove any existing aggregate indicator
				if (_aggregateRect.Width != 0)
					DrawAggregateIndicator(new Rectangle());

				// Show the floating position it would be
				DrawFloatingIndicator(position);

				// Not inside any zone
				_zone = null;
			}
		}

		public virtual void ProcessMouseUp(MouseEventArgs e)
		{
			// Did we find somewhere to dock?
			if (null != _zone)
			{
				if (_zone.Container)
				{
					IDockingSite itemSite = _zone.Item as IDockingSite;

					// Move the whole source docking site?		
					if (null == _content)
					{
						// Add into the new aggregate control (it is the responsabiltiy of
						// the receiving container to extract whatever it needs from the site)
						itemSite.Accept(_dockingSite);

						if (_dockingSite.Count == 0)
						{
							// Now the docking control is no longer required, if the container
							// wants it then it will already have stolen its contents
							_dockingSite.Kill();
						}
					}
					else
					{
						// Provide just a single content to the new docking site
						itemSite.Accept(_dockingSite, _content);

						// No longer needed at source end, so remove it
						_dockingSite.RemoveContent(_content);
					}
				}
				else
				{
					// Default to last in ordering, which is the first DockingBase derived object
					int pos = 0;

					foreach(Control child in _dockingParent.Controls)
					{
						if (child is DockingBase)
							break;
						else
							pos++;
					}					

					if (null == _zone.Item)
					{
						switch(_zone.State)
						{
							case State.DockTop:
								pos = LastPositionOfStyle(DockStyle.Top, _dockingParent.Controls) + 1;
								break;
							case State.DockBottom:
								pos = LastPositionOfStyle(DockStyle.Bottom, _dockingParent.Controls) + 1;
								break;
							case State.DockLeft:
								pos = LastPositionOfStyle(DockStyle.Left, _dockingParent.Controls) + 1;
								break;
							case State.DockRight:
								pos = LastPositionOfStyle(DockStyle.Right, _dockingParent.Controls) + 1;
								break;
						}
					}
					else
					{
						// Find the items current position
						pos = _dockingParent.Controls.GetChildIndex(_zone.Item);
					}

					IDockingSite siteMove = _dockingSite;

					// Move just a content from the source docking site?
					if (null != _content)
					{
						// Get hold of a new docking site to represent then new contents
						siteMove = _dockingSite.DockingSiteFromContent(_content);

						// Add new docking site to the parent
						_dockingParent.Controls.Add(siteMove.Base);
					}

					// Set the new docking position
					siteMove.State = _zone.State;
		
					DockingBase dockingBase = siteMove.Base;

					// Find ourself in the list
					int us = _dockingParent.Controls.GetChildIndex(dockingBase);

					// If resetting the position further forward the we need to 
					// subtract 1 as 'SetChildIndex' seems to remove the presented
					// control and then insert it in the given position.  So the 
					// removal will mean there is one less in the list to count against.
					if (us < pos)
						pos--;

					_dockingParent.Controls.SetChildIndex(dockingBase, pos);
				}
			}
			else
			{
				// Is the source allowed to be floating?
				if (PermissionAllowed(Permissions.Floating))
				{
					// Find current mouse position in screen coordinates BEFORE 
					// constructing the floating control which will reparent the control
					Point cursor = _control.PointToScreen(new Point(e.X, e.Y));

					// Set to the correct location to that drawn by floating indicator
					Point location = new Point(cursor.X - _startCursor.X + _controlArea.Left,
						cursor.Y - _startCursor.Y + _controlArea.Top);

					IDockingSite siteMove = _dockingSite;

					// Move just a content from the source docking site?
					if (null != _content)
					{
						// Get hold of a new docking site to represent then new contents
						siteMove = _dockingSite.DockingSiteFromContent(_content);

						// Add new docking site to the parent
						_dockingParent.Controls.Add(siteMove.Base);
					}

					// Define the new position for floating control to appear
					siteMove.FloatingLocation = location;

					// Tell the control to become floating
					siteMove.State = State.Floating;
				}
			}

			// Remove any existing information
			_hotZones.Clear();
		}

		public override string ToString()
		{
			return "Redocker HotZones=" + _hotZones.Count + " DoubleClick=" + _doubleClick;
		}
		protected void GenerateTopZones(ArrayList toplist, Rectangle area, Size sourceSize, ref int index)
		{
			// Is the source allowed to be docked to the Top?
			bool allowed = PermissionAllowed(Permissions.DockTop);

			int vector = area.Top;
			Control last = null;

			foreach(Control item in toplist)
			{
				Rectangle rect = AddItemAsHotZone(item, State.DockTop, last, sourceSize, allowed, ref index);

				// Move down the screen										  
				vector = rect.Bottom;

				// Remember the last X position added
				area.X = rect.Left;
				area.Width = rect.Width;

				last = item;
			}

			if (allowed)
			{
				_hotZones.Add(new HotZone(new Rectangle(area.Left, vector, area.Width, _hotVector), 
										  new Rectangle(area.Left, vector, area.Width, sourceSize.Height),
										  State.DockTop, last));		
			}
		}

		protected void GenerateLeftZones(ArrayList leftlist, Rectangle area, Size sourceSize, ref int index)
		{
			// Is the source allowed to be docked to the Left?
			bool allowed = PermissionAllowed(Permissions.DockLeft);
			
			int vector = area.Left;
			Control last = null;

			foreach(Control item in leftlist)
			{
				Rectangle rect = AddItemAsHotZone(item, State.DockLeft, last, sourceSize, allowed, ref index);

				// Move down the screen										  
				vector = rect.Right;

				// Remember the last X position added
				area.Y = rect.Top;
				area.Height = rect.Height;

				last = item;
			}

			if (allowed)
			{
				_hotZones.Add(new HotZone(new Rectangle(vector, area.Top, _hotVector, area.Height), 
										  new Rectangle(vector, area.Top, sourceSize.Width, area.Height),
										  State.DockLeft, last));		
			}
		}

		protected void GenerateBottomZones(ArrayList bottomlist, Rectangle area, Size sourceSize, ref int index)
		{
			// Is the source allowed to be docked to the Bottom?
			bool allowed = PermissionAllowed(Permissions.DockBottom);

			int vector = area.Bottom;
			Control last = null;

			foreach(Control item in bottomlist)
			{
				Rectangle rect = AddItemAsHotZone(item, State.DockBottom, last, sourceSize, allowed, ref index);

				// Move down the screen										  
				vector = rect.Top;

				// Remember the last X position added
				area.X = rect.Left;
				area.Width = rect.Width;

				last = item;
			}

			if (allowed)
			{
				_hotZones.Add(new HotZone(new Rectangle(area.Left, vector - _hotVector, area.Width, _hotVector), 
										  new Rectangle(area.Left, vector - sourceSize.Height, area.Width, sourceSize.Height),
										  State.DockBottom, last));		
			}
		}

		protected void GenerateRightZones(ArrayList rightlist, Rectangle area, Size sourceSize, ref int index)
		{
			// Is the source allowed to be docked to the Right?
			bool allowed = PermissionAllowed(Permissions.DockRight);

			int vector = area.Right;
			Control last = null;

			foreach(Control item in rightlist)
			{
				Rectangle rect = AddItemAsHotZone(item, State.DockRight, last, sourceSize, allowed, ref index);

				// Move down the screen										  
				vector = rect.Left;

				// Remember the last X position added
				area.Y = rect.Top;
				area.Height = rect.Height;

				last = item;
			}

			if (allowed)
			{
				_hotZones.Add(new HotZone(new Rectangle(vector - _hotVector, area.Top, _hotVector, area.Height), 
										  new Rectangle(vector - sourceSize.Width, area.Top, sourceSize.Width, area.Height),
										  State.DockRight, last));		
			}
		}

		protected void GenerateExternalZones()
		{
			Form formParent = _dockingParent as Form;

			if (null != formParent)
			{
				// Get collection of Forms owned
				Form[] forms = formParent.OwnedForms;

				foreach(Form owned in forms)
				{
					ExternalForm external = owned as ExternalForm;

					// Check that the Form is a type we know of
					if (null != external) 
					{
						// Can accept back into ourself!
						if (!_dockingSite.MatchingExternalForm(external))
						{
							// Grab the single docking object inside the form
							DockingBase content = external.Content;

							if (null != content)
							{
								// Can this docking object act as a container?
								if (content.IsContainer)
								{
									bool accept = false;

									if (null != _content)
										accept = content.CanAccept(_dockingSite, _content);
									else
										accept = content.CanAccept(_dockingSite);

									// Will it accept us?
									if (accept)
									{
										HotZone newZone = new HotZone(content.RectangleToScreen(content.ClientRectangle), 
																	  content.AcceptRectangle(), 
																	  content); 

										_hotZones.Add(newZone);
									}
								}
							}
						}
					}
				}
			}
		}

		protected Rectangle AddItemAsHotZone(Control item, State state, Control last, Size sourceSize, bool allowed, ref int index)
		{
			// Find hot rectangle for item
			Rectangle rectHot = ControlSizeToScreen(item);

			// Make copies
			Rectangle rectDisplay = rectHot;
			Rectangle rectSelect = rectHot;

			switch(state)
			{
			case State.DockLeft:
				rectDisplay.Width = sourceSize.Width;
				rectSelect.Width = _hotVector;
				break;
			case State.DockTop:
				rectDisplay.Height = sourceSize.Height;
				rectSelect.Height = _hotVector;
				break;
			case State.DockRight:
				rectDisplay.X = rectDisplay.Right - sourceSize.Width;
				rectDisplay.Width = sourceSize.Width;
				rectSelect.Width = _hotVector;
				break;
			case State.DockBottom:
				rectDisplay.Y = rectDisplay.Bottom - sourceSize.Height;
				rectDisplay.Height = sourceSize.Height;
				rectSelect.Y = rectDisplay.Bottom - _hotVector;
				rectSelect.Height = _hotVector;
				break;
			}

			// Each item by default is not a container
			bool container = false;

			IDockingSite dockingItem = item as IDockingSite;

			// Is this item one we recognise?
			if (null != dockingItem)
				container = dockingItem.IsContainer;

			// Is the item in question actually us!
			bool self = (_dockingSite == item);

			// If we have a container then set the hot zone to be the whole of the
			// controls display area (rectHot).  It it is a container then restrict 
			// the hot zone to extend only the hot vector into the control (rectSelect)
			Rectangle rect = (container && !self) ? rectSelect : rectHot;

			// Create new hot zone to represent that display rectangle
			HotZone newZone = new HotZone(rect, rectDisplay, state, last);

			int newIndex = 0;

			// Do we have permission to add this zone?
			if (allowed)
			{
				// Add at end of the current list
				newIndex = _hotZones.Add(newZone);					
			}

			// If this zone represents the control that is being dragged
			if (self)
			{
				// Then return to caller the hot zone that represents it
				index = newIndex;
			}
			else
			{
				// If the control acts as a container for dragging docking controls
				if (container)
				{
					if (null != dockingItem)
					{
						bool accept = false;

						if (null != _content)
							accept = dockingItem.CanAccept(_dockingSite, _content);
						else
							accept = dockingItem.CanAccept(_dockingSite);

						// Will it accept this item?
						if (accept)
						{
							// Create new hot zone to represent aggregation into it
							newZone = new HotZone(rectHot, dockingItem.AcceptRectangle(), item);

							_hotZones.Add(newZone);
						}
					}
				}
			}

			return rectHot;
		}

		protected Rectangle ControlSizeToScreen(Control item)
		{
			// Need to user .Size to get the actual displayed size of the control when
			// docked. Using .ClientSize or .ClientRectangle returns its non docked size
			// event when it is docked!
			Size clientsize = item.Size;

			// Convert to screen coordinates
			return item.RectangleToScreen(new Rectangle(0, 0, clientsize.Width, clientsize.Height));
		}

		protected bool PermissionAllowed(Permissions requested)
		{
			if (null != _content)
			{
				// Check permission for a content within the docking base
				return (_content.Permissions & requested) != 0;
			}
			else
			{
				// Check permission for a docking base
				return (_dockingSite.Permissions & requested) != 0;
			}
		}

		private int LastPositionOfStyle(DockStyle ds, Control.ControlCollection controls)
		{
			for(int index=controls.Count - 1; index>=0; index--)
			{
				if (controls[index].Dock == ds)
					return index;
					
			}

			return 0;
		}

		private void DrawDockedIndicator(Rectangle zone)
		{
			// If the same rectangle details then do nothing
			if (zone != _dockingRect)
			{
				// Remove existing rectangle by drawing it again
				if (_dockingRect.Width != 0)
					ControlPaint.DrawReversibleFrame(_dockingRect, Color.Gray, FrameStyle.Thick);
				
				_dockingRect = zone;

				// Draw the new rectangle
				if (_dockingRect.Width != 0)
					ControlPaint.DrawReversibleFrame(_dockingRect, Color.Gray, FrameStyle.Thick);
			}
		}

		private void DrawAggregateIndicator(Rectangle zone)
		{
			// If the same rectangle details then do nothing
			if (zone != _aggregateRect)
			{
				// Remove existing rectangle by drawing it again
				if (_aggregateRect.Width != 0)
					ControlPaint.FillReversibleRectangle(_aggregateRect, Color.Gray);
				
				_aggregateRect = zone;

				// Draw the new rectangle
				if (_aggregateRect.Width != 0)
					ControlPaint.FillReversibleRectangle(_aggregateRect, Color.Gray);
			}
		}

		private void DrawFloatingIndicator(Point position)
		{
			// Is the source allowed to be floating?
			if (PermissionAllowed(Permissions.Floating))
			{
				if (position != _drawPoint)
				{
					// Remove existing rectangle by drawing it again
					if (_drawPoint.X >= 0)
					{
						// Offset of point from original point
						Rectangle rectOld = new Rectangle(_controlArea.Location, _floatSize);
						
						rectOld.Offset(_drawPoint.X - _startCursor.X, _drawPoint.Y - _startCursor.Y);

						ControlPaint.DrawReversibleFrame(rectOld, Color.Gray, FrameStyle.Dashed);
					}
					
					_drawPoint = position;
					
					// Draw the new rectangle
					if (_drawPoint.X >= 0)
					{
						// Offset of point from original point
						Rectangle rectNew = new Rectangle(_controlArea.Location, _floatSize);
						
						rectNew.Offset(_drawPoint.X - _startCursor.X, _drawPoint.Y - _startCursor.Y);

						ControlPaint.DrawReversibleFrame(rectNew, Color.Gray, FrameStyle.Dashed);
					}
				}
			}
		}

		private void RemoveExistingIndicater()
		{
			// Remove any existing indicators
			DrawDockedIndicator(new Rectangle());
			DrawAggregateIndicator(new Rectangle());
			DrawFloatingIndicator(new Point(-1, -1));
		}

	}
}