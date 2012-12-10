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
using System.Collections;
using System.Windows.Forms;
using System.Resources;
using Crownwood.Magic.Controls;

namespace Crownwood.Magic.Docking
{
	public class DockingDetailHandlePlain : DockingDetailBase
	{
		// Class variables
		protected const int _inset = 3;
		protected const int _offset = 5;
		protected const int _insetButton = 2;
		protected const int _buttonImage = 10;
		protected const int _buttonWidth = 12;
		protected const int _fixedLength = 14;
		protected const int _closeIndex = 0;

		// Static variables
		protected static ImageList _images;

		// Instance variables
		protected int _buttonOffset = 0;
		protected bool _dockLeft = true;
		protected InertButton _close = null;
		protected Redocker _redock = null;

		// Instance events
		public event EventHandler CloseEvent;
		public event EventHandler ContextEvent;

		static DockingDetailHandlePlain()
		{
			// Create storage for our bitmaps
			_images = new ImageList();

			// Define the size of images we supply
			_images.ImageSize = new Size(_buttonImage, _buttonImage);

			// Get access to resources associated with this class
			ResourceManager rm = new ResourceManager(typeof(DockingDetailBase));

			// Load the menu bitmaps
			Bitmap pics = (Bitmap)rm.GetObject("Plain");

			Color backColor = pics.GetPixel(0, 0);
    
			// Make backColor transparent for Bitmap
			pics.MakeTransparent(backColor);
    
			// Load them all !
			_images.Images.AddStrip(pics);
		}

		public DockingDetailHandlePlain()
		{
			InternalConstruct(null, null);
		}

		public DockingDetailHandlePlain(EventHandler closeHandler)
		{
			InternalConstruct(closeHandler, null);
		}

		public DockingDetailHandlePlain(EventHandler closeHandler, EventHandler contextHandler)
		{
			InternalConstruct(closeHandler, contextHandler);
		}

		private void InternalConstruct(EventHandler closeHandler, EventHandler contextHandler)
		{
			// Our size is always fixed at the required length in both directions
			// as one of the sizes will be provided for us because of our docking
			this.Size = new Size(_fixedLength, _fixedLength);

			// Hook caller into our events
			if (null != closeHandler)
				this.CloseEvent += closeHandler;

			if (null != contextHandler)
				this.ContextEvent += contextHandler;

			// Need a button for closing
			_close = CreateHandleButton(_closeIndex, new EventHandler(OnCloseEvent));

			// Prevent drawing flicker by blitting from memory
			SetStyle(ControlStyles.DoubleBuffer, true);

			// Define category/type information
			this.Category = "Handle";
		}

		public override void OnStateChanged(State value)
		{ 
			switch(value)
			{
			case State.DockLeft:
				this.Show();
				_dockLeft = false;
				break;
			case State.DockTop:
				this.Show();
				_dockLeft = true;
				break;
			case State.DockRight:
				this.Show();
				_dockLeft = false;
				break;
			case State.DockBottom:
				this.Show();
				_dockLeft = true;
				break;
			case State.Floating:
				this.Hide();
				_dockLeft = true;
				break;
			}

			if (_dockLeft)
			{
				this.Dock = DockStyle.Left;

				int iStart = _inset;

				// Button position is fixed, regardless of our size
				_close.Location = new Point(_insetButton, iStart);
				_close.Anchor = AnchorStyles.Top;
			}
			else
			{
				this.Dock = DockStyle.Top;

				Size client = this.ClientSize;

				// Button is positioned to right hand side of bar
				_close.Location = new Point(client.Width - _inset - _buttonWidth, _insetButton);
				_close.Anchor = AnchorStyles.Right;
			}

			if (_dockingSite.DockingParent != null)
			{	
				// Can now create redocker object as we have a dockingsite
				_redock = new Redocker(_dockingSite, this, _dockingSite.DockingParent);

				// Define which docking control content this should effect
				_redock.Content = this.Content;
			}
			else
			{
				_redock = null;
			}
		}

		public override void OnCloseRequest()
		{
			// Generate event from ourself
			if (null != CloseEvent)
				CloseEvent(_dockingSite, new EventArgs());
		}

		public virtual void OnCloseEvent(Object sender, EventArgs e)
		{
			// Generate event from ourself
			if (null != CloseEvent)
				CloseEvent(_dockingSite, e);
		}

		public override IDockingSite DockingSite
		{
			set
			{
				// We must have an instance to pass into the redocker
				if (null == value)
					throw new ArgumentNullException("Must provide non-null reference");

				// Must call the base class
				base.DockingSite = value;
			}
		}

		public override string ToString()
		{
			return "DockingDetailHandlePlain DockLeft(" + _dockLeft + ")";
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Check for context menu request
			if (e.Button == MouseButtons.Right)
			{
				// Generate context menu event
				if (null != ContextEvent)
				{
					// Get screen coordinates of the mouse
					Point pt = this.PointToScreen(new Point(e.X, e.Y));
					
					// Box to transfer as parameter
					ContextEvent(pt, new EventArgs());
				}
			}
			else
			{
				CheckRedocker();

				// Use redocking helper object
				_redock.OnMouseDown(e);
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			CheckRedocker();

			// Use redocking helper object
			_redock.OnMouseMove(e);

			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			CheckRedocker();

			// Use redocking helper object
			_redock.OnMouseUp(e);

			base.OnMouseUp(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Size ourSize = this.ClientSize;
			Point[] light = new Point[4];
			Point[] dark = new Point[4];
				
			// Depends on orientation
			if (_dockLeft)
			{
				int iBottom = ourSize.Height - _inset - 1;
				int iRight = _offset + 2;
				int iTop = _inset + _buttonOffset;

				light[3].X = light[2].X = light[0].X = _offset;
				light[2].Y = light[1].Y = light[0].Y = iTop;
				light[1].X = _offset + 1;
				light[3].Y = iBottom;
			
				dark[2].X = dark[1].X = dark[0].X = iRight;
				dark[3].Y = dark[2].Y = dark[1].Y = iBottom;
				dark[0].Y = iTop;
				dark[3].X = iRight - 1;
			}
			else
			{
				int iBottom = _offset + 2;
				int iRight = ourSize.Width - _inset - _buttonOffset;
				
				light[3].X = light[2].X = light[0].X = _inset;
				light[1].Y = light[2].Y = light[0].Y = _offset;
				light[1].X = iRight;
				light[3].Y = _offset + 1;
			
				dark[2].X = dark[1].X = dark[0].X = iRight;
				dark[3].Y = dark[2].Y = dark[1].Y = iBottom;
				dark[0].Y = _offset;
				dark[3].X = _inset;
			}

			using (Pen lightPen = new Pen(SystemColors.ControlLightLight),
					   darkPen = new Pen(SystemColors.ControlDark))
			{
				e.Graphics.DrawLine(lightPen, light[0], light[1]);
				e.Graphics.DrawLine(lightPen, light[2], light[3]);
				e.Graphics.DrawLine(darkPen, dark[0], dark[1]);
				e.Graphics.DrawLine(darkPen, dark[2], dark[3]);

				// Shift coordinates to draw section grab bar
				if (_dockLeft)
				{
					for(int i=0; i<4; i++)
					{
						light[i].X += 4;
						dark[i].X += 4;
					}
				}
				else
				{
					for(int i=0; i<4; i++)
					{
						light[i].Y += 4;
						dark[i].Y += 4;
					}
				}

				e.Graphics.DrawLine(lightPen, light[0], light[1]);
				e.Graphics.DrawLine(lightPen, light[2], light[3]);
				e.Graphics.DrawLine(darkPen, dark[0], dark[1]);
				e.Graphics.DrawLine(darkPen, dark[2], dark[3]);
			}

			base.OnPaint(e);
		}

		protected InertButton CreateHandleButton(int imageIndex, EventHandler clickEvent)
		{
			InertButton button = new InertButton();

			// Define the imagelist and which image to show initially
			button.ImageList = _images;
			button.ImageIndex = imageIndex;

			// Size is the same for all handle buttons
			button.Size = new Size(_buttonWidth, _buttonWidth);

			// Attach events to button
			button.Click += clickEvent;
			button.GotFocus += new EventHandler(OnButtonGotFocus);

			// Reduce painting of lines for button position
			_buttonOffset += _buttonWidth + _insetButton;

			Controls.Add(button);

			return button;
		}

		protected void CheckRedocker()
		{
			if (null == _redock)
			{
				// Can now create redocker object as we have a dockingsite
				_redock = new Redocker(_dockingSite, this, _dockingSite.DockingParent);

				// Define which docking control content this should effect
				_redock.Content = this.Content;
			}
		}

		protected virtual void OnButtonGotFocus(Object sender, EventArgs e)
		{
			// Inform docking site we have the focus
			if (null != _dockingNotify)
				_dockingNotify.NotifyDetailGotFocus(this);
		}

	}

	public class DockingDetailHandleIDE : DockingDetailBase
	{
		// Class constants
		protected const int _yInset = 3;
		protected const int _imageWidth = 12;
		protected const int _imageHeight = 11;
		protected const int _buttonWidth = 14;
		protected const int _buttonHeight = 13;
		protected const int _buttonSpacer = 3;
		protected const int _closeIndexActive = 0;
		protected const int _closeIndexInactive = 1;

		protected static ColorMap activeMap = new ColorMap();
		protected static ColorMap inactiveMap = new ColorMap();
		protected static ImageAttributes _closeActiveAttr = new ImageAttributes();
		protected static ImageAttributes _closeInactiveAttr = new ImageAttributes();

		// Static variables
		protected static ImageList _images;

		// Instance variables
		protected int _fixedLength;
		protected Redocker _redock = null;
		protected HoverButton _close = null;
	
		// Instance events
		public event EventHandler CloseEvent;
		public event EventHandler ContextEvent;

		static DockingDetailHandleIDE()
		{
			// Create storage for our bitmaps
			_images = new ImageList();

			// Define the size of images we supply
			_images.ImageSize = new Size(_imageWidth, _imageHeight);

			// Get access to resources associated with this class
			ResourceManager rm = new ResourceManager(typeof(DockingDetailBase));

			// Load the menu bitmaps
			Bitmap pics = (Bitmap)rm.GetObject("IDE");

			// Load them all !
			_images.Images.AddStrip(pics);

			// Define use of current system colours
			activeMap.OldColor = Color.Black;
			activeMap.NewColor = SystemColors.ActiveCaption;
			inactiveMap.OldColor = Color.White;
			inactiveMap.NewColor = SystemColors.Control;

			// Create remap attributes for use by button
			_closeActiveAttr.SetRemapTable(new ColorMap[]{activeMap}, ColorAdjustType.Bitmap);
			_closeInactiveAttr.SetRemapTable(new ColorMap[]{inactiveMap}, ColorAdjustType.Bitmap);
		}

		public DockingDetailHandleIDE() 
		{
			InternalConstruct(null, null);
		}

		public DockingDetailHandleIDE(EventHandler closeHandler)
		{
			InternalConstruct(closeHandler, null);
		}

		public DockingDetailHandleIDE(EventHandler closeHandler, EventHandler contextHandler)
		{
			InternalConstruct(closeHandler, contextHandler);
		}

		private void InternalConstruct(EventHandler closeHandler, EventHandler contextHandler)
		{
			_fixedLength = SystemInformation.CaptionHeight + _yInset * 2;
				
			// Our size is always fixed at the required length in both directions
			// as one of the sizes will be provided for us because of our docking
			this.Size = new Size(_fixedLength, _fixedLength);

			// Default the caption text
			this.Text = "Default";

			_close = new HoverButton(_images, _closeIndexInactive, _closeInactiveAttr);
			_close.Size = new Size(_buttonWidth, _buttonHeight);
			_close.Anchor = AnchorStyles.Right;
			_close.Location = new Point(_fixedLength - _buttonWidth - _buttonSpacer, 
										(_fixedLength - _yInset * 2 - _buttonHeight) / 2 + _yInset);

			// Attach to button event
			_close.Click += new EventHandler(OnCloseEvent);
						
			// If we have an event handler, add button for it
			if (null != closeHandler)
				this.CloseEvent += closeHandler;

			if (null != contextHandler)
				this.ContextEvent += contextHandler;

			this.Controls.Add(_close);

			// Prevent drawing flicker by blitting from memory
			this.SetStyle(ControlStyles.DoubleBuffer, true);

			// Define category/type information
			this.Category = "Title";
		}

		public override IDockingSite DockingSite
		{
			set 
			{ 
				// We must have an instance to pass into the redocker
				if (null == value)
					throw new ArgumentNullException("Must provide instance");

				// We want to ensure the minimum size is ours
				value.DockingMinimumSize = new Size(value.DockingMinimumSize.Width, 
													value.DockingMinimumSize.Height + _fixedLength);


				// Grab the title text from parent and use it ourself
				this.Text = value.Base.Text;

				// Call base class
				base.DockingSite = value;
			}
		}

		public override void OnStateChanged(State value)
		{ 
			if (value == State.Floating)
			{
				// No need for display of caption
				this.Hide();
				this.Invalidate();
			}
			else
			{
				this.Show();
				this.Dock = DockStyle.Top;
				this.Invalidate();
			}

			if (_dockingSite.DockingParent != null)
			{	
				// Can now create redocker object as we have a dockingsite
				_redock = new Redocker(_dockingSite, this, _dockingSite.DockingParent);

				// Define which docking control content this should effect
				_redock.Content = this.Content;
			}
			else
			{
				_redock = null;
			}	
		}

		public override void OnDockingGotFocus()
		{
			SetButtonState();

			this.Invalidate();
		}

		public override void OnDockingLostFocus()
		{
			SetButtonState();

			this.Invalidate();
		}
		
		public override void OnTextChanged(string title)
		{
			// Use the title given to the containing control
			this.Text = title;
		}

		public override void OnCloseRequest()
		{
			// Generate event from ourself
			if (null != CloseEvent)
				CloseEvent(_dockingSite, new EventArgs());
		}

		public virtual void OnCloseEvent(Object sender, EventArgs e)
		{
			// Generate event from ourself
			if (null != CloseEvent)
				CloseEvent(_dockingSite, e);
		}

		public override string ToString()
		{
			return "DockingDetailHandleIDE";
		}
		protected void SetButtonState()
		{
			SetButtonState(_dockingSite.DetailHasFocus(this));
		}

		protected void SetButtonState(bool focused)
		{
			if (focused)
			{
				if (_close.BackColor != SystemColors.ActiveCaption)
				{
					_close.BackColor = SystemColors.ActiveCaption;
					_close.ImageIndex = _closeIndexActive;
					_close.ImageAttributes = _closeActiveAttr;
					_close.Invalidate();
				}
			}
			else
			{
				if (_close.BackColor != SystemColors.Control)
				{
					_close.BackColor = SystemColors.Control;
					_close.ImageIndex = _closeIndexInactive;
					_close.ImageAttributes = _closeInactiveAttr;
					_close.Invalidate();
				}
			}
		}

		protected override void OnLostFocus(EventArgs e)
		{
			// If the docking site does not have any contents then we
			// will not get informed when the docking site loses focus
			// and so do the necessary work now.
			if (_dockingSite.Count == 0)
				OnDockingLostFocus();

			// Call base class implementation
			base.OnGotFocus(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// Overriden to prevent background being painted
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			bool focused = _dockingSite.DetailHasFocus(this);

			SetButtonState(focused);

			Size ourSize = this.ClientSize;

			int xEnd = ourSize.Width;
			int yEnd = ourSize.Height - _yInset * 2;

			Rectangle rectCaption = new Rectangle(0, _yInset, xEnd, yEnd - _yInset + 1);

			// Is this control Active?
			if (focused)
			{
				// Fill the entire background area
				e.Graphics.FillRectangle(SystemBrushes.Control, e.ClipRectangle);
	
				// Use a solid filled background for text
				e.Graphics.FillRectangle(SystemBrushes.ActiveCaption, rectCaption);
			
				// Start drawing text a little from the left
				rectCaption.X += _buttonSpacer;
				rectCaption.Y += 1;
				rectCaption.Height -= 2;

				e.Graphics.DrawString(this.Text, 
									  SystemInformation.MenuFont, 
									  SystemBrushes.ActiveCaptionText,
									  rectCaption);
			}
			else
			{
				// Fill the entire background area
				e.Graphics.FillRectangle(SystemBrushes.Control, e.ClipRectangle);
	
				// Inactive and so use a rounded rectangle
				using (Pen dark = new Pen(SystemColors.ControlDark))
				{
					e.Graphics.DrawLine(dark, 1, _yInset, xEnd - 2, _yInset);
					e.Graphics.DrawLine(dark, 1, yEnd, xEnd - 2, yEnd);
					e.Graphics.DrawLine(dark, 0, _yInset + 1, 0, yEnd - 1);
					e.Graphics.DrawLine(dark, xEnd - 1, _yInset + 1, xEnd - 1, yEnd - 1);

					// Start drawing text a little from the left
					rectCaption.X += _buttonSpacer;
					rectCaption.Y += 1;
					rectCaption.Height -= 2;

					e.Graphics.DrawString(this.Text, 
										  SystemInformation.MenuFont, 
										  SystemBrushes.ControlText,
										  rectCaption);
				}
			}	

			// Let delegates fire through base
			base.OnPaint(e);

			// Always get the button to repaint as we have painted over their area
			_close.Refresh();
		}				

		protected override void OnResize(EventArgs e)
		{
			// Any resize of control should redraw all of it otherwise when you 
			// stretch to the right it will not paint correctly as we draw a inset
			// border which is not honoured 
			this.Invalidate();

			base.OnResize(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Check for context menu request
			if (e.Button == MouseButtons.Right)
			{
				// Generate context menu event
				if (null != ContextEvent)
				{
					// Get screen coordinates of the mouse
					Point pt = this.PointToScreen(new Point(e.X, e.Y));
					
					// Box to transfer as parameter
					ContextEvent(pt, new EventArgs());
				}
			}
			else
			{
				CheckRedocker();

				// Use redocking helper object
				_redock.OnMouseDown(e);
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			CheckRedocker();

			// Use redocking helper object
			_redock.OnMouseMove(e);

			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			CheckRedocker();

			// Use redocking helper object
			_redock.OnMouseUp(e);

			base.OnMouseUp(e);
		}

		protected void CheckRedocker()
		{
			if (null == _redock)
			{
				// Can now create redocker object as we have a dockingsite
				_redock = new Redocker(_dockingSite, this, _dockingSite.DockingParent);

				// Define which docking control content this should effect
				_redock.Content = this.Content;
			}
		}

	}
}