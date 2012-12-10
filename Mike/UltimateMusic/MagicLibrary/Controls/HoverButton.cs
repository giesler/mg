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
	public class HoverButton : UserControl
	{
		// Instance variables
		protected int _index = 0;
		protected ImageList _images = null;
		protected bool _mouseOver = false;
		protected bool _mouseCapture = false;
		protected MouseButtons _mouseButton = MouseButtons.None;
		protected ImageAttributes _attributes = null;
		
		// Must use parameterised constructor
		private HoverButton() {}

		public HoverButton(ImageList images, int index, ImageAttributes attributes)
		{
			// We insist on having an image to show
			if (null == images)
				throw new ArgumentNullException("Must provide ImageList");

			// Optional parameter
			_attributes = attributes;

			// Set the initial image to show
			_images = images;
			_index = index;

			// Prevent drawing flicker by blitting from memory
			SetStyle(ControlStyles.DoubleBuffer, true);

			// Should not be allowed to select this control
			SetStyle(ControlStyles.Selectable, false);
		}

		public ImageAttributes ImageAttributes
		{
			get { return _attributes; }
			
			set
			{
				// No need to save and repaint if atrributes are unchanged
				if (value != _attributes)
				{
					_attributes = value;

					// Must repaint to show new image
					Refresh();
				}
			}
		}

		public int ImageIndex
		{
			get { return _index; }

			set
			{
				// No need to save and repaint if image is unchanged
				if (value != _index)
				{
					_index = value;

					// Must repaint to show new image
					Refresh();
				}
			}
		}

		public override string ToString()
		{
			return "HoverButton Enabled=" + this.Enabled + " Index=" + _index;
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!_mouseCapture)
			{
				// Mouse is over the button and being pressed, so change
				// state and remember the original button pressed as we only
				// care about the same button when it goes up
				_mouseOver = true;
				_mouseCapture = true;
				_mouseButton = e.Button;

				// Force redraw to show button status
				Refresh();
			}

			base.OnMouseUp(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			// Are we waiting for this button to go up?
			if (e.Button == _mouseButton)
			{
				// Set state back to become normal
				_mouseOver = false;
				_mouseCapture = false;

				// Force redraw to show button state
				Refresh();
			}
			else
			{
				// We don't want to lose capture of mouse
				Capture = true;
			}

			// Let delegates fire thru base
			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Is point inside our client rectangle
			bool over = this.ClientRectangle.Contains(new Point(e.X, e.Y));

			if (over != _mouseOver)
			{
				// Update state
				_mouseOver = over;

				// Force redraw to show button state
				Refresh();
			}

			// Let delegates fire thru base
			base.OnMouseMove(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			// Obviously are over the button at the moment
			_mouseOver = true;

			// Force redraw to show button state
			Refresh();

			// Let delegates fire thru base
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			// Obviously not over the button at the moment
			_mouseOver = false;

			// Force redraw to show button state
			Refresh();

			// Let delegates fire thru base
			base.OnMouseLeave(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// We expect the button images to fill entire area
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (!this.Enabled)
			{
				ControlPaint.DrawImageDisabled(e.Graphics, _images.Images[_index], 1, 1, this.BackColor);
			}
			else
			{
				Image image = _images.Images[_index];

				if (null == _attributes)
				{
					e.Graphics.DrawImage(_images.Images[_index], 
										 (_mouseOver &&  _mouseCapture ? new Point(2,2) : new Point(1,1)));
				}
				else
				{
					// Three points provided are upper-left, upper-right and 
					// lower-left of the destination parallelogram. 
					Point[] pts = new Point[3];
					pts[0].X = (_mouseOver && _mouseCapture) ? 2 : 1;
					pts[0].Y = (_mouseOver && _mouseCapture) ? 2 : 1;
					pts[1].X = pts[0].X + image.Width;
					pts[1].Y = pts[0].Y;
					pts[2].X = pts[0].X;
					pts[2].Y = pts[1].Y + image.Height;

					e.Graphics.DrawImage(_images.Images[_index],							// source image
										 pts,												// destination parallelogram 
										 new Rectangle(0, 0, image.Width, image.Height),	// source rectangle
										 GraphicsUnit.Pixel,								// units being used
										 _attributes);										// extra attributes to apply
				}

				ButtonBorderStyle bs = ButtonBorderStyle.Solid;

				if (_mouseOver && this.Enabled)
					bs = (_mouseCapture ? ButtonBorderStyle.Inset : ButtonBorderStyle.Outset);

				// Get a helper object to perform drawing for us!
				ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, this.BackColor, bs);
			}

			// Let delegates fire thru base
			base.OnPaint(e);
		}

	}
}