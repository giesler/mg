#region Using
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using msn2.net.Configuration;
using System.Diagnostics;
#endregion

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
		protected System.Timers.Timer timerFadeOut;
		private System.Windows.Forms.Panel panelLeft;
		private System.Windows.Forms.Panel panelRight;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.Panel panelSizeNWSE;
		protected System.Timers.Timer timerFadeIn;
		private FormBorderStyle actualFormBorderStyle;
		private System.Windows.Forms.ImageList imageListTitleBar;

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
		private bool	shadedBackground			= false;
		private bool	titleVisible				= true;

		// Form style properties
		private int		titleHeight					= 18;
		private int		borderWidth					= 2;
		private int		buttonWidth					= 8;
        private int		buttonHeight				= 8;
		private int		rollupHeight				= 18;
		private bool	dialog						= false;
		
		private int		fadeInTimerInterval			= 1;
		private int		fadeOutTimerInterval		= 1;
		protected bool	fadeInEnabled				= false;
		private System.Windows.Forms.Panel panelTitleText;
		private System.Windows.Forms.PictureBox pictureBoxFormIcon;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Panel panelMoreButtons;
		private System.Windows.Forms.Panel panelButtons;
		private System.Windows.Forms.Button buttonOpacity;
		private System.Windows.Forms.Button buttonRollup;
		private System.Windows.Forms.Button buttonOnTop;
		private System.Windows.Forms.Button buttonHide;
		private System.Windows.Forms.Panel panelTitle;
		
		private Crownwood.Magic.Controls.TabPage tabPage = null;
		
		#endregion
		#region Constructor and Disposal

		public ShellForm()
		{
			ShellFormInternalConstructor();
		}

		public ShellForm(Data formNode)
		{
			this.formNode	= formNode;
			this.Text		= formNode.Name;

			ShellFormInternalConstructor();

			if (formNode != null)
			{
				Trace.WriteLine("Reading config data for " + this.Name);

				ShellFormConfigData defaultConfig = new ShellFormConfigData(this);
				layoutData = formNode.Get("ShellFormConfigData", defaultConfig, typeof(ShellFormConfigData));
				ShellFormConfigData layoutConfig = (ShellFormConfigData) layoutData.ConfigData;
				layoutConfig.Apply(this);
			}                
		}

		private void ShellFormInternalConstructor()
		{
			if (!DesignMode)
				InitializeComponent();

			timerFadeOut = new System.Timers.Timer(fadeOutTimerInterval);
			timerFadeOut.Elapsed += new System.Timers.ElapsedEventHandler(FadeOut_Elapsed);

			timerFadeIn = new System.Timers.Timer(fadeInTimerInterval);
			timerFadeIn.Elapsed += new System.Timers.ElapsedEventHandler(FadeIn_Elapsed);

			panelMoreButtons.Width = 0;

			AddInstance(this);
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
			this.panelLeft = new System.Windows.Forms.Panel();
			this.panelRight = new System.Windows.Forms.Panel();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.panelSizeNWSE = new System.Windows.Forms.Panel();
			this.imageListTitleBar = new System.Windows.Forms.ImageList(this.components);
			this.panelTitleText = new System.Windows.Forms.Panel();
			this.pictureBoxFormIcon = new System.Windows.Forms.PictureBox();
			this.labelTitle = new System.Windows.Forms.Label();
			this.panelMoreButtons = new System.Windows.Forms.Panel();
			this.panelButtons = new System.Windows.Forms.Panel();
			this.buttonOpacity = new System.Windows.Forms.Button();
			this.buttonRollup = new System.Windows.Forms.Button();
			this.buttonOnTop = new System.Windows.Forms.Button();
			this.buttonHide = new System.Windows.Forms.Button();
			this.panelTitle = new System.Windows.Forms.Panel();
			this.panelBottom.SuspendLayout();
			this.panelTitleText.SuspendLayout();
			this.panelButtons.SuspendLayout();
			this.panelTitle.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelLeft
			// 
			this.panelLeft.BackColor = System.Drawing.Color.DimGray;
			this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelLeft.Location = new System.Drawing.Point(0, 16);
			this.panelLeft.Name = "panelLeft";
			this.panelLeft.Size = new System.Drawing.Size(0, 246);
			this.panelLeft.TabIndex = 2;
			this.panelLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelLeft_MouseUp);
			this.panelLeft.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLeft_Paint);
			this.panelLeft.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelLeft_MouseMove);
			this.panelLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelLeft_MouseDown);
			// 
			// panelRight
			// 
			this.panelRight.BackColor = System.Drawing.Color.DimGray;
			this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelRight.Location = new System.Drawing.Point(344, 16);
			this.panelRight.Name = "panelRight";
			this.panelRight.Size = new System.Drawing.Size(0, 246);
			this.panelRight.TabIndex = 3;
			this.panelRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelRight_MouseUp);
			this.panelRight.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRight_Paint);
			this.panelRight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelRight_MouseMove);
			this.panelRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelRight_MouseDown);
			// 
			// panelBottom
			// 
			this.panelBottom.BackColor = System.Drawing.Color.Gray;
			this.panelBottom.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.panelSizeNWSE});
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelBottom.Location = new System.Drawing.Point(0, 262);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(344, 0);
			this.panelBottom.TabIndex = 4;
			this.panelBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelBottom_MouseUp);
			this.panelBottom.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBottom_Paint);
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
			// panelTitleText
			// 
			this.panelTitleText.BackColor = System.Drawing.Color.Transparent;
			this.panelTitleText.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.pictureBoxFormIcon,
																						 this.labelTitle});
			this.panelTitleText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTitleText.Name = "panelTitleText";
			this.panelTitleText.Size = new System.Drawing.Size(248, 16);
			this.panelTitleText.TabIndex = 5;
			this.panelTitleText.Resize += new System.EventHandler(this.panelTitleText_Resize);
			this.panelTitleText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelTitleText_MouseUp);
			this.panelTitleText.MouseHover += new System.EventHandler(this.panelTitleText_MouseHover);
			this.panelTitleText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelTitleText_MouseMove);
			this.panelTitleText.MouseLeave += new System.EventHandler(this.panelTitleText_MouseLeave);
			this.panelTitleText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitleText_MouseDown);
			// 
			// pictureBoxFormIcon
			// 
			this.pictureBoxFormIcon.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxFormIcon.Name = "pictureBoxFormIcon";
			this.pictureBoxFormIcon.Size = new System.Drawing.Size(16, 16);
			this.pictureBoxFormIcon.TabIndex = 2;
			this.pictureBoxFormIcon.TabStop = false;
			this.pictureBoxFormIcon.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxFormIcon_Paint);
			// 
			// labelTitle
			// 
			this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.labelTitle.BackColor = System.Drawing.Color.Transparent;
			this.labelTitle.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelTitle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.labelTitle.Location = new System.Drawing.Point(16, 0);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(232, 23);
			this.labelTitle.TabIndex = 1;
			this.labelTitle.Text = "[title]";
			this.labelTitle.Visible = false;
			this.labelTitle.TextChanged += new System.EventHandler(this.labelTitle_TextChanged);
			this.labelTitle.DoubleClick += new System.EventHandler(this.labelTitle_DoubleClick);
			// 
			// panelMoreButtons
			// 
			this.panelMoreButtons.BackColor = System.Drawing.Color.Transparent;
			this.panelMoreButtons.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelMoreButtons.ForeColor = System.Drawing.SystemColors.ControlText;
			this.panelMoreButtons.Location = new System.Drawing.Point(248, 0);
			this.panelMoreButtons.Name = "panelMoreButtons";
			this.panelMoreButtons.Size = new System.Drawing.Size(24, 16);
			this.panelMoreButtons.TabIndex = 7;
			// 
			// panelButtons
			// 
			this.panelButtons.BackColor = System.Drawing.Color.Transparent;
			this.panelButtons.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this.buttonOpacity,
																					   this.buttonRollup,
																					   this.buttonOnTop,
																					   this.buttonHide});
			this.panelButtons.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelButtons.Location = new System.Drawing.Point(272, 0);
			this.panelButtons.Name = "panelButtons";
			this.panelButtons.Size = new System.Drawing.Size(72, 16);
			this.panelButtons.TabIndex = 6;
			this.panelButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.panelButtons_Paint);
			// 
			// buttonOpacity
			// 
			this.buttonOpacity.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.buttonOpacity.BackColor = System.Drawing.Color.Transparent;
			this.buttonOpacity.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOpacity.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonOpacity.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonOpacity.Location = new System.Drawing.Point(6, 1);
			this.buttonOpacity.Name = "buttonOpacity";
			this.buttonOpacity.Size = new System.Drawing.Size(14, 14);
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
			this.buttonRollup.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonRollup.Location = new System.Drawing.Point(38, 1);
			this.buttonRollup.Name = "buttonRollup";
			this.buttonRollup.Size = new System.Drawing.Size(14, 14);
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
			this.buttonOnTop.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonOnTop.Location = new System.Drawing.Point(22, 1);
			this.buttonOnTop.Name = "buttonOnTop";
			this.buttonOnTop.Size = new System.Drawing.Size(14, 14);
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
			this.buttonHide.ForeColor = System.Drawing.SystemColors.ControlText;
			this.buttonHide.Location = new System.Drawing.Point(54, 1);
			this.buttonHide.Name = "buttonHide";
			this.buttonHide.Size = new System.Drawing.Size(18, 14);
			this.buttonHide.TabIndex = 5;
			this.buttonHide.Text = "X";
			this.buttonHide.Click += new System.EventHandler(this.button1_Click);
			this.buttonHide.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonHide_Paint);
			// 
			// panelTitle
			// 
			this.panelTitle.BackColor = System.Drawing.Color.Silver;
			this.panelTitle.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.panelTitleText,
																					 this.panelMoreButtons,
																					 this.panelButtons});
			this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTitle.Name = "panelTitle";
			this.panelTitle.Size = new System.Drawing.Size(344, 16);
			this.panelTitle.TabIndex = 0;
			this.panelTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelTitle_MouseUp);
			this.panelTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTitle_Paint);
			this.panelTitle.MouseHover += new System.EventHandler(this.panelTitle_MouseHover);
			this.panelTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelTitle_MouseMove);
			this.panelTitle.MouseLeave += new System.EventHandler(this.panelTitle_MouseLeave);
			this.panelTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitle_MouseDown);
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
			this.MouseHover += new System.EventHandler(this.ShellForm_MouseHover);
			this.Activated += new System.EventHandler(this.ShellForm_Activated);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShellForm_Paint);
			this.MouseEnter += new System.EventHandler(this.ShellForm_MouseEnter);
			this.MouseLeave += new System.EventHandler(this.ShellForm_MouseLeave);
			this.Deactivate += new System.EventHandler(this.ShellForm_Deactivate);
			this.panelBottom.ResumeLayout(false);
			this.panelTitleText.ResumeLayout(false);
			this.panelButtons.ResumeLayout(false);
			this.panelTitle.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		#region Load

		private void ShellForm_Load(object sender, System.EventArgs e)
		{
			// Check if we should lock to another form based on position
//			PerformLockingCheck();
		}

		#endregion
		#region Layout

		private bool savedBorderStyle = false;

		private void ShellForm_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
		{
			// System.Diagnostics.Debug.WriteLine(this.Name + ": layout");

			// Add mouseover cursors if not in a fixed style
			if (!DesignMode && this.Parent == null)
			{
				if (!savedBorderStyle)
				{
					System.Diagnostics.Debug.WriteLine("Display: " + this.FormBorderStyle.ToString());
					actualFormBorderStyle = this.FormBorderStyle;
					this.FormBorderStyle = FormBorderStyle.None;
					savedBorderStyle = true;
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

			// Hide the borders if we are in a tabpage
			if (this.tabPage != null)
			{
				this.panelTitle.Visible		= false;
				this.panelLeft.Visible		= false;
				this.panelRight.Visible		= false;
				this.panelBottom.Visible	= false;
			}
			else
			{
				this.panelTitle.Visible		= true;
				this.panelLeft.Visible		= true;
				this.panelRight.Visible		= true;
				this.panelBottom.Visible	= true;
			}

			LayoutForProjectF();

		}

		#endregion
		#region Properties

		public Crownwood.Magic.Controls.TabPage TabPage
		{
			get
			{
				return tabPage;
			}
			set
			{
				tabPage = value;

				// Add and remove from shellform collection
				ShellForm.RemoveInstance(this);
				ShellForm.AddInstance(this);
			}
		}

		public bool AllowUnload
		{
			get { return allowUnload; }
			set { allowUnload = value; }
		}

		public Control TitleBar
		{
			get { return panelTitle; }
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
				if (labelTitle != null)
					labelTitle.Text = value;
			}
		}

		[Category("Appearance")]
		public bool Dialog
		{
			get
			{
				return this.dialog;
			}
			set
			{
				this.dialog = value;
				if (value)
				{
					ShellForm.RemoveInstance(this);
				}
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

		[Category("Appearance")]
		public bool ShadedBackground
		{
			get
			{
				return shadedBackground;
			}
			set
			{
				shadedBackground = value;
			}
		}

		public bool FadeInEnabled
		{
			get
			{
				return fadeInEnabled;
			}
			set
			{
				fadeInEnabled = value;
			}
		}

		public new Size MaximumSize
		{
			get
			{
				return base.MaximumSize;
			}
			set
			{
				// Adjust fixed size for changes in size due to borders
				if (value.Width > 0)
				{
					value			= new Size(value.Width + this.panelLeft.Width + this.panelRight.Width, value.Height);
					this.Width		= value.Width;
				}
				if (value.Height > 0)
				{
					value			= new Size(value.Width, value.Height + this.panelTitle.Height + this.panelBottom.Height);
					this.Height		= value.Height;
				}
				base.MaximumSize = value;
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
				return titleVisible;
			}
			set
			{
				titleVisible= value;
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

		protected void MoveForm(int xdiff, int ydiff)
		{
			this.Left = this.Left + xdiff;
			this.Top  = this.Top  + ydiff;

			// Move any forms locked to this form
			foreach (ShellForm instance in instances)
			{
				if (instance.lockedTo == this)
				{
					instance.MoveForm(xdiff, ydiff);
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
						instance.FadeIn(this);
						
						// We want to hide buttons that we want controlled by both instances
						this.TopMost					= instance.TopMost;
						this.EnableOpacityChanges		= instance.EnableOpacityChanges;
						this.buttonOnTop.Visible		= false;
						this.buttonOpacity.Visible		= false;

						// Move form to be in correct position
						MoveForm( instance.Left - this.Left, (instance.Top + instance.Height) - this.Top);

						// Snap widths if close
						if (Math.Abs(this.Width - instance.Width) < 20)
						{
							int newWidth = Math.Max(this.Width, instance.Width);
							this.Width		= newWidth;
							instance.Width	= newWidth;
						}

						Debug.WriteLine(this.Name + " locked to " + instance.Name);
						break;
					}

				}
					// we are NOT within 10, so check if we should un-snap
				else
				{
					// only unlock if we are away from current loop form
					if (lockedTo != null && lockedTo == instance)
					{
						Debug.WriteLine(this.Name + " unlocked");
						lockedTo.timerFadeIn.Enabled	= false;
						lockedTo.timerFadeOut.Enabled	= true;
						this.buttonOnTop.Visible		= true;
						this.buttonOpacity.Visible		= true;
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

		#endregion
		#region Close

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ShellForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (dialog)
			{
				// we don't care
			}
			else if (!allowUnload)
			{
				e.Cancel = true;
				this.Visible = false;
			}
			else
			{
				if (formNode != null && layoutData != null)
				{
					ShellFormConfigData data = new ShellFormConfigData(this);
                    layoutData.ConfigData = data;
					layoutData.Save();
				}
			}
		}

		#endregion
		#region Form fade in/out

		private void ShellForm_Activated(object sender, System.EventArgs e)
		{
			//Debug.WriteLine("ShellForm_Activated", this.Name);

			if (enableOpacityChanges && !activating && !suspendOpacityChanges)
			{
				activating = true;

				// recursively fade in all locked forms
				FadeIn(this);

				activating = false;
			}
		}

		protected static bool activating = false;

		protected void FadeIn(ShellForm source)
		{
			this.Opacity = 1;

//			timerFadeOut.Enabled = false;
//			timerFadeIn.Enabled  = true;

			// Fade in any forms locked to me
			foreach (ShellForm instance in instances)
			{
				if (instance != source && (instance.lockedTo == this || this.lockedTo == instance))
				{
					Debug.WriteLine("Fade in " + instance.Name);
					
					instance.FadeIn(this);
					instance.Show();
				}
			}
		}

		private void ShellForm_Deactivate(object sender, System.EventArgs e)
		{
			//Debug.WriteLine("ShellForm_Deactivate", this.Name);

			if (enableOpacityChanges && !activating && !suspendOpacityChanges)
			{
				// Recursively fade out all locked forms
				FadeOut(this);
			}
		}

		protected void FadeOut(ShellForm source)
		{
			this.Opacity = 0.52;

//			timerFadeOut.Enabled = true;
//			timerFadeIn.Enabled  = false;

			// Fade in any forms locked to me
			foreach (ShellForm instance in instances)
			{
				if (instance != source && (instance.lockedTo == this || this.lockedTo == instance))
				{
					Debug.WriteLine("Fade out " + instance.Name);
					instance.FadeOut(this);
					instance.Show();
				}
			}
		}

		private void FadeIn_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{	
			if (this.Opacity < 1.0 && fadeInEnabled)
			{
				this.Opacity += 0.1;
			}
			else
			{
				this.Opacity		= 1.0;
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
			int width	= buttonWidth;
			int height	= buttonHeight;
			int x		= buttonOnTop.Width / 2 - (width / 2);
			int y		= buttonOnTop.Height / 2 - (width / 2);

			if (enableOpacityChanges)
			{
				using (Bitmap bmp = new Bitmap(width, height))
				{
					for (int i = 0; i < buttonWidth; i += 2)
					{
						for (int j = 1; j < buttonHeight; j += 2)
						{
							bmp.SetPixel(i, j, panelTitle.ForeColor);
						}
					}
					using (Brush brush = new TextureBrush(bmp))
					{
						e.Graphics.FillRectangle(brush, x, y, width, height);
					}
				}
			}
			else
			{
				e.Graphics.FillRectangle(new SolidBrush(panelTitle.ForeColor), x, y, width, height);
			}

		}

		private void buttonOpacity_Click(object sender, System.EventArgs e)
		{
			enableOpacityChanges = !enableOpacityChanges;
			buttonOpacity.Refresh();

			foreach (ShellForm instance in instances)
			{
				// Reposition any forms locked to this form
				if (instance.lockedTo == this)
				{
					instance.EnableOpacityChanges = this.enableOpacityChanges;
                    instance.Refresh();
				}
			}
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

		private bool sized = false;

		public void ForceLayout()
		{
			LayoutForProjectF();
		}

		private void LayoutForProjectF()
		{
			// Trace.WriteLine("Laying out " + this.Text);

			// Bail if in design mode
			if (!DesignMode)
			{
				this.SuspendLayout();

				panelTitle.SendToBack();
				panelLeft.SendToBack();
				panelRight.SendToBack();
				panelBottom.SendToBack();

				if (!sized)
				{
					sized = true;                
					panelTitle.Height = titleHeight;
					panelBottom.Height = borderWidth;
					panelRight.Width  = borderWidth;
					panelLeft.Width   = borderWidth;
					panelBottom.Top   = this.Height - panelBottom.Height - panelTitle.Height;
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

				}

				foreach (Control c in this.Controls)
				{
					if (c != panelTitle		&&	c.Parent != panelTitle		&& 
						c != panelBottom	&&	c != panelLeft				&& 
						c != panelRight		&&	c.Dock == DockStyle.None)
					{
						if (!layoutControlList.Contains(c))
						{
							Trace.WriteLine(c.Name);

							layoutControlList.Add(c);

							c.Top    = c.Top + panelTitle.Height;
							c.Left   = c.Left + panelLeft.Width;

							if (c.Dock != DockStyle.None)
							{
								c.Width  = c.Width - panelLeft.Width - panelRight.Width;
								c.Height = c.Height - panelTitle.Height - panelBottom.Height;
							}
							c.BringToFront();
						}
					}
				}

				this.ResumeLayout();
			}
			else
			{
				panelTitle.Height = 0;
			}

			PerformLockingCheck();

		}

		private ArrayList layoutControlList = new ArrayList();

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
			buttonOnTop.Refresh();

			// set ontop for any forms locked to us
			foreach (ShellForm instance in instances)
			{
				// Reposition any forms locked to this form
				if (instance.lockedTo == this)
				{
					instance.TopMost = this.TopMost;
				}
			}
		}

		private void buttonOnTop_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int width	= buttonWidth;
			int height	= buttonHeight;
			int x		= buttonOnTop.Width / 2 - (width / 2);
			int y		= buttonOnTop.Height / 2 - (height / 2);

			if (this.TopMost)
			{
				e.Graphics.FillEllipse(new SolidBrush(panelTitle.ForeColor), x, y, width, height);
			}
			else
			{
				using (Pen pen = new Pen(new SolidBrush(panelTitle.ForeColor)))
				{
					e.Graphics.DrawEllipse(pen, x, y, width, height);
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
				this.Height = rollupHeight;
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
			int width	= buttonWidth;
			int height  = buttonHeight;
			int x		= buttonRollup.Width / 2 - (width / 2);		// = 4
			int y		= buttonRollup.Height / 2 - (height / 2);	// = 4

			using (Pen pen = new Pen(new SolidBrush(panelTitle.ForeColor)))
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
				e.Graphics.FillPolygon(new SolidBrush(panelTitle.ForeColor), points, System.Drawing.Drawing2D.FillMode.Alternate);
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

		private void panelTitleText_Resize(object sender, System.EventArgs e)
		{
			panelTitleText.Width = panelButtons.Left - panelMoreButtons.Width;
		}

		private void pictureBoxFormIcon_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// Darken the background.  Code from MagicLibrary.
			// Calculate the IDE background colour as only half as dark as the control colour
			msn2.net.Common.Drawing.ShadeRegion(e, panelTitle.BackColor);

			pictureBoxFormIcon.Top		= 1;
			labelTitle.Top				= 1;
			panelButtons.Top			= 1;
			panelMoreButtons.Top		= 1;

			// Draw the form icon.
			e.Graphics.DrawIcon(this.Icon, e.ClipRectangle);

		}

		private void labelTitle_DoubleClick(object sender, System.EventArgs e)
		{
			if (this.WindowState != System.Windows.Forms.FormWindowState.Maximized)
			{
				this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			}
			else
			{
				this.WindowState = System.Windows.Forms.FormWindowState.Normal;
			}
		}

		public event System.EventHandler TitleHover;
		public event System.EventHandler TitleMouseLeave;

		#endregion
		#region Show/Hide buttons

		private bool showOpacityButton		= true;
		private bool showRollupButton		= true;
		private bool showCloseButton		= true;
		private bool showTopMostButton		= true;

		[Category("Title Bar Buttons")]
		public bool ShowOpacityButton
		{
			get { return showOpacityButton; }
			set 
			{
				showOpacityButton = value;
				UpdateButtons();
			}
		}

		[Category("Title Bar Buttons")]
		public bool ShowRollupButton
		{
			get { return showRollupButton; }
			set 
			{
				showRollupButton = value;
				UpdateButtons();
			}
		}

		[Category("Title Bar Buttons")]
		public bool ShowCloseButton
		{
			get { return showCloseButton; }
			set
			{
				showCloseButton = value;
				UpdateButtons();
			}
		}

		[Category("Title Bar Buttons")]
		public bool ShowTopMostButton
		{
			get { return showTopMostButton; }
			set 
			{ 
				showTopMostButton = value; 
				UpdateButtons();
			}
		}

		private void UpdateButtons()
		{
			this.SuspendLayout();

			// Show/hide buttons
			buttonOpacity.Visible		= showOpacityButton;
			buttonOnTop.Visible			= showTopMostButton;
			buttonRollup.Visible		= showRollupButton;
			buttonHide.Visible			= showCloseButton;

			int visibleButtons = 0;

			// Go through each button, moving it to leftmost as needed
			if (showCloseButton)
			{
//				buttonHide.Left		= leftmostButton;
				buttonHide.Dock		= DockStyle.Right;
				buttonHide.BringToFront();
				visibleButtons++;
			}
			if (showRollupButton)
			{
//				buttonRollup.Left	= leftmostButton;
				buttonRollup.Dock	= DockStyle.Right;
				buttonRollup.BringToFront();
				visibleButtons++;
			}
			if (showTopMostButton)
			{
//				buttonOnTop.Left	= leftmostButton;
				buttonOnTop.Dock	= DockStyle.Right;
				buttonOnTop.BringToFront();
				visibleButtons++;
			}
			if (showOpacityButton)
			{
//				buttonOpacity.Left	= leftmostButton;
				buttonOpacity.Dock	= DockStyle.Right;
				buttonOpacity.BringToFront();
				visibleButtons++;
			}

			// TODO: Resize to width of buttons visible
            // now resize panel buttons to correct size
//			panelButtons.Left	= this.Width - (visibleButtons * buttonWidth);
//			panelButtons.Width	= (visibleButtons * buttonWidth);

			this.ResumeLayout(true);
		}

		#endregion
		#region Paint

		private void ShellForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (shadedBackground)
			{
				//msn2.net.Common.Drawing.ShadeRegion(e, Color.DarkGray, Color.LightGray);
			}
		}

		#endregion
		#region ShadeRegion


		#endregion
		#region Methods

		public DialogResult ShowShellDialog(System.Windows.Forms.IWin32Window owner)
		{
			ShellForm parentForm = (ShellForm) owner;
			bool opacitySetting = false;
			this.Dialog = true;

			if (parentForm != null)
			{
				opacitySetting = parentForm.enableOpacityChanges;

				if (opacitySetting)
				{
					parentForm.enableOpacityChanges = false;
				}
			}

			DialogResult result = base.ShowDialog(owner);

			if (parentForm != null && opacitySetting)
			{
				parentForm.enableOpacityChanges = true;
			}

			return result;
		}

		#endregion
		#region Mouse events
		private void ShellForm_MouseHover(object sender, System.EventArgs e)
		{
// TODO:			if (this.Opacity != 1.0)
//			{
//				this.Activate();
//			}
		}

		private void ShellForm_MouseEnter(object sender, System.EventArgs e)
		{
			Trace.WriteLine(this.Name + ": MouseEnter");
		}

		private void ShellForm_MouseLeave(object sender, System.EventArgs e)
		{
			Trace.WriteLine(this.Name + ": MouseLeave");
		}
		#endregion

		private void panelTitle_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, panelTitle.BackColor);

			if (titleVisible)
			{
				e.Graphics.DrawString(labelTitle.Text, labelTitle.Font, new SolidBrush(panelTitle.ForeColor), labelTitle.Location);
			}
		}

		private void buttonHide_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		}

		private void panelBottom_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			//msn2.net.Common.Drawing.ShadeRegion(e, panelBottom.BackColor);
		}

		private void panelLeft_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, panelBottom.BackColor);
		}

		private void panelRight_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, panelBottom.BackColor);
		}

		private void panelButtons_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, panelTitle.BackColor);
		}

		private void labelTitle_TextChanged(object sender, System.EventArgs e)
		{
			panelTitle.Refresh();
		}

		private void panelTitle_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			startX = e.X;
			startY = e.Y;
			moving = true;		
		}

		private void panelTitle_MouseHover(object sender, System.EventArgs e)
		{
			if (TitleHover != null)
				TitleHover(this, EventArgs.Empty);	
		}

		private void panelTitle_MouseLeave(object sender, System.EventArgs e)
		{
			if (TitleMouseLeave != null)
				TitleMouseLeave(this, EventArgs.Empty);
		}

		private void panelTitle_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (moving) 
			{
				// Calculate potential new position
				int newLeft		= this.Left + e.X - startX;
				int newTop		= this.Top + e.Y - startY;

				PerformLockingCheck(newLeft, newTop, false);

				// Only move form if it didn't just get locked
				if (this.lockedTo == null)
					MoveForm(e.X - startX, e.Y - startY);
			}
		}

		private void panelTitle_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			moving = false;
		}

		private void panelTitleText_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			panelTitle_MouseDown(sender, e);
		}

		private void panelTitleText_MouseHover(object sender, System.EventArgs e)
		{
			panelTitle_MouseHover(sender, e);
		}

		private void panelTitleText_MouseLeave(object sender, System.EventArgs e)
		{
            panelTitle_MouseLeave(sender, e);		
		}

		private void panelTitleText_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			panelTitle_MouseMove(sender, e);
		}

		private void panelTitleText_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			panelTitle_MouseUp(sender, e);
		}

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

	#region ShellFormmsn2.net.Configuration.ConfigData

	public class ShellFormConfigData: msn2.net.Configuration.ConfigData
	{
		#region Declares
		private int left;
		private int top;
		private bool topmost;
		private bool enableOpacityChanges;
		private ShellForm form;
		#endregion
		#region Constructors
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
		#endregion
		#region Methods
		public void Apply(ShellForm form)
		{
			if (form != null)
			{
				// Make sure we are within bounds of the current screen
				if (this.Left < Screen.PrimaryScreen.Bounds.Width)
				{
					form.Left = this.left;
				}
				else
				{
					form.Left = Screen.PrimaryScreen.Bounds.Width - form.Width;
				}

				// Make sure we are within bounds of the current screen
				if (this.Top < Screen.PrimaryScreen.Bounds.Height)
				{
					form.Top = this.top;
				}
				else
				{
					form.Top = Screen.PrimaryScreen.Bounds.Height - form.Height;
				}

				form.TopMost				= this.topmost;
				form.EnableOpacityChanges	= this.enableOpacityChanges;
			}
		}
		#endregion
		#region Properties
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

		#endregion

	}

	#endregion

}
