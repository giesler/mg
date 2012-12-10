using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ItemTimer
{
	/// <summary>
	/// Summary description for ActiveTimer.
	/// </summary>
	public class ActiveTimer : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Timer timerSecondElapsed;
		private System.ComponentModel.IContainer components;
		protected TimeSpan length;
		private System.Windows.Forms.Label remainingLabel;
		protected DateTime endTime;
		private System.Windows.Forms.Timer timerFlash;
		private Form openingForm;
		protected bool flashOn;

		public ActiveTimer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		public ActiveTimer(Form openingForm, string titleText, TimeSpan length)
		{
			this.openingForm = openingForm;

			endTime = DateTime.Now.Add(length);

			InitializeComponent();

			this.Text = titleText;

			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			TimeSpan remaining = endTime - DateTime.Now;

			string text = remaining.Minutes + ":" + remaining.Seconds.ToString("00");

			if (remaining.Hours > 0)
			{
				text = remaining.Hours.ToString() + ":" + remaining.Minutes.ToString("00");
			}

			if (remaining.TotalSeconds > 0)
			{
				if (text != remainingLabel.Text)
				{
					remainingLabel.Text = text;
				}
			}
			else
			{
				timerSecondElapsed.Enabled = false;
				timerFlash.Enabled = true;

				SpeechModule.Speak(this.Text);
			}


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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.remainingLabel = new System.Windows.Forms.Label();
			this.timerSecondElapsed = new System.Windows.Forms.Timer(this.components);
			this.timerFlash = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// remainingLabel
			// 
			this.remainingLabel.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.remainingLabel.Location = new System.Drawing.Point(5, 4);
			this.remainingLabel.Name = "remainingLabel";
			this.remainingLabel.Size = new System.Drawing.Size(112, 40);
			this.remainingLabel.TabIndex = 0;
			this.remainingLabel.Text = "xx:xx";
			this.remainingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.remainingLabel.Click += new System.EventHandler(this.label1_Click);
			// 
			// timerSecondElapsed
			// 
			this.timerSecondElapsed.Enabled = true;
			this.timerSecondElapsed.Tick += new System.EventHandler(this.timerSecondElapsed_Tick);
			// 
			// timerFlash
			// 
			this.timerFlash.Interval = 500;
			this.timerFlash.Tick += new System.EventHandler(this.timerFlash_Tick);
			// 
			// ActiveTimer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(122, 48);
			this.Controls.Add(this.remainingLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MinimizeBox = false;
			this.Name = "ActiveTimer";
			this.Opacity = 0.85;
			this.Text = "ActiveTimer";
			this.TopMost = true;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ActiveTimer_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		private void timerSecondElapsed_Tick(object sender, System.EventArgs e)
		{
			UpdateDisplay();
		
		}

		private void label1_Click(object sender, System.EventArgs e)
		{
			if (DateTime.Now > endTime)
			{
				this.Close();
			}
		}

		private void ActiveTimer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (openingForm != null)
			{
				openingForm.Close();
				openingForm = null;
			}

		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
		
		}

		private void timerFlash_Tick(object sender, System.EventArgs e)
		{
			if (flashOn)
			{
				this.BackColor = SystemColors.Control;
			}
			else
			{
				this.BackColor = SystemColors.Highlight;
			}

			flashOn = !flashOn;
		}

	}
}
