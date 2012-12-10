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
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Docking;

namespace Crownwood.Magic.Docking
{
	public class ExternalForm : Form
	{
		// Class constants
		private const int WM_NCLBUTTONDBLCLK = 0x00A3;

		// Instance variables
		protected State _oldState;
		protected DockingBase _dockingBase;
		protected bool _fireCloseEvent = true;
		protected Container components = null;

		// Prevent parameterless instantiation
		private ExternalForm() {}

		public ExternalForm(DockingBase dockingBase)
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Check for a valid parameter
			if (null == dockingBase)
				throw new ArgumentNullException("Must provide DockingBase");

			// The caller is responsible for setting our initial screen location
			this.StartPosition = FormStartPosition.Manual;

			// Not in task bar to prevent clutter
			this.ShowInTaskbar = false;

			// Copy any title from the base control
			this.Text = dockingBase.Text;

			// Save details needed to restore control
			_oldState = dockingBase.State;
			_dockingBase = dockingBase;

			// Must fill the entire area
			_dockingBase.Dock = DockStyle.Fill;

			// Add it as only contents, adding here will cause the window to be
			// moved from its old parent window to this one.  So no explicit action
			// required to remove from the old parent.
			Controls.Add(_dockingBase);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "ExternalForm";
		}
		#endregion

		public DockingBase Content
		{
			get { return _dockingBase; }
		}

		public void Restore(bool bState)
		{
			if (null != _dockingBase.DockingParent)
			{
				// Check we have permission to change back to the old state
				if (_dockingBase.CheckPermissionForState(_oldState))
				{
					// Hide before changing docking style, so user does
					// not see it changing position visually
					_dockingBase.Hide();

					// Don't layout controls till we are ready, cause we might want to hide this
					// control once its added to the old parent.  Would like to prevent flicker
					_dockingBase.DockingParent.SuspendLayout();

					// Move parenting of docking control back to previous parent
					_dockingBase.DockingParent.Controls.Add(_dockingBase);

					// Reposition as first docking window to ensure it is sized/positioned last
					_dockingBase.RepositionAtStart();				

					if (bState)
					{
						// Reset old docking style
						_dockingBase.State = _oldState;
					}

					if (!_dockingBase.Hidden)
						_dockingBase.Show();

					// State now ready for display
					_dockingBase.DockingParent.ResumeLayout();

					// Redocking and not closing, so no need to fire close event
					_fireCloseEvent = false;

					// Commit suicide
					Close();
				}
			}
		}

		public override string ToString()
		{
			return "ExternalForm OldState=" + _oldState + " DockingBase=" + _dockingBase.ToString();
		}
		protected override void OnClosing(CancelEventArgs e)
		{
			if (_fireCloseEvent)
			{
				// Cancel the closing of the window
				e.Cancel = true;

				// Inform anyone interested of close event
				_dockingBase.NotifyCloseRequest(null);
			}

			base.OnClosing(e);
		}

		protected override void WndProc(ref Message m)
		{
			// Want to notice when the window is maximized
			if (m.Msg == WM_NCLBUTTONDBLCLK)
			{
				// Redock and kill ourself
				Restore(true);

				// We do not want to let the base process the message as the 
				// restore might fail due to lack of permission to restore to 
				// old state.  In that case we do not want to maximize the window
			}
			else
			{
				base.WndProc(ref m);
			}
		}

	}
}
