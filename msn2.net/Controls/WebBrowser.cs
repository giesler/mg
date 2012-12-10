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
			// webBrowserControl1
			// 
			this.webBrowserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowserControl1.Name = "webBrowserControl1";
			this.webBrowserControl1.Size = new System.Drawing.Size(440, 264);
			this.webBrowserControl1.TabIndex = 7;
			this.webBrowserControl1.TitleChange += new msn2.net.Controls.TitleChangeDelegate(this.webBrowserControl1_TitleChange);
			// 
			// WebBrowser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 264);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.webBrowserControl1});
			this.Name = "WebBrowser";
			this.Text = "Web Browser";
			this.TitleVisible = true;
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void webBrowserControl1_TitleChange(object sender, msn2.net.Controls.TitleChangeEventArgs e)
		{
			this.Text = e.Title;
		}
	}
}

