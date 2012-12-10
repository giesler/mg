using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using msn2.net.Configuration;
using System.Diagnostics;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for ShellForm.
	/// </summary>
	public class ShellForm : System.Windows.Forms.Form
	{

		#region Static instance functionality

		private static ArrayList instances	= new ArrayList();

		private static void AddInstance(ShellForm instance)
		{
			lock (instances)
			{
				instances.Add(instance);
				if (ShellForm_Added != null)
					ShellForm_Added(null, new ShellFormAddedEventArgs(instance));
			}
		}

		private static void AddInstance(ShellForm instance, ShellForm parent)
		{
			lock (instances)
			{
				instances.Add(instance);
				if (ShellForm_Added != null)
					ShellForm_Added(null, new ShellFormAddedEventArgs(instance, parent));
			}
		}

		private static void RemoveInstance(ShellForm instance)
		{
			lock (instances)
			{
				if (instances.Contains(instance))
				{
					instances.Remove(instance);
					if (ShellForm_Removed != null)
						ShellForm_Removed(null, new ShellFormRemovedEventArgs(instance));
				}
				
			}
		}

		
		#region Static Events

		public static event ShellForm_AddedDelegate ShellForm_Added;
		public static event ShellForm_RemovedDelegate ShellForm_Removed;

		#endregion

		#endregion

		#region Declares

		// WinForms declares
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel panelTitle;
		protected System.Timers.Timer timerFadeOut;
		private System.Windows.Forms.Panel panelLeft;
		private System.Windows.Forms.Panel panelRight;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.Panel panelSizeNWSE;
		protected System.Timers.Timer timerFadeIn;
		private FormBorderStyle actualFormBorderStyle;
		private System.Windows.Forms.ImageList imageListTitleBar;
		private System.Windows.Forms.Panel panelTitleText;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Panel panelButtons;
		private System.Windows.Forms.Button buttonOpacity;
		private System.Windows.Forms.Button buttonRollup;
		private System.Windows.Forms.Button buttonOnTop;
		private System.Windows.Forms.Button buttonHide;
		private System.Windows.Forms.Panel panelMoreButtons;

		// Property declares
		private Size	fixedSize					= new Size(0, 0);
		private bool	allowUnload					= true;
		private bool	enableOpacityChanges		= true;
		private Size	savedSize					= new Size(0, 0);
		private bool	rolledUp					= false;
		protected ShellForm lockedTo				= null;
		protected ArrayList lockedForms				= new ArrayList();
		private Data formNode						= null;
		private Data layoutData						= null;

		#endregion

		#region Constructor and Disposal

		public ShellForm()
		{
			//
			// Required for Windows Form Designer support
			//
			if (!DesignMode)
				InitializeComponent();

			timerFadeOut = new System.Timers.Timer(75);
			timerFadeOut.Elapsed += new System.Timers.ElapsedEventHandler(FadeOut_Elapsed);

			timerFadeIn = new System.Timers.Timer(65);
			timerFadeIn.Elapsed += new System.Timers.ElapsedEventHandler(FadeIn_Elapsed);

			panelMoreButtons.Width = 0;

			AddInstance(this);
		}

		public ShellForm(Data formNode): this()
		{
			this.formNode	= formNode;
			this.Text		= formNode.Name;
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
			
			RemoveInstance(this);
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ShellForm));
			this.panelTitle = new System.Windows.Forms.Panel();
			this.panelTitleText = new System.Windows.Forms.Panel();
			this.labelTitle = new System.Windows.Forms.Label();
			this.panelMoreButtons = new System.Windows.Forms.Panel();
			this.panelButtons = new System.Windows.Forms.Panel();
			this.buttonOpacity = new System.Windows.Forms.Button();
			this.buttonRollup = new System.Windows.Forms.Button();
			this.buttonOnTop = new System.Windows.Forms.Button();
			this.buttonHide = new System.Windows.Forms.Button();
			this.panelLeft = new System.Windows.Forms.Panel();
			this.panelRight = new System.Windows.Forms.Panel();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.panelSizeNWSE = new System.Windows.Forms.Panel();
			this.imageListTitleBar = new System.Windows.Forms.ImageList(this.components);
			this.panelTitle.SuspendLayout();
			this.panelTitleText.SuspendLayout();
			this.panelButtons.SuspendLayout();
			this.panelBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelTitle
			// 
			this.panelTitle.BackColor = System.Drawing.Color.DimGray;
			this.panelTitle.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.panelTitleText,
																					 this.panelMoreButtons,
																					 this.panelButtons});
			this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTitle.Name = "panelTitle";
			this.panelTitle.Size = new System.Drawing.Size(344, 0);
			this.panelTitle.TabIndex = 0;
			this.panelTitle.Layout += new System.Windows.Forms.LayoutEventHandler(this.panelTitle_Layout);
			// 
			// panelTitleText
			// 
			this.panelTitleText.BackColor = System.Drawing.Color.Transparent;
			this.panelTitleText.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.labelTitle});
			this.panelTitleText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTitleText.Name = "panelTitleText";
			this.panelTitleText.Size = new System.Drawing.Size(224, 0);
			this.panelTitleText.TabIndex = 5;
			this.panelTitleText.Resize += new System.EventHandler(this.panelTitleText_Resize);
			// 
			// labelTitle
			// 
			this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.labelTitle.BackColor = System.Drawing.Color.Transparent;
			this.labelTitle.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelTitle.ForeColor = System.Drawing.Color.LawnGreen;
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(224, 23);
			this.labelTitle.TabIndex = 1;
			this.labelTitle.Text = "[title]";
			this.labelTitle.MouseHover += new System.EventHandler(this.labelTitle_MouseHover);
			this.labelTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseUp);
			this.labelTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseMove);
			this.labelTitle.MouseLeave += new System.EventHandler(this.labelTitle_MouseLeave);
			this.labelTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseDown);
			// 
			// panelMoreButtons
			// 
			this.panelMoreButtons.BackColor = System.Drawing.Color.Transparent;
			this.panelMoreButtons.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelMoreButtons.Location = new System.Drawing.Point(224, 0);
			this.panelMoreButtons.Name = "panelMoreButtons";
			this.panelMoreButtons.Size = new System.Drawing.Size(24, 0);
			this.panelMoreButtons.TabIndex = 7;
			// 
			// panelButtons
			// 
			this.panelButtons.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this.buttonOpacity,
																					   this.buttonRollup,
																					   this.buttonOnTop,
																					   this.buttonHide});
			this.panelButtons.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelButtons.Location = new System.Drawing.Point(248, 0);
			this.panelButtons.Name = "panelButtons";
			this.panelButtons.Size = new System.Drawing.Size(96, 0);
			this.panelButtons.TabIndex = 6;
			// 
			// buttonOpacity
			// 
			this.buttonOpacity.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOpacity.BackColor = System.Drawing.Color.Transparent;
			this.buttonOpacity.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOpacity.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonOpacity.ForeColor = System.Drawing.Color.LawnGreen;
			this.buttonOpacity.Location = new System.Drawing.Point(4, 0);
			this.buttonOpacity.Name = "buttonOpacity";
			this.buttonOpacity.Size = new System.Drawing.Size(24, 16);
			this.buttonOpacity.TabIndex = 8;
			this.buttonOpacity.Click += new System.EventHandler(this.buttonOpacity_Click);
			this.buttonOpacity.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonOpacity_Paint);
			// 
			// buttonRollup
			// 
			this.buttonRollup.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonRollup.BackColor = System.Drawing.Color.Transparent;
			this.buttonRollup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonRollup.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonRollup.ForeColor = System.Drawing.Color.LawnGreen;
			this.buttonRollup.Location = new System.Drawing.Point(52, 0);
			this.buttonRollup.Name = "buttonRollup";
			this.buttonRollup.Size = new System.Drawing.Size(24, 16);
			this.buttonRollup.TabIndex = 7;
			this.buttonRollup.Click += new System.EventHandler(this.buttonRollup_Click);
			this.buttonRollup.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonRollup_Paint);
			// 
			// buttonOnTop
			// 
			this.buttonOnTop.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOnTop.BackColor = System.Drawing.Color.Transparent;
			this.buttonOnTop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOnTop.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonOnTop.ForeColor = System.Drawing.Color.LawnGreen;
			this.buttonOnTop.Location = new System.Drawing.Point(28, 0);
			this.buttonOnTop.Name = "buttonOnTop";
			this.buttonOnTop.Size = new System.Drawing.Size(24, 16);
			this.buttonOnTop.TabIndex = 6;
			this.buttonOnTop.Click += new System.EventHandler(this.buttonOnTop_Click);
			this.buttonOnTop.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonOnTop_Paint);
			// 
			// buttonHide
			// 
			this.buttonHide.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonHide.BackColor = System.Drawing.Color.Transparent;
			this.buttonHide.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonHide.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonHide.ForeColor = System.Drawing.Color.LawnGreen;
			this.buttonHide.Location = new System.Drawing.Point(76, 0);
			this.buttonHide.Name = "buttonHide";
			this.buttonHide.Size = new System.Drawing.Size(24, 16);
			this.buttonHide.TabIndex = 5;
			this.buttonHide.Text = "X";
			this.buttonHide.Click += new System.EventHandler(this.button1_Click);
			// 
			// panelLeft
			// 
			this.panelLeft.BackColor = System.Drawing.Color.DimGray;
			this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelLeft.Name = "panelLeft";
			this.panelLeft.Size = new System.Drawing.Size(0, 262);
			this.panelLeft.TabIndex = 2;
			this.panelLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelLeft_MouseUp);
			this.panelLeft.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelLeft_MouseMove);
			this.panelLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelLeft_MouseDown);
			// 
			// panelRight
			// 
			this.panelRight.BackColor = System.Drawing.Color.DimGray;
			this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelRight.Location = new System.Drawing.Point(344, 0);
			this.panelRight.Name = "panelRight";
			this.panelRight.Size = new System.Drawing.Size(0, 262);
			this.panelRight.TabIndex = 3;
			this.panelRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelRight_MouseUp);
			this.panelRight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelRight_MouseMove);
			this.panelRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelRight_MouseDown);
			// 
			// panelBottom
			// 
			this.panelBottom.BackColor = System.Drawing.Color.DimGray;
			this.panelBottom.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.panelSizeNWSE});
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelBottom.Location = new System.Drawing.Point(0, 262);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(344, 0);
			this.panelBottom.TabIndex = 4;
			this.panelBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelBottom_MouseUp);
			this.panelBottom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelBottom_MouseMove);
			this.panelBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelBottom_MouseDown);
			// 
			// panelSizeNWSE
			// 
			this.panelSizeNWSE.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.panelSizeNWSE.Location = new System.Drawing.Point(328, -40);
			this.panelSizeNWSE.Name = "panelSizeNWSE";
			this.panelSizeNWSE.Size = new System.Drawing.Size(16, 40);
			this.panelSizeNWSE.TabIndex = 0;
			this.panelSizeNWSE.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelSizeNWSE_MouseUp);
			this.panelSizeNWSE.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelSizeNWSE_MouseMove);
			this.panelSizeNWSE.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelSizeNWSE_MouseDown);
			// 
			// imageListTitleBar
			// 
			this.imageListTitleBar.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageListTitleBar.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListTitleBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTitleBar.ImageStream")));
			this.imageListTitleBar.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ShellForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(344, 262);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelBottom,
																		  this.panelRight,
																		  this.panelLeft,
																		  this.panelTitle});
			this.Name = "ShellForm";
			this.Opacity = 0.51999998092651367;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ShellForm_Closing);
			this.Load += new System.EventHandler(this.ShellForm_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ShellForm_Layout);
			this.Activated += new System.EventHandler(this.ShellForm_Activated);
			this.Deactivate += new System.EventHandler(this.ShellForm_Deactivate);
			this.panelTitle.ResumeLayout(false);
			this.panelTitleText.ResumeLayout(false);
			this.panelButtons.ResumeLayout(false);
			this.panelBottom.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Load

		private void ShellForm_Load(object sender, System.EventArgs e)
		{
			if (formNode != null)
			{
				ShellFormConfigData layoutConfig = new ShellFormConfigData(this);
				layoutData = formNode.Get("ShellFormConfigData", layoutConfig, ConfigTreeLocation.CustomConfigTree);
				layoutConfig.Apply();
			}                
		}

		#endregion

		#region Layout

		private bool savedBorderStyle = false;

		private void ShellForm_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(this.Name + ": " + this.GetHashCode().ToString());

			// Add mouseover cursors if not in a fixed style
			if (!DesignMode && this.Parent == null)
			{
				if (!savedBorderStyle)
				{
					System.Diagnostics.Debug.WriteLine("Display: " + this.FormBorderStyle.ToString());
					actualFormBorderStyle = this.FormBorderStyle;
					this.FormBorderStyle = FormBorderStyle.None;
					savedBorderStyle = true;

					LayoutForProjectF();
				}

				// If window is sizable, we want to show the cursors
				if (actualFormBorderStyle == FormBorderStyle.SizableToolWindow
					|| actualFormBorderStyle == FormBorderStyle.Sizable)
				{
					this.panelLeft.Cursor		= System.Windows.Forms.Cursors.SizeWE;
					this.panelRight.Cursor		= System.Windows.Forms.Cursors.SizeWE;
					this.panelBottom.Cursor		= System.Windows.Forms.Cursors.SizeNS;
					this.panelSizeNWSE.Cursor	= System.Windows.Forms.Cursors.SizeNWSE;
				}
			}		
		}

		#endregion

		#region Properties

		[Category("Layout")]
		public Size FixedSize
		{
			get { return fixedSize; }
			set { fixedSize = value; }
		}

		public bool AllowUnload
		{
			get { return allowUnload; }
			set { allowUnload = value; }
		}

		[Category("Appearance")]
		public override string Text
		{
			get 
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				labelTitle.Text = value;
			}
		}

		[Category("Appearance")]
		public bool EnableOpacityChanges
		{
			get 
			{
				return enableOpacityChanges;
			}
			set 
			{
				enableOpacityChanges = value;
				if (!value)
					Opacity = 1;
			}
		}

		[Category("Appearance")]
		public bool RolledUp
		{
			get
			{
				return rolledUp;
			}
			set
			{
				rolledUp = !value;
				buttonRollup_Click(this, EventArgs.Empty);
			}
		}

		public new Size Size
		{
			get
			{
				return savedSize;
			}
			set
			{
				savedSize = value;
				if (rolledUp)
				{
					base.Size = new Size(value.Width, panelTitle.Height);
				}
				else
				{
					base.Size = value;
				}
			}
		}

		public bool TitleVisible
		{
			get
			{
				return this.labelTitle.Visible;
			}
			set
			{
				this.labelTitle.Visible = value;
			}
		}

		public Data Data
		{
			get { return formNode; }
			set { formNode = value; }
		}

		public Point DefaultLocation
		{
			get { return this.Location; }
		}

		#endregion

		#region Form Moving code

		private bool moving = false;
		private int startX, startY;

		public bool Moving
		{
			get { return moving; }
		}

		private void labelTitle_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			startX = e.X;
			startY = e.Y;
			moving = true;		
		}

		private void labelTitle_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (moving) 
			{
				// Calculate potential new position
				int newLeft		= this.Left + e.X - startX;
				int newTop		= this.Top + e.Y - startY;

				PerformLockingCheck(newLeft, newTop, true);

				// Move any forms locked to this
				foreach (ShellForm instance in instances)
				{
					// Reposition any forms locked to this form
					if (instance.lockedTo == this)
					{
						instance.Left = instance.Left + e.X - startX;
						instance.Top  = instance.Top  + e.Y - startY;
					}
				}
			}
		}

		private void PerformLockingCheck()
		{
			PerformLockingCheck(this.Left, this.Top, false);
		}

		private void PerformLockingCheck(int newLeft, int newTop, bool moveForm)
		{
			bool moved = !moveForm;

			// Loop through instances of ShellForm to see if we are within locking range of any
			foreach (ShellForm instance in instances)
			{
				// if instance is this, ignore
				if (instance == this)
					continue;

				// See if we are within 10 of another instance's lower left
				if ( (Math.Abs(instance.Left - newLeft) < 10)
					&& (Math.Abs(instance.Top + instance.Height - newTop) < 10) )
				{

					// we don't want to move it unless we need to 'snap'
					moved = true;	
						
					// Since we are within 10, we want to lock it - unless already locked
					if (lockedTo == null)
					{
						lockedTo = instance;
						instance.timerFadeIn.Enabled = true;
						instance.timerFadeOut.Enabled = false;

						this.Left = instance.Left;
						this.Top  = instance.Top + instance.Height;

						// Snap widths if close
						if (Math.Abs(this.Width - instance.Width) < 10)
						{
							int newWidth = Math.Max(this.Width, instance.Width);
							this.Width		= newWidth;
							instance.Width	= newWidth;
						}

						Debug.WriteLine(this.Name + " locked to " + instance.Name);
						break;
					}
						// See if we are within 10 of another instances upper left
					else if ( (Math.Abs(instance.Left - newLeft) < 10)
						&& (Math.Abs(newTop + this.Height - instance.Top) < 10) )
					{
						Debug.WriteLine(this.Name + " is within 10 of " + instance.Name);
					}

				}
					// we are NOT within 10, so check if we should un-snap
				else
				{
					// only unlock if we are away from current loop form
					if (lockedTo != null && lockedTo == instance)
					{
						Debug.WriteLine(this.Name + " unlocked");
						lockedTo.timerFadeIn.Enabled = false;
						lockedTo.timerFadeOut.Enabled = true;
						lockedTo = null;
					}
				}

			}

			// If we haven't moved the form yet, we need to move it
			if (!moved)
			{
				// Move mouse by amount in event args
				this.Left = newLeft;
				this.Top  = newTop;
			}
		}

		private void labelTitle_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			moving = false;
		}

		#endregion

		#region Close

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ShellForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!allowUnload)
			{
				e.Cancel = true;
				this.Visible = false;
			}
			else
			{
				if (formNode != null)
				{
					ShellFormConfigData data = new ShellFormConfigData(this);
                    layoutData.ConfigData = data;
					layoutData.Save();
				}
			}
		}

		#endregion

		#region Form fade in/out

		private bool activating = false;

		private void ShellForm_Activated(object sender, System.EventArgs e)
		{
			Debug.WriteLine("ShellForm_Activated", this.Name);

			if (enableOpacityChanges && !activating && !suspendOpacityChanges)
			{
				activating = true;

				timerFadeOut.Enabled = false;
				timerFadeIn.Enabled  = true;

				// Fade in any forms locked to me
				foreach (ShellForm instance in instances)
				{
					if (instance.lockedTo == this || this.lockedTo == instance)
					{
						Debug.WriteLine("Fade in " + instance.Name);
						instance.timerFadeIn.Enabled  = true;
						instance.timerFadeOut.Enabled = false;
						instance.Show();
					}
				}

				activating = false;
			}
		}

		private void ShellForm_Deactivate(object sender, System.EventArgs e)
		{
			Debug.WriteLine("ShellForm_Deactivate", this.Name);

			if (enableOpacityChanges && !activating && !suspendOpacityChanges)
			{
				timerFadeIn.Enabled  = false;
				timerFadeOut.Enabled = true;

				// Fade out any instance locked to me
				foreach (ShellForm instance in instances)
				{
					if (instance.lockedTo == this || this.lockedTo == instance)
					{
						Debug.WriteLine("Fade out " + instance.Name);
						instance.timerFadeIn.Enabled  = false;
						instance.timerFadeOut.Enabled = true;
					}
				}
			}
		}

		private void FadeIn_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{	
			if (this.Opacity < 0.97)
			{
				this.Opacity += 0.1;
			}
			else
			{
				timerFadeIn.Enabled = false;
			}
		}

		private void FadeOut_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{	
			if (this.Opacity > 0.52)
			{
				this.Opacity -= 0.1;
			}
			else
			{
				timerFadeOut.Enabled = false;
			}
		}

		private void buttonOpacity_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int width	= 8;
			int x		= buttonOnTop.Width / 2 - (width / 2);
			int y		= buttonOnTop.Height / 2 - (width / 2);

			if (enableOpacityChanges)
			{
				using (Bitmap bmp = new Bitmap(width, width))
				{
					for (int i = 0; i < 8; i += 2)
					{
						for (int j = 1; j < 8; j += 2)
						{
							bmp.SetPixel(i, j, Color.LawnGreen);
						}
					}
					using (Brush brush = new TextureBrush(bmp))
					{
						e.Graphics.FillRectangle(brush, x, y, width, width);
					}
				}
			}
			else
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.LawnGreen), x, y, width, width);
			}

		}

		private void buttonOpacity_Click(object sender, System.EventArgs e)
		{
			enableOpacityChanges = !enableOpacityChanges;
			buttonOpacity.Refresh();
		}

		private bool suspendOpacityChanges = false;

		public bool SuspendOpactiyChanges
		{
			get
			{
				return suspendOpacityChanges;
			}
			set
			{
				this.suspendOpacityChanges = value;
			}
		}

		#endregion

		#region Layout code

		private void panelTitle_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
		{
			if (!DesignMode && e.AffectedControl == null)
			{
				//LayoutForProjectF();
			}
			else
			{
				//panelTitle.Height = 0;
			}
		}

		private bool layedOut = false;

		private void LayoutForProjectF()
		{
			// Bail if in design mode
			if (!DesignMode && !layedOut)
			{
				layedOut = true;

				// If we are in ProjectF style, move all controls down
			{

				panelTitle.Height = 16;
				panelTitle.SendToBack();

				panelLeft.Width   = 3;
				panelLeft.SendToBack();

				panelRight.Width  = 3;
				panelRight.SendToBack();
				panelBottom.Height = 3;
				panelBottom.Top   = this.Height - panelBottom.Height - panelTitle.Height;
				panelBottom.SendToBack();

				this.Width = this.Width + panelLeft.Width + panelRight.Width;
				if (rolledUp)
				{
					this.Height = panelTitle.Height;
					this.savedSize = new Size(this.Width, this.savedSize.Height + panelTitle.Height + panelBottom.Height);
				}
				else
				{
					this.Height = this.Height + panelTitle.Height + panelBottom.Height;
				}

				foreach (Control c in this.Controls)
				{
					if (c != panelTitle && c.Parent != panelTitle && c != panelBottom && c != panelLeft && c != panelRight
						&& c.Dock == DockStyle.None)
					{
						c.Top    = c.Top + panelTitle.Height;
						c.Left   = c.Left + panelLeft.Width;

						if (c.Dock != DockStyle.None)
						{
							//								c.Width  = c.Width - panelLeft.Width - panelRight.Width;
							//								c.Height = c.Height - panelTitle.Height - panelBottom.Height;
						}
					}
				}
                    
				if (this.FixedSize.Width > 0)
					this.FixedSize = new Size(this.Width, this.FixedSize.Height);
				if (this.FixedSize.Height > 0)
					this.FixedSize = new Size(this.FixedSize.Width, this.Height);
					
			}
			}
			else
			{
				panelTitle.Height = 0;
			}

			PerformLockingCheck();

		}

		#endregion

		#region Sizing code

		private bool sizing = false;

		#region NS

		private void panelBottom_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (actualFormBorderStyle == FormBorderStyle.SizableToolWindow
				|| actualFormBorderStyle == FormBorderStyle.Sizable)
			{
				startY = e.Y;
				sizing = true;		
			}
		}

		private void panelBottom_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (sizing) 
			{
				this.Height = this.Height + e.Y - startY;
			}
		}

		private void panelBottom_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			sizing = false;
		}

		#endregion

		#region NW-SE

		private void panelSizeNWSE_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (actualFormBorderStyle == FormBorderStyle.SizableToolWindow
				|| actualFormBorderStyle == FormBorderStyle.Sizable)
			{
				startX = e.X;
				startY = e.Y;
				sizing = true;		
			}
		}

		private void panelSizeNWSE_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (sizing) 
			{
				this.Width  = this.Width  + e.X - startX;
				this.Height = this.Height + e.Y - startY;
			}
		}

		private void panelSizeNWSE_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			sizing = false;
		}

		#endregion

		#region EW - left panel

		private void panelLeft_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (actualFormBorderStyle == FormBorderStyle.SizableToolWindow
				|| actualFormBorderStyle == FormBorderStyle.Sizable)
			{
				startX = e.X;
				sizing = true;		
			}
		}

		private void panelLeft_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (sizing) 
			{
				this.Width  = this.Width - e.X + startX;
				this.Left   = this.Left + e.X - startX;
			}
		}

		private void panelLeft_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			sizing = false;
		}

		#endregion

		#region EW - right panel

		private void panelRight_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (actualFormBorderStyle == FormBorderStyle.SizableToolWindow
				|| actualFormBorderStyle == FormBorderStyle.Sizable)
			{
				startX = e.X;
				sizing = true;		
			}
		}

		private void panelRight_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (sizing) 
			{
				this.Width  = this.Width  + e.X - startX;
			}
		}

		private void panelRight_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			sizing = false;
		}

		#endregion

		#endregion

		#region TopMost

		private void buttonOnTop_Click(object sender, System.EventArgs e)
		{
			this.TopMost = !this.TopMost;
		}
		
		private void buttonOnTop_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int width	= 8;
			int x		= buttonOnTop.Width / 2 - (width / 2);
			int y		= buttonOnTop.Height / 2 - (width / 2);

			if (this.TopMost)
			{
				e.Graphics.FillEllipse(new SolidBrush(Color.LawnGreen), x, y, width, width);
			}
			else
			{
				using (Pen pen = new Pen(new SolidBrush(Color.LawnGreen)))
				{
					e.Graphics.DrawEllipse(pen, x, y, width, width);
				}
			}
		}

		#endregion

		#region Rollup

		private bool rollupHover = false;

		public event System.EventHandler Rollup_Expand;
		public event System.EventHandler Rollup_Collapse;

		private void buttonRollup_Click(object sender, System.EventArgs e)
		{
			if (rolledUp || rollupHover)
			{
				this.Height		= savedSize.Height;
				rolledUp		= false;
				rollupHover		= false;
				buttonRollup.Refresh();

				if (Rollup_Expand != null)
					Rollup_Expand(this, EventArgs.Empty);

				// Move any locked forms down
				foreach (ShellForm instance in instances)
				{
					if (instance.lockedTo == this)
					{
						instance.Top = this.Top + this.Height;
					}
				}
			}
			else 
			{
				savedSize = new Size(this.Width, this.Height);
				this.Height = 16;
				rolledUp = true;

				if (Rollup_Collapse != null)
					Rollup_Collapse(this, EventArgs.Empty);

				// Move any locked forms up
				foreach (ShellForm instance in instances)
				{
					if (instance.lockedTo == this)
					{
						instance.Top = this.Top + this.Height;
					}
				}
			}
		}

		private void buttonRollup_MouseHover(object sender, System.EventArgs e)
		{
			if (rolledUp)
			{
				rollupHover		= true;
				this.Height		= savedSize.Height;
			}
		}

		private void buttonRollup_MouseLeave(object sender, System.EventArgs e)
		{
			if (rollupHover)
			{
				this.Height		= panelTitle.Height;
				rollupHover		= false;
				rolledUp		= true;
			}
		}

		
		private void buttonRollup_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int width	= 8;
			int height  = 8;
			int x		= buttonRollup.Width / 2 - (width / 2);		// = 4
			int y		= buttonRollup.Height / 2 - (height / 2);	// = 4

			using (Pen pen = new Pen(new SolidBrush(Color.LawnGreen)))
			{
				Point[] points = new Point[4];

				if (rolledUp)
				{
					points[0] = new Point(x + width / 2, y + height);		// center bottom
					points[1] = new Point(x, y);			// upper left
					points[2] = new Point(x + width, y);	// upper right
					points[3] = new Point(x + width / 2, y + height);		// center bottom again
				}
				else
				{
					points[0] = new Point(x + width / 2, y);		// center top
					points[1] = new Point(x, y + height);			// lower left
					points[2] = new Point(x + width, y + height);	// lower right
					points[3] = new Point(x + width / 2, y);		// center top again
				}
				e.Graphics.FillPolygon(new SolidBrush(Color.LawnGreen), points, System.Drawing.Drawing2D.FillMode.Alternate);
			}
		}

		#endregion

		#region Adding Controls

		public void AddTitleBarControl(Control c, int left, int top, int height, bool visible)
		{
			panelTitleText.Controls.Add(c);
			c.Left		= left;
			c.Top		= top;
			c.Height	= height;
			c.Width		= labelTitle.Width;
			c.Visible	= visible;
			c.BringToFront();
			panelTitleText_Resize(this, EventArgs.Empty);
		}

		public void AddButtons(Control c, int width, bool visible)
		{
			panelMoreButtons.Controls.Add(c);
			panelMoreButtons.Width = width;
			c.Visible = visible;
			panelTitleText_Resize(this, EventArgs.Empty);
		}

		public int MoreButtonsWidth
		{
			get
			{
				return panelMoreButtons.Width;
			}
			set
			{
				panelMoreButtons.Width = value;
			}
		}

		#endregion

		#region Title functions

		private void labelTitle_MouseHover(object sender, System.EventArgs e)
		{
			if (TitleHover != null)
				TitleHover(this, EventArgs.Empty);	
		}

		private void panelTitleText_Resize(object sender, System.EventArgs e)
		{
			panelTitleText.Width = panelButtons.Left - panelMoreButtons.Width;
		}

		private void labelTitle_MouseLeave(object sender, System.EventArgs e)
		{
			if (TitleMouseLeave != null)
				TitleMouseLeave(this, EventArgs.Empty);
		}

		public event System.EventHandler TitleHover;
		public event System.EventHandler TitleMouseLeave;

		#endregion

	}

	#region Static delegates

	public delegate void ShellForm_AddedDelegate(object sender, ShellFormAddedEventArgs e);
	public delegate void ShellForm_RemovedDelegate(object sender, ShellFormRemovedEventArgs e);

	#endregion

	#region Static EventArgs classes

	public class ShellFormAddedEventArgs: EventArgs
	{
		private ShellForm form = null;
		private ShellForm parent = null;

		public ShellFormAddedEventArgs(ShellForm form)
		{
			this.form	= form;
		}

		public ShellFormAddedEventArgs(ShellForm form, ShellForm parent)
		{
			this.form	= form;
			this.parent	= parent;
		}

		public ShellForm ShellForm
		{
			get { return form; }
		}

		public ShellForm Parent
		{
			get { return parent; }
		}
	}

	public class ShellFormRemovedEventArgs: EventArgs
	{
		private ShellForm form = null;

		public ShellFormRemovedEventArgs(ShellForm form)
		{
			this.form	= form;
		}

		public ShellForm ShellForm
		{
			get { return form; }
		}
	}

	#endregion

	#region ShellFormConfigData

	public class ShellFormConfigData: msn2.net.Configuration.ConfigData
	{
		private int left;
		private int top;
		private bool topmost;
		private bool enableOpacityChanges;
		private ShellForm form;

		public ShellFormConfigData()
		{}

		public ShellFormConfigData(ShellForm form)
		{
			this.left					= form.Left;
			this.top					= form.Top;
			this.topmost				= form.TopMost;
			this.enableOpacityChanges	= form.EnableOpacityChanges;
			this.form					= form;

			Point p = form.DefaultLocation;
			if (p.X != 0)
				this.left = p.X;
			if (p.Y != 0)
				this.top  = p.Y;
		}

		public void Apply()
		{
			if (form != null)
			{
				form.Left					= this.left;
				form.Top					= this.top;
				form.TopMost				= this.topmost;
				form.EnableOpacityChanges	= this.enableOpacityChanges;
			}
		}

		public int Left
		{
			get { return left; }
			set { left = value; }
		}

		public int Top
		{
			get { return top; }
			set { top = value; }
		}

		public bool TopMost 
		{
			get { return topmost; }
			set { topmost = value; }
		}

		public bool EnableOpacityChanges
		{
			get { return enableOpacityChanges; }
			set { enableOpacityChanges = value; }
		}

	}

	#endregion

}
