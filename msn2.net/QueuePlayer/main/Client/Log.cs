using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace msn2.net.QueuePlayer.Client
{
	/// <summary>
	/// Summary description for Log.
	/// </summary>
	public class Log : msn2.net.Controls.ShellForm
	{
		private System.Windows.Forms.CheckBox checkBoxShowLog;
		private System.Windows.Forms.TextBox textBoxLog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private UMPlayer player;

		public Log(UMPlayer player)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.player = player;
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

		public delegate void AddToLogDelegate(string area, string description);

		public void AddToLog(string area, string description)
		{
            textBoxLog.Text += DateTime.Now.ToShortTimeString() + ": " + area + ": " + description + Environment.NewLine;
			textBoxLog.SelectionStart = textBoxLog.Text.Length;
		}

		public void AddToLog(string area, string description, int mediaId)
		{
			textBoxLog.Text += DateTime.Now.ToShortTimeString() + ": " + area + ": " + description + "[" + mediaId.ToString() + "]" + Environment.NewLine;
			textBoxLog.SelectionStart = textBoxLog.Text.Length;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.checkBoxShowLog = new System.Windows.Forms.CheckBox();
			this.textBoxLog = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// checkBoxShowLog
			// 
			this.checkBoxShowLog.Location = new System.Drawing.Point(8, 0);
			this.checkBoxShowLog.Name = "checkBoxShowLog";
			this.checkBoxShowLog.Size = new System.Drawing.Size(656, 24);
			this.checkBoxShowLog.TabIndex = 5;
			this.checkBoxShowLog.Text = "&Show Activity";
			this.checkBoxShowLog.CheckedChanged += new System.EventHandler(this.checkBoxShowLog_CheckedChanged);
			// 
			// textBoxLog
			// 
			this.textBoxLog.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxLog.BackColor = System.Drawing.SystemColors.Window;
			this.textBoxLog.ForeColor = System.Drawing.SystemColors.ControlText;
			this.textBoxLog.Location = new System.Drawing.Point(8, 24);
			this.textBoxLog.Multiline = true;
			this.textBoxLog.Name = "textBoxLog";
			this.textBoxLog.ReadOnly = true;
			this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxLog.Size = new System.Drawing.Size(360, 144);
			this.textBoxLog.TabIndex = 4;
			this.textBoxLog.Text = "";
			// 
			// Log
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(376, 174);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.checkBoxShowLog,
																		  this.textBoxLog});
			this.Name = "Log";
			this.Text = "Log";
			this.ResumeLayout(false);

		}
		#endregion

		private void checkBoxShowLog_CheckedChanged(object sender, System.EventArgs e)
		{
            player.client.Logging = checkBoxShowLog.Checked;			
		}
	}
}
