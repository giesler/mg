using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class WebBrowserControl : System.Windows.Forms.UserControl
	{
		private AxSHDocVw.AxWebBrowser axWebBrowser1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WebBrowserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		public void Navigate(string url)
		{
			object obj1 = 0; object obj2 = ""; object obj3 = ""; object obj4 = "";
			axWebBrowser1.Navigate(url, ref obj1, ref obj2, ref obj3, ref obj4);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WebBrowserControl));
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.SuspendLayout();
			// 
			// axWebBrowser1
			// 
			this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
			this.axWebBrowser1.Size = new System.Drawing.Size(304, 288);
			this.axWebBrowser1.TabIndex = 0;
			// 
			// WebBrowserControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.axWebBrowser1});
			this.Name = "WebBrowserControl";
			this.Size = new System.Drawing.Size(304, 288);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
