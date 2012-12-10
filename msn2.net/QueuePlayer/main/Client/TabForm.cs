using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace msn2.net.QueuePlayer.Client
{
	public class TabForm : msn2.net.Controls.ShellForm
	{
		public System.Windows.Forms.Panel panelTabs;
		private System.ComponentModel.IContainer components = null;

		public TabForm()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
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

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelTabs = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panelTabs
			// 
			this.panelTabs.BackColor = System.Drawing.Color.Transparent;
			this.panelTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTabs.Name = "panelTabs";
			this.panelTabs.Size = new System.Drawing.Size(264, 168);
			this.panelTabs.TabIndex = 5;
			// 
			// TabForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(264, 168);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelTabs});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "TabForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "QueuePlayer Media";
			this.ResumeLayout(false);

		}
		#endregion
	}
}

