using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for Status.
	/// </summary>
	public class Status : msn2.net.Controls.ShellForm
	{
		#region Declares

		private System.ComponentModel.Container components = null;
		private bool cancel;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button buttonCancel;
		private msn2.net.Controls.ShellLabel labelMessage;
		private Thread thread = null;

		#endregion

		#region Constructors

		public Status(string message)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			labelMessage.Text = message;
			this.Visible = true;
			this.Refresh();
		}

		public Status(string message, int maxProgress) 
		{
			InitializeComponent();
			labelMessage.Text = message;
			this.progressBar1.Maximum = maxProgress;
			this.progressBar1.Visible = true;
			this.Visible = true;
			this.Refresh();
		}

		public Status(string message, Thread thread) 
		{
			InitializeComponent();
			labelMessage.Text = message;
			this.Visible = true;
			this.Refresh();

			this.thread = thread;
		}

		#endregion

		#region Disposal

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

		#region Properties

		public bool Cancel 
		{
			get 
			{
				return cancel;
			}
		}

		public string Message 
		{
			set 
			{
				labelMessage.Text = value;
				this.Refresh();
			}
		}

		public int Progress 
		{
			set 
			{
				this.progressBar1.Value = value;
			}
		}

		#endregion

		#region Methods

		public void Increment(int amount) 
		{
			this.progressBar1.Increment(amount);
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Status));
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelMessage = new msn2.net.Controls.ShellLabel();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// timerFadeOut
			// 
			this.timerFadeOut.Enabled = false;
			// 
			// timerFadeIn
			// 
			this.timerFadeIn.Enabled = false;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(13, 33);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(272, 8);
			this.progressBar1.TabIndex = 8;
			this.progressBar1.Visible = false;
			// 
			// buttonCancel
			// 
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(205, 49);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// labelMessage
			// 
			this.labelMessage.Location = new System.Drawing.Point(5, 9);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(288, 32);
			this.labelMessage.TabIndex = 6;
			// 
			// Status
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(298, 80);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.progressBar1,
																		  this.buttonCancel,
																		  this.labelMessage});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Status";
			this.ShowCloseButton = false;
			this.ShowOpacityButton = false;
			this.ShowRollupButton = false;
			this.ShowTopMostButton = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Please Wait...";
			this.TitleVisible = true;
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Status_Paint);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Button Handlers

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			cancel = true;
			if (thread != null) 
			{
				thread.Abort();
				this.Visible = false; 
			}
		}

		#endregion

		#region Paint

		private void Status_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			msn2.net.Common.Drawing.ShadeRegion(e, Color.LightGray);
		}

		#endregion

	}
}
