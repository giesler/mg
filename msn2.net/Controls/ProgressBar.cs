using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using msn2.net.Common;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for ProgressBar.
	/// </summary>
	public class ProgressBar : System.Windows.Forms.UserControl
	{

		#region Declares

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private int maximum					= 100;
		private int minimum					= 0;
		private int _value					= 0;
		private Cursor cursor				= Cursors.Hand;
		private Color progressBackColor		= Color.Maroon;
		private Color remainingBackColor	= Color.White;
		private bool showProgressValue		= true;
		private System.Windows.Forms.Panel panelContainer;
		private System.Windows.Forms.PictureBox pictureBoxRemaining;
		private System.Windows.Forms.PictureBox pictureBoxProgress;
		private bool showRemainingValue		= true;

		#endregion

		#region Constructor & Disposal

		public ProgressBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
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

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelContainer = new System.Windows.Forms.Panel();
			this.pictureBoxRemaining = new System.Windows.Forms.PictureBox();
			this.pictureBoxProgress = new System.Windows.Forms.PictureBox();
			this.panelContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelContainer
			// 
			this.panelContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelContainer.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.pictureBoxRemaining,
																						 this.pictureBoxProgress});
			this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelContainer.Name = "panelContainer";
			this.panelContainer.Size = new System.Drawing.Size(304, 40);
			this.panelContainer.TabIndex = 10;
			// 
			// pictureBoxRemaining
			// 
			this.pictureBoxRemaining.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.pictureBoxRemaining.BackColor = System.Drawing.Color.White;
			this.pictureBoxRemaining.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBoxRemaining.Location = new System.Drawing.Point(88, 0);
			this.pictureBoxRemaining.Name = "pictureBoxRemaining";
			this.pictureBoxRemaining.Size = new System.Drawing.Size(216, 36);
			this.pictureBoxRemaining.TabIndex = 11;
			this.pictureBoxRemaining.TabStop = false;
			this.pictureBoxRemaining.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxRemaining_Paint);
			this.pictureBoxRemaining.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxRemaining_MouseUp);
			// 
			// pictureBoxProgress
			// 
			this.pictureBoxProgress.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.pictureBoxProgress.BackColor = System.Drawing.Color.Maroon;
			this.pictureBoxProgress.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBoxProgress.Name = "pictureBoxProgress";
			this.pictureBoxProgress.Size = new System.Drawing.Size(176, 40);
			this.pictureBoxProgress.TabIndex = 10;
			this.pictureBoxProgress.TabStop = false;
			this.pictureBoxProgress.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxProgress_Paint);
			this.pictureBoxProgress.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxProgress_MouseUp);
			// 
			// ProgressBar
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelContainer});
			this.Name = "ProgressBar";
			this.Size = new System.Drawing.Size(304, 40);
			this.Resize += new System.EventHandler(this.ProgressBar_Resize);
			this.panelContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Display times

		private void pictureBoxRemaining_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (showRemainingValue)
			{
				string elapsedTime = Utilities.DurationToString(Convert.ToDouble((maximum - minimum) - _value));
				Font font = new Font("Arial", 6);
				using (StringFormat sf = new StringFormat())
				{
					sf.Alignment = StringAlignment.Far;
					e.Graphics.DrawString(elapsedTime, new Font("Arial", 6), new SolidBrush(Color.Black), new RectangleF(0, 0, pictureBoxRemaining.Width, pictureBoxRemaining.Height), sf);
				}
			}
		}

		private void pictureBoxProgress_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (showProgressValue)
			{
				string elapsedTime = Utilities.DurationToString(Convert.ToDouble(_value - minimum));
				e.Graphics.DrawString(elapsedTime, new Font("Arial", 6), new SolidBrush(Color.White), 0, 0);
			}
		}

		#endregion

		#region ChangeValueEvent

		public event ChangeValueDelegate ChangeValueEvent;

		private void pictureBoxProgress_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Get percentage complete, multiply by max val
			if (ChangeValueEvent != null)
			{
				int clickValue = Convert.ToInt32((
					Convert.ToDouble(e.X) / Convert.ToDouble(panelContainer.Width)) 
					* Convert.ToDouble(maximum - minimum));
				ChangeValueEvent(this, new ChangeValueEventArgs(clickValue));
			}
		}

		private void pictureBoxRemaining_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Call above function, offsetting for X value
			if (ChangeValueEvent != null)
			{
				int newX = e.X + pictureBoxProgress.Width;
				pictureBoxProgress_MouseUp(sender, new MouseEventArgs(e.Button, e.Clicks, newX, e.Y, e.Delta));
			}
		}

		#endregion

		#region Properties

		private bool displayValues = true;

		private void ProgressBar_Resize(object sender, System.EventArgs e)
		{
			this.Value = _value;		
		}

		[Category("Appearance")]
		public bool DisplayValues
		{
			get { return displayValues; }
			set { displayValues = value; }
		}

		[Category("Behavior")]
		public int Minimum
		{
			get { return minimum; }
			set { minimum = value; }
		}

		[Category("Behavior")]
		public int Maximum
		{
			get { return maximum; }
			set { maximum = value; }
		}

		[Category("Behavior")]
		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				pictureBoxProgress.Width	= Convert.ToInt32((Convert.ToDouble(value) / (maximum - minimum) * panelContainer.Width));
				pictureBoxRemaining.Left	= pictureBoxProgress.Left + pictureBoxProgress.Width;
				pictureBoxRemaining.Width	= panelContainer.Width - pictureBoxProgress.Width - 2;
				pictureBoxProgress.Refresh();
				pictureBoxRemaining.Refresh();
			}
		}

		[Category("Appearance")]
		public override Cursor Cursor
		{
			get
			{
				return cursor;
			}
			set
			{
				cursor							= value;
				this.pictureBoxProgress.Cursor	= value;
				this.pictureBoxRemaining.Cursor	= value;
			}
		}

		[Category("Appearance")]
		public Color ProgressBackColor
		{
			get 
			{
				return progressBackColor;
			}
			set
			{
				progressBackColor = value;
				this.pictureBoxProgress.BackColor = progressBackColor;
			}
		}

		[Category("Appearance")]
		public Color RemainingBackColor
		{
			get
			{
				return remainingBackColor;
			}
			set
			{
				remainingBackColor = value;
				this.pictureBoxRemaining.BackColor = remainingBackColor;
			}
		}

		[Category("Appearance")]
		public bool ShowProgressValue
		{
			get
			{
				return showProgressValue;
			}
			set
			{
				showProgressValue = value;
			}
		}

		[Category("Appearance")]
		public bool ShowRemainingValue
		{
			get
			{
				return showRemainingValue;
			}
			set
			{
				showRemainingValue = value;
			}
		}

//		[Category("Appearance")]
//		public override Color BackColor
//		{
//			get
//			{
//				return this.panelContainer.BackColor;
//			}
//			set
//			{
//				this.panelContainer.BackColor = value;
//			}
//		}

		[Category("Appearance")]
		public System.Windows.Forms.BorderStyle BorderStyle
		{
			get
			{
				return this.panelContainer.BorderStyle;
			}
			set
			{
				this.panelContainer.BorderStyle = value;
			}
		}

		#endregion

	}

	#region Supporting delegates, classes

	public delegate void ChangeValueDelegate(object sender, ChangeValueEventArgs e);

	[Serializable]
	public class ChangeValueEventArgs: EventArgs
	{
		public int _value;

		public ChangeValueEventArgs(int _value)
		{
			this._value = _value;
		}

		public int Value 
		{
			get { return _value; }
		}
	}

	#endregion

}
