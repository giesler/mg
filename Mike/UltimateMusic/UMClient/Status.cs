using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace UMClient
{
	/// <summary>
	/// Summary description for Status.
	/// </summary>
	public class Status : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label labelMessage;
		private System.Windows.Forms.Button buttonCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ProgressBar progressBar1;
		private bool cancel;
		private Thread thread = null;

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

		public void Increment(int amount) 
		{
			this.progressBar1.Increment(amount);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.labelMessage = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// labelMessage
			// 
			this.labelMessage.Location = new System.Drawing.Point(8, 8);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(288, 32);
			this.labelMessage.TabIndex = 0;
			this.labelMessage.Text = "Loading...";
			// 
			// buttonCancel
			// 
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(216, 48);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(16, 32);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(272, 8);
			this.progressBar1.TabIndex = 2;
			this.progressBar1.Visible = false;
			// 
			// Status
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 80);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.progressBar1,
																		  this.buttonCancel,
																		  this.labelMessage});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Status";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Please Wait...";
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			cancel = true;
			if (thread != null) 
			{
				thread.Abort();
				this.Visible = false; 
			}
		}

		public bool Cancel 
		{
			get 
			{
				return cancel;
			}
		}
	}
}
