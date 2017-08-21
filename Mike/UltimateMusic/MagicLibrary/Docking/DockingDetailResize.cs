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

namespace Crownwood.Magic.Docking
{
	public class DockingDetailResizeBase : DockingDetailBase
	{
		// Instance variables
		protected int _length;
		protected bool _resize;
		protected Point _point;
		protected Point _pointLast;
		protected bool _siteRestrictions;
		
		// Must use parameterised constructor
		private DockingDetailResizeBase() {}

		public DockingDetailResizeBase(int length, bool siteRestrictions)
		{
			// Remember for when added to docking object
			_length = length;

			// Our size is always fixed at the required length in both directions
			// as one of the sizes will be provided for us because of our docking
			this.Size = new Size(_length, _length);

			// Not in resizing mode at the moment
			_resize = false;

			// How are the min/max sizing contraints calculated, if true then it
			// uses the values from the parent IDockingSite.  Otherwise it uses a
			// simple calculation based client area of parent window.
			_siteRestrictions = siteRestrictions;

			// Define our category
			this.Category = "Resize";
		}	

		public override IDockingSite DockingSite
		{
			set 
			{ 
				// We want to ensure the minimum size is at least ours
				value.DockingMinimumSize = new Size(value.DockingMinimumSize.Width + _length, 
											        value.DockingMinimumSize.Height + _length);

				// Call base class properties
				base.DockingSite = value;
				base.DockingNotify = value as IDockingNotify;
			}
		}

		public override void OnStateChanged(State value)
		{ 
			switch(value)
			{
			case State.DockLeft:
				this.Show();
				switch(_orientation)
				{
				case Orientation.Inline:
					this.Dock = DockStyle.Top;
					break;
				case Orientation.Standard:
				default:
					this.Dock = DockStyle.Right;
					break;
				}
				break;
			case State.DockTop:
				this.Show();
				switch(_orientation)
				{
				case Orientation.Inline:
					this.Dock = DockStyle.Left;
					break;
				case Orientation.Standard:
				default:
					this.Dock = DockStyle.Bottom;
					break;
				}
				break;
			case State.DockRight:
				this.Show();
				switch(_orientation)
				{
				case Orientation.Inline:
					this.Dock = DockStyle.Top;
					break;
				case Orientation.Standard:
				default:
					this.Dock = DockStyle.Left;
					break;
				}
				break;
			case State.DockBottom:
				this.Show();
				switch(_orientation)
				{
				case Orientation.Inline:
					this.Dock = DockStyle.Left;
					break;
				case Orientation.Standard:
				default:
					this.Dock = DockStyle.Top;
					break;
				}
				break;
			case State.Floating:
				this.Hide();
				break;
			}
		}

		public override string ToString()
		{
			return "DockingDetailResizeBase Length=" + _length + " SiteRestrictions=" + _siteRestrictions;
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Remember the mouse position and client size when capture occured
			_pointLast = _point = PointToScreen(new Point(e.X, e.Y));

			// Mouse down occured inside control
			_resize = true;
		
			// Show the resize indicator on the screen
			DrawResizeBar(DrawingPoint(_point));

			// Let delegates fire through base
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			// Did the matching mouse down occur inside control?
			if (_resize)
			{
				// Remove the resize indicator by drawing it again
				DrawResizeBar(DrawingPoint(_pointLast));

				// Find the new mouse position
				Point mouse = PointToScreen(new Point(e.X, e.Y));

				// Have we actually moved the mouse?
				if (mouse != _point)
				{
					// Get our dimensions in screen coordinates
					Rectangle client = RectangleToScreen(this.ClientRectangle);

					// Calculate mouse position delta from mouse down
					Point delta = new Point(mouse.X - _point.X, mouse.Y - _point.Y);

					// Bound the delta by parent resizable area
					BoundDelta(client, ref delta);

					// We only want the delta for the correct dimension we 
					// are resizing and remember to invert delta for dragging 
					// in opposite direction to coordinate system
					switch(this.Dock)
					{
					case DockStyle.Left:
						delta.Y = 0;
						delta.X = -delta.X;
						break;
					case DockStyle.Top:
						delta.X = 0;
						delta.Y = -delta.Y;
						break;
					case DockStyle.Right:
						delta.Y = 0;
						break;
					case DockStyle.Bottom:
						delta.X = 0;
						break;
					}

					// Tell our parent docking object of requested change in size
					_dockingNotify.NotifyResize(this, delta.X, delta.Y);
				}

				// Reset ready for next time around
				_resize = false;
			}

			// Let delegates fire through base
			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Cursor depends on if we are vertical or horizontal resize
			if ((this.Dock == DockStyle.Top) || 
				(this.Dock == DockStyle.Bottom))
				this.Cursor = Cursors.HSplit;
			else
				this.Cursor = Cursors.VSplit;

			// Can only resize if we have captured the mouse
			if (this.Capture)
			{
				// Find new drawing position
				Point pointNew = PointToScreen(new Point(e.X, e.Y));
		
				Point oldDraw = DrawingPoint(_pointLast);
				Point newDraw = DrawingPoint(pointNew);

				// Only redraw if position has changed
				if (oldDraw != newDraw)
				{
					DrawResizeBar(oldDraw);
					_pointLast = pointNew;		
					DrawResizeBar(newDraw);
				}
			}

			// Let delegates fire through base
			base.OnMouseMove(e);
		}

		protected Point DrawingPoint(Point cursor)
		{
			// Get our dimensions in screen coordinates
			Rectangle client = RectangleToScreen(this.ClientRectangle);

			// Calculate mouse position delta from mouse down
			Point delta = new Point(cursor.X - _point.X, cursor.Y - _point.Y);

			// Bound the delta by parent resizable area
			BoundDelta(client, ref delta);

			return delta;
		}

		protected void DrawResizeBar(Point cursor)
		{
			// Get our dimensions in screen coordinates
			Rectangle client = RectangleToScreen(this.ClientRectangle);

			// Cursor depends on if we are vertical or horizontal resize
			if ((this.Dock == DockStyle.Left) || (this.Dock == DockStyle.Right))
				client.Offset(cursor.X, 0);
			else
				client.Offset(0, cursor.Y);

			ControlPaint.FillReversibleRectangle(client, Color.Gray);
		}

		protected void BoundDelta(Rectangle client, ref Point delta)
		{
			Control dockingParent = _dockingSite.Base.Parent;

			// Find the parents client area in screen coordinates
			Rectangle parent = dockingParent.RectangleToScreen(dockingParent.ClientRectangle);

			int xMax = parent.Right - client.Right;
			int xMin = parent.Left - client.Left;
			int yMax = parent.Bottom - client.Bottom;
			int yMin = parent.Top - client.Top;

			if (_siteRestrictions)
			{
				// Limit the offset by the control setting
				Size dockSize = _dockingSite.DockingSize;
				Size sizeMin = _dockingSite.DockingMinimumSize;
				Size sizeMax = _dockingSite.DockingMaximumSize;

				switch(this.Dock)
				{
				case DockStyle.Top:
					if (yMax > (dockSize.Height - sizeMin.Height))
						yMax = dockSize.Height - sizeMin.Height;

					if (yMin < (dockSize.Height - sizeMax.Height))
						yMin = dockSize.Height - sizeMax.Height;
					break;
				case DockStyle.Left:
					if (xMax > (dockSize.Width - sizeMin.Width))
						xMax = dockSize.Width - sizeMin.Width;

					if (xMin < (dockSize.Width - sizeMax.Width))
						xMin = dockSize.Width - sizeMax.Width;
					break;
				case DockStyle.Bottom:
					if (yMax > (sizeMax.Height - dockSize.Height))
						yMax = sizeMax.Height - dockSize.Height;

					if (yMin < (sizeMin.Height - dockSize.Height))
						yMin = sizeMin.Height - dockSize.Height;
					break;
				case DockStyle.Right:
					if (xMax > (sizeMax.Width - dockSize.Width))
						xMax = sizeMax.Width - dockSize.Width;

					if (xMin < (sizeMin.Width - dockSize.Width))
						xMin = sizeMin.Width - dockSize.Width;
					break;
				}
			}

			if (delta.X > xMax)
				delta.X = xMax;

			if (delta.X < xMin)
				delta.X = xMin;

			if (delta.Y > yMax)
				delta.Y = yMax;

			if (delta.Y < yMin)
				delta.Y = yMin;
		}

	}

	public class DockingDetailResizeIDE : DockingDetailResizeBase
	{
		// Class constant
		protected const int _lengthIDE = 4;

		public DockingDetailResizeIDE(bool siteRestrictions)
			: base(_lengthIDE, siteRestrictions)
		{
		}	

		public override string ToString()
		{
			return "DockingDetailResizeIDE Length(" + _length + ") SiteRestrictions(" + _siteRestrictions + ")";
		}
	}

	public class DockingDetailResizePlain : DockingDetailResizeBase
	{
		// Class constant
		protected const int _lengthPlain = 6;

		public DockingDetailResizePlain(bool siteRestrictions)
			: base(_lengthPlain, siteRestrictions)
		{
			// Prevent drawing flicker by blitting from memory
			this.SetStyle(ControlStyles.DoubleBuffer, true);
		}	

		public override string ToString()
		{
			return "DockingDetailResizePlain Length=" + _length + " SiteRestrictions=" + _siteRestrictions;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			// Drawing is relative to client area
			Size ourSize = this.ClientSize;

			Point[] light = new Point[2];
			Point[] dark = new Point[2];
			Point[] black = new Point[2];

			// Painting depends on orientation
			if ((this.Dock == DockStyle.Top) || 
				(this.Dock == DockStyle.Bottom))
			{
				// Draw as a horizontal bar
				dark[0].Y = dark[1].Y = ourSize.Height - 2;
				black[0].Y = black[1].Y = ourSize.Height - 1;
				light[1].X = dark[1].X = black[1].X = ourSize.Width;
			}
			else if ((this.Dock == DockStyle.Left) || 
					 (this.Dock == DockStyle.Right))
			{
				// Draw as a vertical bar
				dark[0].X = dark[1].X = ourSize.Width - 2;
				black[0].X = black[1].X = ourSize.Width - 1;
				light[1].Y = dark[1].Y = black[1].Y = ourSize.Height;
			}

			using (Pen penLightLight = new Pen(SystemColors.ControlLightLight),
				       penDark = new Pen(SystemColors.ControlDark),
				       penBlack = new Pen(Color.Black))
			{
				e.Graphics.DrawLine(penLightLight, light[0], light[1]);
				e.Graphics.DrawLine(penDark, dark[0], dark[1]);
				e.Graphics.DrawLine(penBlack, black[0], black[1]);
			}	
	
			// Let delegates fire through base
			base.OnPaint(e);
		}

	}
}
