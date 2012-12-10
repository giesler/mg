using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace msn2.net.Controls
{
	public class WebBrowser : msn2.net.Controls.ShellForm
	{
		private msn2.net.Controls.WebBrowserControl webBrowserControl1;
		private System.ComponentModel.IContainer components = null;

		public WebBrowser(string title, string url)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.Text = title;
			this.webBrowserControl1.Navigate(url);

			this.Width = 600;
			this.Height = 800;
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
			this.webBrowserControl1 = new msn2.net.Controls.WebBrowserControl();
			this.SuspendLayout();
			// 
			// webBrowserControl1
			// 
			this.webBrowserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowserControl1.Name = "webBrowserControl1";
			this.webBrowserControl1.Size = new System.Drawing.Size(440, 264);
			this.webBrowserControl1.TabIndex = 5;
			// 
			// WebBrowser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 264);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.webBrowserControl1});
			this.Name = "WebBrowser";
			this.ResumeLayout(false);

		}
		#endregion
	}
}

