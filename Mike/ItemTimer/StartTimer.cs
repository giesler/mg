using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace ItemTimer
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class StartTimer : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.NumericUpDown hours;
		private System.Windows.Forms.NumericUpDown minutes;
		private System.Windows.Forms.NumericUpDown seconds;
		private System.Windows.Forms.TextBox inkEdit1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public StartTimer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(StartTimer));
			this.nameLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.hours = new System.Windows.Forms.NumericUpDown();
			this.minutes = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.seconds = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.startButton = new System.Windows.Forms.Button();
			this.inkEdit1 = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.hours)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.minutes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.seconds)).BeginInit();
			this.SuspendLayout();
			// 
			// nameLabel
			// 
			this.nameLabel.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.nameLabel.Location = new System.Drawing.Point(16, 16);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(408, 32);
			this.nameLabel.TabIndex = 1;
			this.nameLabel.Text = "Name this timer (if you want):";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(80, 104);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 24);
			this.label1.TabIndex = 3;
			this.label1.Text = "hours";
			// 
			// hours
			// 
			this.hours.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.hours.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.hours.Location = new System.Drawing.Point(16, 96);
			this.hours.Name = "hours";
			this.hours.Size = new System.Drawing.Size(56, 30);
			this.hours.TabIndex = 4;
			this.hours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// minutes
			// 
			this.minutes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.minutes.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.minutes.Location = new System.Drawing.Point(160, 96);
			this.minutes.Name = "minutes";
			this.minutes.Size = new System.Drawing.Size(56, 30);
			this.minutes.TabIndex = 6;
			this.minutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(224, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 24);
			this.label2.TabIndex = 5;
			this.label2.Text = "mins";
			// 
			// seconds
			// 
			this.seconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.seconds.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.seconds.Location = new System.Drawing.Point(288, 96);
			this.seconds.Name = "seconds";
			this.seconds.Size = new System.Drawing.Size(56, 30);
			this.seconds.TabIndex = 8;
			this.seconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(352, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 24);
			this.label3.TabIndex = 7;
			this.label3.Text = "secs";
			// 
			// startButton
			// 
			this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.startButton.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.startButton.Location = new System.Drawing.Point(328, 144);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(96, 32);
			this.startButton.TabIndex = 9;
			this.startButton.Text = "&Start";
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// inkEdit1
			// 
			this.inkEdit1.Location = new System.Drawing.Point(16, 48);
			this.inkEdit1.Name = "inkEdit1";
			this.inkEdit1.Size = new System.Drawing.Size(400, 36);
			this.inkEdit1.TabIndex = 0;
			this.inkEdit1.Text = "";
			// 
			// Form1
			// 
			this.AcceptButton = this.startButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(13, 29);
			this.ClientSize = new System.Drawing.Size(440, 182);
			this.Controls.Add(this.inkEdit1);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.seconds);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.minutes);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.hours);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nameLabel);
			this.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.Text = "New Timer";
			((System.ComponentModel.ISupportInitialize)(this.hours)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.minutes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.seconds)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new StartTimer());
		}

		private void startButton_Click(object sender, System.EventArgs e)
		{
			TimeSpan timerTime = new TimeSpan(0, Convert.ToInt32(hours.Value), Convert.ToInt32(minutes.Value), Convert.ToInt32(seconds.Value));

			string title = "Untitled";
			string txt = inkEdit1.Text;
			if (txt.Length > 0)
			{
				title = txt;
			}
			ActiveTimer form = new ActiveTimer(this, title, timerTime);
			form.Show();

			this.Hide();
		}

	}
}
