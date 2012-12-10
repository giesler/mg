using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for ComputerInfo.
	/// </summary>
	public class ComputerInfo : msn2.net.Controls.ShellForm
	{
		private Graph graph1;
		private System.Diagnostics.PerformanceCounter procPerf;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;
		private LineGraph procLine;

		public ComputerInfo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			procPerf.MachineName	= System.Environment.MachineName;
			this.Text				= System.Environment.MachineName;

            procLine = new LineGraph(100, 100, Color.Black);
            graph1.AddGraphItem(procLine);

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
			this.graph1 = new msn2.net.Controls.Graph();
			this.procPerf = new System.Diagnostics.PerformanceCounter();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.procPerf)).BeginInit();
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
			// graph1
			// 
			this.graph1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graph1.Name = "graph1";
			this.graph1.Size = new System.Drawing.Size(224, 62);
			this.graph1.TabIndex = 0;
			// 
			// procPerf
			// 
			this.procPerf.CategoryName = "Processor";
			this.procPerf.CounterName = "% Processor Time";
			this.procPerf.InstanceName = "_Total";
			this.procPerf.MachineName = "chef";
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// ComputerInfo
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(224, 62);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.graph1});
			this.Name = "ComputerInfo";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "ComputerInfo";
			this.TitleVisible = true;
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.procPerf)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			procLine.AddToHistory(Convert.ToInt32(procPerf.NextValue()));
		}
	}
}
